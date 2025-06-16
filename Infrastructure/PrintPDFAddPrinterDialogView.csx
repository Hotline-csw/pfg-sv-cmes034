#r "PresentationCore"
#r "PresentationFramework"
#r "WindowsBase"
#r "System.Xaml"
#r "System.Xml"


//-----------------------------------------------------------------------------
//   (Class-)Name:   PrintPDFAddPrinterDialogView
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         <Author>
//   Date:           2022-06-29
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   <Author>        2022-06-29    Created
//   
//-----------------------------------------------------------------------------


using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;


[Export("PrintPDFAddPrinterDialogView", typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IClientCustomization))]
[Export("PrintPDFAddPrinterDialogView", typeof(Dialog))]
[Export("PrintPDFAddPrinterDialogView", typeof(HomagGroup.FLS.Infrastructure.Framework.Contracts.IDialogView))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("DialogView (Load Xaml at Runtime)")]
[EnabledScript(true)]
public class PrintPDFAddPrinterDialogView : HomagGroup.Base.UI.Windows.Dialog, HomagGroup.FLS.Infrastructure.Framework.Contracts.IDialogView, IPartImportsSatisfiedNotification
{
    [Import]
    private LooseXaml _LooseXaml;

    public PrintPDFAddPrinterDialogView()
    {
    }

    public string Name
    {
        get
        {
            return "PrintPDFAddPrinterDialogView";
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
        _LooseXaml.LoadXaml(this, "PrintPDFAddPrinterDialogView");
    }
}
