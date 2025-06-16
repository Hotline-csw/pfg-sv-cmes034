//-----------------------------------------------------------------------------
//   (Class-)Name:   AssemblyManualMethods
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         <Author>
//   Date:           2022-07-05
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   <Author>        2022-07-05    Created
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IGlobalCustomization))]
[Export("AssemblyManualMethods", typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IGlobalCustomization))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("Montageanleitung Methoden")]
[EnabledScript(false)]
public class AssemblyManualMethods : UserExitCustomBase, HomagGroup.FLS.Infrastructure.Common.Customization.IGlobalCustomization
{    
    [Import("BinaryFromReportMethods")]
    HomagGroup.FLS.Infrastructure.Common.Customization.IGlobalCustomization binaryFromReportMethods;
        
    [Import("MergePdfMethods")]
    HomagGroup.FLS.Infrastructure.Common.Customization.IGlobalCustomization mergePdfMethods;
    
    [Import("SendMailFromMES")] 
    HomagGroup.FLS.Infrastructure.Common.Customization.IGlobalCustomization sendMailFromMES;
    
    [Import]
    protected IConfigurablePathsServiceDistributed _PathService;
    
    
    public void CreateAssemblyManualWorkOrder(Logger logger, IUnitOfWork unitOfWork, CustWorkOrder workOrder)
    {
        if (workOrder != null)
        {
            var report = unitOfWork.GetRepository<CustReport>().GetFirstOrDefault(x => x.Layout == "AssemblyManual");
                                            
            if (report != null)
            {   
                string planningGroup = GetPlanningGroupFromWorkOrder(logger, unitOfWork, workOrder.WorkOrder);
                
                if ((binaryFromReportMethods as BinaryFromReportMethods)
                        .IsReportDefinedForPlanningGroup(unitOfWork, report.Layout, planningGroup))
                {   
                    // relevante Produktgruppen zu Resource "PDF" aus Konfiguration ermitteln
                    var productCategoriesPdf = unitOfWork.GetRepository<CustMaterialTypeFromProductGroup>()
                                                            .Get(x => x.ResourceType == ResourceType.Pdf)
                                                            .Select(x=>x.ProductGroup)
                                                            .ToList();
                    
                    if (productCategoriesPdf.Any())
                    {                           
                        // Entpr. Kundenaufträge zu Werkauftrag ermitteln
                        var customerOrders = unitOfWork.ProductionOrderRepository
                                                .Get(x => x.CustomWorkOrder == workOrder.WorkOrder
                                                        && (x.CustomCapacityGroupCode == "03" 
                                                            || x.CustomCapacityGroupCode == "07"
                                                            || x.CustomCapacityGroupCode == "08")
                                                        && x.ProductionOrdersResources.Any(por => productCategoriesPdf.Contains(por.CustomProductCategory)))
                                                .GroupBy(g => new { CustomerOrderCode = g.CustomerOrderCode, 
                                                                    ParentPosition = g.CustomParentPosition });
                             //if (productCategoriesSemiFinishedPart.Where(x => x.ProductGroup == assemblyStorageItem.CustomProductCategory).Any())                  
                        foreach (var customerOrder in customerOrders)
                        {
                            //Prüfung 1 mit mind. 2 Positonen wurde entfernt!
                            
                            //2. In den FA, die zu der Überposition gehören, gibt es mindesten 1 Ressourcen mit por.CustomProductCategory = '685' 
                            /*if(customerOrder.Any(x=>x.ProductionOrdersResources.Any(pr=>pr.CustomProductCategory== "685")))                           
                            {*/
                                // FA zur Überposition ermitteln - dient als Headerinformation
                                var productionOrder = unitOfWork.ProductionOrderRepository
                                                        .GetFirstOrDefault(x => x.CustomerOrderCode == customerOrder.Key.CustomerOrderCode
                                                                                && x.CustomParentPosition == customerOrder.Key.ParentPosition);
                                                                                
                                if (productionOrder != null)
                                {
                                    logger.Info($"FA zu Überposition {customerOrder.Key.CustomerOrderCode} / {customerOrder.Key.ParentPosition} ermittelt -> {productionOrder.Code}");
                                    
                                    // Report-Binding erstellen
                                    CustReportBinding reportBinding = (binaryFromReportMethods as BinaryFromReportMethods)
                                                                        .CreateReportBinding(unitOfWork, 
                                                                                            report, 
                                                                                            productionOrder.Sequence,   // ReportSequence 
                                                                                            planningGroup, 
                                                                                            workOrder.WorkOrder, 
                                                                                            string.Empty,               // VirtualCartCode
                                                                                            string.Empty,               // OptimizationCode
                                                                                            (DateTime)workOrder.DateWorkOrder,
                                                                                            "CreateAssemblyManualWorkOrder",                                                                                        
                                                                                            productionOrder.Code,
                                                                                            null,                       // ProductionItemCode
                                                                                            null,                       // CustomerOrderCode
                                                                                            null,                       // CustomerORderPosition
                                                                                            productionOrder.CustomTrainStation,
                                                                                            productionOrder.CustomTrainStationDescription
                                                                                            );
                                                                                
                                    // Kundenauftrag / -position an erzeugtem Report-Binding ergänzen
                                    if (reportBinding != null)
                                    {
                                        reportBinding.CustomerOrderCode = customerOrder.Key.CustomerOrderCode;
                                        reportBinding.CustomerOrderPosition = customerOrder.Key.ParentPosition;
                                                                        
                                        unitOfWork.Save();
                                        
                                        logger.Info($"ReportBinding zu WorkOrder {workOrder.WorkOrder} CustomerOrder {customerOrder.Key.CustomerOrderCode} / {customerOrder.Key.ParentPosition} angelegt (Sequence {reportBinding.Sequence.ToString()})");
                                        
                                        CreateAssemblyManual(logger, unitOfWork, reportBinding);
                                    }
                                }
                            /*}
                            else
                            {
                                logger.Info($"Für die Positionen des Kundenauftrag {customerOrder.Key.CustomerOrderCode} und der"
                                + $" Überposistion {customerOrder.Key.ParentPosition} wurden keine Ressourcen 685 gefunden!");
                            }*/
                        }
                    }                    
                }
            }
        }
    
    }
    
    
    public void CreateAssemblyManual(Logger logger, IUnitOfWork unitOfWork, CustReportBinding reportBinding)
    {
        List<AssemblyManualDocument> listOfDocuments = new List<AssemblyManualDocument>(); // Auflistung der zu mergenden PDF-Dokumente
        List<MissingDocument> listOfMissingDocuments = new List<MissingDocument>(); // Auflistung der fehlenden PDF-Dokumente - zum Fehlerreporting benötigt

        listOfDocuments.Clear();      
        listOfMissingDocuments.Clear();
        
        if (reportBinding != null)
        {
            // Sprache auf Basis ReportSequence und ReportEntity ermitteln
            string documentLanguage = string.Empty;
            
            var productionOrder = unitOfWork.ProductionOrderRepository.GetFirstOrDefault(x => x.Sequence == reportBinding.ReportSequence);
                        
            if (productionOrder != null)
            {
                //Defalut Satz in Tabelle mit InputValue # sollte immer vorhanden sein und auf DEU gemappt sein
                var countryTranslationInputValue = "#";
                
                if(!String.IsNullOrEmpty(productionOrder.CustomCountry))
                {
                    countryTranslationInputValue = productionOrder.CustomCountry;
                }

                //Mapping der Länderkeinnzeichen über die Tabelle cust.CountryTranslation
                var custCountryTranslation = unitOfWork.GetRepository<CustCountryTranslation>()
                                    .GetFirstOrDefault(x=>x.InputValue==countryTranslationInputValue);
                                    
                if(custCountryTranslation!=null)
                {
                    logger.Info($"CustCountryTranslation OutputValue: {custCountryTranslation.OutputValue}");  
                    
                    //Da in DEU kein Suffix an der Datei vergeben ist kann das ignoriert werden!
                    if(custCountryTranslation.OutputValue!="DEU")
                    {
                        documentLanguage = custCountryTranslation.OutputValue;
                    }
                }
                else
                {
                    logger.Warn($"Kein Eintrag in CustCountryTranslation, InputValue: {countryTranslationInputValue}");  
                }
            }
            else
            {
                logger.Warn($"Binary Report Montageanleitung keine FA mit Sequence: {reportBinding.ReportSequence}");  
            }
        
            string customerOrderCode = reportBinding.CustomerOrderCode;
            string customerOrderParentPosition = reportBinding.CustomerOrderPosition;
            
            logger.Info($"Binary Report Montageanleitung {customerOrderCode} / {customerOrderParentPosition}");  
            
            // Ressourcen zu Kundenauftrag / -position ermitteln
            var resources = GetResourcesToCustomerOrderPosition(logger, 
                                                                unitOfWork, 
                                                                customerOrderCode, 
                                                                customerOrderParentPosition);
            if (resources.Any())
            {   
                foreach (var resource in resources)
                {
                    string documentName = resource.CustomArticleNumber; 
                    
                    //Dokumente starten imm mit "A", hier entsprechend ergänzen wenn nicht vorhanden
                    if (!documentName.StartsWith("A"))
                    {
                        documentName = "A" + documentName;
                    }
                
                    string documentNameWithoutLanguage = documentName;
                     
                    //Ggf. Sprache als Suffix ergänzen
                    if(!String.IsNullOrEmpty(documentLanguage))
                    {
                        documentName = documentName+documentLanguage;
                    }
                    
                    logger.Info($"Gebildeter Dokumentenname: {documentName}");
                    
                    string pdfFileNameSource = GetPdfFileNameForArticle(logger, documentName); 
                    
                    //Zusätzlich Prüfungen Landerkennzeichen
				    //Wenn nicht, dann Fehlermeldung
                    if (string.IsNullOrEmpty(pdfFileNameSource) && !String.IsNullOrEmpty(documentLanguage))
                    {
                        logger.Info($"Kein Dokument gefunden mit: {documentName}");    

                        //Wenn PDF mit LK nicht vorhanden, dann prüfe ob PDF mit Sprache EN vorhanden ist
                        pdfFileNameSource = GetPdfFileNameForArticle(logger, documentNameWithoutLanguage + "EN");

                        //Wenn PDF mit EN nicht vorhanden, dann prüfe ob PDF ohne LK vorhanden ist
                        if (string.IsNullOrEmpty(pdfFileNameSource))
                        {
                            logger.Info($"Kein Dokument gefunden mit: {documentNameWithoutLanguage}+EN");

                            pdfFileNameSource = GetPdfFileNameForArticle(logger, documentNameWithoutLanguage);
                        }
                    }
                    
                    if (!string.IsNullOrEmpty(pdfFileNameSource))
                    {
                        logger.Info($"PDF-Dokument {pdfFileNameSource} vorhanden");
                        
                        listOfDocuments.Add( new AssemblyManualDocument()
                                                {
                                                    ArticleNumber =  resource.CustomArticleNumber,
                                                    DocumentName =  pdfFileNameSource,
                                                    Priority = resource.CustomAddArticleDescription3
                                                } );
                    }
                    else
                    {
                        // Wenn Dokument nicht gefunden werden kann zum Fehlerreporting hinzufügen
                        listOfMissingDocuments.Add( new MissingDocument() 
                                                    { 
                                                        ArticleNumber =  resource.CustomArticleNumber,
                                                        DocumentName =  documentName,
                                                        CustomerOrderCode = customerOrderCode,
                                                        CustomerOrderPosition = resource.ProductionOrder.CustomerOrderPosition,
                                                        CustomerOrderParentPosition = customerOrderParentPosition
                                                    } );
                        
                    }
                }
                
                // Einbindung Vertriebsdokument
                // neuer Weg - Vertriebsdokument zu KA + KA-Überposition suchen
                string salesDocumentName = string.Empty;
                string salesDocumentFileNameSource = string.Empty;
                
                var salesDocumentProductionOrder = unitOfWork.GetRepository<HomagGroup.FLS.Domain.Data.ProductionOrder>()
                                                    .GetFirstOrDefault(x => x.CustomerOrderCode == customerOrderCode
                                                                        && x.CustomerOrderPosition == customerOrderParentPosition);
                                                                        
                if (salesDocumentProductionOrder != null)
                {  
                    salesDocumentName = customerOrderCode + customerOrderParentPosition.PadLeft(3, '0'); // KA-Pos 3-stellig mit 0 auffüllen
                    salesDocumentFileNameSource = string.Format(CultureInfo.InvariantCulture, @"{0}\{1}.{2}", _PathService.GetPath("AssemblyManualSalesDocument"), salesDocumentName, "pdf");
                    
                    logger.Info($"Suche Vertriebsdokument zu KA {customerOrderCode} ÜPos. {customerOrderParentPosition}");
                    
                    if (File.Exists(salesDocumentFileNameSource))
                    {
                        logger.Info($"Vertriebsdokument {salesDocumentFileNameSource} zu KA {customerOrderCode} ÜPos. {customerOrderParentPosition} gefunden");
                    
                        listOfDocuments.Add( new AssemblyManualDocument()
                                                {
                                                    ArticleNumber =  salesDocumentProductionOrder.ArticleNumber,
                                                    DocumentName =  salesDocumentFileNameSource,
                                                    Priority = "C"
                                                } );
                    }
                    else
                    {
                        logger.Info($"KEIN Vertriebsdokument {salesDocumentFileNameSource} zu KA {customerOrderCode} ÜPos. {customerOrderParentPosition} gefunden");
                        
                    }
                }
                else
                {
                    logger.Warn($"FA zu KA {customerOrderCode} ÜPos. {customerOrderParentPosition} nicht gefunden!");
                }
                
                /*
                // alter Weg - Vertriebsdokument zu explizitem Artikel suchen
                var salesDocumentProductionOrders = unitOfWork.GetRepository<HomagGroup.FLS.Domain.Data.ProductionOrder>()
                                                    .Get(x => x.CustomerOrderCode == customerOrderCode
                                                            && x.CustomParentPosition == customerOrderParentPosition
                                                            && x.ArticleNumber == "202010");
                                                                        
                if (salesDocumentProductionOrders.Any())
                {                
                    string salesDocumentName = string.Empty;
                    string salesDocumentFileNameSource = string.Empty;
                    
                    foreach (var salesDocumentProductionOrder in salesDocumentProductionOrders)
                    {
                        logger.Info($"Artikel 202010 - FA {salesDocumentProductionOrder.Code} mit Vertriebsdokument zu KA {customerOrderCode} ÜPos. {customerOrderParentPosition} gefunden");
                        
                        salesDocumentName = customerOrderCode + salesDocumentProductionOrder.CustomerOrderPosition.PadLeft(3, '0'); // KA-Pos 3-stellig mit 0 auffüllen
                        salesDocumentFileNameSource = string.Format(CultureInfo.InvariantCulture, @"{0}\{1}.{2}", _PathService.GetPath("AssemblyManualSalesDocument"), salesDocumentName, "pdf");
                        
                        if (File.Exists(salesDocumentFileNameSource))
                        {
                            listOfDocuments.Add( new AssemblyManualDocument()
                                                    {
                                                        ArticleNumber =  salesDocumentProductionOrder.ArticleNumber,
                                                        DocumentName =  salesDocumentFileNameSource,
                                                        Priority = "C"
                                                    } );
                        }
                        else
                        {
                            logger.Warn($"Vertriebsdokument zu FA {salesDocumentProductionOrder.Code} KA {customerOrderCode} ÜPos. {customerOrderParentPosition} nicht gefunden - {salesDocumentFileNameSource}");
                            
                            // Wenn Dokument nicht gefunden werden kann zum Fehlerreporting hinzufügen
                            listOfMissingDocuments.Add( new MissingDocument() 
                                                        { 
                                                            ArticleNumber =  salesDocumentProductionOrder.ArticleNumber,
                                                            DocumentName =  salesDocumentName,
                                                            CustomerOrderCode = customerOrderCode,
                                                            CustomerOrderPosition = salesDocumentProductionOrder.CustomerOrderPosition,
                                                            CustomerOrderParentPosition = customerOrderParentPosition
                                                        } );
                            
                        }
                    }
                }*/
                // --- Ende Vertriebsdokument
                
                // Handling nicht gefundener Dateien
                if (listOfMissingDocuments.Any())
                {
                   MissingDocumentProcess(logger, unitOfWork, listOfMissingDocuments, reportBinding);
                }
                
                // Beauftragung von MergePDF
                if (listOfDocuments.Any() && !listOfMissingDocuments.Any())
                {
                    AssignMergePdf(logger, unitOfWork, listOfDocuments, reportBinding, customerOrderCode, customerOrderParentPosition);
                }
                
                
            } 
            else      
            {
                logger.Warn($"Keine Ressourcen zu CustomerOrder {customerOrderCode} / {customerOrderParentPosition} vorhanden!");
            }
        }
    }
    
    
    
