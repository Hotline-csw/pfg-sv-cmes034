#r ControllerMES.Infrastructure.Common
#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   CustomSetRemainingBulks
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
[Export("CustomSetRemainingBulks", typeof(HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("[93] Set remaining bulks")]
[EnabledScript(true)]
public class CustomSetRemainingBulks : GenericTaskBase, HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask, IPartImportsSatisfiedNotification
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;
    
    [Import]
    private IDistributedServiceProvider _DistributedServiceProvider;
    
    private ICommonServiceDistributed _BulkInfoProvider;

    private Logger _Logger;
    
    private string _TaskName = "CustomSetRemainingBulks";
    
	// Bulk-Bright-Yellow
	private string dks016 = "Demo_Kitchen_Small_016";
	private string dks020 = "Demo_Kitchen_Small_020";
	private string dkm017 = "Demo_Kitchen_Medium_017";
	private string dkm020 = "Demo_Kitchen_Medium_020";
	
	// Bulk-Bright-Red
	private string dks009 = "Demo_Kitchen_Small_009";
	private string dks010 = "Demo_Kitchen_Small_010";
	private string dkm010 = "Demo_Kitchen_Medium_010";
	
	// Bulk-Bright-Blue
	private string dks001 = "Demo_Kitchen_Small_001";
	private string dkm002 = "Demo_Kitchen_Medium_002";
	private string dkm003 = "Demo_Kitchen_Medium_003";
	
	// Bulk-Bright-Black
	private string dks021 = "Demo_Kitchen_Small_021";
	private string dkm023 = "Demo_Kitchen_Medium_023";
	private string dkm025 = "Demo_Kitchen_Medium_025";
	
	// Bulk-Bright-Green
	private string dks013 = "Demo_Kitchen_Small_013";
	private string dkm013 = "Demo_Kitchen_Medium_013";
    

    public override void Execute(IJobExecutionContext executionContext)
    {
        Guard.ThrowOnArgumentNull(executionContext, "executionContext");

        try
        {
            _Logger = LogHelper.GetLogger(executionContext.Area, typeof(CustomSetRemainingBulks));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);

            using(var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
            {
                var prodOrdersRep = unitOfWork.GetRepository<ProductionOrder>();
                
                // Set new bulks + start and end date
                // Bulk-Bright-Yellow
                var prodOrderBulkBrightYellow = prodOrdersRep.Get(
                        po => po.CustomerOrderCode == dks016 || po.CustomerOrderCode == dks020 || 
                              po.CustomerOrderCode == dkm017 || po.CustomerOrderCode == dkm020 );
                
                if(prodOrderBulkBrightYellow != null)
                {               
                    //Get date
                    var bulkBrightYellowDate = prodOrdersRep.GetFirstOrDefault(
                            po => po.ComponentType == ComponentType.SidePanel && po.CustomerOrderCode == dks016);
                                  
                    var bulkBrightYellowStartDate = Convert.ToDateTime(bulkBrightYellowDate.DesiredStartDate);
                    var bulkBrightYellowEndDate = Convert.ToDateTime(bulkBrightYellowDate.DesiredEndDate);
                    
                    //Bulk
                    var bulkBrightYellow = new ManualBulk();               
                    
                    bulkBrightYellow.PlanningNumber = "Bulk-Bright-Yellow";
                    bulkBrightYellow.PlanningState = PlanningState.Planned;
                    bulkBrightYellow.StartDate = bulkBrightYellowStartDate;
                    bulkBrightYellow.Color = "#FFF4FF7E";
                    bulkBrightYellow.CreationDate = DateTime.Now;
                    bulkBrightYellow.CreationSource = "BulkPlanning";
                    bulkBrightYellow.ModificationDate = DateTime.Now;
                    bulkBrightYellow.ModificationSource = "BulkPlanning";
                    bulkBrightYellow.EndDate = bulkBrightYellowEndDate;
                    bulkBrightYellow.SchedulingMode = SchedulingMode.Backward;
                    bulkBrightYellow.ProductionOrders = prodOrderBulkBrightYellow;
                        
                    unitOfWork.AddOrUpdate(new[] {bulkBrightYellow});
                    unitOfWork.Save();
                    
                    _BulkInfoProvider.PlanProductionDaysForBulkWithoutDelete(bulkBrightYellow.PlanningNumber, bulkBrightYellow.StartDate, bulkBrightYellow.EndDate);
                    
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Succesfully added Bulk-Bright-Yellow!", _TaskName)));
                }
                
                else
                {
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Error while generating Bulk-Bright-Yellow!", _TaskName)));
                }
                
                // Bulk-Bright-Red
                var prodOrderBulkBrightRed = prodOrdersRep.Get(
                        po => po.CustomerOrderCode == dks009 || po.CustomerOrderCode == dks010 || 
                              po.CustomerOrderCode == dkm010 );
                
                if(prodOrderBulkBrightRed != null)
                {               
                    //Get date
                    var bulkBrightRedDate = prodOrdersRep.GetFirstOrDefault(
                            po => po.ComponentType == ComponentType.SidePanel && po.CustomerOrderCode == dks016);
                                  
                    var bulkBrightRedStartDate = Convert.ToDateTime(bulkBrightRedDate.DesiredStartDate);
                    var bulkBrightRedEndDate = Convert.ToDateTime(bulkBrightRedDate.DesiredEndDate);
                    
                    //Bulk
                    var bulkBrightRed = new ManualBulk();               
                    
                    bulkBrightRed.PlanningNumber = "Bulk-Bright-Red";
                    bulkBrightRed.PlanningState = PlanningState.Planned;
                    bulkBrightRed.StartDate = bulkBrightRedStartDate;
                    bulkBrightRed.Color = "#FFFAC8C8";
                    bulkBrightRed.CreationDate = DateTime.Now;
                    bulkBrightRed.CreationSource = "BulkPlanning";
                    bulkBrightRed.ModificationDate = DateTime.Now;
                    bulkBrightRed.ModificationSource = "BulkPlanning";
                    bulkBrightRed.EndDate = bulkBrightRedEndDate;
                    bulkBrightRed.SchedulingMode = SchedulingMode.Backward;
                    bulkBrightRed.ProductionOrders = prodOrderBulkBrightRed;
                        
                    unitOfWork.AddOrUpdate(new[] {bulkBrightRed});
                    unitOfWork.Save();
                    
                    _BulkInfoProvider.PlanProductionDaysForBulkWithoutDelete(bulkBrightRed.PlanningNumber, bulkBrightRed.StartDate, bulkBrightRed.EndDate);
                    
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Succesfully added Bulk-Bright-Red!", _TaskName)));
                }
                
                else
                {
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Error while generating Bulk-Bright-Red!", _TaskName)));
                }
                
                // Bulk-Bright-Blue
                var prodOrderBulkBrightBlue = prodOrdersRep.Get(
                        po => po.CustomerOrderCode == dks001 || po.CustomerOrderCode == dkm002 || 
                              po.CustomerOrderCode == dkm003 );
                
                if(prodOrderBulkBrightBlue != null)
                {               
                    //Get date
                    var bulkBrightBlueDate = prodOrdersRep.GetFirstOrDefault(
                            po => po.ComponentType == ComponentType.SidePanel && po.CustomerOrderCode == dks021);
                                  
                    var bulkBrightBlueStartDate = Convert.ToDateTime(bulkBrightBlueDate.DesiredStartDate);
                    var bulkBrightBlueEndDate = Convert.ToDateTime(bulkBrightBlueDate.DesiredEndDate);
                    
                    //Bulk
                    var bulkBrightBlue = new ManualBulk();               
                    
                    bulkBrightBlue.PlanningNumber = "Bulk-Bright-Blue";
                    bulkBrightBlue.PlanningState = PlanningState.Planned;
                    bulkBrightBlue.StartDate = bulkBrightBlueStartDate;
                    bulkBrightBlue.Color = "#FFC8C8FA";
                    bulkBrightBlue.CreationDate = DateTime.Now;
                    bulkBrightBlue.CreationSource = "BulkPlanning";
                    bulkBrightBlue.ModificationDate = DateTime.Now;
                    bulkBrightBlue.ModificationSource = "BulkPlanning";
                    bulkBrightBlue.EndDate = bulkBrightBlueEndDate;
                    bulkBrightBlue.SchedulingMode = SchedulingMode.Backward;
                    bulkBrightBlue.ProductionOrders = prodOrderBulkBrightBlue;
                        
                    unitOfWork.AddOrUpdate(new[] {bulkBrightBlue});
                    unitOfWork.Save();
                    
                    _BulkInfoProvider.PlanProductionDaysForBulkWithoutDelete(bulkBrightBlue.PlanningNumber, bulkBrightBlue.StartDate, bulkBrightBlue.EndDate);
                    
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Succesfully added Bulk-Bright-Blue!", _TaskName)));
                }
                
                else
                {
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Error while generating Bulk-Bright-Blue!", _TaskName)));
                }
                
                // Bulk-Bright-Black
                var prodOrderBulkBrightBlack = prodOrdersRep.Get(
                        po => po.CustomerOrderCode == dks021 || po.CustomerOrderCode == dkm023 || 
                              po.CustomerOrderCode == dkm025 );
                
                if(prodOrderBulkBrightBlack != null)
                {               
                    //Get date
                    var bulkBrightBlackDate = prodOrdersRep.GetFirstOrDefault(
                            po => po.ComponentType == ComponentType.SidePanel && po.CustomerOrderCode == dks021);
                                  
                    var bulkBrightBlackStartDate = Convert.ToDateTime(bulkBrightBlackDate.DesiredStartDate);
                    var bulkBrightBlackEndDate = Convert.ToDateTime(bulkBrightBlackDate.DesiredEndDate);
                    
                    //Bulk
                    var bulkBrightBlack = new ManualBulk();               
                    
                    bulkBrightBlack.PlanningNumber = "Bulk-Bright-Black";
                    bulkBrightBlack.PlanningState = PlanningState.Planned;
                    bulkBrightBlack.StartDate = bulkBrightBlackStartDate;
                    bulkBrightBlack.Color = "#FFE6E6E6";
                    bulkBrightBlack.CreationDate = DateTime.Now;
                    bulkBrightBlack.CreationSource = "BulkPlanning";
                    bulkBrightBlack.ModificationDate = DateTime.Now;
                    bulkBrightBlack.ModificationSource = "BulkPlanning";
                    bulkBrightBlack.EndDate = bulkBrightBlackEndDate;
                    bulkBrightBlack.SchedulingMode = SchedulingMode.Backward;
                    bulkBrightBlack.ProductionOrders = prodOrderBulkBrightBlack;
                        
                    unitOfWork.AddOrUpdate(new[] {bulkBrightBlack});
                    unitOfWork.Save();
                    
                    _BulkInfoProvider.PlanProductionDaysForBulkWithoutDelete(bulkBrightBlack.PlanningNumber, bulkBrightBlack.StartDate, bulkBrightBlack.EndDate);
                    
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Succesfully added Bulk-Bright-Black!", _TaskName)));
                }
                
                else
                {
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Error while generating Bulk-Bright-Black!", _TaskName)));
                }
                
                // Bulk-Bright-Green
                var prodOrderBulkBrightGreen = prodOrdersRep.Get(
                        po => po.CustomerOrderCode == dks013 || po.CustomerOrderCode == dkm013 );
                
                if(prodOrderBulkBrightGreen != null)
                {               
                    //Get date
                    var bulkBrightGreenDate = prodOrdersRep.GetFirstOrDefault(
                            po => po.ComponentType == ComponentType.SidePanel && po.CustomerOrderCode == dks013);
                                  
                    var bulkBrightGreenStartDate = Convert.ToDateTime(bulkBrightGreenDate.DesiredStartDate);
                    var bulkBrightGreenEndDate = Convert.ToDateTime(bulkBrightGreenDate.DesiredEndDate);
                    
                    //Bulk
                    var bulkBrightGreen = new ManualBulk();               
                    
                    bulkBrightGreen.PlanningNumber = "Bulk-Bright-Green";
                    bulkBrightGreen.PlanningState = PlanningState.Planned;
                    bulkBrightGreen.StartDate = bulkBrightGreenStartDate;
                    bulkBrightGreen.Color = "#FFC8FAC8";
                    bulkBrightGreen.CreationDate = DateTime.Now;
                    bulkBrightGreen.CreationSource = "BulkPlanning";
                    bulkBrightGreen.ModificationDate = DateTime.Now;
                    bulkBrightGreen.ModificationSource = "BulkPlanning";
                    bulkBrightGreen.EndDate = bulkBrightGreenEndDate;
                    bulkBrightGreen.SchedulingMode = SchedulingMode.Backward;
                    bulkBrightGreen.ProductionOrders = prodOrderBulkBrightGreen;
                        
                    unitOfWork.AddOrUpdate(new[] {bulkBrightGreen});
                    unitOfWork.Save();
                    
                    _BulkInfoProvider.PlanProductionDaysForBulkWithoutDelete(bulkBrightGreen.PlanningNumber, bulkBrightGreen.StartDate, bulkBrightGreen.EndDate);
                    
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Succesfully added Bulk-Bright-Green!", _TaskName)));
                }
                
                else
                {
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Error while generating Bulk-Bright-Green!", _TaskName)));
                }
                
                unitOfWork.Save();
            }
        }
        catch (Exception e)
        {
            executionContext.JobResultSetComplete(JobResult.Failed);
            _Logger.Error(ResourcesKeys.ErrorInUserExit("CustomSetRemainingBulks"), null, e);
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
