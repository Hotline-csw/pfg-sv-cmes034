#r ControllerMES.Infrastructure.Common
#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   DeleteCustomerOrders
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         T.Stürzer
//   Date:           2023-02-14
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   T.Stürzer       2023-02-14    Created
//   T.Stürzer       2023-03-23    Logging and error messages
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IServerCustomization))]
[Export("DeleteCustomerOrders", typeof(HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("[01] Delete all customer orders")]
[EnabledScript(true)]
public class DeleteCustomerOrders : GenericTaskBase, HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;

    private Logger _Logger;
    
    private string _TaskName = "DeleteCustomerOrders";


    public override void Execute(IJobExecutionContext executionContext)
    {
        Guard.ThrowOnArgumentNull(executionContext, "executionContext");

        try
        {
            _Logger = LogHelper.GetLogger(executionContext.Area, typeof(DeleteCustomerOrders));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);

            using(IUnitOfWork unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
            {
                var customerOrdersRep = unitOfWork.GetRepository<CustomerOrder>();
                var customerOrder = customerOrdersRep.GetQueryable(false);
                
                if(customerOrder.Any())
                {
                    unitOfWork.BulkDelete(customerOrder);
                    
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Deleted all customer orders successfully!", _TaskName)));
                }
                
                else
                {
                    _Logger.Info(ResourcesKeys.CommonMessage(string.Format("{0}: Error while deleting customer orders!", _TaskName)));
                }
                
                unitOfWork.Save();
            }
        }
        catch (Exception e)
        {
            executionContext.JobResultSetComplete(JobResult.Failed);
            _Logger.Error(ResourcesKeys.ErrorInUserExit("DeleteCustomerOrders"), null, e);
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
