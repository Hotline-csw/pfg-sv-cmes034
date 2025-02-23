#r ControllerMES.Infrastructure.Common
#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   AfterFeedbackMessage
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         <Author>
//   Date:           2025-02-23
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   <Author>        2025-02-23    Created
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IServerCustomization))]
[Export("AfterFeedbackMessage", typeof(HomagGroup.FLS.Services.Common.Contracts.JobScheduling.UserExits.IAfterJobExecutionUserExit))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("describe here")]
[EnabledScript(true)]
public class AfterFeedbackMessage : UserExitCustomBase, HomagGroup.FLS.Services.Common.Contracts.JobScheduling.UserExits.IAfterJobExecutionUserExit
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;

    private Logger _Logger;

    public void Execute(IJobExecutionContext executionContext)
    {
        Guard.ThrowOnArgumentNull(executionContext, "executionContext");

        try
        {
            _Logger = LogHelper.GetLogger(executionContext.Area, typeof(AfterFeedbackMessage));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);
            
            using(IUnitOfWork unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
            {
                /*if(UserExitHelper.ViewIdentifier.Contains("GenericManagement.CartBinding"))
                {
                
                }*/
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
