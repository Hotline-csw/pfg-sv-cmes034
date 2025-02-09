#r "PresentationCore"
#r "PresentationFramework"
#r "WindowsBase"
#r "System.Xaml"
#r "System.Xml"


//-----------------------------------------------------------------------------
//   (Class-)Name:   DropDownWorkCenterView
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         <Author>
//   Date:           2025-02-09
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   <Author>        2025-02-09    Created
//   
//-----------------------------------------------------------------------------


using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;


[Export("DropDownWorkCenterView", typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IClientCustomization))]
[Export("DropDownWorkCenterView", typeof(Dialog))]
[Export("DropDownWorkCenterView", typeof(HomagGroup.FLS.Infrastructure.Framework.Contracts.IDialogView))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("DialogView (Load Xaml at Runtime)")]
[EnabledScript(true)]
public class DropDownWorkCenterView : HomagGroup.Base.UI.Windows.Dialog, HomagGroup.FLS.Infrastructure.Framework.Contracts.IDialogView, IPartImportsSatisfiedNotification
{
    [Import]
    private LooseXaml _LooseXaml;

    public DropDownWorkCenterView()
    {
    }

    public string Name
    {
        get
        {
            return "DropDownWorkCenterView";
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
        _LooseXaml.LoadXaml(this, "DropDownWorkCenterView");
    }
}
