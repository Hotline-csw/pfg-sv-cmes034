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


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IClientCustomization))]
[Export("DropDownWorkCenterView", typeof(UserControl))]
[Export("DropDownWorkCenterView", typeof(HomagGroup.FLS.Infrastructure.Framework.Contracts.IDetailView))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("DetailView (Load Xaml at Runtime)")]
[EnabledScript(true)]
public class DropDownWorkCenterView : System.Windows.Controls.UserControl, HomagGroup.FLS.Infrastructure.Framework.Contracts.IDetailView, IPartImportsSatisfiedNotification
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

    public HomagGroup.FLS.Infrastructure.Framework.Contracts.ICustomViewModel ViewModel
    {
        get
        {
            return DataContext as ICustomViewModel;
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
