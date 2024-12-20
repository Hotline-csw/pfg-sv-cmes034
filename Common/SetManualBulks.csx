#r ControllerMES.Infrastructure.Common
#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   SetManualBulks
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         T.Stürzer
//   Date:           2023-02-12
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   T.Stürzer       2023-02-12    Created
//   T.Stürzer       2023-02-22    Added yellow bulk (Demo_Kitchen_Medium_016 & Demo_Kitchen_Medium_018)
//   T.Stürzer       2023-03-15    Added black and dark-blue bulk (Demo_Kitchen_Small_002, Demo_Kitchen_Small_023, Demo_Kitchen_Small_025, 
//                                                                 Demo_Kitchen_Medium_004, Demo_Kitchen_Medium_005, Demo_Kitchen_Medium_021 & Demo_Kitchen_Medium_024)
//   T.Stürzer       2023-03-23    Logging and error messages
//-----------------------------------------------------------------------------


using System.ComponentModel;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IServerCustomization))]
[Export("SetManualBulks", typeof(HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("[07] Set manual bulks")]
[EnabledScript(true)]
public class SetManualBulks : GenericTaskBase, HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask, IPartImportsSatisfiedNotification
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;
    
    [Import]
    private IDistributedServiceProvider _DistributedServiceProvider;
    
    private ICommonServiceDistributed _BulkInfoProvider;

    private Logger _Logger;
    
    private string _TaskName = "SetManualBulks";
    

    public override void Execute(IJobExecutionContext executionContext)
    {
        Guard.ThrowOnArgumentNull(executionContext, "executionContext");

        try
        {
            _Logger = LogHelper.GetLogger(executionContext.Area, typeof(SetManualBulks));
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
                var darkblueBulkOrderCodes = new[]{"Demo_Kitchen_Small_023","Demo_Kitchen_Small_025","Demo_Kitchen_Medium_021","Demo_Kitchen_Medium_024"};
                
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
                                                   
                    //Bulk
                    var blueBulk = new ManualBulk();               
                    
                    blueBulk.PlanningNumber = "Bulk-Blue";
                    blueBulk.PlanningState = PlanningState.Planned;
                    blueBulk.StartDate = blueBulkStartDate;
                    blueBulk.Color = "#FF6464FA";
                    blueBulk.CreationDate = DateTime.Now;
                    blueBulk.CreationSource = "BulkPlanning";
                    blueBulk.ModificationDate = DateTime.Now;
                    blueBulk.ModificationSource = "BulkPlanning";
                    blueBulk.EndDate = blueBulkEndDate;
                    blueBulk.SchedulingMode = SchedulingMode.Backward;
                    blueBulk.ProductionOrders = prodOrderBlueBulk;
                        
                    unitOfWork.AddOrUpdate(new[] {blueBulk});
                    unitOfWork.Save();
                    
                    _BulkInfoProvider.PlanProductionDaysForBulkWithoutDelete(blueBulk.PlanningNumber, blueBulk.StartDate, blueBulk.EndDate);
                    
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Succesfully added Bulk-Blue!", _TaskName)));
                }
                
                else
                {
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Error while generating Bulk-Blue!", _TaskName)));
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
    
                   
                    //Bulk
                    var redBulk = new ManualBulk();
                    
                    redBulk.PlanningNumber = "Bulk-Red";
                    redBulk.PlanningState = PlanningState.Planned;
                    redBulk.StartDate = redBulkStartDate;
                    redBulk.Color = "#FFFA6464";
                    redBulk.CreationDate = DateTime.Now;
                    redBulk.CreationSource = "BulkPlanning";
                    redBulk.ModificationDate = DateTime.Now;
                    redBulk.ModificationSource = "BulkPlanning";
                    redBulk.EndDate = redBulkEndDate;
                    redBulk.SchedulingMode = SchedulingMode.Backward;
                    redBulk.ProductionOrders = prodOrderRedBulk;
                    
                    unitOfWork.AddOrUpdate(new[] {redBulk});
                    unitOfWork.Save();
                    
                    _BulkInfoProvider.PlanProductionDaysForBulkWithoutDelete(redBulk.PlanningNumber, redBulk.StartDate, redBulk.EndDate);               
                    
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Succesfully added Bulk-Red!", _TaskName)));
                }
                
                else
                {
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Error while generating Bulk-Red!", _TaskName)));
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
    
    
                    //Bulk
                    var greenBulk = new ManualBulk();
                    
                    greenBulk.PlanningNumber = "Bulk-Green";
                    greenBulk.PlanningState = PlanningState.Planned;
                    greenBulk.StartDate = greenBulkStartDate;
                    greenBulk.Color = "#FF64FA64";
                    greenBulk.CreationDate = DateTime.Now;
                    greenBulk.CreationSource = "BulkPlanning";
                    greenBulk.ModificationDate = DateTime.Now;
                    greenBulk.ModificationSource = "BulkPlanning";
                    greenBulk.EndDate = greenBulkEndDate;
                    greenBulk.SchedulingMode = SchedulingMode.Backward;
                    greenBulk.ProductionOrders = prodOrderGreenBulk;
                
                    unitOfWork.AddOrUpdate(new[] {greenBulk});
                    unitOfWork.Save();
                    
                    _BulkInfoProvider.PlanProductionDaysForBulkWithoutDelete(greenBulk.PlanningNumber, greenBulk.StartDate, greenBulk.EndDate);
                    
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Succesfully added Bulk-Green!", _TaskName)));
                }
                
                else
                {
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Error while generating Bulk-Green!", _TaskName)));
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
                                                   
                    //Bulk
                    var yellowBulk = new ManualBulk();               
                    
                    yellowBulk.PlanningNumber = "Bulk-Yellow";
                    yellowBulk.PlanningState = PlanningState.Planned;
                    yellowBulk.StartDate = yellowBulkStartDate;
                    yellowBulk.Color = "#FFFFF064";
                    yellowBulk.CreationDate = DateTime.Now;
                    yellowBulk.CreationSource = "BulkPlanning";
                    yellowBulk.ModificationDate = DateTime.Now;
                    yellowBulk.ModificationSource = "BulkPlanning";
                    yellowBulk.EndDate = yellowBulkEndDate;
                    yellowBulk.SchedulingMode = SchedulingMode.Backward;
                    yellowBulk.ProductionOrders = prodOrderYellowBulk;
                        
                    unitOfWork.AddOrUpdate(new[] {yellowBulk});
                    unitOfWork.Save();
                    
                    _BulkInfoProvider.PlanProductionDaysForBulkWithoutDelete(yellowBulk.PlanningNumber, yellowBulk.StartDate, yellowBulk.EndDate);
                    
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Succesfully added Bulk-Yellow!", _TaskName)));
                }
                
                else
                {
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Error while generating Bulk-Yellow!", _TaskName)));
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
                                                   
                    //Bulk
                    var blackBulk = new ManualBulk();               
                    
                    blackBulk.PlanningNumber = "Bulk-Black";
                    blackBulk.PlanningState = PlanningState.Planned;
                    blackBulk.StartDate = blackBulkStartDate;
                    blackBulk.Color = "#FF969696";
                    blackBulk.CreationDate = DateTime.Now;
                    blackBulk.CreationSource = "BulkPlanning";
                    blackBulk.ModificationDate = DateTime.Now;
                    blackBulk.ModificationSource = "BulkPlanning";
                    blackBulk.EndDate = blackBulkEndDate;
                    blackBulk.SchedulingMode = SchedulingMode.Backward;
                    blackBulk.ProductionOrders = prodOrderBlackBulk;
                        
                    unitOfWork.AddOrUpdate(new[] {blackBulk});
                    unitOfWork.Save();
                    
                    _BulkInfoProvider.PlanProductionDaysForBulkWithoutDelete(blackBulk.PlanningNumber, blackBulk.StartDate, blackBulk.EndDate);
                    
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Succesfully added Bulk-Black!", _TaskName)));
                }
                
                else
                {
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Error while generating Bulk-Black!", _TaskName)));
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
                                                   
                    //Bulk
                    var darkBlueBulk = new ManualBulk();               
                    
                    darkBlueBulk.PlanningNumber = "Bulk-Dark-Blue";
                    darkBlueBulk.PlanningState = PlanningState.Planned;
                    darkBlueBulk.StartDate = darkBlueBulkStartDate;
                    darkBlueBulk.Color = "#FF1919C8";
                    darkBlueBulk.CreationDate = DateTime.Now;
                    darkBlueBulk.CreationSource = "BulkPlanning";
                    darkBlueBulk.ModificationDate = DateTime.Now;
                    darkBlueBulk.ModificationSource = "BulkPlanning";
                    darkBlueBulk.EndDate = darkBlueBulkEndDate;
                    darkBlueBulk.SchedulingMode = SchedulingMode.Backward;
                    darkBlueBulk.ProductionOrders = prodOrderDarkBlueBulk;
                        
                    unitOfWork.AddOrUpdate(new[] {darkBlueBulk});
                    unitOfWork.Save();
                    
                    _BulkInfoProvider.PlanProductionDaysForBulkWithoutDelete(darkBlueBulk.PlanningNumber, darkBlueBulk.StartDate, darkBlueBulk.EndDate);
                    
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Succesfully added Bulk-Dark-Blue!", _TaskName)));
                }
                
                else
                {
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Error while generating Bulk-Dark-Blue!", _TaskName)));
                }

                unitOfWork.Save();
            }
        }
        catch (Exception e)
        {
            executionContext.JobResultSetComplete(JobResult.Failed);
            _Logger.Error(ResourcesKeys.ErrorInUserExit("SetManualBulks"), null, e);
            throw;
        }
    }
    
    public void SetManualBulk(IUnitOfWork unitOfWork, string planningNumber, DateTime startDate, string color, DateTime endDate, List<ProductionOrder> prodOrders)
    {
        
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
