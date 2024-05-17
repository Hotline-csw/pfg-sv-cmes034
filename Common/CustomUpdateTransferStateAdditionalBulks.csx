#r ControllerMES.Infrastructure.Common
#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   CustomUpdateTransferStateAdditionalBulks
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
[Export("CustomUpdateTransferStateAdditionalBulks", typeof(HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("[90] Update TransferState for additional kitchens with bulk planning")]
[EnabledScript(true)]
public class CustomUpdateTransferStateAdditionalBulks : GenericTaskBase, HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;
    
    [Import]
    protected IHelperMethodsCommon _HelperMethodsCommon;

    private Logger _Logger;
    
    private string _TaskName = "CustomUpdateTransferStateAdditionalBulks";
    
    // Bulk-Dark-Yellow
    private string dks017 = "Demo_Kitchen_Small_017";
    private string dks018 = "Demo_Kitchen_Small_018";
    private string dks019 = "Demo_Kitchen_Small_019";
    private string dkm019 = "Demo_Kitchen_Medium_019";
    
    // Bulk-Dark-Red
    private string dks007 = "Demo_Kitchen_Small_007";
    private string dks008 = "Demo_Kitchen_Small_008";
    private string dkm007 = "Demo_Kitchen_Medium_007";
    
    // Bulk-Dark-Green
    private string dks012 = "Demo_Kitchen_Small_012";
    private string dks015 = "Demo_Kitchen_Small_015";
    private string dkm011 = "Demo_Kitchen_Medium_011";
    private string dkm015 = "Demo_Kitchen_Medium_015";
    
    // Bulk-Dark-Black
    private string dks022 = "Demo_Kitchen_Small_022";
    private string dks024 = "Demo_Kitchen_Small_024";
    private string dkm022 = "Demo_Kitchen_Medium_022";
    

    public override void Execute(IJobExecutionContext executionContext)
    {
        Guard.ThrowOnArgumentNull(executionContext, "executionContext");

        try
        {
            _Logger = LogHelper.GetLogger(executionContext.Area, typeof(CustomUpdateTransferStateAdditionalBulks));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);

            using(var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
            {
                var wccStagingRecordsRep = unitOfWork.GetRepository<WccStagingRecord>();
                var importOrders = wccStagingRecordsRep.GetQueryable(false).Where(
                        io => //Bulk-Dark-Yellow
                              io.OrderId == dks017 || io.OrderId == dks018 || io.OrderId == dks019 || io.OrderId == dkm019 ||
                              //Bulk-Dark-Red
                              io.OrderId == dks007 || io.OrderId == dks008 || io.OrderId == dkm007 ||
                              //Bulk-Dark-Green
                              io.OrderId == dks012 || io.OrderId == dks015 || io.OrderId == dkm011 || io.OrderId == dkm015 ||
                              //Bulk-Dark-Black
                              io.OrderId == dks022 || io.OrderId == dks024 || io.OrderId == dkm022 );
                
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
            _Logger.Error(ResourcesKeys.ErrorInUserExit("CustomUpdateTransferStateAdditionalBulks"), null, e);
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
