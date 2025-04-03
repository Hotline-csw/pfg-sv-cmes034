#r ControllerMES.Infrastructure.Common
#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   CsvImportCheckRow
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         <Author>
//   Date:           2022-11-29
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   <Author>        2022-11-29    Created
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IServerCustomization))]
[Export("CsvImportCheckRow", typeof(HomagGroup.FLS.Services.DataImport.Contracts.Contracts.UserExits.IDataImportTransformationUserExit))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("CheckCsvFile")]
[EnabledScript(true)]
public class CsvImportCheckRow : UserExitCustomBase, HomagGroup.FLS.Services.DataImport.Contracts.Contracts.UserExits.IDataImportTransformationUserExit
{
    private Logger _Logger;

    public void Execute(IJobExecutionContext executionContext, SourceItem source, TargetItem target)
    {
        Guard.ThrowOnArgumentNull(executionContext, "executionContext");
        Guard.ThrowOnArgumentNull(source, "source");
        Guard.ThrowOnArgumentNull(target, "target");

        try
        {
            _Logger = LogHelper.GetLogger(executionContext.Area, typeof(CsvImportCheckRow));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);

            //throw new NotImplementedException();

            // example to read the value of a parameter, defined below in UserExitInputParameters:
            // string myParameter = Parameters.FirstOrDefault(input => input.Key == "MyParameter").Value.ToString();
            
            _Logger.Info("Check CSV file");
            
            string sCustomerOrderCode= source["CustomerOrderCode"].ToString();
            string sArticleNumber= source["ArticleNumber"].ToString();            
            _Logger.Info("CustomerOrderCode: '" + sCustomerOrderCode + "'" + " ### " + "ArticleNumber: '" + sArticleNumber + "'");
            
        }
        catch (Exception e)
        {
            executionContext.JobResultSetComplete(JobResult.Failed);
            _Logger.Error(ResourcesKeys.ErrorInUserExit(Name), null, e);
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
