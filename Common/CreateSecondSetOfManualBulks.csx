#r ControllerMES.Infrastructure.Common
#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   CreateSecondSetOfManualBulks
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         T.Stürzer
//   Date:           2025-01-02
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   T.Stürzer       2025-01-02    Created
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IServerCustomization))]
[Export("CreateSecondSetOfManualBulks", typeof(HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("[91] Create second set of manual bulks")]
[EnabledScript(true)]
public class CreateSecondSetOfManualBulks : GenericTaskBase, HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask, IPartImportsSatisfiedNotification
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;
    
    [Import]
    private IDistributedServiceProvider _DistributedServiceProvider;
    
    private ICommonServiceDistributed _BulkInfoProvider;

    private Logger _Logger;
    
    private string _TaskName = "CreateSecondSetOfManualBulks";
    

    public override void Execute(IJobExecutionContext executionContext)
    {
        Guard.ThrowOnArgumentNull(executionContext, "executionContext");

        try
        {
            _Logger = LogHelper.GetLogger(executionContext.Area, typeof(CreateSecondSetOfManualBulks));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);

            using(var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
            {
                // Repositorys
                var prodOrdersRep = unitOfWork.GetRepository<ProductionOrder>();
                
                // CustomerOrders sorted by bulk
                // Dark-Yellow-Bulk
                var darkYellowBulkOrderCodes = new[]{"Demo_Kitchen_Small_017","Demo_Kitchen_Small_018","Demo_Kitchen_Small_019","Demo_Kitchen_Medium_019"};
                
                // Dark-Red-Bulk                                
                var darkRedBulkOrderCodes = new[]{"Demo_Kitchen_Small_007","Demo_Kitchen_Small_008","Demo_Kitchen_Medium_007"};
                
                // Dark-Green-Bulk
                var darkGreenBulkOrderCodes = new[]{"Demo_Kitchen_Small_012","Demo_Kitchen_Small_015","Demo_Kitchen_Medium_011","Demo_Kitchen_Medium_015"};
                                                
                // Dark-Black-Bulk
                var darkBlackBulkOrderCodes = new[]{"Demo_Kitchen_Small_022","Demo_Kitchen_Small_024","Demo_Kitchen_Medium_022"};
                
                // Set new bulks + start and end date         
                // Dark-Yellow-Bulk                            
                var prodOrderDarkYellowBulk = prodOrdersRep.GetQueryable(false).Where(po => darkYellowBulkOrderCodes.Contains(po.CustomerOrderCode)).ToList();
            
                if(prodOrderDarkYellowBulk != null)
                {
                    //Get date
                    var darkYellowDate = prodOrdersRep.GetFirstOrDefault(
                            po => po.ComponentType == ComponentType.SidePanel && 
                                  darkYellowBulkOrderCodes.Contains(po.CustomerOrderCode)
                                  );
                    
                    var darkYellowBulkStartDate = Convert.ToDateTime(darkYellowDate.DesiredStartDate);
                    var darkYellowBulkEndDate = Convert.ToDateTime(darkYellowDate.DesiredEndDate);
                    
                    SetManualBulk(unitOfWork, "Dark-Yellow-Bulk", darkYellowBulkStartDate, "#FFFFEA25", darkYellowBulkEndDate, prodOrderDarkYellowBulk);
                }
                else
                {
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Error while generating the Dark-Yellow-Bulk!", _TaskName)));
                }
                
                // Dark-Red-Bulk
                var prodOrderDarkRedBulk = prodOrdersRep.GetQueryable(false).Where(po => darkRedBulkOrderCodes.Contains(po.CustomerOrderCode)).ToList();
            
                if(prodOrderDarkRedBulk != null)
                {
                    //Get date
                    var darkRedDate = prodOrdersRep.GetFirstOrDefault(
                            po => po.ComponentType == ComponentType.SidePanel && 
                                  darkRedBulkOrderCodes.Contains(po.CustomerOrderCode)
                                  );
                    
                    var darkRedBulkStartDate = Convert.ToDateTime(darkRedDate.DesiredStartDate);
                    var darkRedBulkEndDate = Convert.ToDateTime(darkRedDate.DesiredEndDate);
                    
                    SetManualBulk(unitOfWork, "Dark-Red-Bulk", darkRedBulkStartDate, "#FFC84B4B", darkRedBulkEndDate, prodOrderDarkRedBulk);
                }
                else
                {
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Error while generating the Dark-Red-Bulk!", _TaskName)));
                }
            
            
            }
        }
        catch (Exception e)
        {
            executionContext.JobResultSetComplete(JobResult.Failed);
            _Logger.Error(ResourcesKeys.ErrorInUserExit("CreateSecondSetOfManualBulks"), null, e);
            throw;
        }
    }
    
    
    public void SetManualBulk(IUnitOfWork unitOfWork, string planningNumber, DateTime startDate, string color, DateTime endDate, List<ProductionOrder> prodOrders)
    {
        var manualBulk = new ManualBulk();               
        
        manualBulk.PlanningNumber = planningNumber;
        manualBulk.PlanningState = PlanningState.Planned;
        manualBulk.StartDate = startDate;
        manualBulk.Color = color;
        manualBulk.CreationSource = "BulkPlanning";
        manualBulk.EndDate = endDate;
        manualBulk.SchedulingMode = SchedulingMode.Backward;
        manualBulk.ProductionOrders = prodOrders;
            
        unitOfWork.AddOrUpdate(new[] {manualBulk});
        unitOfWork.Save();
        
        _BulkInfoProvider.PlanProductionDaysForBulkWithoutDelete(manualBulk.PlanningNumber, manualBulk.StartDate, manualBulk.EndDate);
        
        //_Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Succesfully added Bulk-Blue!", _TaskName)));
    }


    public override ICollection<UserExitParameter> UserExitInputParameters
    {
        get
        {
            return new List<UserExitParameter>
            {
                // Example for new parameter:
                // new UserExitParameter("MyParameter", typeof(string), true)
            };
        }
    }
    
    public void OnImportsSatisfied()
    {
        _BulkInfoProvider = _DistributedServiceProvider.GetService<ICommonServiceDistributed>();
    }
}
