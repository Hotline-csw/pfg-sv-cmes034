#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   FeedbackCNC
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
[Export("FeedbackCNC", typeof(HomagGroup.FLS.Infrastructure.Framework.Contracts.ICommandUserExit))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("Manual feedback for cnc workcenters")]
[EnabledScript(true)]
public class FeedbackCNC : UserExitCustomBase, HomagGroup.FLS.Infrastructure.Framework.Contracts.ICommandUserExit//, IPartImportsSatisfiedNotification // TODO MESSAGING
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;

    private Logger _Logger;
    
    [Import]
    protected UserExitHelper UserExitHelper { get; set; }
    
    private string _TaskName = "FeedbackCNC";
    
    private string cnc1WorkCenter = "CNC1";
    private string cnc2WorkCenter = "CNC2";
    
    private string cnc1StepCode = "CNC1";
    private string cnc2StepCode = "CNC2";
    

    public void Execute(object parameter)
    {
        Guard.ThrowOnArgumentNull(parameter, "parameter");

        try
        {
            _Logger = LogHelper.GetLogger(Name, typeof(FeedbackCNC));
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
                            var productionItemsStepsDataCnc1 = unitOfWork.GetRepository<ProductionItemsStepsData>().GetFirstOrDefault(
                                    po => 
                                        po.ProductionItemCode == selectedItem.ProductionItemCode && 
                                        po.ProductionStepCode == cnc1StepCode);
                                        
                            var productionItemsStepsDataCnc2 = unitOfWork.GetRepository<ProductionItemsStepsData>().GetFirstOrDefault(
                                    po => 
                                        po.ProductionItemCode == selectedItem.ProductionItemCode && 
                                        po.ProductionStepCode == cnc2StepCode);
                                        
                            if (productionItemsStepsDataCnc1 != null)
                            {
                                productionItem.InsertFeedbackFinishedGood(unitOfWork, _TaskName, cnc1WorkCenter, "", 1, _Logger);
                            }
                            
                            if(productionItemsStepsDataCnc2 != null)
                            {
                                productionItem.InsertFeedbackFinishedGood(unitOfWork, _TaskName, cnc2WorkCenter, "", 1, _Logger);
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
