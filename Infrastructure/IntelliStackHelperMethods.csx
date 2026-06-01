#r ControllerMES.Infrastructure.Common
#r ControllerMES.Infrastructure.Resource
#r HomagGroup.IntelliStack.Domain.Data.dll

//-----------------------------------------------------------------------------
//   (Class-)Name:   IntelliStackHelperMethods
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         <Author>
//   Date:           2023-09-11
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   <Customizing>   2023-09-11    Created
//-----------------------------------------------------------------------------


using System.ComponentModel;
using HomagGroup.IntelliStack.Domain.Data.Parts;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IServerCustomization))]
[Export("IntelliStackHelperMethods", typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IServerCustomization))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("Helper Methods für IntelliStack")]
[EnabledScript(true)]
public class IntelliStackHelperMethods : UserExitCustomBase, HomagGroup.FLS.Infrastructure.Common.Customization.IServerCustomization
{
    [Import]
    private IIntelliStackServiceInternal _StackService;
    
    [Import]
    protected RangeOfNumbersHelper _RangeOfNumbersHelper;
    
    [Import]
    protected IConfigurablePathsServiceDistributed _PathService;
    
    [Import]
    private IHelperMethodsCommon _HelperMethodsCommon;
    
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;
    
    private int _BaseboardLength = 1200;
    private int _BaseboardWidth = 800;
    
