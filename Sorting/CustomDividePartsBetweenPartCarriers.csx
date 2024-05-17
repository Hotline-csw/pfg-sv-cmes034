#r HomagGroup.FLS.Services.Sorting.Service


//-----------------------------------------------------------------------------
//   (Class-)Name:   CustomDividePartsBetweenPartCarriers
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         T.Stürzer
//   Date:           2023-02-20
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   T.Stürzer       2023-02-20    Created
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IServerCustomization))]
[Export("CustomDividePartsBetweenPartCarriers", typeof(HomagGroup.FLS.Services.Sorting.Contracts.Provider.ISortingProvider))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("Divide parts between part carriers")]
[EnabledScript(true)]
public class CustomDividePartsBetweenPartCarriers : HomagGroup.FLS.Services.Sorting.Service.Provider.DefaultProvider.DefaultSortingProvider
{
    private static readonly Logger _Logger = LogHelper.GetLogger("CustomDividePartsBetweenPartCarriers", typeof(CustomDividePartsBetweenPartCarriers));
    
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;   
    
    
    #region ItemFitting Virtual Methods to be overwritten 
        
    /// <summary>
    /// Determines if the given Item is fitting into the given Compartment
    /// </summary>
    /// <param name="productionItem">The production item.</param>
    /// <param name="compartment">The compartment where the item should fit in.</param>
    /// <returns>True if the Item does Fit.</returns>
    public override  bool IsItemFitting(ProductionItem productionItem, Compartment compartment)
    {
        bool result = true;
              

            // you can do any verifications here. 
            // The base method already test the Height / Width / Depth in Horizontal
            // and Vertical layout , including IgnoreHeiht, Width and Depth supplement
        result = base.IsItemFitting(productionItem, compartment);
            
        return result;
    }


    /// <summary>
    /// Possibility to check if Item does match the given partcarrier as the base propose.
    /// </summary>
    /// <param name="productionItem">The production item which should be stored in the partcarrier.</param>
    /// <param name="partCarrier"></param>
    /// <returns>true if item can be stored in the partcarrier</returns>
    public override bool IsPartCarrierCandidate(ProductionItem productionItem, PartCarrier partCarrier)
    {   
        List<int> frontTypes = new List<int>{7,8,9};
        List<int> carcaseTypes = new List<int>{1,2,3,4,5,6,11,12,13,20,21,24};
        List<int> drawerTypes = new List<int>{17,18,19,25};
        List<string> frontCarrierCodes = new List<string>{"FrontCarriage01","FrontCarriage02","FrontCarriage03","FrontCarriage04","FrontCarriage05"};
        List<string> carcaseCarrierCodes = new List<string>{"CarcaseCarriage01","CarcaseCarriage02","CarcaseCarriage03","CarcaseCarriage04","CarcaseCarriage05"};
        List<string> drawerCarrierCodes = new List<string>{"DrawerCarriage01","DrawerCarriage02","DrawerCarriage03","DrawerCarriage04","DrawerCarriage05"};
        
        bool ret = false;
        
        if((frontTypes.Contains((int)productionItem.ProductionOrder.ComponentType) &&
            partCarrier.PartCarrierType == PartCarrierType.FrontsCart &&
            frontCarrierCodes.Contains((string)partCarrier.Code))
            ||       
            (carcaseTypes.Contains((int)productionItem.ProductionOrder.ComponentType) &&
            partCarrier.PartCarrierType == PartCarrierType.CorpusCart &&
            carcaseCarrierCodes.Contains((string)partCarrier.Code))
            ||
            (drawerTypes.Contains((int)productionItem.ProductionOrder.ComponentType) &&
            partCarrier.PartCarrierType == PartCarrierType.ShelfCart &&
            drawerCarrierCodes.Contains((string)partCarrier.Code)))
        {
            return true;
        }
        return ret;
    }


    /// <summary>
    /// Each Item kann be given a custom capacity (see ProductionItemCapacity()) that will be stored in the DB Table PartCarrierProductionItems
    /// This capicity has no unit and is tested in the base against the total capacity of the Compartment
    /// The Compartment Capacity can even be custom modified (see CompartmentCapacity())
    /// </summary>
    /// <param name="productionItem">The production item.</param>
    /// <param name="compartment">The compartment where the item should fit in.</param>
    /// <returns>True if the item does fit</returns>
    public override  bool IsItemCapacityFitting(ProductionItem productionItem, Compartment compartment)
    {
        bool result = true;
        // you can do any verifications here. You can test the weight from the production item.
        result = base.IsItemFitting(productionItem,compartment);
        return result;
    }

    #endregion

    #region Capacity Virtual Methods to be overwritten 

    /// <summary>
    /// Capacity that takes the item. You can give a weight capacity for example with no Unit.
    /// this capacity will be checked against the compartment Capaicty.
    /// <param name="productionItem">The relations between production items and compartments.</param>
    /// </summary>
    /// <returns>capacity</returns>
    public override decimal ProductionItemCapacity(ProductionItem productionItem)
    {

        // you can do any verifications here. You can test the weight from the production item.
        var itemCapacity =  base.ProductionItemCapacity(productionItem);
          
        return itemCapacity;
    }


