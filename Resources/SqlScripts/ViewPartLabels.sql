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
--	  ,LB.EdgeNoCncZeroLine
	  ,PI.OptionalPart
	  ,POTOP.ArticleNumber AS ArticleName
	  ,POTOP.ArticleDescription AS CabinetDescription
      ,PO.CustomerOrderCode
      ,PO.CustomerOrderPosition
      ,PO.ArticleNumber
      ,PO.ArticleDescription
--    ,PO.CustomSandwichInfo
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
--    ,PO.AdditionalLength
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
	  ,po.CustomLvPosition AS LvPosition
	  ,po.CustomLineId AS LineID
	  ,po.CustomIxPartId AS IxPartID
	  ,po.CustomBomDescription AS BomDescr
	  ,po.CustomSurfaceTopId AS SurfTopID
	  ,po.CustomSurfaceBottomId AS SurfBottomID
	  ,CONVERT(XML, poExt.[data]).value('(/ExtensionData/SurfaceFinishTop)[1]','nvarchar(64)') AS SurfaceFinishTop
	  ,CONVERT(XML, poExt.[data]).value('(/ExtensionData/SurfaceFinishBottom)[1]','nvarchar(64)') AS SurfaceFinishBottom
	  ,po.CustomEdgeLineFlag AS EdgeLineFlag
	  ,convert(xml,posRouteExt.[Data]).value('(/ExtensionData/CuttingLength2)[1]','nvarchar(64)') as CuttingLength2
	  ,convert(xml,posRouteExt.[Data]).value('(/ExtensionData/CuttingWidth2)[1]','nvarchar(64)') as CuttingWidth2
	  ,po.CustomManufacturingInformation AS ManufacturingInformation 
	  ,oppa1.data AS InfoPhysicalFlip -- AdditionalInformation65
	  ,oppa2.data AS InfoEdgePositionForAbdFront -- AdditionalInformation66
	  ,oppa3.data AS InfoEdgePositionForAbdBack -- AdditionalInformation67
	  ,oppa4.data AS InfoEdgePositionForAbdLeft -- AdditionalInformation68
	  ,oppa5.data AS InfoEdgePositionForAbdRight -- AdditionalInformation69
	  ,oppa6.data AS OrientationFirstPass -- AdditionalInformation25
	  ,oppa7.data AS EdgePassFrontLeft -- AdditionalInformation27
	  ,oppa8.data AS EdgePassFrontRight -- AdditionalInformation28
	  ,oppa9.data AS EdgePassBackLeft -- AdditionalInformation29
	  ,oppa10.data AS EdgePassBackRight -- AdditionalInformation30
	  ,oppa11.data AS CncZeroLine -- AdditionalInformation26
	  ,oppa12.data AS PnxInfoSurfaceTopIdRosink -- AdditionalInformation35
	  ,oppa13.data AS PnxInfoSurfaceBottomIdRosink -- AdditionalInformation36
	  ,oppa14.data AS PnxInfoSurfaceFinishTopRosink -- AdditionalInformation37
	  ,oppa15.data AS PnxInfoSurfaceFinishBottomRosink -- AdditionalInformation38
	  ,oppa16.Data AS PnxInfoEdgeSouth -- AdditionalInformation10
	  ,oppa17.Data AS PnxInfoEdgeNorth -- AdditionalInformation11
	  ,oppa18.Data AS PnxInfoEdgeWest -- AdditionalInformation12
	  ,oppa19.Data AS PnxInfoEdgeEast -- AdditionalInformation13
	  ,po.FinalEdgeTransition
	  ,(select value from (select value,ROW_NUMBER() OVER(order by (select 0)) AS Row From STRING_SPLIT (po.FinalEdgeTransition, ':')) q where row = 1) AS EdgeTransitionS
	  ,(select value from (select value,ROW_NUMBER() OVER(order by (select 0)) AS Row From STRING_SPLIT (po.FinalEdgeTransition, ':')) q where row = 2) AS EdgeTransitionN
	  ,(select value from (select value,ROW_NUMBER() OVER(order by (select 0)) AS Row From STRING_SPLIT (po.FinalEdgeTransition, ':')) q where row = 3) AS EdgeTransitionW
	  ,(select value from (select value,ROW_NUMBER() OVER(order by (select 0)) AS Row From STRING_SPLIT (po.FinalEdgeTransition, ':')) q where row = 4) AS EdgeTransitionE
	  ,oppa20.Data AS PnxInfoIxPartIdRosink -- AdditionalInformation39
	  ,oppa21.Data AS PnxLabelInfoCncZeroLineNoDc -- AdditionalInformation70
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
--LEFT OUTER JOIN base.ProcessingData as PD on PD.ProductionOrderCode = PI.ProductionOrderCode AND PD.WorkCenterCode = 'BHX' and pd.ProductionStepCode IS NOT NULL
--LEFT OUTER JOIN data.LabelInfoCncZeroLines as LB ON LB.WorkCenterCode = PD.WorkCenterCode AND LB.OrientationY = PD.OrientationY AND LB.OrientationZ=PD.OrientationZ
left OUTER join base.OptimizationPresettingPartsAdditionals AS oppa1 on pi.Code = oppa1.OptimizationPresettingPartCode AND pi.OptimizationCode = oppa1.OptimizationPresettingCode AND oppa1.[key] = 'AdditionalInformation65'
left OUTER join base.OptimizationPresettingPartsAdditionals AS oppa2 on pi.Code = oppa2.OptimizationPresettingPartCode AND pi.OptimizationCode = oppa2.OptimizationPresettingCode AND oppa2.[key] = 'AdditionalInformation66'
left OUTER join base.OptimizationPresettingPartsAdditionals AS oppa3 on pi.Code = oppa3.OptimizationPresettingPartCode AND pi.OptimizationCode = oppa3.OptimizationPresettingCode AND oppa3.[key] = 'AdditionalInformation67'
left OUTER join base.OptimizationPresettingPartsAdditionals AS oppa4 on pi.Code = oppa4.OptimizationPresettingPartCode AND pi.OptimizationCode = oppa4.OptimizationPresettingCode AND oppa4.[key] = 'AdditionalInformation68'
left OUTER join base.OptimizationPresettingPartsAdditionals AS oppa5 on pi.Code = oppa5.OptimizationPresettingPartCode AND pi.OptimizationCode = oppa5.OptimizationPresettingCode AND oppa5.[key] = 'AdditionalInformation69'
left OUTER join base.OptimizationPresettingPartsAdditionals AS oppa6 on pi.Code = oppa6.OptimizationPresettingPartCode AND pi.OptimizationCode = oppa6.OptimizationPresettingCode AND oppa6.[key] = 'AdditionalInformation25'
left OUTER join base.OptimizationPresettingPartsAdditionals AS oppa7 on pi.Code = oppa7.OptimizationPresettingPartCode AND pi.OptimizationCode = oppa7.OptimizationPresettingCode AND oppa7.[key] = 'AdditionalInformation27'
left OUTER join base.OptimizationPresettingPartsAdditionals AS oppa8 on pi.Code = oppa8.OptimizationPresettingPartCode AND pi.OptimizationCode = oppa8.OptimizationPresettingCode AND oppa8.[key] = 'AdditionalInformation28'
left OUTER join base.OptimizationPresettingPartsAdditionals AS oppa9 on pi.Code = oppa9.OptimizationPresettingPartCode AND pi.OptimizationCode = oppa9.OptimizationPresettingCode AND oppa9.[key] = 'AdditionalInformation29'
left OUTER join base.OptimizationPresettingPartsAdditionals AS oppa10 on pi.Code = oppa10.OptimizationPresettingPartCode AND pi.OptimizationCode = oppa10.OptimizationPresettingCode AND oppa10.[key] = 'AdditionalInformation30'
left OUTER join base.OptimizationPresettingPartsAdditionals AS oppa11 on pi.Code = oppa11.OptimizationPresettingPartCode AND pi.OptimizationCode = oppa11.OptimizationPresettingCode AND oppa11.[key] = 'AdditionalInformation26'
left OUTER join base.OptimizationPresettingPartsAdditionals AS oppa12 on pi.Code = oppa12.OptimizationPresettingPartCode AND pi.OptimizationCode = oppa12.OptimizationPresettingCode AND oppa12.[key] = 'AdditionalInformation35'
left OUTER join base.OptimizationPresettingPartsAdditionals AS oppa13 on pi.Code = oppa13.OptimizationPresettingPartCode AND pi.OptimizationCode = oppa13.OptimizationPresettingCode AND oppa13.[key] = 'AdditionalInformation36'
left OUTER join base.OptimizationPresettingPartsAdditionals AS oppa14 on pi.Code = oppa14.OptimizationPresettingPartCode AND pi.OptimizationCode = oppa14.OptimizationPresettingCode AND oppa14.[key] = 'AdditionalInformation37'
left OUTER join base.OptimizationPresettingPartsAdditionals AS oppa15 on pi.Code = oppa15.OptimizationPresettingPartCode AND pi.OptimizationCode = oppa15.OptimizationPresettingCode AND oppa15.[key] = 'AdditionalInformation38'
left OUTER join base.OptimizationPresettingPartsAdditionals AS oppa16 on pi.Code = oppa16.OptimizationPresettingPartCode AND pi.OptimizationCode = oppa16.OptimizationPresettingCode AND oppa16.[key] = 'AdditionalInformation10'
left OUTER join base.OptimizationPresettingPartsAdditionals AS oppa17 on pi.Code = oppa17.OptimizationPresettingPartCode AND pi.OptimizationCode = oppa17.OptimizationPresettingCode AND oppa17.[key] = 'AdditionalInformation11'
left OUTER join base.OptimizationPresettingPartsAdditionals AS oppa18 on pi.Code = oppa18.OptimizationPresettingPartCode AND pi.OptimizationCode = oppa18.OptimizationPresettingCode AND oppa18.[key] = 'AdditionalInformation12'
left OUTER join base.OptimizationPresettingPartsAdditionals AS oppa19 on pi.Code = oppa19.OptimizationPresettingPartCode AND pi.OptimizationCode = oppa19.OptimizationPresettingCode AND oppa19.[key] = 'AdditionalInformation13'
left OUTER join base.OptimizationPresettingPartsAdditionals AS oppa20 on pi.Code = oppa20.OptimizationPresettingPartCode AND pi.OptimizationCode = oppa20.OptimizationPresettingCode AND oppa20.[key] = 'AdditionalInformation39'
left OUTER join base.OptimizationPresettingPartsAdditionals AS oppa21 on pi.Code = oppa21.OptimizationPresettingPartCode AND pi.OptimizationCode = oppa21.OptimizationPresettingCode AND oppa21.[key] = 'AdditionalInformation70'
WHERE PJ.JobName = 'PrintRosinkStandardLabel'
