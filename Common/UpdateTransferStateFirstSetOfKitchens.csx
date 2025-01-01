#r ControllerMES.Infrastructure.Common
#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   UpdateTransferStateFirstSetOfKitchens
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
[Export("UpdateTransferStateFirstSetOfKitchens", typeof(HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("[02] Update TransferState for kitchens with bulk planning and feedbacks")]
[EnabledScript(true)]
public class UpdateTransferStateFirstSetOfKitchens : GenericTaskBase, HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;
    
    [Import]
    protected IHelperMethodsCommon _HelperMethodsCommon;

    private Logger _Logger;
    
    private string _TaskName = "UpdateTransferStateFirstSetOfKitchens";
    

    public override void Execute(IJobExecutionContext executionContext)
    {
        Guard.ThrowOnArgumentNull(executionContext, "executionContext");

        try
        {
            _Logger = LogHelper.GetLogger(executionContext.Area, typeof(UpdateTransferStateFirstSetOfKitchens));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);
            
            using (var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
            {
                var wccStagingRecordsRep = unitOfWork.GetRepository<WccStagingRecord>();
                var importOrderIds = new[] {
                        "Demo_Kitchen_Small_003",   "Demo_Kitchen_Small_004",   "Demo_Kitchen_Small_005",   "Demo_Kitchen_Medium_001",  //Blue-Bulk
                        "Demo_Kitchen_Small_006",   "Demo_Kitchen_Medium_006",  "Demo_Kitchen_Medium_008",  "Demo_Kitchen_Medium_009",  // Red-Bulk
                        "Demo_Kitchen_Small_011",   "Demo_Kitchen_Small_014",   "Demo_Kitchen_Medium_012",  "Demo_Kitchen_Medium_014",  // Green-Bulk
                        "Demo_Kitchen_Medium_016",  "Demo_Kitchen_Medium_018",                                                          // Yellow-Bulk
                        "Demo_Kitchen_Small_023",   "Demo_Kitchen_Small_025",   "Demo_Kitchen_Medium_021",  "Demo_Kitchen_Medium_024",  // Black-Bulk
                        "Demo_Kitchen_Small_002",   "Demo_Kitchen_Medium_004",  "Demo_Kitchen_Medium_005"                               // Dark-Blue-Bulk                
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
            _Logger.Error(ResourcesKeys.ErrorInUserExit("UpdateTransferStateFirstSetOfKitchens"), null, e);
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
