#r ControllerMES.Infrastructure.Common
#r ControllerMES.Infrastructure.Resource


//-----------------------------------------------------------------------------
//   (Class-)Name:   CustomCreatePostProduction
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         T.Stürzer
//   Date:           2023-03-02
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   T.Stürzer       2023-03-02    Created
//   
//-----------------------------------------------------------------------------


using System.ComponentModel;


[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IServerCustomization))]
[Export("CustomCreatePostProduction", typeof(HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[Description("06_Create post-production automatically")]
[EnabledScript(true)]
public class CustomCreatePostProduction : GenericTaskBase, HomagGroup.FLS.Services.Common.Contracts.JobScheduling.Configuration.Tasks.Generic.IGenericTask
{
    [Import]
    private IUnitOfWorkFactory _UnitOfWorkFactory;

    private Logger _Logger;
    
    private string _TaskName = "CustomCreatePostProduction";
    
    // Small Kitchens
    private string dks003 = "Demo_Kitchen_Small_003";
    private string dks005 = "Demo_Kitchen_Small_005";
    
    // Medium Kitchens
    private string dkm001 = "Demo_Kitchen_Medium_001";
    private string dkm006 = "Demo_Kitchen_Medium_006";
    

    public override void Execute(IJobExecutionContext executionContext)
    {
        Guard.ThrowOnArgumentNull(executionContext, "executionContext");

        try
        {
            _Logger = LogHelper.GetLogger(executionContext.Area, typeof(CustomCreatePostProduction));
            _Logger.LogContext.AddOrUpdate(LogHelper.InstanceNameKey, executionContext.Instance);

            using(var unitOfWork = _UnitOfWorkFactory.CreateUnitOfWork())
            {
                var productionOrdersRep = unitOfWork.GetRepository<ProductionOrder>();
                var productionItemsRep = unitOfWork.GetRepository<ProductionItem>();
               
                // Demo_Kitchen_Small_003
                // TopShelf
                var prodOrderDks003TopShelf = productionOrdersRep.GetFirstOrDefault(po => po.CustomerOrderCode == dks003 && po.ComponentType == ComponentType.TopShelf);
                var prodItemDks003TopShelf = productionItemsRep.GetFirstOrDefault(pi => pi.ProductionOrderCode == prodOrderDks003TopShelf.Code);
                
                if(prodOrderDks003TopShelf != null && prodItemDks003TopShelf != null)
                {
                    var postProd = new ProductionItemsValidation();
                    
                    postProd.Code = "1";
                    postProd.ProductionOrderCode = prodOrderDks003TopShelf.Code;
                    postProd.ProductionItemCode = prodItemDks003TopShelf.Code;
                    postProd.ReproductionMode = ReproductionMode.UnchangedPlan;
                    postProd.DesiredQuantity = 1;
                    postProd.ValidationStateSourceCode = "1010";
                    postProd.ValidationStateDetailCode = "1001";
                    postProd.ValidationState = ValidationState.ReproductionNecessary;
                    postProd.ReproductionState = ReproductionState.New;
                    postProd.NecessaryReleaseType = NecessaryReleaseType.WithoutRelease;
                    postProd.CreationSource = "UserExit";
                    postProd.ModificationSource = "UserExit";
                    
                    unitOfWork.AddOrUpdate(new[] {postProd});
                }

                // DoorLeft
                var prodOrderDks003DoorLeft = productionOrdersRep.GetFirstOrDefault(po => po.CustomerOrderCode == dks003 && po.ComponentType == ComponentType.DoorLeft);
                var prodItemDks003DoorLeft = productionItemsRep.GetFirstOrDefault(pi => pi.ProductionOrderCode == prodOrderDks003DoorLeft.Code);
                
                if(prodOrderDks003DoorLeft != null && prodItemDks003DoorLeft != null)
                {
                    var postProd = new ProductionItemsValidation();
                    
                    postProd.Code = "1";
                    postProd.ProductionOrderCode = prodOrderDks003DoorLeft.Code;
                    postProd.ProductionItemCode = prodItemDks003DoorLeft.Code;
                    postProd.ReproductionMode = ReproductionMode.UnchangedPlan;
                    postProd.DesiredQuantity = 1;
                    postProd.ValidationStateSourceCode = "1010";
                    postProd.ValidationStateDetailCode = "1001";
                    postProd.ValidationState = ValidationState.ReproductionNecessary;
                    postProd.ReproductionState = ReproductionState.New;
                    postProd.NecessaryReleaseType = NecessaryReleaseType.WithoutRelease;
                    postProd.CreationSource = "UserExit";
                    postProd.ModificationSource = "UserExit";
                    
                    unitOfWork.AddOrUpdate(new[] {postProd});
                }


                // Demo_Kitchen_Small_005
                // SidePanel
                var prodOrderDks005SidePanel = productionOrdersRep.GetFirstOrDefault(po => po.CustomerOrderCode == dks005 && po.ComponentType == ComponentType.SidePanel);
                var prodItemDks005SidePanel = productionItemsRep.GetFirstOrDefault(pi => pi.ProductionOrderCode == prodOrderDks005SidePanel.Code);
                
                if(prodOrderDks005SidePanel != null && prodItemDks005SidePanel != null)
                {
                    var postProd = new ProductionItemsValidation();
                    
                    postProd.Code = "1";
                    postProd.ProductionOrderCode = prodOrderDks005SidePanel.Code;
                    postProd.ProductionItemCode = prodItemDks005SidePanel.Code;
                    postProd.ReproductionMode = ReproductionMode.UnchangedPlan;
                    postProd.DesiredQuantity = 1;
                    postProd.ValidationStateSourceCode = "1010";
                    postProd.ValidationStateDetailCode = "1001";
                    postProd.ValidationState = ValidationState.ReproductionNecessary;
                    postProd.ReproductionState = ReproductionState.New;
                    postProd.NecessaryReleaseType = NecessaryReleaseType.WithoutRelease;
                    postProd.CreationSource = "UserExit";
                    postProd.ModificationSource = "UserExit";
                    
                    unitOfWork.AddOrUpdate(new[] {postProd});
                }

                
                // Demo_Kitchen_Medium_001
                // TopShelf
                var prodOrderDkm001TopShelf = productionOrdersRep.GetFirstOrDefault(po => po.CustomerOrderCode == dkm001 && po.ComponentType == ComponentType.TopShelf);
                var prodItemDkm001TopShelf = productionItemsRep.GetFirstOrDefault(pi => pi.ProductionOrderCode == prodOrderDkm001TopShelf.Code);
                                
                if(prodOrderDkm001TopShelf != null && prodItemDkm001TopShelf != null)
                {
                var postProd = new ProductionItemsValidation();
                
                postProd.Code = "1";
                postProd.ProductionOrderCode = prodOrderDkm001TopShelf.Code;
                postProd.ProductionItemCode = prodItemDkm001TopShelf.Code;
                postProd.ReproductionMode = ReproductionMode.UnchangedPlan;
                postProd.DesiredQuantity = 1;
                postProd.ValidationStateSourceCode = "1010";
                postProd.ValidationStateDetailCode = "1001";
                postProd.ValidationState = ValidationState.ReproductionNecessary;
                postProd.ReproductionState = ReproductionState.New;
                postProd.NecessaryReleaseType = NecessaryReleaseType.WithoutRelease;
                postProd.CreationSource = "UserExit";
                postProd.ModificationSource = "UserExit";
                
                unitOfWork.AddOrUpdate(new[] {postProd});
                }
                
                // DrawerFront
                var prodOrderDkm001DrawerFront = productionOrdersRep.GetFirstOrDefault(po => po.CustomerOrderCode == dkm001 && po.ComponentType == ComponentType.DrawerFront);
                var prodItemDkm001DrawerFront = productionItemsRep.GetFirstOrDefault(pi => pi.ProductionOrderCode == prodOrderDkm001DrawerFront.Code);
                                
                if(prodOrderDkm001DrawerFront != null && prodItemDkm001DrawerFront != null)
                {
                var postProd = new ProductionItemsValidation();
                
                postProd.Code = "1";
                postProd.ProductionOrderCode = prodOrderDkm001DrawerFront.Code;
                postProd.ProductionItemCode = prodItemDkm001DrawerFront.Code;
                postProd.ReproductionMode = ReproductionMode.UnchangedPlan;
                postProd.DesiredQuantity = 1;
                postProd.ValidationStateSourceCode = "1010";
                postProd.ValidationStateDetailCode = "1001";
                postProd.ValidationState = ValidationState.ReproductionNecessary;
                postProd.ReproductionState = ReproductionState.New;
                postProd.NecessaryReleaseType = NecessaryReleaseType.WithoutRelease;
                postProd.CreationSource = "UserExit";
                postProd.ModificationSource = "UserExit";
                
                unitOfWork.AddOrUpdate(new[] {postProd});
                }
                
                // BackPanel
                var prodOrderDkm001BackPanel = productionOrdersRep.GetFirstOrDefault(po => po.CustomerOrderCode == dkm001 && po.ComponentType == ComponentType.BackPanel);
                var prodItemDkm001BackPanel = productionItemsRep.GetFirstOrDefault(pi => pi.ProductionOrderCode == prodOrderDkm001BackPanel.Code);
                                
                if(prodOrderDkm001BackPanel != null && prodItemDkm001BackPanel != null)
                {
                var postProd = new ProductionItemsValidation();
                
                postProd.Code = "1";
                postProd.ProductionOrderCode = prodOrderDkm001BackPanel.Code;
                postProd.ProductionItemCode = prodItemDkm001BackPanel.Code;
                postProd.ReproductionMode = ReproductionMode.UnchangedPlan;
                postProd.DesiredQuantity = 1;
                postProd.ValidationStateSourceCode = "1010";
                postProd.ValidationStateDetailCode = "1001";
                postProd.ValidationState = ValidationState.ReproductionNecessary;
                postProd.ReproductionState = ReproductionState.New;
                postProd.NecessaryReleaseType = NecessaryReleaseType.WithoutRelease;
                postProd.CreationSource = "UserExit";
                postProd.ModificationSource = "UserExit";
                
                unitOfWork.AddOrUpdate(new[] {postProd});
                }
                
                
                // Demo_Kitchen_Medium_006
                // TopShelf
                var prodOrderDkm006TopShelf = productionOrdersRep.GetFirstOrDefault(po => po.CustomerOrderCode == dkm006 && po.ComponentType == ComponentType.TopShelf);
                var prodItemDkm006TopShelf = productionItemsRep.GetFirstOrDefault(pi => pi.ProductionOrderCode == prodOrderDkm006TopShelf.Code);
                                
                if(prodOrderDkm006TopShelf != null && prodItemDkm006TopShelf != null)
                {
                var postProd = new ProductionItemsValidation();
                
                postProd.Code = "1";
                postProd.ProductionOrderCode = prodOrderDkm006TopShelf.Code;
                postProd.ProductionItemCode = prodItemDkm006TopShelf.Code;
                postProd.ReproductionMode = ReproductionMode.UnchangedPlan;
                postProd.DesiredQuantity = 1;
                postProd.ValidationStateSourceCode = "1010";
                postProd.ValidationStateDetailCode = "1001";
                postProd.ValidationState = ValidationState.ReproductionNecessary;
                postProd.ReproductionState = ReproductionState.New;
                postProd.NecessaryReleaseType = NecessaryReleaseType.WithoutRelease;
                postProd.CreationSource = "UserExit";
                postProd.ModificationSource = "UserExit";
                
                unitOfWork.AddOrUpdate(new[] {postProd});
                }
                
                // DoorRight
                var prodOrderDkm006DoorRight = productionOrdersRep.GetFirstOrDefault(po => po.CustomerOrderCode == dkm006 && po.ComponentType == ComponentType.DoorRight);
                var prodItemDkm006DoorRight = productionItemsRep.GetFirstOrDefault(pi => pi.ProductionOrderCode == prodOrderDkm006DoorRight.Code);
                                
                if(prodOrderDkm006DoorRight != null && prodItemDkm006DoorRight != null)
                {
                var postProd = new ProductionItemsValidation();
                
                postProd.Code = "1";
                postProd.ProductionOrderCode = prodOrderDkm006DoorRight.Code;
                postProd.ProductionItemCode = prodItemDkm006DoorRight.Code;
                postProd.ReproductionMode = ReproductionMode.DataModification;
                postProd.DesiredQuantity = 1;
                postProd.ValidationStateSourceCode = "5020";
                postProd.ValidationStateDetailCode = "5001";
                postProd.ValidationState = ValidationState.ReproductionNecessary;
                postProd.ReproductionState = ReproductionState.New;
                postProd.NecessaryReleaseType = NecessaryReleaseType.ReleaseExplicit;
                postProd.CreationSource = "UserExit";
                postProd.ModificationSource = "UserExit";
                
                unitOfWork.AddOrUpdate(new[] {postProd});
                }
                
                unitOfWork.Save();                
            }

        }
        catch (Exception e)
        {
            executionContext.JobResultSetComplete(JobResult.Failed);
            _Logger.Error(ResourcesKeys.ErrorInUserExit("CustomCreatePostProduction"), null, e);
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
