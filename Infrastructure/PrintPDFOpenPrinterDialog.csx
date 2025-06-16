#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   PrintPDFOpenPrinterDialog
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         <Author>
//   Date:           2022-06-28
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   <Author>        2022-06-28    Created
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;
using System.Collections;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IClientCustomization))]
[Export("PrintPDFOpenPrinterDialog", typeof(HomagGroup.FLS.Infrastructure.Framework.Contracts.ICommandUserExit))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("Druckdialog öffnen")]
[EnabledScript(false)]
public class PrintPDFOpenPrinterDialog : UserExitCustomBase, HomagGroup.FLS.Infrastructure.Framework.Contracts.ICommandUserExit
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;

    [Import]
	protected UserExitHelper  UserExitHelper {get;set;}

    private Logger _Logger;
    
    [Import]
    private LooseXaml _LooseXaml;
    
    [Import("PrintPDFPrinterDialogViewModel")]
    HomagGroup.FLS.Infrastructure.Framework.Contracts.IDialogViewModel printPDFPrinterDialogViewModel;
        
    [Import("CommonHelperMethods")]
    HomagGroup.FLS.Infrastructure.Common.Customization.IGlobalCustomization commonHelperMethods;
    
    [Import("PrintPDFMethods")]
    HomagGroup.FLS.Infrastructure.Common.Customization.IGlobalCustomization printPDFMethods;
    
    [Import("PrintPDFClientMethods")]
    HomagGroup.FLS.Infrastructure.Common.Customization.IClientCustomization printPDFClientMethods;
    
    
    public void Execute(object parameter)
    {
        Guard.ThrowOnArgumentNull(parameter, "parameter");

        try
        {
            _Logger = LogHelper.GetLogger(Name, typeof(PrintPDFOpenPrinterDialog));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, "");

            var itemEnumerable = parameter as IEnumerable;

            if (itemEnumerable != null)
            {
                var viewReportBindings = itemEnumerable.OfType<CustViewReportBinding>();
                
                if(viewReportBindings.Any())
                {
                    (printPDFClientMethods as PrintPDFClientMethods).OpenPrinterDialog(_Logger, viewReportBindings, 1);
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
