#r ControllerMES.Infrastructure.Common
#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   UpdateTransferStateThirdSetOfKitchens
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         T.Stürzer
//   Date:           2025-01-02
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   T.Stürzer       2025-01-02    Created
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IServerCustomization))]
[Export("UpdateTransferStateThirdSetOfKitchens", typeof(HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("describe here")]
[EnabledScript(true)]
public class UpdateTransferStateThirdSetOfKitchens : GenericTaskBase, HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;

    private Logger _Logger;

    public override void Execute(IJobExecutionContext executionContext)
    {
        Guard.ThrowOnArgumentNull(executionContext, "executionContext");

        try
        {
            _Logger = LogHelper.GetLogger(executionContext.Area, typeof(UpdateTransferStateThirdSetOfKitchens));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);

            throw new NotImplementedException();
            
            // example to read the value of a parameter, defined below in UserExitInputParameters:
            // string myParameter = executionContext.Inputs.FirstOrDefault(input => input.Key == "MyParameter").Value.ToString();

        }
        catch (Exception e)
        {
            executionContext.JobResultSetComplete(JobResult.Failed);
            _Logger.Error(ResourcesKeys.ErrorInUserExit("UpdateTransferStateThirdSetOfKitchens"), null, e);
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
