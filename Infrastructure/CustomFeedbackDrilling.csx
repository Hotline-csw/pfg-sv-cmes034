#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   CustomFeedbackDrilling
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
[Export("CustomFeedbackDrilling", typeof(HomagGroup.FLS.Infrastructure.Framework.Contracts.ICommandUserExit))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("Manual feedback for drilling workcenter")]
[EnabledScript(true)]
public class CustomFeedbackDrilling : UserExitCustomBase, HomagGroup.FLS.Infrastructure.Framework.Contracts.ICommandUserExit//, IPartImportsSatisfiedNotification // TODO MESSAGING
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
    
    private string _TaskName = "CustomFeedbackDrilling";
    

    public void Execute(object parameter)
    {
        Guard.ThrowOnArgumentNull(parameter, "parameter");

        try
        {
            _Logger = LogHelper.GetLogger(Name, typeof(CustomFeedbackDrilling));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, "");

            var itemEnumerable = parameter as IEnumerable;

            if (itemEnumerable != null)
            {
                using (var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
                {
                    foreach (var selectedItem in itemEnumerable.OfType<CustViewMasterManualFeedback>())
                    {
                        var productionItemsRepository = unitOfWork.GetRepository<ProductionItem>();
                        var productionItem = productionItemsRepository.GetFirstOrDefault(pi => pi.Code == selectedItem.ProductionItemCode);

                        if (productionItem != null)
                        {
                            var ProductionItemsStepsDataRepository = unitOfWork.GetRepository<ProductionItemsStepsData>();
                            var ProductionItemsStepsDataDrilling = ProductionItemsStepsDataRepository.GetFirstOrDefault(po => po.ProductionItemCode == selectedItem.ProductionItemCode && po.ProductionStepCode == "V200");
                            var ProductionItemsStepsDataCnc = ProductionItemsStepsDataRepository.GetFirstOrDefault(po2 => po2.ProductionItemCode == selectedItem.ProductionItemCode && po2.ProductionStepCode == "E310");
                            
                            if (ProductionItemsStepsDataDrilling != null)
                            {
                                productionItem.InsertFeedbackFinishedGood( unitOfWork, _TaskName,"5010","",1, _Logger);
                            }
                            
                            else if(ProductionItemsStepsDataCnc != null)
                            {
                                productionItem.InsertFeedbackFinishedGood( unitOfWork, _TaskName,"5020","",1, _Logger);
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
