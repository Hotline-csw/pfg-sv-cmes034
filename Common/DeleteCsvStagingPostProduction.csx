#r ControllerMES.Infrastructure.Common
#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   DeleteCsvStagingPostProduction
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         <Author>
//   Date:           2025-04-24
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   <Author>        2025-04-24    Created
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IServerCustomization))]
[Export("DeleteCsvStagingPostProduction", typeof(HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("[01] Delete all post-production parts in base.CsvStagingRecords")]
[EnabledScript(true)]
public class DeleteCsvStagingPostProduction : GenericTaskBase, HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;

    private Logger _Logger;
    
    private string _TaskName = "DeleteCsvStagingPostProduction";    


    public override void Execute(IJobExecutionContext executionContext)
    {
        Guard.ThrowOnArgumentNull(executionContext, "executionContext");

        try
        {
            _Logger = LogHelper.GetLogger(executionContext.Area, typeof(DeleteCsvStagingPostProduction));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);

            using(IUnitOfWork unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
            {
                var csvStagingRecordsRep = unitOfWork.GetRepository<CsvStagingRecord>();
                var csvStagingRecord = csvStagingRecordsRep.GetQueryable(false).Where(wcr => wcr.ReproductionType == 1);
                
                if(csvStagingRecord.Any())
                {
                    unitOfWork.BulkDelete(csvStagingRecord);
                    
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Deleted all post-production parts successfully!", _TaskName)));
                }
                
                else
                {
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Error while deleting post-production parts!", _TaskName)));
                }
                
                unitOfWork.Save();
            }
        }
        catch (Exception e)
        {
            executionContext.JobResultSetComplete(JobResult.Failed);
            _Logger.Error(ResourcesKeys.ErrorInUserExit("DeleteCsvStagingPostProduction"), null, e);
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
