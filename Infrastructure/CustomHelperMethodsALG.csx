//-----------------------------------------------------------------------------
//   (Class-)Name:   CustomHelperMethodsALG
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         <Author>
//   Date:           2023-06-28
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   <Author>        2023-06-28    Created
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IGlobalCustomization))]
[Export("CustomHelperMethodsALG", typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IGlobalCustomization))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("Helper Methods for ALG")]
[EnabledScript(true)]
public class CustomHelperMethodsALG : UserExitCustomBase, HomagGroup.FLS.Infrastructure.Common.Customization.IGlobalCustomization
{

[Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;

    private Logger _Logger;

    
    /// <summary>
	///  Ermittlung der ProductionItems 
	///  logger         = logger
	///  parameter  	= Liste von Parameter
	/// </summary>
    /// <param name="logger"></param>
	/// <param name="parameter"></param>
	/// <returns>Als Rückgabe kommen die BasisDataParameterForOpti zurück </returns>
	
    public List<BasisDataParameterForOpti> GetBasisDataParameterForOptis(Logger logger, string parameter)	
	{
		try
		{
			List<BasisDataParameterForOpti> basisDataParameterForOptis = new List<BasisDataParameterForOpti>();
			
			if(string.IsNullOrEmpty(parameter))
				return basisDataParameterForOptis;
			
			string[] paras = parameter.Split('|');

            foreach(var item in paras)
            {
                string[] key = item.Split(':');				
				basisDataParameterForOptis.Add(new BasisDataParameterForOpti(key[0],key[1]));
            }
			
			return basisDataParameterForOptis;
		}
        catch (Exception e)
        {
            logger.Error($"Fehler in Methode: 'GetBasisDataParameterForOpti' Fehler: {e}");
            throw;
        }		
	}

}

public class BasisDataParameterForOpti
{
	public string 	parameter {get;}
	public string 	value {get;}
	
	public BasisDataParameterForOpti(string parameter,string value)
	{
		this.parameter = parameter;
		this.value = value;
	}
}
