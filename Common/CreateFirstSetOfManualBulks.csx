#r ControllerMES.Infrastructure.Common
#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   CreateFirstSetOfManualBulks
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         T.Stürzer
//   Date:           2025-01-01
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   T.Stürzer       2025-01-01    Created
//-----------------------------------------------------------------------------


using System.ComponentModel;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IServerCustomization))]
[Export("CreateFirstSetOfManualBulks", typeof(HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("[07] Create first set of manual bulks")]
[EnabledScript(true)]
public class CreateFirstSetOfManualBulks : GenericTaskBase, HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask, IPartImportsSatisfiedNotification
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;
    
    [Import]
    private IDistributedServiceProvider _DistributedServiceProvider;
    
    private ICommonServiceDistributed _BulkInfoProvider;

    private Logger _Logger;
    
    private string _TaskName = "CreateFirstSetOfManualBulks";
    

    public override void Execute(IJobExecutionContext executionContext)
    {
        Guard.ThrowOnArgumentNull(executionContext, "executionContext");

        try
        {
            _Logger = LogHelper.GetLogger(executionContext.Area, typeof(CreateFirstSetOfManualBulks));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);
            
            using(var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
            {
                // Repositorys
                var prodOrdersRep = unitOfWork.GetRepository<ProductionOrder>();
                
                // CustomerOrders sorted by bulk
                // Blue-Bulk
                var blueBulkOrderCodes = new[]{"Demo_Kitchen_Small_003","Demo_Kitchen_Small_004","Demo_Kitchen_Small_005","Demo_Kitchen_Medium_001"};
                
                // Red-Bulk                                
                var redBulkOrderCodes = new[]{"Demo_Kitchen_Small_006","Demo_Kitchen_Medium_006","Demo_Kitchen_Medium_008","Demo_Kitchen_Medium_009"};
                
                // Green-Bulk
                var greenBulkOrderCodes = new[]{"Demo_Kitchen_Small_011","Demo_Kitchen_Small_014","Demo_Kitchen_Medium_012","Demo_Kitchen_Medium_014"};
                
                // Yellow-Bulk
                var yellowBulkOrderCodes = new[]{"Demo_Kitchen_Medium_016","Demo_Kitchen_Medium_018"};
                                                
                // Black-Bulk
                var blackBulkOrderCodes = new[]{"Demo_Kitchen_Small_023","Demo_Kitchen_Small_025","Demo_Kitchen_Medium_021","Demo_Kitchen_Medium_024"};
                                                
                // Dark-Blue-Bulk
                var darkblueBulkOrderCodes = new[]{"Demo_Kitchen_Small_002","Demo_Kitchen_Medium_004","Demo_Kitchen_Medium_005"};
                
                // Set new bulks + start and end date         
                // Blue bulk                              
                var prodOrderBlueBulk = prodOrdersRep.GetQueryable(false).Where(po => blueBulkOrderCodes.Contains(po.CustomerOrderCode)).ToList();
                
                if(prodOrderBlueBulk != null)
                {               
                    //Get date
                    var blueBulkDate = prodOrdersRep.GetFirstOrDefault(
                            po => po.ComponentType == ComponentType.SidePanel && 
                                  blueBulkOrderCodes.Contains(po.CustomerOrderCode)
                                  );
                    
                    var blueBulkStartDate = Convert.ToDateTime(blueBulkDate.DesiredStartDate);
                    var blueBulkEndDate = Convert.ToDateTime(blueBulkDate.DesiredEndDate);
                    
                    SetManualBulk(unitOfWork, "Blue-Bulk", blueBulkStartDate, "#FF6464FA", blueBulkEndDate, prodOrderBlueBulk);
                }
                
                else
                {
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Error while generating Blue-Bulk!", _TaskName)));
                }
                

                // Red bulk
                var prodOrderRedBulk = prodOrdersRep.GetQueryable(false).Where(po => redBulkOrderCodes.Contains(po.CustomerOrderCode)).ToList();
                
                if(prodOrderRedBulk != null)
                {
                    //Get date
                    var redBulkDate = prodOrdersRep.GetFirstOrDefault(
                            po => po.ComponentType == ComponentType.SidePanel &&
                                  redBulkOrderCodes.Contains(po.CustomerOrderCode)
                                  );
                    
                    var redBulkStartDate = Convert.ToDateTime(redBulkDate.DesiredStartDate);
                    var redBulkEndDate = Convert.ToDateTime(redBulkDate.DesiredEndDate);
                    
                    SetManualBulk(unitOfWork, "Red-Bulk", redBulkStartDate, "#FFFA6464", redBulkEndDate, prodOrderRedBulk);
                }
                
                else
                {
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Error while generating Red-Bulk!", _TaskName)));
                }                
                
                
                // Green bulk
                var prodOrderGreenBulk = prodOrdersRep.GetQueryable(false).Where(po => greenBulkOrderCodes.Contains(po.CustomerOrderCode)).ToList();
                
                if(prodOrderGreenBulk != null)
                {
                    //Get date
                    var greenBulkDate = prodOrdersRep.GetFirstOrDefault(
                            po => po.ComponentType == ComponentType.SidePanel &&
                                  greenBulkOrderCodes.Contains(po.CustomerOrderCode)
                                  );
                    
                    var greenBulkStartDate = Convert.ToDateTime(greenBulkDate.DesiredStartDate);
                    var greenBulkEndDate = Convert.ToDateTime(greenBulkDate.DesiredEndDate);
                    
                    SetManualBulk(unitOfWork, "Green-Bulk", greenBulkStartDate, "#FF64FA64", greenBulkEndDate, prodOrderGreenBulk);
                }
                
                else
                {
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Error while generating Green-Bulk!", _TaskName)));
                }
                
                        
                // Yellow bulk
                var prodOrderYellowBulk = prodOrdersRep.GetQueryable(false).Where(po => yellowBulkOrderCodes.Contains(po.CustomerOrderCode)).ToList();
                
                if(prodOrderYellowBulk != null)
                {               
                    //Get date
                    var yellowBulkDate = prodOrdersRep.GetFirstOrDefault(
                            po => po.ComponentType == ComponentType.SidePanel &&
                                  yellowBulkOrderCodes.Contains(po.CustomerOrderCode)
                                  );
                    
                    var yellowBulkStartDate = Convert.ToDateTime(yellowBulkDate.DesiredStartDate);
                    var yellowBulkEndDate = Convert.ToDateTime(yellowBulkDate.DesiredEndDate);
                     
                    SetManualBulk(unitOfWork, "Yellow-Bulk", yellowBulkStartDate, "#FFFFF064", yellowBulkEndDate, prodOrderYellowBulk);
                }
                
                else
                {
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Error while generating Yellow-Bulk!", _TaskName)));
                }
                
                
                // Black bulk
                var prodOrderBlackBulk = prodOrdersRep.GetQueryable(false).Where(po => blackBulkOrderCodes.Contains(po.CustomerOrderCode)).ToList();

                
                if(prodOrderBlackBulk != null)
                {               
                    //Get date
                    var blackBulkDate = prodOrdersRep.GetFirstOrDefault(
                            po => po.ComponentType == ComponentType.SidePanel && 
                                  blackBulkOrderCodes.Contains(po.CustomerOrderCode)
                                  );
                    
                    var blackBulkStartDate = Convert.ToDateTime(blackBulkDate.DesiredStartDate);
                    var blackBulkEndDate = Convert.ToDateTime(blackBulkDate.DesiredEndDate);
                    
                    SetManualBulk(unitOfWork, "Black-Bulk", blackBulkStartDate, "#FF969696", blackBulkEndDate, prodOrderBlackBulk);
                }
                
                else
                {
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Error while generating Black-Bulk!", _TaskName)));
                }
                
                // Dark-Blue bulk
                var prodOrderDarkBlueBulk = prodOrdersRep.GetQueryable(false).Where(po => darkblueBulkOrderCodes.Contains(po.CustomerOrderCode)).ToList();
                
                if(prodOrderDarkBlueBulk != null)
                {               
                    //Get date
                    var darkBlueBulkDate = prodOrdersRep.GetFirstOrDefault(
                            po => po.ComponentType == ComponentType.SidePanel && 
                                  darkblueBulkOrderCodes.Contains(po.CustomerOrderCode)
                                  );
                    
                    var darkBlueBulkStartDate = Convert.ToDateTime(darkBlueBulkDate.DesiredStartDate);
                    var darkBlueBulkEndDate = Convert.ToDateTime(darkBlueBulkDate.DesiredEndDate);
                    
                    SetManualBulk(unitOfWork, "Dark-Blue-Bulk", darkBlueBulkStartDate, "#FF1919C8", darkBlueBulkEndDate, prodOrderDarkBlueBulk);
                }
                else
                {
                _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Error while generating Dark-Blue-Bulk!", _TaskName)));
                }
                
                unitOfWork.Save();
            }
        }
        catch (Exception e)
        {
            executionContext.JobResultSetComplete(JobResult.Failed);
            _Logger.Error(ResourcesKeys.ErrorInUserExit("CreateFirstSetOfManualBulks"), null, e);
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
        
        _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Succesfully added {[1]}!", _TaskName, planningNumber)));
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
