#r "PresentationCore"
#r "PresentationFramework"
#r "WindowsBase"


//-----------------------------------------------------------------------------
//   (Class-)Name:   DropDownWorkCenter
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
[Export("DropDownWorkCenterViewModel", typeof(HomagGroup.FLS.Infrastructure.Framework.Contracts.IDialogViewModel))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("generated Class")]
[EnabledScript(true)]
public class DropDownWorkCenterViewModel : HomagGroup.Base.UI.Windows.DialogBaseViewModel, HomagGroup.FLS.Infrastructure.Framework.Contracts.IDialogViewModel
{

    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;

    /// <summary>
    /// Gets or sets the settings
    /// </summary>
    public IComponentSettings Settings { get; set; }


    public System.Windows.ResourceKey FallbackIconResourceKey
    {
        get
        {
            return null;
        }
    }

    public void Initialize(object configuration)
    {
        // DialogResult must be set to null here
        DialogResult = null;
        
        // Liste für Arbeitsplätze anlegen
        DropDownWorkCenters = new List<string>();

        using (IUnitOfWorkBase unitOfWork = _UnitOfWorkFactory.CreateUnitOfWorkBase())
        {
            // Alle Arbeitsplätze aus der DB holen und nach Code absteigend sortieren
            var workCenters = unitOfWork.GetRepository<WorkCenter>().Get(wc => wc.Code != "EB2" && wc.Code != "EB3").OrderBy(wc => wc.FeedbackGroup);
            
            // Ausgabe des WorkCenterCodes im DropDown Menü
            foreach(var workCenter in workCenters)
            {
                DropDownWorkCenters.Add(workCenter.Code + ": \t" + workCenter.Description);
            }
        }
    }

    public string Name
    {
        get
        {
            return "DropDownWorkCenterViewModel";
        }
    }

    // Example of Handling data between ViewModel & View    
    private List<string> _DropDownWorkCenters;
   
    public List<string> DropDownWorkCenters
    {
        get
        {
            return _DropDownWorkCenters;
        }
        set
        {
            _DropDownWorkCenters = value;
            RaisePropertyChanged(() => DropDownWorkCenters);
        }
    }  

    private string _DropDownWorkCenter;
   
    public string DropDownWorkCenter
    {
        get
        {
            return _DropDownWorkCenter;
        }
        set
        {
            _DropDownWorkCenter = value;
            RaisePropertyChanged(() => DropDownWorkCenter);
        }
    }


    // Command to handle OK-Button in Example 
    private System.Windows.Input.ICommand _ApplyCommand;

    public System.Windows.Input.ICommand ApplyCommand
    {
        get
        {
            return this._ApplyCommand ?? (this._ApplyCommand = new RelayCommand(this.Apply));
        }
    }


    private void Apply(object parameters)
    {
        // set DialogResult to a value closes the Dialog
        this.DialogResult = true;
    }
}
