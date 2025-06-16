#r "PresentationCore"
#r "PresentationFramework"
#r "WindowsBase"


//-----------------------------------------------------------------------------
//   (Class-)Name:   PrintPDFQuantityInput
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         <Author>
//   Date:           2022-11-22
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   <Author>        2022-11-22    Created
//   
//-----------------------------------------------------------------------------


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IClientCustomization))]
[Export("PrintPDFQuantityInputViewModel", typeof(HomagGroup.FLS.Infrastructure.Framework.Contracts.IDialogViewModel))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("generated Class")]
[EnabledScript(false)]
public class PrintPDFQuantityInputViewModel : HomagGroup.Base.UI.Windows.DialogBaseViewModel, HomagGroup.FLS.Infrastructure.Framework.Contracts.IDialogViewModel
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;
    
    [Import]
    private LooseXaml _LooseXaml;
    
    [Import("PrintPDFMethods")]
    HomagGroup.FLS.Infrastructure.Common.Customization.IGlobalCustomization printPDFMethods;

    [Import("PrintPDFClientMethods")]
    HomagGroup.FLS.Infrastructure.Common.Customization.IClientCustomization printPDFClientMethods;

    [Import("InfoBallonMessage")]
    HomagGroup.FLS.Infrastructure.Common.Customization.IClientCustomization infoBallonMessage;
    
    private const bool _ShowInfoBallon = true;

    private Logger _Logger;


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
            return "PrintPDFQuantityInputViewModel";
        }
    }
    
    // Konstruktor
    public PrintPDFQuantityInputViewModel()
    {
        _Logger = LogHelper.GetLogger("PrintPDFQuantityInputViewModel", typeof(PrintPDFQuantityInputViewModel));
        _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, "PrintPDFQuantityInputViewModel");
        
        _Logger.Info("Constructor");
        
        PrintOnDefaultPrinterCommand = new RelayCommand(PrintOnDefaultPrinterExecute, PrintOnDefaultPrinterCanExecute);
        OpenPrinterDialogCommand = new RelayCommand(OpenPrinterDialogExecute, OpenPrinterDialogCanExecute);
    }

        
    // zu druckende Menge
    private int _PrintQuantity;

    public int PrintQuantity
    {
        get
        {
            return _PrintQuantity;
        }
        set
        {
            _PrintQuantity = value;
            
            RaisePropertyChanged("PrintQuantity");
            RaisePropertyChanged(() => PrintQuantity);
        }
    }  
    
    // Labeltext
    private string _LabelText;
    
    public string LabelText
    {
        get
        {
            //return _LabelText;
            if (!string.IsNullOrEmpty(_LabelText))
            {
                return _LabelText;
            }
            else
            {
                return "Anzahl Ausdrucke:";
            }
        }
        set
        {
            _LabelText = value;
            
            RaisePropertyChanged("LabelText");
            RaisePropertyChanged(() => LabelText);            
        }
    }
    
    // Kollektion (Auswahl) aus ReportBinding
    private IEnumerable<CustViewReportBinding> _ViewReportBinding;
    
    public IEnumerable<CustViewReportBinding> ViewReportBinding
    {
        get
        {
            return _ViewReportBinding;
        }
        set
        {
            _ViewReportBinding = value;
            RaisePropertyChanged("ViewReportBinding");
        }
    }
    
    // Sichtbarkeit Button DefaultPrint
    private bool _PrintOnDefaultPrinterButtonVisible;
    
    public bool PrintOnDefaultPrinterButtonVisible
    {
        get
        {
            return _PrintOnDefaultPrinterButtonVisible;
        }
        set
        {
            _PrintOnDefaultPrinterButtonVisible = value;
            RaisePropertyChanged("PrintOnDefaultPrinterButtonVisible");
        }
    }
    
    // Sichtbarkeit Button Druckerdialog
    private bool _OpenPrinterDialogButtonVisible;
    
    public bool OpenPrinterDialogButtonVisible
    {
        get
        {
            return _OpenPrinterDialogButtonVisible;
        }
        set
        {
            _OpenPrinterDialogButtonVisible = value;
            RaisePropertyChanged("OpenPrinterDialogButtonVisible");
        }
    }
    
    // Sichtbarkeit OK-Button
    private bool _OkButtonVisible;
    
    public bool OkButtonVisible
    {
        get
        {
            return _OkButtonVisible;
        }
        set
        {
            _OkButtonVisible = value;
            RaisePropertyChanged("OkButtonVisible");
        }
    }
    
    // PrintQuantity bei Beschlagbeutel-Serien-Etikett von Etikettenanzahl auf Seitenanzahl umrechnen
    private int GetPrintQuantity()
    {
        
        return PrintQuantity;

        /*
        var reportBinding = ViewReportBinding.FirstOrDefault();
        
        if (reportBinding.ReportLayout == "FittingSeriesLabel")
        {
            return (int)Math.Ceiling(PrintQuantity / reportBinding.ReportField02.ToDecimal());
        }
        else
        {
            return PrintQuantity;
        }
        */
    }
    
    
    // Aufruf Defaultdruck
    private void PrintOnDefaultPrinterExecute(object sender)
    {
        _Logger.Info("PrintOnDefaultPrinterExecute");
        
        if ((printPDFClientMethods as PrintPDFClientMethods).PrintOnDefaultPrinter(_Logger, ViewReportBinding, GetPrintQuantity()))
        {                
            // Dialog beenden
            this.DialogResult = true;
        }
    }
    
    private bool PrintOnDefaultPrinterCanExecute(object arg)
    {
        if (PrintQuantity > 0)
        {
            return true;
        }
        
        return false;
    }
    
    // Aufruf Drucken / Druckeinstellung
    private void OpenPrinterDialogExecute(object sender)
    {
        _Logger.Info("OpenPrinterDialogExecute");
        
        (printPDFClientMethods as PrintPDFClientMethods).OpenPrinterDialog(_Logger, ViewReportBinding, GetPrintQuantity());
        
        // Dialog beenden
        this.DialogResult = true;
    }
    
    private bool OpenPrinterDialogCanExecute(object arg)
    {
        if (PrintQuantity > 0)
        {
            return true;
        }
        
        return false;
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
    
    
    // Command auf Default-Drucker drucken    
    public System.Windows.Input.ICommand PrintOnDefaultPrinterCommand { get; set; }
    // Command Drucken / Druckeinstellung    
    public System.Windows.Input.ICommand OpenPrinterDialogCommand { get; set; }
    
    
}

