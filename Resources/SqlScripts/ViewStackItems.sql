-- intelliStack view for stack items
CREATE VIEW [cust].[ViewStackItems]
AS
SELECT s.StackCode,
       s.CustomClass AS StackClass,
       s.CustomStackType AS StackType,
       s.CustomBaseBoardType AS BaseBoardType,
       s.CustomExternalStackCode AS ExternalStackCode,
       s.CustomExternalStackCode2 AS ExternalStackCode2,
       s.CustomExternalStackCode3 AS ExternalStackCode3,
       s.CustomStackStructureCode AS StackStructureCode,
       s.CustomDestination AS StackDestination,
       s.CustomAllocationCode AS SorterAllocationCode,
       i.StackItemCode AS ProductionOrderOriginalCode,
       ( SELECT ProductionItemCode
           FROM cust.HandlingItems
          WHERE ProductionOrderExchangeCode = i.StackItemCode
       ) AS ProductionItemExchangedCode,
       i.LayerNumber,
       i.CustomCoordinateX,
       i.CustomCoordinateY,
       i.CustomOrientation,
       i.CustomIntelliStackPositionX,
       i.CustomIntelliStackPositionY,
       i.CustomIntelliStackPositionZ,
       i.QuantityInLayer,
       ( SELECT MAX( LayerNumber )
           FROM base.StackItems
          WHERE StackCode = s.StackCode
       ) AS MaxLayer

       -- i.CustomOrderInStack kann nicht verwendet werden, da nicht konsequent gesetzt
       -- i.PositionInLayer kann nicht verwendet werden, da Wert fix auf 1 gesetzt

  FROM base.Stacks s, base.StackItems i
 WHERE i.StackCode = s.StackCode
