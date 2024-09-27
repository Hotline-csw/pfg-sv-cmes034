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

-- CU1 (SAWTEQ B-300)
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
	ps.WorkCenterCode = 'CU1'
	and
	ps.DisposeState = 0
Group by 
	mbb.PlanningNumber
)
As CU1Percentage,
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
	ps.WorkCenterCode = 'CU1'
	and
	ps.DisposeState = 0
Group by 
	mbb.PlanningNumber
) 
As CU1Qty,

-- EB1 (EDGETEQ S-810)
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
	ps.WorkCenterCode = 'EB1'
	and
	ps.DisposeState = 0
Group by 
	mbb.PlanningNumber
)
As EB1Percentage,
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
	ps.WorkCenterCode = 'EB1'
	and
	ps.DisposeState = 0
Group by 
	mbb.PlanningNumber
) 
As EB1Qty,

-- CNC1 (DRILLTEQ V-200)
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
	ps.WorkCenterCode = 'CNC1'
	and
	ps.DisposeState = 0
Group by 
	mbb.PlanningNumber
)
As CNC1Percentage,
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
	ps.WorkCenterCode = 'CNC1'
	and
	ps.DisposeState = 0
Group by 
	mbb.PlanningNumber
) 
As CNC1Qty,

-- CNC2 (CENTATEQ E-310)
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
	ps.WorkCenterCode = 'CNC2'
	and
	ps.DisposeState = 0
Group by 
	mbb.PlanningNumber
)
As CNC2Percentage,
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
	ps.WorkCenterCode = 'CNC2'
	and
	ps.DisposeState = 0
Group by 
	mbb.PlanningNumber
) 
As CNC2Qty,

-- SP (Manual sorting and picking)
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
	ps.WorkCenterCode = 'SP'
	and
	ps.DisposeState = 0
Group by 
	mbb.PlanningNumber
)
As SPPercentage,
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
	ps.WorkCenterCode = 'SP'
	and
	ps.DisposeState = 0
Group by 
	mbb.PlanningNumber
) 
As SPQty,

-- PRE (Manual pre-assembly of parts)
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
	ps.WorkCenterCode = 'PRE'
	and
	ps.DisposeState = 0
Group by 
	mbb.PlanningNumber
)
As PREPercentage,
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
	ps.WorkCenterCode = 'PRE'
	and
	ps.DisposeState = 0
Group by 
	mbb.PlanningNumber
) 
As PREQty,

-- QC (Manual quality control)
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
	ps.WorkCenterCode = 'QC'
	and
	ps.DisposeState = 0
Group by 
	mbb.PlanningNumber
)
As QCPercentage,
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
	ps.WorkCenterCode = 'QC'
	and
	ps.DisposeState = 0
Group by 
	mbb.PlanningNumber
) 
As QCQty,

-- AS1 (Manual assembly of furnitures)
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
	ps.WorkCenterCode = 'AS1'
	and
	ps.DisposeState = 0
Group by 
	mbb.PlanningNumber
)
As AS1Percentage,
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
	ps.WorkCenterCode = 'AS1'
	and
	ps.DisposeState = 0
Group by 
	mbb.PlanningNumber
) 
As AS1Qty


From base.ProductionOrders po
LEFT OUTER JOIN base.CustomerOrders co on po.CustomerOrderCode = co.Code     
LEFT OUTER JOIN base.ProductionOrders ts on ts.code = po.TopProductionOrderNumber   
LEFT OUTER JOIN base.ManualBulks mb on mb.Sequence = ts.PlanningSequence


Where PlanningNumber is not null


Group by
mb.PlanningNumber
,mb.EndDate

