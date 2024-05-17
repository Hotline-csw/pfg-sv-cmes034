using System.ComponentModel;



[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IUserExit))]
[Export("CustomSelectBlackDarkBlueBulk", typeof(ControllerMES.Infrastructure.BaseCommon.Contracts.IAutomaticOptimizationRuleSelectionUserExit))]
[Description("Select black and dark-blue bulk as mandatory")]
[EnabledScript(true)]
public class CustomSelectBlackDarkBlueBulk : HomagGroup.FLS.Infrastructure.Common.Customization.UserExitBase, ControllerMES.Infrastructure.BaseCommon.Contracts.IAutomaticOptimizationRuleSelectionUserExit
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
        
        logger = LogHelper.GetLogger("LotGeneration", typeof(CustomSelectBlackDarkBlueBulk));
		logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);
		
		using (var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
        {
            var workCenterCode = itemsForAutomaticLotGeneration.WorkCenterCode;
            var manualBulksRep = unitOfWork.GetRepository<ManualBulk>();
            var prodItemsRep = unitOfWork.GetRepository<ProductionItem>();
                        
            var blackBulk = manualBulksRep.GetFirstOrDefault(mb => mb.PlanningNumber == "Bulk-Black");
            var darkBlueBulk = manualBulksRep.GetFirstOrDefault(mb => mb.PlanningNumber == "Bulk-Dark-Blue");
            
            // Mandatory parts
            var prodItemsMand = prodItemsRep.GetQueryable(false).Where(
                    pi => // Bulk-Black
                          pi.OptimizationTransferState == OptimizationTransferState.NotOptimized &&
                          pi.ProductionOrder.OrderType == ProductionOrderType.ConstructionPart &&
                          pi.ProductionOrder.ComponentType != ComponentType.BackPanel &&
                          pi.ProductionOrder.ComponentType != ComponentType.DrawerBottom &&
                          pi.ProductionOrder.ProductionSteps.Any(
                                ps => ps.WorkCenterCode == workCenterCode && ps.DisposeState == DisposeState.Scheduled) &&
                          pi.ProductionOrder.PlanningSequence == blackBulk.Sequence ||
                          // Bulk-Dark-Blue
                          pi.OptimizationTransferState == OptimizationTransferState.NotOptimized &&
                          pi.ProductionOrder.OrderType == ProductionOrderType.ConstructionPart &&
                          pi.ProductionOrder.ComponentType != ComponentType.BackPanel &&
                          pi.ProductionOrder.ComponentType != ComponentType.DrawerBottom &&
                          pi.ProductionOrder.ProductionSteps.Any(
                                ps => ps.WorkCenterCode == workCenterCode && ps.DisposeState == DisposeState.Scheduled) &&
                          pi.ProductionOrder.PlanningSequence == darkBlueBulk.Sequence);
                          
            if (prodItemsMand.Any())
                itemsForAutomaticLotGeneration.MandatoryProductionItems.AddRange(prodItemsMand);
        }
    }
}