    // Ressourcen zu Kundenauftrag / -position ermitteln
    private IEnumerable<HomagGroup.FLS.Domain.Data.ProductionOrdersResource> GetResourcesToCustomerOrderPosition(Logger logger, 
                                                                                                                    IUnitOfWork unitOfWork, 
                                                                                                                    string customerOrderCode,
                                                                                                                    string parentPosition)
    {
        IEnumerable<HomagGroup.FLS.Domain.Data.ProductionOrdersResource> productionOrdersResources = null; 
        
        if (!string.IsNullOrEmpty(customerOrderCode) && !string.IsNullOrEmpty(parentPosition))
        {
            productionOrdersResources = unitOfWork.ProductionOrdersResourceRepository
                                                .Get(x => x.ProductionOrder.CustomerOrderCode == customerOrderCode
                                                        && x.ProductionOrder.CustomParentPosition == parentPosition
                                                        && x.CustomProductCategory == "685")
                                                .OrderBy(o => o.CustomAddArticleDescription3)
                                                    .ThenByDescending(od => od.CustomArticleNumber)
                                                .DistinctBy(x => x.CustomArticleNumber);
                                
            logger.Info($"{productionOrdersResources.Count().ToString()} Ressourcen zu CustomerOrder {customerOrderCode} / {parentPosition} ermittelt");
        }
        
        return productionOrdersResources;
    }
    
