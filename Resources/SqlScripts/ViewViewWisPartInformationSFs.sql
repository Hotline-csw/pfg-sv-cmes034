-- Test VIEW / SF

CREATE VIEW[cust].[ViewViewWisPartInformationSFs]
 AS SELECT DISTINCT 
 
	ProductionStepsComp.[WorkCenterCode]
	,ProductionItem.[Code] As ProductionItemCode
	,ProductionOrderPart.[CustomerOrderCode]
	,ProductionOrderPart.[CustomerOrderPosition]
	,ProductionOrderPart.[Code] As ProductionOrderCode
	,ProductionOrderPart.[ReproductionType]
	,ProductionOrderPart.[ArticleNumber]
	,ResourcePartMaterial.[Description] As MaterialDescription
	,ProductionOrderPart.[Length]
	,ProductionOrderPart.[Width]
	,ProductionOrderPart.[Thickness]
	,ProductionOrderComp.[RouteCode] -- production route assembly group
	,ProductionOrderPart.[TopProductionOrderNumber]
	,ProductionOrderPart.[ParentProductionOrderNumber]
	,ProductionStepsComp.ProductionState As ProductionStepState

	-- drawing
	,(Select top 1 
		[Sequence] 
		from [base].[Binaries] 
		where [Sequence] in (Select [BinarieSequence] from [base].[ProductionOrders_Binaries] where [ProductionOrderCode] = ProductionOrderPart.[Code])
				and OriginalPath like '%_Prt.png') As PartDrawing
				
	,piv.Code As ReProductionCode

from [base].[ProductionItems] ProductionItem
	left outer join [base].[ProductionOrders] ProductionOrderPart on ProductionOrderPart.[Code] = ProductionItem.[ProductionOrderCode]
	left outer join [base].[ProductionOrdersResources] ResourcePartMaterial on ResourcePartMaterial.[ProductionOrderCode] = ProductionOrderPart.[Code]
																				and ResourcePartMaterial.[InternalType] = 4
	left outer join [base].[ProductionOrders] ProductionOrderComp on ProductionOrderComp.[Code] = ProductionOrderPart.[ParentProductionOrderNumber]

	-- assembly group steps
	left outer join [base].[ProductionSteps] ProductionStepsComp on ProductionStepsComp.[ProductionOrderCode] = ProductionOrderComp.[Code] 
																	and ProductionStepsComp.[DisposeState] = 0 -- scheduled
	left outer join [base].[ProductionItemsValidations] piv on piv.ProductionItemCode = ProductionItem.Code

where ProductionOrderPart.[OrderType] = 3		-- parts



