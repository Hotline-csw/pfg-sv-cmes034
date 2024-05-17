#r ControllerMES.Infrastructure.Common
#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   CustomUpdatePrintState
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         T.Stürzer
//   Date:           2023-02-14
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   T.Stürzer       2023-02-14    Created
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IServerCustomization))]
[Export("CustomUpdatePrintState", typeof(HomagGroup.FLS.Services.Common.Contracts.JobScheduling.UserExits.IAfterJobExecutionUserExit))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("30_Update printstate for furniture label")]
[EnabledScript(true)]
public class CustomUpdatePrintState : UserExitCustomBase, HomagGroup.FLS.Services.Common.Contracts.JobScheduling.UserExits.IAfterJobExecutionUserExit
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;

    private Logger _Logger;

    public void Execute(IJobExecutionContext executionContext)
    {
        Guard.ThrowOnArgumentNull(executionContext, "executionContext");

        try
        {
            _Logger = LogHelper.GetLogger(executionContext.Area, typeof(CustomUpdatePrintState));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);

            using(var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
            {
                var printJobRepository = unitOfWork.GetRepository<PrintJobItem>();
                var printFurnLabel = printJobRepository.GetFirstOrDefault(x => x.ProcessingState == PrintJobItemProcessingState.InPrintingProcess && x.JobName == executionContext.Name, "SEQUENCE");
                
                _Logger.Info("unitOfWork");
                
                if(printFurnLabel != null)
                {
                    printFurnLabel.ProcessingState = PrintJobItemProcessingState.Printed; // 20
                    unitOfWork.Save();
                }
            }
        }
        catch (Exception e)
        {
            executionContext.JobResultSetComplete(JobResult.Failed);
            _Logger.Error(ResourcesKeys.ErrorInUserExit(Name), null, e);
            throw;
        }
    }
}
