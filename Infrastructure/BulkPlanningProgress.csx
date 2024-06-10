//-----------------------------------------------------------------------------
//   (Class-)Name:   BulkPlanningProgress
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         <Author>
//   Date:           2024-05-15
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   <Author>        2024-05-15    Created
//   
//-----------------------------------------------------------------------------

using System.ComponentModel;



[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IUserExit))]
[Export("BulkPlanningProgress", typeof(HomagGroup.FLS.Infrastructure.Common.Rest.IRestFunction))]
[Description("PULK: Kalkulation der Werte für den Graphen")]
[EnabledScript(true)]
public class BulkPlanningProgress : HomagGroup.FLS.Infrastructure.Common.Customization.UserExitBase, HomagGroup.FLS.Infrastructure.Common.Rest.IRestFunction
{
    [Import]
    ICommonServiceDistributed _BulkInfoProvider; //CommonService implements the BulkInfoProvider 

    public System.Net.Http.HttpResponseMessage Execute(System.Collections.Generic.IDictionary<string, object> parameters, object content, string contentType)
    {

        var _Logger = LogHelper.GetLogger("RestFunction", typeof(BulkPlanning));
        _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, "BulkPlanningProgress");
       
        var result = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
         
        var data = JsonConvert.DeserializeObject<Data>(content.ToString());
        _Logger.Info(string.Concat(data.entities));
        _Logger.Info("Name: " + data.name);
        _Logger.Info("Farbe: " + data.color);
        _Logger.Info("Datum: " + data.date);
        long[] ids = Array.ConvertAll(data.entities, long.Parse);

        var res = _BulkInfoProvider.SimulateBulkPlanningCapacity(ids, data.date, data.date);
        result.Content = new StringContent(res);
        return result;
    }
   
    private class Data
    {   
        public string[] entities { get; set; }
        public string name { get; set; }
        public DateTime date {get; set; }
        public string color { get; set; } 
    }
}
