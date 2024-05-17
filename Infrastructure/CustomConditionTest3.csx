#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   CustomConditionTest3
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         <Author>
//   Date:           2023-06-28
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   <Author>        2023-06-28    Created
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;
using System.Collections;

[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IUserExit))]
[Export("CustomConditionTest3", typeof(ControllerMES.Infrastructure.BaseCommon.Contracts.IAutomaticOptimizationRuleConditionUserExit))]
[Description("describe here")]
[EnabledScript(true)]
public class CustomConditionTest3 : HomagGroup.FLS.Infrastructure.Common.Customization.UserExitBase, ControllerMES.Infrastructure.BaseCommon.Contracts.IAutomaticOptimizationRuleConditionUserExit
{

	[Import]
	private IUnitOfWorkFactory _UnitOfWorkFactory;

	private Logger _Logger;
	
	public bool IsConditionFulfilled(HomagGroup.FLS.Services.Common.Contracts.JobScheduling.IJobExecutionContext executionContext, HomagGroup.FLS.Domain.Data.OptimizationRulesAllocation optimizationRulesAllocation)
	{
		Guard.ThrowOnArgumentNull(executionContext, "executionContext");
		
		try
		{
			_Logger = LogHelper.GetLogger(executionContext.Area, typeof(CustomConditionTest3));
			_Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);
			
			//Example for set the parameters from Contidtion UE to Selection UE
			executionContext.AddOrUpdateInput("Date",DateTime.Now.ToString("yyyyMMdd"));
			
			
			throw new NotImplementedException();
		}
		catch (Exception e)
		{
			_Logger.Error(ResourcesKeys.ErrorInUserExit("CustomConditionTest3"), null, e);
			throw;
		}
	}
}