    public List<HomagGroup.FLS.Domain.Data.Stack> CreateIntelliStack(IUnitOfWork unitOfWork, List<ProductionItem> productionItems, Logger logger)
    {
        try 
        {
            var productionItemRepository = unitOfWork.GetRepository<ProductionItem>();
            
            logger.Info("Start IntelliStack");
            
            _BaseboardLength = _HelperMethodsCommon.GetProgramSettings("IntelliStack_BaseboardLength",1200);
            _BaseboardWidth = _HelperMethodsCommon.GetProgramSettings("IntelliStack_BaseboardWidth",800);
            
            
             logger.Info("Stapelparameter bestimmen");
            //Stapelparameter bestimmen
            var stackStructure = unitOfWork.GetRepository<CustStackStructure>().GetFirstOrDefault(x=>x.StackStructureCode == 1);
            
            //Liste mit Bauteilen für IntelliStack erstellen
            var intelliStackPartList = new List<PartToStack>();
            
            string subTarget = "A";
            
            //Alle Bauteile nach Auftrag gruppieren, da die Sortierung innerhalb eines Stapels nach Auftrag sein soll
            logger.Info("Bauteile bestimmen");
            var itemGroups = productionItems.GroupBy(x=> new { x.ProductionOrder.CustomerOrderCode, x.ProductionOrder.CustomerOrderPosition} );
            foreach (var itemGroup in itemGroups)
            {
                var items = productionItems.Where(x=>x.ProductionOrder.CustomerOrderCode == itemGroup.Key.CustomerOrderCode && x.ProductionOrder.CustomerOrderPosition == itemGroup.Key.CustomerOrderPosition).OrderBy(x=>x.ProductionOrder.CustomerOrderCode).ThenBy(x=>x.ProductionOrder.CustomerOrderPosition);
                
                foreach(var productionItem in items)
                {
                     /*
                     int sorterAssemblyUnit = productionItem.SorterItems.Select(x => x.AssemblyUnit).FirstOrDefault();
                    
                    if (stackStructure.OrderingRelevance == YesNo.Yes)
                    {
                        if (productionItem.ProductionOrder.CustomProductionType == CustomProductionType.Serienfertigung)
                        {
                            subTarget = productionItem.ProductionOrder.CustomerOrderCode + "_" + productionItem.ProductionOrder.CustomerOrderPosition;
                        }
                        else if (productionItem.ProductionOrder.CustomProductGroup == CustomProduktGruppe.Bankbau)
                        {
                            //Bei Bankbau wird nach Stärke sortiert und nicht nach Kundenauftrag
                            subTarget = productionItem.ProductionOrder.Thickness.ToString();
                        }
                        else
                        {
                            //subTarget = productionItem.ProductionOrder.CustomerOrderCode + "_" + productionItem.ProductionOrder.CustomerOrderPosition + "_" + productionItem.ProductionOrder.ExtensionData.BomQuantity;
                            subTarget = productionItem.ProductionOrder.CustomerOrderCode + "_" + productionItem.ProductionOrder.CustomerOrderPosition + "_" + sorterAssemblyUnit.ToString();
                        }
                    }
                    */
                    
                    var partToStack = new PartToStack();
                    
                    partToStack.Id = productionItem.Code;
                    partToStack.DimX = GetDimension(productionItem, "X", logger);
                    partToStack.DimY = GetDimension(productionItem, "Y", logger);
                    partToStack.DimZ = (double)productionItem.ProductionOrder.Thickness;
                    partToStack.PartType = PartType.Part;
                    partToStack.SubTarget = subTarget; 
                    partToStack.Target = "A"; 
                    partToStack.SortOut = false;             
                    partToStack.Orientation = PartOrientation.Any;                            
                    partToStack.AccessGroup = "1"; 
                    
                    intelliStackPartList.Add(partToStack);
                }
            }
            
            var intelliStackPartListOrder = intelliStackPartList.OrderBy(x => x.SubTarget).ToList();
            
            logger.Info("Settings füllen");
            //Settings für IntelliStack setzen
            string stackCode = _RangeOfNumbersHelper.GetNewUniqueIdentifier(logger ,"STACKCODES");
            StackSettings settings = SetValuesForSetting(unitOfWork, stackStructure, stackCode, logger);
            
            logger.Info("Aufruf");
            //Start IntelliStack
            var returnID = _StackService.BuildStackFromParts(intelliStackPartListOrder, settings);

            if (string.IsNullOrEmpty(returnID))
            {
                return null;
            }
            
            logger.Info("ReturnID " + returnID);
            
            //Ergebnis von IntelliStack speichern
			int stackCounter = 1;
            var returnStackList = new List<HomagGroup.FLS.Domain.Data.Stack>();
            var intelliStackPiles = unitOfWork.GetRepository<IntelliStackPile>().Get(x=>x.IntelliStackCode == returnID);
            foreach (var intelliStackPile in intelliStackPiles)
            {
                var newStack = new HomagGroup.FLS.Domain.Data.Stack();
                newStack.StackCode = stackCode + "_" + stackCounter.ToString();
                newStack.PositionNumber = "1";
                newStack.IsActive = YesNo.Yes;
                newStack.IsReserved = YesNo.Yes;
                newStack.IsValid = YesNo.Yes;
                newStack.CentreX = 0;
                newStack.CentreY = 0;
                newStack.CentreZ = 0;
                newStack.LayerLayout = "";
                newStack.StackLength = 0M;
                newStack.StackWidth = 0M;
                newStack.StackHeight = 0M;
                newStack.CreationSource = "RequestToIntelliStack";

                //Bauteile zum Stapel erzeugen
                int layerNumber = 1;
                decimal layerIntelliStack = 0M; //z.B. 0/16/32/48 für layer 1/2/3
                int positionInlayer = 0; 

                foreach(var intelliStackPileItem in  intelliStackPile.IntelliStackPileItems.OrderBy(x=>x.ZPosition))
                {
                    int qtyInlayer = intelliStackPile.IntelliStackPileItems.Where(x=>x.ZPosition == intelliStackPileItem.ZPosition).Count();
                    
                    if(layerIntelliStack != intelliStackPileItem.ZPosition)
                    {
                        layerNumber++; // Layer hochzählen
                        layerIntelliStack = intelliStackPileItem.ZPosition; // neue ZPosition als Vergleich setzen
                        positionInlayer = 1 ; //Zurücksetzen, da neuer Layer anfängt 
                    }
                    else
                    {
                        positionInlayer++;// Hochzählen innerhalb des selben layers;
                    }
                    
                    var newStackItem = new StackItem();
                    newStackItem.StackCode = newStack.StackCode;
                    newStackItem.LayerNumber = layerNumber;
                    newStackItem.PositionInLayer = positionInlayer;
                    newStackItem.StackItemCode = intelliStackPileItem.ProductionItemCode;
                    newStackItem.StackItemType = StackItemType.ProductionItem;
                    newStackItem.QuantityInLayer = qtyInlayer;
                    newStackItem.CustomCoordinateX = intelliStackPileItem.XPosition;//GetCoordinate(unitOfWork, intelliStackPileItem,"X");
                    newStackItem.CustomCoordinateY = intelliStackPileItem.YPosition;//GetCoordinate(unitOfWork, intelliStackPileItem,"Y");
                    
                    var productionItem = productionItemRepository.GetFirstOrDefault(x=>x.Code == intelliStackPileItem.ProductionItemCode);
                    
                    if (productionItem != null)
                    {
                        newStackItem.CustomOrientation = (productionItem.ProductionOrder.IsLengthGreaterEqualWidth ? Converters.ToInt16(intelliStackPileItem.ZRotate) : Converters.ToInt16(intelliStackPileItem.ZRotate)+90);
                    }
                    
                    newStackItem.CreationSource = "RequestToIntelliStack";
                    newStack.StackItems.Add(newStackItem);
                }
                unitOfWork.AddOrUpdate(new[]{newStack});
                
                returnStackList.Add(newStack);
				
				stackCounter = stackCounter + 1;
            }
            unitOfWork.Save();
    
            return returnStackList;
        }
        catch (Exception e)
        {
            logger.Error("CreateIntelliStack", null, e);
        }
        
        return null;
    }

