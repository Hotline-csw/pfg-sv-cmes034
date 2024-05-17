-- Masterview for tile "Assembly Manager Industry"
-- The schema name has to be 'cust'
-- The view name has to start with the prefix 'View'
CREATE VIEW[cust].[ViewMasterProgressComponentTypeBulks]
AS
Select  
ISNULL(ROW_NUMBER() over (Order by mb.PlanningNumber),-1) as RowID
,mb.PlanningNumber
,Format(mb.EndDate, 'yyyy-MM-dd') As EndDate
,CAST(DATEPART(Week,mb.EndDate) As Nvarchar) + '/' + CAST(DATEPART(Year,mb.EndDate) as Nvarchar) As PlannedEndWeek

-- AssemblyAllowed: OverallPercentage = 100.00
,Case when
(
Select  
	CAST(ROUND(CAST(SUM(ps.CurrentTargetQuantity) as decimal(6,2))/CAST(SUM(ps.DesiredTargetQuantity) as decimal(6,2))*100,2) as decimal(6,2))
From
	base.ProductionSteps ps
	LEFT OUTER JOIN base.ProductionOrders po on po.Code = ps.ProductionOrderCode
	LEFT OUTER JOIN base.ProductionOrders ts on ts.code = po.TopProductionOrderNumber   
	LEFT OUTER JOIN base.ManualBulks mbb on mbb.Sequence = ts.PlanningSequence
Where 
	mbb.PlanningNumber = mb.PlanningNumber
	and
	ps.WorkCenterCode in ('1010','3010','5010','5020','5070')
	and
	ps.DisposeState = 0
Group by 
	mbb.PlanningNumber
)
>= (Select ValueFloat From base.ProgramSettings Where Identifier = 'CompletedOrAllowed')

Then 1 
Else 0 End
As CompletedOrAllowed,


-- Overall
-- Percentage
(
Select  
	CAST(ROUND(CAST(SUM(ps.CurrentTargetQuantity) as decimal(6,2))/CAST(SUM(ps.DesiredTargetQuantity) as decimal(6,2))*100,2) as decimal(6,2))
From
	base.ProductionSteps ps
	LEFT OUTER JOIN base.ProductionOrders po on po.Code = ps.ProductionOrderCode
	LEFT OUTER JOIN base.ProductionOrders ts on ts.code = po.TopProductionOrderNumber   
	LEFT OUTER JOIN base.ManualBulks mbb on mbb.Sequence = ts.PlanningSequence
Where 
	mbb.PlanningNumber = mb.PlanningNumber
	and
	ps.WorkCenterCode in ('1010','3010','5010','5020','5070')
	and
	ps.DisposeState = 0
Group by 
	mbb.PlanningNumber
)
As OverallPercentage,
-- Quantity
(
Select 
	CAST(SUM(ps.CurrentTargetQuantity) as nvarchar) + '/' + CAST(SUM(ps.DesiredTargetQuantity) as nvarchar)
From 
	base.ProductionSteps ps
	LEFT OUTER JOIN base.ProductionOrders po on po.Code = ps.ProductionOrderCode
	LEFT OUTER JOIN base.ProductionOrders ts on ts.code = po.TopProductionOrderNumber   
	LEFT OUTER JOIN base.ManualBulks mbb on mbb.Sequence = ts.PlanningSequence
Where 
	mbb.PlanningNumber = mb.PlanningNumber
	and
	ps.WorkCenterCode in ('1010','3010','5010','5020','5070')
	and
	ps.DisposeState = 0
Group by 
	mbb.PlanningNumber
) 
As OverallQty,

