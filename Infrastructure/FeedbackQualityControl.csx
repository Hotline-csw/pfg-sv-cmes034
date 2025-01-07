#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   FeedbackQualityControl
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         <Author>
//   Date:           2025-01-07
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   <Author>        2025-01-07    Created
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;
using System.Collections;


// TODO MESSAGING: activate, if messaging-functionality is needed (eg. navigate to a tile-view and execute an ad-hoc filter)
// TODO REFRESH: activate, if refreshing a tile-view is needed



[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IClientCustomization))]
[Export("FeedbackQualityControl", typeof(HomagGroup.FLS.Infrastructure.Framework.Contracts.ICommandUserExit))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("describe here")]
[EnabledScript(true)]
public class FeedbackQualityControl : UserExitCustomBase, HomagGroup.FLS.Infrastructure.Framework.Contracts.ICommandUserExit//, IPartImportsSatisfiedNotification // TODO MESSAGING
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

    public void Execute(object parameter)
    {
        Guard.ThrowOnArgumentNull(parameter, "parameter");

        try
        {
            _Logger = LogHelper.GetLogger(Name, typeof(FeedbackQualityControl));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, "");

            var itemEnumerable = parameter as IEnumerable;

            if (itemEnumerable != null)
            {
                throw new NotImplementedException();


                // TODO MESSAGING // TODO: implement filter
                //Filter adHocFilter = null;
                //NavigateToViewMessage navigate2Message = new NavigateToViewMessage(this, "ProductionOrder", "ProductionOrder", adHocFilter); //, navigationParameters);
                //_MessagingService.PublishOnlyLocal(navigate2Message);

                // TODO REFRESH
				// Refresh the active View
                //UserExitHelper.RefreshView();
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



    // TODO MESSAGING
    ///// <summary>
    ///// Called when a part's imports have been satisfied and it is safe to use.
    ///// </summary>
    //public void OnImportsSatisfied()
    //{
    //    _MessagingService = DistributedServiceProvider.GetService<IMessagingService>();
    //}
}
