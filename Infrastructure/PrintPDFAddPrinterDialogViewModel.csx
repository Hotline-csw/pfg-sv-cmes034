#r "PresentationCore"
#r "PresentationFramework"
#r "WindowsBase"


//-----------------------------------------------------------------------------
//   (Class-)Name:   PrintPDFAddPrinterDialogViewModel
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

[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IClientCustomization))]
[Export("PrintPDFAddPrinterDialogViewModel", typeof(HomagGroup.FLS.Infrastructure.Framework.Contracts.IDialogViewModel))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("generated Class")]
[EnabledScript(true)]
public class PrintPDFAddPrinterDialogViewModel : HomagGroup.Base.UI.Windows.DialogBaseViewModel, HomagGroup.FLS.Infrastructure.Framework.Contracts.IDialogViewModel
{
    private static readonly EntityHelper _EntityHelper = ServiceLocatorProvider.Current.Resolve<EntityHelper>();
    
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;
    
    [Import("PrintPDFMethods")]
    HomagGroup.FLS.Infrastructure.Common.Customization.IGlobalCustomization printPDFMethods;
    
    private Logger _Logger { get; set; }

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
    
     private List<HomagGroup.FLS.Infrastructure.Framework.DataGrid.AllowedValueViewModel> _AllowedValues;

    
    public List<HomagGroup.FLS.Infrastructure.Framework.DataGrid.AllowedValueViewModel> AllowedValues
    {
        get
        {
            if (_AllowedValues == null)
               _AllowedValues = new List<HomagGroup.FLS.Infrastructure.Framework.DataGrid.AllowedValueViewModel>();
          
            return _AllowedValues;
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
            return "PrintPDFAddPrinterDialogViewModel";
        }
    }
    
    // Konstruktor
    public PrintPDFAddPrinterDialogViewModel()
    {
        _Logger = LogHelper.GetLogger("PrintPDFAddPrinterDialogViewModel", typeof(PrintPDFAddPrinterDialogViewModel));
        _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, "PrintPDFAddPrinterDialogViewModel");
                
        _Logger.Debug("Constructor");
        var allowedValueGroup = _EntityHelper.GetAllowedValueGroup("PrinterType");
        _Logger.Info(allowedValueGroup.Name);
        AllowedValues.AddRange(GenerateViewModels(allowedValueGroup.AllowedValues));
        
        _Logger.Info($"{AllowedValues.Count().ToString()} PrinterTypes");
    }
    
    static IEnumerable<AllowedValueViewModel> GenerateViewModels(IEnumerable<HomagGroup.FLS.Services.DataAccess.DomainDescription.Contracts.Client.IAllowedValue> allowedValues)
    {
        if (allowedValues == null)
            return null;

        var list = new List<AllowedValueViewModel>();
        foreach (var allowedValue in allowedValues)
        {
            list.Add(new AllowedValueViewModel(allowedValue));
        }

        return list;
    }
    
    // aufrufender CMes-User
    private string _UserName;

    public string UserName
    {
        get
        {
            return _UserName;
        }
        set
        {
            _UserName = value;
            RaisePropertyChanged(() => UserName);
        }
    }
    
    // Name der aufrufenden Kachel
    private string _ViewName;

    public string ViewName
    {
        get
        {
            return _ViewName;
        }
        set
        {
            _ViewName = value;
            RaisePropertyChanged(() => ViewName);
        }
    }
    
    // Name des aufrufenden Report-Layouts
    private string _ReportLayout;

    public string ReportLayout
    {
        get
        {
            return _ReportLayout;
        }
        set
        {
            _ReportLayout = value;
            RaisePropertyChanged(() => ReportLayout);
        }
    }

    // Liste der Drucker für User und Kachel
    private IEnumerable<CustPrinter> _PrinterList;

    public IEnumerable​<CustPrinter> PrinterList
    {
        get
        {
            return _PrinterList;
        }
        set
        {
            _PrinterList = value;
            RaisePropertyChanged(() => PrinterList);
        }
    }

    // ausgewählter, hinzuzufügender Drucker
    private CustPrinter _SelectedPrinter;

    public CustPrinter SelectedPrinter
    {
        get
        {
            return _SelectedPrinter;
        }
        set
        {
            //_Logger.Info($"Printer hinzufügen: {SelectedPrinter.FirstOrDefault().Printer}");
            _SelectedPrinter = value;
            RaisePropertyChanged("SelectedPrinter");
            RaisePropertyChanged(() => SelectedPrinter);
        }
    }


    // Command to handle OK-Button in Example 
    private System.Windows.Input.ICommand _ApplyCommand;

    public System.Windows.Input.ICommand ApplyCommand
    {
        get
        {
            return this._ApplyCommand ?? (this._ApplyCommand = new RelayCommand(this.Apply, ApplyCanExecute));
        }
    }


    private void Apply(object parameters)
    {
        _Logger.Info($"Printer hinzufügen: {SelectedPrinter.Printer}");
        
        if (SelectedPrinter != null)
        {
            using (var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
            {
                (printPDFMethods as PrintPDFMethods).AddPrinterUserTileReportLayout(_Logger, 
                                                                                    unitOfWork, 
                                                                                    UserName, 
                                                                                    ViewName, 
                                                                                    ReportLayout, 
                                                                                    SelectedPrinter);
            }
        }
        
    
        // set DialogResult to a value closes the Dialog
        this.DialogResult = true;
    }
    
    private bool ApplyCanExecute(object arg)
    {
    	if(SelectedPrinter != null && UserName != null && ViewName != null && ReportLayout != null)
    		return true;
    	
    	return false;		
    }
}
