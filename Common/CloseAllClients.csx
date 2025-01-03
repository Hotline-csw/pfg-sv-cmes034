#r ControllerMES.Infrastructure.Common
#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   CloseAllClients
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         T.Stürzer
//   Date:           2025-01-03
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   T.Stürzer       2025-01-03    Created
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IServerCustomization))]
[Export("CloseAllClients", typeof(HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("[99] Task to close all open clients")]
[EnabledScript(true)]
public class CloseAllClients : GenericTaskBase, HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask
                                , IPartImportsSatisfiedNotification
{
    [Import]
    private IDistributedServiceProvider DistributedServiceProvider { get; set; }
    
    [Import]
    protected IMessagingService _MessagingService { get; private set; }

    private Logger _Logger;

    public override void Execute(IJobExecutionContext executionContext)
    {
        Guard.ThrowOnArgumentNull(executionContext, "executionContext");

        try
        {
            _Logger = LogHelper.GetLogger(executionContext.Area, typeof(CloseAllClients));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);

            _MessagingService.Publish(new CloseAllClientsMessage(this));
        }
        catch (Exception e)
        {
            executionContext.JobResultSetComplete(JobResult.Failed);
            _Logger.Error(ResourcesKeys.ErrorInUserExit("CloseAllClients"), null, e);
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
        _MessagingService = DistributedServiceProvider.GetService<IMessagingServiceDistributed>();
    }
}
