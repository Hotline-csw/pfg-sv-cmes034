-- Detailview for MasterProgressWorkCenterCustomerOrders
-- The schema name has to be 'cust'
-- The view name has to start with the prefix 'View'
CREATE VIEW[cust].[ViewDetailProgressInformations]
AS
Select
 po.CustomerOrderCode
,po.Code As ProductionOrderCode
,po.CustomerOrderPosition
,po.ArticleNumber
,po.ProductionState As OrderState
,po.ReproductionType
,po.OriginalProductionOrderCode
,po.OriginalProductionItemCode
,po.RouteCode
,po.ComponentType
,po.OrderType
,po.DesiredEndDate

,ps.Code As ProductionStepCode 
,ps.WorkCenterCode   
,ps.WorkstepDescription
,ps.ProductionState
,ps.DesiredTargetQuantity  
,ps.CurrentTargetQuantity
,ps.CurrentFirstDate
,ps.CurrentLastDate

,mb.PlanningNumber

,pdi.Code As UniqueId

,opp.OptimizationCode 
,

(
Select 
	Top 1 psLast.WorkstepDescription + ' [' + psLast.Code +  ']'
From 
	base.ProductionSteps psLast 
Where	
	psLast.ProductionOrderCode = po.Code
	and
	psLast.ProductionState in (35,25)
	and
	psLast.CurrentTargetQuantity > 0
	and 
	psLast.PossibleRouteCode = ps.PossibleRouteCode
Order by 
	psLast.[Order] desc
)
As LastStep,

(
Select 
	Top 1 psNext.WorkstepDescription + ' [' + psNext.Code +  ']'
From 
	base.ProductionSteps psNext
Where
	psNext.ProductionOrderCode = po.Code
	and
	psNext.ProductionState in (0,20)
	and
	psNext.PossibleRouteCode = ps.PossibleRouteCode
Order by 
	psNext.[Order] asc
)
As NextStep


From base.ProductionOrders po
LEFT OUTER JOIN base.ProductionSteps ps on ps.ProductionOrderCode = po.Code and ps.DisposeState = 0
LEFT OUTER JOIN base.ProductionItems pdi on pdi.ProductionOrderCode = po.Code
LEFT OUTER JOIN base.OptimizationParts opp on opp.ProductionOrderCode = po.Code
LEFT OUTER JOIN base.ProductionOrders ts on ts.Code = po.TopProductionOrderNumber   
LEFT OUTER JOIN base.ManualBulks mb on mb.Sequence = ts.PlanningSequence

