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
//   T.Stürzer       2024-11-15    Changed Workcenters for feedback
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
    private string cuttingWorkCenterCode = "CU1";
    private string edgeWorkCenterCode = "EB1";
    private string drillingWorkCenterCode = "CNC1";
    private string cncWorkCenterCode = "CNC2";
    private string sortingWorkCenterCode = "SP";
    private string preassemblyWorkCenterCode = "PRE";
    private string assemblyWorkCenterCode = "AS1";
    
    // ProductionStepCodes
    private string cuttingStepCode = "CU1";
    private string edgeStepCode = "EB1";
    private string drillingStepCode = "CNC1";
    private string cncStepCode = "CNC2";
    private string sortingStepCode = "SP";
    private string preassemblyStepCode = "PRE";
    private string assemblyStepCode = "AS1";
    
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
                var validPositionsDKS003 = new List<string> { "001", "002", "003", "004", "005", "006", "007" };

                var preassemDKS003 = prodOrdersRep.Get(
                    po => po.CustomerOrderCode == dks003 && 
                          validPositionsDKS003.Contains(po.CustomerOrderPosition) && 
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
                            prodItemPreassem003.InsertFeedback(unitOfWork, _TaskName, 1, 0, 0, preassemblyWorkCenterCode, preassemblyStepCode, 0, FeedbackState.Finished, 0, _Logger);
                        }
                    }
                }
				
                // Assembly Sale Item
                var assemDKS003 = prodOrdersRep.Get(
                    po => po.CustomerOrderCode == dks003 && 
                          validPositionsDKS003.Contains(po.CustomerOrderPosition) && 
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
                                prodItemAssem003.InsertFeedback(unitOfWork, _TaskName, 1, 0, 0, assemblyWorkCenterCode, assemblyStepCode, 0, FeedbackState.Finished, 0, _Logger);
                            }
                        }
                    }
                    
//-----------------------------------------------------------------------------    
    
                // Demo_Kitchen_Small_004
                // Sorting
                var componentTypesDKS004Sorting = new List<ComponentType>
                    {
                        ComponentType.AdjustableShelf,  ComponentType.TopShelf,     ComponentType.BottomShelf,      ComponentType.BackPanel,
                        ComponentType.DoorLeft,         ComponentType.FixedShelf,   ComponentType.Partition,        ComponentType.DrawerBottom, 
                        ComponentType.DrawerSide,       ComponentType.DrawerFront,  ComponentType.Plinth,           ComponentType.WorkTop, 
                        ComponentType.Traverse 
                    };
                
                var sortDKS004 = prodOrdersRep.Get(
                    po => po.CustomerOrderCode == dks004 && componentTypesDKS004Sorting.Contains(po.ComponentType));
                
                if(sortDKS004.Any())
                {   
                    foreach(var sort004 in sortDKS004)
                    {           
                        var prodItemSort004 = prodItemsRep.GetFirstOrDefault(pi => pi.ProductionOrderCode == sort004.Code);
                
                        var prodItemsStepsDataSort004 = prodItemsStepsDataRep.GetFirstOrDefault(
                            pisd => pisd.ProductionOrderCode == sort004.Code && pisd.ProductionStepCode == sortingStepCode);
                
                        if(prodItemSort004 != null && prodItemsStepsDataSort004 != null)
                        {
                            prodItemSort004.InsertFeedback(unitOfWork, _TaskName, 1, 0, 0, sortingWorkCenterCode, sortingStepCode, 0, FeedbackState.Finished, 0, _Logger);
                        }
                    }
                }

                
                // Preassembly 
				var componentTypesDKS004Preassembly = new List<ComponentType>
                    {
                        ComponentType.SidePanel,        ComponentType.Door,     ComponentType.DoorRight
                    };
                    
                var preassemDKS004 = prodOrdersRep.Get(
                    po => po.CustomerOrderCode == dks004 && 
                          componentTypesDKS004Preassembly.Contains(po.ComponentType) && 
                          po.OrderType == ProductionOrderType.ConstructionPart);
                                                        
                if(preassemDKS004.Any())
                {   
                    foreach(var preassem004 in preassemDKS004)
                    {           
                        var prodItemPreassepisd = prodItemsRep.GetFirstOrDefault(pi => pi.ProductionOrderCode == preassem004.Code);
                        
                        var prodItemsStepsDataPreassepisd = prodItemsStepsDataRep.GetFirstOrDefault(
                                pisd => pisd.ProductionOrderCode == preassem004.Code && pisd.ProductionStepCode == preassemblyStepCode);
                        
                        if(prodItemPreassepisd != null && prodItemsStepsDataPreassepisd != null)
                        {
                            prodItemPreassepisd.InsertFeedback(unitOfWork, _TaskName, 1, 0, 0, preassemblyWorkCenterCode, preassemblyStepCode, 0, FeedbackState.Finished, 0, _Logger);
                        }
                    }
                }
   
                // Assembly Sale Item
                var validPositionsDKS004 = new List<string> { "001", "003", "006" };
                
                var assemDKS004 = prodOrdersRep.Get(
                    po => po.CustomerOrderCode == dks004 && 
                          validPositionsDKS004.Contains(po.CustomerOrderPosition) && 
                          po.OrderType == ProductionOrderType.SalesArticle);

                    if(assemDKS004.Any())
                    {   
                        foreach(var assem004 in assemDKS004)
                        {           
                            var prodItemAssem004 = prodItemsRep.GetFirstOrDefault(pi => pi.ProductionOrderCode == assem004.Code);
                            
                            var prodItemsStepsDataAssem004 = prodItemsStepsDataRep.GetFirstOrDefault(
                                pisd => pisd.ProductionOrderCode == assem004.Code && pisd.ProductionStepCode == assemblyStepCode);
                            
                            if(prodItemAssem004 != null && prodItemsStepsDataAssem004 != null)
                            {
                                prodItemAssem004.InsertFeedback(unitOfWork, _TaskName, 1, 0, 0, assemblyWorkCenterCode, assemblyStepCode, 0, FeedbackState.Finished, 0, _Logger);
                            }
                        }
                    }
                    
