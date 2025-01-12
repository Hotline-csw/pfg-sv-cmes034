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

-- CU1 (SAWTEQ B-300)
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
	ps.WorkCenterCode = 'CU1'
	and
	ps.DisposeState = 0
Group by 
	po.CustomerOrderCode
)
As CU1Percentage,
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
	ps.WorkCenterCode = 'CU1'
	and
	ps.DisposeState = 0
Group by 
	po.CustomerOrderCode
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
Where 
	po.CustomerOrderCode = co.Code
	and
	ps.WorkCenterCode = 'EB1'
	and
	ps.DisposeState = 0
Group by 
	po.CustomerOrderCode
)
As EB1Percentage,
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
	ps.WorkCenterCode = 'EB1'
	and
	ps.DisposeState = 0
Group by 
	po.CustomerOrderCode
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
Where 
	po.CustomerOrderCode = co.Code
	and
	ps.WorkCenterCode = 'CNC1'
	and
	ps.DisposeState = 0
Group by 
	po.CustomerOrderCode
)
As CNC1Percentage,
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
	ps.WorkCenterCode = 'CNC1'
	and
	ps.DisposeState = 0
Group by 
	po.CustomerOrderCode
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
Where 
	po.CustomerOrderCode = co.Code
	and
	ps.WorkCenterCode = 'CNC2'
	and
	ps.DisposeState = 0
Group by 
	po.CustomerOrderCode
)
As CNC2Percentage,
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
	ps.WorkCenterCode = 'CNC2'
	and
	ps.DisposeState = 0
Group by 
	po.CustomerOrderCode
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
Where 
	po.CustomerOrderCode = co.Code
	and
	ps.WorkCenterCode = 'SP'
	and
	ps.DisposeState = 0
Group by 
	po.CustomerOrderCode
)
As SPPercentage,
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
	ps.WorkCenterCode = 'SP'
	and
	ps.DisposeState = 0
Group by 
	po.CustomerOrderCode
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
Where 
	po.CustomerOrderCode = co.Code
	and
	ps.WorkCenterCode = 'PRE'
	and
	ps.DisposeState = 0
Group by 
	po.CustomerOrderCode
)
As PREPercentage,
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
	ps.WorkCenterCode = 'PRE'
	and
	ps.DisposeState = 0
Group by 
	po.CustomerOrderCode
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
Where 
	po.CustomerOrderCode = co.Code
	and
	ps.WorkCenterCode = 'QC'
	and
	ps.DisposeState = 0
Group by 
	po.CustomerOrderCode
)
As QCPercentage,
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
	ps.WorkCenterCode = 'QC'
	and
	ps.DisposeState = 0
Group by 
	po.CustomerOrderCode
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
Where 
	po.CustomerOrderCode = co.Code
	and
	ps.WorkCenterCode = 'AS1'
	and
	ps.DisposeState = 0
Group by 
	po.CustomerOrderCode
)
As AS1Percentage,
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
	ps.WorkCenterCode = 'AS1'
	and
	ps.DisposeState = 0
Group by 
	po.CustomerOrderCode
) 
As AS1Qty


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

