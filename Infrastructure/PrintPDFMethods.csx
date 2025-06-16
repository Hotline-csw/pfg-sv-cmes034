#r ControllerMES.Infrastructure.Resource

//-----------------------------------------------------------------------------
//   (Class-)Name:   PrintPDFMethods
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         <Author>
//   Date:           2022-06-30
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   <Author>        2022-06-30    Created
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IGlobalCustomization))]
[Export("PrintPDFMethods", typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IGlobalCustomization))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("Methoden zum Druck mit WebPDF")]
[EnabledScript(false)]
public class PrintPDFMethods : UserExitCustomBase, HomagGroup.FLS.Infrastructure.Common.Customization.IGlobalCustomization
{   
    public void CreatePDFPrintJob(Logger logger, IUnitOfWork unitOfWork, int binariesSequence, CustPrinterDefault printer, int printQuantity)
    {
        if (binariesSequence > 0)
        {
            if (printer != null)
            {
                for (int i = 0; i < printQuantity; i++)
                {
                    var newPrintPdf = new CustPrintPDF();
                    
                    newPrintPdf.BinariesSequence = binariesSequence;
                    newPrintPdf.Printer = printer.Printer;
                    newPrintPdf.Description = printer.ReportLayout;
                    newPrintPdf.ProcessingState = 10;
                    newPrintPdf.ErrorState = 0;
                    
                    unitOfWork.AddOrUpdate(new[] {newPrintPdf});
                }
                    
                unitOfWork.Save();
            }
            else
            {
                logger.Warn($"Kein Printer-Objekt übergeben!");
            }
        }
        else
        {
            logger.Warn($"Kein BinariesSequence übergeben!");
        }
    }
    
    
    public IEnumerable<CustPrinterDefault> GetPrinterList(Logger logger, IUnitOfWork unitOfWork, string cmesUserName, string viewName, string reportLayout, int isDefault)
    {
        logger.Info($"Suche Drucker für User {cmesUserName}, Dialog {viewName}, ReportLayout {reportLayout}, IsDefault {Convert.ToBoolean(isDefault).ToString()}");
        IEnumerable<CustPrinterDefault> printerList = null;
        
        // Printer für User + Kachel ermitteln
        printerList = unitOfWork.GetRepository<CustPrinterDefault>()
                                .Get(x => x.UserName == cmesUserName
                                        && x.Dialogue == viewName
                                        && x.ReportLayout == reportLayout
                                        && x.IsDefault == isDefault, "Printer");
                           
        // Einmal zu jedem Printer die Verknüpfung zu CustPrinter aufrufen, um Daten aus Verknüpfung verfügbar zu haben
        foreach (var printer in printerList)
        {
            var reloadedPrinter = printer.CustPrinter;
            //logger.Info($"{printer.Printer} -> {printer.CustPrinter.Description}");
            
        }

        return printerList;
    }
    
    
    public bool AddPrinterUserTileReportLayout(Logger logger, IUnitOfWork unitOfWork, string cmesUserName, string viewName, string reportLayout, CustPrinter printer)
    {
        // prüfen ob Drucker bereits für User, Kachel und Report konfiguriert ist
        var printerExists = unitOfWork.GetRepository<CustPrinterDefault>()
                                .Get(x => x.Printer == printer.Printer
                                        && x.UserName == cmesUserName
                                        && x.Dialogue == viewName
                                        && x.ReportLayout == reportLayout);
                                        
        if (printerExists.Any())
        {
            return false;
        }
        else
        {
            var newPrinterDefault = new CustPrinterDefault();
            
            newPrinterDefault.UserName = cmesUserName;
            newPrinterDefault.ReportLayout = reportLayout;
            newPrinterDefault.Dialogue = viewName;
            newPrinterDefault.Printer = printer.Printer;
            newPrinterDefault.IsDefault = 0;
            
            unitOfWork.AddOrUpdate(new[] {newPrinterDefault});
            
            unitOfWork.Save();
            
            return true;
        }
        
    }
    
    public bool GetSequenceofPneumaticLabel(Logger logger, IUnitOfWork unitOfWork, string producionItemCode)
    {
        try
        {
            var productionItem = unitOfWork.ProductionItemRepository.GetFirstOrDefault(x=>x.Code==producionItemCode);
            
            if(productionItem!=null)
            {
                //Der Aufkleber wird nur gedruckt, wenn in der Stückliste der Artikel "Z109272" vorhanden ist
                if(productionItem.ProductionOrder.ProductionOrdersResources.Any(x=>x.CustomArticleNumber=="Z109272"))
                {
                    return true;
                }
            }
            else
            {
                logger.Warn($"{producionItemCode}: kein ProductionOrder gefunden!?");
            }
        }
        catch(Exception e)
        {
            logger.Error("ERROR: GetSequenceofPneumaticLabel", null, e);
        }
        
        return false;
    }
}
