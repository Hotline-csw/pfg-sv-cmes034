//-----------------------------------------------------------------------------
//   (Class-)Name:   CustomEPNotOptimizedNotCuttedTEST2
//
//   Description:    
//   Create of edge preview for optimized parts (requirement is deleted  by other Preview UserExit (CutParts)
// 
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         T.Stürzer
//   Date:           2022-12-07
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date           Description
//   T.Stürzer       2023-03-14     Created
//-----------------------------------------------------------------------------

using System.ComponentModel;

[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IUserExit))]
[Export("CustomEPNotOptimizedNotCuttedTEST2", typeof(ControllerMES.Infrastructure.BaseCommon.Contracts.IFillEPRequirementsUserExit))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("!!! Test2 UE Horizon for not optimized and not cutted parts ")]
[EnabledScript(true)]
public class CustomEPNotOptimizedNotCuttedTEST2 : UserExitCustomBase, ControllerMES.Infrastructure.BaseCommon.Contracts.IFillEPRequirementsUserExit
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;
    
    private Logger _Logger;
    
    private string _PreviewName = "NoOptiNoCut-Test2";
    
    
    public void Execute(IJobExecutionContext executionContext)
    {
        if (Convert.ToBoolean(executionContext.Inputs.FirstOrDefault(i => i.Key == "_UnitTest").Value))
            return;
            
        _Logger = LogHelper.GetLogger(executionContext.Area, typeof(CustomEPNotOptimizedNotCuttedTEST2));
        _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);
		
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
			     using (var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWorkBase())
			     {
		            // Set the PreviewHorizon
    				var previewHorizon = edgePreviewHorizonConfiguration.Name;
    				_Logger.Info($"{previewHorizon}");
    				
    				//IRepository<ProductionStep> productionStepRepository = unitOfWork.GetRepository<ProductionStep>();
    				// !!! Disable bulk insert because of parallel processing, otherwise deadlocks will occur !!!
    				unitOfWork.UseBulk = false;

                    //Repository of items to insert + repository of edge requirement (destination of items)
                    var epRequirementRepository = unitOfWork.GetRepository<EPRequirement>();
                    var productionItemRepository = unitOfWork.GetRepository<ProductionItem>();

                    // !!! *** Set here the workCenterCodes of the used edgebanding machines *** !!! //
                    List<string> autoEdgeMachines = new List<string>{"3010","3020","3030"};
                    // !!! ********************************************************************* !!! //
                    
                    DateTime startQueryProdItem = DateTime.Now;
                    
                    List<string> epPreviewItem = unitOfWork.GetRepository<EPRequirement>()
                        .GetQueryable()
                            .Where(
                                ep => ep.PreviewHorizon == previewHorizon)
                            .Select(s => s.ProductionItemCode)
                        .ToList();
                        
                    _Logger.Info(string.Format("{0}: Anzahl epPreviewItems: [{1}]", _PreviewName, epPreviewItem.Count()));
                    
                    List<string> prodItems = unitOfWork.GetRepository<ProductionItem>()
                        .GetQueryable()
                            .Where(
                                pi => pi.OptimizationTransferState == OptimizationTransferState.NotOptimized
                                && pi.OptimizationCode == null
                                && !pi.ProductionItemsHistory.Any()
                    			&& pi.ProductionOrder.EdgePasses.Any())
                            .Select(s => s.Code)	
            			.ToList();
            			
        			_Logger.Info(string.Format("{0}: Anzahl productionItems: [{1}]", _PreviewName, prodItems.Count()));


                    //Get all parts that have been optimized + no feedback + edge passes
                    var productionItems = productionItemRepository.Get(
                                                                    pi => pi.OptimizationTransferState== OptimizationTransferState.NotOptimized
                                                                    && pi.OptimizationCode == null
                                                                    && !pi.ProductionItemsHistory.Any()
                    				                                && pi.ProductionOrder.EdgePasses.Any()
                    				                                ).ToList();
                   				                                
                    TimeSpan queryProdItem = DateTime.Now - startQueryProdItem;
                    
                    DateTime startQueryUpdateEp = DateTime.Now;
                    
                    
                    foreach (var item in productionItems)
                    {                    
                        bool ignoreForCalculation = false;
                        
                        //Check if item exists in EpRequirement
                        var epRequirementItem = item.EPRequirements.FirstOrDefault(x => x.PreviewHorizon == previewHorizon);
/*                        
                        var epRequirementOtherParts = item.EPRequirements.FirstOrDefault(
                                x => x.PreviewHorizon == "No Opti - Cut - OG");
                        
                        if (epRequirementOtherParts != null)
                        {
                            epRequirementRepository.Delete(epRequirementOtherParts);
                        }
*/                        
                        if (epRequirementItem == null)
                        {
                            //Write entry in EPRequirement
                            var epRequirement = new EPRequirement();
                            
                            epRequirement.PreviewHorizon = previewHorizon;
                            epRequirement.ProductionOrderCode = item.ProductionOrderCode;
                            epRequirement.ProductionItemCode = item.Code;
                            epRequirement.CreationSource = _PreviewName;
                            epRequirement.ModificationSource = _PreviewName;
                            epRequirement.ModificationDate = DateTime.Now;
                            
                            if (previewHorizon == previewHorizon)
                            {
                                epRequirement.ExtData = new Dictionary<string, object>
                                {
                                    {"ProductionOrder",item.ProductionOrderCode},                               
                                };
                            }
                            
                            epRequirementRepository.AddOrUpdate(epRequirement); 
                            
                            //Loop through each edge pass of item
                            foreach (var edgePass in item.ProductionOrder.EdgePasses)
                            {
                                //Check if edge has a thickness (empty throughfeeds are not required)
                                if (autoEdgeMachines.Contains(edgePass.WorkCenterCode) && edgePass.EdgeThickness > 0  && edgePass.EdgeThickness != null)
                                {
                                    //Get the correspondent edge profile
                                    var edgeProfile = item.ProductionOrder.EdgeProfiles.FirstOrDefault(x => x.Code == edgePass.EdgeProfileCode);
                                    
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
										CreationSource = _PreviewName,
										ModificationSource = _PreviewName,
										ModificationDate = DateTime.Now

									});
                                }
                                
                                epRequirementRepository.AddOrUpdate(epRequirement);                            
                            }
                        }
                    }

                    unitOfWork.Save();
                    
                    TimeSpan queryEp = DateTime.Now - startQueryUpdateEp;
                    
                    
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
                    
                    _Logger.Info(string.Format("{0}: Teileanzahl Gesamt: [{1}] | Abfragezeit ProdItem: [{3}] | Abfragezeit UpdateEP: [{4}] | Gesamtabfragezeit: [{5}]", _PreviewName, productionItems.Count(), queryProdItem, queryEp, totalRuntime));                    
			     }
		     }
		     
 		    catch (Exception ex)
            {
                _Logger.Error("Error in edge preview for No Opti - No Cut - TEST2",null,ex);
            }
        } 
    }
}