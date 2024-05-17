-- Masterview for tile "Customer Order Progress"
-- The schema name has to be 'cust'
-- The view name has to start with the prefix 'View'
CREATE VIEW[cust].[ViewMasterProgressWorkCenterCustomerOrders]
AS
Select  
ISNULL(ROW_NUMBER() over (Order by co.Code),-1) As RowID
,mb.PlanningNumber
,co.Code As CustomerOrderCode
,Format(co.CustomPlannedEndDate, 'yyyy-MM-dd') As PlannedEndDate
,CAST(DATEPART(Week,co.CustomPlannedEndDate) As Nvarchar) + '/' + CAST(DATEPART(Year,co.CustomPlannedEndDate) as Nvarchar) As PlannedEndWeek
,Format(co.ShippingDate, 'yyyy-MM-dd') As ShippingDate
,CAST(DATEPART(Week,co.ShippingDate) As Nvarchar) + '/' + CAST(DATEPART(Year,co.ShippingDate) as Nvarchar) As ShippingWeek
,Format(co.DeliveryDate, 'yyyy-MM-dd') As DeliveryDate
,CAST(DATEPART(Week,co.DeliveryDate) As Nvarchar) + '/' + CAST(DATEPART(Year,co.DeliveryDate) as Nvarchar) As DeliveryWeek


