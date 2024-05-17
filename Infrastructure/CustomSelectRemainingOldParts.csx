using System.ComponentModel;



[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IUserExit))]
[Export("CustomSelectRemainingOldParts", typeof(ControllerMES.Infrastructure.BaseCommon.Contracts.IAutomaticOptimizationRuleSelectionUserExit))]
[Description("Select old MandatoryParts + OptionalParts")]
[EnabledScript(true)]
public class CustomSelectRemainingOldParts : HomagGroup.FLS.Infrastructure.Common.Customization.UserExitBase, ControllerMES.Infrastructure.BaseCommon.Contracts.IAutomaticOptimizationRuleSelectionUserExit
{
    private Logger logger{get;set;}
    
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="executionContext"></param>
    /// <param name="optimizationRulesAllocation"></param>
    /// <param name="itemsForAutomaticLotGeneration"></param>
    /// <returns></returns>
    
    
    public void SelectProductionItemsToOptimize(HomagGroup.FLS.Services.Common.Contracts.JobScheduling.IJobExecutionContext executionContext, HomagGroup.FLS.Domain.Data.OptimizationRulesAllocation optimizationRulesAllocation, ControllerMES.Infrastructure.Common.PublicClasses.ItemsForAutomaticLotGeneration itemsForAutomaticLotGeneration)
    {
        Guard.ThrowOnArgumentNull(executionContext, "executionContext");
        Guard.ThrowOnArgumentNull(itemsForAutomaticLotGeneration,"itemsForLotGeneration");
        logger = LogHelper.GetLogger("LotGeneration", typeof(CustomSelectRemainingOldParts));
        logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);
        
        
        using (var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
        {
            var workCenterCode = itemsForAutomaticLotGeneration.WorkCenterCode;
            var prodItemsRep = unitOfWork.GetRepository<ProductionItem>();
            var today = DateTime.Today;
            logger.Info(string.Format("Today: {0}", today));
            
                                    
            var prodItemsMand = prodItemsRep.GetQueryable(false).Where(
                    pi => pi.OptimizationTransferState == OptimizationTransferState.NotOptimized &&
                          pi.ProductionOrder.OrderType == ProductionOrderType.ConstructionPart &&
                          pi.ProductionOrder.ProductionSteps.Any(
                                ps => ps.WorkCenterCode == workCenterCode && ps.DisposeState == DisposeState.Scheduled && ps.DesiredStartDateProcessing < today));
            if (prodItemsMand.Any())
                    itemsForAutomaticLotGeneration.MandatoryProductionItems.AddRange(prodItemsMand);
    
        }
    }
}
