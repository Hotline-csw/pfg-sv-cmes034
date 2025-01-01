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
    
    // Bulk-Blue
    private string dks003 = "Demo_Kitchen_Small_003";
    private string dks004 = "Demo_Kitchen_Small_004";
    private string dks005 = "Demo_Kitchen_Small_005";
    private string dkm001 = "Demo_Kitchen_Medium_001";
    
    // Bulk-Red
    private string dks006 = "Demo_Kitchen_Small_006";
    private string dkm006 = "Demo_Kitchen_Medium_006";
    private string dkm008 = "Demo_Kitchen_Medium_008";
    private string dkm009 = "Demo_Kitchen_Medium_009";
    
    // Bulk-Green
    private string dks011 = "Demo_Kitchen_Small_011";
    private string dks014 = "Demo_Kitchen_Small_014";
    private string dkm012 = "Demo_Kitchen_Medium_012";
    private string dkm014 = "Demo_Kitchen_Medium_014";
    
    // Bulk-Yellow
    private string dkm016 = "Demo_Kitchen_Medium_016";
    private string dkm018 = "Demo_Kitchen_Medium_018";
    
    // Bulk-Black
    private string dks023 = "Demo_Kitchen_Small_023";
    private string dks025 = "Demo_Kitchen_Small_025";
    private string dkm021 = "Demo_Kitchen_Medium_021";
    private string dkm024 = "Demo_Kitchen_Medium_024";
    
    // Bulk-Dark-Blue
    private string dks002 = "Demo_Kitchen_Small_002";
    private string dkm004 = "Demo_Kitchen_Medium_004";
    private string dkm005 = "Demo_Kitchen_Medium_005";

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
