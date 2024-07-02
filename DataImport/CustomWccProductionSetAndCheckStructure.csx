#r ControllerMES.Infrastructure.Common
#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   CustomWccProductionSetAndCheckStructure
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         T.Stürzer
//   Date:           2023-02-12
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   T.Stürzer       2023-02-12    Created
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IServerCustomization))]
[Export("CustomWccProductionSetAndCheckStructure", typeof(HomagGroup.FLS.Services.DataImport.Contracts.Contracts.UserExits.IDataImportBeforeSavePackageUserExit))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("[03] Set productionOrder.DesiredEndDate = customerOrder.CustomPlannedEndDate")]
[EnabledScript(true)]
public class CustomWccProductionSetAndCheckStructure : WccProductionSetAndCheckStructure
{
    private Logger _Logger;

    protected override void SetAndCheckDates(IEnumerable<TargetItem> targetItems, int additionalDays)
        {
            Guard.ThrowOnArgumentNull(targetItems, "targetItems");

            var values = targetItems.Select(t => t.Value).ToArray();
            var productionOrders = values.OfType<ProductionOrder>();

            DateTime[] freeDays;
            using (IUnitOfWork unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
            {
                // get all free days after today
                if (additionalDays > 0)
                {
                    DateTime dateTimeAfter = DateTime.Now.AddDays(-1);
                    freeDays = unitOfWork.GetRepository<FreeDay>().Get(a => a.Day >= dateTimeAfter).Select(d => d.Day).ToArray();
                }
                else
                {
                    freeDays = unitOfWork.GetRepository<FreeDay>().Get().Select(d => d.Day).ToArray();
                }

                foreach (ProductionOrder productionOrderInLoop in productionOrders)
                {
                    var productionOrder = productionOrderInLoop;

                    try
                    {
                        if (!productionOrder.DesiredEndDate.HasValue)
                        {
                            if (!productionOrder.DesiredStartDate.HasValue)
                            {
                                // so neither DesiredEndDate nor DesiredStartDate is set at ProductionOrder
                                // ==> consider Date-values out of CustomerOrder


                                // gets the CustomerOrder-Object for the given customerOrderCode;
                                //     looks first inside the ProductionOrders of targetItems, and if no one found second in database
                                //     (searching in database is necessary eg. at reproduction with only one ConstructionPart-ProductionOrder in targetItems) 
                                TargetItem[] targetItemsArray = targetItems.ToArray();
                                CustomerOrder customerOrder = targetItemsArray.GetCustomerOrderForProductionOrder(productionOrder.CustomerOrderCode,
                                                                                                    unitOfWork, Name, _Logger);
                                if (customerOrder != null)
                                {
                                    if (!customerOrder.ShippingDate.HasValue)
                                    {
                                        if (!customerOrder.DeliveryDate.HasValue)
                                        {
                                            productionOrder.DesiredEndDate = DateTime.Today;

                                            if (additionalDays >= 0)
                                            {
                                                productionOrder.DesiredEndDate = productionOrder.DesiredEndDate.Value
                                                    .AddWorkdays((uint)additionalDays, freeDays);
                                            }
                                            else
                                            {
                                                productionOrder.DesiredEndDate = productionOrder.DesiredEndDate.Value
                                                    .SubtractWorkdays((uint)Math.Abs(additionalDays), freeDays);
                                            }

                                            productionOrder.DesiredStartDate = customerOrder.ShippingDate;
                                        }
                                        else
                                        {
                                            productionOrder.DesiredEndDate = customerOrder.ShippingDate;
                                            productionOrder.DesiredStartDate = productionOrder.DesiredEndDate;
                                        }
                                    }
                                    else
                                    {
                                        // Changed customerOrder.ShippingDate to customerOrder.CustomPlannedEndDate
                                        productionOrder.DesiredEndDate = customerOrder.CustomPlannedEndDate;
                                        productionOrder.DesiredStartDate = productionOrder.DesiredEndDate;
                                    }
                                }
                                else
                                {
                                    productionOrder.DesiredStartDate = DateTime.Today;
                                    productionOrder.DesiredEndDate = customerOrder.ShippingDate;
                                }
                            }
                            else
                            {
                                productionOrder.DesiredEndDate = productionOrder.DesiredStartDate;
                            }
                        }
                        else
                        {
                            if (!productionOrder.DesiredStartDate.HasValue)
                            {
                                productionOrder.DesiredStartDate = productionOrder.DesiredEndDate;
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        _Logger.Error("Error", null, ex);
                        HandleError(productionOrder, ProcessingStateMode.NotReleased, ReleaseState.NotReleased,
                            ErrorState.ErrorOnExecuteUserExit, ResourcesKeys.ErrorInUserExit(Name).GetText(GetCultureInfoForPoErrorMessage()),
                            _ExecutionContext, _Logger);
                        throw;
                    }
                }
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