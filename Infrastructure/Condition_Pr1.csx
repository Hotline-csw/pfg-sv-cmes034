
    [Export("Condition_Pr1", typeof(ControllerMES.Infrastructure.BaseCommon.Contracts.IAutomaticOptimizationRuleConditionUserExit))]
    [Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IUserExit))]
    [Description("Condition UE for ALG")]
    [EnabledScript(true)]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class Condition_Pr1 : HomagGroup.FLS.Infrastructure.Common.Customization.UserExitBase, ControllerMES.Infrastructure.BaseCommon.Contracts.IAutomaticOptimizationRuleConditionUserExit
    {
      private Logger logger{get;set;}
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
            logger = LogHelper.GetLogger("LotGeneration", typeof(Condition_Pr1));
            logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);
            var fileName = @"C:\MesDataTransfer\DEV\LotGenerationRules\Condition_";
            if ( !string.IsNullOrEmpty(optimizationRulesAllocation.OptimizationRulesCode))
            {
                if (optimizationRulesAllocation.OptimizationRule.IsPriorityRule == YesNo.Yes)
                {
                    fileName = fileName + optimizationRulesAllocation.OptimizationRulesCode + ".txt";
                }
                else
                {
                    fileName = fileName + optimizationRulesAllocation.OptimizationRulesAreaCode + ".txt";
                }
            }

            bool returnValue = false;
            if (!File.Exists(fileName))
            {
                // Create the file.
                using (FileStream fs = File.Create(fileName))
                {
                    Byte[] info =
                        new UTF8Encoding(true).GetBytes("false");

                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                }
            }

            using (StreamReader sr = File.OpenText(fileName))
            {
                string s = sr.ReadLine().ToUpper(CultureInfo.InvariantCulture);
                {
                    if (s == "TRUE")
                        returnValue = true;
                }
            }

            using (StreamWriter writetext = new StreamWriter(fileName))
            {
                writetext.WriteLine("True");
            }

            return returnValue;
        }

    }

