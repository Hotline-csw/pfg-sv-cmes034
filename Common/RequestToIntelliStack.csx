#r ControllerMES.Infrastructure.Common
#r ControllerMES.Infrastructure.Resource
#r HomagGroup.IntelliStack.Domain.Data.dll


//-----------------------------------------------------------------------------
//   (Class-)Name:   RequestToIntelliStack
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         Roland Jakob
//   Date:           2023-10-10
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   <Author>        2023-10-10    Created
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;
using HomagGroup.IntelliStack.Domain.Data.Parts;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IServerCustomization))]
[Export("RequestToIntelliStack", typeof(HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("controls stack creation by IntelliStack")]
[EnabledScript(false)]
public class RequestToIntelliStack : GenericTaskBase, HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;

    private Logger _Logger;
    
        
    [Import]
    private IIntelliStackServiceInternal _StackService;
    
    [Import]
    protected RangeOfNumbersHelper _RangeOfNumbersHelper;
    
    decimal _BoardLength = 0M;
    decimal _BoardWidth = 0M;
	string _BaseBoard = "";

    public override void Execute(IJobExecutionContext executionContext)
    {
        HomagGroup.FLS.Infrastructure.Common.ComponentModel.Guard.ThrowOnArgumentNull(executionContext, "executionContext");

        try
        {
            _Logger = LogHelper.GetLogger(executionContext.Area, typeof(RequestToIntelliStack));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);

            using (var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
            {
                _Logger.Info("Start RequestToIntelliStack");
                
                var stacks = unitOfWork.GetRepository<HomagGroup.FLS.Domain.Data.Stack>().Get(x=>x.CustomStackType == StackType.StackFromOptimization && x.CustomStackState == StackState.StackCreated).Take(10); 
                
                foreach(var stack in stacks)
                {                    
                    var listOfPartToStack = new List<PartToStack>();
                    _Logger.Info("Aktueller Stapel: " + stack.StackCode);  
                    
                    foreach(var stackItem in stack.StackItems)
                    {
                        var prodItem = unitOfWork.GetRepository<ProductionItem>().GetFirstOrDefault(x=>x.Code == stackItem.StackItemCode);
                        
                        if(prodItem != null)
                        {                            
                            var partToStack = new PartToStack();
                            
                            partToStack.Id = prodItem.Code;
                            partToStack.DimX = GetDimension(prodItem, "X", _Logger);
                            partToStack.DimY = GetDimension(prodItem, "Y", _Logger);
                            partToStack.DimZ = GetThickness(prodItem, unitOfWork, _Logger);
                            partToStack.PartType = PartType.Part;
                            partToStack.Target = "A"; 
                            partToStack.SortOut = false;                            
                            partToStack.Orientation = PartOrientation.Any;                            
                            partToStack.AccessGroup = "1";                            
                            
                            listOfPartToStack.Add(partToStack);
                        }
						else
						{
							_Logger.Error(String.Format("Fehler beim Ermitteln des Items [{0}] ",stackItem.StackItemCode));
						}
                    }
                    
					_BaseBoard = "";// Wird beim Setzen der Settings gefüllt					
					
					_Logger.Info("BaseBoardList ermitteln");
					var firstStackItem = stack.StackItems.FirstOrDefault();
					
					bool validBaseBoards = false;
					var Id1 = "";
					var Id2 = "";
					var Id3 = "";
					var Id4 = "";
					List<ListForValidation> intelliStackResultList = new List<ListForValidation>();// Die Liste ist notwendig, um IntelliStack-ID mit Schonplatte zu verbinden

					var firstItem = unitOfWork.GetRepository<ProductionItem>().GetFirstOrDefault(x=>x.Code == firstStackItem.StackItemCode);
					if (firstItem != null)
					{
                        var stackStructure = unitOfWork.GetRepository<CustStackStructure>().GetFirstOrDefault(x=>x.StackStructureCode == 1 /*firstItem .ProductionOrder.CustomStackStructureCode*/);
                        if (stackStructure != null)
                        {
                            _Logger.Info("BaseBoardList" + stackStructure.BaseBoardList);
							if(stackStructure.BaseBoardList == "SPL1" || stackStructure.BaseBoardList == "SPL2")
                            {
                                StackSettings settings = SetValuesForSetting(unitOfWork,listOfPartToStack,_Logger,stackStructure.BaseBoardList,stack.StackCode);
								Id3 = _StackService.BuildStackFromParts(listOfPartToStack, settings);
								intelliStackResultList.Add(new ListForValidation {Id = Id3,BaseBoard = stackStructure.BaseBoardList});
								validBaseBoards = true; // Zulässige Schonplatte
                                
                            }
                            
					    }
						// Falls keine Stammdaten vorhanden sind, oder eine ungültige Schonplatte angegeben ist, muss die Stapelanforderung trotzdem abgearbeitet werden, 
						//da sonst der Sortierer irgendwann still steht
						// In so einem fall wird immer die große Schonplatte genommen und mit Default Werten gearbeitet
					    if(stackStructure == null || validBaseBoards == false)
						{
							_Logger.Error("Fehler beim Ermitteln der Stammdaten aus StackStructure. Angabe Schonplatte evtl. fehlerhaft");
							_Logger.Info("Es wird daher mit SPL2 und Default Wertden gearbeitet");
							
							StackSettings settings = SetDefaultSettings(unitOfWork,_Logger,"SPL2",stack.StackCode);	
							Id4 = _StackService.BuildStackFromParts(listOfPartToStack, settings);
							intelliStackResultList.Add(new ListForValidation {Id = Id4,BaseBoard = "SPL2"});
						}
						
						// Es wird geprüft,welches IntelliStack Ergebnis genommen werden soll
						// Wenn nur eine SPL angegeben wurde, dann wird der Stapel mit den meisten Bauteil genommen
						// Wenn beide SPL angefragt wurden, wird ebenfalls der Stapel mit den meisten Bauteil genommen
						// Wenn das Ergebnis gleich ist, wird die kleine SPL bevorzugt
						

						
						var validStackCode = CheckValidId(unitOfWork,_Logger,intelliStackResultList);// Konkreter Stapel vom IntelliStack wird zurückgegeben

						
                        //Wird kein ValidStackCode gefunden hat IntelliStack kein Ergebnis geliefert
                        //Dann muss die Stapelanfrage auf den Fehlerstatus gesetzt werden um den Prozess nicht zu blockieren
                        if (string.IsNullOrEmpty(validStackCode))
                        {
                            stack.CustomStackState = StackState.ErrorIntelliStack;
                            unitOfWork.AddOrUpdate(new[]{stack});

                        }
                        else
                        {   
                            var intelliStackPiles = new List<IntelliStackPile>();

                            var intelliStackPileFirst = unitOfWork.GetRepository<IntelliStackPile>().GetFirstOrDefault(x=>x.Code == validStackCode);
                            intelliStackPiles.Add(intelliStackPileFirst);

                            foreach (var intelliStackPile in intelliStackPiles)
                            {
                                var validId = intelliStackPile.IntelliStackCode;
                                
                                _Logger.Info("IntelliStack Code: " + validId);
                                stack.CustomIntelliStackCode = validId;
                                stack.CustomStackState = StackState.StackFinished;
                                
                                stack.ModificationDate = DateTime.Now;
                                stack.ModificationSource = "RequestToIntelliStack";
                                
                                unitOfWork.AddOrUpdate(new[]{stack});
                                
                                // Ergebnis aus IntelliStack in base.Stacks übertragen StackType =1002
                                // Eine Stapelanfrage StackType=1001 kann in mehreren IntelliStacks 1002 enden 
                                // Es wird aber nur 1 Stapel als 1002 in die base.Stacks geschrieben. (Auf Grund der CheckValidId Prüfung)
                                // Alle andere werden nicht übernommen und die Bauteile werden im Sortierer beim nächsten Auslagern selektiert
                                
                                if(intelliStackPile != null)
                                {
                                    var newbaseStack = new HomagGroup.FLS.Domain.Data.Stack();
                                    
                                    newbaseStack.StackCode = _RangeOfNumbersHelper.GetNewUniqueIdentifier(_Logger,"STACKCODES");
                                    newbaseStack.CustomExternalStackCode = stack.CustomExternalStackCode;
                                    newbaseStack.CustomStackType = StackType.StackFromIntelliStack;
                                    newbaseStack.CustomBaseBoardType = _BaseBoard;                            
                                    newbaseStack.PositionNumber = "1";
                                    newbaseStack.IsActive = YesNo.Yes;
                                    newbaseStack.IsReserved = YesNo.Yes;
                                    newbaseStack.IsValid = YesNo.Yes;
                                    newbaseStack.CentreX = 0;
                                    newbaseStack.CentreY = 0;
                                    newbaseStack.CentreZ = 0;
                                    newbaseStack.LayerLayout = "";
                                    newbaseStack.StackLength = 0M;
                                    newbaseStack.StackWidth = 0M;
                                    newbaseStack.StackHeight = 0M;
                                    newbaseStack.CustomStackState = StackState.StackCreated;
                                    newbaseStack.CustomIntelliStackCode = intelliStackPile.Code;
                                    newbaseStack.CustomStackSource = stack.CustomStackSource;
                                    // newbaseStack.CustomClass = 9;
                                    newbaseStack.CustomAllocationCode = stack.CustomAllocationCode;
                                    
                                    newbaseStack.CustomStackStructureCode = stack.CustomStackStructureCode;
                                    newbaseStack.CustomDestination = stack.CustomDestination;
                                    
                                    newbaseStack.CreationSource = "RequestToIntelliStack";

                                    int layerNumber = 1;
                                    decimal layerIntelliStack = 0M; //z.B. 0/16/32/48 für layer 1/2/3
                                    int positionInlayer = 0; 
                                    
                                    foreach(var intelliStackPileItem in  intelliStackPile.IntelliStackPileItems.OrderBy(x=>x.ZPosition))////ZPosition  entspricht layer
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
                                        
                                        newStackItem.StackCode = newbaseStack.StackCode;
                                        newStackItem.LayerNumber = layerNumber;
                                        newStackItem.PositionInLayer = positionInlayer;
                                        newStackItem.StackItemCode = intelliStackPileItem.ProductionItemCode;
                                        newStackItem.StackItemType = StackItemType.ProductionItem;
                                        newStackItem.QuantityInLayer = qtyInlayer;
                                        newStackItem.CustomCoordinateX = GetCoordinate(unitOfWork, intelliStackPileItem,"X");
                                        newStackItem.CustomCoordinateY = GetCoordinate(unitOfWork, intelliStackPileItem,"Y");
                                        
                                        newStackItem.CustomIntelliStackPositionX = intelliStackPileItem.XPosition;
                                        newStackItem.CustomIntelliStackPositionY = intelliStackPileItem.YPosition;
                                        newStackItem.CustomIntelliStackPositionZ = intelliStackPileItem.ZPosition;
                                        
                                        if(intelliStackPileItem.ZRotate != 0)
                                        {
                                            newStackItem.CustomOrientation = 1;
                                        }
                                        else
                                        {
                                            newStackItem.CustomOrientation = 0;
                                        }
                                        
                                        newStackItem.CreationSource = "RequestToIntelliStack";
                                                                        
                                        newbaseStack.StackItems.Add(newStackItem);
                                    }
                                    unitOfWork.AddOrUpdate(new[]{newbaseStack});
                                    unitOfWork.Save();
                                }
                                else
                                {
                                    _Logger.Error(String.Format("Keinen IntelliStack in Tabelle IntelliStackPiles mit IntelliStackCode [{0}] gefunden",validId));
                                }
                            }                        
                        }
									
					}							
					
				}
				_Logger.Info("Ende RequestToIntelliStack");
			}

        }
        catch (Exception e)
        {
            executionContext.JobResultSetComplete(JobResult.Failed);
            _Logger.Error(ResourcesKeys.ErrorInUserExit("RequestToIntelliStack"), null, e);
            throw;
        }
    }
	
	private string CheckValidId(IUnitOfWorkBase unitOfWork, Logger _Logger,List<ListForValidation> intelliStackResultList)
	{		
        var stackwithMaxPartQty = ""; // Es wird der Stapel gemerkt mit der höchsten Bauteil Anzahl
        int maxPartQty = 0;
        
		try
		{	
			_Logger.Info(intelliStackResultList.Count().ToString());
			
			foreach(var listItem in intelliStackResultList.OrderBy(x=>x.BaseBoard)) //Sortiert.Zuerst SPL1, falls vorhanden, dann SPL2
			{
				_Logger.Info(listItem.Id);
				
				var intelliStacks = unitOfWork.GetRepository<IntelliStackPile>().Get(x=>x.IntelliStackCode == listItem.Id);

				foreach(var stack in intelliStacks)//Der Stapel mit den meisten Bauteilen wird gemerkt
				{
					if(maxPartQty < stack.IntelliStackPileItems.Count())
					{
						maxPartQty = stack.IntelliStackPileItems.Count();
						stackwithMaxPartQty = stack.Code;
						_BaseBoard = listItem.BaseBoard; //Merken, welche SPL verwendet wird für den ausgewählten Stapel
					}
				}
			}
		
		}
		catch(Exception e)
		{
			_Logger.Error("Fehler beim Bewerten der Bauteilmengen der IntelliStacks", null,e);
			return null;
		}
		return stackwithMaxPartQty;
	}

	
    private decimal GetCoordinate(IUnitOfWork unitOfWork, IntelliStackPileItem intelliStackPileItem, string axis)
    {        
        decimal length = 0M;
        decimal width = 0M;
        
        var baseBoard = unitOfWork.GetRepository<CustBaseBoard>().GetFirstOrDefault(x=>x.BaseBoardCode == _BaseBoard);
        
        decimal boardLength = baseBoard.Length;
        decimal boardWidth = baseBoard.Width;
        
        
        _Logger.Info(String.Format("BoardLength[{0}] und BoardWidth[{1}]",boardLength,boardWidth));
        
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
        _Logger.Info(String.Format("Item[{0}]Physikalische Länge[{1}] und physikalische Breite[{2}]",intelliStackPileItem.ProductionItemCode,length,width));
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
        _Logger.Info(String.Format("Item[{0}]Bauteil Mittelpunkt X-Achse[{1}] und Mittelpunkt Y-Achse [{2}]",intelliStackPileItem.ProductionItemCode,intelliX,intelliY));
        
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
    
    // Die Funktion wird nur im Fehlerfall benötigt, falls zuvor die Stammdaten nicht gefunden werden konnten
    private StackSettings SetDefaultSettings(IUnitOfWorkBase unitOfWork, Logger _Logger, string board,string stackCode)
	{
		try
		{
		  var settings = new StackSettings();
		  // Das Setting für IntelliStack wird mit festen Werten hinterlegt und ist nur für den Fehlerfall.
			
			var baseBoard = unitOfWork.GetRepository<CustBaseBoard>().GetFirstOrDefault(x=>x.BaseBoardCode == board);
			
			if(baseBoard != null)
			{				
				settings.Name  =  "Stapel_" + stackCode + "_" + _RangeOfNumbersHelper.GetNewUniqueIdentifier(_Logger,"INTELLISTACK") + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
						
				settings.PalletLength = (double)baseBoard.Length;
				settings.OverlapXPlus = 70 ;
				settings.OverlapMaxXPlus = 500 ;

				settings.PalletWidth = (double)baseBoard.Width;
				settings.MaxStackHeight = 730;
				settings.AccessGroupFeature = "1";
				settings.OverlapXMinus = 0;
				
				settings.OverlapYMinus = 0;
				settings.OverlapYPlus = 70;
				settings.OverlapMaxXMinus = 0;
				
				settings.OverlapMaxYMinus = 0;
				settings.OverlapMaxYPlus  = 70;
				settings.DistanceBetweenParts = 20;

				settings.AlternatingOffsetX = 0;
				settings.AlternatingOffsetY = 0;

				settings.Alignment = IntelliStackAlignment.Default;

				settings.StackReversable = false;

				_Logger.Info("P7");
			}
			return settings;
		}
		catch(Exception e)
		{
			_Logger.Error("Fehler beim Ermitteln der Default Stammdaten", null,e );
			return null;
		}
					
	}
	
	
	
    private StackSettings SetValuesForSetting(IUnitOfWorkBase unitOfWork,List<PartToStack> listOfPartsToStack ,Logger _Logger,string baseBoard, string stackCode)
    {
        try
        {            
            _Logger.Info("Start SetValuesForSetting");
			
			//Ermittlung StackStructureCode
            string part = listOfPartsToStack.FirstOrDefault().Id;  
			var prodItem = unitOfWork.GetRepository<ProductionItem>().GetFirstOrDefault(x=>x.Code == part);
			var prodOrderStackStructureCode = prodItem.ProductionOrder.CustomStackStructureCode ?? 99;
			var stackStructure = unitOfWork.GetRepository<CustStackStructure>().GetFirstOrDefault(x=>x.StackStructureCode == prodOrderStackStructureCode);
			
			_Logger.Info(String.Format("CustomStackStructureCode [{0}]",prodOrderStackStructureCode));
            _Logger.Info("Prüfen, ob es sich um eine manuelle Auslagerung handelt");
			
			// Falls es eine Manuelle Auslagerung war, dann muss es ein chaotischer Stapel sein
            var sorterRequestItem = unitOfWork.GetRepository<CustSorterRequestItem>().Get(x=>x.ProductionItemCode == prodItem.Code &&  x.TransferState == 20).OrderByDescending(x=>x.Sequence).FirstOrDefault();
            if (sorterRequestItem != null)
            {
                if (sorterRequestItem.RuleCode == "MANUAL")
                {
                    stackStructure = unitOfWork.GetRepository<CustStackStructure>().GetFirstOrDefault(x=>x.StackStructureCode == 0);
                    _Logger.Info("RuleCode MANUAL. StackStructureCode wird mit 0 überschrieben");
                }
            }
            else
            {
                _Logger.Info("Keine manuelle Auslagerung. StackStructureCode bleibt gültig");
            }            
            
			var settings = new StackSettings();
			
            if(stackStructure != null)
            {
                _Logger.Info(String.Format("Eintrag Stammdaten für StackStructureCode [{0}] gefunden", stackStructure.StackStructureCode));                
                    
                var baseboard = unitOfWork.GetRepository<CustBaseBoard>().GetFirstOrDefault(x=>x.BaseBoardCode == baseBoard);
                				                
                if(baseboard != null)
                {
                    _BoardLength = baseboard.Length; // werte werden später für die Berechnung in Funktion "GetCoordinate" benötigt
                    _BoardWidth = baseboard.Width;
                    
                    settings.Name  = "Stapel_" + stackCode + "_" + _RangeOfNumbersHelper.GetNewUniqueIdentifier(_Logger,"INTELLISTACK") + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    settings.DebugResponseFolder = @"D:\MESDataArchive\IntelliStack";
                    _Logger.Info("Setting.Name" + settings.Name);
					
                    //Bei Serienteilen wird der Kippfaktor aus den ProgramSettings bestimmt. Bei Komm. Teilen wird nichts gesetzt
                    if (prodItem.ProductionOrder.CustomOrderType == "L")
                    {
                        var programSetting = unitOfWork.GetRepository<ProgramSetting>().GetFirstOrDefault(x=>x.Identifier == "IntelliStack_Turret_Factor");
                        
                        settings.WithTurretConfiguration = true;
                        settings.MaxTurretHeightA = 0;
                        settings.MaxTurretHeightB = (double)programSetting.ValueFloat;
						
						_Logger.Info("Serienauftrag. Kippfaktor " + settings.MaxTurretHeightB);
                    }


                    settings.PalletLength = (double)baseboard.Length;
					settings.PalletWidth = (double)baseboard.Width;
					
					// Unterschiedliche Werte für OverlapX... für SPL2/SPL1
                    if (baseboard.BaseBoardCode == "SPL2")
                    {
                        settings.OverlapXPlus = (double)stackStructure.OverlapXPlus2;
                        settings.OverlapXMinus = (double)stackStructure.OverlapXMinus2;

                        settings.OverlapMaxXMinus = (double)stackStructure.OverlapMaxXMinus2;
                        settings.OverlapMaxXPlus = (double)stackStructure.OverlapMaxXPlus2;
                    }
                    else
                    {
                        settings.OverlapXPlus = (double)stackStructure.OverlapXPlus;
                        settings.OverlapXMinus = (double)stackStructure.OverlapXMinus;

                        settings.OverlapMaxXMinus = (double)stackStructure.OverlapMaxXMinus;
                        settings.OverlapMaxXPlus = (double)stackStructure.OverlapMaxXPlus;
                    }                    
                    
                    settings.MaxStackHeight = (double)stackStructure.MaxStackHeight;
                    settings.AccessGroupFeature = "1";

                    settings.OverlapYMinus = (double)stackStructure.OverlapYMinus;
                    settings.OverlapYPlus = (double)stackStructure.OverlapYPlus;
                    settings.OverlapMaxYMinus = (double)stackStructure.OverlapMaxYMinus;
                    settings.OverlapMaxYPlus  = (double)stackStructure.OverlapMaxYPlus;


                    settings.DistanceBetweenParts = (double)stackStructure.DistanceBetweenParts;
                    
                    //Lagenversatz
                    if (stackStructure.UseAlternatingOffset == 1)
                    {
                        settings.UseAlternatingOffset = true;
                        if (stackStructure.AlternatingOffsetX != null && stackStructure.AlternatingOffsetX > 0)
                        {
                            settings.AlternatingOffsetX = (double)stackStructure.AlternatingOffsetX;
                        }
                        if (stackStructure.AlternatingOffsetY != null && stackStructure.AlternatingOffsetY > 0)
                        {
                            settings.AlternatingOffsetY = (double)stackStructure.AlternatingOffsetY;
                        }
                    }
                    
                    //LaneLimits
                    if (stackStructure.UseLaneLimits == 1)
                    {
                        settings.UseLaneLimits = true;
                        if (stackStructure.LaneHeightMin != null && stackStructure.LaneHeightMin > 0)
                        {
                            settings.LaneHeightMin = (double)stackStructure.LaneHeightMin;
                        }
                        settings.UseLaneLimits = true;
                        if (stackStructure.LaneHeightMax != null && stackStructure.LaneHeightMax > 0)
                        {
                            settings.LaneHeightMax = (double)stackStructure.LaneHeightMax;
                        }
                        settings.UseLaneLimits = true;
                        if (stackStructure.LaneHeightTolerance != null && stackStructure.LaneHeightTolerance > 0)
                        {
                            settings.LaneHeightTolerance = (double)stackStructure.LaneHeightTolerance;
                        }
                    }
					
					// Eckenausrichtung des Stapels/chatotischer Stapel
				    if(stackStructure.IntelliStackAlignment == AlignmentForIntelliStack.Default)/*chaotisher Stapel [0]*/
					{
						settings.Alignment = IntelliStackAlignment.Default;
					}
					else if (stackStructure.IntelliStackAlignment == AlignmentForIntelliStack.CorneredSouthWest)/*[2]*/
					{
						settings.Alignment = IntelliStackAlignment.CorneredSouthWest;
					}
					else if (stackStructure.IntelliStackAlignment == AlignmentForIntelliStack.CorneredSouthEast)/*[3]*/
					{
						settings.Alignment = IntelliStackAlignment.CorneredSouthEast;
					}
					else if (stackStructure.IntelliStackAlignment == AlignmentForIntelliStack.CorneredNorthEast)/*[4]*/
					{
						settings.Alignment = IntelliStackAlignment.CorneredNorthEast;
					}
					else if (stackStructure.IntelliStackAlignment == AlignmentForIntelliStack.CorneredNorthWest)/*[5]*/
					{
						settings.Alignment = IntelliStackAlignment.CorneredNorthWest;
					}
					else if (stackStructure.IntelliStackAlignment == AlignmentForIntelliStack.Centric)/*[6]*/
					{
						settings.Alignment = IntelliStackAlignment.Centric;
					}
					
					_Logger.Info("Alignment: " + settings.Alignment);
					
					// In Maske wird Wert 0 angegeben. IntelliStack erwartet Wert NULL
					if(stackStructure.PartsInXDirection == 0 )
					{
						settings.PartsInXDirection = null;
						_Logger.Info("PartsInXDirection Wert 0 wird als NULL gewertet");
					}
					else
					{
						settings.PartsInXDirection = stackStructure.PartsInXDirection;
						_Logger.Info("PartsInXDirection = " + stackStructure.PartsInXDirection);
						
					}
					//
					if(stackStructure.PartsInYDirection == 0 )
					{
						settings.PartsInYDirection = null;
						_Logger.Info("PartsInYDirection Wert 0 wird als NULL gewertet");
					}
					else
					{
						settings.PartsInYDirection = stackStructure.PartsInYDirection;
						_Logger.Info("PartsInYDirection = " + stackStructure.PartsInYDirection);
					}
					
					_Logger.Info(String.Format("Setze PartsInXDirection [{0}] ", settings.PartsInXDirection));
					_Logger.Info(String.Format("Setze PartsInYDirection [{0}] ", settings.PartsInYDirection));
                                        
                    
                    // Lumpensammler, alles was manuell ausgelagert wird soll immer Default sein unabhängig von den Stammdaten
                    sorterRequestItem = unitOfWork.GetRepository<CustSorterRequestItem>().GetFirstOrDefault(x=>x.ProductionItemCode == prodItem.Code &&  x.TransferState == 20);
                    if (sorterRequestItem != null)
                    {
                        if (sorterRequestItem.RuleCode == "MANUAL")
                        {
                            settings.Alignment = IntelliStackAlignment.Default;
                        }
                    }
                    
                    // Stapel in der Reihenfolge umkehrbar
                    if(stackStructure.StackReversable == 1)
                    {
                        settings.StackReversable = true;
                    }
                    else
                    {
                        settings.StackReversable = false;
                    }
                }
            }
            else
            {                
				_Logger.Error("Fehler beim Ermitteln der Stammdaten für StackStructureCode" + prodOrderStackStructureCode);
            }
            return settings;        
        
        }
        catch(Exception e)
        {
            _Logger.Error("Fehler beim Ermitteln der StackSettings",null,e);
            
            return null;
        }
    }
    
    // DimX = physikalische Länge
    // DimY physikalische Breite
    private double GetDimension(ProductionItem prodItem, string dimension, Logger _Logger)
    {
        try
        {          
            if(dimension == "X")
            {
                if(prodItem.ProductionOrder.CuttingLength >= prodItem.ProductionOrder.CuttingWidth)
                {
                    return (double)prodItem.ProductionOrder.CuttingLength;
                }
                else
                {
                    return (double)prodItem.ProductionOrder.CuttingWidth;
                }
            }
            else // dimension == "Y"
            {
                if(prodItem.ProductionOrder.CuttingLength >= prodItem.ProductionOrder.CuttingWidth)
                {
                   return (double)prodItem.ProductionOrder.CuttingWidth; 
                }
                else
                {
                    return (double)prodItem.ProductionOrder.CuttingLength;
                }        
            
            }
        }
        catch(Exception e)
        {
            _Logger.Error("Fehler beim Ermitteln der Maße für ProdItem:" + prodItem.Code,null, e);
            return 0;
        }
        
    
    }
    
    private double GetThickness(ProductionItem prodItem, IUnitOfWorkBase unitOfWork, Logger _Logger)
    {
        try
        {        
            var material = unitOfWork.GetRepository<HomagGroup.FLS.Domain.Data.Material>().GetFirstOrDefault(x=>x.Code == prodItem.ProductionOrder.Material);
            
            double thickness = 0;
            if(material != null && material.CustomThicknessClassified != null && material.CustomThicknessClassified > 0)
            {
                 thickness = (double)material.CustomThicknessClassified;
            }
            else
            {
                 thickness = (double)prodItem.ProductionOrder.Thickness;
            }
            
            return thickness;
        }
        catch(Exception e)
        {
            _Logger.Error("Fehler beim Ermitteln der Stärke", null, e);
            return 0;
        }
    }
    
    

	public class ListForValidation
	{
		public string Id { get; set; }
		public string BaseBoard { get; set; }
	}
	
	
	
    public override ICollection<UserExitParameter> UserExitInputParameters
    {
        get
        {
            return new List<UserExitParameter>
            {
                // Example for new parameter:
                // new UserExitParameter("MyParameter", typeof(string), true)
            };
        }
    }
}
