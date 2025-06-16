//-----------------------------------------------------------------------------
//   (Class-)Name:   PrintPDFClientMethods
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


using System.ComponentModel;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IClientCustomization))]
[Export("PrintPDFClientMethods", typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IClientCustomization))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("Client-Methoden für Druck über PrintPDF / WebPDF")]
[EnabledScript(false)]
public class PrintPDFClientMethods : UserExitCustomBase, HomagGroup.FLS.Infrastructure.Common.Customization.IClientCustomization
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;
    
    [Import]
    protected UserExitHelper  UserExitHelper {get;set;}
    
    [Import("BinaryFromReportMethods")]
    HomagGroup.FLS.Infrastructure.Common.Customization.IGlobalCustomization binaryFromReportMethods;
    
    [Import("PrintPDFMethods")]
    HomagGroup.FLS.Infrastructure.Common.Customization.IGlobalCustomization printPDFMethods;

    [Import("InfoBallonMessage")]
    HomagGroup.FLS.Infrastructure.Common.Customization.IClientCustomization infoBallonMessage;
    
    [Import("PrintPDFPrinterDialogViewModel")]
    HomagGroup.FLS.Infrastructure.Framework.Contracts.IDialogViewModel printPDFPrinterDialogViewModel;
    
    [Import]
    private LooseXaml _LooseXaml;
    
    private const bool _ShowInfoBallon = true;
    
    // Drucken auf Default-Drucker
    public bool PrintOnDefaultPrinter(Logger logger, IEnumerable<CustViewReportBinding> viewReportBindings, int printQuantity)
    {
        using(var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
        {            
            if(viewReportBindings.Any())
            {
                string currentCmesUser = Thread.CurrentPrincipal.GetUserCode();
                string viewName = UserExitHelper.ViewIdentifier.Replace("GenericManagement.","");
                
                // Gruppierung nach ReportLayout
                var groupedReportBindings = viewReportBindings.GroupBy(x => x.ReportLayout);
                foreach (var groupedReportBinding in groupedReportBindings)
                {
                    logger.Info(groupedReportBinding.Key);
                    
                    var defaultPrinter = (printPDFMethods as PrintPDFMethods).GetPrinterList(logger, 
                                                                                                unitOfWork, 
                                                                                                currentCmesUser, 
                                                                                                viewName,
                                                                                                groupedReportBinding.Key,
                                                                                                1)
                                                                                    .FirstOrDefault();
                    
                    if (defaultPrinter != null)
                    {
                        foreach(var viewReportBinding in viewReportBindings.Where(x => x.ReportLayout == groupedReportBinding.Key))
                        {
                            (printPDFMethods as PrintPDFMethods).CreatePDFPrintJob(logger, 
                                                                                    unitOfWork, 
                                                                                    (int)viewReportBinding.BinariesSequence, 
                                                                                    defaultPrinter, 
                                                                                    printQuantity);
                        }
                        
                        if (_ShowInfoBallon)
                        {
                            string printJobWording = printQuantity == 1 ? "Druckauftrag" : "Druckaufträge";
                            string reportWording = viewReportBindings.Count() == 1 ? "Report" : "Reports";
            				(infoBallonMessage as InfoBallonMessage).UserFeedback($"Defaultdruck für {groupedReportBinding.Key}"
            						, $"{printQuantity.ToString()} {printJobWording} für {viewReportBindings.Count().ToString()} {reportWording} an Default-Drucker\n{defaultPrinter.Printer}\ngesendet."
            						, 6000
            						, HomagGroup.Base.UI.DeviceState.Ok);
            	        }
                    }
                    else
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke(
                        () =>
                        {
                            HomagGroup.Base.UI.Windows.MessageBox.Show($"Kein Default-Drucker zu Report \"{groupedReportBinding.Key}\"\nfür User \"{currentCmesUser}\" in dieser Kachel konfiguriert!\n\nBitte Default-Drucker konfigurieren.", "Kein Default-Drucker konfiguriert");
                        });
                        
                        return false;
                    
                        
                        
                    }
                }
            }
        }
        
        return true;
    }
    
    
    // Drucken / Druckeinstellung öffnen
    public bool OpenPrinterDialog(Logger logger, IEnumerable<CustViewReportBinding> viewReportBindings, int printQuantity)
    {
        bool dialogResult = false;
        
        if(viewReportBindings.Any())
        {
            //logger.Info($"{viewReportBindings.Count().ToString()} ViewReportBindings erhalten");
            //logger.Info($"{PrintScreens.Count().ToString()} PrintScreens erhalten");
            using (var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
            {
                
                
                //var viewReportBindings = itemEnumerable.OfType<CustViewReportBinding>();
                var groupedViewReportBindings = viewReportBindings.GroupBy(x => x.ReportLayout);
                logger.Info($"{groupedViewReportBindings.Count().ToString()} verschiedene Report-Layouts in Auswahl");
                if (groupedViewReportBindings.Count() > 1)
                {
                    // mehr als 1 Report-Layout selektiert
                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        HomagGroup.Base.UI.Windows.MessageBox.Show($"{groupedViewReportBindings.Count().ToString()} verschiedene Report-Layouts in Auswahl.\nAufruf ist nur mit einem Report-Layout möglich.", "Fehler in Auswahl");
                    });
                }
                else
                {
                    // 1 Report-Layout selektiert
                    List<int> listOfBinariesSequence = new List<int>();
                    
                    foreach (var viewReportBinding in viewReportBindings)
                    {
                        logger.Info($"ViewReportBinding {viewReportBinding.Sequence.ToString()} mit BinariesSequence {viewReportBinding.BinariesSequence.ToString()}");
                        if (viewReportBinding.BinariesSequence > 0)
                        {
                            listOfBinariesSequence.Add((int)viewReportBinding.BinariesSequence);
                        }
                    }
                    
                    logger.Info($"{listOfBinariesSequence.Count().ToString()} Binaries an PrintPDFPrinterDialogViewModel übergeben");
                
                    string currentCmesUser = Thread.CurrentPrincipal.GetUserCode();
                    string viewName = UserExitHelper.ViewIdentifier.Replace("GenericManagement.","");
                    
                    // Defaultprinter für User + Kachel ermitteln
                    var defaultPrinter = (printPDFMethods as PrintPDFMethods).GetPrinterList(logger, 
                                                                                            unitOfWork, 
                                                                                            currentCmesUser, 
                                                                                            viewName,
                                                                                            viewReportBindings.FirstOrDefault().ReportLayout,
                                                                                            1);
                                       
                    // Für User + Kachel konfigurierte Printer ermitteln
                    var userTilePrinterList = (printPDFMethods as PrintPDFMethods).GetPrinterList(logger, 
                                                                                                unitOfWork, 
                                                                                                currentCmesUser,
                                                                                                viewName, 
                                                                                                viewReportBindings.FirstOrDefault().ReportLayout,
                                                                                                0);
                    
                    (printPDFPrinterDialogViewModel as PrintPDFPrinterDialogViewModel).DefaultPrinter = defaultPrinter;
                    
                    if (defaultPrinter.Any())
                    {
                        logger.Info($"DefaultPrinter {defaultPrinter.FirstOrDefault().Printer}");
                    }
                    
                    (printPDFPrinterDialogViewModel as PrintPDFPrinterDialogViewModel).PrinterList = userTilePrinterList;
                    logger.Info($"PrinterList {userTilePrinterList.Count().ToString()}");
                    
                    (printPDFPrinterDialogViewModel as PrintPDFPrinterDialogViewModel).IsVisiblePrint = false;
                    (printPDFPrinterDialogViewModel as PrintPDFPrinterDialogViewModel).UserName = currentCmesUser;
                    (printPDFPrinterDialogViewModel as PrintPDFPrinterDialogViewModel).ViewName = viewName;
                    (printPDFPrinterDialogViewModel as PrintPDFPrinterDialogViewModel).ReportLayout = viewReportBindings.FirstOrDefault().ReportLayout;
                    if (listOfBinariesSequence.Any())
                    {
                        (printPDFPrinterDialogViewModel as PrintPDFPrinterDialogViewModel).BinariesSequence = listOfBinariesSequence;
                    }
                    
                    (printPDFPrinterDialogViewModel as PrintPDFPrinterDialogViewModel).PrintQuantity = printQuantity;
                    if (PrintScreens != null)
                    {
                        (printPDFPrinterDialogViewModel as PrintPDFPrinterDialogViewModel).BinariesSequence = null;
                        (printPDFPrinterDialogViewModel as PrintPDFPrinterDialogViewModel).PrintScreen = null;
                        (printPDFPrinterDialogViewModel as PrintPDFPrinterDialogViewModel).PrintScreens = PrintScreens;                        
                    }
                    
                    dialogResult = (bool)_LooseXaml.ShowDialog("PrintPDFPrinterDialogView", printPDFPrinterDialogViewModel);
                    
                }
            }
        }
        
        return dialogResult;
    }
    
    // Dialog für Eingabe Listennummer / Druck BA10-Etikettenschlange öffnen
    public bool OpenBA10LabelQueueDialog(Logger logger, string listNumber)
    {/*
        (printPDFBA10LabelQueueDialogViewModel as PrintPDFBA10LabelQueueDialogViewModel).PrintQuantity = 1;
        (printPDFBA10LabelQueueDialogViewModel as PrintPDFBA10LabelQueueDialogViewModel).ListNumber = listNumber;
                
        var result = _LooseXaml.ShowDialog("PrintPDFBA10LabelQueueDialogView", printPDFBA10LabelQueueDialogViewModel);
        if (result.HasValue && result == true)
        {
        }
        
        return (bool)result;*/
        return true;
    }
    
    // Beauftragung Druck mit PrintScreens-Kollektion - z.B. bei Packstückeingabe
