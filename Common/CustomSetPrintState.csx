#r ControllerMES.Infrastructure.Common
#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   CustomSetPrintState
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
[Export("CustomSetPrintState", typeof(HomagGroup.FLS.Services.Common.Contracts.JobScheduling.UserExits.IBeforeJobExecutionUserExit))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("30_Set printstate for furniture label")]
[EnabledScript(true)]
public class CustomSetPrintState : UserExitCustomBase, HomagGroup.FLS.Services.Common.Contracts.JobScheduling.UserExits.IBeforeJobExecutionUserExit
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;

    private Logger _Logger;

    public void Execute(IJobExecutionContext executionContext)
    {
        Guard.ThrowOnArgumentNull(executionContext, "executionContext");

        try
        {
            _Logger = LogHelper.GetLogger(executionContext.Area, typeof(CustomSetPrintState));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);

            using(var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
            {
                var printJobRepository = unitOfWork.GetRepository<PrintJobItem>();
                var printFurnLabel = printJobRepository.GetFirstOrDefault(x => x.ProcessingState == PrintJobItemProcessingState.ReadyForPrinting && x.JobName == executionContext.Name, "SEQUENCE");
                
                if(printFurnLabel != null)
                {
                    printFurnLabel.ProcessingState = PrintJobItemProcessingState.InPrintingProcess; //15
                    unitOfWork.Save();
                }
                
                else
                {
                    // No pending printJob
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
