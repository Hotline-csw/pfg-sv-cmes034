#r "PresentationCore"
#r "PresentationFramework"
#r "WindowsBase"


//-----------------------------------------------------------------------------
//   (Class-)Name:   DropDownWorkCenterViewModel
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


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IClientCustomization))]
[Export("DropDownWorkCenterViewModel", typeof(HomagGroup.FLS.Infrastructure.Framework.Contracts.IDetailViewModel))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("generated Class")]
[EnabledScript(true)]
public class DropDownWorkCenterViewModel : HomagGroup.Base.UI.NotifyPropertyChangedBase, HomagGroup.FLS.Infrastructure.Framework.Contracts.IDetailViewModel
{
    private IEnumerable<object> _Parents;

    /// <summary>
    /// Gets or sets the settings
    /// </summary>
    public IComponentSettings Settings { get; set; }

    public DetailViewConfiguration Configuration { get; set; }

    public System.Windows.ResourceKey FallbackIconResourceKey
    {
        get
        {
            return null;
        }
    }

    public void Initialize(Type parentType, DetailViewConfiguration configuration, IRefreshableViewModel refreshableParent = null)
    {
        Configuration = configuration;
    }

    public string Name
    {
        get
        {
            return "DropDownWorkCenterViewModel";
        }
    }

    /// <summary>
    /// Gets the display name of view model.
    /// </summary>
    public string DisplayName
    {
        get
        {
            return Name;
        }
    }

    /// <summary>
    /// Gets the Tye of the Entity
    /// </summary>
    public Type EntityType { get; private set; }

    /// <summary>
    /// Gets the entity
    /// </summary>
    //public object Entity { get; private set; }

    /// <summary>
    /// Gets or sets the parent entity. 
    /// </summary>
    public IEnumerable<object> Parents
    {
        get
        {
            return _Parents;
        }
        set
        {
            _Parents = value;
            MultipleParentsSelected = Parents != null && Parents.Count() > 1;
            var firstParent = _Parents == null ? null : _Parents.FirstOrDefault();

            RaisePropertyChanged(() => Parents);
        }
    }

    private bool _MultipleParentsSelected;
    /// <summary>
    /// Gets or sets a value, identifying that multiple parents are selected in parent ViewModel
    /// </summary>
    public bool MultipleParentsSelected
    {
        get
        {
            return _MultipleParentsSelected;
        }
        private set
        {
            if (_MultipleParentsSelected != value)
            {
                _MultipleParentsSelected = value;
                RaisePropertyChanged(() => MultipleParentsSelected);
            }
        }
    }
}
