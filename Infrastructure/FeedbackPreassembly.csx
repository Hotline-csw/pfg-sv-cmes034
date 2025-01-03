#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   FeedbackPreassembly
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
[Export("FeedbackPreassembly", typeof(HomagGroup.FLS.Infrastructure.Framework.Contracts.ICommandUserExit))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("Manual feedback for preassembly workcenter")]
[EnabledScript(true)]
public class FeedbackPreassembly : UserExitCustomBase, HomagGroup.FLS.Infrastructure.Framework.Contracts.ICommandUserExit//, IPartImportsSatisfiedNotification // TODO MESSAGING
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;

    private Logger _Logger;
    
    private string _TaskName = "FeedbackPreassembly";
    
    
    public void Execute(object parameter)
    {
        Guard.ThrowOnArgumentNull(parameter, "parameter");

        try
        {
            _Logger = LogHelper.GetLogger(Name, typeof(FeedbackPreassembly));
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
                            var ProductionItemsStepsData = ProductionItemsStepsDataRepository.GetFirstOrDefault(po => po.ProductionItemCode == selectedItem.ProductionItemCode && po.ProductionStepCode == "PREASSEM");

                            if (ProductionItemsStepsData != null)
                            {
                                productionItem.InsertFeedbackFinishedGood( unitOfWork, _TaskName,"6010","",1, _Logger);
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
