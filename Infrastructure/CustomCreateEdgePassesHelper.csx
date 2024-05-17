//-----------------------------------------------------------------------------
//   (Class-)Name:   CustomCreateEdgePassesHelper
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         T.Stürzer
//   Date:           2023-04-24
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   T.Stürzer       2023-04-24    Created
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;


[Export(typeof(ICreateEdgePassesHelper))]
[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IServerCustomization))]
[Export("CustomCreateEdgePassesHelper", typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IServerCustomization))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("Overwrite CreateEdgePassesHelper - disable check for more than one groove on one side")]
[EnabledScript(true)]
public class CustomCreateEdgePassesHelper : CreateEdgePassesHelper
{
    /// <summary>
    /// Checks the quantity of EdgeGrooves for each GrooveInProcess-value (must be ==1)
    /// together with the needed MacroNumber (aggregate-number).
    /// Sets HandleError(), if not OK.
    /// </summary>
    /// <param name="productionOrder">Current instance of ProductionOrder.</param>
    /// <param name="possibleRoute">Current instance of PossibleRoute.</param>
    /// <param name="productionStep">Current instance of ProductionStep.</param>
    /// <param name="currentEdgePass">Current instance of EdgePass.</param>
    /// <param name="currentShapePass">Current instance of ShapePass.</param>
    /// <param name="currentEdgeGrooves">Collection of EdgeGrooves - EdgeGrooves of the current (Possible-) Route.</param>
    /// <param name="currentGroovesInProcessWithMacroNumber">List of instances of GrooveInProcessWithMacroNumber of all valid GrooveInProcess-values (from DC ShapePass) and the needed MacroNumbers (aggregate - numbers).</param>
    /// <param name="returnErrorMessage">ResourceIdentifier - error-message to return back to the caller.</param>
    /// <returns>bool - true, if OK.</returns>
    protected override bool CheckGrooveQuantityForGroovesInProcess(ProductionOrder productionOrder, PossibleRoute possibleRoute,
                                                                    ProductionStep productionStep, EdgePass currentEdgePass, ShapePass currentShapePass,
                                                                    IEnumerable<EdgeGroove> currentEdgeGrooves,
                                                                    IEnumerable<GrooveInProcessWithMacroNumber> currentGroovesInProcessWithMacroNumber,
                                                                    out ResourceIdentifier returnErrorMessage)
    {
        Guard.ThrowOnArgumentNull(productionOrder, "productionOrder");
        Guard.ThrowOnArgumentNull(possibleRoute, "possibleRoute");
        Guard.ThrowOnArgumentNull(productionStep, "productionStep");
        Guard.ThrowOnArgumentNull(currentEdgePass, "currentEdgePass");
        Guard.ThrowOnArgumentNull(currentShapePass, "currentShapePass");
        Guard.ThrowOnArgumentNull(currentEdgeGrooves, "currentEdgeGrooves");
        Guard.ThrowOnArgumentNull(currentGroovesInProcessWithMacroNumber, "currentGroovesInProcessWithMacroNumber");

        bool success = true;
        //ResourceIdentifier msg = null;
        returnErrorMessage = null;


        foreach (GrooveInProcessWithMacroNumber currentGrooveInProcessWithMacroNumber in currentGroovesInProcessWithMacroNumber)
        {
            // In übergebener Groove-Liste sollten keine Nuten enthalten sein, die keinem Step zugeordnet sind.
            IEnumerable<EdgeGroove> edgeGroovesWithoutLinkToProductionStep = currentEdgeGrooves
                .Where(eg => eg.AlternateCode == currentGrooveInProcessWithMacroNumber.GrooveInProcess &&
                             eg.MacroNumber == currentGrooveInProcessWithMacroNumber.MacroNumberForGroove &&
                             eg.ProductionStep == null);

            // Error - these EdgeGrooves are not linked to the ProductionStep-entity!
            if (edgeGroovesWithoutLinkToProductionStep.Any())
            {
                success = false;

                // {0}: FA='{1}', fehlende Verknüpfung von '{2}' {3} zu {4}] !
                returnErrorMessage = ResourcesKeys.ErrorMissingLinkToparentEntity(_Taskname, productionOrder.Code, edgeGroovesWithoutLinkToProductionStep.Count(), "EdgeGrooves", "ProductionStep");
            }
            else
            {
                // Ermitteln Anzahl Nuten zur aktuellen Bauteilseite (AlternateCode) und Aggregat (MacroNumber) für aktuellen Step
                int quantityEdgeGrooves = currentEdgeGrooves.Count(eg => eg.AlternateCode == currentGrooveInProcessWithMacroNumber.GrooveInProcess &&
                                                                         eg.MacroNumber == currentGrooveInProcessWithMacroNumber.MacroNumberForGroove &&
                                                                         eg.ProductionStepCode == productionStep.Code);

                if (quantityEdgeGrooves == 0)
                {
                    success = false;

                    // {0}: FA='{1}', Route='{2}', Arbeitsgang='{3}', ShapePass.Sequence='{4}': Erwartete Nute nicht vorhanden in Tabelle EdgeGrooves für Kante '{5}' und MacroNumber '{6}'.
                    returnErrorMessage = ResourcesKeys.ErrorEdgePassesRequiredEdgeGroove(_Taskname, productionOrder.Code, possibleRoute.Code,
                        productionStep.Code + " / " + productionStep.Order, currentShapePass.Sequence, currentGrooveInProcessWithMacroNumber.GrooveInProcess, currentGrooveInProcessWithMacroNumber.MacroNumberForGroove);
                }
                /*
                else if (quantityEdgeGrooves > 1)
                {
                    success = false;

                    // {0}: FA='{1}', Route='{2}', Arbeitsgang='{3}', ShapePass.Sequence='{4}': '{5}' Nuten gefunden, nur eine erlaubt in Tabelle EdgeGrooves für Kante '{6}' und MacroNumber '{7}'.
                    returnErrorMessage = ResourcesKeys.ErrorEdgePassesQuantityEdgeGrooves(_Taskname, productionOrder.Code, possibleRoute.Code,
                        productionStep.Code + " / " + productionStep.Order, currentShapePass.Sequence, quantityEdgeGrooves, currentGrooveInProcessWithMacroNumber.GrooveInProcess, currentGrooveInProcessWithMacroNumber.MacroNumberForGroove);
                }
                */
            }
        }

        return success;
    }

}
