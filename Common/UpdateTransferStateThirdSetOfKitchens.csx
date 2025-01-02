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
[Description("[92] Update TransferState of the third set of kitchens with bulk planning")]
[EnabledScript(true)]
public class UpdateTransferStateThirdSetOfKitchens : GenericTaskBase, HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;

    private Logger _Logger;
    
    private string _TaskName = "UpdateTransferStateThirdSetOfKitchens";
    

    public override void Execute(IJobExecutionContext executionContext)
    {
        Guard.ThrowOnArgumentNull(executionContext, "executionContext");

        try
        {
            _Logger = LogHelper.GetLogger(executionContext.Area, typeof(UpdateTransferStateThirdSetOfKitchens));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);

            using (var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
            {
                var wccStagingRecordsRep = unitOfWork.GetRepository<WccStagingRecord>();
                var importOrderIds = new[] {
                        "Demo_Kitchen_Small_016",   "Demo_Kitchen_Small_020",   "Demo_Kitchen_Medium_017",  "Demo_Kitchen_Medium_020",  // Bright-Yellow-Bulk
                        "Demo_Kitchen_Small_009",   "Demo_Kitchen_Small_010",   "Demo_Kitchen_Medium_010",                              // Bright-Red-Bulk
                        "Demo_Kitchen_Small_001",   "Demo_Kitchen_Medium_002",  "Demo_Kitchen_Medium_003",                              // Bright-Blue-Bulk
                        "Demo_Kitchen_Small_021",   "Demo_Kitchen_Medium_023",  "Demo_Kitchen_Medium_025",                              // Bright-Black-Bulk
                        "Demo_Kitchen_Small_013",   "Demo_Kitchen_Medium_013"                                                           // Bright-Green-Bulk
                };
                
            
                var importOrders = wccStagingRecordsRep.GetQueryable(false).Where(io => importOrderIds.Contains(io.OrderId)).ToList();
            
                if (importOrders.Any())
                {
                    foreach (var importOrder in importOrders)
                    {
                        importOrder.TransferState = HomagGroup.FLS.Domain.Data.WccStagingTransferState.ImportFromWccToStagingCompleted;
                    }
            
                    unitOfWork.BulkUpdate(importOrders);
                }
            }
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