//-----------------------------------------------------------------------------

                // Demo_Kitchen_Small_005
                // CNC 310
                var componentTypesDKS005CNC = new List<ComponentType>
                    {
                        ComponentType.DrawerFront
                    };
                    
                var cncDKS005 = prodOrdersRep.Get(
                    po => po.CustomerOrderCode == dks004 && componentTypesDKS005CNC.Contains(po.ComponentType));
                                                        
                if(cncDKS005.Any())
                {   
                    foreach(var cnc005 in cncDKS005)
                    {           
                        var prodItemCnc005 = prodItemsRep.GetFirstOrDefault(
                            pi => pi.ProductionOrderCode == cnc005.Code);
                            
                        var prodItemsStepsDataCnc005 = prodItemsStepsDataRep.GetFirstOrDefault(
                            pisd => pisd.ProductionOrderCode == cnc005.Code && pisd.ProductionStepCode == cncStepCode);
                        
                        if(prodItemCnc005 != null && prodItemsStepsDataCnc005 != null)
                        {
                            prodItemCnc005.InsertFeedback(unitOfWork, _TaskName, 1, 0, 0, cncWorkCenterCode, cncStepCode, 0, FeedbackState.Finished, 0, _Logger);
                        }
                    }
                }
                
                // Sorting
				var componentTypesDKS005Sorting = new List<ComponentType>
                    {
                        ComponentType.SidePanel,        ComponentType.Panel,        ComponentType.Door,         ComponentType.DoorLeft,
                        ComponentType.DoorRight,        ComponentType.Partition,    ComponentType.DrawerSide,   ComponentType.DrawerFront,
                        ComponentType.WorkTop                        
                    };
                    
                var sortDKS005 = prodOrdersRep.Get(
                    po => po.CustomerOrderCode == dks005 && componentTypesDKS005Sorting.Contains(po.ComponentType) && 
                          po.ReproductionType == ReproductionType.NoReproduction);
                                                        
                if(sortDKS005.Any())
                {   
                    foreach(var sort005 in sortDKS005)
                    {           
                        var prodItemSort005 = prodItemsRep.GetFirstOrDefault(pi => pi.ProductionOrderCode == sort005.Code);
                        
                        var prodItemsStepsDataSort005 = prodItemsStepsDataRep.GetFirstOrDefault(
                                pisd => pisd.ProductionOrderCode == sort005.Code && pisd.ProductionStepCode == sortingStepCode);
                        
                        if(prodItemSort005 != null && prodItemsStepsDataSort005 != null)
                        {
                            prodItemSort005.InsertFeedback(unitOfWork, _TaskName, 1, 0, 0, sortingWorkCenterCode, sortingStepCode, 0, FeedbackState.Finished, 0, _Logger);
                        }
                    }
                }
                
                // Preassembly                
                var componentTypesDKS005Preassembly = new List<ComponentType>
                {
                    ComponentType.AdjustableShelf,      ComponentType.TopShelf,     ComponentType.BottomShelf,      ComponentType.BackPanel,
                    ComponentType.FixedShelf,           ComponentType.DrawerBottom, ComponentType.Plinth,           ComponentType.Traverse
                };
                
                var preassemDKS005 = prodOrdersRep.Get(
                    po => po.CustomerOrderCode == dks005 && componentTypesDKS005Preassembly.Contains(po.ComponentType));

                if(preassemDKS005.Any())
                {   
                    foreach(var preassem005 in preassemDKS005)
                    {           
                        var prodItemPreassem005 = prodItemsRep.GetFirstOrDefault(pi => pi.ProductionOrderCode == preassem005.Code);
                        
                        var prodItemsStepsDataPreassem005 = prodItemsStepsDataRep.GetFirstOrDefault(
                                pisd => pisd.ProductionOrderCode == preassem005.Code && pisd.ProductionStepCode == preassemblyStepCode);
                        
                        if(prodItemPreassem005 != null && prodItemsStepsDataPreassem005 != null)
                        {
                            prodItemPreassem005.InsertFeedback(unitOfWork, _TaskName, 1, 0, 0, preassemblyWorkCenterCode, preassemblyStepCode, 0, FeedbackState.Finished, 0, _Logger);
                        }
                    }
                }
