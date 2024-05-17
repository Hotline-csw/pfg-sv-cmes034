using System.ComponentModel;



[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IUserExit))]
[Export("CustomSelectPartsTest2", typeof(ControllerMES.Infrastructure.BaseCommon.Contracts.IAutomaticOptimizationRuleSelectionUserExit))]
[Description("Test2")]
[EnabledScript(true)]
public class CustomSelectPartsTest2 : HomagGroup.FLS.Infrastructure.Common.Customization.UserExitBase, ControllerMES.Infrastructure.BaseCommon.Contracts.IAutomaticOptimizationRuleSelectionUserExit
{
  
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;
    /// </summary>
    /// <param name="executionContext"></param>
    /// <param name="optimizationRulesAllocation"></param>
    /// <param name="itemsForAutomaticLotGeneration"></param>
    public void SelectProductionItemsToOptimize(IJobExecutionContext executionContext, OptimizationRulesAllocation optimizationRulesAllocation, ItemsForAutomaticLotGeneration itemsForAutomaticLotGeneration)
    {
        Guard.ThrowOnArgumentNull(executionContext, "executionContext");
        Guard.ThrowOnArgumentNull(itemsForAutomaticLotGeneration,"itemsForLotGeneration");
    
        using (var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
        {
            var minQuantityMandatoryItems = 5;
            var minQuantityOptionalItems = 0;
            var maxQuantityOptionalItems = 30;
            var maxQuantityAllParts = 50;
            
            var workCenterCode = itemsForAutomaticLotGeneration.WorkCenterCode;
            var productionItems = unitOfWork.GetRepository<ProductionItem>()
                .Get(pi => pi.OptimizationTransferState == OptimizationTransferState.NotOptimized &&
                           pi.ProductionOrder.Material != null &&
                           pi.ProductionOrder.ProductionSteps.Any( ps => ps.WorkCenterCode == workCenterCode && ps.DisposeState == DisposeState.Scheduled ),
                    "ProductionOrder.DesiredEndDate,ProductionOrder.Sequence").Take(minQuantityMandatoryItems);
            if (productionItems.Any())
                itemsForAutomaticLotGeneration.MandatoryProductionItems.AddRange(productionItems);
    
            productionItems = unitOfWork.GetRepository<ProductionItem>()
                .Get(pi => pi.OptimizationTransferState == OptimizationTransferState.NotOptimized &&
                           pi.ProductionOrder.Material != null &&
                           pi.ProductionOrder.ProductionSteps.Any(ps => ps.WorkCenterCode == workCenterCode && ps.DisposeState == DisposeState.Scheduled),
                    "ProductionOrder.DesiredEndDate,ProductionOrder.Sequence").Take(minQuantityOptionalItems);
            productionItems = productionItems.Skip(minQuantityMandatoryItems);
            if (productionItems.Any())
                itemsForAutomaticLotGeneration.OptionalProductionItems.AddRange(productionItems);
        }
    }

}

