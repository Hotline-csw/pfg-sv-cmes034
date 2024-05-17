-- Deatilview for MasterManualFeedbacks
-- The schema name has to be 'cust'
-- The view name has to start with the prefix 'View'
CREATE VIEW[cust].[ViewDetailManualFeedbacks]
AS
Select
 pi.Code As ProductionItemCode
,pi.ProductionOrderCode

,ps.[Order]
,ps.Code As ProductionStepCode
,ps.WorkstepDescription
,ps.WorkCenterCode
,ps.ProductionState
,ps.DesiredStartDateProcessing
,ps.DesiredEndDateProcessing
,ps.Capacity
,ps.CurrentFirstDate
,ps.CurrentLastDate

,ISNULL(pih.FeedbackState, -1) As FeedbackState
,ISNULL(pih.Timestamp, NULL) As Timestamp

From base.ProductionItems pi

INNER JOIN base.ProductionSteps ps on ps.ProductionOrderCode = pi.ProductionOrderCode
LEFT OUTER JOIN base.ProductionItemsHistory pih ON pih.ProductionItemCode = pi.Code and pih.ProductionOrderCode = ps.ProductionOrderCode and pih.ProductionStepCode = ps.Code

Where ps.DisposeState = 0

