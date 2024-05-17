//-----------------------------------------------------------------------------
//   (Class-)Name:   CustomEPOptimizedCutted
//
//   Description:    
//      Insert ep requirement for cut parts
//      Delete in feedback job of Homag Automation
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
//   T.Brehm         2022-12-15     Delete entries with feedback older than 7 days
//-----------------------------------------------------------------------------

using System.ComponentModel;
using System.Collections.Generic;

using HomagGroup.FLS.Infrastructure.Common.Database;
using System.ComponentModel.Composition;
using ControllerMES.Infrastructure.Common.UserExits;
using HomagGroup.FLS.Services.DataAccess.Contracts;
using ControllerMES.Infrastructure.Common.Helpers;
using HomagGroup.FLS.Services.Common.Contracts.DatabaseAndConnectionString;
using HomagGroup.FLS.Infrastructure.Common.Logging;
using HomagGroup.FLS.Services.Common.Contracts.JobScheduling;
using HomagGroup.FLS.Infrastructure.Common.ComponentModel;
using System;
using System.Linq;
using ControllerMES.Services.EdgePreview.Contracts.Configuration;
using HomagGroup.FLS.Domain.Data;
using System.Globalization;
using System.Data.SqlClient;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IUserExit))]
[Export("CustomEPOptimizedCutted", typeof(ControllerMES.Infrastructure.BaseCommon.Contracts.IFillEPRequirementsUserExit))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("Horizon for optimized and cutted parts")]
[EnabledScript(true)]
public class CustomEPOptimizedCutted : UserExitCustomBase, ControllerMES.Infrastructure.BaseCommon.Contracts.IFillEPRequirementsUserExit
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;

    private Logger _Logger;
    
    public void Execute(IJobExecutionContext executionContext)
    {
        if (Convert.ToBoolean(executionContext.Inputs.FirstOrDefault(i => i.Key == "_UnitTest").Value))
            return;
            
        _Logger = LogHelper.GetLogger(executionContext.Area, typeof(CustomEPOptimizedCutted));
        _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);
		_Logger.Debug("HorizontCutParts starts before config" );
		
		//Laden der Konfigurationen
        // Get the edgeMachinesConfiguration
        var edgeMachinesConfigurationCollection =
            executionContext.Inputs.FirstOrDefault(i => i.Key == "_Machines").Value as EdgeMachinesConfigurationCollection;
    
        // Get the edgePreviewHorizonConfiguration
        var edgePreviewHorizonConfiguration =
            executionContext.Inputs.FirstOrDefault(i => i.Key == "_PreviewHorizon").Value as EdgePreviewHorizonConfiguration;

        _Logger.Debug("CustomEPOptimizedCutted startet");
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
                    
                    //Get all items that have been optimized + Edgeband passes + feedback of saw
                    var productionItems = productionItemRepository.GetQueryable(false)
                                                                    .Where(pi => pi.OptimizationTransferState == OptimizationTransferState.Optimized
    				                                                            && pi.OptimizationCode != null
    				                                                            && pi.ProductionItemsHistory.Any(pih => cuttingMachines.Contains(pih.WorkCenterCode))
    				                                                            && pi.ProductionItemsHistory.Count() == 1 //only one feedback from saw
    				                                                            && pi.ProductionOrder.EdgePasses.Any()
                                                                                ).ToList();
                    
                    foreach (var productionItem in productionItems)
                    {
                        bool ignoreForCalculation = false;
                        
                        //Item can exist for preview horizon of sorted parts
                        var epRequirementItem = productionItem.EPRequirements.FirstOrDefault(x => x.PreviewHorizon == previewHorizon);
                        
                        var epRequirementOtherParts = productionItem.EPRequirements.FirstOrDefault(
                                x => x.PreviewHorizon == "Not Optimized / Not Cut" ||
                                     x.PreviewHorizon == "Not Optimized / Cut" ||
                                     x.PreviewHorizon == "Optimized / Not Cut" /*||
                                     //x.PreviewHorizon == "Optimized / Cut"*/);
                        
                        if (epRequirementOtherParts != null)
                        {
                            epRequirementRepository.Delete(epRequirementOtherParts);
                        }
                        
                        if (epRequirementItem == null)
                        {
                            //Write entry in EPRequirement
                            _Logger.Debug("Write EPRequirements for item:" + productionItem.Code);
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
                                    {"OptimizationNumber",productionItem.OptimizationCode}
                                
                                };
                            }
                            
                            epRequirementRepository.AddOrUpdate(epRequirement);
                            
                            //Loop through each throughfeed
                            foreach (var edgePass in productionItem.ProductionOrder.EdgePasses)
                            {
                                if (autoEdgeMachines.Contains(edgePass.WorkCenterCode) && edgePass.EdgeThickness > 0  && edgePass.EdgeThickness != null)
                                {
                                    var edgeProfile = productionItem.ProductionOrder.EdgeProfiles.FirstOrDefault(x => x.Code == edgePass.EdgeProfileCode);
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
                    unitOfWork.Save();
                    /*****************************************************************************************************/

			     }
			     
		     }
		     
 		    catch (Exception ex)
            {
                _Logger.Error("Error in edge preview for Optimized / Cut",null,ex);
            }
        } 
    }
}
