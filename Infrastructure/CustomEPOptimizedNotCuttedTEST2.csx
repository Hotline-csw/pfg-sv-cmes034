//-----------------------------------------------------------------------------
//   (Class-)Name:   CustomEPOptimizedNotCuttedTEST2
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         <Author>
//   Date:           2023-08-16
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   <Author>        2023-08-16    Created
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IUserExit))]
[Export("CustomEPOptimizedNotCuttedTEST2", typeof(ControllerMES.Infrastructure.BaseCommon.Contracts.IFillEPRequirementsUserExit))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("!!!Horizon for optimized but not cutted parts")]
[EnabledScript(true)]
public class CustomEPOptimizedNotCuttedTEST2 : UserExitCustomBase, ControllerMES.Infrastructure.BaseCommon.Contracts.IFillEPRequirementsUserExit
{
[Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;
    
    [Import("CustomHelperMethodsEdgePreview")] 
    IGlobalCustomization _CustomHelperMethodsEdgePreview;
    
    private Logger _Logger;
    
    public void Execute(IJobExecutionContext executionContext)
    {
        if (Convert.ToBoolean(executionContext.Inputs.FirstOrDefault(i => i.Key == "_UnitTest").Value))
            return;
            
        _Logger = LogHelper.GetLogger(executionContext.Area, typeof(CustomEPOptimizedNotCuttedTEST2));
        _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);
		//_Logger.Debug("Horizon 1 start befor config" );
		
		// Laden der Konfigurationen
        // Get the edgeMachinesConfiguration
        var edgeMachinesConfigurationCollection =
            executionContext.Inputs.FirstOrDefault(i => i.Key == "_Machines").Value as EdgeMachinesConfigurationCollection;
    
        // Get the edgePreviewHorizonConfiguration
        var edgePreviewHorizonConfiguration =
            executionContext.Inputs.FirstOrDefault(i => i.Key == "_PreviewHorizon").Value as EdgePreviewHorizonConfiguration;
            
        DateTime startUE = DateTime.Now;
//        _Logger.Info($"NotOptimizedNotCutted - Startzeit: [{startUE}]");
        
        if (edgeMachinesConfigurationCollection != null && edgePreviewHorizonConfiguration != null)
        {
            try
            {       			 
			     using (var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWorkBase())
			     {
		            // Set the PreviewHorizon
    				var previewHorizon = edgePreviewHorizonConfiguration.Name;
    				//_Logger.Debug("Horizon:" + previewHorizon);
    				
    				//IRepository<ProductionStep> productionStepRepository = unitOfWork.GetRepository<ProductionStep>();
    				// !!! Disable bulk insert because of parallel processing, otherwise deadlocks will occur !!!
    				unitOfWork.UseBulk = false;

                    //Repository of items to insert + repository of edge requirement (destination of items)
                    var epRequirementRepository = unitOfWork.GetRepository<EPRequirement>();
                    var productionItemRepository = unitOfWork.GetRepository<ProductionItem>();

                    // !!! *** Set here the workCenterCodes of the used edgebanding machines *** !!! //
                    //List<string> autoEdgeMachines = new List<string>{"3010","3020","3030"};
                    string workCenter = "3010";
                    // !!! ********************************************************************* !!! //
                    
                    
                    DateTime startQueryProdItem = DateTime.Now;
                    
                    //Get all parts that have been optimized + no feedback + edge passes
                    var productionItems = productionItemRepository.GetQueryable(false).Where(
                                                                    pi => pi.OptimizationTransferState == OptimizationTransferState.Optimized
                    				                                && pi.OptimizationCode != null
                    				                                && !pi.ProductionItemsHistory.Any()
                    				                                && pi.ProductionOrder.EdgePasses.Any()
                    				                                ).ToList();
                    TimeSpan queryProdItem = DateTime.Now - startQueryProdItem;                 
                    //_Logger.Info($"CustomEPOptimizedNotCuttedTEST2 - Abfragezeit ProdItem: [{queryProdItem}] - Teileanzahl: " + productionItems.Count());                                 
                    
                    DateTime startQueryUpdateEp = DateTime.Now;
                    
                    foreach (var prodItem in productionItems)
                    {
                        //bool ignoreForCalculation = false;
                                            
                        //_Logger.Debug("Loop through item:" + item.Code);
                        
                        //Check if item exists in EpRequirement
                        var epRequirementItem = prodItem.EPRequirements.FirstOrDefault(x => x.PreviewHorizon == previewHorizon);
                        
                        var epRequirementOtherParts = prodItem.EPRequirements.FirstOrDefault(
                                x => x.PreviewHorizon == "Not Optimized / Not Cut" ||
                                     x.PreviewHorizon == "Not Optimized / Cut" ||
                                     /*x.PreviewHorizon == "Optimized / Not Cut" ||*/
                                     x.PreviewHorizon == "Optimized / Cut");
                        
                        if (epRequirementOtherParts != null)
                        {
                            epRequirementRepository.Delete(epRequirementOtherParts);
                        }
                        
                        if (epRequirementItem == null)
                        {
                            (_CustomHelperMethodsEdgePreview as CustomHelperMethodsEdgePreview).WriteToEpRequirements(
                                    edgeMachinesConfigurationCollection,
                                    prodItem,
                                    DateTime.Now,
                                    workCenter,
                                    previewHorizon,
                                    _Logger);
                        }
                        else
                        {
                            //_Logger.Debug($"Kein ProductionItem gefunden für ProductionitemCode='{prodItem.Code}'");
                        }
                    }
                    
                    TimeSpan queryEp = DateTime.Now - startQueryUpdateEp;
                    //_Logger.Info($"CustomEPOptimizedNotCuttedTEST2 - Abfragezeit UpdateEP: [{queryEp}]");
                    
                    //**************************************** Delete Items of this horizon ****************************************//
                    //Delete parts which have at least one feedback OR that have been optimized and not produced for more than 30 days 
                    DateTime timeOldParts = DateTime.Now.AddDays(-30);
                    
                    var epRequirementsToDelete = epRequirementRepository.GetQueryable(false)
						.Where(x => x.PreviewHorizon == previewHorizon
						&& 
						(x.ProductionItem.ProductionItemsHistory.Any() || x.ProductionItem.ProductionOrder.OptimizationParts.Any(y => y.CreationDate < timeOldParts))
						).ToList();
                    
                    unitOfWork.BulkDelete(epRequirementsToDelete);
                    //***************************************************************************************************************//
                    
                    unitOfWork.Save();
                    
                    TimeSpan totalRuntime = DateTime.Now - startUE;
                    //_Logger.Info($"CustomEPOptimizedNotCuttedTEST2 - Gesamtabfragezeit: [{totalRuntime}]"); 
                     
			     }
		     }
		     
 		    catch (Exception ex)
            {
                _Logger.Error("Error in edge preview for Not Optimized / Not Cut",null,ex);
            }
        } 
    }
}
