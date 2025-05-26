#r ControllerMES.Infrastructure.Resource
#r ToastNotifications
#r WindowsBase
#r PresentationFramework

//-----------------------------------------------------------------------------
//   (Class-)Name:   StackStart
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         E.Hain
//   Date:           2023-03-06
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   S.Feist         2025-05-26    Created
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;
using System.Collections;

// TODO MESSAGING: activate, if messaging-functionality is needed (eg. navigate to a tile-view and execute an ad-hoc filter)
// TODO REFRESH: activate, if refreshing a tile-view is needed



[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IClientCustomization))]
[Export("StackStart", typeof(HomagGroup.FLS.Infrastructure.Framework.Contracts.ICommandUserExit))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("Stapel-ID generieren und Stapel 'eröffnen'")]
[EnabledScript(true)]
public class StackStart : UserExitCustomBase, HomagGroup.FLS.Infrastructure.Framework.Contracts.ICommandUserExit//, IPartImportsSatisfiedNotification // TODO MESSAGING
{
    //Deklarationen
    private const string _Taskname = "StackStart";
    [Import]
    protected UserExitHelper UserExitHelper { get; set; }
    [Import]
    protected RangeOfNumbersHelper _RangeOfNumbersHelper;
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;
    private Logger _Logger;
    /// <summary>
    /// ToastNotification-instance.
    /// </summary>
    protected ToastNotification ToastNotificationInfo { get; private set; }
    /// <summary>
    /// bool - true, if ToastNotification for info-messages has been shown.
    /// </summary>
    protected bool ToastNotificationInfoActive = false;



    // UE-Anweisung, verwendete Funktionen sind darunter aufgeführt
    public void Execute(object parameter)
    {
        Guard.ThrowOnArgumentNull(parameter, "parameter");

        try
        {
            _Logger = LogHelper.GetLogger(Name, typeof(StackStart));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, "");

            var itemEnumerable = parameter as IEnumerable;
            ResourceIdentifier infoMsg01 = null;
            
            if (itemEnumerable != null)
            {
                
               using (var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
               {
                //Abfrage der neuen Stapel-ID inkl. Loggermeldung    
                string resultMessage = GetStackId();
                string resMes = resultMessage.ToString();
                _Logger.Info(resMes);
                
                //Datensatz in base.Stacks schreiben                
                WriteStackRecord(unitOfWork,resMes);

                // TODO REFRESH
				// Refresh the active View
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



    //Prüfung der Ausführbarkeit
    public bool CanExecute(object parameter)
    {
       using (var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
       {
            //Wenn es bereits einen offenen Stapel gibt, Ausführung verhindern (ausgegraut)
            var stacks = unitOfWork.GetRepository<HomagGroup.FLS.Domain.Data.Stack>().GetFirstOrDefault(x => x.IsValid == YesNo.No && x.CustomIsPaused == 0);

            if (stacks != null)
            {
                return false;
            }
            else 
            {
                return true;
            }
       }
        
    }
    
    
    
    // Holt einen neuen Wert aus der Tabelle base.RangeOfNumbers mit der Bedingung Area=StackId
    public string GetStackId ()
    {
        //Using of standard function to get the current value from RangeOfNumbers for entry StackId
        int newStackIdNumber = 999; 
        newStackIdNumber = int.Parse(_RangeOfNumbersHelper.GetNewUniqueIdentifier(_Logger, "StackId"));
        string newStackId;
        newStackId = string.Format("MS-{0}",newStackIdNumber);
        
        
        using (var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWorkBase())
        {
            //Reset current value to minimum value, once maximum value is reached. 
            IRepository<RangeOfNumbers> rangeOfNumbersRepository = unitOfWork.GetRepository<RangeOfNumbers>();
            var rangeOfNumbers = rangeOfNumbersRepository.GetFirstOrDefault(x => x.Area == "StackId");
        
            if (newStackIdNumber == rangeOfNumbers.MaxValue-1)
            {
                rangeOfNumbers.CurrentValue = rangeOfNumbers.MinValue;
                _Logger.Info(string.Format("CreateStackId: Maximum value reached. StackId reset to 0"));
                
                unitOfWork.Save();
            }
        }
        return newStackId;
    }
    
    
    
    // Schreibt einen neuen Datensatz in die Tabelle base.Stacks
    private void WriteStackRecord(IUnitOfWork unitOfWork, string stackId)
        {
            _Logger.Info("Writing Stack-Record");

            var stackRecord = new HomagGroup.FLS.Domain.Data.Stack();
            
            stackRecord.StackCode = stackId;
            stackRecord.PositionNumber = "PosNr";
            stackRecord.IsActive = 0;
            stackRecord.IsReserved = 0;
            stackRecord.IsValid = 0;
            
            stackRecord.CentreX = null;
            stackRecord.CentreY = null;
            stackRecord.CentreZ = null;
            
            stackRecord.LayerLayout = "LayerLayout";
            
            stackRecord.StackLength = 0;
            stackRecord.StackWidth = 0;
            stackRecord.StackHeight = 0;

            stackRecord.CreationDate = DateTime.Now;
            stackRecord.CreationSource = "cMES"; // Wahrscheinlich dann aber User
            stackRecord.ModificationDate = DateTime.Now;
            stackRecord.ModificationSource = "cMES"; // Wahrscheinlich dann aber User
            stackRecord.Locked = false;
            stackRecord.LockSource = "default";
            
            stackRecord.CustomIsPaused = 0;

            unitOfWork.AddOrUpdate(new[] { stackRecord });
            unitOfWork.Save();
        }
    
}
