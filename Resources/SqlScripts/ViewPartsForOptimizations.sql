CREATE VIEW [cust].[ViewFredPatternParts]
    AS select
    Orders.Code as ProductionOrderCodeFred
    ,Items.Code as ProductionItemCode
    ,Items.OptimizationCode
    ,Items.OptimizationMethod
    ,Items.OptimizationCuttingPlanCode
    ,Items.Offcut
    ,Items.Quantity
    ,Items.CuttingLength
    ,Items.CuttingWidth
    ,Orders.ArticleDescription
    ,Orders.Length
    ,Orders.Width
    ,Orders.Material
    ,History.LastWorkCenter
    ,History.LastFeedback
from base.OptimizationParts as Items
left join base.ProductionOrders as Orders on Items.ProductionOrderCode = Orders.Code
outer apply (SELECT   Top 1
                    d.ProductionItemCode,
                    d.WorkCenterCode as LastWorkCenter
                    ,d.Timestamp as LastFeedback
                    FROM base.ProductionItemsHistory as d
                    where d.ProductionItemCode = Items.Code
                    order by d.Timestamp desc) as History