using System.ComponentModel;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IUserExit))]
[Export("Custom_Select_08mm_Parts_White", typeof(ControllerMES.Infrastructure.BaseCommon.Contracts.IAutomaticOptimizationRuleSelectionUserExit))]
[Description("Select 08mm Parts ALG")]
[EnabledScript(true)]
public class Custom_Select_08mm_Parts_White : HomagGroup.FLS.Infrastructure.Common.Customization.UserExitBase, ControllerMES.Infrastructure.BaseCommon.Contracts.IAutomaticOptimizationRuleSelectionUserExit
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;
    /// </summary>
    /// <param name="executionContext"></param>
    /// <param name="optimizationRulesAllocation"></param>
    /// <param name="itemsForAutomaticLotGeneration"></param>
    
    public void SelectProductionItemsToOptimize(HomagGroup.FLS.Services.Common.Contracts.JobScheduling.IJobExecutionContext executionContext, HomagGroup.FLS.Domain.Data.OptimizationRulesAllocation optimizationRulesAllocation, ControllerMES.Infrastructure.Common.PublicClasses.ItemsForAutomaticLotGeneration itemsForAutomaticLotGeneration)
    {
        Guard.ThrowOnArgumentNull(executionContext, "executionContext");
        Guard.ThrowOnArgumentNull(itemsForAutomaticLotGeneration,"itemsForLotGeneration");
        
        using (var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
        {
            var workCenterCode = itemsForAutomaticLotGeneration.WorkCenterCode;
            var quantityMandatoryItems = 30;
            var prodItems = unitOfWork.GetRepository<ProductionItem>().Get(
                    pi => pi.OptimizationTransferState == OptimizationTransferState.NotOptimized &&
                          pi.ProductionOrder.Material == "DTP_NGR_PRL_08_WHITE" &&
                          pi.ProductionOrder.ProductionSteps.Any(
                                ps => ps.WorkCenterCode == workCenterCode && ps.DisposeState == DisposeState.Scheduled),
                "ProductionOrder.DesiredEndDate,ProductionOrder.Sequence").Take(quantityMandatoryItems);
                
            if (prodItems.Any())
                itemsForAutomaticLotGeneration.MandatoryProductionItems.AddRange(prodItems);
        }
    }
}
