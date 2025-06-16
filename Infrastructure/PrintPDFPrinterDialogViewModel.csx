#r "PresentationCore"
#r "PresentationFramework"
#r "WindowsBase"


//-----------------------------------------------------------------------------
//   (Class-)Name:   PrintPDFPrinterDialogViewModel
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         <Author>
//   Date:           2022-06-28
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   <Author>        2022-06-28    Created
//   
//-----------------------------------------------------------------------------


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IClientCustomization))]
[Export("PrintPDFPrinterDialogViewModel", typeof(HomagGroup.FLS.Infrastructure.Framework.Contracts.IDialogViewModel))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("View Model für Dialog Druckeinstellung")]
[EnabledScript(true)]
public class PrintPDFPrinterDialogViewModel : HomagGroup.Base.UI.Windows.DialogBaseViewModel, HomagGroup.FLS.Infrastructure.Framework.Contracts.IDialogViewModel
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;
    
    [Import]
    private LooseXaml _LooseXaml;
    
    [Import("BinaryFromReportMethods")]
    HomagGroup.FLS.Infrastructure.Common.Customization.IGlobalCustomization binaryFromReportMethods;
    
    [Import("PrintPDFAddPrinterDialogViewModel")]
    HomagGroup.FLS.Infrastructure.Framework.Contracts.IDialogViewModel printPDFAddPrinterDialogViewModel;

    [Import("PrintPDFMethods")]
    HomagGroup.FLS.Infrastructure.Common.Customization.IGlobalCustomization printPDFMethods;

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
            return "PrintPDFPrinterDialogViewModel";
        }
    }
    
    // Konstruktor
    public PrintPDFPrinterDialogViewModel()
    {
        _Logger = LogHelper.GetLogger("PrintPDFPrinterDialogViewModel", typeof(PrintPDFPrinterDialogViewModel));
        _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, "PrintPDFPrinterDialogViewModel");
        
        _Logger.Info("Constructor");
        
        AddPrinterCommand = new RelayCommand(AddPrinterExecute, AddPrinterCanExecute);
        SetDefaultPrinterCommand = new RelayCommand(SetDefaultPrinterExecute, SetDefaultPrinterCanExecute);
        RemovePrinterCommand = new RelayCommand(RemovePrinterExecute, RemovePrinterCanExecute);
        PrintCommand = new RelayCommand(PrintExecute, PrintCanExecute);
    }

    // Defaultprinter
    private IEnumerable<CustPrinterDefault> _DefaultPrinter;

    public IEnumerable<CustPrinterDefault> DefaultPrinter
    {
        get
        {
            return _DefaultPrinter;
        }
        set
        {
            _DefaultPrinter = value;
            RaisePropertyChanged("DefaultPrinter");
            RaisePropertyChanged(() => DefaultPrinter);
        }
    }
    
    // Liste der Drucker für User und Kachel
    private IEnumerable<CustPrinterDefault> _PrinterList;

    public IEnumerable<CustPrinterDefault> PrinterList
    {
        get
        {
            return _PrinterList;
        }
        set
        {
            _PrinterList = value;
            RaisePropertyChanged("PrinterList");
            RaisePropertyChanged(() => PrinterList);
        }
    }
    
    // Sichtbarkeit Schaltfläche für Druck
    private bool _IsVisiblePrint;
    
    public bool IsVisiblePrint
    {
        get
        {
            return _IsVisiblePrint;
        }
        set
        {
            _IsVisiblePrint = value;
            RaisePropertyChanged(() => IsVisiblePrint);
            RaisePropertyChanged("IsVisiblePrint");
        }
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
    
    // zu druckendes Binary
    private List<int> _BinariesSequence;

    public List<int> BinariesSequence
    {
        get
        {
            return _BinariesSequence;
        }
        set
        {
            _BinariesSequence = value;
            RaisePropertyChanged(() => BinariesSequence);
        }
    }
    
    
    //2022-11-16 B.Schmidt
    // zu druckende PrintScreens
/*
    private CustPrintScreen _PrintScreen;

    public CustPrintScreen PrintScreen
    {
        get
        {
            return _PrintScreen;
        }
        set
        {
            _PrintScreen = value;
            RaisePropertyChanged(() => PrintScreen);
        }
    }
*/
    
    // Liste von PrintScreen-Objekten
/*
    private List<CustPrintScreen> _PrintScreens;
    
    public List<CustPrintScreen> PrintScreens
    {
        get
        {
            return _PrintScreens;
        }
        set
        {
            _PrintScreens = value;
            RaisePropertyChanged("PrintScreens");
        }
    }
*/    
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
        }
    }  
    
    private void AddPrinterExecute(object sender)
    {
        _Logger.Info("AddPrinterExecute");
                
        //CustPrinter sendPrinter = (CustPrinter)sender;
        
        using (var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
        {                
            var printer = unitOfWork.GetRepository<CustPrinter>()
                            .Get(x => x.Printer != x.PrinterDefaults.FirstOrDefault(pd => pd.UserName == UserName
                                                                                        && pd.ReportLayout == ReportLayout
                                                                                        && pd.Dialogue == ViewName).Printer);
                                                                                        
            _Logger.Info($"{printer.Count().ToString()} Drucker für AddPrinter gefunden");            
            
            /*if (printer.Any())
            {*/
                (printPDFAddPrinterDialogViewModel as PrintPDFAddPrinterDialogViewModel).UserName = UserName;
                (printPDFAddPrinterDialogViewModel as PrintPDFAddPrinterDialogViewModel).ViewName = ViewName;
                (printPDFAddPrinterDialogViewModel as PrintPDFAddPrinterDialogViewModel).ReportLayout = ReportLayout;
                (printPDFAddPrinterDialogViewModel as PrintPDFAddPrinterDialogViewModel).PrinterList = printer;               
               
                
                var result = _LooseXaml.ShowDialog("PrintPDFAddPrinterDialogView", printPDFAddPrinterDialogViewModel);
                                
                if (result.HasValue && result == true)
                {
                    PrinterList = (printPDFMethods as PrintPDFMethods).GetPrinterList(_Logger, unitOfWork, UserName, ViewName, ReportLayout, 0);
                    
                }
            //}
        }
        
    }
    
    private bool AddPrinterCanExecute(object arg)
    {
        return true;
    }
    
    
    private void SetDefaultPrinterExecute(object sender)
    {
        _Logger.Info($"SetDefaultPrinterExecute");
        
        CustPrinterDefault printer = (CustPrinterDefault)sender;
        
        if (printer != null)
        {
            _Logger.Info($"Default setzen: {printer.Printer}");
            
            using (var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
            {
                // Defaultkennzeichen von aktuellem Default-Printer entfernen
                if (DefaultPrinter.Any())
                { 
                    var defaultPrinter = DefaultPrinter.FirstOrDefault();
                    if (defaultPrinter != null)
                    {
                        var currentDefaultPrinter = unitOfWork.GetRepository<CustPrinterDefault>()
                                                        .GetFirstOrDefault(x => x.Sequence == defaultPrinter.Sequence);
        
                        currentDefaultPrinter.IsDefault = 0;
                    }
                }
                
                // Defaultkennzeichen bei neu gewähltem Printer setzen 
                var newDefaultPrinter = unitOfWork.GetRepository<CustPrinterDefault>()
                                            .GetFirstOrDefault(x => x.Sequence == printer.Sequence);
                                            
                newDefaultPrinter.IsDefault = 1;
                
                unitOfWork.Save();
                
                DefaultPrinter = (printPDFMethods as PrintPDFMethods).GetPrinterList(_Logger, unitOfWork, UserName, ViewName, ReportLayout, 1);
                PrinterList = (printPDFMethods as PrintPDFMethods).GetPrinterList(_Logger, unitOfWork, UserName, ViewName, ReportLayout, 0);

                
            }
            
        }
    }
    
    private bool SetDefaultPrinterCanExecute(object arg)
    {
        return true;
    }
    
    
    private void RemovePrinterExecute(object sender)
    {
        _Logger.Info($"RemovePrinterExecute");
        
        CustPrinterDefault printer = (CustPrinterDefault)sender;
        
        if (printer != null)
        {
            bool removePrinter = false;
            
                
            
            /*System.Windows.Application.Current.Dispatcher.Invoke(() =>{
                var result = HomagGroup.Base.UI.Windows.MessageBox.Show($"Soll der Drucker \"{printer.Printer}\" aus der Liste entfernt werden?","Drucker aus Liste entfernen",MessageBoxButton.YesNo);
                if (result == System.Windows.MessageBoxResult.Yes)
                {	
                    removePrinter = true;
                     
                }
						
			});*/
			
			using (var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
            {
    			/*if (removePrinter)
                {*/	
                    var currentPrinter = unitOfWork.GetRepository<CustPrinterDefault>()
                                                .Get(x => x.Printer == printer.Printer
                                                        && x.UserName == UserName
                                                        && x.Dialogue == ViewName
                                                        && x.ReportLayout == ReportLayout);
                                                
                    _Logger.Info($"Drucker entfernen: {printer.Printer}");
                    
                    if (currentPrinter.Any())
                    {    
                        printer = null; 
                    
                        _Logger.Info($"Drucker löschen: {currentPrinter.FirstOrDefault().Sequence}");
                        
                        unitOfWork.BulkDelete(currentPrinter);
                        unitOfWork.Save();
                        /*
                        var temp = unitOfWork.GetRepository<CustPrinterDefault>()
                                            .GetFirstOrDefault(x => x.Sequence == currentPrinter.FirstOrDefault().Sequence);
                        if (temp != null)
                        {
                            _Logger.Info($"Drucker noch da: {temp.Sequence}");
                        }
                        */
                        
                        PrinterList = (printPDFMethods as PrintPDFMethods).GetPrinterList(_Logger, unitOfWork, UserName, ViewName, ReportLayout, 0);
                    }
                        
                    
                //}
			}
        }
    }
    
    private bool RemovePrinterCanExecute(object arg)
    {
        return true;
    }
    
    
    private void PrintExecute(object sender)
    {
        try
        {
        
            _Logger.Info($"PrintExecute");
            
            CustPrinterDefault printer = (CustPrinterDefault)sender;
            
            if (printer != null)
            {
                _Logger.Debug($"test1");
                _Logger.Info($"gewählter Drucker [{printer.Printer}]");

                //2022-11-16 B.Schmidt
                //Prüfen ob es sich um einen Druck aus Binaries oder PrintScreen handelt
                //Druck aus Binary an Print PDF
/*                
                if(BinariesSequence!=null && PrintScreens == null)
                {
                    _Logger.Info($"{BinariesSequence.Count().ToString()} Binaries auf {printer.Printer} drucken");
                    
                    using (var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
                    {
                        foreach (var binariesSequence in BinariesSequence)
                        {
                            (printPDFMethods as PrintPDFMethods).CreatePDFPrintJob(_Logger, unitOfWork, binariesSequence, printer, PrintQuantity);
                        }
                    }
                    
                    if (BinariesSequence.Count() > 0 )
                    {
                        if (_ShowInfoBallon)
                        {
                            string printJobWording = PrintQuantity == 1 ? "Druckauftrag" : "Druckaufträge";
                            string reportWording = BinariesSequence.Count() == 1 ? "Report" : "Reports";
            				(infoBallonMessage as InfoBallonMessage).UserFeedback(this.Name
            						, PrintQuantity.ToString() + " " + printJobWording + " für " + BinariesSequence.Count().ToString() + " " + reportWording + " an Drucker\n" + printer.Printer + "\ngesendet."
            						, 6000
            						, HomagGroup.Base.UI.DeviceState.Ok);
            	        }
        	        }
                }
*/               
                if(BinariesSequence!=null)
                {
                    _Logger.Info($"{BinariesSequence.Count().ToString()} Binaries auf {printer.Printer} drucken");
                    
                    using (var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
                    {
                        foreach (var binariesSequence in BinariesSequence)
                        {
                            (printPDFMethods as PrintPDFMethods).CreatePDFPrintJob(_Logger, unitOfWork, binariesSequence, printer, PrintQuantity);
                        }
                    }
                    
                    if (BinariesSequence.Count() > 0 )
                    {
                        if (_ShowInfoBallon)
                        {
                            string printJobWording = PrintQuantity == 1 ? "Druckauftrag" : "Druckaufträge";
                            string reportWording = BinariesSequence.Count() == 1 ? "Report" : "Reports";
            				(infoBallonMessage as InfoBallonMessage).UserFeedback(this.Name
            						, PrintQuantity.ToString() + " " + printJobWording + " für " + BinariesSequence.Count().ToString() + " " + reportWording + " an Drucker\n" + printer.Printer + "\ngesendet."
            						, 6000
            						, HomagGroup.Base.UI.DeviceState.Ok);
            	        }
        	        }
                }

                _Logger.Debug($"test2");
                //Beauftragung über PrintScreen
/*                
                if(PrintScreen != null )
                {
                    _Logger.Debug($"Druck PrintScreen1");
                     
                    _Logger.Info($"Druck PrintScreen: {PrintScreen.TaggedLinesGroupName} ");//auf Drucker: {printer.Printer}
                    
                    using (var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
                    {
                        var report = (binaryFromReportMethods as BinaryFromReportMethods).GetReport(unitOfWork, ReportLayout);
                            
                        if (report != null)
                        {
                            //Printer aus Auswahl übernehmen
                            PrintScreen.Printer = printer.Printer;
                            
                            unitOfWork.AddOrUpdate(new[]{PrintScreen});
                            
                            unitOfWork.Save();
                            
                            //Erstellen PrintJob
                            var custPrintJob = new CustPrintJob();
                            
                            custPrintJob.TaggedLinesGroupName = PrintScreen.TaggedLinesGroupName;
                            custPrintJob.JobName = report.JobName;
                            custPrintJob.TransferState = 10;
                            custPrintJob.CreationSource = PrintScreen.CreationSource;
                            custPrintJob.ReportBindingsSequence = Convert.ToInt64(PrintScreen.TaggedLinesGroupName);
                            
                            unitOfWork.AddOrUpdate(new[]{custPrintJob});
                            
                            //Erstellen ReportBinding
                            var custReportBinding = new CustReportBinding();
                                    
                            custReportBinding.DateReportRelevance = DateTime.Now;
                            custReportBinding.TaggedLinesGroupName = PrintScreen.TaggedLinesGroupName;
                            custReportBinding.ReportLayout = report.Layout;
                            custReportBinding.ReportEntity = report.Entity;
                            custReportBinding.ReportSequence = Convert.ToInt64(PrintScreen.TaggedLinesGroupName);
                            custReportBinding.PlanningGroup = string.Empty;
                            custReportBinding.WorkOrder = string.Empty;
                            
                            unitOfWork.AddOrUpdate(new[]{custReportBinding});
                            
                            unitOfWork.Save();
                            
                            (infoBallonMessage as InfoBallonMessage).UserFeedback(this.Name
                						, PrintScreen.TaggedLinesGroupName + " an Drucker\n" + printer.Printer + "\ngesendet."
                						, 6000
                						, HomagGroup.Base.UI.DeviceState.Ok);
						}
						else
						{
                            _Logger.Warn($"Report {ReportLayout} nicht konfiguriert!");
						}
                    }
                }
                
                if(PrintScreens != null)
                {     
                    ExecutePrintWithPrintScreens(_Logger, PrintScreens, printer);
        
                }
 */               
                // Dialog nach Druckbeauftragung schließen
                this.DialogResult = true;
            }
        }
        catch(Exception e)
        {
            _Logger.Error($"ERROR: PrintExecute", null, e);
            
            (infoBallonMessage as InfoBallonMessage).UserFeedback(this.Name
        						, "FEHLER: Beim Drucken, bitte Meldungen prüfen!"
        						, 6000
        						, HomagGroup.Base.UI.DeviceState.Alarm);
        }
    }
    
    private bool PrintCanExecute(object arg)
    {
        return true;
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
    
    // Command neue Drucker hinzufügen    
    public System.Windows.Input.ICommand AddPrinterCommand { get; set; }
    // Command Drucker als Default definieren
    public System.Windows.Input.ICommand SetDefaultPrinterCommand { get; set; }
    // Command Drucker aus Liste zu User, Kachel, Report entfernen
    public System.Windows.Input.ICommand RemovePrinterCommand { get; set; }
    // Command auf Drucker drucken
    public System.Windows.Input.ICommand PrintCommand { get; set; }
    

    private void Apply(object parameters)
    {
        // set DialogResult to a value closes the Dialog
        this.DialogResult = true;
    }
    
    
    // Beauftragung Druck mit PrintScreens-Kollektion - z.B. bei Packstückeingabe
/*
    public void ExecutePrintWithPrintScreens(Logger logger, IEnumerable<CustPrintScreen> printScreens, CustPrinterDefault printer)
    {
        if (printScreens != null)
        {
            using (var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
            {
                var printScreenTaggedLinesGroupNames = printScreens.Select(x => x.TaggedLinesGroupName).ToList();
                var viewReportBindings = unitOfWork.GetRepository<CustViewReportBinding>()
                                            .Get(x => printScreenTaggedLinesGroupNames.Contains(x.TaggedLinesGroupName));
                
                if (viewReportBindings.Any())
                {
                    foreach (var printScreen in printScreens)
                    {
                        var viewReportBinding = viewReportBindings.FirstOrDefault(x => x.TaggedLinesGroupName == printScreen.TaggedLinesGroupName);
                                                                                
                        if (viewReportBinding != null)
                        {
                        
                            logger.Info($"ReportBinding.Sequence {viewReportBinding.Sequence} zu PrintScreen.TaggedLinesGroupName {printScreen.TaggedLinesGroupName} gefunden");
                        
                            var report = (binaryFromReportMethods as BinaryFromReportMethods).GetReport(unitOfWork, viewReportBinding.ReportLayout);
                            
                            if (report != null)
                            {
                                logger.Info($"Report.Sequence {report.Sequence} zu ReportLayout {viewReportBinding.ReportLayout} gefunden");
                                logger.Info($"PrintScreen {printScreen.TaggedLinesGroupName} um Drucker {printer.Printer} ergänzen");
                                
                                //Printer aus Auswahl übernehmen
                                printScreen.Printer = printer.Printer;
                                
                                unitOfWork.AddOrUpdate(new[]{printScreen});
                                
                                unitOfWork.Save();
                                                            
                                //Erstellen PrintJob
                                var custPrintJob = new CustPrintJob();
                                
                                custPrintJob.ProductionOrderCode = viewReportBinding.ProductionOrderCode;
                                custPrintJob.TaggedLinesGroupName = printScreen.TaggedLinesGroupName;
                                custPrintJob.JobName = report.JobName;
                                custPrintJob.ReportField01 = viewReportBinding.ReportField01;
                                custPrintJob.ReportField02 = viewReportBinding.ReportField02;
                                custPrintJob.TransferState = 10;
                                custPrintJob.CreationSource = printScreen.CreationSource;
                                custPrintJob.ReportBindingsSequence = Convert.ToInt64(viewReportBinding.TaggedLinesGroupName);
                                
                                unitOfWork.AddOrUpdate(new[]{custPrintJob});
                                
                                unitOfWork.Save();
                            }
                            else
                            {   
                                logger.Info($"Kein Report zu ReportLayout {viewReportBinding.ReportLayout} gefunden");
                            }
                        
                        }
                        else
                        {
                            logger.Warn($"Kein ReportBinding zu PrintScreen.TaggedLinesGroupName {printScreen.TaggedLinesGroupName} gefunden");
                        }
                        
                    }
                    
                    string balloonMessage = string.Empty;
                    
                    int packageNumberMax = viewReportBindings.Max(x => x.ReportField02).ToInt32(); // Gesamtanzahl Packstücke
                    
                    string packageString = "Packstück";
                    string pluralizedPackageString = packageNumberMax > 1? packageString + "en" : packageString;
                    
                    if (viewReportBindings.Count() == packageNumberMax)
                    {
                        balloonMessage = $"Alle Etiketten zu {viewReportBindings.Max(x => x.ReportField02)} {pluralizedPackageString}";
                    }
                    else
                    {
                        balloonMessage = $"Etikett {viewReportBindings.FirstOrDefault().ReportField01} von {viewReportBindings.FirstOrDefault().ReportField02} {pluralizedPackageString}";
                    }
                    
                    balloonMessage += $" an Drucker\n{printer.Printer}\ngesendet.";
                    
                    
                    (infoBallonMessage as InfoBallonMessage).UserFeedback(this.Name
        						, balloonMessage
        						, 6000
        						, HomagGroup.Base.UI.DeviceState.Ok);
    						
    			}
            }
        }
    }
*/    
    
}


