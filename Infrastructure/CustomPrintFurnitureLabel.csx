#r ControllerMES.Infrastructure.Resource
#r ToastNotifications
#r WindowsBase
#r PresentationFramework

//-----------------------------------------------------------------------------
//   (Class-)Name:   CustomPrintFurnitureLabel
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         T.Stürzer
//   Date:           2023-02-14
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   T.Stürzer       2023-02-14    Created
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;
using System.Collections;
using ToastNotifications;
using ToastNotifications.Position;
using HomagGroup.FLS.Services.Common.Service.Messaging;
using HomagGroup.FLS.Services.Common.Service.Messaging.Provider;


// TODO MESSAGING: activate, if messaging-functionality is needed (eg. navigate to a tile-view and execute an ad-hoc filter)
// TODO REFRESH: activate, if refreshing a tile-view is needed



[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IClientCustomization))]
[Export("CustomPrintFurnitureLabel", typeof(HomagGroup.FLS.Infrastructure.Framework.Contracts.ICommandUserExit))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("Print furniture label in tile 'Assembly'")]
[EnabledScript(true)]
public class CustomPrintFurnitureLabel : UserExitCustomBase, HomagGroup.FLS.Infrastructure.Framework.Contracts.ICommandUserExit, IPartImportsSatisfiedNotification // TODO MESSAGING
{
#pragma warning disable 0649
    // TODO REFRESH
    ///// <summary>
    ///// UserExitHelper
    ///// </summary>
    //[Import]
    //protected UserExitHelper UserExitHelper { get; set; }



    // TODO MESSAGING
    ///// <summary>
    ///// DistributedServiceProvider
    ///// </summary>
    //[Import]
    //protected IDistributedServiceProvider DistributedServiceProvider { get; set; }
#pragma warning restore 0649



    // TODO MESSAGING
    ///// <summary>
    ///// Messaging service interface.
    ///// </summary>
    //private IMessagingService _MessagingService;


    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;

    private Logger _Logger;
    
    ///// <summary>
    ///// ToastNotification-instance.
    ///// </summary>
     protected ToastNotification ToastNotificationInfo { get; private set; }
     
    ///// <summary>
    ///// bool - true, if ToastNotification for info-messages has been shown.
    ///// </summary>
    protected bool ToastNotificationInfoActive = false;
    
    [Import]
    protected IDistributedServiceProvider DistributedServiceProvider { get; private set; }
    
    [Import]
    protected IMessagingService _MessagingService { get; private set; }
    
    [Import]
    protected IHelperMethodsCommon HelperMethodsCommon { get; set; }
    
    private ToastNotification _ToastNotification = null;
    
    public CustomPrintFurnitureLabel()
    {
        ToastNotificationInfo = new ToastNotification(Corner.BottomCenter, 0.0D, 500, 1000, 5, 5);
    }

    public void Execute(object parameter)
    {
        Guard.ThrowOnArgumentNull(parameter, "parameter");

        try
        {
            _Logger = LogHelper.GetLogger(Name, typeof(CustomPrintFurnitureLabel));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, "");

            var itemEnumerable = parameter as IEnumerable;
            string infoBalloonMessage = "";
            ResourceIdentifier infoMsg01 = null;

            if (itemEnumerable != null)
            {               
                using(var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
                {                     
                    foreach(var item in itemEnumerable.OfType<HomagGroup.FLS.Domain.Data.ProductionOrder>())
                    {                        
                        var prodOrders = unitOfWork.GetRepository<ProductionOrder>().Get(po => po.Code == item.Code && po.OrderType == ProductionOrderType.SalesArticle);
                        
                        foreach(var prodOrder in prodOrders)
                        {
                            foreach(var prodItem in prodOrder.ProductionItems)
                            {
                                var printJob = new PrintJobItem();
                                
                                printJob.ProductionItemCode = prodItem.Code;
                                printJob.ProductionOrderCode = prodItem.ProductionOrderCode;
                                printJob.JobName = "PrintFurnitureLabel";
                                printJob.ProcessingState = PrintJobItemProcessingState.ReadyForPrinting; // Status = 10
                                printJob.CreationSource = "PrintFurnitureLabel";
                                
                                unitOfWork.AddOrUpdate(new[] {printJob});
                            }
                            
                            infoMsg01 = ResourcesKeys.CommonMessage(string.Format("{0}: Furniture label for the article {1} will be printed", prodOrder.CustomerOrderCode, prodOrder.ArticleDescription));
                            SendToastNotificationInfo(infoMsg01);
                        }
                       
                        unitOfWork.Save();
                    }
                }
            }
        }
        catch (Exception e)
        {
            _Logger.Error(ResourcesKeys.ErrorInUserExit(Name), null, e);
            throw;
        }
        
        //// Try to delete Toast-Message
        try
        {
            
            if(!ToastNotificationInfoActive) { ToastNotificationInfo.Dispose(); }
        }
        catch (Exception ex)
        {
            _Logger.Error(ResourcesKeys.EndingTaskWithFailure(Name), null, ex);
            throw;
        }        
    }
    
    ///// <summary>
    ///// Shows a ToastNotification of type 'Information'.
    ///// </summary>
    ///// <param name="msg">ResourceIdentifier - message to display.</param>
    protected virtual void SendToastNotificationInfo(ResourceIdentifier msg)
    {
        Guard.ThrowOnArgumentNull(msg, "msg");

        if (ToastNotificationInfo != null)
        {

            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                ToastNotificationInfo.Show(ToastNotification.ToastNotificationType.Success, msg.GetText(HelperMethodsCommonStatic.GetCultureInfoForPoErrorMessage()));
            });


            ToastNotificationInfoActive = true;

            _Logger.Info(msg);
        }
    }
    
    // Ausgabe ToastNotification
    public void SendToastNotification(ToastNotification.ToastNotificationType notificationType, string message)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            _ToastNotification.Show(notificationType, message);
        });
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



    // TODO MESSAGING
    ///// <summary>
    ///// Called when a part's imports have been satisfied and it is safe to use.
    ///// </summary>
    public void OnImportsSatisfied()
    {
        _MessagingService = DistributedServiceProvider.GetService<IMessagingService>();
    }
}
