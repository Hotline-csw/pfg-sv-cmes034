#r ControllerMES.Infrastructure.Common
#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   CustomPeriodicCutPart
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         T.Stürzer
//   Date:           2023-09-04
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   T.Stürzer       2023-09-04    Created
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IServerCustomization))]
[Export("CustomPeriodicCutPart", typeof(HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("[98] Feedbacks a part to be cut every minute")]
[EnabledScript(true)]
public class CustomPeriodicCutPart : GenericTaskBase, HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;
    
    [Import]
    protected IDistributedServiceProvider DistributedServiceProvider { get; private set; }

    private Logger _Logger;
    
    private string _TaskName = "CustomPeriodicCutPart";
    
    // WorkCenterCodes
    private string cuttingWorkCenterCode = "1010";
    
    // ProductionStepCodes
    private string cuttingStepCode = "B300";
    

    public override void Execute(IJobExecutionContext executionContext)
    {
        Guard.ThrowOnArgumentNull(executionContext, "executionContext");

        try
        {
            _Logger = LogHelper.GetLogger(executionContext.Area, typeof(CustomPeriodicCutPart));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);

            using(IUnitOfWork unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
            {
                var prodStepRep = unitOfWork.GetRepository<ProductionStep>();
                var prodItemsRep = unitOfWork.GetRepository<ProductionItem>();
                var prodItemsStepsDataRep = unitOfWork.GetRepository<ProductionItemsStepsData>(); 
                
                var cutPart = prodStepRep.GetFirstOrDefault(
                        ps => ps.ProductionState == ProductionStepState.New && ps.Code == cuttingStepCode);
                
                _Logger.Debug(string.Format("{0}: Gefundenes Bauteil: [{1}]", _TaskName, cutPart.ProductionOrderCode));

                if(cutPart != null)
                {
                    var prodItem = prodItemsRep.GetFirstOrDefault(pi => pi.ProductionOrderCode == cutPart.ProductionOrderCode);
                    
                    if(prodItem != null)
                    {
                        var prodItemStepData = prodItemsStepsDataRep.GetFirstOrDefault(
                                pisd => pisd.ProductionOrderCode == cutPart.ProductionOrderCode && pisd.ProductionStepCode == cuttingStepCode);
                            
                        _Logger.Debug(string.Format("{0}: Gefundenes Bauteil: [{1}] | StepCode: [{2}]", _TaskName, prodItemStepData.ProductionOrderCode, prodItemStepData.ProductionStepCode));
                    
                        if(prodItemStepData != null)
                        {
                            prodItem.InsertFeedbackFinishedGood(unitOfWork, _TaskName, cuttingWorkCenterCode, "", 1, _Logger);
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            executionContext.JobResultSetComplete(JobResult.Failed);
            _Logger.Error(ResourcesKeys.ErrorInUserExit("CustomPeriodicCutPart"), null, e);
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
}
