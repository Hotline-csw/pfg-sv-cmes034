#r ControllerMES.Infrastructure.Common
#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   CustomWccStagingSetAndCheckValues
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         T.Stürzer
//   Date:           2024-07-04
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   T.Stürzer       2024-07-04    Created
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IServerCustomization))]
[Export("CustomWccStagingSetAndCheckValues", typeof(HomagGroup.FLS.Services.DataImport.Contracts.Contracts.UserExits.IDataImportTransformationUserExit))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("describe here")]
[EnabledScript(true)]
public class CustomWccStagingSetAndCheckValues : WccStagingSetAndCheckValues
{
    private Logger _Logger;

    /// <summary>
    /// Set the ProductionOrderCode at the "targetRecord.Code"
    /// </summary>
    /// <param name="source">The source item.</param>
    /// <param name="targetRecord">The target item</param>
    protected override void SetProductionOrderCodeAtTargetRecord(SourceItem source, WccStagingRecord targetRecord)
    {
        Guard.ThrowOnArgumentNull(source, "source");
        Guard.ThrowOnArgumentNull(targetRecord, "targetRecord");
        
        // Get ICNID and convert it from int to string
        string productionOrderCode = source["ArticleInfo1"].ToString();
        
        if (!String.IsNullOrEmpty(productionOrderCode))
        {
            targetRecord.Code = productionOrderCode;
        }
    }
}
