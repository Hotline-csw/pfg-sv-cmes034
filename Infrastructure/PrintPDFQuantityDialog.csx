#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   PrintPDFQuantityDialog
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         <Author>
//   Date:           2022-11-22
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   <Author>        2022-11-22    Created
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;
using System.Collections;


// TODO MESSAGING: activate, if messaging-functionality is needed (eg. navigate to a tile-view and execute an ad-hoc filter)
// TODO REFRESH: activate, if refreshing a tile-view is needed



[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IClientCustomization))]
[Export("PrintPDFQuantityDialog", typeof(HomagGroup.FLS.Infrastructure.Framework.Contracts.ICommandUserExit))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("Druckdialog mit Mengeneingabe öffnen")]
[EnabledScript(true)]
public class PrintPDFQuantityDialog : UserExitCustomBase, HomagGroup.FLS.Infrastructure.Framework.Contracts.ICommandUserExit//, IPartImportsSatisfiedNotification // TODO MESSAGING
{
    [Import("PrintPDFQuantityInputViewModel")]
    HomagGroup.FLS.Infrastructure.Framework.Contracts.IDialogViewModel printPDFQuantityInputViewModel;
    
    [Import]
    private LooseXaml _LooseXaml;

    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;

    private Logger _Logger;

    public void Execute(object parameter)
    {
        Guard.ThrowOnArgumentNull(parameter, "parameter");

        try
        {
            _Logger = LogHelper.GetLogger(Name, typeof(PrintPDFQuantityDialog));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, "");

            var itemEnumerable = parameter as IEnumerable;

            if (itemEnumerable != null)
            {
                var viewReportBindings = itemEnumerable.OfType<CustViewReportBinding>();
                    
                if(viewReportBindings.Any())
                {
                    // Button konfigurieren
                    (printPDFQuantityInputViewModel as PrintPDFQuantityInputViewModel).PrintOnDefaultPrinterButtonVisible = true;
                    (printPDFQuantityInputViewModel as PrintPDFQuantityInputViewModel).OpenPrinterDialogButtonVisible = true;
                    (printPDFQuantityInputViewModel as PrintPDFQuantityInputViewModel).OkButtonVisible = false;
                
                    if (viewReportBindings.FirstOrDefault().ReportLayout == "FittingSeriesLabel")
                    {
                        (printPDFQuantityInputViewModel as PrintPDFQuantityInputViewModel).LabelText = "Anzahl Etiketten:";
                        //(printPDFQuantityInputViewModel as PrintPDFQuantityInputViewModel).PrintQuantity = viewReportBindings.FirstOrDefault().ReportField01.ToInt32();
                    }
                    else
                    {
                        (printPDFQuantityInputViewModel as PrintPDFQuantityInputViewModel).PrintQuantity = 1;
                    }
                    (printPDFQuantityInputViewModel as PrintPDFQuantityInputViewModel).ViewReportBinding = viewReportBindings;
                    
                    var result = _LooseXaml.ShowDialog("PrintPDFQuantityInputView", printPDFQuantityInputViewModel);
                    if (result.HasValue && result == true)
                    {
                        _Logger.Info("PrintPDFQuantityInputViewModel");    
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
