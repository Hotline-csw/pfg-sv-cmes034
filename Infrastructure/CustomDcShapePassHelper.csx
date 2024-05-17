//-----------------------------------------------------------------------------
//   (Class-)Name:   CustomDcShapePassHelper
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         T.Stürzer
//   Date:           2023-04-21
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   T.Stürzer        2023-04-21    Created
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;


[Export(typeof(IDcShapePassHelper))]
[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IServerCustomization))]
[Export("CustomDcShapePassHelper", typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IServerCustomization))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("Overwrite DcShapePassHelper - shorten the groove string")]
[EnabledScript(true)]
public class CustomDcShapePassHelper : DcShapePassHelper
{
    /// <summary>
    /// Adjusts a GrooveShape for use as a search-value in the data-completion 'GetShapePass'.
    /// </summary>
    /// <param name="sourceValues">string[] - array of GrooveShapes to be concatenated.</param>
    /// <param name="productionOrder">Current instance of ProductionOrder.</param>
    /// <param name="productionStep">Current instance of ProductionStep.</param>
    /// <returns>string - adjusted string.</returns>
    protected override string GetDataCompletionStringFromGrooveShape(string[] sourceValues,
                                                                    ProductionOrder productionOrder, 
                                                                    ProductionStep productionStep)
    {
        Guard.ThrowOnArgumentNull(sourceValues, "sourceValues");
        // productionOrder can be null.
        // productionStep can be null.


        string returnValue = string.Empty;

        
        // TODO: if parameter 'productionOrder' is needed, check for it to be not null !
        // TODO: if parameter 'productionStep' is needed, check for it to be not null !

        //--------------------------------------------------------------------------------
        // Example for project-specific adjustment of GrooveShape-value for DC-input-data:
        //      reduce GrooveShape-string, which is used for DC-input-value
        //      => take single parts of the standard-GrooveShape-string
        //--------------------------------------------------------------------------------

        // loop over possibly multiple GrooveShapes
        if (sourceValues.Length > 0)
        {
            bool isFirstElm = true;

            // "N10VGB083"
            foreach (string sourceValue in sourceValues)
            {
                string returnValueTmp = string.Format(CultureInfo.InvariantCulture, "{0}{1}"
                    , sourceValue.Left(1) // "N"
                    , sourceValue.Mid(1,2)); // "10"
                    //, sourceValue.Mid(3, 1) // "V"
                    //, sourceValue.Mid(4, 1) // "G"
                    //, sourceValue.Mid(5, 1) // "B"
                    //, sourceValue.Mid(6, 3)); // "110"

                if (isFirstElm)
                {
                    returnValue = returnValueTmp;
                }
                else
                {
                    returnValue = string.Format(CultureInfo.InvariantCulture, "{0}_{1}", returnValue, returnValueTmp);
                }

                isFirstElm = false;
            }
        }

        return returnValue;
        

        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //      consider changes also in method                                         !!
        //      DcMakroGrooveHelper::GetDataCompletionStringFromGrooveShape()           !!
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

/*
        // Standard: consider only first GrooveShape from input-list
        if (sourceValues.Length > 0)
            returnValue = sourceValues.FirstOrDefault();

        return returnValue;
*/
    }
}
