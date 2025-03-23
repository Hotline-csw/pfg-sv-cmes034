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
//   T.Stürzer       2024-11-15    Changed Workcenters for feedback
//   T.Stürzer       2025-03-23    Added WriteFeedback
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
                    
                var edgeDKS006 = prodOrdersRep.GetQueryable(false).Where(
                    po => 
                        po.CustomerOrderCode == dks006 && 
                        po.ReproductionType == ReproductionType.NoReproduction &&
                        componentTypesDKS006Edge.Contains(po.ComponentType)).ToList();
                                                        
                if(edgeDKS006.Any())
                {   
                    foreach(var edge006 in edgeDKS006)
                    {           
                        var prodItemEdge006 = prodItemsRep.GetFirstOrDefault(pi => pi.ProductionOrderCode == edge006.Code);
                        
                        var prodItemsStepsDataEdge006 = prodItemsStepsDataRep.GetFirstOrDefault(
                            pisd => pisd.ProductionOrderCode == edge006.Code && pisd.ProductionStepCode == edgeStepCode);
                                
                        if(prodItemEdge006 != null && prodItemsStepsDataEdge006 != null)
                        {
                            //prodItemEdge006.InsertFeedback(unitOfWork, _TaskName, 1, 0, 0, edgeWorkCenterCode, edgeStepCode, 0, FeedbackState.Finished, 0, _Logger);
                            WriteFeedback(unitOfWork, prodItemEdge006.Code, edgeStepCode, edgeWorkCenterCode, _Logger);
                        }
                    }
                }
                
                // CNC 310                           
                var componentTypesDKS006Cnc = new List<ComponentType>
                {
                    ComponentType.Door,     ComponentType.DrawerBottom,     ComponentType.DrawerSide
                };
                
                var cncDKS006 = prodOrdersRep.GetQueryable(false).Where(
                    po => 
                        po.CustomerOrderCode == dks006 &&
                        po.ReproductionType == ReproductionType.NoReproduction &&
                        componentTypesDKS006Cnc.Contains(po.ComponentType)).ToList();
                                                        
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
                            //prodItemCnc006.InsertFeedback(unitOfWork, _TaskName, 1, 0, 0, cncWorkCenterCode, cncStepCode, 0, FeedbackState.Finished, 0, _Logger);
                            WriteFeedback(unitOfWork, prodItemCnc006.Code, cncStepCode, cncWorkCenterCode, _Logger);
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
                
                var sortDKS006 = prodOrdersRep.GetQueryable(false).Where(
                    po => po.CustomerOrderCode == dks006 && componentTypesDKS006Sorting.Contains(po.ComponentType)).ToList();
                                                        
                if(sortDKS006.Any())
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
                var componentTypesDKM006Cutting = new List<ComponentType>
                {
                    ComponentType.TopShelf,
                };
                
                var cutDKM006 = prodOrdersRep.GetQueryable(false).Where(
                        po => po.CustomerOrderCode == dkm006 && componentTypesDKM006Cutting.Contains(po.ComponentType) && 
                              po.ReproductionType == ReproductionType.Standard).ToList();
                                                        
                if(cutDKM006.Any())
                {   
                    foreach(var cut006 in cutDKM006)
                    {           
                        var prodItemCut006 = prodItemsRep.GetFirstOrDefault(pi => pi.ProductionOrderCode == cut006.Code);
                        
                        var prodItemsStepsDataCut006 = prodItemsStepsDataRep.GetFirstOrDefault(
                            pisd => pisd.ProductionOrderCode == cut006.Code && pisd.ProductionStepCode == cuttingStepCode);                        
                        
                        if(prodItemCut006 != null && prodItemsStepsDataCut006 != null)
                        {
                            prodItemCut006.InsertFeedback(unitOfWork, _TaskName, 1, 0, 0, cuttingWorkCenterCode, cuttingStepCode, 0, FeedbackState.Finished, 0, _Logger);
                        }
                    }
                }                
                
                // Edgebanding S810
                var componentTypesDKM006Edge = new List<ComponentType>
                {
                    ComponentType.AdjustableShelf,      ComponentType.Panel,        ComponentType.Door,     ComponentType.DoorLeft,
                    ComponentType.FixedShelf
                };
                
                var edgeDKM006 = prodOrdersRep.GetQueryable(false).Where(
                    po => po.CustomerOrderCode == dkm006 && componentTypesDKM006Edge.Contains(po.ComponentType)).ToList();
                                                        
                if(edgeDKM006.Any())
                {   
                    foreach(var edge006 in edgeDKM006)
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
                
                // Drilling V200
                var componentTypesDKM006Drill = new List<ComponentType>
                {
                    ComponentType.SidePanel,        ComponentType.BottomShelf,      ComponentType.BackPanel,        ComponentType.WorkTop,
                    ComponentType.Traverse
                };
                
                var drillDKM006 = prodOrdersRep.GetQueryable(false).Where(
                    po => po.CustomerOrderCode == dkm006 && componentTypesDKM006Drill.Contains(po.ComponentType)).ToList();
                                                        
                if(drillDKM006.Any())
                {   
                    foreach(var drill006 in drillDKM006)
                    {           
                        var prodItemDrill006 = prodItemsRep.GetFirstOrDefault(
                            pi => pi.ProductionOrderCode == drill006.Code);
                            
                        var prodItemsStepsDataDrill006 = prodItemsStepsDataRep.GetFirstOrDefault(
                            pisd => pisd.ProductionOrderCode == drill006.Code && pisd.ProductionStepCode == drillingStepCode);
                        
                        if(prodItemDrill006 != null && prodItemsStepsDataDrill006 != null)
                        {
                            prodItemDrill006.InsertFeedback(unitOfWork, _TaskName, 1, 0, 0, drillingWorkCenterCode, drillingStepCode, 0, FeedbackState.Finished, 0, _Logger);
                        }
                    }
                }
				
				// CNC 310
                var componentTypesDKM006CNC = new List<ComponentType>
                {
                    ComponentType.DoorRight,        ComponentType.DrawerBottom,     ComponentType.DrawerSide
                };
                
                var cncDKM006 = prodOrdersRep.GetQueryable(false).Where(
                    po => po.CustomerOrderCode == dkm006 && componentTypesDKM006CNC.Contains(po.ComponentType) &&
                          po.ReproductionType == ReproductionType.NoReproduction).ToList();
                                                        
                if(cncDKM006.Any())
                {   
                    foreach(var cnc006 in cncDKM006)
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
                var componentTypesDKM006Sorting = new List<ComponentType>
                {
                    ComponentType.TopShelf,     ComponentType.DrawerFront,      ComponentType.Partition,        ComponentType.Plinth
                };
                
                var sortDKM006 = prodOrdersRep.GetQueryable(false).Where(
                    po => po.CustomerOrderCode == dkm006 && componentTypesDKM006Sorting.Contains(po.ComponentType) &&
                          po.ReproductionType == ReproductionType.NoReproduction).ToList();
                                                        
                if(sortDKM006.Any())
                {   
                    foreach(var sort006 in sortDKM006)
                    {           
                        var prodItemSort006 = prodItemsRep.GetFirstOrDefault(pi => pi.ProductionOrderCode == sort006.Code);
                        
                        var prodItemsStepsDataSort006 = prodItemsStepsDataRep.GetFirstOrDefault(
                                pisd => pisd.ProductionOrderCode == sort006.Code && pisd.ProductionStepCode == sortingStepCode);
                                
                        if(prodItemSort006 != null && prodItemsStepsDataSort006 != null)
                        {
                            prodItemSort006.InsertFeedback(unitOfWork, _TaskName, 1, 0, 0, sortingWorkCenterCode, sortingStepCode, 0, FeedbackState.Finished, 0, _Logger);
                        }
                    }
                }
				
//-----------------------------------------------------------------------------
                
                // Demo_Kitchen_Medium_008
                // Edgebanding S810
                var componentTypesDKM008Edge = new List<ComponentType>
                {
                    ComponentType.SidePanel,        ComponentType.BottomShelf,      ComponentType.Panel,        ComponentType.DrawerSide,
                    ComponentType.DrawerFront
                };
                
                var edgeDKM008 = prodOrdersRep.GetQueryable(false).Where(
                    po => po.CustomerOrderCode == dkm008 && componentTypesDKM008Edge.Contains(po.ComponentType)).ToList();
                                                        
                if(edgeDKM008.Any())
                {   
                    foreach(var edge008 in edgeDKM008)
                    {           
                        var prodItemEdge008 = prodItemsRep.GetFirstOrDefault(pi => pi.ProductionOrderCode == edge008.Code);
                        
                        var prodItemsStepsDataEdge008 = prodItemsStepsDataRep.GetFirstOrDefault(
                                pisd => pisd.ProductionOrderCode == edge008.Code && pisd.ProductionStepCode == edgeStepCode);
                                
                        if(prodItemEdge008 != null && prodItemsStepsDataEdge008 != null)
                        {
                            prodItemEdge008.InsertFeedback(unitOfWork, _TaskName, 1, 0, 0, edgeWorkCenterCode, edgeStepCode, 0, FeedbackState.Finished, 0, _Logger);
                        }
                    }
                }
                
                // Drilling V200
                var componentTypesDKM008Drill = new List<ComponentType>
                {
                    ComponentType.AdjustableShelf,      ComponentType.TopShelf,     ComponentType.BackPanel,        ComponentType.FixedShelf,
                    ComponentType.Partition,            ComponentType.Plinth,       ComponentType.WorkTop,          ComponentType.Traverse
                };
                
                var drillDKM008 = prodOrdersRep.GetQueryable(false).Where(
                    po => po.CustomerOrderCode == dkm008 && componentTypesDKM008Drill.Contains(po.ComponentType)).ToList();
                                                        
                if(drillDKM008.Any())
                {   
                    foreach(var drill008 in drillDKM008)
                    {           
                        var prodItemDrill008 = prodItemsRep.GetFirstOrDefault(
                            pi => pi.ProductionOrderCode == drill008.Code);
                            
                        var prodItemsStepsDataDrill008 = prodItemsStepsDataRep.GetFirstOrDefault(
                                pisd => pisd.ProductionOrderCode == drill008.Code && pisd.ProductionStepCode == drillingStepCode);
                                
                        if(prodItemDrill008 != null && prodItemsStepsDataDrill008 != null)
                        {
                            prodItemDrill008.InsertFeedback(unitOfWork, _TaskName, 1, 0, 0, drillingWorkCenterCode, drillingStepCode, 0, FeedbackState.Finished, 0, _Logger);
                        }
                    }
                }
				
				// CNC 310
                var componentTypesDKM008CNC = new List<ComponentType>
                {
                    ComponentType.Door,     ComponentType.DoorLeft,     ComponentType.DoorRight,        ComponentType.DrawerBottom
                };
                
                var cncDKM008 = prodOrdersRep.GetQueryable(false).Where(
                    po => po.CustomerOrderCode == dkm008 && componentTypesDKM008CNC.Contains(po.ComponentType)).ToList();
                                                        
                if(cncDKM008 != null)
                {   
                    foreach(var cnc008 in cncDKM008)
                    {           
                        var prodItemCnc008 = prodItemsRep.GetFirstOrDefault(
                            pi => pi.ProductionOrderCode == cnc008.Code);
                            
                        var prodItemsStepsDataCnc008 = prodItemsStepsDataRep.GetFirstOrDefault(
                                pisd => pisd.ProductionOrderCode == cnc008.Code && pisd.ProductionStepCode == cncStepCode);
                                
                        if(prodItemCnc008 != null && prodItemsStepsDataCnc008 != null)
                        {
                            prodItemCnc008.InsertFeedback(unitOfWork, _TaskName, 1, 0, 0, cncWorkCenterCode, cncStepCode, 0, FeedbackState.Finished, 0, _Logger);
                        }
                    }
                }
				
//-----------------------------------------------------------------------------
                
                // Demo_Kitchen_Medium_009
                // Edgebanding S810
                var componentTypesDKM009Edge = new List<ComponentType>
                {
                    ComponentType.AdjustableShelf,      ComponentType.Panel,        ComponentType.Door,         ComponentType.DoorRight,
                    ComponentType.FixedShelf,           ComponentType.Partition,    ComponentType.DrawerFront,  ComponentType.WorkTop
                };
                
                var edgeDKM009 = prodOrdersRep.GetQueryable(false).Where(
                    po => po.CustomerOrderCode == dkm009 && componentTypesDKM009Edge.Contains(po.ComponentType)).ToList();
                                                        
                if(edgeDKM009.Any())
                {   
                    foreach(var edge009 in edgeDKM009)
                    {           
                        var prodItemEdge009 = prodItemsRep.GetFirstOrDefault(pi => pi.ProductionOrderCode == edge009.Code);
                        
                        var prodItemsStepsDataEdge009 = prodItemsStepsDataRep.GetFirstOrDefault(
                                pisd => pisd.ProductionOrderCode == edge009.Code && pisd.ProductionStepCode == edgeStepCode);
                                
                        if(prodItemEdge009 != null && prodItemsStepsDataEdge009 != null)
                        {
                            prodItemEdge009.InsertFeedback(unitOfWork, _TaskName, 1, 0, 0, edgeWorkCenterCode, edgeStepCode, 0, FeedbackState.Finished, 0, _Logger);
                        }
                    }
                }
                
                // Drilling V200
                var componentTypesDKM009Drill = new List<ComponentType>
                {
                    ComponentType.SidePanel,        ComponentType.TopShelf,     ComponentType.BottomShelf,      ComponentType.BackPanel,
                    ComponentType.Plinth,           ComponentType.Traverse
                };
                
                var drillDKM009 = prodOrdersRep.GetQueryable(false).Where(
                    po => po.CustomerOrderCode == dkm009 && componentTypesDKM009Drill.Contains(po.ComponentType)).ToList();
                                                        
                if(drillDKM009.Any())
                {   
                    foreach(var drill009 in drillDKM009)
                    {           
                        var prodItemDrill009 = prodItemsRep.GetFirstOrDefault(
                            pi => pi.ProductionOrderCode == drill009.Code);
                            
                        var prodItemsStepsDataDrill009 = prodItemsStepsDataRep.GetFirstOrDefault(
                                pisd => pisd.ProductionOrderCode == drill009.Code && pisd.ProductionStepCode == drillingStepCode);
                                
                        if(prodItemDrill009 != null && prodItemsStepsDataDrill009 != null)
                        {
                            prodItemDrill009.InsertFeedback(unitOfWork, _TaskName, 1, 0, 0, drillingWorkCenterCode, drillingStepCode, 0, FeedbackState.Finished, 0, _Logger);
                        }
                    }
                }			
				
				// CNC 310                            
                var componentTypesDKM009CNC = new List<ComponentType>
                {
                    ComponentType.DoorLeft,     ComponentType.DrawerBottom,     ComponentType.DrawerSide
                };
                
                var cncDKM009 = prodOrdersRep.GetQueryable(false).Where(
                    po => po.CustomerOrderCode == dkm009 && componentTypesDKM009CNC.Contains(po.ComponentType)).ToList();
                                                        
                if(cncDKM009.Any())
                {   
                    foreach(var cnc009 in cncDKM009)
                    {           
                        var prodItemCnc009 = prodItemsRep.GetFirstOrDefault(
                            pi => pi.ProductionOrderCode == cnc009.Code);
                            
                        var prodItemsStepsDataCnc009 = prodItemsStepsDataRep.GetFirstOrDefault(
                                pisd => pisd.ProductionOrderCode == cnc009.Code && pisd.ProductionStepCode == cncStepCode);
                                
                        if(prodItemCnc009 != null && prodItemsStepsDataCnc009 != null)
                        {
                            prodItemCnc009.InsertFeedback(unitOfWork, _TaskName, 1, 0, 0, cncWorkCenterCode, cncStepCode, 0, FeedbackState.Finished, 0, _Logger);
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
    
    
    public void WriteFeedback(IUnitOfWork unitOfWork, string prodItemCode, string prodStepCode, string workCenterCode, Logger _Logger)
    {
        try
        {
            var feedback = new Feedback();
                                    
            feedback.ProductionItemCode = prodItemCode;
            feedback.ProductionStepCode = prodStepCode;
            feedback.WorkcenterCode = workCenterCode;
            feedback.CountGood = 1;
            feedback.CountScrap = 0;
            feedback.CountRework = 0;
            feedback.Timestamp = DateTime.Now;
            feedback.FeedbackState = FeedbackState.Finished;
            feedback.ProcessingState = 0;
            feedback.ProcessingTime = 0;
            feedback.CreationDate = DateTime.Now;
            feedback.CreationSource = "SetFeedbackRedBulk";
            feedback.ModificationDate = DateTime.Now;
            feedback.ModificationSource = "SetFeedbackRedBulk";
            
            unitOfWork.AddOrUpdate(new[] { feedback } );
        }
        catch (Exception e)
        {
            _Logger.Error(ResourcesKeys.ErrorInUserExit(Name), null, e);
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