-- Rework
-- Percentage
(
Select  
	CAST(ROUND(CAST(SUM(ps.CurrentTargetQuantity) as decimal(6,2))/CAST(SUM(ps.DesiredTargetQuantity) as decimal(6,2))*100,2) as decimal(6,2))
From
	base.ProductionSteps ps
	LEFT OUTER JOIN base.ProductionOrders po on po.Code = ps.ProductionOrderCode
	LEFT OUTER JOIN base.ProductionOrders ts on ts.code = po.TopProductionOrderNumber   
	LEFT OUTER JOIN base.ManualBulks mbb on mbb.Sequence = ts.PlanningSequence
Where 
	mbb.PlanningNumber = mb.PlanningNumber
	and
	ps.WorkCenterCode in ('1010','3010','5010','5020','5070')
	and
	po.ReproductionType IN (1)
	and
	ps.DisposeState = 0
Group by 
	mbb.PlanningNumber
)
As ReworkPercentage,
-- Quantity
(
Select 
	CAST(SUM(ps.CurrentTargetQuantity) as nvarchar) + '/' + CAST(SUM(ps.DesiredTargetQuantity) as nvarchar)
From 
	base.ProductionSteps ps
	LEFT OUTER JOIN base.ProductionOrders po on po.Code = ps.ProductionOrderCode
	LEFT OUTER JOIN base.ProductionOrders ts on ts.code = po.TopProductionOrderNumber   
	LEFT OUTER JOIN base.ManualBulks mbb on mbb.Sequence = ts.PlanningSequence
Where 
	mbb.PlanningNumber = mb.PlanningNumber
	and
	ps.WorkCenterCode in ('1010','3010','5010','5020','5070')
	and
	po.ReproductionType IN (1)
	and
	ps.DisposeState = 0
Group by 
	mbb.PlanningNumber
) 
As ReworkQty,

-- Front Parts
-- Percentage
(
Select  
	CAST(ROUND(CAST(SUM(ps.CurrentTargetQuantity) as decimal(6,2))/CAST(SUM(ps.DesiredTargetQuantity) as decimal(6,2))*100,2) as decimal(6,2))
From
	base.ProductionSteps ps
	LEFT OUTER JOIN base.ProductionOrders po on po.Code = ps.ProductionOrderCode
	LEFT OUTER JOIN base.ProductionOrders ts on ts.code = po.TopProductionOrderNumber   
	LEFT OUTER JOIN base.ManualBulks mbb on mbb.Sequence = ts.PlanningSequence
Where 
	mbb.PlanningNumber = mb.PlanningNumber
	and
	ps.WorkCenterCode in ('1010','3010','5010','5020','5070')
	and
	po.ComponentType in (6,7,8,9) -- Filler, Door, DoorLeft, DoorRight
	and
	ps.DisposeState = 0
Group by 
	mbb.PlanningNumber
)
As FrontPercentage,
-- Quantity
(
Select 
	CAST(SUM(ps.CurrentTargetQuantity) as nvarchar) + '/' + CAST(SUM(ps.DesiredTargetQuantity) as nvarchar)
From 
	base.ProductionSteps ps
	LEFT OUTER JOIN base.ProductionOrders po on po.Code = ps.ProductionOrderCode
	LEFT OUTER JOIN base.ProductionOrders ts on ts.code = po.TopProductionOrderNumber   
	LEFT OUTER JOIN base.ManualBulks mbb on mbb.Sequence = ts.PlanningSequence
Where 
	mbb.PlanningNumber = mb.PlanningNumber
	and
	ps.WorkCenterCode in ('1010','3010','5010','5020','5070')
	and
	po.ComponentType in (6,7,8,9) -- Filler, Door, DoorLeft, DoorRight
	and
	ps.DisposeState = 0
Group by 
	mbb.PlanningNumber
) 
As FrontQty,

