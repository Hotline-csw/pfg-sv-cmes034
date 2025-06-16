#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   PrintPDFOnDefaultPrinter
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         <Author>
//   Date:           2022-06-07
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   <Author>        2022-06-07    Created
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;
using System.Collections;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IClientCustomization))]
[Export("PrintPDFOnDefaultPrinter", typeof(HomagGroup.FLS.Infrastructure.Framework.Contracts.ICommandUserExit))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("PrintPDF - Drucken auf Default-Drucker")]
[EnabledScript(true)]
public class PrintPDFOnDefaultPrinter : UserExitCustomBase, HomagGroup.FLS.Infrastructure.Framework.Contracts.ICommandUserExit
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;
    
    [Import]
    protected UserExitHelper  UserExitHelper {get;set;}
    
    [Import("CommonHelperMethods")]
    HomagGroup.FLS.Infrastructure.Common.Customization.IGlobalCustomization commonHelperMethods;
    
    [Import("PrintPDFMethods")]
    HomagGroup.FLS.Infrastructure.Common.Customization.IGlobalCustomization printPDFMethods;
    
    [Import("PrintPDFClientMethods")]
    HomagGroup.FLS.Infrastructure.Common.Customization.IClientCustomization printPDFClientMethods;

    [Import("InfoBallonMessage")]
    HomagGroup.FLS.Infrastructure.Common.Customization.IClientCustomization infoBallonMessage;
    
    private const bool _ShowInfoBallon = true;

    private Logger _Logger;

    public void Execute(object parameter)
    {
        Guard.ThrowOnArgumentNull(parameter, "parameter");

        try
        {
            _Logger = LogHelper.GetLogger(Name, typeof(PrintPDFOnDefaultPrinter));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, "");

            var itemEnumerable = parameter as IEnumerable;

            if (itemEnumerable != null)
            {
                var viewReportBindings = itemEnumerable.OfType<CustViewReportBinding>();
                    
                if(viewReportBindings.Any())
                {
                    (printPDFClientMethods as PrintPDFClientMethods).PrintOnDefaultPrinter(_Logger, viewReportBindings, 1);
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
