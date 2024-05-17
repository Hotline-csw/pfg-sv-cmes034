//-----------------------------------------------------------------------------
//   (Class-)Name:   CustomEPOptimizedNotCutted
//
//   Description:    
//      Create of edge preview for optimized parts (requirement is deleted  by other Preview UserExit (CutParts)
// 
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         T.Brehm
//   Date:           2022-12-07
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date           Description
//   T.Brehm         2022-12-07     Created
//   T.Brehm         2022-12-20     Delete old entries and entries with feedback
//-----------------------------------------------------------------------------

using System.ComponentModel;

[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IUserExit))]
[Export("CustomEPOptimizedNotCutted", typeof(ControllerMES.Infrastructure.BaseCommon.Contracts.IFillEPRequirementsUserExit))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("Horizon for optimized but not cutted parts")]
[EnabledScript(true)]
public class CustomEPOptimizedNotCutted : UserExitCustomBase, ControllerMES.Infrastructure.BaseCommon.Contracts.IFillEPRequirementsUserExit
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;
    
    private Logger _Logger;
    
    public void Execute(IJobExecutionContext executionContext)
    {
        if (Convert.ToBoolean(executionContext.Inputs.FirstOrDefault(i => i.Key == "_UnitTest").Value))
            return;
            
        _Logger = LogHelper.GetLogger(executionContext.Area, typeof(CustomEPOptimizedNotCutted));
        _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);
		_Logger.Debug("Horizon 1 start befor config" );
		
		// Laden der Konfigurationen
        // Get the edgeMachinesConfiguration
        var edgeMachinesConfigurationCollection =
            executionContext.Inputs.FirstOrDefault(i => i.Key == "_Machines").Value as EdgeMachinesConfigurationCollection;
    
        // Get the edgePreviewHorizonConfiguration
        var edgePreviewHorizonConfiguration =
            executionContext.Inputs.FirstOrDefault(i => i.Key == "_PreviewHorizon").Value as EdgePreviewHorizonConfiguration;

        _Logger.Debug("CustomEPOptimizedNotCutted startet" );
        
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

                    //Repository of items to insert + repository of edge requirement (destination of items)
                    var epRequirementRepository = unitOfWork.GetRepository<EPRequirement>();
                    var productionItemRepository = unitOfWork.GetRepository<ProductionItem>();

                    // !!! *** Set here the workCenterCodes of the used edgebanding machines *** !!! //
                    List<string> autoEdgeMachines = new List<string>{"3010","3020","3030"};
                    // !!! ********************************************************************* !!! //

                    //Get all parts that have been optimized + no feedback + edge passes
                    var productionItems = productionItemRepository.GetQueryable(false).Where(
                                                                    pi => pi.OptimizationTransferState == OptimizationTransferState.Optimized
                    				                                && pi.OptimizationCode != null
                    				                                && !pi.ProductionItemsHistory.Any()
                    				                                && pi.ProductionOrder.EdgePasses.Any()
                    				                                ).ToList();
                                        
                    _Logger.Debug("Number of parts:" + productionItems.Count());
                    
                    foreach (var item in productionItems)
                    {
                        bool ignoreForCalculation = false;
                                            
                        //_Logger.Debug("Loop through item:" + item.Code);
                        
                        //Check if item exists in EpRequirement
                        var epRequirementItem = item.EPRequirements.FirstOrDefault(x => x.PreviewHorizon == previewHorizon);
                        
                        var epRequirementOtherParts = item.EPRequirements.FirstOrDefault(
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
                            //Write entry in EPRequirement
                            _Logger.Debug("Write EPRequirements for item:" + item.Code);
                            
                            var epRequirement = new EPRequirement();
                            
                            epRequirement.PreviewHorizon = previewHorizon;
                            epRequirement.ProductionOrderCode = item.ProductionOrderCode;
                            epRequirement.ProductionItemCode = item.Code;
                            epRequirement.CreationSource = previewHorizon;
                            epRequirement.ModificationSource = Name;
                            epRequirement.ModificationDate = DateTime.Now;
                            
                            if (previewHorizon == previewHorizon)
                            {
                                epRequirement.ExtData = new Dictionary<string, object>
                                {
                                    {"OptimizationNumber",item.OptimizationCode},
                                    // {"RunNumber",item.ProductionOrder.CustomRunNumberErp}
                                
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

                                    _Logger.Debug("Insert EPRequirement edge:" + edgePass.ProductionOrderCode);
                                    
                                    // Insert EPRequirementsEdges for each edge
									epRequirement.EPRequirementsEdges.Add(new EPRequirementsEdge
									{
										Workcenter = edgePass.WorkCenterCode,
										MachineName = edgePass.WorkCenterCode,
										Pass = edgePass.Pass,
										IgnoreForCalculate = ignoreForCalculation,
										EdgeMacro = edgePass.EdgeMacro,
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
                    
			     }
		     }
		     
 		    catch (Exception ex)
            {
                _Logger.Error("Error in edge preview for Optimized / Not Cut",null,ex);
            }
        } 
    }
}