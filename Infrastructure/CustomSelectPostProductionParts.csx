using System.ComponentModel;



[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IUserExit))]
[Export("CustomSelectPostProductionParts", typeof(ControllerMES.Infrastructure.BaseCommon.Contracts.IAutomaticOptimizationRuleSelectionUserExit))]
[Description("Select post-production parts")]
[EnabledScript(true)]
public class CustomSelectPostProductionParts : HomagGroup.FLS.Infrastructure.Common.Customization.UserExitBase, ControllerMES.Infrastructure.BaseCommon.Contracts.IAutomaticOptimizationRuleSelectionUserExit
{
    private Logger logger{get;set;}
    
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
    
        logger = LogHelper.GetLogger("LotGeneration", typeof(CustomSelectPostProductionParts));
		logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);
		
	   using (var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
        {
            var workCenterCode = itemsForAutomaticLotGeneration.WorkCenterCode;
            var prodItemsRep = unitOfWork.GetRepository<ProductionItem>();
            var today = DateTime.Today;
            
            // Post-production parts mandatory
            var prodItemsMand = prodItemsRep.GetQueryable(false).Where(
                    pi => pi.OptimizationTransferState == OptimizationTransferState.NotOptimized &&
                          pi.ProductionOrder.OrderType == ProductionOrderType.ConstructionPart &&
                          pi.ProductionOrder.ReproductionType == ReproductionType.Standard &&
                          !pi.ProductionItemsHistory.Any() &&
                          pi.ProductionOrder.ProductionSteps.Any(ps => ps.WorkCenterCode == workCenterCode && ps.DisposeState == DisposeState.Scheduled));
            if (prodItemsMand.Any())
                itemsForAutomaticLotGeneration.MandatoryProductionItems.AddRange(prodItemsMand);
                
            // Post-production parts optional
            var prodItemsOpt = prodItemsRep.GetQueryable(false).Where(
                    pi => pi.OptimizationTransferState == OptimizationTransferState.NotOptimized &&
                          pi.ProductionOrder.OrderType == ProductionOrderType.ConstructionPart &&
                          !pi.ProductionItemsHistory.Any() &&
                          pi.ProductionOrder.ProductionSteps.Any(ps => ps.WorkCenterCode == workCenterCode && ps.DisposeState == DisposeState.Scheduled && ps.DesiredStartDateProcessing == today));
            if (prodItemsMand.Any())
                itemsForAutomaticLotGeneration.OptionalProductionItems.AddRange(prodItemsOpt);
        }
    }
}
