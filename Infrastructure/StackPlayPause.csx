#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   StackPlayPause
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         E.Hain
//   Date:           2023-02-27/2023-04-20
//
//-----------------------------------------------------------------------------
//  Revision History:
//  Name            Date            Description
//  S.Feist         2025-05-26      Created
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;
using System.Collections;


// TODO MESSAGING: activate, if messaging-functionality is needed (eg. navigate to a tile-view and execute an ad-hoc filter)
// TODO REFRESH: activate, if refreshing a tile-view is needed



[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IClientCustomization))]
[Export("StackPlayPause", typeof(HomagGroup.FLS.Infrastructure.Framework.Contracts.ICommandUserExit))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("Offenen Stapel pausieren bzw. pausierten Stapel wieder aufnehmen")]
[EnabledScript(true)]
public class StackPlayPause : UserExitCustomBase, HomagGroup.FLS.Infrastructure.Framework.Contracts.ICommandUserExit//, IPartImportsSatisfiedNotification // TODO MESSAGING
{

    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;
    
    [Import]
    protected UserExitHelper UserExitHelper { get; set; }
    
	[Import]
	protected IHelperMethodsCommon _HelperMethodsCommon;

    private Logger _Logger;

    public void Execute(object parameter)
    {
        string taskName = this.Name;
        Guard.ThrowOnArgumentNull(parameter, "parameter");

        try
        {
            _Logger = LogHelper.GetLogger(Name, typeof(StackPlayPause));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, "");

            var itemEnumerable = parameter as IEnumerable;

            if (itemEnumerable != null)
            {
                using (var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
                {                   
                    //loop through all selected rows of base.Stacks
                    foreach(var selectedItem in itemEnumerable.OfType<HomagGroup.FLS.Domain.Data.Stack>())
                    {
                        var currentStackId = selectedItem.StackCode;
                        var resultMessageBox = System.Windows.MessageBoxResult.No;
                        
                        //if the selcted stack is open and not reserved 
                        if (selectedItem.IsValid        == YesNo.No && 
                            selectedItem.IsReserved     == YesNo.No &&
                            selectedItem.CustomIsPaused == YesNo.Yes)
                        {
                            //Messagebox to confirm reopening the stack
    					    System.Windows.Application.Current.Dispatcher.Invoke(() =>
    					    {
    					       resultMessageBox = HomagGroup.Base.UI.Windows.MessageBox.Show(string.Format(
    					       "Soll der Stapel {0} erneut aufgenommen werden?", currentStackId),
    					       "ControllerMES",MessageBoxButton.YesNo);
    					    });
    					    
    					    //if msgBox returns yes
                            if(resultMessageBox == System.Windows.MessageBoxResult.Yes)
                            {
                                _Logger.Debug(string.Format("Messagebox wurde für Stapel '{0}'mit 'Ja' bestätigt", currentStackId));
                                
                                // update stack status 
                                selectedItem.CustomIsPaused = YesNo.No;
                            }
                        }
                        
                        else if (selectedItem.IsValid        == YesNo.No && 
                                 selectedItem.IsReserved     == YesNo.No &&
                                 selectedItem.CustomIsPaused == YesNo.No)
                        {
                            //Messagebox to confirm pausing the stack
    					    System.Windows.Application.Current.Dispatcher.Invoke(() =>
    					    {
    					       resultMessageBox = HomagGroup.Base.UI.Windows.MessageBox.Show(string.Format(
    					       "Soll der Stapel {0} pausiert werden?", currentStackId),
    					       "ControllerMES",MessageBoxButton.YesNo);
    					    });
    					
    					    //if msgBox returns yes
                            if(resultMessageBox == System.Windows.MessageBoxResult.Yes)
                            {
                                _Logger.Debug(string.Format("Messagebox wurde für Stapel '{0}'mit 'Ja' bestätigt", currentStackId));
                            
                                // -01- update stack status 
                                selectedItem.CustomIsPaused = YesNo.Yes;
                                
                                // -02- calling methode to write print job record
                                WritePrintJobRecord(unitOfWork, taskName, currentStackId);
                                
                                // -03-	return info message to confirm everything is done
                                TileViewHelperStatic.SetInfoBalloon(string.Format(
                                "Stapel {0} wurde erfolgreich pausiert.",currentStackId), 
                                HomagGroup.Base.UI.DeviceState.Ok,30,3000);
                            } 
                        }
                        
                        //save changes in table
                        unitOfWork.AddOrUpdate(new[] { selectedItem });
                        unitOfWork.Save();
                        _Logger.Debug("Eintrag in base.Stacks wurde geschrieben");
                    }
                }
			// Refresh the active View
            UserExitHelper.RefreshView();
            }
        }
        catch (Exception e)
        {
            _Logger.Error(ResourcesKeys.ErrorInUserExit(Name), null, e);
            throw;
        }
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
