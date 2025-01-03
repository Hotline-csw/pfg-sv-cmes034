#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   FeedbackSortingAndPicking
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
//   T.Stürzer       2025-01-03    Changed workcenter and added RefreshView
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;
using System.Collections;


// TODO MESSAGING: activate, if messaging-functionality is needed (eg. navigate to a tile-view and execute an ad-hoc filter)
// TODO REFRESH: activate, if refreshing a tile-view is needed



[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IClientCustomization))]
[Export("FeedbackSortingAndPicking", typeof(HomagGroup.FLS.Infrastructure.Framework.Contracts.ICommandUserExit))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("Manual feedback for sorting and picking workcenter")]
[EnabledScript(true)]
public class FeedbackSortingAndPicking : UserExitCustomBase, HomagGroup.FLS.Infrastructure.Framework.Contracts.ICommandUserExit//, IPartImportsSatisfiedNotification // TODO MESSAGING
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;

    private Logger _Logger;
    
    
    private string _TaskName = "FeedbackSortingAndPicking";
    

    public void Execute(object parameter)
    {
        Guard.ThrowOnArgumentNull(parameter, "parameter");

        try
        {
            _Logger = LogHelper.GetLogger(Name, typeof(FeedbackSortingAndPicking));
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
                            var ProductionItemsStepsData = ProductionItemsStepsDataRepository.GetFirstOrDefault(po => po.ProductionItemCode == selectedItem.ProductionItemCode && po.ProductionStepCode == "SORT");

                            if (ProductionItemsStepsData != null)
                            {
                                productionItem.InsertFeedbackFinishedGood( unitOfWork, _TaskName,"5070","",1, _Logger);
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
}
