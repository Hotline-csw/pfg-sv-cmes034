#r ControllerMES.Infrastructure.Common
#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   CustomSetAdditionalManualBulks
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         T.Stürzer
//   Date:           2023-04-12
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   T.Stürzer       2023-04-12    Created
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IServerCustomization))]
[Export("CustomSetAdditionalManualBulks", typeof(HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("[91] Set additional manual bulks for videos/screenshots")]
[EnabledScript(true)]
public class CustomSetAdditionalManualBulks : GenericTaskBase, HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask, IPartImportsSatisfiedNotification
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;
    
    [Import]
    private IDistributedServiceProvider _DistributedServiceProvider;
    
    private ICommonServiceDistributed _BulkInfoProvider;

    private Logger _Logger;
    
    private string _TaskName = "CustomSetAdditionalManualBulks";
    
    // Bulk-Dark-Yellow
    private string dks017 = "Demo_Kitchen_Small_017";
    private string dks018 = "Demo_Kitchen_Small_018";
    private string dks019 = "Demo_Kitchen_Small_019";
    private string dkm019 = "Demo_Kitchen_Medium_019";
    
    // Bulk-Dark-Red
    private string dks007 = "Demo_Kitchen_Small_007";
    private string dks008 = "Demo_Kitchen_Small_008";
    private string dkm007 = "Demo_Kitchen_Medium_007";
    
    // Bulk-Dark-Green
    private string dks012 = "Demo_Kitchen_Small_012";
    private string dks015 = "Demo_Kitchen_Small_015";
    private string dkm011 = "Demo_Kitchen_Medium_011";
    private string dkm015 = "Demo_Kitchen_Medium_015";
    
    // Bulk-Dark-Black
    private string dks022 = "Demo_Kitchen_Small_022";
    private string dks024 = "Demo_Kitchen_Small_024";
    private string dkm022 = "Demo_Kitchen_Medium_022";


    public override void Execute(IJobExecutionContext executionContext)
    {
        Guard.ThrowOnArgumentNull(executionContext, "executionContext");

        try
        {
            _Logger = LogHelper.GetLogger(executionContext.Area, typeof(CustomSetAdditionalManualBulks));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);

            using(var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
            {
                var prodOrdersRep = unitOfWork.GetRepository<ProductionOrder>();
                
                // Set new bulks + start and end date
                // Bulk-Dark-Yellow
                var prodOrderBulkDarkYellow = prodOrdersRep.Get(
                        po => po.CustomerOrderCode == dks017 || po.CustomerOrderCode == dks018 || 
                              po.CustomerOrderCode == dks019 || po.CustomerOrderCode == dkm019 );
                
                if(prodOrderBulkDarkYellow != null)
                {               
                    //Get date
                    var bulkDarkYellowDate = prodOrdersRep.GetFirstOrDefault(
                            po => po.ComponentType == ComponentType.SidePanel && po.CustomerOrderCode == dks017);
                                  
                    var bulkDarkYellowStartDate = Convert.ToDateTime(bulkDarkYellowDate.DesiredStartDate);
                    var bulkDarkYellowEndDate = Convert.ToDateTime(bulkDarkYellowDate.DesiredEndDate);
                    
                    //Bulk
                    var bulkDarkYellow = new ManualBulk();               
                    
                    bulkDarkYellow.PlanningNumber = "Bulk-Dark-Yellow";
                    bulkDarkYellow.PlanningState = PlanningState.Planned;
                    bulkDarkYellow.StartDate = bulkDarkYellowStartDate;
                    bulkDarkYellow.Color = "#FFFFEA25";
                    bulkDarkYellow.CreationDate = DateTime.Now;
                    bulkDarkYellow.CreationSource = "BulkPlanning";
                    bulkDarkYellow.ModificationDate = DateTime.Now;
                    bulkDarkYellow.ModificationSource = "BulkPlanning";
                    bulkDarkYellow.EndDate = bulkDarkYellowEndDate;
                    bulkDarkYellow.SchedulingMode = SchedulingMode.Backward;
                    bulkDarkYellow.ProductionOrders = prodOrderBulkDarkYellow;
                        
                    unitOfWork.AddOrUpdate(new[] {bulkDarkYellow});
                    unitOfWork.Save();
                    
                    _BulkInfoProvider.PlanProductionDaysForBulkWithoutDelete(bulkDarkYellow.PlanningNumber, bulkDarkYellow.StartDate, bulkDarkYellow.EndDate);
                    
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Succesfully added Bulk-Dark-Yellow!", _TaskName)));
                }
                
                else
                {
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Error while generating Bulk-Dark-Yellow!", _TaskName)));
                }
                
                
                // Bulk-Dark-Red
                var prodOrderBulkDarkRed = prodOrdersRep.Get(
                        po => po.CustomerOrderCode == dks007 || po.CustomerOrderCode == dks008 || 
                              po.CustomerOrderCode == dkm007 );
                
                if(prodOrderBulkDarkRed != null)
                {               
                    //Get date
                    var bulkDarkRedDate = prodOrdersRep.GetFirstOrDefault(
                            po => po.ComponentType == ComponentType.SidePanel && po.CustomerOrderCode == dks017); 
                                  
                    var bulkDarkRedStartDate = Convert.ToDateTime(bulkDarkRedDate.DesiredStartDate);
                    var bulkDarkRedEndDate = Convert.ToDateTime(bulkDarkRedDate.DesiredEndDate);
                    
                    //Bulk
                    var bulkDarkRed = new ManualBulk();               
                    
                    bulkDarkRed.PlanningNumber = "Bulk-Dark-Red";
                    bulkDarkRed.PlanningState = PlanningState.Planned;
                    bulkDarkRed.StartDate = bulkDarkRedStartDate;
                    bulkDarkRed.Color = "#FFC84B4B";
                    bulkDarkRed.CreationDate = DateTime.Now;
                    bulkDarkRed.CreationSource = "BulkPlanning";
                    bulkDarkRed.ModificationDate = DateTime.Now;
                    bulkDarkRed.ModificationSource = "BulkPlanning";
                    bulkDarkRed.EndDate = bulkDarkRedEndDate;
                    bulkDarkRed.SchedulingMode = SchedulingMode.Backward;
                    bulkDarkRed.ProductionOrders = prodOrderBulkDarkRed;
                        
                    unitOfWork.AddOrUpdate(new[] {bulkDarkRed});
                    unitOfWork.Save();
                    
                    _BulkInfoProvider.PlanProductionDaysForBulkWithoutDelete(bulkDarkRed.PlanningNumber, bulkDarkRed.StartDate, bulkDarkRed.EndDate);
                    
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Succesfully added Bulk-Dark-Red!", _TaskName)));
                }
                
                else
                {
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Error while generating Bulk-Dark-Red!", _TaskName)));
                }
                
                
                // Bulk-Dark-Green
                var prodOrderBulkDarkGreen = prodOrdersRep.Get(
                        po => po.CustomerOrderCode == dks012 || po.CustomerOrderCode == dks015 || 
                              po.CustomerOrderCode == dkm011 || po.CustomerOrderCode == dkm015 );
                
                if(prodOrderBulkDarkGreen != null)
                {               
                    //Get date
                    var bulkDarkGreenDate = prodOrdersRep.GetFirstOrDefault(
                            po => po.ComponentType == ComponentType.SidePanel && po.CustomerOrderCode == dks022);
                                  
                    var bulkDarkGreenStartDate = Convert.ToDateTime(bulkDarkGreenDate.DesiredStartDate);
                    var bulkDarkGreenEndDate = Convert.ToDateTime(bulkDarkGreenDate.DesiredEndDate);
                    
                    //Bulk
                    var bulkDarkGreen = new ManualBulk();               
                    
                    bulkDarkGreen.PlanningNumber = "Bulk-Dark-Green";
                    bulkDarkGreen.PlanningState = PlanningState.Planned;
                    bulkDarkGreen.StartDate = bulkDarkGreenStartDate;
                    bulkDarkGreen.Color = "#FF4BC84B";
                    bulkDarkGreen.CreationDate = DateTime.Now;
                    bulkDarkGreen.CreationSource = "BulkPlanning";
                    bulkDarkGreen.ModificationDate = DateTime.Now;
                    bulkDarkGreen.ModificationSource = "BulkPlanning";
                    bulkDarkGreen.EndDate = bulkDarkGreenEndDate;
                    bulkDarkGreen.SchedulingMode = SchedulingMode.Backward;
                    bulkDarkGreen.ProductionOrders = prodOrderBulkDarkGreen;
                        
                    unitOfWork.AddOrUpdate(new[] {bulkDarkGreen});
                    unitOfWork.Save();
                    
                    _BulkInfoProvider.PlanProductionDaysForBulkWithoutDelete(bulkDarkGreen.PlanningNumber, bulkDarkGreen.StartDate, bulkDarkGreen.EndDate);
                    
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Succesfully added Bulk-Dark-Green!", _TaskName)));
                }
                
                else
                {
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Error while generating Bulk-Dark-Green!", _TaskName)));
                }
                
                
                // Bulk-Dark-Black
                var prodOrderBulkDarkBlack = prodOrdersRep.Get(
                        po => po.CustomerOrderCode == dks022 || po.CustomerOrderCode == dks024 || 
                              po.CustomerOrderCode == dkm022 );
                
                if(prodOrderBulkDarkBlack != null)
                {               
                    //Get date
                    var bulkDarkBlackDate = prodOrdersRep.GetFirstOrDefault(
                            po => po.ComponentType == ComponentType.SidePanel && po.CustomerOrderCode == dks022);
                                  
                    var bulkDarkBlackStartDate = Convert.ToDateTime(bulkDarkBlackDate.DesiredStartDate);
                    var bulkDarkBlackEndDate = Convert.ToDateTime(bulkDarkBlackDate.DesiredEndDate);
                    
                    //Bulk
                    var bulkDarkBlack = new ManualBulk();               
                    
                    bulkDarkBlack.PlanningNumber = "Bulk-Dark-Black";
                    bulkDarkBlack.PlanningState = PlanningState.Planned;
                    bulkDarkBlack.StartDate = bulkDarkBlackStartDate;
                    bulkDarkBlack.Color = "#FF323232";
                    bulkDarkBlack.CreationDate = DateTime.Now;
                    bulkDarkBlack.CreationSource = "BulkPlanning";
                    bulkDarkBlack.ModificationDate = DateTime.Now;
                    bulkDarkBlack.ModificationSource = "BulkPlanning";
                    bulkDarkBlack.EndDate = bulkDarkBlackEndDate;
                    bulkDarkBlack.SchedulingMode = SchedulingMode.Backward;
                    bulkDarkBlack.ProductionOrders = prodOrderBulkDarkBlack;
                        
                    unitOfWork.AddOrUpdate(new[] {bulkDarkBlack});
                    unitOfWork.Save();
                    
                    _BulkInfoProvider.PlanProductionDaysForBulkWithoutDelete(bulkDarkBlack.PlanningNumber, bulkDarkBlack.StartDate, bulkDarkBlack.EndDate);
                    
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Succesfully added Bulk-Dark-Black!", _TaskName)));
                }
                
                else
                {
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Error while generating Bulk-Dark-Black!", _TaskName)));
                }
                
                unitOfWork.Save();                
            }

        }
        catch (Exception e)
        {
            executionContext.JobResultSetComplete(JobResult.Failed);
            _Logger.Error(ResourcesKeys.ErrorInUserExit("CustomSetAdditionalManualBulks"), null, e);
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
