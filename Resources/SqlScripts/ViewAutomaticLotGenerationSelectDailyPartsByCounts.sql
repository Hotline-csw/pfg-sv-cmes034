-- CREATING NEW VIEW
-- The schema name has to be 'cust'
-- The view name has to start with the prefix 'View'
CREATE VIEW[cust].[ViewAutomaticLotGenerationSelectDailyPartsByCounts]
AS 
 
SELECT ISNULL(ROW_NUMBER() OVER(ORDER BY po.Material),-1)AS RowID, po.Material, COUNT(*) AS EntryCount, MIN(ps.DesiredStartDateProcessing) AS EarliestStartDate
FROM base.ProductionOrders po
INNER JOIN base.ProductionItems pi ON pi.ProductionOrderCode = po.Code
INNER JOIN base.ProductionSteps ps on ps.ProductionOrderCode = po.Code
WHERE po.Material IS NOT NULL
AND ps.DesiredStartDateProcessing >=    DATEADD(DAY, 0, CAST(GETDATE() AS DATE))
AND ps.DesiredStartDateProcessing < DATEADD(DAY, 1, CAST(GETDATE() AS DATE))
AND pi.OptimizationTransferState = '0'
GROUP BY po.Material

