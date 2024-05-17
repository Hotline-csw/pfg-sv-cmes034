-- Masterview for tile "Bulk Progress"
-- The schema name has to be 'cust'
-- The view name has to start with the prefix 'View'
CREATE VIEW[cust].[ViewMasterProgressBulks]
AS
Select  
ISNULL(ROW_NUMBER() over (Order by mb.PlanningNumber),-1) As RowID
,mb.PlanningNumber
,Format(mb.EndDate, 'yyyy-MM-dd') As EndDate
,CAST(DATEPART(Week,mb.EndDate) As Nvarchar) + '/' + CAST(DATEPART(Year,mb.EndDate) as Nvarchar) As PlannedEndWeek

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
	ps.DisposeState = 0
Group by 
	mbb.PlanningNumber
)
>= (Select ValueFloat From base.ProgramSettings Where Identifier = 'CompletedOrAllowed') 

then 1 
else 0 end
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
	po.ReproductionType IN (1)
	and
	ps.DisposeState = 0
Group by 
	mbb.PlanningNumber
) 
As ReworkQty,

-- B300
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
	ps.WorkCenterCode = '1010'
	and
	ps.DisposeState = 0
Group by 
	mbb.PlanningNumber
)
As B300Percentage,
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
	ps.WorkCenterCode = '1010'
	and
	ps.DisposeState = 0
Group by 
	mbb.PlanningNumber
) 
As B300Qty,

-- EDGETEQ
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
	ps.WorkCenterCode = '3010'
	and
	ps.DisposeState = 0
Group by 
	mbb.PlanningNumber
)
As EDGETEQPercentage,
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
	ps.WorkCenterCode = '3010'
	and
	ps.DisposeState = 0
Group by 
	mbb.PlanningNumber
) 
As EDGETEQQty,

-- V200
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
	ps.WorkCenterCode = '5010'
	and
	ps.DisposeState = 0
Group by 
	mbb.PlanningNumber
)
As V200Percentage,
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
	ps.WorkCenterCode = '5010'
	and
	ps.DisposeState = 0
Group by 
	mbb.PlanningNumber
) 
As V200Qty,

-- E310
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
	ps.WorkCenterCode = '5020'
	and
	ps.DisposeState = 0
Group by 
	mbb.PlanningNumber
)
As E310Percentage,
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
	ps.WorkCenterCode = '5020'
	and
	ps.DisposeState = 0
Group by 
	mbb.PlanningNumber
) 
As E310Qty,

-- SORTING
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
	ps.WorkCenterCode = '5070'
	and
	ps.DisposeState = 0
Group by 
	mbb.PlanningNumber
)
As SORTPercentage,
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
	ps.WorkCenterCode = '5070'
	and
	ps.DisposeState = 0
Group by 
	mbb.PlanningNumber
) 
As SORTQty,

-- PREASSEMBLY
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
	ps.WorkCenterCode = '6010'
	and
	ps.DisposeState = 0
	and
	po.OrderType = '3'
Group by 
	mbb.PlanningNumber
)
As PREASSPercentage,
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
	ps.WorkCenterCode = '6010'
	and
	ps.DisposeState = 0
	and
	po.OrderType = '3'
Group by 
	mbb.PlanningNumber
) 
As PREASSQty,

-- ASSEMBLY
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
	ps.WorkCenterCode = '6010'
	and
	ps.DisposeState = 0
	and
	po.OrderType = '1'
Group by 
	mbb.PlanningNumber
)
As ASSPercentage,
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
	ps.WorkCenterCode = '6010'
	and
	ps.DisposeState = 0
	and
	po.OrderType = '1'
Group by 
	mbb.PlanningNumber
) 
As ASSQty


From base.ProductionOrders po
LEFT OUTER JOIN base.CustomerOrders co on po.CustomerOrderCode = co.Code     
LEFT OUTER JOIN base.ProductionOrders ts on ts.code = po.TopProductionOrderNumber   
LEFT OUTER JOIN base.ManualBulks mb on mb.Sequence = ts.PlanningSequence


Where PlanningNumber is not null


Group by
mb.PlanningNumber
,mb.EndDate

