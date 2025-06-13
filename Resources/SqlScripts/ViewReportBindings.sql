-- Printing Center Confi
-- SF - 2025-06-13

CREATE VIEW[cust].[ViewReportBindings]
AS
SELECT rb.Sequence,
       rb.WorkOrder,
       rb.ProductionOrderCode,
       rb.ProductionItemCode,
       rb.CustomerOrderCode,
       rb.CustomerOrderPosition,
       rb.OptimizationCode,
       rb.ReportLayout,
       r.Description AS ReportDescription,
       CASE ISNULL( rb.BinariesSequence, 0 )
            WHEN 0
            THEN 0
            ELSE 1
        END AS ReportIsFinished,
       rb.ReportEntity,
       rb.ReportSequence,
       rb.BinariesSequence
FROM cust.ReportBindings rb
JOIN cust.Reports r ON rb.ReportLayout = r.Layout
