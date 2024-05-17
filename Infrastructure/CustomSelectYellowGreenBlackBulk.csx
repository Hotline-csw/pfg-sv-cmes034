using System.ComponentModel;



[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IUserExit))]
[Export("CustomSelectYellowGreenBlackBulk", typeof(ControllerMES.Infrastructure.BaseCommon.Contracts.IAutomaticOptimizationRuleSelectionUserExit))]
[Description("Select yellow and green bulk as mandatory - black bulk as optional")]
[EnabledScript(true)]
public class CustomSelectYellowGreenBlackBulk : HomagGroup.FLS.Infrastructure.Common.Customization.UserExitBase, ControllerMES.Infrastructure.BaseCommon.Contracts.IAutomaticOptimizationRuleSelectionUserExit
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
        
        logger = LogHelper.GetLogger("LotGeneration", typeof(CustomSelectYellowGreenBlackBulk));
		logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);
		
		
		using (var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
        {
            var workCenterCode = itemsForAutomaticLotGeneration.WorkCenterCode;
            var manualBulksRep = unitOfWork.GetRepository<ManualBulk>();
            var prodItemsRep = unitOfWork.GetRepository<ProductionItem>();
                        
            var yellowBulk = manualBulksRep.GetFirstOrDefault(mb => mb.PlanningNumber == "Bulk-Yellow");
            var greenBulk = manualBulksRep.GetFirstOrDefault(mb => mb.PlanningNumber == "Bulk-Green");
            var blackBulk = manualBulksRep.GetFirstOrDefault(mb => mb.PlanningNumber == "Bulk-Black");
            
            // Mandatory parts of yellow and green bulk
            var prodItemsMand = prodItemsRep.GetQueryable(false).Where(
                    pi => pi.OptimizationTransferState == OptimizationTransferState.NotOptimized &&
                          pi.ProductionOrder.OrderType == ProductionOrderType.ConstructionPart &&
                          pi.ProductionOrder.ComponentType != ComponentType.BackPanel &&
                          pi.ProductionOrder.ComponentType != ComponentType.DrawerBottom &&
                          pi.ProductionOrder.ProductionSteps.Any(
                                ps => ps.WorkCenterCode == workCenterCode && ps.DisposeState == DisposeState.Scheduled) &&
                          pi.ProductionOrder.PlanningSequence == yellowBulk.Sequence ||
                          pi.OptimizationTransferState == OptimizationTransferState.NotOptimized &&
                          pi.ProductionOrder.OrderType == ProductionOrderType.ConstructionPart &&
                          pi.ProductionOrder.ComponentType != ComponentType.BackPanel &&
                          pi.ProductionOrder.ComponentType != ComponentType.DrawerBottom &&
                          pi.ProductionOrder.ProductionSteps.Any(
                                ps => ps.WorkCenterCode == workCenterCode && ps.DisposeState == DisposeState.Scheduled) &&
                          pi.ProductionOrder.PlanningSequence == greenBulk.Sequence);
            if (prodItemsMand.Any())
                itemsForAutomaticLotGeneration.MandatoryProductionItems.AddRange(prodItemsMand);
           
           
           // Optional parts of black
            var prodItemsOpt = prodItemsRep.GetQueryable(false).Where(
                    pi => pi.OptimizationTransferState == OptimizationTransferState.NotOptimized &&
                          pi.ProductionOrder.OrderType == ProductionOrderType.ConstructionPart &&
                          pi.ProductionOrder.ComponentType != ComponentType.BackPanel &&
                          pi.ProductionOrder.ComponentType != ComponentType.DrawerBottom &&
                          pi.ProductionOrder.ProductionSteps.Any(
                                ps => ps.WorkCenterCode == workCenterCode && ps.DisposeState == DisposeState.Scheduled) &&
                          pi.ProductionOrder.PlanningSequence == blackBulk.Sequence);
            if (prodItemsOpt.Any())
                itemsForAutomaticLotGeneration.OptionalProductionItems.AddRange(prodItemsOpt);
        }
	}
}