//-----------------------------------------------------------------------------
//   (Class-)Name:   BulkPlanningSet
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         A. Plumeyer
//   Date:           2024-03-25
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   <Author>        2024-03-25    Created
//   A. Kais         2024-06-04    Fix Bug in RangeOfNumbersHelper
//   
//-----------------------------------------------------------------------------

using System.ComponentModel;

[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IUserExit))]
[Export("BulkPlanningSet", typeof(HomagGroup.FLS.Infrastructure.Common.Rest.IRestFunction))]
[Description("PULK: Initialisieren von PulkPlanung in CMES Web")]
[EnabledScript(true)]
public class BulkPlanningSet : HomagGroup.FLS.Infrastructure.Common.Customization.UserExitBase, HomagGroup.FLS.Infrastructure.Common.Rest.IRestFunction
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory { get; set; }

    [Import]
    protected RangeOfNumbersHelper _RangeOfNumbersHelper;
    
    [Import]
    private ISettingsHelper SettingsHelper { get; set; }

    public static string[] IndexColors = new string[]
    {
        "#000000", "#010067", "#D5FF00", "#FF0056", "#9E008E", "#0E4CA1", "#FFE502", "#005F39", "#00FF00", "#95003A",
        "#FF937E", "#A42400", "#001544", "#91D0CB", "#620E00", "#6B6882", "#0000FF", "#007DB5", "#6A826C", "#00AE7E",
        "#C28C9F", "#BE9970", "#008F9C", "#5FAD4E", "#FF0000", "#FF00F6", "#FF029D", "#683D3B", "#FF74A3", "#968AE8",
        "#98FF52", "#A75740", "#01FFFE", "#FFEEE8", "#FE8900", "#BDC6FF", "#01D0FF", "#BB8800", "#7544B1", "#A5FFD2",
        "#FFA6FE", "#774D00", "#7A4782", "#263400", "#004754", "#43002C", "#B500FF", "#FFB167", "#FFDB66", "#90FB92",
        "#7E2DD2", "#BDD393", "#E56FFE", "#DEFF74", "#00FF78", "#009BFF", "#006401", "#0076FF", "#85A900", "#00B917",
        "#788231", "#00FFC6", "#FF6E41", "#E85EBE"
    };

    public System.Net.Http.HttpResponseMessage Execute(System.Collections.Generic.IDictionary<string, object> parameters, object content, string contentType)
    {

        var _Logger = LogHelper.GetLogger("RestFunction", typeof(BulkPlanningSet));
        _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, "BulkPlanningSet");
       
        var result = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        try
        {
            var settings = SettingsHelper.GetOrCreateComponentSetting("GenerateBulks", "Color", "Index", null);
            var lastIndex = settings.GetValue<int>("Value", 0);
            lastIndex = lastIndex >= IndexColors.Length? 0 : lastIndex;            
            _Logger.Info("vor deserialize: " + content.ToString());
            var data = JsonConvert.DeserializeObject<Data>(content.ToString());
            
            //data.name = (_RangeOfNumbers as RangeOfNumbersHelper).GetNewUniqueIdentifier(_Logger, "BulkNumbers");
            data.name = _RangeOfNumbersHelper.GetNewUniqueIdentifier(_Logger,"BulkNumbers");
            data.color =  IndexColors[lastIndex++];
            data.date = DateTime.Today.AddWorkdays(5, new DateTime[0]).ToString("yyyy-MM-dd");
            settings.SetValue("Value", lastIndex);
            result.Content = new StringContent(JsonConvert.SerializeObject(data));
        }
        catch(Exception Ex)
        {
             result.StatusCode = System.Net.HttpStatusCode.BadRequest;
             result.Content = new StringContent(Ex.Message);
        }
        return result;
    }
    
    public static string GetDayName(DateTime date)
    {
        string _ret = string.Empty;
        var culture = new System.Globalization.CultureInfo("de-DE"); 
        _ret = culture.DateTimeFormat.GetDayName(date.DayOfWeek); 
        _ret = culture.TextInfo.ToTitleCase(_ret.ToLower()); 
        return _ret;
    }

    private class Data
    {   
        public string name { get; set; }
        public string date {get; set; }
        public string color { get; set; } 
    }
}