-- OrderCompleted: OverallPercentage = 100.00
,Case when
(
Select  
	CAST(ROUND(CAST(SUM(ps.CurrentTargetQuantity) as decimal(6,2))/CAST(SUM(ps.DesiredTargetQuantity) as decimal(6,2))*100,2) as decimal(6,2))
From
	base.ProductionSteps ps
	LEFT OUTER JOIN base.ProductionOrders po on po.Code = ps.ProductionOrderCode
Where 
	po.CustomerOrderCode = co.Code
	and
	ps.DisposeState = 0
Group by 
	po.CustomerOrderCode
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
Where 
	po.CustomerOrderCode = co.Code
	and
	ps.DisposeState = 0
Group by 
	po.CustomerOrderCode
)
As OverallPercentage,
-- Quantity
(
Select 
	CAST(SUM(ps.CurrentTargetQuantity) as nvarchar) + '/' + CAST(SUM(ps.DesiredTargetQuantity) as nvarchar)
From 
	base.ProductionSteps ps
	LEFT OUTER JOIN base.ProductionOrders po on po.Code = ps.ProductionOrderCode
Where 
	po.CustomerOrderCode = co.Code
	and
	ps.DisposeState = 0
Group by 
	po.CustomerOrderCode
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
Where 
	po.CustomerOrderCode = co.Code
	and
	po.ReproductionType IN (1)
	and
	ps.DisposeState = 0
Group by 
	po.CustomerOrderCode
)
As ReworkPercentage,
-- Quantity
(
Select 
	CAST(SUM(ps.CurrentTargetQuantity) as nvarchar) + '/' + CAST(SUM(ps.DesiredTargetQuantity) as nvarchar)
From 
	base.ProductionSteps ps
	LEFT OUTER JOIN base.ProductionOrders po on po.Code = ps.ProductionOrderCode
Where 
	po.CustomerOrderCode = co.Code
	and
	po.ReproductionType IN (1)
	and
	ps.DisposeState = 0
Group by 
	po.CustomerOrderCode
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
Where 
	po.CustomerOrderCode = co.Code
	and
	ps.WorkCenterCode = '1010'
	and
	ps.DisposeState = 0
Group by 
	po.CustomerOrderCode
)
As B300Percentage,
-- Quantity
(
Select 
	CAST(SUM(ps.CurrentTargetQuantity) as nvarchar) + '/' + CAST(SUM(ps.DesiredTargetQuantity) as nvarchar)
From 
	base.ProductionSteps ps
	LEFT OUTER JOIN base.ProductionOrders po on po.Code = ps.ProductionOrderCode
Where 
	po.CustomerOrderCode = co.Code
	and
	ps.WorkCenterCode = '1010'
	and
	ps.DisposeState = 0
Group by 
	po.CustomerOrderCode
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
Where 
	po.CustomerOrderCode = co.Code
	and
	ps.WorkCenterCode = '3010'
	and
	ps.DisposeState = 0
Group by 
	po.CustomerOrderCode
)
As EDGETEQPercentage,
-- Quantity
(
Select 
	CAST(SUM(ps.CurrentTargetQuantity) as nvarchar) + '/' + CAST(SUM(ps.DesiredTargetQuantity) as nvarchar)
From 
	base.ProductionSteps ps
	LEFT OUTER JOIN base.ProductionOrders po on po.Code = ps.ProductionOrderCode
Where 
	po.CustomerOrderCode = co.Code
	and
	ps.WorkCenterCode = '3010'
	and
	ps.DisposeState = 0
Group by 
	po.CustomerOrderCode
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
Where 
	po.CustomerOrderCode = co.Code
	and
	ps.WorkCenterCode = '5010'
	and
	ps.DisposeState = 0
Group by 
	po.CustomerOrderCode
)
As V200Percentage,
-- Quantity
(
Select 
	CAST(SUM(ps.CurrentTargetQuantity) as nvarchar) + '/' + CAST(SUM(ps.DesiredTargetQuantity) as nvarchar)
From 
	base.ProductionSteps ps
	LEFT OUTER JOIN base.ProductionOrders po on po.Code = ps.ProductionOrderCode
Where 
	po.CustomerOrderCode = co.Code
	and
	ps.WorkCenterCode = '5010'
	and
	ps.DisposeState = 0
Group by 
	po.CustomerOrderCode
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
Where 
	po.CustomerOrderCode = co.Code
	and
	ps.WorkCenterCode = '5020'
	and
	ps.DisposeState = 0
Group by 
	po.CustomerOrderCode
)
As E310Percentage,
-- Quantity
(
Select 
	CAST(SUM(ps.CurrentTargetQuantity) as nvarchar) + '/' + CAST(SUM(ps.DesiredTargetQuantity) as nvarchar)
From 
	base.ProductionSteps ps
	LEFT OUTER JOIN base.ProductionOrders po on po.Code = ps.ProductionOrderCode
Where 
	po.CustomerOrderCode = co.Code
	and
	ps.WorkCenterCode = '5020'
	and
	ps.DisposeState = 0
Group by 
	po.CustomerOrderCode
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
Where 
	po.CustomerOrderCode = co.Code
	and
	ps.WorkCenterCode = '5070'
	and
	ps.DisposeState = 0
Group by 
	po.CustomerOrderCode
)
As SORTPercentage,
-- Quantity
(
Select 
	CAST(SUM(ps.CurrentTargetQuantity) as nvarchar) + '/' + CAST(SUM(ps.DesiredTargetQuantity) as nvarchar)
From 
	base.ProductionSteps ps
	LEFT OUTER JOIN base.ProductionOrders po on po.Code = ps.ProductionOrderCode
Where 
	po.CustomerOrderCode = co.Code
	and
	ps.WorkCenterCode = '5070'
	and
	ps.DisposeState = 0
Group by 
	po.CustomerOrderCode
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
Where 
	po.CustomerOrderCode = co.Code
	and
	ps.WorkCenterCode = '6010'
	and
	ps.DisposeState = 0
	and
	po.OrderType = '3'
Group by 
	po.CustomerOrderCode
)
As PREASSPercentage,
-- Quantity
(
Select 
	CAST(SUM(ps.CurrentTargetQuantity) as nvarchar) + '/' + CAST(SUM(ps.DesiredTargetQuantity) as nvarchar)
From 
	base.ProductionSteps ps
	LEFT OUTER JOIN base.ProductionOrders po on po.Code = ps.ProductionOrderCode
Where 
	po.CustomerOrderCode = co.Code
	and
	ps.WorkCenterCode = '6010'
	and
	ps.DisposeState = 0
	and
	po.OrderType = '3'
Group by 
	po.CustomerOrderCode
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
Where 
	po.CustomerOrderCode = co.Code
	and
	ps.WorkCenterCode = '6010'
	and
	ps.DisposeState = 0
	and
	po.OrderType = '1'
Group by 
	po.CustomerOrderCode
)
As ASSPercentage,
-- Quantity
(
Select 
	CAST(SUM(ps.CurrentTargetQuantity) as nvarchar) + '/' + CAST(SUM(ps.DesiredTargetQuantity) as nvarchar)
From 
	base.ProductionSteps ps
	LEFT OUTER JOIN base.ProductionOrders po on po.Code = ps.ProductionOrderCode
Where 
	po.CustomerOrderCode = co.Code
	and
	ps.WorkCenterCode = '6010'
	and
	ps.DisposeState = 0
	and
	po.OrderType = '1'
Group by 
	po.CustomerOrderCode
) 
As ASSQty


From base.ProductionOrders po
LEFT OUTER JOIN base.CustomerOrders co on po.CustomerOrderCode = co.Code     
LEFT OUTER JOIN base.ProductionOrders ts on ts.code = po.TopProductionOrderNumber   
LEFT OUTER JOIN base.ManualBulks mb on mb.Sequence = ts.PlanningSequence


Group by
co.Code
,co.CustomPlannedEndDate
,co.ShippingDate
,co.DeliveryDate
,mb.PlanningNumber

