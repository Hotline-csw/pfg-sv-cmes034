#r ControllerMES.Infrastructure.Common
#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   UpdateTransferStateKitchensWithoutBulksAndFeedback
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         T.Stürzer
//   Date:           2023-04-06
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   T.Stürzer       2023-04-06    Created
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IServerCustomization))]
[Export("UpdateTransferStateKitchensWithoutBulksAndFeedback", typeof(HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("09_Update TransferState for kitchens without bulk planning and feedbacks")]
[EnabledScript(true)]
public class UpdateTransferStateKitchensWithoutBulksAndFeedback : GenericTaskBase, HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;
    
    [Import]
    protected IHelperMethodsCommon _HelperMethodsCommon;

    private Logger _Logger;
    
    private string _TaskName = "UpdateTransferStateKitchensWithoutBulksAndFeedback";

    // Small Kitchens
    private string dks001 = "Demo_Kitchen_Small_001";
    private string dks007 = "Demo_Kitchen_Small_007";
    private string dks008 = "Demo_Kitchen_Small_008";
    private string dks009 = "Demo_Kitchen_Small_009";
    private string dks010 = "Demo_Kitchen_Small_010";
    private string dks012 = "Demo_Kitchen_Small_012";
    private string dks013 = "Demo_Kitchen_Small_013";
    private string dks015 = "Demo_Kitchen_Small_015";
    private string dks016 = "Demo_Kitchen_Small_016";
    private string dks017 = "Demo_Kitchen_Small_017";
    private string dks018 = "Demo_Kitchen_Small_018";
    private string dks019 = "Demo_Kitchen_Small_019";
    private string dks020 = "Demo_Kitchen_Small_020";
    private string dks021 = "Demo_Kitchen_Small_021";
    private string dks022 = "Demo_Kitchen_Small_022";
    private string dks024 = "Demo_Kitchen_Small_024";
    
    // Medium Kitchens
    private string dkm002 = "Demo_Kitchen_Medium_002";
    private string dkm003 = "Demo_Kitchen_Medium_003";
    private string dkm007 = "Demo_Kitchen_Medium_007";
    private string dkm010 = "Demo_Kitchen_Medium_010";
    private string dkm011 = "Demo_Kitchen_Medium_011";
    private string dkm013 = "Demo_Kitchen_Medium_013";
    private string dkm015 = "Demo_Kitchen_Medium_015";
    private string dkm017 = "Demo_Kitchen_Medium_017";
    private string dkm019 = "Demo_Kitchen_Medium_019";
    private string dkm020 = "Demo_Kitchen_Medium_020";
    private string dkm022 = "Demo_Kitchen_Medium_022";
    private string dkm023 = "Demo_Kitchen_Medium_023";
    private string dkm025 = "Demo_Kitchen_Medium_025";


    public override void Execute(IJobExecutionContext executionContext)
    {
        Guard.ThrowOnArgumentNull(executionContext, "executionContext");

        try
        {
            _Logger = LogHelper.GetLogger(executionContext.Area, typeof(UpdateTransferStateKitchensWithoutBulksAndFeedback));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);
            
            using(var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
            {
                var wccStagingRecordsRep = unitOfWork.GetRepository<WccStagingRecord>();
                var importOrders = wccStagingRecordsRep.GetQueryable(false).Where(
                        io => // Small Kitchens
                              io.OrderId == dks001 && io.ReproductionType == 0 ||
                              io.OrderId == dks007 && io.ReproductionType == 0 ||
                              io.OrderId == dks008 && io.ReproductionType == 0 ||
                              io.OrderId == dks009 && io.ReproductionType == 0 ||
                              io.OrderId == dks010 && io.ReproductionType == 0 ||
                              io.OrderId == dks012 && io.ReproductionType == 0 ||
                              io.OrderId == dks013 && io.ReproductionType == 0 ||
                              io.OrderId == dks015 && io.ReproductionType == 0 ||
                              io.OrderId == dks016 && io.ReproductionType == 0 ||
                              io.OrderId == dks017 && io.ReproductionType == 0 ||
                              io.OrderId == dks018 && io.ReproductionType == 0 ||
                              io.OrderId == dks019 && io.ReproductionType == 0 ||
                              io.OrderId == dks020 && io.ReproductionType == 0 ||
                              io.OrderId == dks021 && io.ReproductionType == 0 ||
                              io.OrderId == dks022 && io.ReproductionType == 0 ||
                              io.OrderId == dks024 && io.ReproductionType == 0 ||
                              // Medium Kitchens
                              io.OrderId == dkm002 && io.ReproductionType == 0 ||
                              io.OrderId == dkm003 && io.ReproductionType == 0 ||
                              io.OrderId == dkm007 && io.ReproductionType == 0 ||
                              io.OrderId == dkm010 && io.ReproductionType == 0 ||
                              io.OrderId == dkm011 && io.ReproductionType == 0 ||
                              io.OrderId == dkm013 && io.ReproductionType == 0 ||
                              io.OrderId == dkm015 && io.ReproductionType == 0 ||
                              io.OrderId == dkm017 && io.ReproductionType == 0 ||
                              io.OrderId == dkm019 && io.ReproductionType == 0 ||
                              io.OrderId == dkm020 && io.ReproductionType == 0 ||
                              io.OrderId == dkm022 && io.ReproductionType == 0 ||
                              io.OrderId == dkm023 && io.ReproductionType == 0 ||
                              io.OrderId == dkm025 && io.ReproductionType == 0 );
                              
                if(importOrders != null)
                {
                    foreach(var importOrder in importOrders)
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
            _Logger.Error(ResourcesKeys.ErrorInUserExit("UpdateTransferStateKitchensWithoutBulksAndFeedback"), null, e);
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
