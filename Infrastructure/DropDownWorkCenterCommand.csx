//#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   DropDownWorkCenter
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         T.Stürzer
//   Date:           2025-02-09
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   T.Stürzer       2025-02-09    Created
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Collections;
using System.Globalization;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IClientCustomization))]
[Export("DropDownWorkCenterCommand", typeof(HomagGroup.FLS.Infrastructure.Framework.Contracts.ICommandUserExit))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("describe here")]
[EnabledScript(true)]
public class DropDownWorkCenterCommand : UserExitCustomBase, HomagGroup.FLS.Infrastructure.Framework.Contracts.ICommandUserExit
{
    private const string _ViewName = "DropDownWorkCenterView";
    private const string _ViewModelName = "DropDownWorkCenterViewModel";

    [Import]
    private LooseXaml _LooseXaml;

    [Import(_ViewModelName)]
    HomagGroup.FLS.Infrastructure.Framework.Contracts.IDialogViewModel dialogViewModel;

    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;

    private Logger _Logger;
    
    private string _TaskName = "DropDownWorkCenterCommand";
    

    public void Execute(object parameter)
    {
        Guard.ThrowOnArgumentNull(parameter, "parameter");

        try
        {
            _Logger = LogHelper.GetLogger(Name, typeof(DropDownWorkCenterCommand));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, "");

            var itemEnumerable = parameter as IEnumerable;

            if (itemEnumerable != null)
            {
                using (var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
                {
                    var selectedProdOrderFeedbacks = itemEnumerable.Cast<CustViewMasterManualFeedback>().ToArray();
                    
                    string selectedCustomerOrdersString = selectedProdOrderFeedbacks.Select(co => co.CustomerOrderCode)
                            .Where(x => x != null)
                                .Distinct()
                                    .OrderBy(x => x)
                                        .Aggregate((current, next) => current + ", " + next);
                            
                    string selectedBulksString = selectedProdOrderFeedbacks.Select(mb => mb.PlanningNumber)
                            .Where(x => x != null)
                                .Distinct()
                                    .OrderBy(x => x)
                                        .Aggregate((current, next) => current + ", " + next);
                            
                    string selectedOptimizationsString = selectedProdOrderFeedbacks.Select(op => op.OptimizationCode)
                            .Where(x => x != null)
                                .Distinct()
                                    .OrderBy(x => x)
                                        .Aggregate((current, next) => current + ", " + next);
                            
                    _Logger.Info(string.Format("{0}: CustomerOrders: [{1}]", _TaskName, selectedCustomerOrdersString));
                    _Logger.Info(string.Format("{0}: Bulks: [{1}]", _TaskName, selectedBulksString));
                    _Logger.Info(string.Format("{0}: Opti: [{1}]", _TaskName, selectedOptimizationsString));

                    var resultBox = System.Windows.MessageBoxResult.No;
                                    
                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        resultBox = HomagGroup.Base.UI.Windows.MessageBox.Show(
                                    "CustomerOrders:  " + selectedCustomerOrdersString
                                    + System.Environment.NewLine
                                    + "Bulks:  " + selectedBulksString
                                    + System.Environment.NewLine
                                    + "Optimizations: " + selectedOptimizationsString
                                    + System.Environment.NewLine
                                    ,"Feedback?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    });                   
                    
                    
                    string dropDownWorkCenter = "";
                    
                    (dialogViewModel as DropDownWorkCenterViewModel).DropDownWorkCenter = dropDownWorkCenter;
                    
                    bool? result = _LooseXaml.ShowDialog(_ViewName, dialogViewModel);
                    
                    dropDownWorkCenter = (dialogViewModel as DropDownWorkCenterViewModel).DropDownWorkCenter;
                    
                    _Logger.Info(string.Format("{0}: WorkCenter: [{1}]", _TaskName, dropDownWorkCenter));
/*                    
                    foreach (var selectedItem in selectedProdOrderFeedbacks)
                    {
                        var prodItem = unitOfWork.GetRepository<ProductionItem>().GetFirstOrDefault(
                                pi => pi.Code == selectedItem.ProductionItemCode);
                                
                        var prodStep = unitOfWork.GetRepository<ProductionStep>().GetFirstOrDefault(
                                ps => 
                                    ps.WorkCenterCode == dropDownWorkCenter &&
                                    ps.ProductionOrderCode == prodItem.ProductionOrderCode);

                        if (prodItem != null && prodStep != null)
                        {
                            var prodItemsStepsData = unitOfWork.GetRepository<ProductionItemsStepsData>().GetFirstOrDefault(
                                    po => 
                                        po.ProductionItemCode == prodItem.Code && 
                                        po.ProductionStepCode == prodStep.Code);

                            if (prodItemsStepsData != null)
                            {
                                prodItem.InsertFeedback(unitOfWork, _TaskName, 1, 0, 0, dropDownWorkCenter, prodStep.Code, 0, FeedbackState.Finished, 0, _Logger);
                            } 
                        }
                    }
                    
                    //UserExitHelper.RefreshView();
*/
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