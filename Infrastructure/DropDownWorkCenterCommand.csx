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
                    
                    int selectedSidePanels = selectedProdOrderFeedbacks.Where(po => po.ComponentType == ComponentType.SidePanel).Count();
                    int selectedAdjustableShelves = selectedProdOrderFeedbacks.Where(po => po.ComponentType == ComponentType.AdjustableShelf).Count();
                    int selectedTopShelves = selectedProdOrderFeedbacks.Where(po => po.ComponentType == ComponentType.TopShelf).Count();
                    int selectedBottomShelves = selectedProdOrderFeedbacks.Where(po => po.ComponentType == ComponentType.BottomShelf).Count();
                    int selectedBackPanels = selectedProdOrderFeedbacks.Where(po => po.ComponentType == ComponentType.BackPanel).Count();
                    int selectedFillers = selectedProdOrderFeedbacks.Where(po => po.ComponentType == ComponentType.Panel).Count();
                    int selectedFlaps = selectedProdOrderFeedbacks.Where(po => po.ComponentType == ComponentType.Door).Count();
                    int selectedDoorsLeft = selectedProdOrderFeedbacks.Where(po => po.ComponentType == ComponentType.DoorLeft).Count();
                    int selectedDoorsRight = selectedProdOrderFeedbacks.Where(po => po.ComponentType == ComponentType.DoorRight).Count();
                    int selectedFixedShelves = selectedProdOrderFeedbacks.Where(po => po.ComponentType == ComponentType.FixedShelf).Count();
                    int selectedPartitions = selectedProdOrderFeedbacks.Where(po => po.ComponentType == ComponentType.Partition).Count();
                    int selectedDrawerBottoms = selectedProdOrderFeedbacks.Where(po => po.ComponentType == ComponentType.DrawerBottom).Count();
                    int selectedDrawerSides = selectedProdOrderFeedbacks.Where(po => po.ComponentType == ComponentType.DrawerSide).Count();
                    int selectedDrawerFronts = selectedProdOrderFeedbacks.Where(po => po.ComponentType == ComponentType.DrawerFront).Count();
                    int selectedToekicks = selectedProdOrderFeedbacks.Where(po => po.ComponentType == ComponentType.Plinth).Count();
                    int selectedWorktops = selectedProdOrderFeedbacks.Where(po => po.ComponentType == ComponentType.WorkTop).Count();
                    int selectedTraverses = selectedProdOrderFeedbacks.Where(po => po.ComponentType == ComponentType.Traverse).Count();
                    int selectedTotal = selectedProdOrderFeedbacks.Count();


                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        HomagGroup.Base.UI.Windows.MessageBox.Show(
                                "Side Panels: " + selectedSidePanels
                                + System.Environment.NewLine
                                + "Adjustable Shelves: " + selectedAdjustableShelves
                                + System.Environment.NewLine
                                + "Top Shelves: " + selectedTopShelves
                                + System.Environment.NewLine
                                + "Bottom Shelves: " + selectedBottomShelves
                                + System.Environment.NewLine
                                + "Back Panels: " + selectedBackPanels
                                + System.Environment.NewLine
                                + "Fillers: " + selectedFillers
                                + System.Environment.NewLine
                                + "Flaps: " + selectedFlaps
                                + System.Environment.NewLine
                                + "Doors Left: " + selectedDoorsLeft
                                + System.Environment.NewLine
                                + "Doors Right: " + selectedDoorsRight
                                + System.Environment.NewLine
                                + "Fixed Shelves: " + selectedFixedShelves
                                + System.Environment.NewLine                                    
                                + "Partitions: " + selectedPartitions
                                + System.Environment.NewLine
                                + "Drawer Bottoms: " + selectedDrawerBottoms
                                + System.Environment.NewLine                                       
                                + "Drawer Sides: " + selectedDrawerSides
                                + System.Environment.NewLine                                      
                                + "Drawer Fronts: " + selectedDrawerFronts
                                + System.Environment.NewLine                                     
                                + "Toekicks: " + selectedToekicks
                                + System.Environment.NewLine                                       
                                + "Worktops: " + selectedWorktops
                                + System.Environment.NewLine                                      
                                + "Traverses: " + selectedTraverses
                                + System.Environment.NewLine
                                + System.Environment.NewLine
                                + "\bTotal: " + selectedTotal
                                + System.Environment.NewLine
                                ,"Selected Parts", MessageBoxButton.OK, MessageBoxImage.Information);
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