    private StackSettings SetValuesForSetting(IUnitOfWork unitOfWork,CustStackStructure stackStructure, string stackCode, Logger logger)
    {
        try
        {            
            var settings = new StackSettings();
            settings.Name  = stackCode + "_" + DateTime.Now.ToString("yyyyMMddHHmm");
            settings.DebugResponseFolder = _PathService.GetPath("IntelliStack_Request_Archive");

            //Stapelunterlage
            settings.PalletLength = _BaseboardLength;
            settings.PalletWidth = _BaseboardWidth;

            //Overlapwerte setzen
            settings.OverlapXPlus = (double)stackStructure.OverlapXPlus;
            settings.OverlapXMinus = (double)stackStructure.OverlapXMinus;
            settings.OverlapYPlus = (double)stackStructure.OverlapYPlus;
            settings.OverlapYMinus = (double)stackStructure.OverlapYMinus;

            //Max Overlap setzen
            settings.OverlapMaxXPlus = (double)stackStructure.OverlapMaxXPlus;
            settings.OverlapMaxXMinus = (double)stackStructure.OverlapMaxXMinus;
            settings.OverlapMaxYPlus = (double)stackStructure.OverlapMaxYPlus;
            settings.OverlapMaxYMinus = (double)stackStructure.OverlapMaxYMinus;

            //Abstand zwischen den Teilen
            settings.DistanceBetweenParts = (double)stackStructure.DistanceBetweenParts;

            //Lagenversatz
            if (stackStructure.UseAlternatingOffset == YesNo.Yes)
            {
                settings.UseAlternatingOffset = true;
                settings.AlternatingOffsetX = (double)stackStructure.AlternatingOffsetX;
                settings.AlternatingOffsetY = (double)stackStructure.AlternatingOffsetY;
            }
            

            //Lane Limits
            if (stackStructure.UseLaneLimits == YesNo.Yes)
            {
                settings.UseLaneLimits = true;
                settings.LaneHeightMin = (double)stackStructure.LaneHeightMin;
                settings.LaneHeightMax = (double)stackStructure.LaneHeightMax;
                settings.LaneHeightTolerance = (double)stackStructure.LaneHeightTolerance;
            }

            //Stapelumkehrbarkeit
            if (stackStructure.StackReversable == YesNo.Yes)
            {
                settings.StackReversable = true;
            }
            
            //Anzahl der Bauteile in X-Richtun; NULL = beliebig
            if(stackStructure.PartsInXDirection == 0)
            {
                settings.PartsInXDirection = null;
            }
            else
            {
                settings.PartsInXDirection = stackStructure.PartsInXDirection;
            }
            
            //Anzahl der Bauteile in Y-Richtun; NULL = beliebig
            if(stackStructure.PartsInYDirection == 0)
            {
                settings.PartsInYDirection = null;
            }
            else
            {
                settings.PartsInYDirection = stackStructure.PartsInYDirection;
            }

            // Eckenausrichtung des Stapels/chatotischer Stapel
			if (stackStructure.IntelliStackAlignment == HomagGroup.FLS.Domain.Data.IntelliStackAlignment.CorneredSouthWest)
			{
				settings.Alignment = HomagGroup.FLS.Services.IntelliStack.Contracts.IntelliStackAlignment.CorneredSouthWest;
			}
			else if (stackStructure.IntelliStackAlignment == HomagGroup.FLS.Domain.Data.IntelliStackAlignment.CorneredSouthEast)
			{
				settings.Alignment = HomagGroup.FLS.Services.IntelliStack.Contracts.IntelliStackAlignment.CorneredSouthEast;
			}
			else if (stackStructure.IntelliStackAlignment == HomagGroup.FLS.Domain.Data.IntelliStackAlignment.CorneredNorthEast)
			{
				settings.Alignment = HomagGroup.FLS.Services.IntelliStack.Contracts.IntelliStackAlignment.CorneredNorthEast;
			}
			else if (stackStructure.IntelliStackAlignment == HomagGroup.FLS.Domain.Data.IntelliStackAlignment.CorneredNorthWest)
			{
				settings.Alignment = HomagGroup.FLS.Services.IntelliStack.Contracts.IntelliStackAlignment.CorneredNorthWest;
			}
			else if (stackStructure.IntelliStackAlignment == HomagGroup.FLS.Domain.Data.IntelliStackAlignment.Centric)
			{
				settings.Alignment = HomagGroup.FLS.Services.IntelliStack.Contracts.IntelliStackAlignment.Centric;
			}
			else
			{
			    settings.Alignment = HomagGroup.FLS.Services.IntelliStack.Contracts.IntelliStackAlignment.Default; /*chaotisher Stapel [0]*/
			}
			
			//Neue Parameter für Asymetrische und Symetrische Türmchen
			var checkAlternatives = _HelperMethodsCommon.GetProgramSettings("IntelliStack_CheckAlternatives","true");
            if (checkAlternatives == "true")
            {
                var factorOverhangX = unitOfWork.GetRepository<ProgramSetting>().GetFirstOrDefault(x=>x.Identifier == "IntelliStack_FactorOverhangX");
                var factorOverhangY = unitOfWork.GetRepository<ProgramSetting>().GetFirstOrDefault(x=>x.Identifier == "IntelliStack_FactorOverhangY");
                var factorOverhangSymPartsX = unitOfWork.GetRepository<ProgramSetting>().GetFirstOrDefault(x=>x.Identifier == "IntelliStack_FactorOverhangSymPartsX");
                var factorOverhangSymPartsY = unitOfWork.GetRepository<ProgramSetting>().GetFirstOrDefault(x=>x.Identifier == "IntelliStack_FactorOverhangSymPartsY");
                
                if (factorOverhangX != null && factorOverhangY != null && factorOverhangSymPartsY != null && factorOverhangSymPartsY != null)
                {
                    settings.FactorOverhangX = Convert.ToDouble(factorOverhangX.ValueFloat);
                    settings.FactorOverhangY = Convert.ToDouble(factorOverhangY.ValueFloat);
                    settings.FactorOverhangSymPartsX = Convert.ToDouble(factorOverhangSymPartsX.ValueFloat);
                    settings.FactorOverhangSymPartsY = Convert.ToDouble(factorOverhangSymPartsY.ValueFloat);
                }
            }
            
            return settings;   

        }
        catch(Exception e)
        {
            return null;
        }
    }
    
