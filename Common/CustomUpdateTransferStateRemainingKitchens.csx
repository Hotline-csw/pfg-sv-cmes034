#r ControllerMES.Infrastructure.Common
#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   CustomUpdateTransferStateRemainingKitchens
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         T.Stürzer
//   Date:           2023-04-12
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   T.Stürzer       2023-04-12    Created
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IServerCustomization))]
[Export("CustomUpdateTransferStateRemainingKitchens", typeof(HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("[92] Update TransferState for remaining kitchens for bulk planning")]
[EnabledScript(true)]
public class CustomUpdateTransferStateRemainingKitchens : GenericTaskBase, HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;
    
    [Import]
    protected IHelperMethodsCommon _HelperMethodsCommon;

    private Logger _Logger;
    
    private string _TaskName = "CustomUpdateTransferStateRemainingKitchens";
    
    // Small Kitchens
    private string dks001 = "Demo_Kitchen_Small_001";
    private string dks009 = "Demo_Kitchen_Small_009";
    private string dks010 = "Demo_Kitchen_Small_010";
    private string dks013 = "Demo_Kitchen_Small_013";
    private string dks016 = "Demo_Kitchen_Small_016";
    private string dks020 = "Demo_Kitchen_Small_020";
    private string dks021 = "Demo_Kitchen_Small_021";
    
    // Medium Kitchens
    private string dkm002 = "Demo_Kitchen_Medium_002";
    private string dkm003 = "Demo_Kitchen_Medium_003";
    private string dkm010 = "Demo_Kitchen_Medium_010";
    private string dkm013 = "Demo_Kitchen_Medium_013";
    private string dkm017 = "Demo_Kitchen_Medium_017";
    private string dkm020 = "Demo_Kitchen_Medium_020";
    private string dkm023 = "Demo_Kitchen_Medium_023";
    private string dkm025 = "Demo_Kitchen_Medium_025";
    

    public override void Execute(IJobExecutionContext executionContext)
    {
        Guard.ThrowOnArgumentNull(executionContext, "executionContext");

        try
        {
            _Logger = LogHelper.GetLogger(executionContext.Area, typeof(CustomUpdateTransferStateRemainingKitchens));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);

            using(var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
            {
                var wccStagingRecordsRep = unitOfWork.GetRepository<WccStagingRecord>();
                var importOrders = wccStagingRecordsRep.GetQueryable(false).Where(
                        io => //Small Kitchens
                              io.OrderId == dks001 || io.OrderId == dks009 || io.OrderId == dks010 || io.OrderId == dks013 ||
                              io.OrderId == dks016 || io.OrderId == dks020 || io.OrderId == dks021 ||
                              //Medium Kitchens
                              io.OrderId == dkm002 || io.OrderId == dkm003 || io.OrderId == dkm010 || io.OrderId == dkm013 ||
                              io.OrderId == dkm017 || io.OrderId == dkm020 || io.OrderId == dkm023 || io.OrderId == dkm025 );
                
                if(importOrders != null)
                {
                    foreach(var importOrder in importOrders)
                    {
                        importOrder.TransferState = WccStagingTransferState.ImportFromWccToStagingCompleted; // 10
                    }
                    unitOfWork.Save();
                }
            }

        }
        catch (Exception e)
        {
            executionContext.JobResultSetComplete(JobResult.Failed);
            _Logger.Error(ResourcesKeys.ErrorInUserExit("CustomUpdateTransferStateRemainingKitchens"), null, e);
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
