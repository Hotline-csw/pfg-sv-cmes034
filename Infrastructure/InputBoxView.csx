#r "PresentationCore"
#r "PresentationFramework"
#r "WindowsBase"
#r "System.Xaml"
#r "System.Xml"


//-----------------------------------------------------------------------------
//   (Class-)Name:   InputBoxView
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         <Author>
//   Date:           2023-05-09
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   <Author>        2023-05-09    Created
//   
//-----------------------------------------------------------------------------


using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;


[Export("InputBoxView", typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IClientCustomization))]
[Export("InputBoxView", typeof(Dialog))]
[Export("InputBoxView", typeof(HomagGroup.FLS.Infrastructure.Framework.Contracts.IDialogView))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("DialogView (Load Xaml at Runtime)")]
[EnabledScript(true)]
public class InputBoxView : HomagGroup.Base.UI.Windows.Dialog, HomagGroup.FLS.Infrastructure.Framework.Contracts.IDialogView, IPartImportsSatisfiedNotification
{
    [Import]
    private LooseXaml _LooseXaml;

    public InputBoxView()
    {
    }

    public string Name
    {
        get
        {
            return "InputBoxView";
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
        _LooseXaml.LoadXaml(this, "InputBoxView");
    }
}
