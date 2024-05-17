using System.ComponentModel;



[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IUserExit))]
[Export("CustomSelectDailyBulks", typeof(ControllerMES.Infrastructure.BaseCommon.Contracts.IAutomaticOptimizationRuleSelectionUserExit))]
[Description("Select daily bulks for optimization")]
[EnabledScript(true)]
public class CustomSelectDailyBulks : HomagGroup.FLS.Infrastructure.Common.Customization.UserExitBase, ControllerMES.Infrastructure.BaseCommon.Contracts.IAutomaticOptimizationRuleSelectionUserExit
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
            var manualBulksRep = unitOfWork.GetRepository<ManualBulk>();
            var prodItemsRep = unitOfWork.GetRepository<ProductionItem>();
            var today = DateTime.Today;
                        
            var dailyBulk = manualBulksRep.GetFirstOrDefault(mb => mb.PlanningNumber.Contains("Bulk"));
            
            // Mandatory parts of blue bulk
            var prodItemsMand = prodItemsRep.GetQueryable(false).Where(
                    pi => pi.OptimizationTransferState == OptimizationTransferState.NotOptimized &&
                          pi.ProductionOrder.OrderType == ProductionOrderType.ConstructionPart &&
                          pi.ProductionOrder.ComponentType != ComponentType.BackPanel &&
                          pi.ProductionOrder.ComponentType != ComponentType.DrawerBottom &&
                          pi.ProductionOrder.ProductionSteps.Any(
                                ps =>   ps.WorkCenterCode == workCenterCode &&    
                                        ps.DisposeState == DisposeState.Scheduled &&
                                        ps.DesiredStartDateProcessing == today 
     //                                   pi.ProductionOrder.PlanningSequence == dailyBulk.Sequence
                                        ));
            if (prodItemsMand.Any())
                itemsForAutomaticLotGeneration.MandatoryProductionItems.AddRange(prodItemsMand);
        }
    }
}
