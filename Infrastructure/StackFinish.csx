#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   StackFinish
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:        E.Hain/T.Brehm
//   Date:          2023-04-05
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   S.Feist         2025-05-26    Created
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;
using System.Collections;
using System.Data.SqlClient;


// TODO MESSAGING: activate, if messaging-functionality is needed (eg. navigate to a tile-view and execute an ad-hoc filter)
// TODO REFRESH: activate, if refreshing a tile-view is needed



[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IClientCustomization))]
[Export("StackFinish", typeof(HomagGroup.FLS.Infrastructure.Framework.Contracts.ICommandUserExit))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("Abschließen eines Stapels (Staus ändern, PrintJob schreiben, Stapelinhalt an KAM)")]
[EnabledScript(false)]
public class StackFinish : UserExitCustomBase, HomagGroup.FLS.Infrastructure.Framework.Contracts.ICommandUserExit//, IPartImportsSatisfiedNotification // TODO MESSAGING
{
    [Import]
    protected RangeOfNumbersHelper _RangeOfNumbersHelper;

    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;
    
    [Import]
    protected UserExitHelper UserExitHelper { get; set; }
    
	[Import]
	protected IHelperMethodsCommon _HelperMethodsCommon;
	
	[Import]
	private LooseXaml _LooseXaml;
	
	[Import("InputBoxViewModel")]
	HomagGroup.FLS.Infrastructure.Framework.Contracts.IDialogViewModel dialogViewModel;
	
	[Import("ItemInStack")]
	HomagGroup.FLS.Infrastructure.Common.Customization.IClientCustomization ItemInStack;
	

    private Logger _Logger;


