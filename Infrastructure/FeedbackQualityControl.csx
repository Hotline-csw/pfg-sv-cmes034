#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   FeedbackQualityControl
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
[Export("FeedbackQualityControl", typeof(HomagGroup.FLS.Infrastructure.Framework.Contracts.ICommandUserExit))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("[06] Manual feedback for quality control workcenter")]
[EnabledScript(true)]
public class FeedbackQualityControl : UserExitCustomBase, HomagGroup.FLS.Infrastructure.Framework.Contracts.ICommandUserExit//, IPartImportsSatisfiedNotification // TODO MESSAGING
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;

    private Logger _Logger;
    
    [Import]
    protected UserExitHelper UserExitHelper { get; set; }
    
    private string _TaskName = "FeedbackQualityControl";
    
    private string qualityControlWorkCenter = "QC";
    private string qualityControlStepCode = "QC";

    public void Execute(object parameter)
    {
        Guard.ThrowOnArgumentNull(parameter, "parameter");

        try
        {
            _Logger = LogHelper.GetLogger(Name, typeof(FeedbackQualityControl));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, "");

            var itemEnumerable = parameter as IEnumerable;

            if (itemEnumerable != null)
            {
                using (var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
                {
                    foreach (var selectedItem in itemEnumerable.OfType<CustViewMasterManualFeedback>())
                    {
                        var productionItem = unitOfWork.GetRepository<ProductionItem>().GetFirstOrDefault(
                                pi => pi.Code == selectedItem.ProductionItemCode);

                        if (productionItem != null)
                        {
                            var productionItemsStepsData = unitOfWork.GetRepository<ProductionItemsStepsData>().GetFirstOrDefault(
                                    po => 
                                        po.ProductionItemCode == selectedItem.ProductionItemCode && 
                                        po.ProductionStepCode == qualityControlStepCode);

                            if (productionItemsStepsData != null)
                            {
                                productionItem.InsertFeedbackFinishedGood(unitOfWork, _TaskName, qualityControlWorkCenter, "", 1, _Logger);
                            } 
                        }
                    }
                    
                    UserExitHelper.RefreshView();
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
