-- View for the manual bulk planing in the web environment
-- 2024-09-24 SG: Created
CREATE VIEW[cust].[ViewBulkPlannings]
 AS SELECT 
	PO.Sequence AS [Sequence],
	CO.Code AS OrderNumber,
	CO.OrderDate,
	PO.NarrowPartType AS NarrowPart,
	PO.CustomVolume AS Volume,
	PO.Material				
	
	FROM base.CustomerOrders CO
	join base.ProductionOrders PO on CO.Code = PO.CustomerOrderCode
