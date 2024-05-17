using System.ComponentModel;



[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IUserExit))]
[Export("CustomSelectOldParts", typeof(ControllerMES.Infrastructure.BaseCommon.Contracts.IAutomaticOptimizationRuleSelectionUserExit))]
[Description("Select-UE for Parts with ProductionStartDate < Today")]
[EnabledScript(true)]
public class CustomSelectOldParts : HomagGroup.FLS.Infrastructure.Common.Customization.UserExitBase, ControllerMES.Infrastructure.BaseCommon.Contracts.IAutomaticOptimizationRuleSelectionUserExit
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
        logger = LogHelper.GetLogger("LotGeneration", typeof(CustomSelectDailyProductionParts));
        logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);
        
        
        using (var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
        {
            var Test = optimizationRulesAllocation.UserExitParameter;
            var workCenterCode = itemsForAutomaticLotGeneration.WorkCenterCode;
            var prodItemsRep = unitOfWork.GetRepository<ProductionItem>();
            
             var maxMaterial = unitOfWork.GetRepository<CustViewAutomaticLotGenerationSelectOldPartsByCount>()
                    .Get().OrderByDescending(alg => alg.EntryCount).FirstOrDefault();
                    
            var remaining = DateTime.Today.AddDays(-14);
            var today = DateTime.Today;
            
            logger.Info(string.Format("Remaining: {0}",remaining));
            logger.Info(string.Format("Today: {0}", today));
            
                                    
            var prodItemsMand = prodItemsRep.GetQueryable(false).Where(
                    pi => pi.OptimizationTransferState == OptimizationTransferState.NotOptimized &&
                          pi.ProductionOrder.OrderType == ProductionOrderType.ConstructionPart &&
                          pi.ProductionOrder.Material == maxMaterial.Material &&
                          pi.ProductionOrder.ProductionSteps.Any(
                                ps => ps.WorkCenterCode == workCenterCode && ps.DisposeState == DisposeState.Scheduled && (ps.DesiredStartDateProcessing >= remaining) && (ps.DesiredStartDateProcessing < today)));
            if (prodItemsMand.Any())
                    itemsForAutomaticLotGeneration.MandatoryProductionItems.AddRange(prodItemsMand);
    
            var prodItemsOpt = prodItemsRep.GetQueryable(false).Where(
                    pi => pi.OptimizationTransferState == OptimizationTransferState.NotOptimized &&
                          pi.ProductionOrder.OrderType == ProductionOrderType.ConstructionPart &&
                          pi.ProductionOrder.Material == maxMaterial.Material &&
                          pi.ProductionOrder.ProductionSteps.Any(
                                ps => ps.WorkCenterCode == workCenterCode && ps.DisposeState == DisposeState.Scheduled && ps.DesiredStartDateProcessing == today));
            if (prodItemsOpt.Any())
                    itemsForAutomaticLotGeneration.OptionalProductionItems.AddRange(prodItemsOpt);
        }
    }
}
