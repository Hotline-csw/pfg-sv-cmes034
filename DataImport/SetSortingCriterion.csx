#r ControllerMES.Infrastructure.Common
#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   SetSortingCriterion
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         T.Stürzer
//   Date:           2023-02-20
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   T.Stürzer       2023-02-20    Created
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IServerCustomization))]
[Export("SetSortingCriterion", typeof(HomagGroup.FLS.Services.DataImport.Contracts.Contracts.UserExits.IDataImportBeforeSavePackageUserExit))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("[03] Divide parttypes in different sorting criterions")]
[EnabledScript(true)]
public class SetSortingCriterion : UserExitCustomBase, HomagGroup.FLS.Services.DataImport.Contracts.Contracts.UserExits.IDataImportBeforeSavePackageUserExit
{
    private Logger _Logger;
    
    private string _TaskName = "SetSortingCriterion";
    
    [Import]
    protected IUnitOfWorkFactory UnitOfWorkFactory { get; set; }
    
    [Import]
    protected IHelperMethodsCommon HelperMethodsCommon { get; set; }


    public void Execute(IJobExecutionContext executionContext, TargetItem[] targetItems)
    {
        Guard.ThrowOnArgumentNull(executionContext, "executionContext");
        Guard.ThrowOnArgumentNull(targetItems, "targetItems");

        try
        {
            _Logger = LogHelper.GetLogger(executionContext.Area, typeof(SetSortingCriterion));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);

            using (IUnitOfWork unitOfWork = UnitOfWorkFactory.CreateUnitOfWork())
            {

            // Gets a collection of instances of ProductionOrder (or ProductionOrdersResource.ProductionOrder) from the current TargetItems-array.
            ICollection<ProductionOrder> productionOrders = targetItems.GetProductionOrdersFromTargetItemsEqualInProcessForImport(Name, _Logger);

                foreach (TargetItem targetItem in targetItems)
                {
                    ProductionOrder productionOrder = targetItem.Value as ProductionOrder;
                    
                    if(productionOrder != null)
                    {
                        // Only ConstructionParts are allowed to be sorted
                        if(productionOrder.OrderType == ProductionOrderType.ConstructionPart)
                        {
                            // Front Parts
                            if(productionOrder.ComponentType == ComponentType.Panel ||
                               productionOrder.ComponentType == ComponentType.Door ||
                               productionOrder.ComponentType == ComponentType.DoorLeft ||
                               productionOrder.ComponentType == ComponentType.DoorRight)
                            {
                                productionOrder.CustomSortingCriterion = productionOrder.CustomerOrderCode + "_Front";
                            }
                            
                            // Carcase Parts
                            else if(productionOrder.ComponentType == ComponentType.SidePanel ||
                                    productionOrder.ComponentType == ComponentType.TopShelf ||
                                    productionOrder.ComponentType == ComponentType.BottomShelf ||
                                    productionOrder.ComponentType == ComponentType.BackPanel ||
                                    productionOrder.ComponentType == ComponentType.CenterPanel ||
                                    productionOrder.ComponentType == ComponentType.FixedShelf ||
                                    productionOrder.ComponentType == ComponentType.Partition ||
                                    productionOrder.ComponentType == ComponentType.Traverse ||
                                    productionOrder.ComponentType == ComponentType.AdjustableShelf ||
                                    productionOrder.ComponentType == ComponentType.Plinth ||
                                    productionOrder.ComponentType == ComponentType.WorkTop)
                            {
                                productionOrder.CustomSortingCriterion = productionOrder.CustomerOrderCode + "_Carcase";
                            }
                            
                            // Drawer Parts
                            else if(productionOrder.ComponentType == ComponentType.DrawerBack ||
                                    productionOrder.ComponentType == ComponentType.DrawerBottom||
                                    productionOrder.ComponentType == ComponentType.DrawerFront ||
                                    productionOrder.ComponentType == ComponentType.DrawerSide)
                            {
                                productionOrder.CustomSortingCriterion = productionOrder.CustomerOrderCode + "_Drawer";
                            }
                        
                            else
                            {
                            string errorMessage = string.Format("{0}: Aktueller Bauteiltyp des Fertigungsauftrags [{1}] ist nicht definiert [{2}]",_TaskName,productionOrder.Code,productionOrder.ComponentType);
                            
                            HelperMethodsCommon.HandleError(productionOrder, ProcessingStateMode.NotReleased,ReleaseState.NotReleased,ErrorState.ErrorOnExecuteUserExit,
                            errorMessage,executionContext,_TaskName,_Logger);                                
                            }
                        }
                    }    
                }
                unitOfWork.Save();
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