    // Dokument zu Artikel suchen und mit Pfad zurückgeben
    private string GetPdfFileNameForArticle(Logger logger, string articleNumber)
    {
        // in PlaningDocs suchen
        // wenn nichts gefunden in DrawDocs suchen
        
        string pdfFileNamePath = string.Empty;
        
        //_PathService.GetPath("PathPlaningDocs")
        //_PathService.GetPath("PathDrawDocs")
        string subDirectory = GetDrawingNumberPath(articleNumber.PadLeft(6, '0'));
        
        pdfFileNamePath = string.Format(CultureInfo.InvariantCulture, @"{0}\{1}\{2}.{3}", _PathService.GetPath("PathPlaningDocs"), subDirectory, articleNumber.PadLeft(6, '0'), "pdf");
        
        logger.Info($"PDF zu Artikel {articleNumber} suchen --> {pdfFileNamePath}");
        
        if (!File.Exists(pdfFileNamePath))
        {
            pdfFileNamePath = string.Format(CultureInfo.InvariantCulture, @"{0}\{1}\{2}.{3}", _PathService.GetPath("PathDrawDocs"), subDirectory, articleNumber.PadLeft(6, '0'), "pdf");
            
            logger.Info($"PDF zu Artikel {articleNumber} suchen --> {pdfFileNamePath}");
            
            if (!File.Exists(pdfFileNamePath))
            {
                pdfFileNamePath = string.Empty;
                
                logger.Warn($"Kein PDF-Dokument zu Artikel {articleNumber} vorhanden!");
            }            
        }
        
        return pdfFileNamePath;
    }
    
    
    // Beauftragung von MergePDF
    private void AssignMergePdf(Logger logger, 
                                IUnitOfWork unitOfWork, 
                                List<AssemblyManualDocument> listOfDocuments, 
                                CustReportBinding reportBinding,
                                string customerOrderCode,
                                string parentPosition)
    {
        if (listOfDocuments.Any())
        {
            Int64 mergePdfSequence = 0; // Referenz zu MergePDF-Beauftragung in ReportBinding
            string mergePdfDescription = customerOrderCode + "/" + parentPosition;
            
            // Liste der zu mergenden PDF-Dokumente zur Übergabe an MergePDF aufbereiten
            List<string> listOfComponents = new List<string>(); // Auflistung der zu mergenden PDF-Dokumente
            listOfComponents.Clear(); 
            
            foreach (var document in listOfDocuments.OrderBy(o => o.Priority)
                                                .ThenByDescending(od => od.ArticleNumber))
            {
                listOfComponents.Add(document.DocumentName);
            }
            
            // nur ein Dokument für Mergevorgang vorhanden?
            //  --> Leerseite als 2. Dokument anfügen um Mergevorgang zu ermöglichen
            if (listOfComponents.Count() == 1)
            {
                logger.Info($"Leerseite für Montageanleitung {mergePdfDescription} benötigt");
                
                var programSetting = unitOfWork.GetRepository<ProgramSetting>()
                                        .GetFirstOrDefault(x => x.Identifier == "EmptyPageForMergePDF");
                                        
                if (programSetting != null)
                {
                    listOfComponents.Add(programSetting.ValueString);
                    
                    logger.Info($"Leerseite {programSetting.ValueString} zu Montageanleitung {mergePdfDescription} hinzugefügt");
                }
                else
                {
                    logger.Warn($"ProgramSetting [EmptyPageForMergePDF] für Leerseite nicht vorhanden - bitte Konfiguration anlegen!");
                }
            }
            
            
            
            mergePdfSequence = (mergePdfMethods as MergePdfMethods)
                            .CreateMergePdfJob(logger, unitOfWork, mergePdfDescription, listOfComponents, 2);
                            
            // Sequence der MergePDF-Beauftragung an ReportBinding speichern, um nach PDF-Merge referenzieren zu können
            if (reportBinding != null)
            {   
                reportBinding.MergePDFSequence = mergePdfSequence;
                
                unitOfWork.Save();
                
                logger.Info($"MergePDFSequence {mergePdfSequence.ToString()} an ReportBinding Sequence {reportBinding.Sequence.ToString()} ergänzt");
            }
        }
    }
    
