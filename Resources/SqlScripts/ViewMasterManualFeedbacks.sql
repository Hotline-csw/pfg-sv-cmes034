-- Masterview for tile "Manual Feedback"
-- The schema name has to be 'cust'
-- The view name has to start with the prefix 'View'
CREATE VIEW[cust].[ViewMasterManualFeedbacks]
AS
Select
 po.Sequence
,po.Code As ProductionOrderCode
,po.ArticleNumber
,po.ComponentType
,po.ArticleDescription
,po.CustomerOrderCode
,po.CustomerOrderPosition
,po.DesiredTargetQuantity as Quantity
,po.ReleaseState
,po.Length
,po.Width
,po.Thickness
,po.CuttingLength
,po.CuttingWidth
,po.ParentProductionOrderNumber
,po.TopProductionOrderNumber
,po.OrderType
,po.RouteCode
,po.ProcessingState
,po.DesiredStartDate
,po.DesiredEndDate
,po.Bracket
,po.ProductionState

,pi.Code As ProductionItemCode

,mb.PlanningNumber

,op.OptimizationCode

From base.ProductionOrders po

INNER JOIN base.ProductionItems pi on pi.ProductionOrderCode = po.Code
LEFT OUTER JOIN base.ManualBulks mb on mb.Sequence = po.PlanningSequence
LEFT OUTER JOIN base.OptimizationParts op on op.ProductionOrderCode = po.Code

Where po.OrderType  = 3

