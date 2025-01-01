#r ControllerMES.Infrastructure.Common
#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   UpdateTransferStateSecondSetOfKitchens
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         T.Stürzer
//   Date:           2025-01-01
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   T.Stürzer       2025-01-01    Created
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IServerCustomization))]
[Export("UpdateTransferStateSecondSetOfKitchens", typeof(HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("[10] Update TransferState of the second set of kitchens with bulk planning and feedbacks")]
[EnabledScript(true)]
public class UpdateTransferStateSecondSetOfKitchens : GenericTaskBase, HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;

    private Logger _Logger;
    
    private string _TaskName = "UpdateTransferStateSecondSetOfKitchens";
    

    public override void Execute(IJobExecutionContext executionContext)
    {
        Guard.ThrowOnArgumentNull(executionContext, "executionContext");

        try
        {
            _Logger = LogHelper.GetLogger(executionContext.Area, typeof(UpdateTransferStateSecondSetOfKitchens));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);

            using (var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
            {
                var wccStagingRecordsRep = unitOfWork.GetRepository<WccStagingRecord>();
                var importOrderIds = new[] {
                        "Demo_Kitchen_Small_017",   "Demo_Kitchen_Small_018",   "Demo_Kitchen_Small_019",   "Demo_Kitchen_Medium_019",  // Dark-Yellow-Bulk
                        "Demo_Kitchen_Small_007",   "Demo_Kitchen_Small_008",   "Demo_Kitchen_Medium_007",                              // Dark-Red-Bulk
                        "Demo_Kitchen_Small_012",   "Demo_Kitchen_Small_015",   "Demo_Kitchen_Medium_011",  "Demo_Kitchen_Medium_015",  // Dark-Green-Bulk
                        "Demo_Kitchen_Small_022",   "Demo_Kitchen_Small_024",   "Demo_Kitchen_Medium_022"                               // Dark-Black-Bulk
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
            _Logger.Error(ResourcesKeys.ErrorInUserExit("UpdateTransferStateSecondSetOfKitchens"), null, e);
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