    private double GetDimension(ProductionItem prodItem, string dimension, Logger _Logger)
    {
        try
        {          

            decimal length = prodItem.ProductionOrder.Length;
            decimal width = prodItem.ProductionOrder.Width;
            
            if(dimension == "X")
            {
                if(length >= width)
                {
                    return (double)length;
                }
                else
                {
                    return (double)width;
                }
            }
            else // dimension == "Y"
            {
                if(length >= width)
                {
                   return (double)width; 
                }
                else
                {
                    return (double)length;
                }        
            
            }
        }
        catch(Exception e)
        {
            _Logger.Error("Fehler beim Ermitteln der Maße für ProdItem:" + prodItem.Code,null, e);
            return 0;
        }
        
    
    }
    
    private decimal GetCoordinate(IUnitOfWork unitOfWork, IntelliStackPileItem intelliStackPileItem, string axis)
    {        
        decimal length = 0M;
        decimal width = 0M;
        
        
        decimal boardLength = _BaseboardLength;
        decimal boardWidth = _BaseboardWidth;
        
        if(intelliStackPileItem.ProductionItem.ProductionOrder.CuttingLength >= intelliStackPileItem.ProductionItem.ProductionOrder.CuttingWidth)
        {
        	length = (decimal)intelliStackPileItem.ProductionItem.ProductionOrder.CuttingLength;
        	width = (decimal)intelliStackPileItem.ProductionItem.ProductionOrder.CuttingWidth;
        }
        else
        {
        	length = (decimal)intelliStackPileItem.ProductionItem.ProductionOrder.CuttingWidth;
        	width = (decimal)intelliStackPileItem.ProductionItem.ProductionOrder.CuttingLength;
        }

        //Erst einmal den aktuellen Mittelpunkt des Bauteils berechnen
        decimal intelliX = 0M;
        decimal intelliY = 0M;
        
        
        if(intelliStackPileItem.ZRotate == 0)
        {
        	intelliY = width/2 + intelliStackPileItem.YPosition;
        	intelliX = length/2 + intelliStackPileItem.XPosition;
        }
        else
        {
        	intelliY = length/2 + intelliStackPileItem.YPosition;
        	intelliX = width/2 + intelliStackPileItem.XPosition;
        }

        
        decimal newPositionX = 0M;
        decimal newPositionY = 0M;
        
        if(axis == "X")
        {
            if(intelliX > boardLength/2)
            {
            	newPositionX = intelliX - (boardLength/2);
            }	
            else if(intelliX < boardLength/2)
            {
            	newPositionX = (-1)*((boardLength/2) - intelliX);
            }
            else
            {
            	newPositionX = 0;
            }
            
            return newPositionX;
        }
        else
        {
            if(intelliY > boardWidth/2)
            {
            	newPositionY = (-1)*((intelliY - boardWidth/2));
            }	
            else if(intelliY < boardWidth/2)
            {
            	newPositionY = (boardWidth/2)-intelliY;
            }
            else
            {
            	newPositionY = 0;
            }
            
            return newPositionY;
        
        }
    }

/* S.Feist - Deaktivierung für die testumgebung

	public void UpdateHandlingItemsAndSorterItems(IUnitOfWork unitOfWork, List<HomagGroup.FLS.Domain.Data.Stack> stacks, Logger logger)
	{
	   foreach (var stack in stacks)
	   {
	   
	   //START: Erweiterung durch TB - Ermittlung Stapelziel//
	       int totalItemsInStack = stack.StackItems.Count(); //Anzahl Bauteile im Stapel
	       int countItemsCnc = 0; //Anzahl Bauteile im Stapel mit BHX Bearbeitung - Variable initilizieren
	       int stackDestination = 99; // Defaultwert für Stapelziel
	       
	       //Berechnung der Anzahl Bauteile im Stapel die über die BHX laufen
	       foreach (var itemStack in stack.StackItems)
	       {
	           var productionItem = unitOfWork.GetRepository<ProductionItem>().GetFirstOrDefault(x => x.Code == itemStack.StackItemCode);
	           
	           if (productionItem != null && productionItem.ProductionOrder.ProductionSteps.Any(x => x.WorkCenterCode == "BHX"))
	           {
	               countItemsCnc++;
	           }
	       }
	       
	       //In ProgramSettings wird definiert, ab welchem % ein Stapel als Stapel mit vielen (oder wenige) CNC Bauteile betrachtet wird
	       var progSettings = unitOfWork.GetRepository<ProgramSetting>().GetFirstOrDefault(x => x.Identifier == "ValueStapelCncTeile");
	       
	       if (progSettings != null)
	       {
	           decimal ratioCncParts = (countItemsCnc/totalItemsInStack)*100;
	           
	           //Wenn ein Stapel wenige CNC Bauteile hat, dann soll diese im Stapelplatz 1 ausgelagert werden.
	           if (ratioCncParts < progSettings.ValueFloat)
	           {
	               stackDestination = 1;
	           }
	           
	           //Wenn ein Stapel viele CNC Bauteile hat, dann soll diese im Stapelplatz 1 ausgelagert werden.
	           else
	           {
	               stackDestination = 2;
	           }
	       }
	       
	       //ENDE: Erweiterung durch TB - Ermittlung Stapelziel//
	       
    	   foreach (var stackItem in stack.StackItems.OrderBy(x=>x.LayerNumber).ThenBy(x=>x.PositionInLayer))
    	   {
    	       var handlingItem = unitOfWork.GetRepository<CustHandlingItem>().GetFirstOrDefault(x=>x.ProductionItemCode == stackItem.StackItemCode);
    	       if (handlingItem != null)
    	       {
    	           handlingItem.SortingGroupInput = stackItem.StackCode + "|" + stackItem.LayerNumber.ToString().PadLeft(3,'0');;
    	       }
    	       
    	       
    	       var sorterItem = unitOfWork.GetRepository<CustSorterItem>().GetFirstOrDefault(x=>x.ProductionItemCode == stackItem.StackItemCode);
    	       if (sorterItem != null)
    	       {
        	       sorterItem.SortingUnitGroup = stackItem.StackCode;
        	       sorterItem.SortingGroupInput  = stackItem.StackCode + "|" + stackItem.LayerNumber.ToString().PadLeft(3,'0');
        	       sorterItem.SortingGroupOutput = stackItem.StackCode + "|" + stackItem.LayerNumber.ToString().PadLeft(3,'0');	       
        	       sorterItem.CustomStackingDestination = stackDestination;
    	       }
    	   }
	   }
	   
	   unitOfWork.Save ();
	}
*/

	public bool CheckIsResultValid(IUnitOfWork unitOfWork,List<ProductionItem> productionItems, List<HomagGroup.FLS.Domain.Data.Stack> stacks, Logger logger)
	{
	   //Prüfen ob alle Teile die angefordert waren auch in einem Stapel auftauchen, wenn nicht liegt ein Fehler vor
        foreach (var productionItem in productionItems)
        {
            int isInStack = 0;
            foreach (var stack in stacks)
            {
                if (stack.StackItems.Where(x=>x.StackItemCode == productionItem.Code).Count() > 0)
                {
                    isInStack = 1;
                    break;
                }
            }
            
            if (isInStack == 0)
            {
                logger.Error("Bauteil ist in keinem Stapel enthalten " + productionItem.Code);
                return false;
            }
        }
	   
	   
	   return true;
	}
	
	
}
