-- View to print furniture label
-- The schema name has to be 'cust'
-- The view name has to start with the prefix 'View'
CREATE VIEW[cust].[ViewFurnitureLabels]
AS
Select
ISNULL(row_number() over (Order By po.Sequence),-1) As RowID

,co.Code As CustomerOrderCode
,co.Customer
,Format(co.ShippingDate, 'yyyy-MM-dd') As ShippingDate
,Format(co.DeliveryDate, 'yyyy-MM-dd') As DeliveryDate
,co.Reference

,po.Code As ProductionOrderCode
,po.CustomerOrderPosition
,po.ArticleNumber
,po.ArticleDescription
,po.Length
,po.Width
,po.Thickness
,CAST(po.Length as nvarchar) + ' x ' + CAST(po.Width as nvarchar) + ' x ' + CAST(po.Thickness as nvarchar) As Dimension

,pji.ProcessingState
,pji.JobName

From base.PrintJobItems pji
INNER JOIN base.ProductionOrders po On po.Code = pji.ProductionOrderCode
INNER JOIN base.CustomerOrders co On co.Code = po.CustomerOrderCode

Where pji.JobName = 'PrintFurnitureLabel'

