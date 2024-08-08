#r ControllerMES.Infrastructure.Common
#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   UpdateFeedbacks
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         T.Stürzer
//   Date:           2024-08-08
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   T.Stürzer       2024-08-08    Created
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IServerCustomization))]
[Export("UpdateFeedbacks", typeof(HomagGroup.FLS.Services.Common.Contracts.JobScheduling.UserExits.IAfterJobExecutionUserExit))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("[08] Update ProcessingState of Feedbacks")]
[EnabledScript(true)]
public class UpdateFeedbacks : UserExitCustomBase, HomagGroup.FLS.Services.Common.Contracts.JobScheduling.UserExits.IAfterJobExecutionUserExit
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;

    private Logger _Logger;

    public void Execute(IJobExecutionContext executionContext)
    {
        Guard.ThrowOnArgumentNull(executionContext, "executionContext");

        try
        {
            _Logger = LogHelper.GetLogger(executionContext.Area, typeof(UpdateFeedbacks));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);

            using(IUnitOfWork unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
            {
                
                List<Feedback> feedbacksToUpdates = unitOfWork.GetRepository<Feedback>()
                        .Get(fb => fb.ProcessingState == 0).ToList();
                
                if(feedbacksToUpdates != null)
                {
                    foreach(var feedback in feedbacksToUpdates)
                    {
                        feedback.ProcessingState = 10;
                    }
                    
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