    /// <summary>
    /// Determines the Max Capacity for the compartment, this value is saved into the Database table Comapartments !
    /// This capaicty is also used , when filled, to calculate the fillind Degree.
    /// The filling degree is the greatest occupancy  type between : WidthUsed (not for palette and Floorstorage) / places used / capacity.
    /// <param name="compartment">compartment</param>
    /// <param name="productionItem">production item</param>
    /// </summary>
    /// <returns>capacity</returns>
    public override decimal CompartmentCapacity(Compartment compartment,ProductionItem productionItem)
    {
        var compartmentCapacity = base.CompartmentCapacity(compartment, productionItem);
            
        return compartmentCapacity;
    }

    #endregion

    #region OrderBy Virtual Methods to be overwritten 

    /// <summary>
    /// Determines in wich sequence the production items are stored.
    /// </summary>
    /// <returns>OrderBy string.</returns>
    public override string ProductionItemsOrderBy()
    {
        // the Base Order by string looks like "ProductionOrder.Length DESC, ProductionOrder.Width DESC, ProductionOrder.Thickness DESC"
        // to sort Ascend Use ASC or nothing, to sort Descend use DESC
    return  base.ProductionItemsOrderBy();
    }


    /// <summary>
    /// Determines in wich sequence the compartments are choosen for an item to be stored if several are available.
    /// </summary>
    /// <returns>OrderBy string</returns>
    public override string CompartmentsOrderBy()
    {

        // the Base Order by string looks like "Code"
        // to sort Ascend Use ASC or nothing, to sort Descend use DESC
        return base.CompartmentsOrderBy();
    }

    
    /// <summary>
    /// Determines the sorting order in wich the partcarriers are taken as candidate
    /// </summary>
    /// <returns>Sorting order string</returns>
    public override string PartCarriersOrderBy()
    {
        // Use to determin if the first available PartCarrier from a group of candidates is the once with the most
        // remaining places or this with the less remaining places. This orderBy is made on RemainingPlaces.
        // the possible values are  : return "ASC" (our Default);  or  return "DESC";
        return base.PartCarriersOrderBy();
    }

    #endregion

    #region Picking / Sorting Virtual Methods to be overwritten 

    /// <summary>
    /// This Function cann be used to do some custom action before and after picking.
    /// </summary>
    /// <param name="sortStep">Actual Sortstep.</param>
    /// <param name="sortFeatureValue">SortFeatureValue for wich to Sort</param>
    /// <param name="compartment">Compartment to sort In</param>
    /// <param name="productionItem">Item to be stored</param>
    /// <param name="errorMessage">Error Message to be diplayed</param>
    /// <returns>True if the picking was succesfull</returns>
    public override bool Sort(SortStep sortStep,string sortFeatureValue, Compartment compartment, ProductionItem productionItem, out string errorMessage)
    {
        errorMessage = string.Empty;
        bool ret = base.Sort(sortStep,sortFeatureValue, compartment, productionItem, out errorMessage);
        
        return ret;
        
             using (IUnitOfWork unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
             {
                var productionItemFeedback = unitOfWork.GetRepository<ProductionItem>().Get(i => i.Code == productionItem.Code).FirstOrDefault();
                 if (ret && productionItemFeedback != null)
                 {
                    var feedBack = new Feedback();
                    feedBack.ProductionItemCode = productionItemFeedback.Code;
                    feedBack.ProductionStepCode = "SORT";
                    feedBack.WorkcenterCode = "5070";
                    feedBack.CountGood = 1;
                    feedBack.CountScrap = 0;
                    feedBack.CountRework = 0;
                    feedBack.Timestamp = DateTime.Now;
                    feedBack.FeedbackState = FeedbackState.Finished;
                    feedBack.ProcessingState = 10;
                    feedBack.ProcessingTime = 0;

                    feedBack.CreationDate = DateTime.Now;
                    feedBack.Locked = false;
                    feedBack.ModificationDate = DateTime.Now;
                    
                    //-------- CreationSource --------
                    feedBack.CreationSource ="Sorting";
                    
                    //-------- ModificationSource --------
                    feedBack.ModificationSource="Sorting";
                    feedBack.LockSource="default";

                    unitOfWork.AddOrUpdate(new[] {feedBack});
                    unitOfWork.Save();
                 }
                 else
                 {
                    _Logger.Error("Part has not been sorted");
                 }
             }
             return ret;        
    }


    /// <summary>
    /// This Function cann be used to do some custom action before and after picking.
    /// </summary>
    /// <param name="compartment">compartment</param>
    /// <param name="partCarrierProductionItems">list to be picked</param>
    /// <returns>True if the picking was succesfull</returns>
    public override bool Pick(Compartment compartment, ICollection<PartCarrierProductionItem> partCarrierProductionItems)
    {
            
        return base.Pick(compartment, partCarrierProductionItems);
            
           
    }
       
       
    /// <summary>
    /// Force the picking, even if items are not completly sorted.
    /// ForcePicking = true means that no message will send to user when Items are missing, the picking will be done
    /// <param name="compartment">compartment that was selected</param>
    /// </summary>
    public override bool ForcePicking(Compartment compartment)
    {
        return false;
    }

    #endregion
}
