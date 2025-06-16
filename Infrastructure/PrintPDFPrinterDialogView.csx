#r "PresentationCore"
#r "PresentationFramework"
#r "WindowsBase"
#r "System.Xaml"
#r "System.Xml"


//-----------------------------------------------------------------------------
//   (Class-)Name:   PrintPDFPrinterDialogView
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


using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;


[Export("PrintPDFPrinterDialogView", typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IClientCustomization))]
[Export("PrintPDFPrinterDialogView", typeof(Dialog))]
[Export("PrintPDFPrinterDialogView", typeof(HomagGroup.FLS.Infrastructure.Framework.Contracts.IDialogView))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("DialogView (Load Xaml at Runtime)")]
[EnabledScript(true)]
public class PrintPDFPrinterDialogView : HomagGroup.Base.UI.Windows.Dialog, HomagGroup.FLS.Infrastructure.Framework.Contracts.IDialogView, IPartImportsSatisfiedNotification
{
    [Import]
    private LooseXaml _LooseXaml;

    public PrintPDFPrinterDialogView()
    {
    }

    public string Name
    {
        get
        {
            return "PrintPDFPrinterDialogView";
        }
    }

    public HomagGroup.FLS.Infrastructure.Framework.Contracts.IDialogViewModel ViewModel
    {
        get
        {
            return DataContext as IDialogViewModel;
        }
        set
        {
            DataContext = value;
        }
    }

    public void OnImportsSatisfied()
    {
        _LooseXaml.LoadXaml(this, "PrintPDFPrinterDialogView");
    }
}
