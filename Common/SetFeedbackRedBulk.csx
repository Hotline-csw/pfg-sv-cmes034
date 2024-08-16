#r ControllerMES.Infrastructure.Common
#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   SetFeedbackRedBulk
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
//   T.Stürzer       2024-08-16    Changed Feedback from InsertFeedbackFinishedGood to InsertFeedback
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IServerCustomization))]
[Export("SetFeedbackRedBulk", typeof(HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("[08] Set feedback for Bulk-Red")]
[EnabledScript(true)]
public class SetFeedbackRedBulk : GenericTaskBase, HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;
    
    [Import]
    protected IDistributedServiceProvider DistributedServiceProvider { get; private set; }

    private Logger _Logger;
    
    private string _TaskName = "SetFeedbackRedBulk";
    
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
    private string dks006 = "Demo_Kitchen_Small_006";
    
    // Medium Kitchens
    private string dkm006 = "Demo_Kitchen_Medium_006";
    private string dkm008 = "Demo_Kitchen_Medium_008";
    private string dkm009 = "Demo_Kitchen_Medium_009";


    public override void Execute(IJobExecutionContext executionContext)
    {
        Guard.ThrowOnArgumentNull(executionContext, "executionContext");

        try
        {
            _Logger = LogHelper.GetLogger(executionContext.Area, typeof(SetFeedbackRedBulk));
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

                // Demo_Kitchen_Small_006
                // Edgebanding S810
                var componentTypesDKS006Edge = new List<ComponentType>
                {
                    ComponentType.DoorRight,
                };
                    
                var edgeDKS006 = prodOrdersRep.Get(
                    po => po.CustomerOrderCode == dks006 && componentTypesDKS006Edge.Contains(po.ComponentType));
                                                        
                if(edgeDKS006.Any())
                {   
                    foreach(var edge006 in edgeDKS006)
                    {           
                        var prodItemEdge006 = prodItemsRep.GetFirstOrDefault(pi => pi.ProductionOrderCode == edge006.Code);
                        
                        var prodItemsStepsDataEdge006 = prodItemsStepsDataRep.GetFirstOrDefault(
                            pisd => pisd.ProductionOrderCode == edge006.Code && pisd.ProductionStepCode == edgeStepCode);
                                
                        if(prodItemEdge006 != null && prodItemsStepsDataEdge006 != null)
                        {
                            prodItemEdge006.InsertFeedback(unitOfWork, _TaskName, 1, 0, 0, edgeWorkCenterCode, edgeStepCode, 0, FeedbackState.Finished, 0, _Logger);
                        }
                    }
                }
                
                // CNC 310                           
                var componentTypesDKS006Cnc = new List<ComponentType>
                {
                    ComponentType.Door,     ComponentType.DrawerBottom,     ComponentType.DrawerSide
                };
                
                var cncDKS006 = prodOrdersRep.Get(
                    po => po.CustomerOrderCode == dks006 && componentTypesDKS006Cnc.Contains(po.ComponentType));
                                                        
                if(cncDKS006.Any())
                {   
                    foreach(var cnc006 in cncDKS006)
                    {           
                        var prodItemCnc006 = prodItemsRep.GetFirstOrDefault(
                            pi => pi.ProductionOrderCode == cnc006.Code);
                            
                    var prodItemsStepsDataCnc006 = prodItemsStepsDataRep.GetFirstOrDefault(
                            pisd => pisd.ProductionOrderCode == cnc006.Code && pisd.ProductionStepCode == cncStepCode);
                            
                        if(prodItemCnc006 != null && prodItemsStepsDataCnc006 != null)
                        {
                            prodItemCnc006.InsertFeedback(unitOfWork, _TaskName, 1, 0, 0, cncWorkCenterCode, cncStepCode, 0, FeedbackState.Finished, 0, _Logger);
                        }
                    }
                }
                
                // Sorting                            
                var componentTypesDKS006Sorting = new List<ComponentType>
                {
                    ComponentType.SidePanel,        ComponentType.AdjustableShelf,      ComponentType.TopShelf,     ComponentType.BottomShelf,
                    ComponentType.BackPanel,        ComponentType.Panel,                ComponentType.DoorLeft,     ComponentType.FixedShelf,
                    ComponentType.Partition,        ComponentType.DrawerFront,          ComponentType.Plinth,       ComponentType.WorkTop,
                    ComponentType.Traverse
                };
                
                var sortDKS006 = prodOrdersRep.Get(
                    po => po.CustomerOrderCode == dks006 && componentTypesDKS006Sorting.Contains(po.ComponentType));

                                                        
                if(sortDKS006 != null)
                {   
                    foreach(var sort006 in sortDKS006)
                    {           
                        var prodItemSort006 = prodItemsRep.GetFirstOrDefault(pi => pi.ProductionOrderCode == sort006.Code);
                        
                        var prodItemsStepsDataSort006 = prodItemsStepsDataRep.GetFirstOrDefault(
                            pisd => pisd.ProductionOrderCode == sort006.Code && pisd.ProductionStepCode == sortingStepCode);

                        if(prodItemSort006 != null && prodItemsStepsDataSort006!=null)
                        {
                            prodItemSort006.InsertFeedback(unitOfWork, _TaskName, 1, 0, 0, sortingWorkCenterCode, sortingStepCode, 0, FeedbackState.Finished, 0, _Logger);
                        }
                    }
                }
				
//-----------------------------------------------------------------------------
                
                // Demo_Kitchen_Medium_006
                // Cutting B300
                var cutDKM006 = prodOrdersRep.Get(
                        po => po.CustomerOrderCode == dkm006 && po.ComponentType == ComponentType.TopShelf && po.ReproductionType == ReproductionType.Standard);
                                                        
                if(cutDKM006 != null)
                {   
                    foreach(var cut006 in cutDKM006)
                    {           
                        var prodItemCut006 = prodItemsRep.GetFirstOrDefault(pi => pi.ProductionOrderCode == cut006.Code);
                        
                        if(prodItemCut006 != null)
                        {
                            var prodItemsStepsDataCut006 = prodItemsStepsDataRep.GetFirstOrDefault(
                                pisd => pisd.ProductionOrderCode == cut006.Code && pisd.ProductionStepCode == cuttingStepCode);
                            
                            if(prodItemsStepsDataCut006 != null)
                            {
                                prodItemCut006.InsertFeedbackFinishedGood(unitOfWork, _TaskName, cuttingWorkCenterCode, "", 1, _Logger);
                            }
                        }
                    }
                }                
                
                // Edgebanding S810
                var edgeDKM006 = prodOrdersRep.Get(
                    po => po.CustomerOrderCode == dkm006 && po.ComponentType == ComponentType.AdjustableShelf ||
                            po.CustomerOrderCode == dkm006 && po.ComponentType == ComponentType.Panel ||
                            po.CustomerOrderCode == dkm006 && po.ComponentType == ComponentType.Door ||
                            po.CustomerOrderCode == dkm006 && po.ComponentType == ComponentType.DoorLeft ||
                            po.CustomerOrderCode == dkm006 && po.ComponentType == ComponentType.FixedShelf);
                                                        
                if(edgeDKM006 != null)
                {   
                    foreach(var edge006 in edgeDKM006)
                    {           
                        var prodItemEdge006 = prodItemsRep.GetFirstOrDefault(pi => pi.ProductionOrderCode == edge006.Code);
                        
                        if(prodItemEdge006 != null)
                        {
                            var prodItemsStepsDataEdge006 = prodItemsStepsDataRep.GetFirstOrDefault(
                                pisd => pisd.ProductionOrderCode == edge006.Code && pisd.ProductionStepCode == edgeStepCode);
                            
                            if(prodItemsStepsDataEdge006 != null)
                            {
                                prodItemEdge006.InsertFeedbackFinishedGood(unitOfWork, _TaskName, edgeWorkCenterCode, "", 1, _Logger);
                            }
                        }
                    }
                }
                
                // Drilling V200
                var drillDKM006 = prodOrdersRep.Get(
                    po => po.CustomerOrderCode == dkm006 && po.ComponentType == ComponentType.BottomShelf ||
                            po.CustomerOrderCode == dkm006 && po.ComponentType == ComponentType.WorkTop ||
                            po.CustomerOrderCode == dkm006 && po.ComponentType == ComponentType.BackPanel ||
                            po.CustomerOrderCode == dkm006 && po.ComponentType == ComponentType.SidePanel ||
                            po.CustomerOrderCode == dkm006 && po.ComponentType == ComponentType.Traverse);
                                                        
                if(drillDKM006 != null)
                {   
                    foreach(var drill006 in drillDKM006)
                    {           
                        var prodItemDrill006 = prodItemsRep.GetFirstOrDefault(
                            pi => pi.ProductionOrderCode == drill006.Code);
                        
                        if(prodItemDrill006 != null)
                        {
                            var prodItemsStepsDataDrill006 = prodItemsStepsDataRep.GetFirstOrDefault(
                                pisd => pisd.ProductionOrderCode == drill006.Code && pisd.ProductionStepCode == drillingStepCode);
                            
                            if(prodItemsStepsDataDrill006 != null)
                            {
                                prodItemDrill006.InsertFeedbackFinishedGood(unitOfWork, _TaskName, drillingWorkCenterCode, "", 1, _Logger);
                            }
                        }
                    }
                }
				
				// CNC 310
                var cncDKM006 = prodOrdersRep.Get(
                    po => po.CustomerOrderCode == dkm006 && po.ComponentType == ComponentType.DrawerBottom ||
                            po.CustomerOrderCode == dkm006 && po.ComponentType == ComponentType.DoorRight && po.ReproductionType == ReproductionType.NoReproduction ||
                            po.CustomerOrderCode == dkm006 && po.ComponentType == ComponentType.DrawerSide);
                                                        
                if(cncDKM006 != null)
                {   
                    foreach(var cnc006 in cncDKM006)
                    {           
                        var prodItemCnc006 = prodItemsRep.GetFirstOrDefault(
                            pi => pi.ProductionOrderCode == cnc006.Code);
                        
                        if(prodItemCnc006 != null)
                        {
                            var prodItemsStepsDataCnc006 = prodItemsStepsDataRep.GetFirstOrDefault(
                                pisd => pisd.ProductionOrderCode == cnc006.Code && pisd.ProductionStepCode == cncStepCode);
                            
                            if(prodItemsStepsDataCnc006 != null)
                            {
                                prodItemCnc006.InsertFeedbackFinishedGood(unitOfWork, _TaskName, cncWorkCenterCode, "", 1, _Logger);
                            }
                        }
                    }
                }			
				
                // Sorting
                var sortDKM006 = prodOrdersRep.Get(
                    po => po.CustomerOrderCode == dkm006 && po.ComponentType == ComponentType.Partition ||
                            po.CustomerOrderCode == dkm006 && po.ComponentType == ComponentType.DrawerFront ||
                            po.CustomerOrderCode == dkm006 && po.ComponentType == ComponentType.TopShelf && po.ReproductionType == ReproductionType.NoReproduction ||
                            po.CustomerOrderCode == dkm006 && po.ComponentType == ComponentType.Plinth);
                                                        
                if(sortDKM006 != null)
                {   
                    foreach(var sort006 in sortDKM006)
                    {           
                        var prodItemSort006 = prodItemsRep.GetFirstOrDefault(pi => pi.ProductionOrderCode == sort006.Code);
                        
                        if(prodItemSort006 != null)
                        {
                            var prodItemsStepsDataSort006 = prodItemsStepsDataRep.GetFirstOrDefault(
                                pisd => pisd.ProductionOrderCode == sort006.Code && pisd.ProductionStepCode == sortingStepCode);
                            
                            if(prodItemsStepsDataSort006!=null)
                            {
                                prodItemSort006.InsertFeedbackFinishedGood(unitOfWork, _TaskName, sortingWorkCenterCode, "", 1, _Logger);
                            }
                        }
                    }
                }
				
//-----------------------------------------------------------------------------
                
                // Demo_Kitchen_Medium_008
                // Edgebanding S810
                var edgeDKM008 = prodOrdersRep.Get(
                    po => po.CustomerOrderCode == dkm008 && po.ComponentType == ComponentType.SidePanel ||
                            po.CustomerOrderCode == dkm008 && po.ComponentType == ComponentType.BottomShelf ||
                            po.CustomerOrderCode == dkm008 && po.ComponentType == ComponentType.Panel ||
                            po.CustomerOrderCode == dkm008 && po.ComponentType == ComponentType.DrawerSide ||
                            po.CustomerOrderCode == dkm008 && po.ComponentType == ComponentType.DrawerFront);
                                                        
                if(edgeDKM008 != null)
                {   
                    foreach(var edge008 in edgeDKM008)
                    {           
                        var prodItemEdge008 = prodItemsRep.GetFirstOrDefault(pi => pi.ProductionOrderCode == edge008.Code);
                        
                        if(prodItemEdge008 != null)
                        {
                            var prodItemsStepsDataEdge008 = prodItemsStepsDataRep.GetFirstOrDefault(
                                pisd => pisd.ProductionOrderCode == edge008.Code && pisd.ProductionStepCode == edgeStepCode);
                            
                            if(prodItemsStepsDataEdge008 != null)
                            {
                                prodItemEdge008.InsertFeedbackFinishedGood(unitOfWork, _TaskName, edgeWorkCenterCode, "", 1, _Logger);
                            }
                        }
                    }
                }
                
                // Drilling V200
                var drillDKM008 = prodOrdersRep.Get(
                    po => po.CustomerOrderCode == dkm008 && po.ComponentType == ComponentType.TopShelf ||
                            po.CustomerOrderCode == dkm008 && po.ComponentType == ComponentType.Partition ||
                            po.CustomerOrderCode == dkm008 && po.ComponentType == ComponentType.Plinth ||
                            po.CustomerOrderCode == dkm008 && po.ComponentType == ComponentType.WorkTop ||
                            po.CustomerOrderCode == dkm008 && po.ComponentType == ComponentType.BackPanel ||
                            po.CustomerOrderCode == dkm008 && po.ComponentType == ComponentType.AdjustableShelf ||
                            po.CustomerOrderCode == dkm008 && po.ComponentType == ComponentType.FixedShelf ||
                            po.CustomerOrderCode == dkm008 && po.ComponentType == ComponentType.Traverse);
                                                        
                if(drillDKM008 != null)
                {   
                    foreach(var drill008 in drillDKM008)
                    {           
                        var prodItemDrill008 = prodItemsRep.GetFirstOrDefault(
                            pi => pi.ProductionOrderCode == drill008.Code);
                        
                        if(prodItemDrill008 != null)
                        {
                            var prodItemsStepsDataDrill008 = prodItemsStepsDataRep.GetFirstOrDefault(
                                pisd => pisd.ProductionOrderCode == drill008.Code && pisd.ProductionStepCode == drillingStepCode);
                            
                            if(prodItemsStepsDataDrill008 != null)
                            {
                                prodItemDrill008.InsertFeedbackFinishedGood(unitOfWork, _TaskName, drillingWorkCenterCode, "", 1, _Logger);
                            }
                        }
                    }
                }
				
				// CNC 310
                var cncDKM008 = prodOrdersRep.Get(
                    po => po.CustomerOrderCode == dkm008 && po.ComponentType == ComponentType.Door ||
                            po.CustomerOrderCode == dkm008 && po.ComponentType == ComponentType.DoorLeft ||
                            po.CustomerOrderCode == dkm008 && po.ComponentType == ComponentType.DrawerBottom ||
                            po.CustomerOrderCode == dkm008 && po.ComponentType == ComponentType.DoorRight);
                                                        
                if(cncDKM008 != null)
                {   
                    foreach(var cnc008 in cncDKM008)
                    {           
                        var prodItemCnc008 = prodItemsRep.GetFirstOrDefault(
                            pi => pi.ProductionOrderCode == cnc008.Code);
                        
                        if(prodItemCnc008 != null)
                        {
                            var prodItemsStepsDataCnc008 = prodItemsStepsDataRep.GetFirstOrDefault(
                                pisd => pisd.ProductionOrderCode == cnc008.Code && pisd.ProductionStepCode == cncStepCode);
                            
                            if(prodItemsStepsDataCnc008 != null)
                            {
                                prodItemCnc008.InsertFeedbackFinishedGood(unitOfWork, _TaskName, cncWorkCenterCode, "", 1, _Logger);
                            }
                        }
                    }
                }
				
//-----------------------------------------------------------------------------
                
                // Demo_Kitchen_Medium_009
                // Edgebanding S810
                var edgeDKM009 = prodOrdersRep.Get(
                    po => po.CustomerOrderCode == dkm009 && po.ComponentType == ComponentType.AdjustableShelf ||
                            po.CustomerOrderCode == dkm009 && po.ComponentType == ComponentType.Panel ||
                            po.CustomerOrderCode == dkm009 && po.ComponentType == ComponentType.Door ||
                            po.CustomerOrderCode == dkm009 && po.ComponentType == ComponentType.DoorRight ||
                            po.CustomerOrderCode == dkm009 && po.ComponentType == ComponentType.FixedShelf ||
                            po.CustomerOrderCode == dkm009 && po.ComponentType == ComponentType.DrawerFront ||
                            po.CustomerOrderCode == dkm009 && po.ComponentType == ComponentType.Partition ||
                            po.CustomerOrderCode == dkm009 && po.ComponentType == ComponentType.WorkTop);
                                                        
                if(edgeDKM009 != null)
                {   
                    foreach(var edge009 in edgeDKM009)
                    {           
                        var prodItemEdge009 = prodItemsRep.GetFirstOrDefault(pi => pi.ProductionOrderCode == edge009.Code);
                        
                        if(prodItemEdge009 != null)
                        {
                            var prodItemsStepsDataEdge009 = prodItemsStepsDataRep.GetFirstOrDefault(
                                pisd => pisd.ProductionOrderCode == edge009.Code && pisd.ProductionStepCode == edgeStepCode);
                            
                            if(prodItemsStepsDataEdge009 != null)
                            {
                                prodItemEdge009.InsertFeedbackFinishedGood(unitOfWork, _TaskName, edgeWorkCenterCode, "", 1, _Logger);
                            }
                        }
                    }
                }
                
                // Drilling V200
                var drillDKM009 = prodOrdersRep.Get(
                    po => po.CustomerOrderCode == dkm009 && po.ComponentType == ComponentType.TopShelf ||
                            po.CustomerOrderCode == dkm009 && po.ComponentType == ComponentType.Plinth ||
                            po.CustomerOrderCode == dkm009 && po.ComponentType == ComponentType.BackPanel ||
                            po.CustomerOrderCode == dkm009 && po.ComponentType == ComponentType.SidePanel ||
                            po.CustomerOrderCode == dkm009 && po.ComponentType == ComponentType.BottomShelf ||
                            po.CustomerOrderCode == dkm009 && po.ComponentType == ComponentType.Traverse);
                                                        
                if(drillDKM009 != null)
                {   
                    foreach(var drill009 in drillDKM009)
                    {           
                        var prodItemDrill009 = prodItemsRep.GetFirstOrDefault(
                            pi => pi.ProductionOrderCode == drill009.Code);
                        
                        if(prodItemDrill009 != null)
                        {
                            var prodItemsStepsDataDrill009 = prodItemsStepsDataRep.GetFirstOrDefault(
                                pisd => pisd.ProductionOrderCode == drill009.Code && pisd.ProductionStepCode == drillingStepCode);
                            
                            if(prodItemsStepsDataDrill009 != null)
                            {
                                prodItemDrill009.InsertFeedbackFinishedGood(unitOfWork, _TaskName, drillingWorkCenterCode, "", 1, _Logger);
                            }
                        }
                    }
                }			
				
				// CNC 310
                var cncDKM009 = prodOrdersRep.Get(
                    po => po.CustomerOrderCode == dkm009 && po.ComponentType == ComponentType.DoorLeft ||
                            po.CustomerOrderCode == dkm009 && po.ComponentType == ComponentType.DrawerBottom ||
                            po.CustomerOrderCode == dkm009 && po.ComponentType == ComponentType.DrawerSide);
                                                        
                if(cncDKM009 != null)
                {   
                    foreach(var cnc009 in cncDKM009)
                    {           
                        var prodItemCnc009 = prodItemsRep.GetFirstOrDefault(
                            pi => pi.ProductionOrderCode == cnc009.Code);
                        
                        if(prodItemCnc009 != null)
                        {
                            var prodItemsStepsDataCnc009 = prodItemsStepsDataRep.GetFirstOrDefault(
                                pisd => pisd.ProductionOrderCode == cnc009.Code && pisd.ProductionStepCode == cncStepCode);
                            
                            if(prodItemsStepsDataCnc009 != null)
                            {
                                prodItemCnc009.InsertFeedbackFinishedGood(unitOfWork, _TaskName, cncWorkCenterCode, "", 1, _Logger);
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
            _Logger.Error(ResourcesKeys.ErrorInUserExit("SetFeedbackRedBulk"), null, e);
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
