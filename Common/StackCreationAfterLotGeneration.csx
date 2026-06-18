#r ControllerMES.Infrastructure.Common
#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   StackCreationAfterLotGeneration
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         <Author>
//   Date:           2024-06-26
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   <Author>        2024-06-26    Created
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IServerCustomization))]
[Export("StackCreationAfterLotGeneration", typeof(HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("Get cutting plans from lot generation and bulk it into stacks")]
[EnabledScript(true)]
public class StackCreationAfterLotGeneration : GenericTaskBase, HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;

    [Import]
    protected RangeOfNumbersHelper _RangeOfNumbersHelper;

    private Logger _Logger;

    public override void Execute(IJobExecutionContext executionContext)
    {
        Guard.ThrowOnArgumentNull(executionContext, "executionContext");

        try
        {
            _Logger = LogHelper.GetLogger(executionContext.Area, typeof(StackCreationAfterLotGeneration));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);

			using (var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
			{
				var cuttingPlans = unitOfWork.GetRepository<OptimizationCuttingPlan>().Get(a => a.CuttingPlanState == HomagGroup.FLS.Domain.Data.CuttingPlanState.Released);
				var stack = new HomagGroup.FLS.Domain.Data.Stack();
				stack.StackCode = Convert.ToString(_RangeOfNumbersHelper.GetNewUniqueIdentifier(_Logger, Name));
				stack.IsActive = YesNo.Yes;
				stack.IsReserved = YesNo.Yes;
				stack.IsValid = YesNo.Yes;
				stack.LayerLayout = "-";
				stack.StackLength = 0;
				stack.StackWidth = 0;
				stack.StackHeight = 0;
				stack.CustomStackType = StackType.StackFromOptimization;
				stack.CustomStackState = StackState.StackCreated;
				
				foreach (var plan in cuttingPlans)
				{
					var optimizationPartsOfCuttingPlan = unitOfWork.GetRepository<OptimizationPart>().Get(a => a.OptimizationCode == plan.OptimizationCode 
					&& a.OptimizationCuttingPlanCode == plan.Code
					&& a.Offcut == 0); // Ergänzt am 2026-06-18
					foreach (var part in optimizationPartsOfCuttingPlan)
					{
						var stackItem = new HomagGroup.FLS.Domain.Data.StackItem();
						stack.StackItems.Add(stackItem);
						
						stackItem.LayerNumber = 0;
						stackItem.PositionInLayer = 0;
						stackItem.StackItemCode = part.Code;
						stackItem.StackItemType = StackItemType.ProductionItem;
						stackItem.QuantityInLayer = 0;
					}					
				}
				unitOfWork.AddOrUpdate(new[]{stack});
				unitOfWork.Save();
			}
        }
        catch (Exception e)
        {
            executionContext.JobResultSetComplete(JobResult.Failed);
            _Logger.Error(ResourcesKeys.ErrorInUserExit("StackCreationAfterLotGeneration"), null, e);
            throw;
        }
    }


    public override ICollection<UserExitParameter> UserExitInputParameters
    {
        get
        {
            return new List<UserExitParameter>
            {
                // Example for new parameter:
                // new UserExitParameter("MyParameter", typeof(string), true)
            };
        }
    }
}
