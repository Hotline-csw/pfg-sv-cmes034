#r ControllerMES.Infrastructure.Common
#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   CustomSetProductionStepCapacity
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         T.Stürzer
//   Date:           2023-02-20
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   T.Stürzer       2023-02-20    Created
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IServerCustomization))]
[Export("CustomSetProductionStepCapacity", typeof(HomagGroup.FLS.Services.DataImport.Contracts.Contracts.UserExits.IDataImportBeforeSavePackageUserExit))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("04_Set capacity for production step")]
[EnabledScript(true)]
public class CustomSetProductionStepCapacity : UserExitCustomBase, HomagGroup.FLS.Services.DataImport.Contracts.Contracts.UserExits.IDataImportBeforeSavePackageUserExit
{
    [Import]
    protected IHelperMethodsCommon HelperMethodsCommon { get; set; }

    private Logger _Logger;
    
    private string _TaskName = "CustomSetProductionStepCapacity";
    

        /// Executes the task.
    /// </summary>
    /// <param name="executionContext">Execution context of the job.</param>
    /// <param name="targetItems">Array of TargetItems to process.</param>
    public void Execute(IJobExecutionContext executionContext, TargetItem[] targetItems)
    {
		Guard.ThrowOnArgumentNull(executionContext, "executionContext");
		Guard.ThrowOnArgumentNull(targetItems, "targetItems");

		_Logger = LogHelper.GetLogger(executionContext.Area, typeof(CustomSetProductionStepCapacity));
		_Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);
		
        //string calculationTypes =  HelperMethodsCommon.GetParameter(Parameters, "CalculationTypes", string.Empty);
		string calculationTypes = HelperMethodsCommon.GetProgramSettings("CalculationTypes", "6010;40");
        
