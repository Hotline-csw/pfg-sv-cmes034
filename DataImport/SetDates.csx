#r ControllerMES.Infrastructure.Common
#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   SetDates
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
[Export("SetDates", typeof(HomagGroup.FLS.Services.DataImport.Contracts.Contracts.UserExits.IDataImportBeforeSavePackageUserExit))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("[03] Set planned end date, shipping date and delivery date")]
[EnabledScript(true)]
public class SetDates : UserExitCustomBase, HomagGroup.FLS.Services.DataImport.Contracts.Contracts.UserExits.IDataImportBeforeSavePackageUserExit
{
    private Logger _Logger;
    
    private string _TaskName = "SetDates";
    
    [Import]
    protected IHelperMethodsCommon _HelperMethodsCommon;
    
    [Import]
    protected IUnitOfWorkFactory _UnitOfWorkFactory;
    
    // Small Kitchens
    private string dks001 = "Demo_Kitchen_Small_001";
    private string dks002 = "Demo_Kitchen_Small_002";
    private string dks003 = "Demo_Kitchen_Small_003";
    private string dks004 = "Demo_Kitchen_Small_004";
    private string dks005 = "Demo_Kitchen_Small_005";
    private string dks006 = "Demo_Kitchen_Small_006";
    private string dks007 = "Demo_Kitchen_Small_007";
    private string dks008 = "Demo_Kitchen_Small_008";
    private string dks009 = "Demo_Kitchen_Small_009";
    private string dks010 = "Demo_Kitchen_Small_010";
    private string dks011 = "Demo_Kitchen_Small_011";
    private string dks012 = "Demo_Kitchen_Small_012";
    private string dks013 = "Demo_Kitchen_Small_013";
    private string dks014 = "Demo_Kitchen_Small_014";
    private string dks015 = "Demo_Kitchen_Small_015";
    private string dks016 = "Demo_Kitchen_Small_016";
    private string dks017 = "Demo_Kitchen_Small_017";
    private string dks018 = "Demo_Kitchen_Small_018";
    private string dks019 = "Demo_Kitchen_Small_019";
    private string dks020 = "Demo_Kitchen_Small_020";
    private string dks021 = "Demo_Kitchen_Small_021";
    private string dks022 = "Demo_Kitchen_Small_022";
    private string dks023 = "Demo_Kitchen_Small_023";
    private string dks024 = "Demo_Kitchen_Small_024";
    private string dks025 = "Demo_Kitchen_Small_025";
    
    // Medium Kitchens
    private string dkm001 = "Demo_Kitchen_Medium_001";
    private string dkm002 = "Demo_Kitchen_Medium_002";
    private string dkm003 = "Demo_Kitchen_Medium_003";
    private string dkm004 = "Demo_Kitchen_Medium_004";
    private string dkm005 = "Demo_Kitchen_Medium_005";
    private string dkm006 = "Demo_Kitchen_Medium_006";
    private string dkm007 = "Demo_Kitchen_Medium_007";
    private string dkm008 = "Demo_Kitchen_Medium_008";
    private string dkm009 = "Demo_Kitchen_Medium_009";
    private string dkm010 = "Demo_Kitchen_Medium_010";
    private string dkm011 = "Demo_Kitchen_Medium_011";
    private string dkm012 = "Demo_Kitchen_Medium_012";
    private string dkm013 = "Demo_Kitchen_Medium_013";
    private string dkm014 = "Demo_Kitchen_Medium_014";
    private string dkm015 = "Demo_Kitchen_Medium_015";
    private string dkm016 = "Demo_Kitchen_Medium_016";
    private string dkm017 = "Demo_Kitchen_Medium_017";
    private string dkm018 = "Demo_Kitchen_Medium_018";
    private string dkm019 = "Demo_Kitchen_Medium_019";
    private string dkm020 = "Demo_Kitchen_Medium_020";
    private string dkm021 = "Demo_Kitchen_Medium_021";
    private string dkm022 = "Demo_Kitchen_Medium_022";
    private string dkm023 = "Demo_Kitchen_Medium_023";
    private string dkm024 = "Demo_Kitchen_Medium_024";
    private string dkm025 = "Demo_Kitchen_Medium_025";


