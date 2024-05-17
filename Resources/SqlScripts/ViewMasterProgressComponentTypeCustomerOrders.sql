-- Masterview for tile "Assembly Manager"
-- The schema name has to be 'cust'
-- The view name has to start with the prefix 'View'
CREATE VIEW[cust].[ViewMasterProgressComponentTypeCustomerOrders]
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


-- AssemblyAllowed: OverallPercentage = 100.00
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
	ps.WorkCenterCode in ('1010','3010','5010','5020','5070')
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
	ps.WorkCenterCode in ('1010','3010','5010','5020','5070')
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
	ps.WorkCenterCode in ('1010','3010','5010','5020','5070')
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
	ps.WorkCenterCode in ('1010','3010','5010','5020','5070')
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
	ps.WorkCenterCode in ('1010','3010','5010','5020','5070')
	and
	po.ReproductionType IN (1)
	and
	ps.DisposeState = 0
Group by 
	po.CustomerOrderCode
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
Where 
	po.CustomerOrderCode = co.Code
	and
	ps.WorkCenterCode in ('1010','3010','5010','5020','5070')
	and
	po.ComponentType in (6,7,8,9) -- Filler, Door, DoorLeft, DoorRight
	and
	ps.DisposeState = 0
Group by 
	po.CustomerOrderCode
)
As FrontPercentage,
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
	ps.WorkCenterCode in ('1010','3010','5010','5020','5070')
	and
	po.ComponentType in (6,7,8,9) -- Filler, Door, DoorLeft, DoorRight
	and
	ps.DisposeState = 0
Group by 
	po.CustomerOrderCode
) 
As FrontQty,

-- Carcase Parts
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
	ps.WorkCenterCode in ('1010','3010','5010','5020','5070')
	and
	po.ComponentType in (1,2,3,4,5,12,13,20,21,24) -- SidePanel, AdjustableShelf, TopShelf, BottomShelf, BackPanel, FixedShelf, Partition, Toekick, Worktop, Traverse
	and
	ps.DisposeState = 0
Group by 
	po.CustomerOrderCode
)
As CarcasePercentage,
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
	ps.WorkCenterCode in ('1010','3010','5010','5020','5070')
	and
	po.ComponentType in (1,2,3,4,5,12,13,20,21,24) -- SidePanel, AdjustableShelf, TopShelf, BottomShelf, BackPanel, FixedShelf, Partition, Toekick, Worktop, Traverse
	and
	ps.DisposeState = 0
Group by 
	po.CustomerOrderCode
) 
As CarcaseQty,

-- Drawer Parts
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
	ps.WorkCenterCode in ('1010','3010','5010','5020','5070')
	and
	po.ComponentType in (17,18,19) -- DrawerBottom, DrawerSide, DrawerFront
	and
	ps.DisposeState = 0
Group by 
	po.CustomerOrderCode
)
As DrawerPercentage,
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
	ps.WorkCenterCode in ('1010','3010','5010','5020','5070')
	and
	po.ComponentType in (17,18,19) -- DrawerBottom, DrawerSide, DrawerFront
	and
	ps.DisposeState = 0
Group by 
	po.CustomerOrderCode
) 
As DrawerQty,

-- Long Parts
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
	ps.WorkCenterCode in ('1010','3010','5010','5020','5070')
	and
	po.ComponentType in (20,21) -- Toekick, Worktop
	and
	ps.DisposeState = 0
Group by 
	po.CustomerOrderCode
)
As LongPercentage,
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
	ps.WorkCenterCode in ('1010','3010','5010','5020','5070')
	and
	po.ComponentType in (20,21) -- Toekick, Worktop
	and
	ps.DisposeState = 0
Group by 
	po.CustomerOrderCode
) 
As LongQty,

-- Vertical Parts
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
	ps.WorkCenterCode in ('1010','3010','5010','5020','5070')
	and
	po.ComponentType in (1) -- SidePanel
	and
	ps.DisposeState = 0
Group by 
	po.CustomerOrderCode
)
As VerticalPercentage,
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
	ps.WorkCenterCode in ('1010','3010','5010','5020','5070')
	and
	po.ComponentType in (1) -- SidePanel
	and
	ps.DisposeState = 0
Group by 
	po.CustomerOrderCode
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
Where 
	po.CustomerOrderCode = co.Code
	and
	ps.WorkCenterCode in ('1010','3010','5010','5020','5070')
	and
	po.ComponentType in (2,3,4,5,12,13,24) -- AdjustableShelf, TopShelf, BottomShelf, BackPanel, FixedShelf, Partition, Traverse
	and
	ps.DisposeState = 0
Group by 
	po.CustomerOrderCode
)
As HorizontalPercentage,
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
	ps.WorkCenterCode in ('1010','3010','5010','5020','5070')
	and
	po.ComponentType in (2,3,4,5,12,13,24) -- AdjustableShelf, TopShelf, BottomShelf, BackPanel, FixedShelf, Partition, Traverse
	and
	ps.DisposeState = 0
Group by 
	po.CustomerOrderCode
) 
As HorizontalQty


From base.ProductionOrders po
LEFT OUTER JOIN base.CustomerOrders co on po.CustomerOrderCode = co.Code     
LEFT OUTER JOIN base.ProductionOrders ts on ts.Code = po.TopProductionOrderNumber   
LEFT OUTER JOIN base.ManualBulks mb on mb.Sequence = ts.PlanningSequence


Group by
co.Code
,mb.PlanningNumber
,co.CustomPlannedEndDate
,co.ShippingDate
,co.DeliveryDate

