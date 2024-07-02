#r ControllerMES.Infrastructure.Common
#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   CalculateSalesItemsVolume
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         T.Stürzer
//   Date:           2023-02-20
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   T.Stürzer       2023-02-20    Created
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IServerCustomization))]
[Export("CalculateSalesItemsVolume", typeof(HomagGroup.FLS.Services.DataImport.Contracts.Contracts.UserExits.IDataImportTransformationUserExit))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("[03] Calculate volume of sales items")]
[EnabledScript(true)]
public class CalculateSalesItemsVolume : UserExitCustomBase, HomagGroup.FLS.Services.DataImport.Contracts.Contracts.UserExits.IDataImportTransformationUserExit
{
    private Logger _Logger;

    public void Execute(IJobExecutionContext executionContext, SourceItem source, TargetItem target)
    {
        Guard.ThrowOnArgumentNull(executionContext, "executionContext");
        Guard.ThrowOnArgumentNull(source, "source");
        Guard.ThrowOnArgumentNull(target, "target");

        try
        {
            _Logger = LogHelper.GetLogger(executionContext.Area, typeof(CalculateSalesItemsVolume));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);

            ProductionOrder productionOrder = target.Value as ProductionOrder;
            if (productionOrder != null)
            {
                if (productionOrder.OrderType == ProductionOrderType.SalesArticle)      
                {
                    if (productionOrder.Length > 0 && productionOrder.Width > 0 && productionOrder.Thickness > 0)
                    {
                        productionOrder.CustomVolume = Math.Round((productionOrder.Length * productionOrder.Width * productionOrder.Thickness / 10000000000),2);
                    }
                    
                    else
                     {
                         _Logger.Error("Article ProductionOrder: " + productionOrder.Code + " Length/Width/Thickness is missing!");
                     }
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
