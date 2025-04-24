#r ControllerMES.Infrastructure.Common
#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   DeleteAllCsvStagingRecords
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         M. Schiebel
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
[Export("DeleteAllCsvStagingRecords", typeof(HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("[01] Delete all post-production parts in base.CsvStagingRecords")]
[EnabledScript(true)]
public class DeleteAllCsvStagingRecords : GenericTaskBase, HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;

    private Logger _Logger;
    
    private string _TaskName = "DeleteAllCsvStagingRecords";    


    public override void Execute(IJobExecutionContext executionContext)
    {
        Guard.ThrowOnArgumentNull(executionContext, "executionContext");

        try
        {
            _Logger = LogHelper.GetLogger(executionContext.Area, typeof(DeleteAllCsvStagingRecords));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);

            using(IUnitOfWork unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
            {
                var csvStagingRecordsRep = unitOfWork.GetRepository<CsvStagingRecord>();
                var csvStagingRecord = csvStagingRecordsRep.GetQueryable(false);
                
                if(csvStagingRecord.Any())
                {
                    unitOfWork.BulkDelete(csvStagingRecord);
                    
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Deleted all csv staging records successfully!", _TaskName)));
                }
                
                else
                {
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Error while deleting csv staging records!", _TaskName)));
                }
                
                unitOfWork.Save();
            }
        }
        catch (Exception e)
        {
            executionContext.JobResultSetComplete(JobResult.Failed);
            _Logger.Error(ResourcesKeys.ErrorInUserExit("DeleteAllCsvStagingRecords"), null, e);
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
