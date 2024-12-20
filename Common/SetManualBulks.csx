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

    // Small Kitchens
    private string dks002 = "Demo_Kitchen_Small_002";
    private string dks003 = "Demo_Kitchen_Small_003";
    private string dks004 = "Demo_Kitchen_Small_004";
    private string dks005 = "Demo_Kitchen_Small_005";
    private string dks006 = "Demo_Kitchen_Small_006";
    private string dks011 = "Demo_Kitchen_Small_011";
    private string dks014 = "Demo_Kitchen_Small_014";
    private string dks023 = "Demo_Kitchen_Small_023";
    private string dks025 = "Demo_Kitchen_Small_025";
    
    // Medium Kitchens
    private string dkm001 = "Demo_Kitchen_Medium_001";
    private string dkm004 = "Demo_Kitchen_Medium_004";
    private string dkm005 = "Demo_Kitchen_Medium_005";
    private string dkm006 = "Demo_Kitchen_Medium_006";
    private string dkm008 = "Demo_Kitchen_Medium_008";
    private string dkm009 = "Demo_Kitchen_Medium_009";
    private string dkm012 = "Demo_Kitchen_Medium_012";
    private string dkm014 = "Demo_Kitchen_Medium_014";
    private string dkm016 = "Demo_Kitchen_Medium_016";
    private string dkm018 = "Demo_Kitchen_Medium_018";
    private string dkm021 = "Demo_Kitchen_Medium_021";
    private string dkm024 = "Demo_Kitchen_Medium_024";
    

    public override void Execute(IJobExecutionContext executionContext)
    {
        Guard.ThrowOnArgumentNull(executionContext, "executionContext");

        try
        {
            _Logger = LogHelper.GetLogger(executionContext.Area, typeof(SetManualBulks));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);
            
            using(var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
            {
                var prodOrdersRep = unitOfWork.GetRepository<ProductionOrder>();
                
                // Set new bulks + start and end date         
                // Blue bulk
                /*var prodOrderBlueBulk = prodOrdersRep.Get(
                        po => po.CustomerOrderCode == dks003 ||
                              po.CustomerOrderCode == dks004 || 
                              po.CustomerOrderCode == dks005 || 
                              po.CustomerOrderCode == dkm001
                              );*/
                              
                var prodOrderBlueBulk = prodOrdersRep.GetQueryable(false).Where(
                        po => po.CustomerOrderCode == dks003 ||
                              po.CustomerOrderCode == dks004 || 
                              po.CustomerOrderCode == dks005 || 
                              po.CustomerOrderCode == dkm001
                              ).ToList();
                
                if(prodOrderBlueBulk != null)
                {               
                    //Get date
                    var blueBulkDate = prodOrdersRep.GetFirstOrDefault(
                            po => po.ComponentType == ComponentType.SidePanel && 
                                  po.CustomerOrderCode == dks003 || 
                                  po.CustomerOrderCode == dks004 || 
                                  po.CustomerOrderCode == dks005 || 
                                  po.CustomerOrderCode == dkm001
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
                var prodOrderRedBulk = prodOrdersRep.Get(
                        po => po.CustomerOrderCode == dks006 || 
                              po.CustomerOrderCode == dkm006 || 
                              po.CustomerOrderCode == dkm008 || 
                              po.CustomerOrderCode == dkm009
                              );
                
                if(prodOrderRedBulk != null)
                {
                    //Get date
                    var redBulkDate = prodOrdersRep.GetFirstOrDefault(
                            po => po.ComponentType == ComponentType.SidePanel &&
                                  po.CustomerOrderCode == dks006 || 
                                  po.CustomerOrderCode == dkm006 || 
                                  po.CustomerOrderCode == dkm008 || 
                                  po.CustomerOrderCode == dkm009
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
                var prodOrderGreenBulk = prodOrdersRep.Get(
                        po => po.CustomerOrderCode == dks011 || 
                              po.CustomerOrderCode == dks014 || 
                              po.CustomerOrderCode == dkm012 || 
                              po.CustomerOrderCode == dkm014
                              );
                
                if(prodOrderGreenBulk != null)
                {
                    //Get date
                    var greenBulkDate = prodOrdersRep.GetFirstOrDefault(
                            po => po.ComponentType == ComponentType.SidePanel &&
                                  po.CustomerOrderCode == dks011 || 
                                  po.CustomerOrderCode == dks014 || 
                                  po.CustomerOrderCode == dkm012 || 
                                  po.CustomerOrderCode == dkm014
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
                var prodOrderYellowBulk = prodOrdersRep.Get(
                        po => po.CustomerOrderCode == dkm016 || 
                              po.CustomerOrderCode == dkm018
                              );
                
                if(prodOrderYellowBulk != null)
                {               
                    //Get date
                    var yellowBulkDate = prodOrdersRep.GetFirstOrDefault(
                            po => po.ComponentType == ComponentType.SidePanel &&
                                  po.CustomerOrderCode == dkm016 || 
                                  po.CustomerOrderCode == dkm018
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
                var prodOrderBlackBulk = prodOrdersRep.Get(
                        po => po.CustomerOrderCode == dks023 ||
                              po.CustomerOrderCode == dks025 ||
                              po.CustomerOrderCode == dkm021 ||
                              po.CustomerOrderCode == dkm024);
                
                if(prodOrderBlackBulk != null)
                {               
                    //Get date
                    var blackBulkDate = prodOrdersRep.GetFirstOrDefault(
                            po => po.ComponentType == ComponentType.SidePanel && 
                                  po.CustomerOrderCode == dks023 ||
                                  po.CustomerOrderCode == dks025 ||
                                  po.CustomerOrderCode == dkm021 ||
                                  po.CustomerOrderCode == dkm024);
                    
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
                var prodOrderDarkBlueBulk = prodOrdersRep.Get(
                        po => po.CustomerOrderCode == dks002 ||
                              po.CustomerOrderCode == dkm004 ||
                              po.CustomerOrderCode == dkm005);
                
                if(prodOrderDarkBlueBulk != null)
                {               
                    //Get date
                    var darkBlueBulkDate = prodOrdersRep.GetFirstOrDefault(
                            po => po.ComponentType == ComponentType.SidePanel && 
                                  po.CustomerOrderCode == dks002 ||
                                  po.CustomerOrderCode == dkm004 ||
                                  po.CustomerOrderCode == dkm005);
                    
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
