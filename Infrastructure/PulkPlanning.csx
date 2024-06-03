//-----------------------------------------------------------------------------
//   (Class-)Name:   CustomBulkPlanningExecuteCust
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         A. Plumeyer
//   Date:           2024-01-22
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   A. Plumeyer     2024-01-22    Created
//   
//-----------------------------------------------------------------------------

using System.ComponentModel;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IUserExit))]
[Export("PulkPlanning", typeof(HomagGroup.FLS.Infrastructure.Common.Rest.ILocalizedRestFunction))]
[Description("Pulk-Verplanung (Anlegen/Zuweisen/Umplanen)")]
[EnabledScript(true)]
public class PulkPlanning : HomagGroup.FLS.Infrastructure.Common.Customization.UserExitBase, HomagGroup.FLS.Infrastructure.Common.Rest.ILocalizedRestFunction
{
    [Import]
    ICommonServiceDistributed _BulkInfoProvider; //CommonService implements the BulkInfoProvider 

    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory { get; set; }
    
    public System.Net.Http.HttpResponseMessage Execute(System.Collections.Generic.IDictionary<string, object> parameters, object content, string contentType, string language)
    {

        var _Logger = LogHelper.GetLogger("RestFunction", typeof(PulkPlanning));
        _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, "PulkPlanning");
       
        var result = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        try
        {           
            var data = JsonConvert.DeserializeObject<Data>(content.ToString());
            _Logger.Info(string.Concat(data.entities));
            _Logger.Info("Name: " + data.name);
            _Logger.Info("Farbe: " + data.color);
            _Logger.Info("Datum: " + data.date);
            long[] ids = Array.ConvertAll(data.entities, long.Parse);

            using(var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
            {
                var bulkRepo = unitOfWork.GetRepository<ManualBulk>();
                var bulk = new ManualBulk();
                bulk.PlanningNumber = data.name;
                bulk.Color = data.color;
                bulk.StartDate = DateTime.Today;
                bulk.EndDate = data.date;
                bulk.PlanningState = PlanningState.Planned;
                bulk.SchedulingMode = SchedulingMode.Backward;
                bulkRepo.AddOrUpdate(bulk);
                unitOfWork.Save();
                _Logger.Info("Id: " + bulk.Sequence);
                
                unitOfWork.GetRepository<HomagGroup.FLS.Domain.Data.ProductionOrder>().GetQueryable(false).Where(x => ids.Contains(x.Sequence)).UpdateFromQuery(x => new HomagGroup.FLS.Domain.Data.ProductionOrder {PlanningSequence = bulk.Sequence});
                
                _BulkInfoProvider.PlanProductionDaysForBulkWithoutDelete(bulk.PlanningNumber, bulk.EndDate, bulk.EndDate);

                result.Content = new StringContent("{\"msg\": \"Aufträge erfolgreich verplant.\"}");
            }
            
            
        }
        catch(Exception Ex)
        {
             result.StatusCode = System.Net.HttpStatusCode.BadRequest;
             result.Content = new StringContent("{\"msg\": \"Fehler bei der Verplanung.\"}");
        }
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
