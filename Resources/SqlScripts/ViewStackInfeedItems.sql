-- intelliStack view for infeed items
CREATE VIEW [cust].[ViewStackInfeedItems]
AS
SELECT Sequence,
       StackCode,
       LayerNumber,
       PositionInLayer,
       StackItemCode,
       StackItemType,
       QuantityInLayer,
       CreationDate,
       CustomBatchNumberBoard,
       CustomSupplierName,
       CustomOrderNumber,
       CustomOrderPosition,
       CustomReceiptNumber,
       CustomReceiptDate,
       CustomReceiptLocation,
       CustomOrderInStack,
       CustomCoordinateX,
       CustomCoordinateY,
       CustomOrientation,
       ProductionOrderCode,
       OuterSurfaceExtern,
       CONVERT( NVARCHAR(32), CASE WHEN OuterSurfaceExtern = 0
                                   THEN 'Unten'
                                   ELSE 'Oben'
                              END ) AS OuterSurfaceExternDescription,
       LeadingEdge,
       CONVERT( NVARCHAR(32), CASE CustomAlternateCodeExtern
                                   WHEN 'N' THEN 'Nord'
                                   WHEN 'W' THEN 'West'
                                   WHEN 'S' THEN 'Süd'
                                   WHEN 'E' THEN 'Ost'
                                   ELSE 'Datenfehler'
                              END ) AS LeadingEdgeDescription

  FROM
       ( SELECT si.Sequence,
                si.StackCode,
                si.LayerNumber,
                si.PositionInLayer,
                si.StackItemCode,
                si.StackItemType,
                si.QuantityInLayer,
                si.CreationDate,
                si.CustomBatchNumberBoard,
                si.CustomSupplierName,
                si.CustomOrderNumber,
                si.CustomOrderPosition,
                si.CustomReceiptNumber,
                si.CustomReceiptDate,
                si.CustomReceiptLocation,
                si.CustomOrderInStack,
                si.CustomCoordinateX,
                si.CustomCoordinateY,
                si.CustomOrientation,
                po.ProductionOrderCode,
                CONVERT( FLOAT, ISNULL( aussenseite.ValueFloat, 0 ) ) AS OuterSurfaceExtern,
                CONVERT( FLOAT, ISNULL( vorderkante.ValueFloat, 1 ) ) AS LeadingEdge,
                kompass.AlternateCode,
                kompass.CustomAlternateCodeExtern
           FROM base.StackItems si

          OUTER APPLY
          ( SELECT ProductionOrderCode
              FROM base.ProductionItems
             WHERE Code = si.StackItemCode
          ) po

          OUTER APPLY
          ( SELECT ValueFloat
              FROM cust.ProductionOrderAttributes
             WHERE ProductionOrderCode = po.ProductionOrderCode
               AND AttributeCode = 'SM_BT_AUSSENSEITE_MAN'
          ) aussenseite

          OUTER APPLY
          ( SELECT ValueFloat
              FROM cust.ProductionOrderAttributes
             WHERE ProductionOrderCode = po.ProductionOrderCode
               AND AttributeCode = 'SM_BT_SEITEVORDERKANTE'
          ) vorderkante

          OUTER APPLY
          ( SELECT TOP 1 AlternateCode, CustomAlternateCodeExtern
              FROM base.EdgeProfiles
             WHERE ProductionOrderCode = po.ProductionOrderCode
               AND LEN( ISNULL( ProductionStepCode, '' ) ) = 0
               AND CustomEdgeSide = 'S' + CONVERT( NVARCHAR(32), vorderkante.ValueFloat )
          ) kompass
       ) x

