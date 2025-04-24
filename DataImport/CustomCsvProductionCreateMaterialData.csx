#r ControllerMES.Infrastructure.Common
#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   CustomCsvProductionCreateMaterialData
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         D.Strom
//   Date:           2022-09-01
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   D.Strom         2022-09-01    Created
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IServerCustomization))]
[Export("CustomCsvProductionCreateMaterialData", typeof(HomagGroup.FLS.Services.DataImport.Contracts.Contracts.UserExits.IDataImportTransformationUserExit))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("Anlage Materialdatensatz")]
[EnabledScript(true)]
public class CustomCsvProductionCreateMaterialData : UserExitCustomBase, HomagGroup.FLS.Services.DataImport.Contracts.Contracts.UserExits.IDataImportTransformationUserExit
{
    private Logger _Logger;

    public void Execute(IJobExecutionContext executionContext, SourceItem source, TargetItem target)
    {
        Guard.ThrowOnArgumentNull(executionContext, "executionContext");
        Guard.ThrowOnArgumentNull(source, "source");
        Guard.ThrowOnArgumentNull(target, "target");

        try
        {
            _Logger = LogHelper.GetLogger(executionContext.Area, typeof(CustomCsvProductionCreateMaterialData));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);

            var productionOrder = target.Value as ProductionOrder;


            if (productionOrder != null)
            {
                // Material
                string material = productionOrder.Material;
                
                
                if (!string.IsNullOrEmpty(material))
                {
                    CreateResourceData(productionOrder,material,"_4",Convert.ToInt32(productionOrder.DesiredTargetQuantity)
                        ,ResourceType.Material,productionOrder.Thickness,material,"","Material",(Grain)productionOrder.Grain,productionOrder.MaterialCategory);                 
                }
            }
            
            
        }
        catch (Exception e)
        {
            executionContext.JobResultSetComplete(JobResult.Failed);
            _Logger.Error(ResourcesKeys.ErrorInUserExit(Name), null, e);
            throw;
        }
    }

    public void CreateResourceData(ProductionOrder productionOrder, string description, string codeSuffix, int qty
            , ResourceType resType, decimal thickness, string material, string addInfo, string type, Grain grain , string category)
    {
        ProductionOrdersResource productionOrderResource = new ProductionOrdersResource()
        {
            ProductionOrderCode = productionOrder.Code,
            Description = description,
            Code = productionOrder.Code + codeSuffix,
            DesiredQuantity = qty,
            QuantityUnit = "Stk",
            Order = 1,
            InternalType = resType,
            Length = productionOrder.Length,
            Width = productionOrder.Width,
            Thickness = thickness,
            Material = material,
            AdditionalInformation = addInfo,
            Type = type,
            Grain = grain,
        };
        productionOrder.ProductionOrdersResources.Add(productionOrderResource);
    }
    
    public override ICollection<UserExitParameter> UserExitInputParameters
    {
        get
        {
            return new List<UserExitParameter>
            {
                // Example for new parameter:
                // new UserExitParameter("MyParameter", typeof(string), true)
            };
        }
    }
}
