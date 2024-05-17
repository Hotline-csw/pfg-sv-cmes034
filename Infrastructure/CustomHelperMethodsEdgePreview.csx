//-----------------------------------------------------------------------------
//   (Class-)Name:   CustomHelperMethodsEdgePreview
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         T.Stürzer
//   Date:           2023-08-15
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   T.Stürzer       2023-08-15    Created
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IGlobalCustomization))]
[Export("CustomHelperMethodsEdgePreview", typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IGlobalCustomization))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("HelperMethods for EdgePreview")]
[EnabledScript(true)]
public class CustomHelperMethodsEdgePreview : UserExitCustomBase, HomagGroup.FLS.Infrastructure.Common.Customization.IGlobalCustomization
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;
    
    /// <summary>
    /// Method to write data in the edge preview tables
	/// </summary>
	/// <param name="edgeMachinesConfigurationCollection">EdgeMachinesConfigurationCollection</param>
	/// <param name="productionItem">ProductionItem</param>
	/// <param name="lastMovement">Zeitstempel letzte Rückmeldung vom Bauteil</param>
	/// <param name="workcenter">Name des Arbeitsplatzes</param>
    /// <param name="previewHorizon">Name der Preview</param>    
    /// <param name="logger">Logger</param>	
	/// <returns>Returns true if succesful, otherwise returns false</returns>
	
	public bool WriteToEpRequirements(
	       EdgeMachinesConfigurationCollection edgeMachinesConfigurationCollection, 
	       ProductionItem productionItem, DateTime lastMovement, 
	       string workcenter, string previewHorizon, Logger logger)
	{
        try
        {
            using (var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
            {
                // !!! Bulk-Insert ausschalten wegen Parallelverarbeitung, sonst kommt es zu deadlocks !!!
                //TODO: Klären, ob das hier notwendig ist
                //unitOfWork.UseBulk = false;
                
                int minimalLength = 0;
                
                //Pruefen ob die Daten schon vorhanden sind						
    			if(productionItem.EPRequirements != null && productionItem.EPRequirements.Any(a => a.PreviewHorizon == previewHorizon))
    			{
                    EPRequirement ePRequirement = productionItem.EPRequirements.FirstOrDefault();
    				if(ePRequirement != null)
    				{
    					if(ePRequirement.CustomLastMovementDate < lastMovement)
    					{
    						ePRequirement.CustomLastMovementDate = lastMovement;
    						logger.Debug($"customlastmovementDate von {ePRequirement.ProductionItemCode} aktualisiert!");
    						
    						unitOfWork.Save();
    					}									
    				}
    				if(logger.IsDebugEnabled)
                    logger.Debug($"Daten sind schon fuer ProductionItem {productionItem.Code} mit ProductionOrderCode {productionItem.ProductionOrderCode} in den EpRequirements enthalten");
    				return true;
    			}
    			
    			//Prüfen ob die die Kantenanlage im Arbeitsplan enthalten ist!
				if (productionItem.ProductionOrder.ProductionSteps.Any(x=>x.WorkCenterCode == workcenter))	
				{
					EPRequirement epRequirement = new EPRequirement();
					
					epRequirement.ProductionItemCode = productionItem.Code;
					epRequirement.ProductionOrderCode = productionItem.ProductionOrderCode;
					epRequirement.CustomLastMovementDate = lastMovement;
					epRequirement.PreviewHorizon = previewHorizon;
					epRequirement.CreationDate = DateTime.Now;
					epRequirement.CreationSource = previewHorizon;
					
					List<EdgePass> edgePassesKam = productionItem.ProductionOrder.EdgePasses.Where(a => a.WorkCenterCode == workcenter && a.PassMachiningType == "E").ToList();
					
					if(edgePassesKam == null || !edgePassesKam.Any())
					{
					   logger.Debug($"Es wurde keine EdgePass mit dem WorkCenterCode {workcenter} gefunden");
					   return true;
				    }
					
					foreach (var edgePass in edgePassesKam)
					{
                        //Get the correspondent edge profile
                        var edgeProfile = edgePass.ProductionOrder.EdgeProfiles.FirstOrDefault(x => x.Code == edgePass.EdgeProfileCode);					
						EPRequirementsEdge epRequirementsEdge = new EPRequirementsEdge();
					
						epRequirementsEdge.EPRequirementsSequence = epRequirement.Sequence;
						epRequirementsEdge.Workcenter = workcenter;
						epRequirementsEdge.MachineName = edgeMachinesConfigurationCollection.FirstOrDefault(x => x.WorkCenter == workcenter && x.Passes.Contains(Convert.ToString(edgePass.Pass)))?.MachineId;
						epRequirementsEdge.Pass = edgePass.Pass;
						epRequirementsEdge.IgnoreForCalculate = false;	
						decimal length = productionItem.ProductionOrder.EdgeProfiles.FirstOrDefault(x => x.AlternateCode == edgePass.EdgeInProcess && x.ProductionStepCode == edgePass.ProductionStepCode)?.Length ?? minimalLength;
						epRequirementsEdge.Length = length;
						epRequirementsEdge.CreationDate = DateTime.Now;
						epRequirementsEdge.CreationDate = DateTime.Now;
						epRequirementsEdge.CreationSource = previewHorizon;
						epRequirementsEdge.EdgeMacro = edgePass.CustomerEdge;
						epRequirement.EPRequirementsEdges.Add(epRequirementsEdge);
					}								
					
					unitOfWork.AddOrUpdate(new[] {epRequirement});

					unitOfWork.Save();
				}
				else
				logger.Debug($"Bauteil: {productionItem.Code} enthält keinen Arbeitgangs für {workcenter}!");
			}
			return true;
        }
        catch (Exception e)
        {
        	logger.Error(ResourcesKeys.ErrorInUserExit("WriteToEpRequirements"), null, e);
        	return false;
        }
	}
}
