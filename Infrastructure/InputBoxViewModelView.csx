#r "PresentationCore"
#r "PresentationFramework"
#r "WindowsBase"
#r "System.Xaml"
#r "System.Xml"


//-----------------------------------------------------------------------------
//   (Class-)Name:   InputBoxViewModelView
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         <Author>
//   Date:           2026-06-02
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   <Author>        2026-06-02    Created
//   
//-----------------------------------------------------------------------------


using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IClientCustomization))]
[Export("InputBoxViewModelView", typeof(UserControl))]
[Export("InputBoxViewModelView", typeof(HomagGroup.FLS.Infrastructure.Framework.Contracts.IDetailView))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("DetailView (Load Xaml at Runtime)")]
[EnabledScript(true)]
public class InputBoxViewModelView : System.Windows.Controls.UserControl, HomagGroup.FLS.Infrastructure.Framework.Contracts.IDetailView, IPartImportsSatisfiedNotification
{
    [Import]
    private LooseXaml _LooseXaml;

    public InputBoxViewModelView()
    {
    }

    public string Name
    {
        get
        {
            return "InputBoxViewModelView";
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
        _LooseXaml.LoadXaml(this, "InputBoxViewModelView");
    }
}