    // Handling fehlender Dokumente 
    private void MissingDocumentProcess(Logger logger, IUnitOfWork unitOfWork, List<MissingDocument> listOfMissingDocuments, CustReportBinding reportBinding)
    {
        if (listOfMissingDocuments.Any())
        {                        
            string errorDescription = string.Empty;
            List<string>listOfMergePdfErrors = new List<string>();
            listOfMergePdfErrors.Clear();
            
            int order = 0;
            
            foreach (var missingDocument in listOfMissingDocuments)
            {
                //tmpMessage = $"Fehlendes Dokument {missingDocument.DrawingNumber} zu Artikel {missingDocument.ArticleNumber} aus Kundenauftrag {missingDocument.CustomerOrderCode} Pos. {missingDocument.CustomerOrderPosition}";
                errorDescription = String.Format("Fehlendes Dokument {0} zu Artikel {1} aus Kundenauftrag {2} Pos. {3}" + System.Environment.NewLine+ System.Environment.NewLine, 
                                                    missingDocument.DocumentName,
                                                    missingDocument.ArticleNumber,
                                                    missingDocument.CustomerOrderCode,
                                                    missingDocument.CustomerOrderPosition);
                
                
                logger.Warn(errorDescription);
                
                listOfMergePdfErrors.Add(errorDescription);
            }
            
            // Fehler-PDF erstellen
            CreateErrorReportPdf(logger, unitOfWork, listOfMergePdfErrors, reportBinding);
            
            //logger.Info($"Zeichenlänge Message für Mail: {message.ToArray().Length.ToString()}");
            /*
            if (message.Any())
            {
                string messageString = string.Join(" ", message.ToArray());
                logger.Info(messageString);
                
                var sendMail = new CustSendMail();
                sendMail.Mailadress = "Patrik.Augustin@homag.com";
                sendMail.Subject = $"Montageanleitung - Fehlende Dokumente zu Kundenauftrag {listOfMissingDocuments.FirstOrDefault().CustomerOrderCode}/{listOfMissingDocuments.FirstOrDefault().CustomerOrderParentPosition}";
                sendMail.Message = messageString.Left(1023);
                sendMail.TransferState = 10;
                sendMail.CreationSource = "MissingDocumentProcess";
                
                unitOfWork.AddOrUpdate(new []{sendMail});
                unitOfWork.Save();
            }*/
        }
    }
    