    public void Execute(object parameter)
    {
        string taskName = this.Name;
        Guard.ThrowOnArgumentNull(parameter, "parameter");

        try
        {
            _Logger = LogHelper.GetLogger(Name, typeof(StackFinish));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, "");

            var itemEnumerable = parameter as IEnumerable;

            if (itemEnumerable != null)
            {
                using (var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
                {
                    IRepository<ProductionItem> productionItemRep = unitOfWork.GetRepository<ProductionItem>();                
                    //loop through all selected rows of base.Stacks
                    foreach(var selectedItem in itemEnumerable.OfType<HomagGroup.FLS.Domain.Data.Stack>())
                    {
                        var currentStackId = selectedItem.StackCode;
                        var resultMessageBox = System.Windows.MessageBoxResult.No;
                        
                        //if the selcted stack is open and not reserved 
                        if (selectedItem.IsValid    == YesNo.No && 
                            selectedItem.IsReserved == YesNo.No)
                        {
                            //Messagebox to confirm closing the stack
    					    System.Windows.Application.Current.Dispatcher.Invoke(() =>
    					    {
    					       resultMessageBox = HomagGroup.Base.UI.Windows.MessageBox.Show(string.Format("Soll der Stapel {0} abgeschlossen werden?", currentStackId),"ControllerMES",MessageBoxButton.YesNo);
    					    });
    					
    					    //if msgBox returns yes
                            if(resultMessageBox == System.Windows.MessageBoxResult.Yes)
                            {
                                _Logger.Debug(string.Format("Messagebox wurde für Stapel '{0}'mit 'Ja' bestätigt", currentStackId));
                                
                                var getFirstStackItem = selectedItem.StackItems.FirstOrDefault();
                                _Logger.Debug(string.Format("getFirstStackItem equals: '{0}'", getFirstStackItem));
                                
                                if (getFirstStackItem != null)
                                {
                                    var itemExists = productionItemRep.GetFirstOrDefault(x => x.Code == getFirstStackItem.StackItemCode);

                                    if (itemExists != null)
                                    {
                                    
                                        CustomProductionType productionType = itemExists.ProductionOrder.CustomProductionType ?? CustomProductionType.Losgroesse1;
                                        
                                        //Serienstapel
                                        if (productionType == CustomProductionType.Serienfertigung)
                                        {
                                            (dialogViewModel as InputBoxViewModel).TestData = String.Empty;
                                            var result = _LooseXaml.ShowDialog("InputBoxView",dialogViewModel);
                                            
                                            if (result.HasValue && result == true)
                                            {
                                                //Wert ausgeben der vom Bediener eingetragen wurde
                                                string insertResult = (dialogViewModel as InputBoxViewModel).TestData;
                                                
                                                int nrOfParts;
                                                //Prüfen ob Wert ein integer ist
                                                if (int.TryParse(insertResult, out nrOfParts))
                                                {
                                                    //Nur Anzahl Bauteile übernehmen, die der Benutzer eingetragen hat. Das gescannte Bauteile darf nicht übernommen werden.
                                                    var selectProductionItems = productionItemRep.GetQueryable(false)
                                                                                                    .Where(x => x.ProductionOrderCode == itemExists.ProductionOrderCode 
                                                                                                            && x.Code != itemExists.Code 
                                                                                                            && !(x.ProductionItemsHistory.Any(y => y.ProductionItemCode == x.Code && y.WorkCenterCode == "JOOST")))
                                                                                                    .Take(nrOfParts);
                                                   
                                                    foreach (var selectProductionItem in selectProductionItems)
                                                    {
                                                        _Logger.Info(selectProductionItem.Code);
                                                        (ItemInStack as ItemInStack).InsertStackItem(unitOfWork,_Logger,selectProductionItem,selectedItem,"StackFinish");
                                                    }
                                                    
                                                    if(!selectProductionItems.Any())
                                                    {
                                                        _Logger.Error("Keine Bauteil gefunden");
                                                    }
                                                }
                                                
                                                else
                                                {
                                                    TileViewHelperStatic.SetInfoBalloon(string.Format(
                                                    "Es wurde eine ungültiger Wert eingetragen{0}",insertResult), 
                                                    HomagGroup.Base.UI.DeviceState.Error,30,5000);
                                                }
                                            
                                            }
                                        
                                        }
                                        
                                        //Call methods for both series and lot 1
                                        // -01- calling method to update stack status 
                                        SetStackStatusToClosed(unitOfWork, selectedItem, currentStackId);
                                        
                                        // -02- calling methode to write print job record
                                        WritePrintJobRecord(unitOfWork, taskName, currentStackId);
                                        
                                        // -03-	return info message to confirm everything is done
                                        TileViewHelperStatic.SetInfoBalloon(string.Format(
                                        "Stapel {0} wurde erfolgreich abgeschlossen, an die Kante übertragen und der Etikettendruck angestoßen.",currentStackId), 
                                        HomagGroup.Base.UI.DeviceState.Ok,30,5000);
                                    
                                    }
                                    
                                }
                                //If stack has no part(s) assignt
                                else if(getFirstStackItem == null)

                                _Logger.Debug(string.Format("Stack '{0}' contains no parts", currentStackId));
                                
    					        System.Windows.Application.Current.Dispatcher.Invoke(() =>
    					        {
    					            resultMessageBox = HomagGroup.Base.UI.Windows.MessageBox.Show(string.Format(
    					            "Dem Stapel '{0}' ist kein Bauteil zugewiesen und kann daher nicht abgeschlossen werden. \nBitte Bauteil zuweisen oder Stapel vorerst pausieren!", currentStackId),"ControllerMES",MessageBoxButton.OK);
    					        });
                                
                            }                                
                        }
                    }
            	//Refresh the active View
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


    //Update stack status
    private void SetStackStatusToClosed (IUnitOfWork unitOfWork, HomagGroup.FLS.Domain.Data.Stack selectedItem, string currentStackId)
        {
            //write data set
            selectedItem.CustomTransferState = 10;  // 00 = Created; 
                                                    // 10 = ReadyForExport; 
                                                    // 15 = InExportProcess; 
                                                    // 20 = Exported
            selectedItem.IsActive       = YesNo.Yes;
            selectedItem.IsReserved     = YesNo.No;
            selectedItem.IsValid        = YesNo.Yes;
            selectedItem.CustomIsPaused = YesNo.No;
            
            //save changes
            unitOfWork.AddOrUpdate(new[] { selectedItem });
            unitOfWork.Save();
            _Logger.Debug(string.Format("Status für Stapel {0} wurde auf Abgeschlossen gesetzt.", currentStackId));
        }
            

    //Write print job record
    private void WritePrintJobRecord(IUnitOfWork unitOfWork, string taskName, string currentStackId)
        {
            _Logger.Debug("Schreibe Eintrag in StackPrintJobItems");
            var stacks = unitOfWork.GetRepository<HomagGroup.FLS.Domain.Data.Stack>().GetFirstOrDefault(x => x.IsValid == YesNo.No && x.CustomIsPaused == 0);
            var printJobRecord = new HomagGroup.FLS.Domain.Data.CustStackPrintJobItem();
            
            //write data set
            printJobRecord.StackCode =          currentStackId;
            printJobRecord.ProcessingState =    0;  // 0=Created; 
                                                    // 10=ReadyForPrinting; 
                                                    // 15=InPrintingProcess; 
                                                    // 20=Printed
            printJobRecord.JobName =            "PrintStackLabel"; // Name des PrintJobs, der aufgerufen werden soll
            printJobRecord.CreationDate =       DateTime.Now;
            printJobRecord.CreationSource =     taskName;
            printJobRecord.ModificationDate =   DateTime.Now;
            printJobRecord.ModificationSource = taskName; //wird von cMES mit Benutzernamen überschrieben
            printJobRecord.Locked =             false;
            printJobRecord.LockSource =         "default";
            
            //save changes in table
            unitOfWork.AddOrUpdate(new[] { printJobRecord });
            unitOfWork.Save();
            _Logger.Debug("Eintrag in StackPrintJobItems wurde geschrieben");
        }


    // Validation of executability
    public bool CanExecute(object parameter)
    {
        IEnumerable itemEnumerable = parameter as IEnumerable;

        if (itemEnumerable != null)
        {
            return true;
        }

        return false;
    }



    // TODO MESSAGING
    ///// <summary>
    ///// Called when a part's imports have been satisfied and it is safe to use.
    ///// </summary>
    //public void OnImportsSatisfied()
    //{
    //    _MessagingService = DistributedServiceProvider.GetService<IMessagingService>();
    //}
}

