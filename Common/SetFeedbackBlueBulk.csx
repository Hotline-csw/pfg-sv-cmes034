#r ControllerMES.Infrastructure.Common
#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   SetFeedbackBlueBulk
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         T.Stürzer
//   Date:           2023-02-12
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   T.Stürzer       2023-02-12    Created
//   T.Stürzer       2023-03-03    Added Condition "NoReproduction"
//   T.Stürzer       2024-08-08    Changed Feedback from InsertFeedbackFinishedGood to InsertFeedback
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IServerCustomization))]
[Export("SetFeedbackBlueBulk", typeof(HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("[08] Set feedback for Bulk-Blue")]
[EnabledScript(true)]
public class SetFeedbackBlueBulk : GenericTaskBase, HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;
    
    [Import]
    protected IDistributedServiceProvider DistributedServiceProvider { get; private set; }

    private Logger _Logger;
    
    private string _TaskName = "SetFeedbackBlueBulk";
    
    // WorkCenterCodes
    private string cuttingWorkCenterCode = "1010";
    private string edgeWorkCenterCode = "3010";
    private string drillingWorkCenterCode = "5010";
    private string cncWorkCenterCode = "5020";
    private string sortingWorkCenterCode = "5070";
    private string preassemblyWorkCenterCode = "6010";
    private string assemblyWorkCenterCode = "6010";
    
    // ProductionStepCodes
    private string cuttingStepCode = "B300";
    private string edgeStepCode = "KALWZ14";
    private string drillingStepCode = "V200";
    private string cncStepCode = "E310";
    private string sortingStepCode = "SORT";
    private string preassemblyStepCode = "PREASSEM";
    private string assemblyStepCode = "ASSEM";
    
    // Small Kitchens
    private string dks003 = "Demo_Kitchen_Small_003";
    private string dks004 = "Demo_Kitchen_Small_004";
    private string dks005 = "Demo_Kitchen_Small_005";
    
    // Medium Kitchens
    private string dkm001 = "Demo_Kitchen_Medium_001";
    

    public override void Execute(IJobExecutionContext executionContext)
    {
        Guard.ThrowOnArgumentNull(executionContext, "executionContext");

        try
        {
            _Logger = LogHelper.GetLogger(executionContext.Area, typeof(SetFeedbackBlueBulk));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);


//-----------------------------------------------------------------------------
// ComponentTypes available for Demo_Kitchen_Small and Demo_Kitchen_Medium
//  0 = undefinied, sale items and assembly units, RouteCode: ASSEM and BG
//
//  ComponentType 			Parts type (MES)        Item designation(MES)			SortCrit       RouteCode 
//
//  1 = SidePanel			Side panel              Side panel left/right           Carcase        B300_KALWZ14_V200_SORT_PREASSEM
//  2 = AdjustableShelf     Adjustable shelf        Adjustable shelf                Carcase        B300_KALWZ14_V200_SORT_PREASSEM
//  3 = TopShelf			Top shelf               Top shelf                       Carcase        B300_KALWZ14_V200_SORT_PREASSEM
//  4 = BottomShelf         Bottom shelf            Bottom shelf                    Carcase        B300_KALWZ14_V200_SORT_PREASSEM
//  5 = BackPanel           Back panel              Back panel                      Carcase        B300_V200_SORT_PREASSEM
//  6 = Panel               Board                   Filler                          Front          B300_KALWZ14_E310_SORT_PREASSEM
//  7 = Door                door                    Flap                            Front          B300_KALWZ14_E310_SORT_PREASSEM
//  8 = DoorLeft            Left-hand door          Door left                       Front          B300_KALWZ14_E310_SORT_PREASSEM
//  9 = DoorRight           Right-hand door         Door right                      Front          B300_KALWZ14_E310_SORT_PREASSEM
// 12 = FixedShelf          Shelf                   Fixed Shelf                     Carcase        B300_KALWZ14_V200_SORT_PREASSEM
// 13 = Partition           Partition               Partition                       Carcase        B300_KALWZ14_V200_SORT_PREASSEM
// 17 = DrawerBottom        Drawer bottom           Drawer bottom                   Drawer         B300_E310_SORT_PREASSEM
// 18 = DrawerSide          Drawer side             Drawer front/back/side panel    Drawer         B300_KALWZ14_E310_SORT_PREASSEM
// 19 = DrawerFront         Drawer duplicate        Drawer front (panel)            Drawer         B300_KALWZ14_E310_SORT_PREASSEM
// 20 = Plinth              Toekick                 Toekick                         Carcase        B300_KALWZ14_V200_SORT_PREASSEM
// 21 = WorkTop             Countertop              Worktop                         Carcase        B300_KALWZ14_V200_SORT_PREASSEM
// 24 = Traverse            Rail                    Traverse                        Carcase        B300_KALWZ14_V200_SORT_PREASSEM
//-----------------------------------------------------------------------------

            using (var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
            {
                var prodOrdersRep = unitOfWork.GetRepository<ProductionOrder>();
                var prodItemsRep = unitOfWork.GetRepository<ProductionItem>();
                var prodItemsStepsDataRep = unitOfWork.GetRepository<ProductionItemsStepsData>(); 

                
                // Demo_Kitchen_Small_003
				// Preassembly
                var validPositions = new List<string> { "001", "002", "003", "004", "005", "006", "007" };

                var preassemDKS003 = prodOrdersRep.Get(
                    po => po.CustomerOrderCode == dks003 && 
                          validPositions.Contains(po.CustomerOrderPosition) && 
                          po.OrderType == ProductionOrderType.ConstructionPart);
            
                if(preassemDKS003.Any())
                {   
                    foreach(var preassem003 in preassemDKS003)
                    {           
                        var prodItemPreassem003 = prodItemsRep.GetFirstOrDefault(pi => pi.ProductionOrderCode == preassem003.Code);
                
                        var prodItemsStepsDataPreassem003 = prodItemsStepsDataRep.GetFirstOrDefault(
                                pisd => pisd.ProductionOrderCode == preassem003.Code && pisd.ProductionStepCode == preassemblyStepCode);
                
                        if(prodItemPreassem003 != null && prodItemsStepsDataPreassem003 != null)
                        {
                            prodItemPreassem003.InsertFeedback(unitOfWork, _TaskName, 1, 0, 0, preassemblyWorkCenterCode, "PREASSEM", 0, FeedbackState.Finished, 0, _Logger);
                        }
                    }
                }
				
                // Assembly
                var assemDKS003 = prodOrdersRep.Get(
                    po => po.CustomerOrderCode == dks003 && 
                          validPositions.Contains(po.CustomerOrderPosition) && 
                          po.OrderType == ProductionOrderType.SalesArticle);

                    if(assemDKS003.Any())
                    {   
                        foreach(var assem003 in assemDKS003)
                        {           
                            var prodItemAssem003 = prodItemsRep.GetFirstOrDefault(pi => pi.ProductionOrderCode == assem003.Code);
                            
                            var prodItemsStepsDataAssem003 = prodItemsStepsDataRep.GetFirstOrDefault(
                                    pisd => pisd.ProductionOrderCode == assem003.Code && pisd.ProductionStepCode == assemblyStepCode);
                            
                            if(prodItemAssem003 != null && prodItemsStepsDataAssem003 != null)
                            {
                                prodItemAssem003.InsertFeedback(unitOfWork, _TaskName, 1, 0, 0, assemblyWorkCenterCode, "ASSEM", 0, FeedbackState.Finished, 0, _Logger);
                            }
                        }
                    }
                    
//-----------------------------------------------------------------------------    
    
                // Demo_Kitchen_Small_004
                // Sorting
                var componentTypes = new List<ComponentType> {
                    ComponentType.AdjustableShelf, ComponentType.DoorLeft, ComponentType.FixedShelf, ComponentType.Partition, 
                    ComponentType.DrawerBottom, ComponentType.DrawerSide, ComponentType.DrawerFront, ComponentType.Plinth, ComponentType.WorkTop, 
                    ComponentType.Traverse, ComponentType.BackPanel, ComponentType.BottomShelf, ComponentType.TopShelf};
                
                var sortDKS004 = prodOrdersRep.Get(
                    po => po.CustomerOrderCode == dks004 && componentTypes.Contains(po.ComponentType));
                
                if(sortDKS004.Any())
                {   
                    foreach(var sort004 in sortDKS004)
                    {           
                        var prodItemSort004 = prodItemsRep.GetFirstOrDefault(pi => pi.ProductionOrderCode == sort004.Code);
                
                        var prodItemsStepsDataSort004 = prodItemsStepsDataRep.GetFirstOrDefault(
                            pisd => pisd.ProductionOrderCode == sort004.Code && pisd.ProductionStepCode == sortingStepCode);
                
                        if(prodItemSort004 != null && prodItemsStepsDataSort004 != null)
                        {
                            prodItemSort004.InsertFeedback(unitOfWork, _TaskName, 1, 0, 0, sortingWorkCenterCode, "SORT", 0, FeedbackState.Finished, 0, _Logger);
                        }
                    }
                }
/*
                var sortDKS004 = prodOrdersRep.Get(
						po => po.CustomerOrderCode == dks004 && po.ComponentType == ComponentType.AdjustableShelf ||
							  po.CustomerOrderCode == dks004 && po.ComponentType == ComponentType.DoorLeft ||
							  po.CustomerOrderCode == dks004 && po.ComponentType == ComponentType.FixedShelf ||
							  po.CustomerOrderCode == dks004 && po.ComponentType == ComponentType.Partition ||
							  po.CustomerOrderCode == dks004 && po.ComponentType == ComponentType.DrawerBottom ||
							  po.CustomerOrderCode == dks004 && po.ComponentType == ComponentType.DrawerSide ||
							  po.CustomerOrderCode == dks004 && po.ComponentType == ComponentType.DrawerFront ||
							  po.CustomerOrderCode == dks004 && po.ComponentType == ComponentType.Plinth ||
							  po.CustomerOrderCode == dks004 && po.ComponentType == ComponentType.WorkTop ||
							  po.CustomerOrderCode == dks004 && po.ComponentType == ComponentType.Traverse ||
							  po.CustomerOrderCode == dks004 && po.ComponentType == ComponentType.BackPanel ||
							  po.CustomerOrderCode == dks004 && po.ComponentType == ComponentType.BottomShelf ||
                              po.CustomerOrderCode == dks004 && po.ComponentType == ComponentType.TopShelf);
                                                        
                if(sortDKS004 != null)
                {   
                    foreach(var sort004 in sortDKS004)
                    {           
                        var prodItemSort004 = prodItemsRep.GetFirstOrDefault(pi => pi.ProductionOrderCode == sort004.Code);
                        
                        if(prodItemSort004 != null)
                        {
                            var prodItemsStepsDataSort004 = prodItemsStepsDataRep.GetFirstOrDefault(
                                pisd => pisd.ProductionOrderCode == sort004.Code && pisd.ProductionStepCode == sortingStepCode);
                            
                            if(prodItemsStepsDataSort004!=null)
                            {
                                prodItemSort004.InsertFeedback(unitOfWork, _TaskName, 1, 0, 0, sortingWorkCenterCode, "SORT", 0, FeedbackState.Finished, 0, _Logger);
                            }
                        }
                    }
                }
*/
                
                // Preassembly 
                var preassemDKS004 = prodOrdersRep.Get(
						po => po.CustomerOrderCode == dks004 && po.ComponentType == ComponentType.Door && po.OrderType == ProductionOrderType.ConstructionPart ||
							  po.CustomerOrderCode == dks004 && po.ComponentType == ComponentType.SidePanel && po.OrderType == ProductionOrderType.ConstructionPart ||
							  po.CustomerOrderCode == dks004 && po.ComponentType == ComponentType.DoorRight && po.OrderType == ProductionOrderType.ConstructionPart);
                                                        
                if(preassemDKS004 != null)
                {   
                    foreach(var preassem004 in preassemDKS004)
                    {           
                        var prodItemPreassepisd = prodItemsRep.GetFirstOrDefault(pi => pi.ProductionOrderCode == preassem004.Code);
                        
                        if(prodItemPreassepisd != null)
                        {
                            var prodItemsStepsDataPreassepisd = prodItemsStepsDataRep.GetFirstOrDefault(
                                pisd => pisd.ProductionOrderCode == preassem004.Code && pisd.ProductionStepCode == preassemblyStepCode);
                            
                            if(prodItemsStepsDataPreassepisd != null)
                            {
                                prodItemPreassepisd.InsertFeedback(unitOfWork, _TaskName, 1, 0, 0, preassemblyWorkCenterCode, "PREASSEM", 0, FeedbackState.Finished, 0, _Logger);

                            }
                        }
                    }
                }
   
                // Assembly Sale Item            
                var assemDKS004 = prodOrdersRep.Get(
						po => po.CustomerOrderCode == dks004 && po.CustomerOrderPosition == "001" && po.OrderType == ProductionOrderType.SalesArticle ||
							  po.CustomerOrderCode == dks004 && po.CustomerOrderPosition == "003" && po.OrderType == ProductionOrderType.SalesArticle ||
							  po.CustomerOrderCode == dks004 && po.CustomerOrderPosition == "006" && po.OrderType == ProductionOrderType.SalesArticle);
                                                        
                if(assemDKS004 != null)
                {   
                    foreach(var assem004 in assemDKS004)
                    {           
                        var prodItemAssem004 = prodItemsRep.GetFirstOrDefault(pi => pi.ProductionOrderCode == assem004.Code);
                        
                        if(prodItemAssem004 != null)
                        {
                            var prodItemsStepsDataAssem004 = prodItemsStepsDataRep.GetFirstOrDefault(
                                pisd => pisd.ProductionOrderCode == assem004.Code && pisd.ProductionStepCode == assemblyStepCode);
                            
                            if(prodItemsStepsDataAssem004 != null)
                            {
                                prodItemAssem004.InsertFeedback(unitOfWork, _TaskName, 1, 0, 0, assemblyWorkCenterCode, "ASSEM", 0, FeedbackState.Finished, 0, _Logger);
                            }
                        }
                    }
                }
				
//-----------------------------------------------------------------------------

                // Demo_Kitchen_Small_005
                // CNC 310
                var cncDKS005 = prodOrdersRep.Get(
						po => po.CustomerOrderCode == dks005 && po.ComponentType == ComponentType.DrawerFront);
                                                        
                if(cncDKS005 != null)
                {   
                    foreach(var cnc005 in cncDKS005)
                    {           
                        var prodItemCnc005 = prodItemsRep.GetFirstOrDefault(
                            pi => pi.ProductionOrderCode == cnc005.Code);
                        
                        if(prodItemCnc005 != null)
                        {
                            var prodItemsStepsDataCnc005 = prodItemsStepsDataRep.GetFirstOrDefault(
                                pisd => pisd.ProductionOrderCode == cnc005.Code && pisd.ProductionStepCode == cncStepCode);
                            
                            if(prodItemsStepsDataCnc005 != null)
                            {
                                prodItemCnc005.InsertFeedback(unitOfWork, _TaskName, 1, 0, 0, cncWorkCenterCode, "E310", 0, FeedbackState.Finished, 0, _Logger);
                            }
                        }
                    }
                }
                
                // Sorting
                var sortDKS005 = prodOrdersRep.Get(
						po => po.CustomerOrderCode == dks005 && po.ComponentType == ComponentType.Panel ||
							  po.CustomerOrderCode == dks005 && po.ComponentType == ComponentType.DoorLeft ||
							  po.CustomerOrderCode == dks005 && po.ComponentType == ComponentType.DoorRight ||
							  po.CustomerOrderCode == dks005 && po.ComponentType == ComponentType.Partition ||
							  po.CustomerOrderCode == dks005 && po.ComponentType == ComponentType.DrawerSide ||
							  po.CustomerOrderCode == dks005 && po.ComponentType == ComponentType.WorkTop ||
						      po.CustomerOrderCode == dks005 && po.ComponentType == ComponentType.DrawerFront ||
						      po.CustomerOrderCode == dks005 && po.ComponentType == ComponentType.SidePanel && po.ReproductionType == ReproductionType.NoReproduction ||
						      po.CustomerOrderCode == dks005 && po.ComponentType == ComponentType.Door);
                                                        
                if(sortDKS005 != null)
                {   
                    foreach(var sort005 in sortDKS005)
                    {           
                        var prodItemSort005 = prodItemsRep.GetFirstOrDefault(pi => pi.ProductionOrderCode == sort005.Code);
                        
                        if(prodItemSort005 != null)
                        {
                            var prodItemsStepsDataSort005 = prodItemsStepsDataRep.GetFirstOrDefault(
                                pisd => pisd.ProductionOrderCode == sort005.Code && pisd.ProductionStepCode == sortingStepCode);
                            
                            if(prodItemsStepsDataSort005!=null)
                            {
                                prodItemSort005.InsertFeedback(unitOfWork, _TaskName, 1, 0, 0, sortingWorkCenterCode, "SORT", 0, FeedbackState.Finished, 0, _Logger);
                            }
                        }
                    }
                }
                
                // Preassembly 
                var preassemDKS005 = prodOrdersRep.Get(
                    po => po.CustomerOrderCode == dks005 && po.ComponentType == ComponentType.AdjustableShelf && po.OrderType == ProductionOrderType.ConstructionPart ||
                            po.CustomerOrderCode == dks005 && po.ComponentType == ComponentType.TopShelf && po.OrderType == ProductionOrderType.ConstructionPart ||
                            po.CustomerOrderCode == dks005 && po.ComponentType == ComponentType.BackPanel && po.OrderType == ProductionOrderType.ConstructionPart ||
                            po.CustomerOrderCode == dks005 && po.ComponentType == ComponentType.FixedShelf && po.OrderType == ProductionOrderType.ConstructionPart ||
                            po.CustomerOrderCode == dks005 && po.ComponentType == ComponentType.DrawerBottom && po.OrderType == ProductionOrderType.ConstructionPart ||
                            po.CustomerOrderCode == dks005 && po.ComponentType == ComponentType.Plinth && po.OrderType == ProductionOrderType.ConstructionPart ||
                            po.CustomerOrderCode == dks005 && po.ComponentType == ComponentType.BottomShelf && po.OrderType == ProductionOrderType.ConstructionPart ||
                            po.CustomerOrderCode == dks005 && po.ComponentType == ComponentType.Traverse && po.OrderType == ProductionOrderType.ConstructionPart);
                                                        
                if(preassemDKS005 != null)
                {   
                    foreach(var preassem005 in preassemDKS005)
                    {           
                        var prodItemPreassem005 = prodItemsRep.GetFirstOrDefault(pi => pi.ProductionOrderCode == preassem005.Code);
                        
                        if(prodItemPreassem005 != null)
                        {
                            var prodItemsStepsDataPreassem005 = prodItemsStepsDataRep.GetFirstOrDefault(
                                pisd => pisd.ProductionOrderCode == preassem005.Code && pisd.ProductionStepCode == preassemblyStepCode);
                            
                            if(prodItemsStepsDataPreassem005 != null)
                            {
                                prodItemPreassem005.InsertFeedback(unitOfWork, _TaskName, 1, 0, 0, preassemblyWorkCenterCode, "PREASSEM", 0, FeedbackState.Finished, 0, _Logger);
                            }
                        }
                    }
                }
//-----------------------------------------------------------------------------
                
                // Demo_Kitchen_Medium_001
                // Cutting B300
                var cutDKM001 = prodOrdersRep.Get(
                        po => po.CustomerOrderCode == dkm001 && po.ComponentType == ComponentType.TopShelf && po.ReproductionType == ReproductionType.Standard);
                                                        
                if(cutDKM001 != null)
                {   
                    foreach(var cut001 in cutDKM001)
                    {           
                        var prodItemCut001 = prodItemsRep.GetFirstOrDefault(pi => pi.ProductionOrderCode == cut001.Code);
                        
                        if(prodItemCut001 != null)
                        {
                            var prodItemsStepsDataCut001 = prodItemsStepsDataRep.GetFirstOrDefault(
                                pisd => pisd.ProductionOrderCode == cut001.Code && pisd.ProductionStepCode == cuttingStepCode);
                            
                            if(prodItemsStepsDataCut001 != null)
                            {
                                prodItemCut001.InsertFeedback(unitOfWork, _TaskName, 1, 0, 0, cuttingWorkCenterCode, "B300", 0, FeedbackState.Finished, 0, _Logger);
                            }
                        }
                    }
                }
                
                // Drilling V200
                var drillDKM001 = prodOrdersRep.Get(
                    po => po.CustomerOrderCode == dkm001 && po.ComponentType == ComponentType.BottomShelf ||
						  po.CustomerOrderCode == dkm001 && po.ComponentType == ComponentType.BackPanel ||
                          po.CustomerOrderCode == dkm001 && po.ComponentType == ComponentType.AdjustableShelf ||
                          po.CustomerOrderCode == dkm001 && po.ComponentType == ComponentType.Plinth ||
                          po.CustomerOrderCode == dkm001 && po.ComponentType == ComponentType.Traverse ||
                          po.CustomerOrderCode == dkm001 && po.ComponentType == ComponentType.Panel);
                                                        
                if(drillDKM001 != null)
                {   
                    foreach(var drill001 in drillDKM001)
                    {           
                        var prodItemDrill001 = prodItemsRep.GetFirstOrDefault(
                            pi => pi.ProductionOrderCode == drill001.Code);
                        
                        if(prodItemDrill001 != null)
                        {
                            var prodItemsStepsDataDrill001 = prodItemsStepsDataRep.GetFirstOrDefault(
                                pisd => pisd.ProductionOrderCode == drill001.Code && pisd.ProductionStepCode == drillingStepCode);
                            
                            if(prodItemsStepsDataDrill001 != null)
                            {
                                prodItemDrill001.InsertFeedback(unitOfWork, _TaskName, 1, 0, 0, drillingWorkCenterCode, "V200", 0, FeedbackState.Finished, 0, _Logger);
                            }
                        }
                    }
                }
				
				// CNC 310
                var cncDKM001 = prodOrdersRep.Get(
                    po => po.CustomerOrderCode == dkm001 && po.ComponentType == ComponentType.Door ||
                            po.CustomerOrderCode == dkm001 && po.ComponentType == ComponentType.DrawerFront && po.ReproductionType == ReproductionType.Standard);
                                                        
                if(cncDKM001 != null)
                {   
                    foreach(var cnc001 in cncDKM001)
                    {           
                        var prodItemCnc001 = prodItemsRep.GetFirstOrDefault(
                            pi => pi.ProductionOrderCode == cnc001.Code);
                        
                        if(prodItemCnc001 != null)
                        {
                            var prodItemsStepsDataCnc001 = prodItemsStepsDataRep.GetFirstOrDefault(
                                pisd => pisd.ProductionOrderCode == cnc001.Code && pisd.ProductionStepCode == cncStepCode);
                            
                            if(prodItemsStepsDataCnc001 != null)
                            {
                                prodItemCnc001.InsertFeedback(unitOfWork, _TaskName, 1, 0, 0, cncWorkCenterCode, "E310", 0, FeedbackState.Finished, 0, _Logger);
                            }
                        }
                    }
                }				
                
                // Sorting
                var sortDKM001 = prodOrdersRep.Get(
                    po => po.CustomerOrderCode == dkm001 && po.ComponentType == ComponentType.DoorLeft ||
                            po.CustomerOrderCode == dkm001 && po.ComponentType == ComponentType.DoorRight ||
                            po.CustomerOrderCode == dkm001 && po.ComponentType == ComponentType.Partition ||
                            po.CustomerOrderCode == dkm001 && po.ComponentType == ComponentType.DrawerSide ||
                            po.CustomerOrderCode == dkm001 && po.ComponentType == ComponentType.FixedShelf ||
                            po.CustomerOrderCode == dkm001 && po.ComponentType == ComponentType.WorkTop);
                                                        
                if(sortDKM001 != null)
                {   
                    foreach(var sort001 in sortDKM001)
                    {           
                        var prodItemSort001 = prodItemsRep.GetFirstOrDefault(pi => pi.ProductionOrderCode == sort001.Code);
                        
                        if(prodItemSort001 != null)
                        {
                            var prodItemsStepsDataSort001 = prodItemsStepsDataRep.GetFirstOrDefault(
                                pisd => pisd.ProductionOrderCode == sort001.Code && pisd.ProductionStepCode == sortingStepCode);
                            
                            if(prodItemsStepsDataSort001!=null)
                            {
                                prodItemSort001.InsertFeedback(unitOfWork, _TaskName, 1, 0, 0, sortingWorkCenterCode, "SORT", 0, FeedbackState.Finished, 0, _Logger);
                            }
                        }
                    }
                }
                
                // Preassembly 
                var preassemDKM001 = prodOrdersRep.Get(
                    po => po.CustomerOrderCode == dkm001 && po.ComponentType == ComponentType.SidePanel && po.OrderType == ProductionOrderType.ConstructionPart ||
                            po.CustomerOrderCode == dkm001 && po.ComponentType == ComponentType.TopShelf && po.OrderType == ProductionOrderType.ConstructionPart && po.ReproductionType == ReproductionType.NoReproduction ||
                            po.CustomerOrderCode == dkm001 && po.ComponentType == ComponentType.DrawerBottom && po.OrderType == ProductionOrderType.ConstructionPart ||
                            po.CustomerOrderCode == dkm001 && po.ComponentType == ComponentType.DrawerFront && po.OrderType == ProductionOrderType.ConstructionPart && po.ReproductionType == ReproductionType.NoReproduction);
                                                        
                if(preassemDKM001 != null)
                {   
                    foreach(var preassem001 in preassemDKM001)
                    {           
                        var prodItemPreassem001 = prodItemsRep.GetFirstOrDefault(pi => pi.ProductionOrderCode == preassem001.Code);
                        
                        if(prodItemPreassem001 != null)
                        {
                            var prodItemsStepsDataPreassem001 = prodItemsStepsDataRep.GetFirstOrDefault(
                                pisd => pisd.ProductionOrderCode == preassem001.Code && pisd.ProductionStepCode == preassemblyStepCode);
                            
                            if(prodItemsStepsDataPreassem001 != null)
                            {
                                prodItemPreassem001.InsertFeedback(unitOfWork, _TaskName, 1, 0, 0, preassemblyWorkCenterCode, "PREASSEM", 0, FeedbackState.Finished, 0, _Logger);
                            }
                        }
                    }
                }
                
                unitOfWork.Save();
            }

        }
        catch (Exception e)
        {
            executionContext.JobResultSetComplete(JobResult.Failed);
            _Logger.Error(ResourcesKeys.ErrorInUserExit("SetFeedbackBlueBulk"), null, e);
            throw;
        }
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