-- Vertical Parts
-- Percentage
(
Select  
	CAST(ROUND(CAST(SUM(ps.CurrentTargetQuantity) as decimal(6,2))/CAST(SUM(ps.DesiredTargetQuantity) as decimal(6,2))*100,2) as decimal(6,2))
From
	base.ProductionSteps ps
	LEFT OUTER JOIN base.ProductionOrders po on po.Code = ps.ProductionOrderCode
	LEFT OUTER JOIN base.ProductionOrders ts on ts.code = po.TopProductionOrderNumber   
	LEFT OUTER JOIN base.ManualBulks mbb on mbb.Sequence = ts.PlanningSequence
Where 
	mbb.PlanningNumber = mb.PlanningNumber
	and
	ps.WorkCenterCode in ('1010','3010','5010','5020','5070')
	and
	po.ComponentType in (1,18,19) -- SidePanel, DrawerSide, DrawerFront
	and
	ps.DisposeState = 0
Group by 
	mbb.PlanningNumber
)
As VerticalPercentage,
-- Quantity
(
Select 
	CAST(SUM(ps.CurrentTargetQuantity) as nvarchar) + '/' + CAST(SUM(ps.DesiredTargetQuantity) as nvarchar)
From 
	base.ProductionSteps ps
	LEFT OUTER JOIN base.ProductionOrders po on po.Code = ps.ProductionOrderCode
	LEFT OUTER JOIN base.ProductionOrders ts on ts.code = po.TopProductionOrderNumber   
	LEFT OUTER JOIN base.ManualBulks mbb on mbb.Sequence = ts.PlanningSequence
Where 
	mbb.PlanningNumber = mb.PlanningNumber
	and
	ps.WorkCenterCode in ('1010','3010','5010','5020','5070')
	and
	po.ComponentType in (1,18,19) -- SidePanel, DrawerSide, DrawerFront
	and
	ps.DisposeState = 0
Group by 
	mbb.PlanningNumber
) 
As VerticalQty,

-- Horizontal Parts
-- Percentage
(
Select  
	CAST(ROUND(CAST(SUM(ps.CurrentTargetQuantity) as decimal(6,2))/CAST(SUM(ps.DesiredTargetQuantity) as decimal(6,2))*100,2) as decimal(6,2))
From
	base.ProductionSteps ps
	LEFT OUTER JOIN base.ProductionOrders po on po.Code = ps.ProductionOrderCode
	LEFT OUTER JOIN base.ProductionOrders ts on ts.code = po.TopProductionOrderNumber   
	LEFT OUTER JOIN base.ManualBulks mbb on mbb.Sequence = ts.PlanningSequence
Where 
	mbb.PlanningNumber = mb.PlanningNumber
	and
	ps.WorkCenterCode in ('1010','3010','5010','5020','5070')
	and
	po.ComponentType in (2,3,4,5,12,13,17,20,21,24) -- AdjustableShelf, TopShelf, BottomShelf, BackPanel, FixedShelf, Partition, DrawerBottom, Toekick, Worktop, Traverse
	and
	ps.DisposeState = 0
Group by 
	mbb.PlanningNumber
)
As HorizontalPercentage,
-- Quantity
(
Select 
	CAST(SUM(ps.CurrentTargetQuantity) as nvarchar) + '/' + CAST(SUM(ps.DesiredTargetQuantity) as nvarchar)
From 
	base.ProductionSteps ps
	LEFT OUTER JOIN base.ProductionOrders po on po.Code = ps.ProductionOrderCode
	LEFT OUTER JOIN base.ProductionOrders ts on ts.code = po.TopProductionOrderNumber   
	LEFT OUTER JOIN base.ManualBulks mbb on mbb.Sequence = ts.PlanningSequence
Where 
	mbb.PlanningNumber = mb.PlanningNumber
	and
	ps.WorkCenterCode in ('1010','3010','5010','5020','5070')
	and
	po.ComponentType in (2,3,4,5,12,13,17,20,21,24) -- AdjustableShelf, TopShelf, BottomShelf, BackPanel, FixedShelf, Partition, DrawerBottom, Toekick, Worktop, Traverse
	and
	ps.DisposeState = 0
Group by 
	mbb.PlanningNumber
) 
As HorizontalQty


From base.ProductionOrders po   
LEFT OUTER JOIN base.ProductionOrders ts on ts.Code = po.TopProductionOrderNumber   
LEFT OUTER JOIN base.ManualBulks mb on mb.Sequence = ts.PlanningSequence


Where mb.PlanningNumber is not null


Group by
mb.PlanningNumber
,mb.EndDate