/*
    public void ExecutePrintWithPrintScreens(Logger logger, IEnumerable<CustPrintScreen> printScreens, CustPrinterDefault printer)
    {
        using (var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
        {
            var printScreenTaggedLinesGroupNames = PrintScreens.Select(x => x.TaggedLinesGroupName).ToList();
            var viewReportBindings = unitOfWork.GetRepository<CustViewReportBinding>()
                                        .Get(x => printScreenTaggedLinesGroupNames.Contains(x.TaggedLinesGroupName));
            
            if (viewReportBindings.Any())
            {
                foreach (var printScreen in PrintScreens)
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
                            
                            custPrintJob.TaggedLinesGroupName = printScreen.TaggedLinesGroupName;
                            custPrintJob.JobName = report.JobName;
                            custPrintJob.ReportField01 = viewReportBinding.ReportField01;
                            custPrintJob.ReportField02 = viewReportBinding.ReportField02;
                            custPrintJob.TransferState = 10;
                            custPrintJob.CreationSource = printScreen.CreationSource;
                            custPrintJob.ReportBindingsSequence = viewReportBinding.Sequence;
                            
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
*/       
    
    // Liste von PrintScreen-Objekten
/*    
    public List<CustPrintScreen> PrintScreens { get; set; }
*/    
    
    // Druck Bauteiletikett auf Defaultdrucker
    public bool PrintPartLabelDefaultPrinter(Logger logger, List<string> productionItemCodes)
    {
        if (productionItemCodes.Any())
        {
            var viewReportBindings = GetProductionItemsPartLabels(logger, productionItemCodes);
                                                                    
            if(viewReportBindings != null && viewReportBindings.Any())
            {
                return PrintOnDefaultPrinter(logger, viewReportBindings, 1);
            }
        }    

        return false;
    }
    
    // Druck Bauteiletikett - Druckerdialog
    public bool PrintPartLabelPrinterDialog(Logger logger, List<string> productionItemCodes)
    {
        if (productionItemCodes.Any())
        {               
            var viewReportBindings = GetProductionItemsPartLabels(logger, productionItemCodes);
                                                                    
            //if(viewReportBindings != null && viewReportBindings.Any())
            //{
                return OpenPrinterDialog(logger, viewReportBindings, 1);
            //}
        } 

        return false;
    }
    
    // BT-Etiketten zu ProductionItem-Liste aus Druckzentrale ermitteln 
    private IEnumerable<CustViewReportBinding> GetProductionItemsPartLabels(Logger logger, List<string> productionItemCodes)
    {
        IEnumerable<CustViewReportBinding> viewReportBindings = null;
        
        // Liste für ProductionItems zu denen Eintrag in Druckzentrale fehlt
        List<string> listFaultyReportBindings = new List<string>();
     
        logger.Info($"PrintPDFClientMethods.GetProductionItemsPartLabels - Partlabel zu {productionItemCodes.Count().ToString()} Bauteilen ermitteln");

        if (productionItemCodes.Any())
        {
            using(var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
            {
                viewReportBindings = unitOfWork.GetRepository<CustViewReportBinding>()
                                            .Get(x => productionItemCodes.Contains(x.ProductionItemCode)
                                                    && x.ReportLayout == "PartLabel")
                                            .OrderBy(o => productionItemCodes.IndexOf(o.ProductionItemCode));
                                                    
                /*productionItemCodes.ForEach(delegate (string item)
                                                        {
                                                            viewReportBindings = unitOfWork.GetRepository<CustViewReportBinding>()
                                                                .Get(x => x.ProductionItemCode == item
                                                                        && x.ReportLayout == "PartLabel");
                                                        });*/
                
                                                    
                                                    
                logger.Info($"PrintPDFClientMethods.GetProductionItemsPartLabels - {viewReportBindings.Count().ToString()} Partlabel in Druckzentrale gefunden");
                                
                // ermitteln zu welchen Items aus Beauftragung keine ReportBindings vorhanden sind         
                listFaultyReportBindings = productionItemCodes
                                            .Except(viewReportBindings.Select(x => x.ProductionItemCode))
                                            .ToList();
                
                logger.Info($"PrintPDFClientMethods.GetProductionItemsPartLabels - {listFaultyReportBindings.Count().ToString()} Partlabel in Druckzentrale n i c h t gefunden");
                
                // Fehlermeldung für Druckaufträge zu denen Eintrag in Druckzentrale fehlt
                if (listFaultyReportBindings.Any())
                {
				    (infoBallonMessage as InfoBallonMessage).UserFeedbackDetail($"Fehler bei Druck"
						, $"Für folgende Bauteile sind keine Daten in der Druckzentrale vorhanden:"
						, listFaultyReportBindings
						, 0
						, HomagGroup.Base.UI.DeviceState.Alarm);
						
					foreach (var listFaultyReportBinding in listFaultyReportBindings)
					{
					   logger.Warn($"Kein ReportBinding zu Item [{listFaultyReportBinding}] gefunden!");
					}
                } 
            }
        } 
        
        return viewReportBindings;
    }

}