//-----------------------------------------------------------------------------
                
                // Demo_Kitchen_Medium_001
                // Cutting B300                        
                var componentTypesDKM001Cutting = new List<ComponentType>
                    {
                        ComponentType.TopShelf                   
                    };
                    
                var cutDKM001 = prodOrdersRep.Get(
                    po => po.CustomerOrderCode == dkm001 && componentTypesDKM001Cutting.Contains(po.ComponentType) && 
                          po.ReproductionType == ReproductionType.NoReproduction);
                                                        
                if(cutDKM001.Any())
                {   
                    foreach(var cut001 in cutDKM001)
                    {           
                        var prodItemCut001 = prodItemsRep.GetFirstOrDefault(pi => pi.ProductionOrderCode == cut001.Code);
                        
                        var prodItemsStepsDataCut001 = prodItemsStepsDataRep.GetFirstOrDefault(
                                pisd => pisd.ProductionOrderCode == cut001.Code && pisd.ProductionStepCode == cuttingStepCode);
                        
                        if(prodItemCut001 != null && prodItemsStepsDataCut001 != null)
                        {
                            prodItemCut001.InsertFeedback(unitOfWork, _TaskName, 1, 0, 0, cuttingWorkCenterCode, cuttingStepCode, 0, FeedbackState.Finished, 0, _Logger);
                        }
                    }
                }
                
                // Drilling V200                          
                var componentTypesDKM001Drilling = new List<ComponentType>
                {
                    ComponentType.AdjustableShelf,      ComponentType.BottomShelf,      ComponentType.BackPanel,        ComponentType.Panel,
                    ComponentType.Plinth,               ComponentType.Traverse
                };
                
                var drillDKM001 = prodOrdersRep.Get(
                    po => po.CustomerOrderCode == dkm001 && componentTypesDKM001Drilling.Contains(po.ComponentType));
                                                        
                if(drillDKM001.Any())
                {   
                    foreach(var drill001 in drillDKM001)
                    {           
                        var prodItemDrill001 = prodItemsRep.GetFirstOrDefault(
                            pi => pi.ProductionOrderCode == drill001.Code);
                            
                        var prodItemsStepsDataDrill001 = prodItemsStepsDataRep.GetFirstOrDefault(
                            pisd => pisd.ProductionOrderCode == drill001.Code && pisd.ProductionStepCode == drillingStepCode);

                        
                        if(prodItemDrill001 != null && prodItemsStepsDataDrill001 != null)
                        {
                            prodItemDrill001.InsertFeedback(unitOfWork, _TaskName, 1, 0, 0, drillingWorkCenterCode, drillingStepCode, 0, FeedbackState.Finished, 0, _Logger);
                        }
                    }
                }
				
				// CNC 310
                var componentTypesDKM001CNC = new List<ComponentType>
                    {
                        ComponentType.Door,     ComponentType.DrawerFront
                    };
                    
                var cncDKM001 = prodOrdersRep.Get(
                    po => po.CustomerOrderCode == dkm001 && componentTypesDKM001CNC.Contains(po.ComponentType) && 
                          po.ReproductionType == ReproductionType.NoReproduction);
                                                        
                if(cncDKM001.Any())
                {   
                    foreach(var cnc001 in cncDKM001)
                    {           
                        var prodItemCnc001 = prodItemsRep.GetFirstOrDefault(
                            pi => pi.ProductionOrderCode == cnc001.Code);
                            
                        var prodItemsStepsDataCnc001 = prodItemsStepsDataRep.GetFirstOrDefault(
                            pisd => pisd.ProductionOrderCode == cnc001.Code && pisd.ProductionStepCode == cncStepCode);
                                                    
                        if(prodItemCnc001 != null && prodItemsStepsDataCnc001 != null)
                        {
                            prodItemCnc001.InsertFeedback(unitOfWork, _TaskName, 1, 0, 0, cncWorkCenterCode, "E310", 0, FeedbackState.Finished, 0, _Logger);
                        }
                    }
                }				
                
                // Sorting                            
                var componentTypesDKM001Sorting = new List<ComponentType>
                    {
                        ComponentType.DoorLeft,     ComponentType.DoorRight,        ComponentType.FixedShelf,       ComponentType.Partition,
                        ComponentType.DrawerSide,   ComponentType.WorkTop
                    };
                    
                var sortDKM001 = prodOrdersRep.Get(
                    po => po.CustomerOrderCode == dkm001 && componentTypesDKM001Sorting.Contains(po.ComponentType));
                                                        
                if(sortDKM001.Any())
                {   
                    foreach(var sort001 in sortDKM001)
                    {           
                        var prodItemSort001 = prodItemsRep.GetFirstOrDefault(pi => pi.ProductionOrderCode == sort001.Code);
                        
                        var prodItemsStepsDataSort001 = prodItemsStepsDataRep.GetFirstOrDefault(
                            pisd => pisd.ProductionOrderCode == sort001.Code && pisd.ProductionStepCode == sortingStepCode);
                                
                        if(prodItemSort001 != null && prodItemsStepsDataSort001!=null)
                        {
                            prodItemSort001.InsertFeedback(unitOfWork, _TaskName, 1, 0, 0, sortingWorkCenterCode, sortingStepCode, 0, FeedbackState.Finished, 0, _Logger);
                        }
                    }
                }
                
                // Preassembly 
                var componentTypesDKM001Preassembly = new List<ComponentType>
                {
                    ComponentType.SidePanel,        ComponentType.TopShelf,     ComponentType.DrawerBottom,     ComponentType.DrawerFront,
                };
                
                var preassemDKM001 = prodOrdersRep.Get(
                    po => po.CustomerOrderCode == dkm001 && componentTypesDKM001Preassembly.Contains(po.ComponentType) && 
                          po.ReproductionType == ReproductionType.NoReproduction);

                if(preassemDKM001 != null)
                {   
                    foreach(var preassem001 in preassemDKM001)
                    {           
                        var prodItemPreassem001 = prodItemsRep.GetFirstOrDefault(pi => pi.ProductionOrderCode == preassem001.Code);
                        
                        var prodItemsStepsDataPreassem001 = prodItemsStepsDataRep.GetFirstOrDefault(
                            pisd => pisd.ProductionOrderCode == preassem001.Code && pisd.ProductionStepCode == preassemblyStepCode);
                        
                        if(prodItemPreassem001 != null && prodItemsStepsDataPreassem001 != null)
                        {
                            prodItemPreassem001.InsertFeedback(unitOfWork, _TaskName, 1, 0, 0, preassemblyWorkCenterCode, preassemblyStepCode, 0, FeedbackState.Finished, 0, _Logger);
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
