-- Masterview for tile "Optimizationlot Progress"
-- The schema name has to be 'cust'
-- The view name has to start with the prefix 'View'
CREATE VIEW[cust].[ViewMasterProgressOptimizations]
AS
Select  
ISNULL(ROW_NUMBER() over (Order by op.OptimizationCode),-1) as RowID
,op.OptimizationCode

-- OrderCompleted: OverallPercentage = 100.00 without Preassembly, Quality Control and Assembly
,Case when
(
Select  
	CAST(ROUND(CAST(SUM(ps.CurrentTargetQuantity) as decimal(6,2))/CAST(SUM(ps.DesiredTargetQuantity) as decimal(6,2))*100,2) as decimal(6,2))
From
	base.ProductionSteps ps
	LEFT OUTER JOIN base.ProductionOrders po on po.Code = ps.ProductionOrderCode
	LEFT OUTER JOIN base.OptimizationParts opc on opc.ProductionOrderCode = po.Code
Where 
	opc.OptimizationCode = op.OptimizationCode
	and
	ps.WorkCenterCode in ('CU1','EB1','CNC1','CNC2','SP')
	and
	ps.DisposeState = 0
Group by 
	opc.OptimizationCode
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
	LEFT OUTER JOIN base.OptimizationParts opc on opc.ProductionOrderCode = po.Code
Where 
	opc.OptimizationCode = op.OptimizationCode
	and
	ps.WorkCenterCode in ('CU1','EB1','CNC1','CNC2','SP')
	and
	ps.DisposeState = 0
Group by 
	opc.OptimizationCode
)
As OverallPercentage,
-- Quantity
(
Select 
	CAST(SUM(ps.CurrentTargetQuantity) as nvarchar) + '/' + CAST(SUM(ps.DesiredTargetQuantity) as nvarchar)
From 
	base.ProductionSteps ps
	LEFT OUTER JOIN base.ProductionOrders po on po.Code = ps.ProductionOrderCode
	LEFT OUTER JOIN base.OptimizationParts opc on opc.ProductionOrderCode = po.Code
Where 
	opc.OptimizationCode = op.OptimizationCode
	and
	ps.WorkCenterCode in ('CU1','EB1','CNC1','CNC2','SP')
	and
	ps.DisposeState = 0
Group by 
	opc.OptimizationCode
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
	LEFT OUTER JOIN base.OptimizationParts opc on opc.ProductionOrderCode = po.Code
Where 
	opc.OptimizationCode = op.OptimizationCode
	and
	ps.WorkCenterCode in ('CU1','EB1','CNC1','CNC2','SP')
	and
	po.ReproductionType IN (1)
	and
	ps.DisposeState = 0
Group by 
	opc.OptimizationCode
)
As ReworkPercentage,
-- Quantity
(
Select 
	CAST(SUM(ps.CurrentTargetQuantity) as nvarchar) + '/' + CAST(SUM(ps.DesiredTargetQuantity) as nvarchar)
From 
	base.ProductionSteps ps
	LEFT OUTER JOIN base.ProductionOrders po on po.Code = ps.ProductionOrderCode
	LEFT OUTER JOIN base.OptimizationParts opc on opc.ProductionOrderCode = po.Code
Where 
	opc.OptimizationCode = op.OptimizationCode
	and
	ps.WorkCenterCode in ('CU1','EB1','CNC1','CNC2','SP')
	and
	po.ReproductionType IN (1)
	and
	ps.DisposeState = 0
Group by 
	opc.OptimizationCode
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
	LEFT OUTER JOIN base.OptimizationParts opc on opc.ProductionOrderCode = po.Code
Where 
	opc.OptimizationCode = op.OptimizationCode
	and
	ps.WorkCenterCode = 'CU1'
	and
	ps.DisposeState = 0
Group by 
	opc.OptimizationCode
)
As CU1Percentage,
-- Quantity
(
Select 
	CAST(SUM(ps.CurrentTargetQuantity) as nvarchar) + '/' + CAST(SUM(ps.DesiredTargetQuantity) as nvarchar)
From 
	base.ProductionSteps ps
	LEFT OUTER JOIN base.ProductionOrders po on po.Code = ps.ProductionOrderCode
	LEFT OUTER JOIN base.OptimizationParts opc on opc.ProductionOrderCode = po.Code
Where 
	opc.OptimizationCode = op.OptimizationCode
	and
	ps.WorkCenterCode = 'CU1'
	and
	ps.DisposeState = 0
Group by 
	opc.OptimizationCode
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
	LEFT OUTER JOIN base.OptimizationParts opc on opc.ProductionOrderCode = po.Code
Where 
	opc.OptimizationCode = op.OptimizationCode
	and
	ps.WorkCenterCode = 'EB1'
	and
	ps.DisposeState = 0
Group by 
	opc.OptimizationCode
)
As EB1Percentage,
-- Quantity
(
Select 
	CAST(SUM(ps.CurrentTargetQuantity) as nvarchar) + '/' + CAST(SUM(ps.DesiredTargetQuantity) as nvarchar)
From 
	base.ProductionSteps ps
	LEFT OUTER JOIN base.ProductionOrders po on po.Code = ps.ProductionOrderCode
	LEFT OUTER JOIN base.OptimizationParts opc on opc.ProductionOrderCode = po.Code
Where 
	opc.OptimizationCode = op.OptimizationCode
	and
	ps.WorkCenterCode = 'EB1'
	and
	ps.DisposeState = 0
Group by 
	opc.OptimizationCode
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
	LEFT OUTER JOIN base.OptimizationParts opc on opc.ProductionOrderCode = po.Code
Where 
	opc.OptimizationCode = op.OptimizationCode
	and
	ps.WorkCenterCode = 'CNC1'
	and
	ps.DisposeState = 0
Group by 
	opc.OptimizationCode
)
As CNC1Percentage,
-- Quantity
(
Select 
	CAST(SUM(ps.CurrentTargetQuantity) as nvarchar) + '/' + CAST(SUM(ps.DesiredTargetQuantity) as nvarchar)
From 
	base.ProductionSteps ps
	LEFT OUTER JOIN base.ProductionOrders po on po.Code = ps.ProductionOrderCode
	LEFT OUTER JOIN base.OptimizationParts opc on opc.ProductionOrderCode = po.Code
Where 
	opc.OptimizationCode = op.OptimizationCode
	and
	ps.WorkCenterCode = 'CNC1'
	and
	ps.DisposeState = 0
Group by 
	opc.OptimizationCode
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
	LEFT OUTER JOIN base.OptimizationParts opc on opc.ProductionOrderCode = po.Code
Where 
	opc.OptimizationCode = op.OptimizationCode
	and
	ps.WorkCenterCode = 'CNC2'
	and
	ps.DisposeState = 0
Group by 
	opc.OptimizationCode
)
As CNC2Percentage,
-- Quantity
(
Select 
	CAST(SUM(ps.CurrentTargetQuantity) as nvarchar) + '/' + CAST(SUM(ps.DesiredTargetQuantity) as nvarchar)
From 
	base.ProductionSteps ps
	LEFT OUTER JOIN base.ProductionOrders po on po.Code = ps.ProductionOrderCode
	LEFT OUTER JOIN base.OptimizationParts opc on opc.ProductionOrderCode = po.Code
Where 
	opc.OptimizationCode = op.OptimizationCode
	and
	ps.WorkCenterCode = 'CNC2'
	and
	ps.DisposeState = 0
Group by 
	opc.OptimizationCode
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
	LEFT OUTER JOIN base.OptimizationParts opc on opc.ProductionOrderCode = po.Code
Where 
	opc.OptimizationCode = op.OptimizationCode
	and
	ps.WorkCenterCode = 'SP'
	and
	ps.DisposeState = 0
Group by 
	opc.OptimizationCode
)
As SPPercentage,
-- Quantity
(
Select 
	CAST(SUM(ps.CurrentTargetQuantity) as nvarchar) + '/' + CAST(SUM(ps.DesiredTargetQuantity) as nvarchar)
From 
	base.ProductionSteps ps
	LEFT OUTER JOIN base.ProductionOrders po on po.Code = ps.ProductionOrderCode
	LEFT OUTER JOIN base.OptimizationParts opc on opc.ProductionOrderCode = po.Code
Where 
	opc.OptimizationCode = op.OptimizationCode
	and
	ps.WorkCenterCode = 'SP'
	and
	ps.DisposeState = 0
Group by 
	opc.OptimizationCode
) 
As SPQty


From base.ProductionOrders po
LEFT OUTER JOIN base.OptimizationParts op on op.ProductionOrderCode = po.Code


Where 
op.OptimizationCode Is not null


Group by
op.OptimizationCode

