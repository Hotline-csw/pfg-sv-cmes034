using System.ComponentModel;



[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IUserExit))]
[Export("CustomConditionPostProductionParts", typeof(ControllerMES.Infrastructure.BaseCommon.Contracts.IAutomaticOptimizationRuleConditionUserExit))]
[Description("Condition: post-production parts must exist")]
[EnabledScript(true)]
public class CustomConditionPostProductionParts : HomagGroup.FLS.Infrastructure.Common.Customization.UserExitBase, ControllerMES.Infrastructure.BaseCommon.Contracts.IAutomaticOptimizationRuleConditionUserExit
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
    
    public bool IsConditionFulfilled(HomagGroup.FLS.Services.Common.Contracts.JobScheduling.IJobExecutionContext executionContext, HomagGroup.FLS.Domain.Data.OptimizationRulesAllocation optimizationRulesAllocation)
    {
        Guard.ThrowOnArgumentNull(executionContext, nameof(executionContext));
        Guard.ThrowOnArgumentNull(optimizationRulesAllocation, nameof(optimizationRulesAllocation));
        
        logger = LogHelper.GetLogger("LotGeneration", typeof(CustomConditionPostProductionParts));
        logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);
        
        bool returnValue = false;
        
		using (var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
        {
            var prodItemsRep = unitOfWork.GetRepository<ProductionItem>();
            
            // Post-production parts
            var postProdParts = prodItemsRep.GetQueryable(false).Where(
                    pi => pi.OptimizationTransferState == OptimizationTransferState.NotOptimized &&
                          pi.ProductionOrder.OrderType == ProductionOrderType.ConstructionPart &&
                          pi.ProductionOrder.ReproductionType == ReproductionType.Standard &&
                          !pi.ProductionItemsHistory.Any());
            
            if(postProdParts != null)
            {
                returnValue = true;
            }
            
            else
            {
                returnValue = false;
            }
        }
        
        return returnValue;
    }
}
