#r ControllerMES.Infrastructure.Common
#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   CreateThirdSetOfManualBulks
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
[Export("CreateThirdSetOfManualBulks", typeof(HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("[93] Create third set of manual bulks")]
[EnabledScript(true)]
public class CreateThirdSetOfManualBulks : GenericTaskBase, HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask, IPartImportsSatisfiedNotification
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;
    
    [Import]
    private IDistributedServiceProvider _DistributedServiceProvider;
    
    private ICommonServiceDistributed _BulkInfoProvider;

    private Logger _Logger;
    
    private string _TaskName = "CreateThirdSetOfManualBulks";
    

    public override void Execute(IJobExecutionContext executionContext)
    {
        Guard.ThrowOnArgumentNull(executionContext, "executionContext");

        try
        {
            _Logger = LogHelper.GetLogger(executionContext.Area, typeof(CreateThirdSetOfManualBulks));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);

            using(var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
            {
/*
                // Repositorys
                var prodOrdersRep = unitOfWork.GetRepository<ProductionOrder>();
                
                // CustomerOrders sorted by bulk
                // Bright-Yellow-Bulk
                var brightYellowBulkOrderCodes = new[]{"Demo_Kitchen_Small_016","Demo_Kitchen_Small_020","Demo_Kitchen_Medium_017","Demo_Kitchen_Medium_020"};
                
                // Bright-Red-Bulk                                
                var brightRedBulkOrderCodes = new[]{"Demo_Kitchen_Small_009","Demo_Kitchen_Small_010","Demo_Kitchen_Medium_010"};
                
                // Bright-Blue-Bulk                                
                var brightBlueBulkOrderCodes = new[]{"Demo_Kitchen_Small_001","Demo_Kitchen_Medium_002","Demo_Kitchen_Medium_003"};
                
                // Bright-Black-Bulk
                var brightBlackBulkOrderCodes = new[]{"Demo_Kitchen_Small_021","Demo_Kitchen_Medium_023","Demo_Kitchen_Medium_025"};
                
                // Bright-Green-Bulk
                var brightGreenBulkOrderCodes = new[]{"Demo_Kitchen_Small_013","Demo_Kitchen_Medium_013"};
                
                // Set new bulks + start and end date         
                // Bright-Yellow-Bulk                            
                var prodOrderBrightYellowBulk = prodOrdersRep.GetQueryable(false).Where(po => brightYellowBulkOrderCodes.Contains(po.CustomerOrderCode)).ToList();
            
                if(prodOrderBrightYellowBulk != null)
                {
                    //Get date
                    var brightYellowDate = prodOrdersRep.GetFirstOrDefault(
                            po => po.ComponentType == ComponentType.SidePanel && 
                                  brightYellowBulkOrderCodes.Contains(po.CustomerOrderCode)
                                  );
                    
                    var brightYellowBulkStartDate = Convert.ToDateTime(brightYellowDate.DesiredStartDate);
                    var brightYellowBulkEndDate = Convert.ToDateTime(brightYellowDate.DesiredEndDate);
                    
                    SetManualBulk(unitOfWork, "Bright-Yellow-Bulk", brightYellowBulkStartDate, "#FFF4FF7E", brightYellowBulkEndDate, prodOrderBrightYellowBulk);
                }
                else
                {
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Error while generating the Bright-Yellow-Bulk!", _TaskName)));
                }
                
                // Bright-Red-Bulk
                var prodOrderBrightRedBulk = prodOrdersRep.GetQueryable(false).Where(po => brightRedBulkOrderCodes.Contains(po.CustomerOrderCode)).ToList();
            
                if(prodOrderBrightRedBulk != null)
                {
                    //Get date
                    var brightRedDate = prodOrdersRep.GetFirstOrDefault(
                            po => po.ComponentType == ComponentType.SidePanel && 
                                  brightRedBulkOrderCodes.Contains(po.CustomerOrderCode)
                                  );
                    
                    var brightRedBulkStartDate = Convert.ToDateTime(brightRedDate.DesiredStartDate);
                    var brightRedBulkEndDate = Convert.ToDateTime(brightRedDate.DesiredEndDate);
                    
                    SetManualBulk(unitOfWork, "Bright-Red-Bulk", brightRedBulkStartDate, "#FFFAC8C8", brightRedBulkEndDate, prodOrderBrightRedBulk);
                }
                else
                {
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Error while generating the Bright-Red-Bulk!", _TaskName)));
                }
                
                // Bright-Blue-Bulk
                var prodOrderBrightBlueBulk = prodOrdersRep.GetQueryable(false).Where(po => brightBlueBulkOrderCodes.Contains(po.CustomerOrderCode)).ToList();
            
                if(prodOrderBrightBlueBulk != null)
                {
                    //Get date
                    var brightBlueDate = prodOrdersRep.GetFirstOrDefault(
                            po => po.ComponentType == ComponentType.SidePanel && 
                                  brightBlueBulkOrderCodes.Contains(po.CustomerOrderCode)
                                  );
                    
                    var brightBlueBulkStartDate = Convert.ToDateTime(brightBlueDate.DesiredStartDate);
                    var brightBlueBulkEndDate = Convert.ToDateTime(brightBlueDate.DesiredEndDate);
                    
                    SetManualBulk(unitOfWork, "Bright-Blue-Bulk", brightBlueBulkStartDate, "#FFC8C8FA", brightBlueBulkEndDate, prodOrderBrighBlueBulk);
                }
                else
                {
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Error while generating the Bright-Blue-Bulk!", _TaskName)));
                }
                
                // Dark-Green-Bulk
                var prodOrderDarkGreenBulk = prodOrdersRep.GetQueryable(false).Where(po => darkGreenBulkOrderCodes.Contains(po.CustomerOrderCode)).ToList();
            
                if(prodOrderDarkGreenBulk != null)
                {
                    //Get date
                    var darkGreenDate = prodOrdersRep.GetFirstOrDefault(
                            po => po.ComponentType == ComponentType.SidePanel && 
                                  darkGreenBulkOrderCodes.Contains(po.CustomerOrderCode)
                                  );
                    
                    var darkGreenBulkStartDate = Convert.ToDateTime(darkGreenDate.DesiredStartDate);
                    var darkGreenBulkEndDate = Convert.ToDateTime(darkGreenDate.DesiredEndDate);
                    
                    SetManualBulk(unitOfWork, "Dark-Green-Bulk", darkGreenBulkStartDate, "#FF4BC84B", darkGreenBulkEndDate, prodOrderDarkGreenBulk);
                }
                else
                {
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Error while generating the Dark-Green-Bulk!", _TaskName)));
                }
                
                // Dark-Black-Bulk
                var prodOrderDarkBlackBulk = prodOrdersRep.GetQueryable(false).Where(po => darkBlackBulkOrderCodes.Contains(po.CustomerOrderCode)).ToList();
            
                if(prodOrderDarkBlackBulk != null)
                {
                    //Get date
                    var darkBlackDate = prodOrdersRep.GetFirstOrDefault(
                            po => po.ComponentType == ComponentType.SidePanel && 
                                  darkBlackBulkOrderCodes.Contains(po.CustomerOrderCode)
                                  );
                    
                    var darkBlackBulkStartDate = Convert.ToDateTime(darkBlackDate.DesiredStartDate);
                    var darkBlackBulkEndDate = Convert.ToDateTime(darkBlackDate.DesiredEndDate);
                    
                    SetManualBulk(unitOfWork, "Dark-Black-Bulk", darkBlackBulkStartDate, "#FF323232", darkBlackBulkEndDate, prodOrderDarkBlackBulk);
                }
                else
                {
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Error while generating the Dark-Black-Bulk!", _TaskName)));
                }
                
                unitOfWork.Save();*/
            }
        }
        catch (Exception e)
        {
            executionContext.JobResultSetComplete(JobResult.Failed);
            _Logger.Error(ResourcesKeys.ErrorInUserExit("CreateThirdSetOfManualBulks"), null, e);
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
