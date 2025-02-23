-- Masterview for tile "Unfinished Parts"
-- The schema name has to be 'cust'
-- The view name has to start with the prefix 'View'
CREATE VIEW[cust].[ViewMasterUnfinishedParts]
AS
WITH StepsData (ShippingDate, WorkCenterCode, DesQty, CurQty, OrderType) As (select co.ShippingDate,ps.WorkCenterCode, SUM(ps.DesiredTargetQuantity) As DesQty,SUM(ps.CurrentTargetQuantity) As CurQty, po2.OrderType 
					from base.ProductionSteps ps 
					left outer join base.ProductionOrders po2 on ps.ProductionOrderCode=po2.Code
					left outer join base.CustomerOrders co on po2.CustomerOrderCode=co.Code
					where ps.DisposeState=0 -- Geplant
					group by co.ShippingDate, ps.WorkCenterCode, po2.OrderType),

	ShippingDateData (ShippingDate) as
					 (select Distinct co.ShippingDate
					  from base.CustomerOrders co)

Select 

ISNULL(ROW_NUMBER() over (order by ShippingDateData.ShippingDate),-1) as RowID
,ShippingDateData.ShippingDate As ShippingDate
,CAST(DATEPART(Week,ShippingDateData.ShippingDate) As Nvarchar) + '/' + CAST(DATEPART(Year,ShippingDateData.ShippingDate) As Nvarchar) As ShippingWeek

-- B300
,B300.DesQty - B300.CurQty As B300Rest
-- EDGETEQ
,EDGETEQ.DesQty - EDGETEQ.CurQty As EDGETEQRest
-- V200
,V200.DesQty - V200.CurQty As V200Rest
-- E310
,E310.DesQty - E310.CurQty As E310Rest
-- SORT
,SORT.DesQty - SORT.CurQty As SORTRest
-- PREASSEM
,PREASSEM.DesQty - PREASSEM.CurQty As PREASSEMRest
-- ASSEM
,ASSEM.DesQty - ASSEM.CurQty As ASSEMRest

from ShippingDateData

-- B300
left outer join StepsData B300 on B300.ShippingDate = ShippingDateData.ShippingDate and B300.WorkCenterCode='CU1'
-- EDGETEQ
left outer join StepsData EDGETEQ on EDGETEQ.ShippingDate = ShippingDateData.ShippingDate and EDGETEQ.WorkCenterCode='EB1'
-- V200
left outer join StepsData V200 on V200.ShippingDate = ShippingDateData.ShippingDate and V200.WorkCenterCode='CNC1'
-- E310
left outer join StepsData E310 on E310.ShippingDate = ShippingDateData.ShippingDate and E310.WorkCenterCode='CNC2'
-- SORT
left outer join StepsData SORT on SORT.ShippingDate = ShippingDateData.ShippingDate and SORT.WorkCenterCode='SP'
-- PREASSEM
left outer join StepsData PREASSEM on PREASSEM.ShippingDate = ShippingDateData.ShippingDate and PREASSEM.WorkCenterCode='PRE'
-- ASSEM
left outer join StepsData ASSEM on ASSEM.ShippingDate = ShippingDateData.ShippingDate and ASSEM.WorkCenterCode='AS1'