    public void Execute(IJobExecutionContext executionContext, TargetItem[] targetItems)
    {
        Guard.ThrowOnArgumentNull(executionContext, "executionContext");
        Guard.ThrowOnArgumentNull(targetItems, "targetItems");

        try
        {
            var values = targetItems.Select(t => t.Value).ToArray();
            var productionOrders = values.OfType<ProductionOrder>();
            
            using (IUnitOfWork unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
            {               
                foreach (ProductionOrder prodOrder in productionOrders)
                {   
                    //Day 1
                    var plannedEndDateOne = DateTime.Today.AddDays(8);
                    var shipDateOne = plannedEndDateOne.AddDays(2);
                    var deliveryDateOne = shipDateOne.AddDays(4);
                    
                    //PlannedEndDate Day 1
                    if(plannedEndDateOne.DayOfWeek == DayOfWeek.Saturday)
                    {
                        plannedEndDateOne = plannedEndDateOne.AddDays(2);
                    }
                    if(plannedEndDateOne.DayOfWeek == DayOfWeek.Sunday)
                    {
                        plannedEndDateOne = plannedEndDateOne.AddDays(1);
                    }                   
                    
                    //ShippingDate Day 1
                    if(shipDateOne.DayOfWeek == DayOfWeek.Saturday)
                    {
                        shipDateOne = shipDateOne.AddDays(2);
                    }
                    if(shipDateOne.DayOfWeek == DayOfWeek.Sunday)
                    {
                        shipDateOne = shipDateOne.AddDays(1);
                    }
                    
                    //DeliveryDate Day 1
                    if(deliveryDateOne.DayOfWeek == DayOfWeek.Saturday)
                    {
                        deliveryDateOne = deliveryDateOne.AddDays(2);
                    }
                    if(deliveryDateOne.DayOfWeek == DayOfWeek.Sunday)
                    {
                        deliveryDateOne = deliveryDateOne.AddDays(1);
                    }
                    
                    
                    //Day 2
                    var plannedEndDateTwo = plannedEndDateOne.AddDays(1);
                    var shipDateTwo = shipDateOne.AddDays(1);
                    var deliveryDateTwo = deliveryDateOne.AddDays(1);
                    
                    //PlannedEndDate Day 2
                    if(plannedEndDateTwo.DayOfWeek == DayOfWeek.Saturday)
                    {
                        plannedEndDateTwo = plannedEndDateTwo.AddDays(2);
                    }
                    if(plannedEndDateTwo.DayOfWeek == DayOfWeek.Sunday)
                    {
                        plannedEndDateTwo = plannedEndDateTwo.AddDays(1);
                    }
                    
                    //ShippingDate Day 2
                    if(shipDateTwo.DayOfWeek == DayOfWeek.Saturday)
                    {
                        shipDateTwo = shipDateTwo.AddDays(2);
                    }
                    if(shipDateTwo.DayOfWeek == DayOfWeek.Sunday)
                    {
                        shipDateTwo = shipDateTwo.AddDays(1);
                    }
                    
                    //DeliveryDate Day 2
                    if(deliveryDateTwo.DayOfWeek == DayOfWeek.Saturday)
                    {
                        deliveryDateTwo = deliveryDateTwo.AddDays(2);
                    }
                    if(deliveryDateTwo.DayOfWeek == DayOfWeek.Sunday)
                    {
                        deliveryDateTwo = deliveryDateTwo.AddDays(1);
                    }
                    
                    
                    //Day 3
                    var plannedEndDateThree = plannedEndDateTwo.AddDays(1);
                    var shipDateThree = shipDateTwo.AddDays(1);
                    var deliveryDateThree = deliveryDateTwo.AddDays(1);
                    
                    //PlannedEndDate Day 3
                    if(plannedEndDateThree.DayOfWeek == DayOfWeek.Saturday)
                    {
                        plannedEndDateThree = plannedEndDateThree.AddDays(2);
                    }
                    if(plannedEndDateThree.DayOfWeek == DayOfWeek.Sunday)
                    {
                        plannedEndDateThree = plannedEndDateThree.AddDays(1);
                    }
                    
                    //ShippingDate Day 3
                    if(shipDateThree.DayOfWeek == DayOfWeek.Saturday)
                    {
                        shipDateThree = shipDateThree.AddDays(2);
                    }
                    if(shipDateThree.DayOfWeek == DayOfWeek.Sunday)
                    {
                        shipDateThree = shipDateThree.AddDays(1);
                    }
                    
                    //DeliveryDate Day 3
                    if(deliveryDateThree.DayOfWeek == DayOfWeek.Saturday)
                    {
                        deliveryDateThree = deliveryDateThree.AddDays(2);
                    }
                    if(deliveryDateThree.DayOfWeek == DayOfWeek.Sunday)
                    {
                        deliveryDateThree = deliveryDateThree.AddDays(1);
                    }


                    //Day 4
                    var plannedEndDateFour = plannedEndDateThree.AddDays(1);
                    var shipDateFour = shipDateThree.AddDays(1);
                    var deliveryDateFour = deliveryDateThree.AddDays(1);
                    
                    //PlannedEndDate Day 4
                    if(plannedEndDateFour.DayOfWeek == DayOfWeek.Saturday)
                    {
                        plannedEndDateFour = plannedEndDateFour.AddDays(2);
                    }
                    if(plannedEndDateFour.DayOfWeek == DayOfWeek.Sunday)
                    {
                        plannedEndDateFour = plannedEndDateFour.AddDays(1);
                    }
                                        
                    //ShippingDate Day 4
                    if(shipDateFour.DayOfWeek == DayOfWeek.Saturday)
                    {
                        shipDateFour = shipDateFour.AddDays(2);
                    }
                    if(shipDateFour.DayOfWeek == DayOfWeek.Sunday)
                    {
                        shipDateFour = shipDateFour.AddDays(1);
                    }
                    
                    //DeliveryDate Day 4
                    if(deliveryDateFour.DayOfWeek == DayOfWeek.Saturday)
                    {
                        deliveryDateFour = deliveryDateFour.AddDays(2);
                    }
                    if(deliveryDateFour.DayOfWeek == DayOfWeek.Sunday)
                    {
                        deliveryDateFour = deliveryDateFour.AddDays(1);
                    }

                    
                    //Day 5
                    var plannedEndDateFive = plannedEndDateFour.AddDays(1);
                    var shipDateFive = shipDateFour.AddDays(1);
                    var deliveryDateFive = deliveryDateFour.AddDays(1);
                    
                    //PlannedEndDate Day 5
                    if(plannedEndDateFive.DayOfWeek == DayOfWeek.Saturday)
                    {
                        plannedEndDateFive = plannedEndDateFive.AddDays(2);
                    }
                    if(plannedEndDateFive.DayOfWeek == DayOfWeek.Sunday)
                    {
                        plannedEndDateFive = plannedEndDateFive.AddDays(1);
                    }
                    
                    //ShippingDate Day 5
                    if(shipDateFive.DayOfWeek == DayOfWeek.Saturday)
                    {
                        shipDateFive = shipDateFive.AddDays(2);
                    }
                    if(shipDateFive.DayOfWeek == DayOfWeek.Sunday)
                    {
                        shipDateFive = shipDateFive.AddDays(1);
                    }
                    
                    //DeliveryDate Day 5
                    if(deliveryDateFive.DayOfWeek == DayOfWeek.Saturday)
                    {
                        deliveryDateFive = deliveryDateFive.AddDays(2);
                    }
                    if(deliveryDateFive.DayOfWeek == DayOfWeek.Sunday)
                    {
                        deliveryDateFive = deliveryDateFive.AddDays(1);
                    }


                    //Day 6
                    var plannedEndDateSix = plannedEndDateFive.AddDays(1);
                    var shipDateSix = shipDateFive.AddDays(1);
                    var deliveryDateSix = deliveryDateFive.AddDays(1);
                    
                    //PlannedEndDate Day 6
                    if(plannedEndDateSix.DayOfWeek == DayOfWeek.Saturday)
                    {
                        plannedEndDateSix = plannedEndDateSix.AddDays(2);
                    }
                    if(plannedEndDateSix.DayOfWeek == DayOfWeek.Sunday)
                    {
                        plannedEndDateSix = plannedEndDateSix.AddDays(1);
                    }
                    
                    //ShippingDate Day 6
                    if(shipDateSix.DayOfWeek == DayOfWeek.Saturday)
                    {
                        shipDateSix = shipDateSix.AddDays(2);
                    }
                    if(shipDateSix.DayOfWeek == DayOfWeek.Sunday)
                    {
                        shipDateSix = shipDateSix.AddDays(1);
                    }
                    
                    //DeliveryDate Day 6
                    if(deliveryDateSix.DayOfWeek == DayOfWeek.Saturday)
                    {
                        deliveryDateSix = deliveryDateSix.AddDays(2);
                    }
                    if(deliveryDateSix.DayOfWeek == DayOfWeek.Sunday)
                    {
                        deliveryDateSix = deliveryDateSix.AddDays(1);
                    }


                    //Day 7
                    var plannedEndDateSeven = plannedEndDateSix.AddDays(1);
                    var shipDateSeven = shipDateSix.AddDays(1);
                    var deliveryDateSeven = deliveryDateSix.AddDays(1);
                    
                    //PlannedEndDate Day 7
                    if(plannedEndDateSeven.DayOfWeek == DayOfWeek.Saturday)
                    {
                        plannedEndDateSeven = plannedEndDateSeven.AddDays(2);
                    }
                    if(plannedEndDateSeven.DayOfWeek == DayOfWeek.Sunday)
                    {
                        plannedEndDateSeven = plannedEndDateSeven.AddDays(1);
                    }
                    
                    //ShippingDate Day 7
                    if(shipDateSeven.DayOfWeek == DayOfWeek.Saturday)
                    {
                        shipDateSeven = shipDateSeven.AddDays(2);
                    }
                    if(shipDateSeven.DayOfWeek == DayOfWeek.Sunday)
                    {
                        shipDateSeven = shipDateSeven.AddDays(1);
                    }
                    
                    //DeliveryDate Day 7
                    if(deliveryDateSeven.DayOfWeek == DayOfWeek.Saturday)
                    {
                        deliveryDateSeven = deliveryDateSeven.AddDays(2);
                    }
                    if(deliveryDateSeven.DayOfWeek == DayOfWeek.Sunday)
                    {
                        deliveryDateSeven = deliveryDateSeven.AddDays(1);
                    }


                    //Day 8
                    var plannedEndDateEight = plannedEndDateSeven.AddDays(1);
                    var shipDateEight = shipDateSeven.AddDays(1);
                    var deliveryDateEight = deliveryDateSeven.AddDays(1);
                    
                    //PlannedEndDate Day 8
                    if(plannedEndDateEight.DayOfWeek == DayOfWeek.Saturday)
                    {
                        plannedEndDateEight = plannedEndDateEight.AddDays(2);
                    }
                    if(plannedEndDateEight.DayOfWeek == DayOfWeek.Sunday)
                    {
                        plannedEndDateEight = plannedEndDateEight.AddDays(1);
                    }
                    
                    //ShippingDate Day 8
                    if(shipDateEight.DayOfWeek == DayOfWeek.Saturday)
                    {
                        shipDateEight = shipDateEight.AddDays(2);
                    }
                    if(shipDateEight.DayOfWeek == DayOfWeek.Sunday)
                    {
                        shipDateEight = shipDateEight.AddDays(1);
                    }
                    
                    //DeliveryDate Day 8
                    if(deliveryDateEight.DayOfWeek == DayOfWeek.Saturday)
                    {
                        deliveryDateEight = deliveryDateEight.AddDays(2);
                    }
                    if(deliveryDateEight.DayOfWeek == DayOfWeek.Sunday)
                    {
                        deliveryDateEight = deliveryDateEight.AddDays(1);
                    }


                    TargetItem[] targetItemsArray = targetItems.ToArray();
                    CustomerOrder custOrder = targetItemsArray.GetCustomerOrderForProductionOrder(prodOrder.CustomerOrderCode, unitOfWork, Name, _Logger);
                    
                    //Day 1
                    if(prodOrder.CustomerOrderCode == dks003 || prodOrder.CustomerOrderCode == dks004 || //Blue
                       prodOrder.CustomerOrderCode == dks005 || prodOrder.CustomerOrderCode == dkm001 ||
                       
                       prodOrder.CustomerOrderCode == dks006 || prodOrder.CustomerOrderCode == dkm006 || //Red
                       prodOrder.CustomerOrderCode == dkm008 || prodOrder.CustomerOrderCode == dkm009)
                       
                    {
                        custOrder.CustomPlannedEndDate = plannedEndDateOne;
                        custOrder.ShippingDate = shipDateOne;
                        custOrder.DeliveryDate = deliveryDateOne;
                    }
                    
                    //Day 2
                    if(prodOrder.CustomerOrderCode == dks011 || prodOrder.CustomerOrderCode == dks014 || //Green
                       prodOrder.CustomerOrderCode == dkm012 || prodOrder.CustomerOrderCode == dkm014 ||
                       
                       prodOrder.CustomerOrderCode == dkm016 || prodOrder.CustomerOrderCode == dkm018)   //Yellow
                    {
                        custOrder.CustomPlannedEndDate = plannedEndDateTwo;
                        custOrder.ShippingDate = shipDateTwo;
                        custOrder.DeliveryDate = deliveryDateTwo;
                    }
                    
                    //Day 3
                    if(prodOrder.CustomerOrderCode == dkm004 || prodOrder.CustomerOrderCode == dkm005 || //Blue
                    
                       prodOrder.CustomerOrderCode == dks007 || prodOrder.CustomerOrderCode == dks008 || //Red
                       prodOrder.CustomerOrderCode == dkm007 ||
                       
                       prodOrder.CustomerOrderCode == dks025 || prodOrder.CustomerOrderCode == dkm021)   //Black
                    {
                        custOrder.CustomPlannedEndDate = plannedEndDateThree;
                        custOrder.ShippingDate = shipDateThree;
                        custOrder.DeliveryDate = deliveryDateThree;
                    }
                    
                    //Day 4
                    if(prodOrder.CustomerOrderCode == dks002 ||                                          //Blue
                    
                       prodOrder.CustomerOrderCode == dks012 || prodOrder.CustomerOrderCode == dks015 || //Green
                       prodOrder.CustomerOrderCode == dkm011 || prodOrder.CustomerOrderCode == dkm015 ||
                       
                       prodOrder.CustomerOrderCode == dks017 || prodOrder.CustomerOrderCode == dks018 || //Yellow
                       prodOrder.CustomerOrderCode == dks019 || prodOrder.CustomerOrderCode == dkm019 ||
                       
                       prodOrder.CustomerOrderCode == dks023 || prodOrder.CustomerOrderCode == dkm024)   //Black
                    {
                        custOrder.CustomPlannedEndDate = plannedEndDateFour;
                        custOrder.ShippingDate = shipDateFour;
                        custOrder.DeliveryDate = deliveryDateFour;
                    }
                    
                    //Day 5
                    if(prodOrder.CustomerOrderCode == dks009 || prodOrder.CustomerOrderCode == dks010 || //Red
                       prodOrder.CustomerOrderCode == dkm010 || 
                       
                       prodOrder.CustomerOrderCode == dks022 || prodOrder.CustomerOrderCode == dks024 || //Black
                       prodOrder.CustomerOrderCode == dkm022)
                    {
                        custOrder.CustomPlannedEndDate = plannedEndDateFive;
                        custOrder.ShippingDate = shipDateFive;
                        custOrder.DeliveryDate = deliveryDateFive;
                    }
                    
                    //Day 6
                    if(prodOrder.CustomerOrderCode == dks001 || prodOrder.CustomerOrderCode == dkm002 || //Blue
                       prodOrder.CustomerOrderCode == dkm003 || 
                       
                       prodOrder.CustomerOrderCode == dks016 || prodOrder.CustomerOrderCode == dks020 || //Yellow
                       prodOrder.CustomerOrderCode == dkm017 || prodOrder.CustomerOrderCode == dkm020)
                    {
                        custOrder.CustomPlannedEndDate = plannedEndDateSix;
                        custOrder.ShippingDate = shipDateSix;
                        custOrder.DeliveryDate = deliveryDateSix;
                    }
                    
                    //Day 7
                    if(prodOrder.CustomerOrderCode == dks021 || prodOrder.CustomerOrderCode == dkm023 || //Black
                       prodOrder.CustomerOrderCode == dkm025)
                    {   
                        custOrder.CustomPlannedEndDate = plannedEndDateSeven;                    
                        custOrder.ShippingDate = shipDateSeven;
                        custOrder.DeliveryDate = deliveryDateSeven;
                    }
                    
                    //Day 8
                    if(prodOrder.CustomerOrderCode == dks013 || prodOrder.CustomerOrderCode == dkm013)   //Green
                    {
                        custOrder.CustomPlannedEndDate = plannedEndDateEight;
                        custOrder.ShippingDate = shipDateEight;
                        custOrder.DeliveryDate = deliveryDateEight;
                    }                   
                }
            }      
        }
        catch (Exception e)
        {
            executionContext.JobResultSetComplete(JobResult.Failed);
            _Logger.Error(ResourcesKeys.ErrorInUserExit(Name), null, e);
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
