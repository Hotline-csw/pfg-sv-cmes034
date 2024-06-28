#r ControllerMES.Infrastructure.Common
#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   DeleteOptimizations
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         T.Stürzer
//   Date:           2023-02-23
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   T.Stürzer       2023-02-23    Created
//   T.Stürzer       2023-03-23    Logging and error messages
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IServerCustomization))]
[Export("DeleteOptimizations", typeof(HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("[01] Delete all optimizations")]
[EnabledScript(true)]
public class DeleteOptimizations : GenericTaskBase, HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;

    private Logger _Logger;
    
    private string _TaskName = "DeleteOptimizations";
    

    public override void Execute(IJobExecutionContext executionContext)
    {
        Guard.ThrowOnArgumentNull(executionContext, "executionContext");

        try
        {
            _Logger = LogHelper.GetLogger(executionContext.Area, typeof(DeleteOptimizations));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);

            using(IUnitOfWork unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
            {
                var optimizationsRep = unitOfWork.GetRepository<Optimization>();
                var optimization = optimizationsRep.GetQueryable(false);
                
                if(optimization.Any())
                {
                    unitOfWork.BulkDelete(optimization);
                    
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Deleted all optimizations successfully!", _TaskName)));
                }
                
                else
                {
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Problems while deleting optimizations!", _TaskName)));
                }
                
                unitOfWork.Save();
            }
        }
        catch (Exception e)
        {
            executionContext.JobResultSetComplete(JobResult.Failed);
            _Logger.Error(ResourcesKeys.ErrorInUserExit("DeleteOptimizations"), null, e);
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
