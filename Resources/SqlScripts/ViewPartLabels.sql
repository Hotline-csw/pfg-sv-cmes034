-- View für Etiketten
-- Last Changes: 
-- 2022-11-10 - addinfo fields - by SF


CREATE VIEW[cust].[ViewPartsLabels]
 AS SELECT 
	   PI.ProductionOrderCode
	  ,PI.Code
	  ,PI.DesiredQuantity
	  ,PI.OptimizationCode
	  ,CO.Address1
      ,CO.Address2
      ,CO.Address3
      ,CO.Address4
      ,CO.Address5
	  ,PI.OptionalPart
	  ,POTOP.ArticleNumber AS ArticleName
	  ,POTOP.ArticleDescription AS CabinetDescription
      ,PO.CustomerOrderCode
      ,PO.CustomerOrderPosition
      ,PO.ArticleNumber
      ,PO.ArticleDescription
      ,PO.DesiredStartDate
      ,PO.DesiredEndDate
      ,PO.DesiredTargetQuantityMin
      ,PO.DesiredTargetQuantity
      ,PO.DesiredTargetQuantityMax
      ,PO.CurrentFirstDate
      ,PO.CurrentLastDate
      ,PO.ProductionState
      ,PO.ReleaseState
      ,PO.Length
      ,PO.CuttingLength
      ,PO.Width
      ,PO.CuttingWidth
      ,PO.Thickness
      ,PO.CuttingThickness
      ,PO.RouteCode
      ,PO.ParentProductionOrderNumber
      ,PO.TopProductionOrderNumber
      ,PO.Material
      ,PO.PlanningSequence
      ,PO.InputSourceType
      ,PO.OrderType
      ,PO.ComponentType
      ,PO.NcProgramReference
      ,PO.Grain
      ,PO.GrainOrientation
      ,PO.SpecialPartType
      ,PO.EdgeTransition
      ,PO.MaterialCategory
      ,PO.NarrowPartType
      ,PO.IsLengthGreaterEqualWidth
      ,PO.IsRatioPart
      ,PO.IsSquarePart
      ,PO.EdgeShape
      ,PO.CornerNorthWest
      ,PO.CornerWestSouth
      ,PO.CornerSouthEast
      ,PO.CornerEastNorth
      ,PO.Bracket
      ,PO.ProductionRoute
      ,PO.PartGeometry
      ,PO.Mirror
      ,PO.OriginalProductionOrderCode
      ,PO.OriginalProductionItemCode
      ,PO.ReproductionType
      ,PO.CreationDate
      ,PO.CreationSource
      ,PO.ModificationDate
      ,PO.ModificationSource
      ,PO.OriginalStartDate
      ,PO.OriginalEndDate
      ,PO.Instance
	  ,EdgeSouth.EdgeId AS EdgeMaterialSouth
	  ,EdgeSouth.MaterialCode AS EdgeMaterialCodeSouth
	  ,EdgeSouth.Trim AS EdgeTrimSouth
	  ,EdgeEast.EdgeId AS EdgeMaterialEast
	  ,EdgeEast.MaterialCode AS EdgeMaterialCodeEast
	  ,EdgeEast.Trim AS EdgeTrimEast
	  ,EdgeNorth.EdgeId AS EdgeMaterialNorth 
	  ,EdgeNorth.MaterialCode AS EdgeMaterialCodeNorth
	  ,EdgeNorth.Trim AS EdgeTrimNorth
	  ,EdgeWest.EdgeId AS EdgeMaterialWest
	  ,EdgeWest.MaterialCode AS EdgeMaterialCodeWest
	  ,EdgeWest.Trim AS EdgeTrimWest
	  ,PJ.ProcessingState
	  ,PJ.JobName
	  ,co.Customer
	  ,co.Employee
	  ,po.FinalEdgeTransition
	  ,(select value from (select value,ROW_NUMBER() OVER(order by (select 0)) AS Row From STRING_SPLIT (po.FinalEdgeTransition, ':')) q where row = 1) AS EdgeTransitionS
	  ,(select value from (select value,ROW_NUMBER() OVER(order by (select 0)) AS Row From STRING_SPLIT (po.FinalEdgeTransition, ':')) q where row = 2) AS EdgeTransitionN
	  ,(select value from (select value,ROW_NUMBER() OVER(order by (select 0)) AS Row From STRING_SPLIT (po.FinalEdgeTransition, ':')) q where row = 3) AS EdgeTransitionW
	  ,(select value from (select value,ROW_NUMBER() OVER(order by (select 0)) AS Row From STRING_SPLIT (po.FinalEdgeTransition, ':')) q where row = 4) AS EdgeTransitionE

FROM [base].PrintJobItems AS PJ
INNER JOIN base.ProductionItems PI on PJ.ProductionItemCode = PI.Code
LEFT OUTER JOIN base.ProductionOrders PO on PO.Code = PI.ProductionOrderCode
LEFT OUTER JOIN base.CustomerOrders as CO on CO.Code = PO.CustomerOrderCode
LEFT OUTER JOIN base.ProductionOrders AS POTOP ON POTOP.Code = PO.TopProductionOrderNumber
LEFT OUTER JOIN base.EdgeProfiles as EdgeSouth ON PO.Code = EdgeSouth.ProductionOrderCode AND EdgeSouth.AlternateCode = 'S' AND EdgeSouth.EdgeToStepsType = '2' AND EdgeSouth.ProductionStepCode IS NOT NULL
LEFT OUTER JOIN base.EdgeProfiles as EdgeEast ON PO.Code = EdgeEast.ProductionOrderCode AND EdgeEast.AlternateCode = 'E' AND EdgeEast.EdgeToStepsType = '2' AND EdgeEast.ProductionStepCode IS NOT NULL
LEFT OUTER JOIN base.EdgeProfiles as EdgeNorth ON PO.Code = EdgeNorth.ProductionOrderCode AND EdgeNorth.AlternateCode = 'N' AND EdgeNorth.EdgeToStepsType = '2' AND EdgeNorth.ProductionStepCode IS NOT NULL
LEFT OUTER JOIN base.EdgeProfiles as EdgeWest ON PO.Code = EdgeWest.ProductionOrderCode AND EdgeWest.AlternateCode = 'W' AND EdgeWest.EdgeToStepsType = '2' AND EdgeWest.ProductionStepCode IS NOT NULL
LEFT OUTER JOIN base.ProductionOrdersExtensions as poExt ON PO.Code = poExt.ProductionOrderCode and poExt.[Key]='ExtensionData' and poExt.[Type]='XML'
left outer join base.PossibleRoutes posRoute on posRoute.ProductionOrderCode = po.Code and  posRoute.DisposeState = 0 -- Scheduled
left outer join base.PossibleRoutesExtensions posRouteExt on posRouteExt.ProductionOrderCode = po.Code and posRouteExt.PossibleRouteCode = posRoute.Code and posRouteExt.[Key]='ExtensionData' and posRouteExt.[Type]='XML'
WHERE PJ.JobName = 'PrintStandardLabel'