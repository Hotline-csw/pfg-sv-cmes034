-- Masterview for tile "DashboardProgressData"
-- The schema name has to be 'cust'
-- The view name has to start with the prefix 'View'
CREATE VIEW[cust].[ViewDashboardProgressDatas]
AS
Select Top 1

ISNULL(ROW_NUMBER() over (Order by po.Code),-1) As RowID,


(
SELECT COUNT(pi.ProductionOrderCode)
From base.ProductionItems pi
INNER JOIN base.ProductionOrders po ON po.Code=pi.ProductionOrderCode
INNER JOIN base.ProductionSteps ps ON po.Code=ps.ProductionOrderCode
WHERE po.ReleaseState = 0
AND po.ProductionState <> 35
AND ps.WorkCenterCode = '1010'
AND pi.OptimizationTransferState = '0'
) as ItemsNotReleased,

(
SELECT COUNT(pi.ProductionOrderCode)
From base.ProductionItems pi
INNER JOIN base.ProductionOrders po ON po.Code=pi.ProductionOrderCode
INNER JOIN base.ProductionSteps ps ON po.Code=ps.ProductionOrderCode
WHERE po.ReleaseState = 1
AND po.ProductionState <> 35
AND ps.WorkCenterCode = '1010'
AND pi.OptimizationTransferState = '0'
) as ItemsReleasedAndNotOptimized,

(
(SELECT COUNT(pi.ProductionOrderCode)
From base.ProductionItems pi
INNER JOIN base.ProductionOrders po ON po.Code=pi.ProductionOrderCode
INNER JOIN base.ProductionSteps ps ON po.Code=ps.ProductionOrderCode
WHERE po.ReleaseState = 1
AND	po.ProductionState <> 35
AND ps.WorkCenterCode = '1010'
AND pi.OptimizationTransferState = '4'
)
-
(
Select Count(pi.ProductionOrderCode)
From base.ProductionItems pi
INNER JOIN base.ProductionItemsHistory pih ON pi.Code=pih.ProductionItemCode
WHERE pih.WorkCenterCode = '1010'
)
) as ItemsOptimizedAndNotProduced,

-- Workstation 1010 B300 Saw

(
select count(*)
from base.ProductionItems pi
join base.ProductionSteps ps on ps.ProductionOrderCode = pi.ProductionOrderCode and ps.WorkCenterCode = '1010' and ps.DesiredEndDateProcessing <= Convert(date,GetDate()) and ps.DesiredEndDateProcessing >= Convert(date,GetDate()-30)
where  pi.code not in(select pih.ProductionItemCode from base.ProductionItemsHistory pih where pih.ProductionItemCode =pi.Code and  pih.WorkCenterCode = '1010')
) as B300Remaining,


-- Workstation 3010 EDGETEQ-S810 Edgebanding

(
select count(*)
from base.ProductionItems pi
join base.ProductionSteps ps on ps.ProductionOrderCode = pi.ProductionOrderCode and ps.WorkCenterCode = '3010' and ps.DesiredEndDateProcessing <= Convert(date,GetDate()) and ps.DesiredEndDateProcessing >= Convert(date,GetDate()-30)
where  pi.code not in(select pih.ProductionItemCode from base.ProductionItemsHistory pih where pih.ProductionItemCode =pi.Code and  pih.WorkCenterCode = '3010')
) as EDGERemaining,


-- Workstation 5010 DRILLTEQ-V200

(
select count(*)
from base.ProductionItems pi
join base.ProductionSteps ps on ps.ProductionOrderCode = pi.ProductionOrderCode and ps.WorkCenterCode = '5010' and ps.DesiredEndDateProcessing <= Convert(date,GetDate()) and ps.DesiredEndDateProcessing >= Convert(date,GetDate()-30)
where  pi.code not in(select pih.ProductionItemCode from base.ProductionItemsHistory pih where pih.ProductionItemCode =pi.Code and  pih.WorkCenterCode = '5010')
) as V200Remaining,


-- Workstation 5020 CENTATEQ-E310

(
select count(*)
from base.ProductionItems pi
join base.ProductionSteps ps on ps.ProductionOrderCode = pi.ProductionOrderCode and ps.WorkCenterCode = '5020' and ps.DesiredEndDateProcessing <= Convert(date,GetDate()) and ps.DesiredEndDateProcessing >= Convert(date,GetDate()-30)
where  pi.code not in(select pih.ProductionItemCode from base.ProductionItemsHistory pih where pih.ProductionItemCode =pi.Code and  pih.WorkCenterCode = '5020')
) as E310Remaining,


-- Workstation 5070 Sorting/Commissioning

(
select count(*)
from base.ProductionItems pi
join base.ProductionSteps ps on ps.ProductionOrderCode = pi.ProductionOrderCode and ps.WorkCenterCode = '5070' and ps.DesiredEndDateProcessing <= Convert(date,GetDate()) and ps.DesiredEndDateProcessing >= Convert(date,GetDate()-30)
where  pi.code not in(select pih.ProductionItemCode from base.ProductionItemsHistory pih where pih.ProductionItemCode =pi.Code and  pih.WorkCenterCode = '5070')
) as SORTRemaining,


-- Workstation 6010 Preassembly

(
select count(*)
from base.ProductionItems pi
join base.ProductionSteps ps on ps.ProductionOrderCode = pi.ProductionOrderCode and ps.WorkCenterCode = '6010' and ps.Code = 'PREASSEM' and ps.DesiredEndDateProcessing <= Convert(date,GetDate()) and ps.DesiredEndDateProcessing >= Convert(date,GetDate()-30)
where  pi.code not in(select pih.ProductionItemCode from base.ProductionItemsHistory pih where pih.ProductionItemCode =pi.Code and  pih.WorkCenterCode = '6010')
) as PREASSRemaining,


-- Workstation 6010 Assembly

(
select count(*)
from base.ProductionItems pi
join base.ProductionSteps ps on ps.ProductionOrderCode = pi.ProductionOrderCode and ps.WorkCenterCode = '6010' and ps.Code = 'ASSEM' and ps.DesiredEndDateProcessing <= Convert(date,GetDate()) and ps.DesiredEndDateProcessing >= Convert(date,GetDate()-30)
where  pi.code not in(select pih.ProductionItemCode from base.ProductionItemsHistory pih where pih.ProductionItemCode =pi.Code and  pih.WorkCenterCode = '6010')
) as ASSEMRemaining


From base.ProductionOrders po

