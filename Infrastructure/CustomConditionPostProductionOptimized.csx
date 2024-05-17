using System.ComponentModel;



[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IUserExit))]
[Export("CustomConditionPostProductionOptimized", typeof(ControllerMES.Infrastructure.BaseCommon.Contracts.IAutomaticOptimizationRuleConditionUserExit))]
[Description("Condition: All post-production parts are optimized")]
[EnabledScript(true)]
public class CustomConditionPostProductionOptimized : HomagGroup.FLS.Infrastructure.Common.Customization.UserExitBase, ControllerMES.Infrastructure.BaseCommon.Contracts.IAutomaticOptimizationRuleConditionUserExit
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
        
        logger = LogHelper.GetLogger("LotGeneration", typeof(CustomConditionPostProductionOptimized));
        logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);
        
        bool returnValue = false;
        
		using (var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
        {
            var prodItemsRep = unitOfWork.GetRepository<ProductionItem>();
            
            int minPostProdOpt = 2;
            
            // Post-production parts optimized
            var countPostProdPartsOpti = prodItemsRep.GetCount(
                    pi => pi.OptimizationTransferState == OptimizationTransferState.Optimized &&
                          pi.ProductionOrder.ReproductionType == ReproductionType.Standard);
            
            if(countPostProdPartsOpti >= minPostProdOpt)
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
