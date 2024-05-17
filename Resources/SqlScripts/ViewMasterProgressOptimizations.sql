-- Masterview for tile "Optimizationlot Progress"
-- The schema name has to be 'cust'
-- The view name has to start with the prefix 'View'
CREATE VIEW[cust].[ViewMasterProgressOptimizations]
AS
Select  
ISNULL(ROW_NUMBER() over (Order by op.OptimizationCode),-1) as RowID
,op.OptimizationCode

-- OrderCompleted: OverallPercentage = 100.00 without Preassembly and Assembly
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
	ps.WorkCenterCode in ('1010','3010','5010','5020','5070')
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
	ps.WorkCenterCode in ('1010','3010','5010','5020','5070')
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
	ps.WorkCenterCode in ('1010','3010','5010','5020','5070')
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
	ps.WorkCenterCode in ('1010','3010','5010','5020','5070')
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
	ps.WorkCenterCode in ('1010','3010','5010','5020','5070')
	and
	po.ReproductionType IN (1)
	and
	ps.DisposeState = 0
Group by 
	opc.OptimizationCode
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
	LEFT OUTER JOIN base.OptimizationParts opc on opc.ProductionOrderCode = po.Code
Where 
	opc.OptimizationCode = op.OptimizationCode
	and
	ps.WorkCenterCode = '1010'
	and
	ps.DisposeState = 0
Group by 
	opc.OptimizationCode
)
As B300Percentage,
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
	ps.WorkCenterCode = '1010'
	and
	ps.DisposeState = 0
Group by 
	opc.OptimizationCode
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
	LEFT OUTER JOIN base.OptimizationParts opc on opc.ProductionOrderCode = po.Code
Where 
	opc.OptimizationCode = op.OptimizationCode
	and
	ps.WorkCenterCode = '3010'
	and
	ps.DisposeState = 0
Group by 
	opc.OptimizationCode
)
As EDGETEQPercentage,
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
	ps.WorkCenterCode = '3010'
	and
	ps.DisposeState = 0
Group by 
	opc.OptimizationCode
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
	LEFT OUTER JOIN base.OptimizationParts opc on opc.ProductionOrderCode = po.Code
Where 
	opc.OptimizationCode = op.OptimizationCode
	and
	ps.WorkCenterCode = '5010'
	and
	ps.DisposeState = 0
Group by 
	opc.OptimizationCode
)
As V200Percentage,
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
	ps.WorkCenterCode = '5010'
	and
	ps.DisposeState = 0
Group by 
	opc.OptimizationCode
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
	LEFT OUTER JOIN base.OptimizationParts opc on opc.ProductionOrderCode = po.Code
Where 
	opc.OptimizationCode = op.OptimizationCode
	and
	ps.WorkCenterCode = '5020'
	and
	ps.DisposeState = 0
Group by 
	opc.OptimizationCode
)
As E310Percentage,
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
	ps.WorkCenterCode = '5020'
	and
	ps.DisposeState = 0
Group by 
	opc.OptimizationCode
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
	LEFT OUTER JOIN base.OptimizationParts opc on opc.ProductionOrderCode = po.Code
Where 
	opc.OptimizationCode = op.OptimizationCode
	and
	ps.WorkCenterCode = '5070'
	and
	ps.DisposeState = 0
Group by 
	opc.OptimizationCode
)
As SORTPercentage,
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
	ps.WorkCenterCode = '5070'
	and
	ps.DisposeState = 0
Group by 
	opc.OptimizationCode
) 
As SORTQty


From base.ProductionOrders po
LEFT OUTER JOIN base.OptimizationParts op on op.ProductionOrderCode = po.Code


Where 
op.OptimizationCode Is not null


Group by
op.OptimizationCode

