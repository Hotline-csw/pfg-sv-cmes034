#r ControllerMES.Infrastructure.Common
#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   CustomCreateProductionItems
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         T.Stürzer
//   Date:           2024-07-09
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   T.Stürzer       2024-07-09    Created
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IServerCustomization))]
[Export("CustomCreateProductionItems", typeof(HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("[05] Copy ProductionItemCodes from production order CustomProdItemCode")]
[EnabledScript(true)]
public class CustomCreateProductionItems : CreateProductionItems
{
    /// <summary>
    /// Create new ProductionItems for an order
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="productionOrder"></param>
    protected override void CreateNewProductionItems(Logger logger, ProductionOrder productionOrder)
    {
        Guard.ThrowOnArgumentNull(logger, "logger");
        Guard.ThrowOnArgumentNull(productionOrder, "productionOrder");

        productionOrder.ProductionItems.Add(new ProductionItem
        {
//            Code = "2"+productionOrder.Code,       // Get the ProductionItemCode for a new ProductionItem;  Standard: Create the value by RangeOfNumbers
            Code = productionOrder.CustomProdItemCode,
            DesiredQuantity = productionOrder.DesiredTargetQuantity ?? 1,
            CreationDate = DateTime.Now,
            ModificationDate = DateTime.Now,
            CreationSource = "FLS",
            ModificationSource = "FLS",
        });
    }
}