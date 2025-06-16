#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   ReportBindingRegenerateReports
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         <Author>
//   Date:           2022-05-12
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   <Author>        2022-05-12    Created
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;
using System.Collections;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IClientCustomization))]
[Export("ReportBindingRegenerateReports", typeof(HomagGroup.FLS.Infrastructure.Framework.Contracts.ICommandUserExit))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("Druckzentrale - Report(s) neu generieren")]
[EnabledScript(true)]
public class ReportBindingRegenerateReports : UserExitCustomBase, HomagGroup.FLS.Infrastructure.Framework.Contracts.ICommandUserExit
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;
    
    [Import]
	protected UserExitHelper  UserExitHelper {get;set;}	
        
    [Import("BinaryFromReportMethods")]
    HomagGroup.FLS.Infrastructure.Common.Customization.IGlobalCustomization binaryFromReportMethods;
/*    
    [Import("AssemblyManualMethods")]
    HomagGroup.FLS.Infrastructure.Common.Customization.IGlobalCustomization assemblyManualMethods;
*/    
    [Import("InfoBallonMessage")]
    HomagGroup.FLS.Infrastructure.Common.Customization.IClientCustomization infoBallonMessage;
    
    private const bool _ShowInfoBallon = true;

    private Logger _Logger;

    public void Execute(object parameter)
    {
        Guard.ThrowOnArgumentNull(parameter, "parameter");

        try
        {
            _Logger = LogHelper.GetLogger(Name, typeof(ReportBindingRegenerateReports));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, "");

            var itemEnumerable = parameter as IEnumerable;

            if (itemEnumerable != null)
            {
                bool showRegenerateBallon = false;

                var viewReportBindings = itemEnumerable.OfType<CustViewReportBinding>();
                
                if (viewReportBindings != null)
                {                
                    _Logger.Info($"ReportBindingRegenerateReports - neu zu generierende Reports: {viewReportBindings.Count().ToString()}");
                    
                    using (var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
                    {
                    
                        foreach (var viewReportBinding in viewReportBindings)
                        {    
                            var reportBinding = unitOfWork.GetRepository<CustReportBinding>()
                                                        .GetFirstOrDefault(x => x.Sequence == viewReportBinding.Sequence);
                                                        
                            if (reportBinding != null)
                            {
                                if (reportBinding.ReportLayout == "PatternDrawing") 
                                {
                                    // PatternDrawing kann nicht neu erzeugt werden - da aus CutRite
                                    if (_ShowInfoBallon)
                                    {
                        				(infoBallonMessage as InfoBallonMessage).UserFeedback($"Neugenerierung Reports"
                        						, $"Neugenerierung von Report \n{viewReportBinding.ReportDescription}\nist nicht möglich!"
                        						, 0
                        						, HomagGroup.Base.UI.DeviceState.Alarm);
                        	        }
                                }
                                else
                                {
                                    reportBinding.BinariesSequence = 0;
                                    
                                    /*
                                    if (reportBinding.MergePDFSequence > 0)
                                    {
                                        reportBinding.MergePDFSequence = 0;
                                    }
                                    */
                                    
                                    /*
                                    if (reportBinding.ReportLayout == "AssemblyManual") // Montageanleitung
                                    {
                                        (assemblyManualMethods as AssemblyManualMethods).CreateAssemblyManual(_Logger, unitOfWork, reportBinding);
                                    }
                                    */
                                    
                                    // else // alle anderen Reports
                                    //{
                                    
                                        if (reportBinding.ReportLayout == "FittingLabelBE" &&
                                            (String.IsNullOrEmpty(reportBinding.ProductionOrderCode) || 
                                            String.IsNullOrEmpty(reportBinding.ProductionOrderCode)))
                                        {
                                            // Wenn ProductionORder / ProductionItem nicht gefüllt bei FittingLabelBE über Referenz-Entity FA suchen
                                            // kann bei alten Daten vorkommen, da Felder ProductionOrder / ProductionItem in ReportBinding erst in 09/2022 implementiert wurden
                                            // anhand der ReportSequence den FA ermitteln
                                            var productionOrder = unitOfWork.ProductionOrderRepository
                                                                    .GetFirstOrDefault(x => x.Sequence == reportBinding.ReportSequence);
                                                                    
                                            if (productionOrder != null)
                                            {
                                                (binaryFromReportMethods as BinaryFromReportMethods)
                                                .CreateReportBindingBinary( 
                                                                            reportBinding.ReportLayout, 
                                                                            reportBinding.ReportSequence, 
                                                                            //reportBinding.PlanningGroup,
                                                                            //reportBinding.WorkOrder,
                                                                            //reportBinding.VirtualCartCode,
                                                                            reportBinding.OptimizationCode,
                                                                            productionOrder.Code,
                                                                            productionOrder.Code,
                                                                            //reportBinding.DateReportRelevance,
                                                                            //reportBinding.TrainStation,
                                                                            //reportBinding.TrainStationDescription,
                                                                            reportBinding.CustomerOrderCode,
                                                                            reportBinding.CustomerOrderPosition
                                                                            //reportBinding.CapacitySequenceCode,
										                                    //reportBinding.CapacitySequenceDescription,
                                                                            //reportBinding.TaggedLinesGroupName,
                                                                            //reportBinding.ReportField01,
                                                                            //reportBinding.ReportField02
                                                                            );
                                            }
                                            else
                                            {
                                                _Logger.Warn($"ReportBindingRegenerateReports - FA zu ReportSequence {reportBinding.ReportSequence.ToString()} ist nicht vorhanden");
                                            }
                                        }
                                        else
                                        {
                                            // Standartweg
                                            (binaryFromReportMethods as BinaryFromReportMethods)
                                                .CreateReportBindingBinary( 
                                                                            reportBinding.ReportLayout, 
                                                                            reportBinding.ReportSequence, 
                                                                            //reportBinding.PlanningGroup,
                                                                            //reportBinding.WorkOrder,
                                                                            //reportBinding.VirtualCartCode,
                                                                            reportBinding.OptimizationCode,
                                                                            reportBinding.ProductionOrderCode,
                                                                            reportBinding.ProductionItemCode,
                                                                            //reportBinding.DateReportRelevance,
                                                                            //reportBinding.TrainStation,
                                                                            //reportBinding.TrainStationDescription,
                                                                            reportBinding.CustomerOrderCode,
                                                                            reportBinding.CustomerOrderPosition
                                                                            //reportBinding.CapacitySequenceCode,
										                                    //reportBinding.CapacitySequenceDescription,
                                                                            //reportBinding.TaggedLinesGroupName,
                                                                            //reportBinding.ReportField01,
                                                                            //reportBinding.ReportField02
                                                                            );
                                        }
                                    //}
                                    
                                    showRegenerateBallon = true; // Infoballon für Neugenerierung aktivieren
                                }
                            }
                            
                        }
                        
                        if (_ShowInfoBallon && showRegenerateBallon)
                        {
                            string regenerateWording = viewReportBindings.Count() == 1 ? "Report" : "Reports";
            				(infoBallonMessage as InfoBallonMessage).UserFeedback($"Neugenerierung Reports"
            						, $"Neugenerierung für {viewReportBindings.Count().ToString()} {regenerateWording} beauftragt."
            						, 6000
            						, HomagGroup.Base.UI.DeviceState.Ok);
            	        }                        
                
                    }
                    
                    UserExitHelper.RefreshView();
                }

            }
        }
        catch (Exception e)
        {
            _Logger.Error(ResourcesKeys.ErrorInUserExit(Name), null, e);
            throw;
        }
    }

    public bool CanExecute(object parameter)
    {
        IEnumerable itemEnumerable = parameter as IEnumerable;

        if (itemEnumerable != null)
        {
            return true;
        }

        return false;
    }
}
