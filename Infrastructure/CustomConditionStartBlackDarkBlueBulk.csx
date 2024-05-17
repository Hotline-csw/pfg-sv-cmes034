using System.ComponentModel;



[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IUserExit))]
[Export("CustomConditionStartBlackDarkBlueBulk", typeof(ControllerMES.Infrastructure.BaseCommon.Contracts.IAutomaticOptimizationRuleConditionUserExit))]
[Description("Condition: Bulk black and dark blue starts, when bulk yellow/green has enough saw feedbacks")]
[EnabledScript(true)]
public class CustomConditionStartBlackDarkBlueBulk : HomagGroup.FLS.Infrastructure.Common.Customization.UserExitBase, ControllerMES.Infrastructure.BaseCommon.Contracts.IAutomaticOptimizationRuleConditionUserExit
{
    private Logger logger{get;set;}
    
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;
      
    /// <summary>
    /// 
    /// </summary>
    /// <param name="executionContext"></param>
    /// <param name="optimizationRulesAllocation"></param>
    /// <returns></returns>
    
    
    public bool IsConditionFulfilled(IJobExecutionContext executionContext, OptimizationRulesAllocation optimizationRulesAllocation)
    {
        Guard.ThrowOnArgumentNull(executionContext, nameof(executionContext));
        Guard.ThrowOnArgumentNull(optimizationRulesAllocation, nameof(optimizationRulesAllocation));
        logger = LogHelper.GetLogger("LotGeneration", typeof(CustomConditionStartBlackDarkBlueBulk));
        logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);
        
        bool returnValue = false;
        
		using (var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
        {
            var prodItemsRep = unitOfWork.GetRepository<ProductionItem>();
            var manualBulksRep = unitOfWork.GetRepository<ManualBulk>();
            
            var yellowBulk = manualBulksRep.GetFirstOrDefault(mb => mb.PlanningNumber == "Bulk-Yellow");
            var greenBulk = manualBulksRep.GetFirstOrDefault(mb => mb.PlanningNumber == "Bulk-Green");
            var blackBulk = manualBulksRep.GetFirstOrDefault(mb => mb.PlanningNumber == "Bulk-Black");
            
            int minFeedback = 0;
            
            // Bulk-Items
            var countBulkItems = prodItemsRep.GetCount(
                    pi => // Bulk-Yellow
                          pi.OptimizationTransferState == OptimizationTransferState.Optimized &&
                          pi.ProductionOrder.PlanningSequence == yellowBulk.Sequence &&
                          pi.ProductionItemsHistory.Any(pih => pih.WorkCenterCode == "1010") &&
                          pi.ProductionItemsHistory.Count() == 1 || //only one feedback from saw
                          // Bulk-Green
                          pi.OptimizationTransferState == OptimizationTransferState.Optimized &&
                          pi.ProductionOrder.PlanningSequence == greenBulk.Sequence &&
                          pi.ProductionItemsHistory.Any(pih => pih.WorkCenterCode == "1010") &&
                          pi.ProductionItemsHistory.Count() == 1 || //only one feedback from saw
                          //Bulk-Black
                          pi.OptimizationTransferState == OptimizationTransferState.Optimized &&
                          pi.ProductionOrder.PlanningSequence == blackBulk.Sequence &&
                          pi.ProductionItemsHistory.Any(pih => pih.WorkCenterCode == "1010") &&
                          pi.ProductionItemsHistory.Count() == 1); //only one feedback from saw
                          
            logger.Info("Leberkas");
            logger.Info(string.Format("Gezählte Bauteile: {0}", countBulkItems));
            logger.Info("ohne Ketchup");
            
            if(countBulkItems >= minFeedback)
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
