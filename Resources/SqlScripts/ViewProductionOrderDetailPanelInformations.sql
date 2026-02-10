-- Zusatzinformationen in der Fertigungsauftragsmaske
-- created by S.Feist / 2026-02-10

CREATE VIEW[cust].[ViewProductionOrderDetailPanelInformations]
 AS 
 SELECT 
	-- Allgemeine Informationen
	po.[Code] As ProductionOrderCode
	,po.[CustomerOrderCode]
	,po.[CustomerOrderPosition]
	,po.[ArticleNumber]
	,po.[ArticleDescription]
	,po.[DesiredTargetQuantity]
	,po.[Material]
	,po.[MaterialCategory]
	,po.[Grain]
	
	-- Dimensionen
	,po.[Length]
	,po.[CuttingLength]
	,po.[Width]
	,po.[CuttingWidth]
	,po.[Thickness]
	,po.[CuttingThickness]

	-- Bekantung
	,po.[EdgeTransition]
	,po.[EdgeShape]

	-- Fertigungsweg
	,po.[ComponentType]
	,po.[RouteCode]
	,po.[ProductionRoute]

	-- Nachfertigung
	,po.[ReproductionType]
	,po.[OriginalProductionOrderCode]
	,po.[OriginalProductionItemCode]
	
	-- Sonstige Informationen (cust-Felder)
	,po.[CustomProductionType]

from base.ProductionOrders po
