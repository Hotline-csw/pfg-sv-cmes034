//-----------------------------------------------------------------------------
//   (Class-)Name:   BinaryFromReportMethods
//
//   Description:    Konfi für Druckerzentrale auf der PFG-TestVM
//                      S.Feist
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         <Author>
//   Date:           2022-01-19
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   <Author>        2022-01-19    Created
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IGlobalCustomization))]
[Export("BinaryFromReportMethods", typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IGlobalCustomization))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("Methoden um im Hintergrund Binary aus Report zu erzeugen")]
[EnabledScript(false)]
public class BinaryFromReportMethods : UserExitCustomBase, HomagGroup.FLS.Infrastructure.Common.Customization.IGlobalCustomization
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;
    
    

    // Report-Binding inkl. Binary anlegen
    public void CreateReportBindingBinary(  
											string reportLayout, 
                                            Int64 reportSequence, 
                                            //string planningGroup, 
                                            string workOrder, 
                                            //string virtualCartCode, 
                                            string optimizationCode, 
                                            string productionOrderCode, 
                                            string productionItemCode, 
                                            //DateTime dateReportRelevance,
											//string trainStation = null,
											//string trainStationDescription = null,
											string customerOrderCode = null,
											string customerOrderPosition = null
											//string capacitySequenceCode = null,
											//string capacitySequenceDescription = null,
											//string taggedLinesGroupName = null,
											//string reportField01 = null,
											//string reportField02 = null
											)
    {
        using(var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
		{
            var report = GetReport(unitOfWork, reportLayout);
                                        
            if (report != null)
            {            
                
                //if (IsReportDefinedForPlanningGroup(unitOfWork, reportLayout, planningGroup))
                //{      
                    // Report-Binding erzeugen / aktualisieren
                    var reportBinding = CreateReportBinding(
															unitOfWork, 
                                                            report, 
                                                            reportSequence, 
                                                            //planningGroup, 
                                                            workOrder, 
                                                            // virtualCartCode,
                                                            optimizationCode, 
                                                            // dateReportRelevance,
                                                            "CreateReportBindingBinary",
                                                            productionOrderCode,
                                                            productionItemCode,
                                                            customerOrderCode, 
                                                            customerOrderPosition 
                                                            // trainStation, 
                                                            // trainStationDescription,
                                                            // capacitySequenceCode,
                                                            // capacitySequenceDescription,
                                                            // taggedLinesGroupName,
                                                            // reportField01,
                                                            // reportField02
															);
                    
                    // Binary erzeugen und an Report-Binding speichern
                    if (reportBinding != null)
                    {
                        CreatePrintJob(reportBinding, report);
                    }
                    
                    unitOfWork.Save();
                //}
            }
        }
    }
    
    
    // Report-Binding anlegen und Binary aus PDF-Import erzeugen
    public void CreateReportBindingImportPdf(Logger logger, string reportLayout, Int64 reportSequence, /*string planningGroup,*/ string workOrder, string virtualCartCode, string optimizationCode /* DateTime dateReportRelevance*/, string file)
    {
        if (File.Exists(file))
        {
            logger.Info($"CreateReportBindingImportPdf File {file}");
            using(var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
    		{
                var report = GetReport(unitOfWork, reportLayout);
                                            
                if (report != null)
                {                   
                    // Report-Binding erzeugen / aktualisieren
                    var reportBinding = CreateReportBinding(unitOfWork, 
                                                            report, 
                                                            reportSequence, 
                                                            //planningGroup, 
                                                            workOrder, 
                                                            //string.Empty,    // VirtualCartCode 
                                                            optimizationCode, 
                                                            // dateReportRelevance, 
                                                            "CreateReportBindingBinary",
                                                            string.Empty,   // ProductionOrderCode
                                                            string.Empty,   // ProductionItemCode
                                                            string.Empty,   // CustomerOrderCode
                                                            string.Empty   // CustomerOrderPosition
                                                            // string.Empty,   // TrainStation
                                                            // string.Empty    // TrainStationDescription
                                                            );
                    
                    // Binary erzeugen und an Report-Binding speichern
                    if (reportBinding != null)
                    {
                        //reportBinding.DateReportRelevance = 
                        // erzeugte Binary-Sequence an ReportBinding speichern
                        BindBinaryToReportBinding(logger, reportSequence, report.Layout, ImportPdfInBinaries(logger, unitOfWork, file));
                        
                    }
                    
                    unitOfWork.Save();
                }
            }
        }
        else
        {
            logger.Error($"ERROR: BinaryFromReportMethods.CreateReportBindingImportPdf - File not found [{file}]");
        }
    }
    
    
    // Report-Bindung erstellen / aktualisieren
    public CustReportBinding CreateReportBinding(
													IUnitOfWorkBase unitOfWork, 
                                                    CustReport report, 
                                                    Int64 reportSequence, 
                                                    //string planningGroup, 
                                                    string workOrder, 
                                                    //string virtualCartCode, 
                                                    string optimizationCode,
                                                    //DateTime dateReportRelevance,
                                                    string creationSource,
                                                    string productionOrderCode = null,
                                                    string productionItemCode = null,
                                                    string customerOrderCode = null,
													string customerOrderPosition = null
													// string trainStation = null,
													// string trainStationDescription = null,
        											// string capacitySequenceCode = null,
        											// string capacitySequenceDescription = null,
        											// string taggedLinesGroupName = null,
        											// string reportField01 = null,
        											// string reportField02 = null
													)
    {
        
        var reportBinding = new CustReportBinding();
        
        var existingReportBinding = unitOfWork.GetRepository<CustReportBinding>()
                                        .GetFirstOrDefault(x => x.ReportLayout == report.Layout
                                                            && x.ReportEntity == report.Entity
                                                            && x.ReportSequence == reportSequence);
                                                            
        if (existingReportBinding != null)
        {
            // vorhandenes ReportBinding updaten
            reportBinding = existingReportBinding;
            
            //reportBinding.DateReportRelevance = dateReportRelevance;
            //reportBinding.PlanningGroup = planningGroup;
            reportBinding.WorkOrder = workOrder;
            reportBinding.ProductionOrderCode = productionOrderCode;
            reportBinding.ProductionItemCode = productionItemCode;
            reportBinding.CustomerOrderCode = customerOrderCode;
			reportBinding.CustomerOrderPosition = customerOrderPosition;
			// reportBinding.TrainStation = trainStation;
			// reportBinding.TrainStationDescription = trainStationDescription;
			// reportBinding.CapacitySequenceCode = capacitySequenceCode;
			// reportBinding.CapacitySequenceDescription = capacitySequenceDescription;
            reportBinding.OptimizationCode = optimizationCode;
            // reportBinding.VirtualCartCode = virtualCartCode;
            // reportBinding.TaggedLinesGroupName = taggedLinesGroupName;
            // reportBinding.ReportField01 = reportField01;
            // reportBinding.ReportField02 = reportField02;
            reportBinding.BinariesSequence = 0;
            reportBinding.ModificationSource = creationSource;
        }
        else
        {
            // neues ReportBinding anlegen
            
            //reportBinding.DateReportRelevance = dateReportRelevance;
            //reportBinding.PlanningGroup = planningGroup;
            reportBinding.WorkOrder = workOrder;
            reportBinding.ProductionOrderCode = productionOrderCode;
            reportBinding.ProductionItemCode = productionItemCode;
            reportBinding.CustomerOrderCode = customerOrderCode;
			reportBinding.CustomerOrderPosition = customerOrderPosition;
			// reportBinding.TrainStation = trainStation;
			// reportBinding.CapacitySequenceCode = capacitySequenceCode;
			// reportBinding.CapacitySequenceDescription = capacitySequenceDescription;
			// reportBinding.TrainStationDescription = trainStationDescription;
            reportBinding.OptimizationCode = optimizationCode;
            // reportBinding.VirtualCartCode = virtualCartCode;
            // reportBinding.TaggedLinesGroupName = taggedLinesGroupName;
            // reportBinding.ReportField01 = reportField01;
            // reportBinding.ReportField02 = reportField02;
            reportBinding.ReportLayout = report.Layout;
            reportBinding.ReportEntity = report.Entity;
            reportBinding.ReportSequence = reportSequence;
            reportBinding.CreationSource = creationSource;
            
            unitOfWork.AddOrUpdate(new[] {reportBinding}); 
        }
		                        
        
     	return reportBinding;
    }
    
    public void CreatePrintJob(CustReportBinding reportBinding, CustReport report)
    {
        using(var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
		{		
            //PrintJob erstellen
            var printJob = new CustPrintJob();
            
            printJob.ProductionItemCode = reportBinding.ProductionItemCode;
            printJob.ProductionOrderCode = reportBinding.ProductionOrderCode;
            printJob.CustomerOrderCode = reportBinding.CustomerOrderCode;
            printJob.CustomerOrderPosition = reportBinding.CustomerOrderPosition;
            //printJob.VirtualCartId = reportBinding.VirtualCartCode;
            printJob.WorkOrder = reportBinding.WorkOrder;
            printJob.OptimizationCode = reportBinding.OptimizationCode;
            printJob.TransferState = 10;
            printJob.JobName = report.JobName;
            printJob.Quantity = 1;
            //printJob.ReportBindingsSequence = reportBinding.ReportSequence;
            //printJob.ReportField01 = reportBinding.ReportField01;
            //printJob.ReportField02 = reportBinding.ReportField02;
            
            if (report.JobName == "PrintPDFMergePDFError")
            {
                printJob.CreationSource = reportBinding.ReportLayout;
            }
            else
            {
                printJob.CreationSource = "ReportBinding";
            }
            
            printJob.ModificationSource = "";
            
            unitOfWork.AddOrUpdate(new[] {printJob}); 
                   
            unitOfWork.Save();
                
        }
    }

/*
    public bool IsReportDefinedForPlanningGroup(IUnitOfWorkBase unitOfWork, string reportName, string planningGroup)
    {
        var planningGroupReport = unitOfWork.GetRepository<CustPlanningGroupReport>()
                                    .GetFirstOrDefault(x => x.PlanningGroup == planningGroup
                                                            && x.Layout == reportName);
                                                            
        if (planningGroupReport != null)
            return true;
            
        return false;
    }
*/

/*
    // erzeugtes Binary an CartPlanning speichern
    public void BindBinaryToCartPlanning(Logger logger, string virtualCartCode, Binary binary)
    {
        using(var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
		{
		  var cartPlanning = unitOfWork.GetRepository<CustCartPlanning>()
		                          .GetFirstOrDefault(x => x.VirtualCartCode == virtualCartCode);
		                          
          cartPlanning.BinariesSequence = binary.Sequence;
          
                    
          unitOfWork.Save();
          
          logger.Info($"Binary[{binary.Sequence}] an CartPlanning virtueller Wagen [{cartPlanning.VirtualCartCode}] gespeichert");
		}
        
    }
*/
  
    // erzeugtes Binary an ReportBinding speichern
    public void BindBinaryToReportBinding(Logger logger, long reportSequence, string reportLayout, Binary binary)
    {
        if (binary != null)
        {
            using(var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
    		{
                var reportBinding = unitOfWork.GetRepository<CustReportBinding>()
                                      .GetFirstOrDefault(x => x.ReportSequence == reportSequence
                                                            && x.ReportLayout == reportLayout);
                              
                if (reportBinding != null)
                {
                    reportBinding.BinariesSequence = binary.Sequence;
                
                    unitOfWork.Save();
                    
                    logger.Info($"Binary[{binary.Sequence}] an ReportBinding zu ReportSequence [{reportSequence}] gespeichert");
                }
                else
                {
                    logger.Warn($"ReportBinding zu ReportSequence [{reportSequence}] / ReportLayout {reportLayout} nicht gefunden");
                }
                
    		}
        }
        else
        {
            logger.Error($"ERROR: BinaryFromReportMethods.BinaryFromReportMethods - Kein Binary-Objekt übergeben / übergebenes Binary-Objekt ist leer.");
        }
    }
    
    
    public string GetReportLayoutToJobname(Logger logger, IUnitOfWorkBase unitOfWork, string reportingJobName)
    {
        string returnValue = string.Empty;
        
        var report = unitOfWork.GetRepository<CustReport>()
                        .GetFirstOrDefault(x => x.JobName == reportingJobName);
        
        if (report != null)
        {
            returnValue = report.Layout;
        }
        
        
        return returnValue;
    }

    // Objekt zu Report laden
    public CustReport GetReport(IUnitOfWorkBase unitOfWork, string reportLayout)
    {
        
        return unitOfWork.GetRepository<CustReport>().GetFirstOrDefault(x => x.Layout == reportLayout);
    
    }
    
    
    // PDF in Binaries importieren
    private Binary ImportPdfInBinaries(Logger logger, IUnitOfWorkBase unitOfWork, string file)
    {       
        Binary returnValue = null;    
        try
        {
            
            if (File.Exists(file))
            {
                //Prüfen ob es den Binary schon gibt!
                var existingBinary = unitOfWork.GetRepository<Binary>().GetFirstOrDefault(x => x.OriginalPath == file);
                
                if(existingBinary!=null)
                {
                    //Update Binary
                    
                    existingBinary.Data = File.ReadAllBytes(file);
                    
                    unitOfWork.Save();
                    
                    returnValue = existingBinary;
                }
                else
                {
                    // neues Binary anlegen
                    
                    var binary = new Binary();
                    //picture.BinaryType = BinaryType.Image; // 1
                    binary.BinaryType = BinaryType.PDF;
                    binary.Data = File.ReadAllBytes(file);
                    binary.OriginalPath = file;
                    
                    unitOfWork.AddOrUpdate(new[] { binary });
                    
                    unitOfWork.Save();
                    
                    returnValue = binary;
                }        
            }
            else
            {
                logger.Error($"ERROR: BinaryFromReportMethods.ImportPdfInBinaries - File not found [{file}]");
            }
        }
        catch(Exception e)
        {
             logger.Error("ERROR: BinaryFromReportMethods.ImportPdfInBinaries", null, e);
        }
        
        return returnValue;
    }
    
    // Datum zu Werkauftrag ermitteln
    public DateTime GetDateWorkOrder (IUnitOfWork unitOfWork, string workOrder)
    {
        DateTime returnValue = new DateTime();
        
        try
        {
            if (!string.IsNullOrEmpty(workOrder))
            {
                var workOrderObject = unitOfWork.GetRepository<CustWorkOrder>()
                                        .GetFirstOrDefault(x => x.WorkOrder == workOrder);
                
                if (workOrderObject != null)
                {
                    returnValue = (DateTime)workOrderObject.DateWorkOrder;
                }
    
            }
        }
        catch (Exception e)
        {
            
        }
        
        return returnValue;
        
    }
    
    
    
    
    //+-- Beginn Beschlagbeutel ---
    public void CreateFittingListReports(   
											Logger logger, 
                                            IUnitOfWorkBase unitOfWork, 
                                            Int64 reportSequence, 
                                            //string planningGroup, 
                                            string workOrder 
                                            //DateTime dateReportRelevance
											)
    {
        // Binary Report Beschlagbeutel-Etikett
        var fittingLabelReports = unitOfWork.GetRepository<CustReport>()
                                    .Get(x => x.Layout.StartsWith("FittingLabel")
                                            && x.Layout != "FittingLabelBE");
        
        if (fittingLabelReports.Any())
        {
            foreach (var fittingLabelReport in fittingLabelReports)
            {
                //if (IsReportDefinedForPlanningGroup(unitOfWork, fittingLabelReport.Layout, planningGroup))
                //{
                    logger.Info($"Binary Report Beschlagbeutel-Etikett ({fittingLabelReport.Layout}) zu Werkauftrag {workOrder}");   
            
                    CreateReportBindingBinary(
													fittingLabelReport.Layout, 
                                                    reportSequence, 
                                                    //planningGroup, 
                                                    workOrder,
                                                    string.Empty,
                                                    string.Empty,
                                                    string.Empty, // ProductionOrderCode
                                                    string.Empty, // ProductionItemCode
                                                    // dateReportRelevance,
                                                    //string.Empty, // TrainStation
                                                    //string.Empty, // TrainStationDescription
                                                    string.Empty, // CustomerOrderCode
                                                    string.Empty  // CustomerOrderPosition
                                                    );
                //}
            }
        }
        
        // Binary Report Beschlagbeutel Kommissionierliste
        var fittingListReports = unitOfWork.GetRepository<CustReport>()
                                    .Get(x => x.Layout.StartsWith("FittingList"));
        
        if (fittingListReports.Any())
        {
            foreach (var fittingListReport in fittingListReports)
            {
                //if (IsReportDefinedForPlanningGroup(unitOfWork, fittingListReport.Layout, planningGroup))
                //{
                    logger.Info($"Binary Report Beschlagbeutel Kommissionierliste ({fittingListReport.Layout}) zu Werkauftrag {workOrder}");   
            
                    CreateReportBindingBinary(
													fittingListReport.Layout, 
                                                    reportSequence, 
                                                    //planningGroup, 
                                                    workOrder,
                                                    string.Empty,
                                                    string.Empty,
                                                    string.Empty, // ProductionOrderCode
                                                    string.Empty, // ProductionItemCode
                                                    // dateReportRelevance,
                                                    //string.Empty, // TrainStation
                                                    //string.Empty, // TrainStationDescription
                                                    string.Empty, // CustomerOrderCode
                                                    string.Empty  // CustomerOrderPosition
                                                    );
                //}
            }
        }
    }
    //--Ende Beschlagbeutel---+
    
    
    //+-- Beginn Beschlagbeutel Serienaufträge ---
    public void CreateFittingSeriesReports( Logger logger, 
                                            IUnitOfWorkBase unitOfWork, 
                                            HomagGroup.FLS.Domain.Data.ProductionOrder productionOrder)
    {
        // Binary Report Beschlagbeutel-Etikett
        var fittingLabelReports = unitOfWork.GetRepository<CustReport>()
                                    .Get(x => x.Layout.StartsWith("FittingStockLabel"));
        
        if (fittingLabelReports.Any())
        {
            foreach (var fittingLabelReport in fittingLabelReports)
            {
                //if (IsReportDefinedForPlanningGroup(unitOfWork, fittingLabelReport.Layout, productionOrder.CustomPlanningGroup))
                //{
                    logger.Info($"Binary Report Beschlagbeutel-Etikett Serienaufträg zu FA {productionOrder.Code}");   
            
                    CreateReportBindingBinary(fittingLabelReport.Layout, 
                                                productionOrder.Sequence, 
                                                //productionOrder.CustomPlanningGroup, 
                                                productionOrder.CustomWorkOrder,
                                                //string.Empty,         // VirtualCartCode
                                                string.Empty,         // OptimizationCode
                                                productionOrder.Code, // ProductionOrderCode
                                                productionOrder.Code, // ProductionItemCode
                                                (DateTime)productionOrder.DesiredStartDate,
                                                //productionOrder.CustomTrainStation,           
                                                //productionOrder.CustomTrainStationDescription,
    											productionOrder.CustomerOrderCode,
    											productionOrder.CustomerOrderPosition,
    											//productionOrder.CustomCapacitySequenceCode,
    											productionOrder.CustomCapacitySequence,
    											// null,                // TaggedLinesGroupName
    											productionOrder.DesiredTargetQuantity.ToString(),
    											"20");
                //}
            }
        }
        
        // Binary Report Beschlagbeutel Kommissionierliste
        var fittingListReports = unitOfWork.GetRepository<CustReport>()
                                    .Get(x => x.Layout.StartsWith("FittingStockList"));
        
        if (fittingListReports.Any())
        {
            foreach (var fittingListReport in fittingListReports)
            {
                //if (IsReportDefinedForPlanningGroup(unitOfWork, fittingListReport.Layout, productionOrder.CustomPlanningGroup))
                //{
                    logger.Info($"Binary Report Beschlagbeutel-Kommissionierliste Serienaufträg zu FA {productionOrder.Code}");   
            
                    CreateReportBindingBinary(fittingListReport.Layout, 
                                                productionOrder.Sequence, 
                                                //productionOrder.CustomPlanningGroup, 
                                                productionOrder.CustomWorkOrder,
                                                //string.Empty,         // VirtualCartCode
                                                string.Empty,         // OptimizationCode
                                                productionOrder.Code, // ProductionOrderCode
                                                productionOrder.Code, // ProductionItemCode
                                                (DateTime)productionOrder.DesiredStartDate,
                                                //productionOrder.CustomTrainStation,           
                                                //productionOrder.CustomTrainStationDescription,
    											productionOrder.CustomerOrderCode,
    											productionOrder.CustomerOrderPosition,
    											//productionOrder.CustomCapacitySequenceCode,
    											productionOrder.CustomCapacitySequence,
    											// null,                // TaggedLinesGroupName
    											null,                // ReportField01
    											null);               // ReportField01
                //}
            }
        }
    }
    //--Ende Beschlagbeutel---+
    
    // Binary Report Bauteiletikett
    public void CreatePartLabel (Logger logger, IUnitOfWork unitOfWork, HomagGroup.FLS.Domain.Data.ProductionOrder productionOrder)
    {
        //if (IsReportDefinedForPlanningGroup(unitOfWork, "PartLabel", productionOrder.CustomPlanningGroup))
        //{
            foreach (var productionItem in productionOrder.ProductionItems)
            {
                // kleinstes Startdatum aus AG ermitteln
                var productionStepStartDateMin = productionOrder.ProductionSteps.Min(x => x.DesiredStartDateProcessing);
                
                if (productionStepStartDateMin != null)
                {
                    logger.Info($"Binary Report Bauteiletikett (PartLabel) zu FA {productionOrder.Code} / Bauteil {productionItem.Code}");
                    
                    CreateReportBindingBinary(
													"PartLabel", 
                                                    productionItem.Sequence, 
                                                    //productionOrder.CustomPlanningGroup, 
                                                    productionOrder.CustomWorkOrder,
                                                    //string.Empty, // VirtualCartCode   
                                                    string.Empty, // OptimizationCode
                                                    productionOrder.Code, // ProductionOrderCode
                                                    productionItem.Code, // ProductionItemCode
                                                    (DateTime)productionStepStartDateMin,
                                                    //productionOrder.CustomTrainStation, // TrainStation
                                                    //productionOrder.CustomTrainStationDescription, // TrainStationDescription
                                                    productionOrder.CustomerOrderCode, // CustomerOrderCode
                                                    productionOrder.CustomerOrderPosition  // CustomerOrderPosition
                                                    );
                }
                else
                {
                    logger.Warn($"FA {productionOrder.Code} - kleinstes Startdatum aus Arbeitsgängen konnte nicht ermittelt werden!");
                }
            }
        /*
        }
        else
        {
            logger.Info($"FA {productionOrder.Code} - Dispogruppe {productionOrder.CustomPlanningGroup} nicht für Report Bauteiletikett (PartLabel) konfiguriert");
        }
        */
    }
    // ----- Ende Bauteiletikett
    
}




// Klasse zum Übergeben der Aufrufparameter an Erstellung ReportBinding
public class ReportBindingCreationObject
{
    // Name des Report
    public string ReportLayout { get; set; }
    
    // Sequence zu ReportEntity
    public Int64 ReportSequence { get; set; }
    
    // Planungsgruppe 
    public string planningGroup { get; set; }
    
    // Werkauftrag
    public string workOrder { get; set; }
    
    // Virtuelle Wagennummer
    public string virtualCartCode { get; set; }
    
    // Optimierungslos - wenn aus Optimierung
    public string optimizationCode { get; set; }
    
}
