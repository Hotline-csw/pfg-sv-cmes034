//-----------------------------------------------------------------------------
//   (Class-)Name:   CustomEPNotOptimizedCutted
//
//   Description:    
//   Insert ep requirement for cut parts
//   Delete in feedback job of Homag Automation
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         T.Brehm
//   Date:           2022-12-07
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date           Description
//   T.Stürzer       2023-03-14     Modified -> Not Optimized but cutted
//-----------------------------------------------------------------------------

using System.ComponentModel;
using System.Collections.Generic;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IUserExit))]
[Export("CustomEPNotOptimizedCutted", typeof(ControllerMES.Infrastructure.BaseCommon.Contracts.IFillEPRequirementsUserExit))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("!!! Original UE Horizon for not optimized but cutted parts")]
[EnabledScript(true)]
public class CustomEPNotOptimizedCutted : UserExitCustomBase, ControllerMES.Infrastructure.BaseCommon.Contracts.IFillEPRequirementsUserExit
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;

    private Logger _Logger;
    
    private string _PreviewName = "NoOptiCut-OG";
    
    
    public void Execute(IJobExecutionContext executionContext)
    {
        if (Convert.ToBoolean(executionContext.Inputs.FirstOrDefault(i => i.Key == "_UnitTest").Value))
            return;
            
        _Logger = LogHelper.GetLogger(executionContext.Area, typeof(CustomEPNotOptimizedCutted));
        _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);
		
		//Laden der Konfigurationen
        // Get the edgeMachinesConfiguration
        var edgeMachinesConfigurationCollection =
            executionContext.Inputs.FirstOrDefault(i => i.Key == "_Machines").Value as EdgeMachinesConfigurationCollection;
    
        // Get the edgePreviewHorizonConfiguration
        var edgePreviewHorizonConfiguration =
            executionContext.Inputs.FirstOrDefault(i => i.Key == "_PreviewHorizon").Value as EdgePreviewHorizonConfiguration;
            
        DateTime startUE = DateTime.Now;

        if (edgeMachinesConfigurationCollection != null && edgePreviewHorizonConfiguration != null)
        {
            try
            {        
			     DateTime startTimeUserExit = DateTime.Now;
			 
			     using (var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWorkBase())
			     {
		            // Set the PreviewHorizon
    				var previewHorizon = edgePreviewHorizonConfiguration.Name;
    				_Logger.Debug("Horizon:" + previewHorizon);
    				
    				//IRepository<ProductionStep> productionStepRepository = unitOfWork.GetRepository<ProductionStep>();
    				// !!! Disable bulk insert because of parallel processing, otherwise deadlocks will occur !!!
    				unitOfWork.UseBulk = false;

                    /*****************************************************************************************************/
                    /********************************* Loop through all optimized items **********************************/
                    /*****************************************************************************************************/
    				var epRequirementRepository = unitOfWork.GetRepository<EPRequirement>();
                    IRepository<ProductionItem> productionItemRepository = unitOfWork.GetRepository<ProductionItem>();
                    
                    // !!! *** Set here the workCenterCodes of the used cutting and edgebanding machines *** !!! //
                    List<string> cuttingMachines = new List<string>{"1010"};
                    List<string> autoEdgeMachines = new List<string>{"3010","3020","3030"};
                    // !!! ********************************************************************************* !!! //
                    
                    DateTime startQueryProdItem = DateTime.Now;
                    
                    //Get all items that have been optimized + Edgeband passes + feedback of saw
                    var productionItems = productionItemRepository.GetQueryable(false)
                                                                    .Where(pi => pi.OptimizationTransferState== OptimizationTransferState.NotOptimized
                                                                           && pi.OptimizationCode == null
                                                                           && pi.ProductionItemsHistory.Any(pih => cuttingMachines.Contains(pih.WorkCenterCode))
				                                                           && pi.ProductionItemsHistory.Count() == 1 //only one feedback from saw
    				                                                       && pi.ProductionOrder.EdgePasses.Any()
                                                                           ).ToList();
                                                                           
                    TimeSpan queryProdItem = DateTime.Now - startQueryProdItem;
                    
                    
                    DateTime startQueryUpdateEp = DateTime.Now;
                                                                           
                    
                    foreach (var productionItem in productionItems)
                    {
                        bool ignoreForCalculation = false;
                        
                        //Item can exist for preview horizon of sorted parts
                        var epRequirementItem = productionItem.EPRequirements.FirstOrDefault(x => x.PreviewHorizon == previewHorizon);
                        
                        var epRequirementOtherParts = productionItem.EPRequirements.FirstOrDefault(
                                x => x.PreviewHorizon == "No Opti - No Cut - OG");
                        
                        if (epRequirementOtherParts != null)
                        {
                            epRequirementRepository.Delete(epRequirementOtherParts);
                        }
                        
                        if (epRequirementItem == null)
                        {
                            //Write entry in EPRequirement
                            var epRequirement = new EPRequirement();
                            
                            epRequirement.PreviewHorizon = previewHorizon;
                            epRequirement.ProductionOrderCode = productionItem.ProductionOrderCode;
                            epRequirement.ProductionItemCode = productionItem.Code;
                            epRequirement.CreationSource = previewHorizon;
                            epRequirement.ModificationSource = Name;
                            epRequirement.ModificationDate = DateTime.Now;                                  
                            
                            if (previewHorizon == previewHorizon)
                            {
                                epRequirement.ExtData = new Dictionary<string, object>
                                {
                                    {"ProductionOrderCode",productionItem.ProductionOrderCode}
                                
                                };
                            }
                            
                            epRequirementRepository.AddOrUpdate(epRequirement);
                            
                            //Loop through each throughfeed
                            foreach (var edgePass in productionItem.ProductionOrder.EdgePasses)
                            {
                                if (autoEdgeMachines.Contains(edgePass.WorkCenterCode) && edgePass.EdgeThickness > 0  && edgePass.EdgeThickness != null)
                                {
                                    var edgeProfile = productionItem.ProductionOrder.EdgeProfiles.FirstOrDefault(x => x.Code == edgePass.EdgeProfileCode);
                                    
                                    // Insert EPRequirementsEdges for each edge
									epRequirement.EPRequirementsEdges.Add(new EPRequirementsEdge
									{
										Workcenter = edgePass.WorkCenterCode,
										MachineName = edgePass.WorkCenterCode,
										Pass = edgePass.Pass,
										IgnoreForCalculate = ignoreForCalculation,
										//EdgeMacro = edgePass.EdgeMacro,
										EdgeMacro = edgePass.CustomerEdge,
										Length = edgeProfile.Length,
										CreationSource = Name,
										ModificationSource = Name,
										ModificationDate = DateTime.Now

									});
                                }
                                
                                epRequirementRepository.AddOrUpdate(epRequirement);                            
                            
                            }
                        }
                    }
                    
                    unitOfWork.Save();
                    
                    TimeSpan queryEp = DateTime.Now - startQueryUpdateEp;
                    
                    /*****************************************************************************************************/
                    /*********************** Delete EpRequirements with feedback older than 7 days ***********************/
                    /*****************************************************************************************************/
                    DateTime timeOldParts = DateTime.Now.AddDays(-7);
                    
                    var epRequirementsToDelete = epRequirementRepository.GetQueryable(false)
						.Where(x => x.PreviewHorizon == previewHorizon
						&& 
						(x.ProductionItem.ProductionItemsHistory.Any(pih => autoEdgeMachines.Contains(pih.WorkCenterCode))
						||
						 x.ProductionItem.ProductionItemsHistory.Any(pihi => cuttingMachines.Contains(pihi.WorkCenterCode) &&  pihi.CreationDate < timeOldParts))
						).ToList();
                    
                    unitOfWork.BulkDelete(epRequirementsToDelete);
                    /*****************************************************************************************************/
                    unitOfWork.Save();
                    
                    TimeSpan totalRuntime = DateTime.Now - startUE;
                    
                    _Logger.Info(string.Format("{0}: Teileanzahl: [{1}] | Abfragezeit ProdItem: [{2}] | Abfragezeit UpdateEP: [{3}] | Gesamtabfragezeit: [{4}]", 
                    _PreviewName, productionItems.Count(), queryProdItem, queryEp, totalRuntime));                    
			     }
		     }
		     
 		    catch (Exception ex)
            {
                _Logger.Error("Error in edge preview for Not Optimized / Cut",null,ex);
            }
        } 
    }
}
