#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   CustomConditionTest2
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         <C.Wölfl>
//   Date:           2023-06-12
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   <C.Wölfl>        2023-06-12    Created
//   
//-----------------------------------------------------------------------------

using System.ComponentModel;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IUserExit))]
[Export("CustomConditionTest2", typeof(ControllerMES.Infrastructure.BaseCommon.Contracts.IAutomaticOptimizationRuleConditionUserExit))]
[Description("Condition2:No Old Parts Remaining")]
[EnabledScript(false)]
public class CustomConditionTest2 : HomagGroup.FLS.Infrastructure.Common.Customization.UserExitBase, ControllerMES.Infrastructure.BaseCommon.Contracts.IAutomaticOptimizationRuleConditionUserExit
{
    private Logger logger{get;set;}
    private string _Taskname = "CustomConditionTest2";
    
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
        
        logger = LogHelper.GetLogger("LotGeneration", typeof(CustomConditionTest));
        logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);
        
        bool returnValue = false;
        
		using (var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
        {  
            // Old Parts
            var oldParts = unitOfWork.GetRepository<CustViewAutomaticLotGenerationSelectOldPartsByCount>().Get().FirstOrDefault();
            
            logger.Info("Kartoffel2");       
            
            if(oldParts != null)
            {
                returnValue = false;
                logger.Info("Karotten2");   
            }
            
            else
            {
                returnValue = true;
                logger.Info("Kürbis2");
            }
        }
        
        return returnValue;
        
            logger.Info(string.Format("{0}: Value: [{1}]", _Taskname, returnValue));
            logger.Error(string.Format("{0}: Value: [{1}]", _Taskname, returnValue));
    }
}
