#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   CustomFeedbackSalesItem
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         T.Stürzer
//   Date:           2023-02-13
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   T.Stürzer       2023-02-13    Created
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;
using System.Collections;


// TODO MESSAGING: activate, if messaging-functionality is needed (eg. navigate to a tile-view and execute an ad-hoc filter)
// TODO REFRESH: activate, if refreshing a tile-view is needed



[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IClientCustomization))]
[Export("CustomFeedbackSalesItem", typeof(HomagGroup.FLS.Infrastructure.Framework.Contracts.ICommandUserExit))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("Manual feedback for sales items")]
[EnabledScript(true)]
public class CustomFeedbackSalesItem : UserExitCustomBase, HomagGroup.FLS.Infrastructure.Framework.Contracts.ICommandUserExit//, IPartImportsSatisfiedNotification // TODO MESSAGING
{
#pragma warning disable 0649
    // TODO REFRESH
    ///// <summary>
    ///// UserExitHelper
    ///// </summary>
    //[Import]
    //protected UserExitHelper UserExitHelper { get; set; }



    // TODO MESSAGING
    ///// <summary>
    ///// DistributedServiceProvider
    ///// </summary>
    //[Import]
    //protected IDistributedServiceProvider DistributedServiceProvider { get; set; }
#pragma warning restore 0649



    // TODO MESSAGING
    ///// <summary>
    ///// Messaging service interface.
    ///// </summary>
    //private IMessagingService _MessagingService;


    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;

    private Logger _Logger;

    public void Execute(object parameter)
    {
        Guard.ThrowOnArgumentNull(parameter, "parameter");

        try
        {
            _Logger = LogHelper.GetLogger(Name, typeof(CustomFeedbackSalesItem));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, "");

            var itemEnumerable = parameter as IEnumerable;

            if (itemEnumerable != null)
            {
                using (var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
                {
                    foreach (var selectedItem in itemEnumerable.OfType<ProductionOrder>())
                    {
                        var productionOrdersRepository = unitOfWork.GetRepository<ProductionOrder>();
                        var productionOrders = productionOrdersRepository.Get(po => po.TopProductionOrderNumber == selectedItem.Code);
                        
                        if (productionOrders != null)
                        {
                           foreach(var productionOrder in productionOrders)
                           {
                               var productionItemsRepository = unitOfWork.GetRepository<ProductionItem>();
                               var productionItems = productionItemsRepository.Get(pi => pi.ProductionOrderCode == productionOrder.Code);
                               
                                 if (productionItems != null)
                                 {
                                    foreach(var productionItem in productionItems)
                                    {
                                        var ProductionItemsStepsDataRepository = unitOfWork.GetRepository<ProductionItemsStepsData>();
                                        var ProductionItemsStepsDataPart = ProductionItemsStepsDataRepository.GetFirstOrDefault(po => po.ProductionOrderCode == productionItem.ProductionOrderCode && po.ProductionStepCode == "PREASSEM");
                                          
                                          if (ProductionItemsStepsDataPart != null)
                                          {
                                            productionItem.Feedback(ProductionItemsStepsDataPart.ProductionStepCode, unitOfWork, 1, 0, 0, DateTime.Now);
                                          } 
                                     }
                                     
                                    foreach(var productionItem in productionItems)
                                    {
                                        var ProductionItemsStepsDataRepository = unitOfWork.GetRepository<ProductionItemsStepsData>();
                                        var ProductionItemsStepsDataSalesItem = ProductionItemsStepsDataRepository.GetFirstOrDefault(po => po.ProductionOrderCode == productionItem.ProductionOrderCode && po.ProductionStepCode == "ASSEM");
                                          
                                          if (ProductionItemsStepsDataSalesItem != null)
                                          {
                                            productionItem.Feedback(ProductionItemsStepsDataSalesItem.ProductionStepCode, unitOfWork, 1, 0, 0, DateTime.Now);
                                          } 
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            _Logger.Error(ResourcesKeys.ErrorInUserExit(Name), null, e);
            throw;
        }
    }

    public bool CanExecute(object parameter)
    {
        IEnumerable itemEnumerable = parameter as IEnumerable;

        if (itemEnumerable != null)
        {
            return true;
        }

        return false;
    }



    // TODO MESSAGING
    ///// <summary>
    ///// Called when a part's imports have been satisfied and it is safe to use.
    ///// </summary>
    //public void OnImportsSatisfied()
    //{
    //    _MessagingService = DistributedServiceProvider.GetService<IMessagingService>();
    //}
}
