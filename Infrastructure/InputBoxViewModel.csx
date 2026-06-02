#r "PresentationCore"
#r "PresentationFramework"
#r "WindowsBase"


//-----------------------------------------------------------------------------
//   (Class-)Name:   InputBox
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


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IClientCustomization))]
[Export("InputBoxViewModel", typeof(HomagGroup.FLS.Infrastructure.Framework.Contracts.IDialogViewModel))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("generated Class")]
[EnabledScript(true)]
public class InputBoxViewModel : HomagGroup.Base.UI.Windows.DialogBaseViewModel, HomagGroup.FLS.Infrastructure.Framework.Contracts.IDialogViewModel
{

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
    }

    public string Name
    {
        get
        {
            return "InputBoxViewModel";
        }
    }

    // Example of Handling data between ViewModel & View
    private string _TestData;

    public string TestData
    {
        get
        {
            return _TestData;
        }
        set
        {
            _TestData = value;
            RaisePropertyChanged(() => TestData);
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
// Example to use the Dialog from a CommandUserExit:
// TODO:
// - Restart client after generating View/ViewModel.
// - Create Command-UE and type the same name, like you did for creating the Vire/ViewModel.
// - Exchange the content of the generated Command-UE with the content below.
// - Activate the line '#r ControllerMES.Infrastructure.Resource'.
// - Inside the Execute()-function, delete the line calling a NotImplementedException ('throw new NotImplementedException();')
//   and ...
//---------------------------------------------------
/*
//#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   InputBox
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


using System.ComponentModel;
using System.Collections;
using System.Globalization;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IClientCustomization))]
[Export("InputBoxCommand", typeof(HomagGroup.FLS.Infrastructure.Framework.Contracts.ICommandUserExit))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("describe here")]
[EnabledScript(true)]
public class InputBoxCommand : UserExitCustomBase, HomagGroup.FLS.Infrastructure.Framework.Contracts.ICommandUserExit
{
    private const string _ViewName = "InputBoxView";
    private const string _ViewModelName = "InputBoxViewModel";

    [Import]
    private LooseXaml _LooseXaml;

    [Import(_ViewModelName)]
    HomagGroup.FLS.Infrastructure.Framework.Contracts.IDialogViewModel dialogViewModel;

    //[Import]
    //private IUnitOfWorkFactory _UnitOfWorkFactory;

    private Logger _Logger;

    public void Execute(object parameter)
    {
        Guard.ThrowOnArgumentNull(parameter, "parameter");



		throw new NotImplementedException();



        try
        {
            _Logger = LogHelper.GetLogger(Name, typeof(InputBoxCommand));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, "");

            var itemEnumerable = parameter as IEnumerable;

            if (itemEnumerable != null)
            {
			    (dialogViewModel as InputBoxViewModel).TestData = string.Empty;
                bool? result = _LooseXaml.ShowDialog(_ViewName, dialogViewModel);
                
                if(!result.HasValue)
                {
                    //string msgText = string.Format(CultureInfo.InvariantCulture, "Cannot start tile-view with view-name='{0}' and viewmodel-name '{1}' !", _ViewName, _ViewModelName);
                    //TileViewHelperStatic.SetInfoBalloon(msgText, HomagGroup.Base.UI.DeviceState.Error);
                }
                else if(result == false)
                {
                    //TileViewHelperStatic.SetInfoBalloon("Canceled", HomagGroup.Base.UI.DeviceState.Off);
                }
                else
                {
                    string testData = (dialogViewModel as InputBoxViewModel).TestData.ToString();
                        
                    if(testData == null)
                    {
                        //string msgText = string.Format(CultureInfo.InvariantCulture, "testData='{0}' not found !", testData);
                        //TileViewHelperStatic.SetInfoBalloon(msgText, HomagGroup.Base.UI.DeviceState.Error);
                    }
                    else
                    {
                        //string msgText = string.Format(CultureInfo.InvariantCulture, "testData='{0}'.", testData);
                        //TileViewHelperStatic.SetInfoBalloon(msgText, HomagGroup.Base.UI.DeviceState.Ok);
                    }
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
*/