		if (!string.IsNullOrEmpty(calculationTypes))
		{
			// Gets a collection of instances of ProductionOrder from the current TargetItems-array (where ProcessingState == ProcessingStateMode.InProcessForImport / 5).
			// All TargetItem that are not of type ProductionOrder, are tried with the following types (to get the ProductionOrder-entity from there):
			// - ProductionOrdersResource (only if Type=="RES_PO").
			ICollection<ProductionOrder> productionOrders = targetItems.GetProductionOrdersFromTargetItems(Name, _Logger);

			List<string> calculationTypesList = calculationTypes
				.Split('|')
				.ToList();

			foreach (var calculationType in calculationTypesList) // "1010;10" bzw. "3010;30" "1010;10|3010;30"
			{
				List<string> calculationTypeList = calculationType
					.Split(';')
					.ToList();

				if (calculationTypeList.Count > 1)
				{
					foreach (var productionOrder in productionOrders)
					{
						ProductionStep productionStep = productionOrder.ProductionSteps
							.FirstOrDefault(ps => ps.WorkCenterCode == calculationTypeList[0]);

						if (productionOrder.ProductionItems != null && productionStep != null && productionStep.WorkCenterCode == calculationTypeList[0])
						{
							switch (calculationTypeList[1])
							{
								case "10": // 10=square-millimeters
									{
										SetProductionStepCapacityToSquareMillimeters(productionOrder, productionStep);
										break;
									}
								case "21": // 21=combined length of edgebanding from EdgeProfile-records
									{
										SetProductionStepCapacityToEdgeProfilesAddedLengthInMillimeter(targetItems, productionOrder, productionStep);
										break;
									}
								case "30": // 30=quantity of EdgePasses-records
									{
										SetProductionStepCapacityToEdgePassesQuantity(targetItems, productionOrder, productionStep);
										break;
									}
								case "31": // 31=combined length of edgebanding from EdgePasses-records
									{
										SetProductionStepCapacityToEdgePassesAddedLengthInMillimeter(targetItems, productionOrder, productionStep);
										break;
									}
								case "40": // 40 = Article volume - Capacity for Assembly
									{
										SetProductionStepCapacityVolumeArticle(productionOrder, productionStep);
										break;
									}

							}
						}
					}
				}
			}
		}
    }
    


	/// <summary>
    /// Calculates the square-millimeters of a part (a production-order).
    /// </summary>
    /// <param name="productionOrder">Current ProductionOrder.</param>
    /// <param name="productionStep">Edgebanding-ProductionStep.</param>
    private void SetProductionStepCapacityToSquareMillimeters(ProductionOrder productionOrder, ProductionStep productionStep)
    {
		Guard.ThrowOnArgumentNull(productionOrder, "productionOrder");
		Guard.ThrowOnArgumentNull(productionStep, "productionStep");

		decimal squareMillimeter = productionOrder.Length * productionOrder.Width;

		productionStep.Capacity = ((int) Math.Round(squareMillimeter));

		if (_Logger.IsDebugEnabled)
		{
			_Logger.Debug(string.Format(CultureInfo.InvariantCulture, "{0}: WorkCenterCode=[{1}], Capacity=[{2}] (square dimension in mm=[{3}], length=[{4}], width=[{5}])", Name, productionStep.WorkCenterCode ?? "", productionStep.Capacity, squareMillimeter, productionOrder.Length, productionOrder.Width));
		}
    }



    /// <summary>
    /// Sets base.ProductionSteps.Capacity based of the added length of base.EdgeProfiles of the current production-order.
    /// </summary>
    /// <param name="targetItems">Array of TargetItems to process.</param>
    /// <param name="productionOrder">Current ProductionOrder.</param>
    /// <param name="productionStep">Edgebanding-ProductionStep.</param>
    private void SetProductionStepCapacityToEdgeProfilesAddedLengthInMillimeter(TargetItem[] targetItems, ProductionOrder productionOrder, ProductionStep productionStep)
    {
		Guard.ThrowOnArgumentNull(targetItems, "targetItems");
		Guard.ThrowOnArgumentNull(productionOrder, "productionOrder");
		Guard.ThrowOnArgumentNull(productionStep, "productionStep");

        decimal addedLengthInMillimeter = 0.0M;
        string alternateCodes = "";

		IEnumerable<HomagGroup.FLS.Domain.Data.EdgeProfile> edgeProfiles = GetEdgeProfiles(targetItems, productionOrder);

		foreach (HomagGroup.FLS.Domain.Data.EdgeProfile edgeProfile in edgeProfiles)
		{
			if (edgeProfile.AlternateCode == "S" || edgeProfile.AlternateCode == "N") // Edge south or north
			{
				addedLengthInMillimeter += productionOrder.Length;
			}
			else if (edgeProfile.AlternateCode == "W" || edgeProfile.AlternateCode == "E") // Edge west or east
			{
				addedLengthInMillimeter += productionOrder.Width;
			}

			string separator = "-";
			if (alternateCodes.Length == 0)
			{
				separator = "";
			}

			alternateCodes = string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", alternateCodes, separator, edgeProfile.AlternateCode);
		}


		productionStep.Capacity = (int) Math.Round(addedLengthInMillimeter);

		if (_Logger.IsDebugEnabled)
		{
			_Logger.Debug(string.Format(CultureInfo.InvariantCulture, "{0}: WorkCenterCode=[{1}], Capacity=[{2}] (edges in process=[{3}], added EdgeProfiles-length in mm=[{4}])", Name, productionStep.WorkCenterCode ?? "", productionStep.Capacity, alternateCodes, addedLengthInMillimeter));
		}
    }



    /// <summary>
    /// Sets base.ProductionSteps.Capacity based of the quantity of base.EdgePasses-records of the current production-order.
    /// </summary>
    /// <param name="targetItems">Array of TargetItems to process.</param>
    /// <param name="productionOrder">Current ProductionOrder.</param>
    /// <param name="productionStep">Edgebanding-ProductionStep.</param>
    private void SetProductionStepCapacityToEdgePassesQuantity(TargetItem[] targetItems, ProductionOrder productionOrder, ProductionStep productionStep)
    {
		Guard.ThrowOnArgumentNull(targetItems, "targetItems");
		Guard.ThrowOnArgumentNull(productionOrder, "productionOrder");
		Guard.ThrowOnArgumentNull(productionStep, "productionStep");

		//IEnumerable<EdgePass> edgePasses = GetEdgePasses(targetItems, productionOrder);

		productionStep.Capacity = productionOrder.EdgePasses.Count();

		if (_Logger.IsDebugEnabled)
		{
			_Logger.Debug(string.Format(CultureInfo.InvariantCulture, "{0}: WorkCenterCode=[{1}], Capacity=[{2}] (quantity of EdgePasses=[{3}])", Name, productionStep.WorkCenterCode ?? "", productionStep.Capacity, productionOrder.EdgePasses.Count()));
		}
    }



    /// <summary>
    /// Sets base.ProductionSteps.Capacity based of the added length of base.EdgePasses of the current production-order.
    /// ************************************************************************************
    /// *** For EdgePass.Orientation2== 0/180 the ProductionOrder.Length is being added. ***
    /// *** For EdgePass.Orientation2==90/270 the ProductionOrder.Width is being added.  ***
    /// ************************************************************************************
    /// </summary>
    /// <param name="targetItems">Array of TargetItems to process.</param>
    /// <param name="productionOrder">Current ProductionOrder.</param>
    /// <param name="productionStep">Edgebanding-ProductionStep.</param>
    private void SetProductionStepCapacityToEdgePassesAddedLengthInMillimeter(TargetItem[] targetItems, ProductionOrder productionOrder, ProductionStep productionStep)
    {
		Guard.ThrowOnArgumentNull(targetItems, "targetItems");
		Guard.ThrowOnArgumentNull(productionOrder, "productionOrder");
		Guard.ThrowOnArgumentNull(productionStep, "productionStep");

        decimal addedLengthInMillimeter = 0.0M;
        string edgesInProcess = "";

		//IEnumerable<EdgePass> edgePasses = GetEdgePasses(targetItems, productionOrder);

		foreach (EdgePass edgePass in productionOrder.EdgePasses)
		{
			int oriantation2 = edgePass.Orientation2 ?? 0;
			string edgeInProcess = edgePass.EdgeInProcess ?? "";
			if (edgeInProcess == "S" || edgeInProcess == "N" ||
			    oriantation2 == 0 || oriantation2 == 180) // Edge south or north
			{
				addedLengthInMillimeter += productionOrder.Length;
			}
			else if (edgeInProcess == "W" || edgeInProcess == "E" ||
			         oriantation2 == 90 || oriantation2 == 270 || oriantation2 == -90) // Edge west or east
			{
				addedLengthInMillimeter += productionOrder.Width;
			}

			string separator = "-";
			if (edgesInProcess.Length == 0)
			{
				separator = "";
			}

			edgesInProcess = string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", edgesInProcess, separator, edgePass.EdgeInProcess);
		}

		productionStep.Capacity = (int) Math.Round(addedLengthInMillimeter);

		if (_Logger.IsDebugEnabled)
		{
			_Logger.Debug(string.Format(CultureInfo.InvariantCulture, "{0}: WorkCenterCode=[{1}], Capacity=[{2}] (edges in process=[{3}], added EdgeProfiles-length in mm=[{4}])", Name, productionStep.WorkCenterCode ?? "", productionStep.Capacity, edgesInProcess, addedLengthInMillimeter));
		}
    }



    /// <summary>
    /// Sets base.ProductionSteps.Capacity based on Article Volume - Capacity for Assembly
    /// </summary>
    /// <param name="productionOrder">Current ProductionOrder.</param>
    /// <param name="productionStep">Assembly-ProductionStep.</param>
    private void SetProductionStepCapacityVolumeArticle(ProductionOrder productionOrder, ProductionStep productionStep)
    {
		Guard.ThrowOnArgumentNull(productionOrder, "productionOrder");
		Guard.ThrowOnArgumentNull(productionStep, "productionStep");

		productionStep.Capacity = Convert.ToInt32(productionOrder.CustomVolume, CultureInfo.InvariantCulture);

		if (_Logger.IsDebugEnabled)
		{
			_Logger.Debug(string.Format(CultureInfo.InvariantCulture, "{0}: WorkCenterCode=[{1}], Capacity=[{2}].", Name, productionStep.WorkCenterCode ?? "", productionStep.Capacity));
		}
    }

	/// <summary>
	/// Method to get edge profiles.
	/// </summary>
    /// <param name="targetItems">Array of TargetItems to process.</param>
	/// <param name="productionOrder"></param>
	/// <returns>Collection of EdgeProfiles.</returns>
	private IEnumerable<HomagGroup.FLS.Domain.Data.EdgeProfile> GetEdgeProfiles(TargetItem[] targetItems, ProductionOrder productionOrder)
	{
		Guard.ThrowOnArgumentNull(targetItems, "targetItems");
		Guard.ThrowOnArgumentNull(productionOrder, "productionOrder");

		// First try: Read EdgeProfiles from TargetItems.
		IEnumerable<HomagGroup.FLS.Domain.Data.EdgeProfile> edgeProfiles = targetItems.Select(p => p.Value).OfType<HomagGroup.FLS.Domain.Data.EdgeProfile>()
						.Where(ep => ep.ProductionOrderCode == productionOrder.Code && ep.IsThroughFeed);

		// Second try (if no EdgeProfiles exist as TargetItem) 
		// read them from the current ProductionOrder.
		if (productionOrder != null && !edgeProfiles.Any())
		{
			edgeProfiles = productionOrder.EdgeProfiles.Where(ep => ep.IsThroughFeed).ToArray();
		}


		return edgeProfiles;
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