    public void CreateErrorReportPdf(Logger logger, IUnitOfWork unitOfWork, List<string> listOfMergePdfErrors, CustReportBinding reportBinding)
    {
        if (reportBinding != null && listOfMergePdfErrors.Any())
        {
            // Error-Datensätze erzeugen
            (mergePdfMethods as MergePdfMethods).CreateMergePDFErrorRecord(logger, unitOfWork, listOfMergePdfErrors, reportBinding);
                
            // Druckbeauftragung
            var report = (binaryFromReportMethods as BinaryFromReportMethods).GetReport(unitOfWork, "MergePDFError");
            if (report != null)
            {
                (binaryFromReportMethods as BinaryFromReportMethods).CreatePrintJob(reportBinding, report);
            }
        }
    }
    
    // Planungsgruppe zu Werkauftrag über beliebigen FA aus Werkauftrag ermitteln
    // -- Duplikat zu Methode in CommonHelperMethods da GlobalCustomization nicht in ServerCustomization eingebunden werden kann
    public string GetPlanningGroupFromWorkOrder(Logger logger, IUnitOfWork unitOfWork, string workOrder)
    {
        string returnValue = string.Empty;
            
        var planningGroup = unitOfWork.GetRepository<HomagGroup.FLS.Domain.Data.ProductionOrder>()
                                    .Get(x => x.CustomWorkOrder == workOrder)
                                    .Select(x => new { PlanningGroup = x.CustomPlanningGroup })
                                    .Take(1);
                                    
        if (planningGroup.Any())
        {
            returnValue = planningGroup.FirstOrDefault().PlanningGroup;
        }
        else
        {
            // Wenn kein Werkauftrag zu Code gefunden werden kann, versuchen zu Code einen FA zu finden
            // relevant bei Serienaufträgen 
            planningGroup = unitOfWork.GetRepository<HomagGroup.FLS.Domain.Data.ProductionOrder>()
                                        .Get(x => x.Code == workOrder)
                                        .Select(x => new { PlanningGroup = x.CustomPlanningGroup })
                                        .Take(1);
                                        
            if (planningGroup.Any())
            {
                returnValue = planningGroup.FirstOrDefault().PlanningGroup;
            }
        }
        
        return returnValue; 
    }
    
    
    // Teilpfad von Unterverzeichnis zu Zeichnungsnr. ermitteln
    public string GetDrawingNumberPath(string programFileName)
    {
        return programFileName.Substring(0,2) + "0XXX";
    }
    
