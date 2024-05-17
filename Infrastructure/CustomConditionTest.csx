#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   CustomConditionTest
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
[Export("CustomConditionTest", typeof(ControllerMES.Infrastructure.BaseCommon.Contracts.IAutomaticOptimizationRuleConditionUserExit))]
[Description("Condition: Old Parts Remaining")]
[EnabledScript(true)]
public class CustomConditionTest : HomagGroup.FLS.Infrastructure.Common.Customization.UserExitBase, ControllerMES.Infrastructure.BaseCommon.Contracts.IAutomaticOptimizationRuleConditionUserExit
{
    private Logger logger{get;set;}
    private string _Taskname = "CustomConditionTest";
    
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
            
            logger.Info("Kartoffel");       
            
            if(oldParts != null)
            {
                returnValue = true;
                logger.Info("Karotten");   
            }
            
            else
            {
                returnValue = false;
                logger.Info("Kürbis");
            }
        }
        
        return returnValue;
        
            logger.Info(string.Format("{0}: Value: [{1}]", _Taskname, returnValue));
            logger.Error(string.Format("{0}: Value: [{1}]", _Taskname, returnValue));
    }
}
