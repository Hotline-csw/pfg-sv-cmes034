//-----------------------------------------------------------------------------
//   (Class-)Name:   NewUserExit
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         <Author>
//   Date:           2024-06-03
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   <Author>        2024-06-03    Created
//   
//-----------------------------------------------------------------------------


using System.Collections;
using System.ComponentModel;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IServerCustomization))]
[Export("NewUserExit", typeof(HomagGroup.FLS.Infrastructure.Common.Rest.IRestFunction))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("describe here")]
[EnabledScript(true)]
public class NewUserExit : UserExitCustomBase, HomagGroup.FLS.Infrastructure.Common.Rest.IRestFunction
{
    
    public HttpResponseMessage Execute(IDictionary<string, object> parameters, object content, string contentType)
    {
        throw new NotImplementedException();
    }
}