    /*
    // Email-Versand
    public void SendErrorReportMail(Logger logger)
    {
        string mailRecipient = "Patrik.Augustin@homag.com"; 
        List<string> to = new List<string>();
        List<string> cc = new List<string>();
        List<string> mailValue =  new List<string>();
        List<string> mailAttachment =  new List<string>();
        string mailSubject = String.Empty;
        
        to.Clear();
        cc.Clear();
        mailValue.Clear();
        mailAttachment.Clear();
        
        to.Add(mailRecipient);
        mailSubject = $"Montageanleitung - Fehlende Dokumente zu Kundenauftrag {listOfMissingDocuments.FirstOrDefault().CustomerOrderCode}/{listOfMissingDocuments.FirstOrDefault().CustomerOrderParentPosition}";
        
        //Trennzeichen zur Zeilenumbruch ersezten
        //mailValue.Add(sendMail.Message);
        
        //Anhang an Mail
        //mailAttachment.Add();
        
        bool result = (sendMailFromMES as SendMailFromMES).SendMail(logger, to, cc, mailSubject, mailValue, mailAttachment);
        if (result == true)
        {
            //logger.Info(String.Format("Mail gesendet an [{0}]; Betreff [{1}]",sendMail.Mailadress,sendMail.Subject));
            
            //countSendMails++;
        }
    }*/
}

public class AssemblyManualDocument
{
    public string ArticleNumber { get; set; }
    public string DocumentName { get; set; }
    public string Priority { get; set; }
}

public class MissingDocument
{
    public string ArticleNumber { get; set; }
    public string DocumentName { get; set; }
    public string CustomerOrderCode { get; set; }
    public string CustomerOrderPosition { get; set; }
    public string CustomerOrderParentPosition { get; set; }
}


