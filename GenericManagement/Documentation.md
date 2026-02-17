# 1 ProgrammSettings : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|true|false|Information|100079|ProgramSetting||Management Tiles|false|false|false||true|ProgrammSettings|||||Medium|||0|
# 2 MasterProgressWorkCenterCustomerOrder : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|Information|100106|CustViewMasterProgressWorkCenterCustomerOrder||Management|false|false|false||true|MasterProgressWorkCenterCustomerOrder||||EmptyTile|Medium|||0|
## 2.1 AS1Percentage : Column
|Name|Style|
|--|--|
|AS1Percentage|DetailedProgressbarSalesItem_AS1|
## 2.2 CU1Percentage : Column
|Name|Style|
|--|--|
|CU1Percentage|DetailedProgressbarParts_CU1|
## 2.3 CompletedOrAllowed : Column
|Name|Style|
|--|--|
|CompletedOrAllowed|CompletedOrAllowed|
## 2.4 CNC2Percentage : Column
|Name|Style|
|--|--|
|CNC2Percentage|DetailedProgressbarParts_CNC2|
## 2.5 EB1Percentage : Column
|Name|Style|
|--|--|
|EB1Percentage|DetailedProgressbarParts_EB1|
## 2.6 OverallPercentage : Column
|Name|Style|
|--|--|
|OverallPercentage|DetailedOverall|
## 2.7 PREPercentage : Column
|Name|Style|
|--|--|
|PREPercentage|DetailedProgressbarParts_PRE|
## 2.8 ReworkPercentage : Column
|Name|Style|
|--|--|
|ReworkPercentage|DetailedProgressbarSteps_Rework|
## 2.9 SPPercentage : Column
|Name|Style|
|--|--|
|SPPercentage|DetailedProgressbarParts_SP|
## 2.10 CNC1Percentage : Column
|Name|Style|
|--|--|
|CNC1Percentage|DetailedProgressbarParts_CNC1|
## 2.11 QCPercentage : Column
|Name|Style|
|--|--|
|QCPercentage|DetailedProgressbarParts_QC|
## 2.12 FeedbackRW : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#FAC8C8|1|#1E1E1E|FeedbackRW|ReworkPercentage!=NULL and ReworkPercentage!=100|
## 2.13 FeedbackRWFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#ECECF4|2|#1E1E1E|FeedbackRWFinished|ReworkPercentage!=NULL and ReworkPercentage==100|
## 2.14 B300 : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|CustomerOrderCode|B300|1|CustViewDetailProgressInformation|WorkCenterCode == "CU1"|false|true|B300|CustomerOrderCode||
### 2.14.1 InProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|1|#199619|InProcessing|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 2.14.2 InProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|2|#199619|InProcessingFinished|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 2.14.3 Finished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|3|#199619|Finished|OrderState = 35 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 2.14.4 RWNotStarted : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#FAC8C9|4|#961919|RWNotStarted|OrderState = 0 and ReproductionType = 1 and CurrentTargetQuantity = 0|
### 2.14.5 RWInProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|5|#961919|RWInProcessing|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 2.14.6 RWInProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|6|#961919|RWInProcessingFinished|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
### 2.14.7 RWFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|7|#961919|RWFinished|OrderState = 35 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
## 2.15 EDGETEQ : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|CustomerOrderCode|EDGETEQ|2|CustViewDetailProgressInformation|WorkCenterCode == "EB1"|false|true|EDGETEQ|CustomerOrderCode||
### 2.15.1 InProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|1|#199619|InProcessing|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 2.15.2 InProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|2|#199619|InProcessingFinished|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 2.15.3 Finished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|3|#199619|Finished|OrderState = 35 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 2.15.4 RWNotStarted : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#FAC8C9|4|#961919|RWNotStarted|OrderState = 0 and ReproductionType = 1 and CurrentTargetQuantity = 0|
### 2.15.5 RWInProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|5|#961919|RWInProcessing|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 2.15.6 RWInProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|6|#961919|RWInProcessingFinished|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
### 2.15.7 RWFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|7|#961919|RWFinished|OrderState = 35 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
## 2.16 V200 : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|CustomerOrderCode|V200|3|CustViewDetailProgressInformation|WorkCenterCode == "CNC1"|false|true|V200|CustomerOrderCode||
### 2.16.1 InProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|1|#199619|InProcessing|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 2.16.2 InProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|2|#199619|InProcessingFinished|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 2.16.3 Finished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|3|#199619|Finished|OrderState = 35 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 2.16.4 RWNotStarted : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#FAC8C9|4|#961919|RWNotStarted|OrderState = 0 and ReproductionType = 1 and CurrentTargetQuantity = 0|
### 2.16.5 RWInProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|5|#961919|RWInProcessing|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 2.16.6 RWInProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|6|#961919|RWInProcessingFinished|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
### 2.16.7 RWFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|7|#961919|RWFinished|OrderState = 35 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
## 2.17 E310 : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|CustomerOrderCode|E310|4|CustViewDetailProgressInformation|WorkCenterCode == "CNC2"|false|true|E310|CustomerOrderCode||
### 2.17.1 InProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|1|#199619|InProcessing|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 2.17.2 InProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|2|#199619|InProcessingFinished|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 2.17.3 Finished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|3|#199619|Finished|OrderState = 35 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 2.17.4 RWNotStarted : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#FAC8C9|4|#961919|RWNotStarted|OrderState = 0 and ReproductionType = 1 and CurrentTargetQuantity = 0|
### 2.17.5 RWInProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|5|#961919|RWInProcessing|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 2.17.6 RWInProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|6|#961919|RWInProcessingFinished|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
### 2.17.7 RWFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|7|#961919|RWFinished|OrderState = 35 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
## 2.18 Sorting : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|CustomerOrderCode|Sorting|5|CustViewDetailProgressInformation|WorkCenterCode == "SP"|false|true|Sorting|CustomerOrderCode||
### 2.18.1 InProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|1|#199619|InProcessing|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 2.18.2 InProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|2|#199619|InProcessingFinished|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 2.18.3 Finished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|3|#199619|Finished|OrderState = 35 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 2.18.4 RWNotStarted : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#FAC8C9|4|#961919|RWNotStarted|OrderState = 0 and ReproductionType = 1 and CurrentTargetQuantity = 0|
### 2.18.5 RWInProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|5|#961919|RWInProcessing|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 2.18.6 RWInProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|6|#961919|RWInProcessingFinished|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
### 2.18.7 RWFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|7|#961919|RWFinished|OrderState = 35 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
## 2.19 Preassembly : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|CustomerOrderCode|Preassembly|6|CustViewDetailProgressInformation|WorkCenterCode == "PRE"|false|true|Preassembly|CustomerOrderCode||
### 2.19.1 InProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|1|#199619|InProcessing|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 2.19.2 InProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|2|#199619|InProcessingFinished|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 2.19.3 Finished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|3|#199619|Finished|OrderState = 35 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 2.19.4 RWNotStarted : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#FAC8C9|4|#961919|RWNotStarted|OrderState = 0 and ReproductionType = 1 and CurrentTargetQuantity = 0|
### 2.19.5 RWInProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|5|#961919|RWInProcessing|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 2.19.6 RWInProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|6|#961919|RWInProcessingFinished|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
### 2.19.7 RWFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|7|#961919|RWFinished|OrderState = 35 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
## 2.20 Quality-Check : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|CustomerOrderCode|Quality-Check|7|CustViewDetailProgressInformation|WorkCenterCode == "QC"|false|false|Quality-Check|CustomerOrderCode||
### 2.20.1 InProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|1|#199619|InProcessing|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 2.20.2 InProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|2|#199619|InProcessingFinished|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 2.20.3 Finished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|3|#199619|Finished|OrderState = 35 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 2.20.4 RWNotStarted : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#FAC8C9|4|#961919|RWNotStarted|OrderState = 0 and ReproductionType = 1 and CurrentTargetQuantity = 0|
### 2.20.5 RWInProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|5|#961919|RWInProcessing|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 2.20.6 RWInProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|6|#961919|RWInProcessingFinished|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
### 2.20.7 RWFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|7|#961919|RWFinished|OrderState = 35 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
## 2.21 Assembly : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|CustomerOrderCode|Assembly|8|CustViewDetailProgressInformation|WorkCenterCode == "AS1"|false|true|Assembly|CustomerOrderCode||
### 2.21.1 InProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|1|#199619|InProcessing|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 2.21.2 InProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|2|#199619|InProcessingFinished|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 2.21.3 Finished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|3|#199619|Finished|OrderState = 35 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 2.21.4 RWNotStarted : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#FAC8C9|4|#961919|RWNotStarted|OrderState = 0 and ReproductionType = 1 and CurrentTargetQuantity = 0|
### 2.21.5 RWInProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|5|#961919|RWInProcessing|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 2.21.6 RWInProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|6|#961919|RWInProcessingFinished|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
### 2.21.7 RWFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|7|#961919|RWFinished|OrderState = 35 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
## 2.22 Post-production : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|CustomerOrderCode|Post-production|9|CustViewDetailProgressInformation|ReproductionType = 1 && WorkCenterCode == "CU1"|false|true|Post-production|CustomerOrderCode||
### 2.22.1 RWNotStarted : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#FAC8C9|1|#961919|RWNotStarted|OrderState = 0 and ReproductionType = 1 and CurrentTargetQuantity = 0|
### 2.22.2 RWInProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|2|#961919|RWInProcessing|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 2.22.3 RWInProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|3|#961919|RWInProcessingFinished|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
### 2.22.4 RWFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|4|#961919|RWFinished|OrderState = 35 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
# 3 MasterProgressComponentTypeCustomerOrder : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|Information|100107|CustViewMasterProgressComponentTypeCustomerOrder||Management|false|false|false||true|MasterProgressComponentTypeCustomerOrder||||EmptyTile|Medium|||0|
## 3.1 CustomReleaseToAssemblyLineOne : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|F5|arrow|Release to Assembly Line 1|100064|CustomReleaseToAssemblyLineOne|
### 3.1.1 CustomReleaseToAssemblyLineOne : CommandUserExit
|Name|Order|
|--|--|
|CustomReleaseToAssemblyLineOne|0|
## 3.2 CustomReleaseToAssemblyLineTwo : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|F4|arrow|Release to Assembly Line 2|100064|CustomReleaseToAssemblyLineTwo|
### 3.2.1 CustomReleaseToAssemblyLineTwo : CommandUserExit
|Name|Order|
|--|--|
|CustomReleaseToAssemblyLineTwo|0|
## 3.3 CustomReleaseToAssemblyLineThree : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|F3|arrow|Release to Assembly Line 3|100064|CustomReleaseToAssemblyLineThree|
### 3.3.1 CustomReleaseToAssemblyLineThree : CommandUserExit
|Name|Order|
|--|--|
|CustomReleaseToAssemblyLineThree|0|
## 3.4 CustomReleaseToAssemblyLineFour : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|F2|arrow|Release to Assembly Line 4|100064|CustomReleaseToAssemblyLineFour|
### 3.4.1 CustomReleaseToAssemblyLineFour : CommandUserExit
|Name|Order|
|--|--|
|CustomReleaseToAssemblyLineFour|0|
## 3.5 CarcasePercentage : Column
|Name|Style|
|--|--|
|CarcasePercentage|DetailedProgressbarSteps_Carcase|
## 3.6 CompletedOrAllowed : Column
|Name|Style|
|--|--|
|CompletedOrAllowed|CompletedOrAllowed|
## 3.7 DrawerPercentage : Column
|Name|Style|
|--|--|
|DrawerPercentage|DetailedProgressbarSteps_Drawer|
## 3.8 FrontPercentage : Column
|Name|Style|
|--|--|
|FrontPercentage|DetailedProgressbarSteps_Front|
## 3.9 OverallPercentage : Column
|Name|Style|
|--|--|
|OverallPercentage|DetailedOverall|
## 3.10 ReworkPercentage : Column
|Name|Style|
|--|--|
|ReworkPercentage|DetailedProgressbarSteps_Rework|
## 3.11 HorizontalPercentage : Column
|Name|Style|
|--|--|
|HorizontalPercentage|DetailedProgressbarSteps_HorizontalParts|
## 3.12 LongPercentage : Column
|Name|Style|
|--|--|
|LongPercentage|DetailedProgressbarSteps_LongParts|
## 3.13 VerticalPercentage : Column
|Name|Style|
|--|--|
|VerticalPercentage|DetailedProgressbarSteps_VerticalParts|
## 3.14 FeedbackRW : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#FAC8C9|1|#1E1E1E|FeedbackRW|ReworkPercentage!=NULL and ReworkPercentage!=100|
## 3.15 FeedbackRWFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#ECECF4|2|#1E1E1E|FeedbackRWFinished|ReworkPercentage!=NULL and ReworkPercentage==100|
## 3.16 Front : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|CustomerOrderCode|Front|1|CustViewDetailProgressInformation|ComponentType in (6,7,8,9) && WorkCenterCode == "SP"|false|true|Front|CustomerOrderCode||
### 3.16.1 InProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|1|#199619|InProcessing|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 3.16.2 InProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|2|#199619|InProcessingFinished|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 3.16.3 Finished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|3|#199619|Finished|OrderState = 35 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 3.16.4 RWNotStarted : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#FAC8C8|4|#961919|RWNotStarted|OrderState = 0 and ReproductionType = 1 and CurrentTargetQuantity = 0|
### 3.16.5 RWInProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|5|#961919|RWInProcessing|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 3.16.6 RWInProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|6|#961919|RWInProcessingFinished|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
### 3.16.7 RWFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|7|#961919|RWFinished|OrderState = 35 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
## 3.17 Carcase : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|CustomerOrderCode|Carcase|2|CustViewDetailProgressInformation|ComponentType in (1,2,3,4,5,12,13,20,21,24) && WorkCenterCode == "SP"|false|true|Carcase|CustomerOrderCode||
### 3.17.1 InProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|1|#199619|InProcessing|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 3.17.2 InProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|2|#199619|InProcessingFinished|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 3.17.3 Finished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|3|#199619|Finished|OrderState = 35 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 3.17.4 RWNotStarted : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#FAC8C8|4|#961919|RWNotStarted|OrderState = 0 and ReproductionType = 1 and CurrentTargetQuantity = 0|
### 3.17.5 RWInProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|5|#961919|RWInProcessing|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 3.17.6 RWInProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|6|#961919|RWInProcessingFinished|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
### 3.17.7 RWFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|7|#961919|RWFinished|OrderState = 35 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
## 3.18 Drawer : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|CustomerOrderCode|Drawer|3|CustViewDetailProgressInformation|ComponentType in (17,18,19) && WorkCenterCode == "SP"|false|true|Drawer|CustomerOrderCode||
### 3.18.1 InProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|1|#199619|InProcessing|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 3.18.2 InProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|2|#199619|InProcessingFinished|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 3.18.3 Finished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|3|#199619|Finished|OrderState = 35 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 3.18.4 RWNotStarted : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#FAC8C8|4|#961919|RWNotStarted|OrderState = 0 and ReproductionType = 1 and CurrentTargetQuantity = 0|
### 3.18.5 RWInProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|5|#961919|RWInProcessing|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 3.18.6 RWInProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|6|#961919|RWInProcessingFinished|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
### 3.18.7 RWFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|7|#961919|RWFinished|OrderState = 35 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
## 3.19 Post-production : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|CustomerOrderCode|Post-production|4|CustViewDetailProgressInformation|ReproductionType = 1 && WorkCenterCode == "SP"|false|true|Post-production|CustomerOrderCode||
### 3.19.1 RWNotStarted : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#FAC8C8|4|#961919|RWNotStarted|OrderState = 0 and ReproductionType = 1 and CurrentTargetQuantity = 0|
### 3.19.2 RWInProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|5|#961919|RWInProcessing|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 3.19.3 RWInProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|6|#961919|RWInProcessingFinished|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
### 3.19.4 RWFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|7|#961919|RWFinished|OrderState = 35 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
# 4 MasterProgressOptimization : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|Information|100108|CustViewMasterProgressOptimization||Management|false|false|false||true|MasterProgressOptimization||||EmptyTile|Medium|||0|
## 4.1 B300Percentage : Column
|Name|Style|
|--|--|
|B300Percentage|DetailedProgressbarParts_B300|
## 4.2 CompletedOrAllowed : Column
|Name|Style|
|--|--|
|CompletedOrAllowed|CompletedOrAllowed|
## 4.3 E310Percentage : Column
|Name|Style|
|--|--|
|E310Percentage|DetailedProgressbarParts_E310|
## 4.4 EDGETEQPercentage : Column
|Name|Style|
|--|--|
|EDGETEQPercentage|DetailedProgressbarParts_EDGETEQ|
## 4.5 OverallPercentage : Column
|Name|Style|
|--|--|
|OverallPercentage|DetailedOverall|
## 4.6 ReworkPercentage : Column
|Name|Style|
|--|--|
|ReworkPercentage|DetailedProgressbarSteps_Rework|
## 4.7 SORTPercentage : Column
|Name|Style|
|--|--|
|SORTPercentage|DetailedProgressbarParts_SORT|
## 4.8 V200Percentage : Column
|Name|Style|
|--|--|
|V200Percentage|DetailedProgressbarParts_V200|
## 4.9 FeedbackRW : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#FAC8C8|1|#1E1E1E|FeedbackRW|ReworkPercentage!=NULL and ReworkPercentage!=100|
## 4.10 FeedbackRWFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#ECECF4|2|#1E1E1E|FeedbackRWFinished|ReworkPercentage!=NULL and ReworkPercentage==100|
## 4.11 B300 : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|OptimizationCode|B300|1|CustViewDetailProgressInformation|WorkCenterCode == "CU1"|false|true|B300|OptimizationCode||
### 4.11.1 InProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|1|#199619|InProcessing|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 4.11.2 InProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|2|#199619|InProcessingFinished|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 4.11.3 Finished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|3|#199619|Finished|OrderState = 35 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 4.11.4 RWNotStarted : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#FAC8C8|4|#961919|RWNotStarted|OrderState = 0 and ReproductionType = 1 and CurrentTargetQuantity = 0|
### 4.11.5 RWInProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|5|#961919|RWInProcessing|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 4.11.6 RWInProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|6|#961919|RWInProcessingFinished|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
### 4.11.7 RWFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|7|#961919|RWFinished|OrderState = 35 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
## 4.12 EDGETEQ : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|OptimizationCode|EDGETEQ|2|CustViewDetailProgressInformation|WorkCenterCode == "EB1"|false|true|EDGETEQ|OptimizationCode||
### 4.12.1 InProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|1|#199619|InProcessing|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 4.12.2 InProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|2|#199619|InProcessingFinished|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 4.12.3 Finished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|3|#199619|Finished|OrderState = 35 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 4.12.4 RWNotStarted : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#FAC8C8|4|#961919|RWNotStarted|OrderState = 0 and ReproductionType = 1 and CurrentTargetQuantity = 0|
### 4.12.5 RWInProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|5|#961919|RWInProcessing|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 4.12.6 RWInProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|6|#961919|RWInProcessingFinished|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
### 4.12.7 RWFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|7|#961919|RWFinished|OrderState = 35 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
## 4.13 V200 : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|OptimizationCode|V200|3|CustViewDetailProgressInformation|WorkCenterCode == "CNC1"|false|true|V200|OptimizationCode||
### 4.13.1 InProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|1|#199619|InProcessing|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 4.13.2 InProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|2|#199619|InProcessingFinished|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 4.13.3 Finished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|3|#199619|Finished|OrderState = 35 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 4.13.4 RWNotStarted : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#FAC8C8|4|#961919|RWNotStarted|OrderState = 0 and ReproductionType = 1 and CurrentTargetQuantity = 0|
### 4.13.5 RWInProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|5|#961919|RWInProcessing|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 4.13.6 RWInProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|6|#961919|RWInProcessingFinished|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
### 4.13.7 RWFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|7|#961919|RWFinished|OrderState = 35 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
## 4.14 E310 : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|OptimizationCode|E310|4|CustViewDetailProgressInformation|WorkCenterCode == "CNC2"|false|true|E310|OptimizationCode||
### 4.14.1 InProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|1|#199619|InProcessing|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 4.14.2 InProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|2|#199619|InProcessingFinished|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 4.14.3 Finished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|3|#199619|Finished|OrderState = 35 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 4.14.4 RWNotStarted : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#FAC8C8|4|#961919|RWNotStarted|OrderState = 0 and ReproductionType = 1 and CurrentTargetQuantity = 0|
### 4.14.5 RWInProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|5|#961919|RWInProcessing|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 4.14.6 RWInProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|6|#961919|RWInProcessingFinished|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
### 4.14.7 RWFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|7|#961919|RWFinished|OrderState = 35 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
## 4.15 Sorting : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|OptimizationCode|Sorting|5|CustViewDetailProgressInformation|WorkCenterCode == "SP"|false|true|Sorting|OptimizationCode||
### 4.15.1 InProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|1|#199619|InProcessing|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 4.15.2 InProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|2|#199619|InProcessingFinished|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 4.15.3 Finished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|3|#199619|Finished|OrderState = 35 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 4.15.4 RWNotStarted : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#FAC8C8|4|#961919|RWNotStarted|OrderState = 0 and ReproductionType = 1 and CurrentTargetQuantity = 0|
### 4.15.5 RWInProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|5|#961919|RWInProcessing|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 4.15.6 RWInProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|6|#961919|RWInProcessingFinished|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
### 4.15.7 RWFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|7|#961919|RWFinished|OrderState = 35 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
## 4.16 Post-production : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|OptimizationCode|Post-production|6|CustViewDetailProgressInformation|ReproductionType = 1 && WorkCenterCode == "SP"|false|true|Post-production|OptimizationCode||
### 4.16.1 RWNotStarted : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#FAC8C8|1|#961919|RWNotStarted|OrderState = 0 and ReproductionType = 1 and CurrentTargetQuantity = 0|
### 4.16.2 RWInProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|2|#961919|RWInProcessing|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 4.16.3 RWInProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|3|#961919|RWInProcessingFinished|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
### 4.16.4 RWFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|4|#961919|RWFinished|OrderState = 35 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
# 5 MasterProgressBulk : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|Information|100109|CustViewMasterProgressBulk||Management|false|false|false||true|MasterProgressBulk||||EmptyTile|Medium|||0|
## 5.1 AS1Percentage : Column
|Name|Style|
|--|--|
|AS1Percentage|DetailedProgressbarSalesItem_AS1|
## 5.2 CNC1Percentage : Column
|Name|Style|
|--|--|
|CNC1Percentage|DetailedProgressbarParts_CNC1|
## 5.3 CompletedOrAllowed : Column
|Name|Style|
|--|--|
|CompletedOrAllowed|CompletedOrAllowed|
## 5.4 CNC2Percentage : Column
|Name|Style|
|--|--|
|CNC2Percentage|DetailedProgressbarParts_CNC2|
## 5.5 CU1Percentage : Column
|Name|Style|
|--|--|
|CU1Percentage|DetailedProgressbarParts_CU1|
## 5.6 OverallPercentage : Column
|Name|Style|
|--|--|
|OverallPercentage|DetailedOverall|
## 5.7 EB1Percentage : Column
|Name|Style|
|--|--|
|EB1Percentage|DetailedProgressbarParts_EB1|
## 5.8 ReworkPercentage : Column
|Name|Style|
|--|--|
|ReworkPercentage|DetailedProgressbarSteps_Rework|
## 5.9 PREPercentage : Column
|Name|Style|
|--|--|
|PREPercentage|DetailedProgressbarParts_PRE|
## 5.10 SPPercentage : Column
|Name|Style|
|--|--|
|SPPercentage|DetailedProgressbarParts_SP|
## 5.11 QCPercentage : Column
|Name|Style|
|--|--|
|QCPercentage|DetailedProgressbarParts_QC|
## 5.12 FeedbackRW : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#FAC8C9|1|#1E1E1E|FeedbackRW|ReworkPercentage!=NULL and ReworkPercentage!=100|
## 5.13 FeedbackRWFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#ECECF4|2|#1E1E1E|FeedbackRWFinished|ReworkPercentage!=NULL and ReworkPercentage==100|
## 5.14 B300 : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|PlanningNumber|B300|1|CustViewDetailProgressInformation|WorkCenterCode == "CU1"|false|true|B300|PlanningNumber||
### 5.14.1 InProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|1|#199619|InProcessing|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 5.14.2 InProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|2|#199619|InProcessingFinished|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 5.14.3 Finished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|3|#199619|Finished|OrderState = 35 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 5.14.4 RWNotStarted : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#FAC8C9|4|#961919|RWNotStarted|OrderState = 0 and ReproductionType = 1 and CurrentTargetQuantity = 0|
### 5.14.5 RWInProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|5|#961919|RWInProcessing|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 5.14.6 RWInProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|6|#961919|RWInProcessingFinished|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
### 5.14.7 RWFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|7|#961919|RWFinished|OrderState = 35 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
## 5.15 EDGETEQ : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|PlanningNumber|EDGETEQ|2|CustViewDetailProgressInformation|WorkCenterCode == "EB1"|false|true|EDGETEQ|PlanningNumber||
### 5.15.1 InProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|1|#199619|InProcessing|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 5.15.2 InProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|2|#199619|InProcessingFinished|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 5.15.3 Finished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|3|#199619|Finished|OrderState = 35 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 5.15.4 RWNotStarted : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#FAC8C9|4|#961919|RWNotStarted|OrderState = 0 and ReproductionType = 1 and CurrentTargetQuantity = 0|
### 5.15.5 RWInProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|5|#961919|RWInProcessing|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 5.15.6 RWInProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|6|#961919|RWInProcessingFinished|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
### 5.15.7 RWFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|7|#961919|RWFinished|OrderState = 35 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
## 5.16 V200 : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|PlanningNumber|V200|3|CustViewDetailProgressInformation|WorkCenterCode == "CNC1"|false|true|V200|PlanningNumber||
### 5.16.1 InProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|1|#199619|InProcessing|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 5.16.2 InProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|2|#199619|InProcessingFinished|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 5.16.3 Finished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|3|#199619|Finished|OrderState = 35 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 5.16.4 RWNotStarted : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#FAC8C9|4|#961919|RWNotStarted|OrderState = 0 and ReproductionType = 1 and CurrentTargetQuantity = 0|
### 5.16.5 RWInProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|5|#961919|RWInProcessing|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 5.16.6 RWInProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|6|#961919|RWInProcessingFinished|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
### 5.16.7 RWFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|7|#961919|RWFinished|OrderState = 35 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
## 5.17 E310 : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|PlanningNumber|E310|4|CustViewDetailProgressInformation|WorkCenterCode == "CNC2"|false|true|E310|PlanningNumber||
### 5.17.1 InProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|1|#199619|InProcessing|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 5.17.2 InProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|2|#199619|InProcessingFinished|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 5.17.3 Finished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|3|#199619|Finished|OrderState = 35 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 5.17.4 RWNotStarted : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#FAC8C9|4|#961919|RWNotStarted|OrderState = 0 and ReproductionType = 1 and CurrentTargetQuantity = 0|
### 5.17.5 RWInProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|5|#961919|RWInProcessing|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 5.17.6 RWInProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|6|#961919|RWInProcessingFinished|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
### 5.17.7 RWFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|7|#961919|RWFinished|OrderState = 35 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
## 5.18 Sorting : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|PlanningNumber|Sorting|5|CustViewDetailProgressInformation|WorkCenterCode == "SP"|false|true|Sorting|PlanningNumber||
### 5.18.1 InProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|1|#199619|InProcessing|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 5.18.2 InProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|2|#199619|InProcessingFinished|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 5.18.3 Finished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|3|#199619|Finished|OrderState = 35 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 5.18.4 RWNotStarted : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#FAC8C8|4|#961919|RWNotStarted|OrderState = 0 and ReproductionType = 1 and CurrentTargetQuantity = 0|
### 5.18.5 RWInProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|5|#961919|RWInProcessing|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 5.18.6 RWInProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|6|#961919|RWInProcessingFinished|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
### 5.18.7 RWFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|7|#961919|RWFinished|OrderState = 35 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
## 5.19 Preassembly : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|PlanningNumber|Preassembly|6|CustViewDetailProgressInformation|WorkCenterCode == "PRE"|false|true|Preassembly|PlanningNumber||
### 5.19.1 InProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|1|#199619|InProcessing|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 5.19.2 InProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|2|#199619|InProcessingFinished|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 5.19.3 Finished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|3|#199619|Finished|OrderState = 35 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 5.19.4 RWNotStarted : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#FAC8C8|4|#961919|RWNotStarted|OrderState = 0 and ReproductionType = 1 and CurrentTargetQuantity = 0|
### 5.19.5 RWInProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|5|#961919|RWInProcessing|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 5.19.6 RWInProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|6|#961919|RWInProcessingFinished|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
### 5.19.7 RWFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|7|#961919|RWFinished|OrderState = 35 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
## 5.20 Quality control : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|PlanningNumber|Quality control|7|CustViewDetailProgressInformation|WorkCenterCode == "QC"|false|true|Quality control|PlanningNumber||
### 5.20.1 InProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|1|#199619|InProcessing|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 5.20.2 InProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|2|#199619|InProcessingFinished|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 5.20.3 Finished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|3|#199619|Finished|OrderState = 35 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 5.20.4 RWNotStarted : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#FAC8C8|4|#961919|RWNotStarted|OrderState = 0 and ReproductionType = 1 and CurrentTargetQuantity = 0|
### 5.20.5 RWInProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|5|#961919|RWInProcessing|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 5.20.6 RWInProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|6|#961919|RWInProcessingFinished|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
### 5.20.7 RWFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|7|#961919|RWFinished|OrderState = 35 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
## 5.21 Assembly : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|PlanningNumber|Assembly|8|CustViewDetailProgressInformation|WorkCenterCode == "AS1"|false|true|Assembly|PlanningNumber||
### 5.21.1 InProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|1|#199619|InProcessing|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 5.21.2 InProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|2|#199619|InProcessingFinished|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 5.21.3 Finished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|3|#199619|Finished|OrderState = 35 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 5.21.4 RWNotStarted : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#FAC8C8|4|#961919|RWNotStarted|OrderState = 0 and ReproductionType = 1 and CurrentTargetQuantity = 0|
### 5.21.5 RWInProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|5|#961919|RWInProcessing|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 5.21.6 RWInProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|6|#961919|RWInProcessingFinished|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
### 5.21.7 RWFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|7|#961919|RWFinished|OrderState = 35 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
## 5.22 Post-production : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|PlanningNumber|Post-production|9|CustViewDetailProgressInformation|ReproductionType = 1 && WorkCenterCode == "PRE"|false|true|Post-production|PlanningNumber||
### 5.22.1 RWNotStarted : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#FAC8C8|1|#961919|RWNotStarted|OrderState = 0 and ReproductionType = 1 and CurrentTargetQuantity = 0|
### 5.22.2 RWInProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|2|#961919|RWInProcessing|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 5.22.3 RWInProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|3|#961919|RWInProcessingFinished|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
### 5.22.4 RWFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|4|#961919|RWFinished|OrderState = 35 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
# 6 MasterProgressComponentTypeBulk : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|Information|100110|CustViewMasterProgressComponentTypeBulk||Management|false|false|false||true|MasterProgressComponentTypeBulk||||EmptyTile|Medium|||0|
## 6.1 CustomReleaseToAssemblyLineOne : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|F5|arrow|Release to Assembly Line 1|100064|CustomReleaseToAssemblyLineOne|
### 6.1.1 CustomReleaseToAssemblyLineOne : CommandUserExit
|Name|Order|
|--|--|
|CustomReleaseToAssemblyLineOne|0|
## 6.2 CustomReleaseToAssemblyLineTwo : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|F4|arrow|Release to Assembly Line 2|100064|CustomReleaseToAssemblyLineTwo|
### 6.2.1 CustomReleaseToAssemblyLineTwo : CommandUserExit
|Name|Order|
|--|--|
|CustomReleaseToAssemblyLineTwo|0|
## 6.3 CustomReleaseToAssemblyLineThree : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|F3|arrow|Release to Assembly Line 3|100064|CustomReleaseToAssemblyLineThree|
### 6.3.1 CustomReleaseToAssemblyLineThree : CommandUserExit
|Name|Order|
|--|--|
|CustomReleaseToAssemblyLineThree|0|
## 6.4 CustomReleaseToAssemblyLineFour : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|F2|arrow|Release to Assembly Line 4|100064|CustomReleaseToAssemblyLineFour|
### 6.4.1 CustomReleaseToAssemblyLineFour : CommandUserExit
|Name|Order|
|--|--|
|CustomReleaseToAssemblyLineFour|0|
## 6.5 CompletedOrAllowed : Column
|Name|Style|
|--|--|
|CompletedOrAllowed|CompletedOrAllowed|
## 6.6 FrontPercentage : Column
|Name|Style|
|--|--|
|FrontPercentage|DetailedProgressbarSteps_Front|
## 6.7 HorizontalPercentage : Column
|Name|Style|
|--|--|
|HorizontalPercentage|DetailedProgressbarSteps_HorizontalParts|
## 6.8 OverallPercentage : Column
|Name|Style|
|--|--|
|OverallPercentage|DetailedOverall|
## 6.9 ReworkPercentage : Column
|Name|Style|
|--|--|
|ReworkPercentage|DetailedProgressbarSteps_Rework|
## 6.10 VerticalPercentage : Column
|Name|Style|
|--|--|
|VerticalPercentage|DetailedProgressbarSteps_VerticalParts|
## 6.11 FeedbackRW : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#FAC8C9|1|#1E1E1E|FeedbackRW|ReworkPercentage!=NULL and ReworkPercentage!=100|
## 6.12 FeedbackRWFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#ECECF4|2|#1E1E1E|FeedbackRWFinished|ReworkPercentage!=NULL and ReworkPercentage==100|
## 6.13 Front : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|PlanningNumber|Front|1|CustViewDetailProgressInformation|ComponentType in (6,7,8,9) && WorkCenterCode == "SP"|false|true|Front|PlanningNumber||
### 6.13.1 InProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|1|#199619|InProcessing|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 6.13.2 InProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|2|#199619|InProcessingFinished|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 6.13.3 Finished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|3|#199619|Finished|OrderState = 35 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 6.13.4 RWNotStarted : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#FAC8C8|4|#961919|RWNotStarted|OrderState = 0 and ReproductionType = 1 and CurrentTargetQuantity = 0|
### 6.13.5 RWInProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|5|#961919|RWInProcessing|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 6.13.6 RWInProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|6|#961919|RWInProcessingFinished|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
### 6.13.7 RWFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|7|#961919|RWFinished|OrderState = 35 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
## 6.14 Vertical : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|PlanningNumber|Vertical|2|CustViewDetailProgressInformation|ComponentType in (1,18,19) && WorkCenterCode == "SP"|false|true|Vertical|PlanningNumber||
### 6.14.1 InProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|1|#199619|InProcessing|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 6.14.2 InProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|2|#199619|InProcessingFinished|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 6.14.3 Finished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|3|#199619|Finished|OrderState = 35 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 6.14.4 RWNotStarted : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#FAC8C8|4|#961919|RWNotStarted|OrderState = 0 and ReproductionType = 1 and CurrentTargetQuantity = 0|
### 6.14.5 RWInProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|5|#961919|RWInProcessing|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 6.14.6 RWInProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|6|#961919|RWInProcessingFinished|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
### 6.14.7 RWFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|7|#961919|RWFinished|OrderState = 35 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
## 6.15 Horizontal : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|PlanningNumber|Horizontal|3|CustViewDetailProgressInformation|ComponentType in (2,3,4,5,12,13,17,20,21,24) && WorkCenterCode == "SP"|false|true|Horizontal|PlanningNumber||
### 6.15.1 InProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|1|#199619|InProcessing|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 6.15.2 InProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|2|#199619|InProcessingFinished|OrderState = 25 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 6.15.3 Finished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|3|#199619|Finished|OrderState = 35 and ReproductionType = 0 and CurrentTargetQuantity = DesiredTargetQuantity|
### 6.15.4 RWNotStarted : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#FAC8C8|4|#961919|RWNotStarted|OrderState = 0 and ReproductionType = 1 and CurrentTargetQuantity = 0|
### 6.15.5 RWInProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|5|#961919|RWInProcessing|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 6.15.6 RWInProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|6|#961919|RWInProcessingFinished|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
### 6.15.7 RWFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|7|#961919|RWFinished|OrderState = 35 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
## 6.16 Post-production : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|PlanningNumber|Post-production|4|CustViewDetailProgressInformation|ReproductionType = 1 && WorkCenterCode == "SP"|false|true|Post-production|PlanningNumber||
### 6.16.1 RWNotStarted : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#FAC8C8|1|#961919|RWNotStarted|OrderState = 0 and ReproductionType = 1 and CurrentTargetQuantity = 0|
### 6.16.2 RWInProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|2|#961919|RWInProcessing|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity >= 0 and CurrentTargetQuantity != DesiredTargetQuantity|
### 6.16.3 RWInProcessingFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|3|#961919|RWInProcessingFinished|OrderState = 25 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
### 6.16.4 RWFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|4|#961919|RWFinished|OrderState = 35 and ReproductionType = 1 and CurrentTargetQuantity = DesiredTargetQuantity|
# 7 MasterManualFeedback : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|Information|100141|CustViewMasterManualFeedback||Action|false|false|false||true|MasterManualFeedback||||table|Medium|||0|
## 7.1 FeedbackCutting : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|F11|cut|Feedback cutting|100064|FeedbackCutting|
### 7.1.1 FeedbackCutting : CommandUserExit
|Name|Order|
|--|--|
|FeedbackCutting|0|
## 7.2 FeedbackEdgebanding : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|F10|edge|Feedback edgebanding|100064|FeedbackEdgebanding|
### 7.2.1 FeedbackEdgebanding : CommandUserExit
|Name|Order|
|--|--|
|FeedbackEdgebanding|0|
## 7.3 FeedbackCNC : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|F9|drill|Feedback drilling|100064|FeedbackCNC|
### 7.3.1 FeedbackCNC : CommandUserExit
|Name|Order|
|--|--|
|FeedbackCNC|0|
## 7.4 FeedbackSortingAndPicking : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|F8|sort|Feedback sorting|100064|FeedbackSortingAndPicking|
### 7.4.1 FeedbackSortingAndPicking : CommandUserExit
|Name|Order|
|--|--|
|FeedbackSortingAndPicking|0|
## 7.5 FeedbackPreassembly : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|F7|Drawer|Feedback preassembly|100064|FeedbackPreassembly|
### 7.5.1 FeedbackPreassembly : CommandUserExit
|Name|Order|
|--|--|
|FeedbackPreassembly|0|
## 7.6 CustomFeedbackCompleteCustomerOrder : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|F2|Assembly|Feedback customer order|100064|CustomFeedbackCompleteCustomerOrder|
### 7.6.1 CustomFeedbackCompleteCustomerOrder : CommandUserExit
|Name|Order|
|--|--|
|CustomFeedbackCompleteCustomerOrder|0|
## 7.7 FeedbackQualityControl : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|F12|qualitycheck|Feedback quality control|100064|FeedbackQualityControl|
### 7.7.1 FeedbackQualityControl : CommandUserExit
|Name|Order|
|--|--|
|FeedbackQualityControl|0|
## 7.8 DropDownWorkCenterCommand : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|F1|EmptyTile|Create Feedback|100064|DropDownWorkCenterCommand|
### 7.8.1 DropDownWorkCenterCommand : CommandUserExit
|Name|Order|
|--|--|
|DropDownWorkCenterCommand|0|
## 7.9 Completed : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|1|#199619|Completed|ProductionState = 35|
## 7.10 InProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|2|#199619|InProcessing|ProductionState = 25|
## 7.11 ProductionSteps : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|ProductionItemCode|ProductionSteps|1|CustViewDetailManualFeedback||false|true|ProductionSteps|ProductionItemCode||
### 7.11.1 Feedback : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|1|#199619|Feedback|FeedbackState=2|
# 8 MasterUnfinishedPart : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|Information|100161|CustViewMasterUnfinishedPart||Management|false|false|false||true|MasterUnfinishedPart||||EmptyTile|Medium|||0|
# 9 ReportBinding : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|Information|100189|CustViewReportBinding||Management|false|false|false||true|ReportBinding||||printer|Medium|||0|
## 9.1 ReportBindingRegenerateReports : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|None|refresh|New Generation of Report(s)|100064|ReportBindingRegenerateReports|
### 9.1.1 ReportBindingRegenerateReports : CommandUserExit
|Name|Order|
|--|--|
|ReportBindingRegenerateReports|0|
## 9.2 PrintPDFOpenPrinterDialog : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|None||Printing / Print Settings|100064|PrintPDFOpenPrinterDialog|
### 9.2.1 PrintPDFOpenPrinterDialog : CommandUserExit
|Name|Order|
|--|--|
|PrintPDFOpenPrinterDialog|0|
## 9.3 PrintPDFQuantityDialog : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|None||Printing w Qty Info|100064|PrintPDFQuantityDialog|
### 9.3.1 PrintPDFQuantityDialog : CommandUserExit
|Name|Order|
|--|--|
|PrintPDFQuantityDialog|0|
## 9.4 PrintPDFOnDefaultPrinter : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|None|printer|Printing on default Printer|100064|PrintPDFOnDefaultPrinter|
### 9.4.1 PrintPDFOnDefaultPrinter : CommandUserExit
|Name|Order|
|--|--|
|PrintPDFOnDefaultPrinter|0|
## 9.5 ReportBinary : DetailImageView
|DisplayName|DisplayOrder|Field|IsSecondary|Name|
|--|--|--|--|--|
|ReportBinary|1|BinariesSequence|false|ReportBinary|
# 10 Optimizations : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|Production|10174|Optimization||Action|false|false|false||true|Optimizations|||||Small|||0|
## 10.1 OptimizationBoards : DetailListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DisplayName|DisplayOrder|Filter|IsSecondary|IsSortable|Name|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|
|true|false|false|false|false|Optimierte Platten|3||false|true|OptimizationBoards||
## 10.2 OptimizationMaterialToOptimize : DetailListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DisplayName|DisplayOrder|Filter|IsSecondary|IsSortable|Name|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|
|true|false|false|false|false|Optimierte Materialien|4||false|true|OptimizationMaterialToOptimize||
## 10.3 ProductionItems : DetailListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DisplayName|DisplayOrder|Filter|IsSecondary|IsSortable|Name|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|
|true|false|false|false|false|Production Items|5||false|true|ProductionItems||
## 10.4 ProductionItemsToOptimize : DetailListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DisplayName|DisplayOrder|Filter|IsSecondary|IsSortable|Name|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|
|true|false|false|false|false|Production Items To Optimize|6||false|true|ProductionItemsToOptimize||
## 10.5 OptimizationCuttingPlans : DetailListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DisplayName|DisplayOrder|Filter|IsSecondary|IsSortable|Name|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|
|true|false|false|false|false|Optimierte Schnittpläne|1||false|true|OptimizationCuttingPlans||
## 10.6 OptimizationParts : DetailListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DisplayName|DisplayOrder|Filter|IsSecondary|IsSortable|Name|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|
|true|false|false|false|false|Optimierte Teile|2||false|true|OptimizationParts||
# 11 OptimizationRulesAreas : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|true|true|true|Production|10235|OptimizationRulesArea||Management|false|false|false||true|OptimizationRulesAreas|||||Medium|||0|
# 12 OptimizationRules : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|true|true|true|Production|10236|OptimizationRule||Management|false|false|false||true|OptimizationRules|||||Medium|||0|
# 13 OptimizationRulesAllocations : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|true|true|true|Production|10237|OptimizationRulesAllocation||Management|false|false|false||true|OptimizationRulesAllocations|||||Medium|||0|
# 14 OptimizationMethod : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|true|true|true|Production|10215|OptimizationMethod||Action|false|false|false||true|OptimizationMethod||||EmptyTile.png|Medium|EmptyTileView|EmptyTileViewMode|0|
# 15 CustomerOrder : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|true|false|false|Production|10016|CustomerOrder||Action|false|false|false||true|CustomerOrder||||EmptyTile.png|Medium|EmptyTileView|EmptyTileViewMode|0|
## 15.1 ProductionOrders : DetailListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DisplayName|DisplayOrder|Filter|IsSecondary|IsSortable|Name|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|Fertigungsaufträge|1||false|true|ProductionOrders||
## 15.2 ProductionOrderSalesArticle : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|CustomerOrderCode|Artikel-Fertigungsaufträge|2|ProductionOrder|OrderType="SalesArticle"|false|true|ProductionOrderSalesArticle|Code||
# 16 ProductionOrder : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|Production|10000|ProductionOrder||Action|false|false|false||true|ProductionOrder||||EmptyTile.png|Medium|EmptyTileView|EmptyTileViewMode|0|
## 16.1 PoInfo : DetailEntityView
|DetailProperties|DisplayName|DisplayOrder|Entity|IsSecondary|Name|ParentProperties|
|--|--|--|--|--|--|--|
|ProductionOrderCode|PoInfo|1|CustViewProductionOrderDetailPanelInformation|false|PoInfo|Code|
## 16.2 ResetBracketToStaging : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|F4|EmptyTile.png|Aufträge für den erneuten Import vorbereiten|Alle Aufträge des Brackets für den erneuten Import vorbereiten|ResetBracketToStaging|
### 16.2.1 ResetBracketToStaging : CommandUserExit
|Name|Order|
|--|--|
|ResetBracketToStaging|1|
## 16.3 NavigateToImportMessage : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|None|EmptyTile.png|Importmeldungen anzeigen|Importmeldungen anzeigen|NavigateToImportMessage|
### 16.3.1 NavigateToImportMessage : CommandUserExit
|Name|Order|
|--|--|
|NavigateToImportMessage|1|
## 16.4 ProductionSteps : DetailListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DisplayName|DisplayOrder|Filter|IsSecondary|IsSortable|Name|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|Arbeitsgänge|1|DisposeState="Scheduled"|false|true|ProductionSteps||
### 16.4.1 StatusBeendet : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#A8FAC5|1|#199619|StatusBeendet|ProductionState="Finished"|
## 16.5 ProductionItems : DetailListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DisplayName|DisplayOrder|Filter|IsSecondary|IsSortable|Name|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|Teile|2||false|true|ProductionItems||
## 16.6 Material : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|ProductionOrderCode|Material|3|ViewMaterial||false|true|Material|Code||
## 16.7 Oberfläche : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|ProductionOrderCode|Oberflächen|4|ViewSurface||false|true|Oberfläche|Code||
## 16.8 EdgeProfiles : DetailListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DisplayName|DisplayOrder|Filter|IsSecondary|IsSortable|Name|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|Kanten|5|ProductionStep.DisposeState="Scheduled" && (EdgeToStepsType == EdgeToStepsType.undefined \|\| EdgeToStepsType == EdgeToStepsType.UtilizedAtStep)|false|true|EdgeProfiles||
## 16.9 EdgeProfilesImport : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|ProductionOrderCode|Kanten (Importdaten)|6|EdgeProfile|ProductionStep=null|false|true|EdgeProfilesImport|Code||
## 16.10 EdgeGrooves : DetailListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DisplayName|DisplayOrder|Filter|IsSecondary|IsSortable|Name|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|Nuten|7|ProductionStep.DisposeState="Scheduled"|false|true|EdgeGrooves||
## 16.11 EdgeGroovesImport : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|ProductionOrderCode|Nuten (Importdaten)|8|EdgeGroove|ProductionStep=null|false|true|EdgeGroovesImport|Code||
## 16.12 Beschläge : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|ProductionOrderCode|Beschläge|9|ViewFitting||false|true|Beschläge|Code||
## 16.13 ProcessingData : DetailListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DisplayName|DisplayOrder|Filter|IsSecondary|IsSortable|Name|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|Bearbeitungsdaten|10|ProductionStep.DisposeState="Scheduled"|false|true|ProcessingData||
## 16.14 Strukturanzeige : DetailTreeView
|DisplayName|DisplayOrder|Field|IsSecondary|Name|
|--|--|--|--|--|
|Baumansicht|11||false|Strukturanzeige|
## 16.15 Binaries : DetailImageView
|DisplayName|DisplayOrder|Field|IsSecondary|Name|
|--|--|--|--|--|
|Zeichnungen|12|Binaries|false|Binaries|
## 16.16 PossibleRoutes : DetailListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DisplayName|DisplayOrder|Filter|IsSecondary|IsSortable|Name|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|Alternative Fertigungswege|13||false|true|PossibleRoutes||
### 16.16.1 DisposeStateAlternative : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#E7CEEC|1|#000000|DisposeStateAlternative|DisposeState="Alternative"|
### 16.16.2 ChangeScheduledRouteCommand : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|None|EmptyTile.png|Fertigungsweg auf planmäßig ändern|Fertigungsweg auf planmäßig ändern|ChangeScheduledRouteCommand|
#### 16.16.2.1 ChangeScheduledRouteCommand : CommandUserExit
|Name|Order|
|--|--|
|ChangeScheduledRouteCommand|1|
## 16.17 EdgePasses : DetailListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DisplayName|DisplayOrder|Filter|IsSecondary|IsSortable|Name|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|Kantendurchläufe|14|ProductionStep.DisposeState="Scheduled"|false|true|EdgePasses||
## 16.18 ProductionOrderGeometryDetailView : GenericDetailView
|DisplayName|DisplayOrder|IsSecondary|Name|Parameter|View|ViewModel|
|--|--|--|--|--|--|--|
|Grafische Anzeige|15|false|ProductionOrderGeometryDetailView|MinimumAspectRatioForLeftInfoText=1.5;DisabledEdgeTexts=98\|99|ProductionOrderGeometryDetailView|ProductionOrderGeometryDetailViewModel|
## 16.19 FehlerBeimAuftrag : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CD5C5C|CustomErrorState|1|#FFFFFF|FehlerBeimAuftrag|CustomErrorState !="ValidData"|
## 16.20 AuftragGesperrt : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CD5C5C|ReleaseState|2|#FFFFFF|AuftragGesperrt|ReleaseState="NotReleased"|
## 16.21 StatusBeendet : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#A8FAC5|1|#199619|StatusBeendet|ProductionState="Finished"|
## 16.22 StatusInBearbeitung : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#EEE8AA|2|#199619|StatusInBearbeitung|ProductionState="Processing"|
# 17 ProductionOrderAllData : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|Production|10202|ProductionOrder||Management2|false|false|false||true|ProductionOrderAllData||||EmptyTile.png|Medium|EmptyTileView|EmptyTileViewMode|0|
## 17.1 ResetBracketToStaging : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|F4|EmptyTile.png|Aufträge für den erneuten Import vorbereiten|Alle Aufträge des Brackets für den erneuten Import vorbereiten|ResetBracketToStaging|
### 17.1.1 ResetBracketToStaging : CommandUserExit
|Name|Order|
|--|--|
|ResetBracketToStaging|1|
## 17.2 NavigateToImportMessage : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|None|EmptyTile.png|Importmeldungen anzeigen|Importmeldungen anzeigen|NavigateToImportMessage|
### 17.2.1 NavigateToImportMessage : CommandUserExit
|Name|Order|
|--|--|
|NavigateToImportMessage|1|
## 17.3 ProductionSteps : DetailListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DisplayName|DisplayOrder|Filter|IsSecondary|IsSortable|Name|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|Arbeitsgänge|1||false|true|ProductionSteps||
### 17.3.1 StatusBeendet : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#A8FAC5|1|#199619|StatusBeendet|ProductionState="Finished"|
### 17.3.2 DisposeStateAlternative : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#E7CEEC|2|#000000|DisposeStateAlternative|DisposeState="Alternative"|
## 17.4 ProductionItems : DetailListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DisplayName|DisplayOrder|Filter|IsSecondary|IsSortable|Name|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|Teile|2||false|true|ProductionItems||
## 17.5 Material : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|ProductionOrderCode|Material|3|ViewMaterial||false|true|Material|Code||
## 17.6 Oberfläche : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|ProductionOrderCode|Oberflächen|4|ViewSurface||false|true|Oberfläche|Code||
## 17.7 EdgeProfiles : DetailListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DisplayName|DisplayOrder|Filter|IsSecondary|IsSortable|Name|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|Kanten|5||false|true|EdgeProfiles||
### 17.7.1 DisposeStateAlternative : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#E7CEEC|1|#000000|DisposeStateAlternative|ProductionStep != null  &&  ProductionStep.DisposeState="Alternative"|
### 17.7.2 ProductionStepNull : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#CCD0D8|2|#000000|ProductionStepNull|ProductionStep=null|
## 17.8 EdgeProfilesImport : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|ProductionOrderCode|Kanten (Importdaten)|6|EdgeProfile|ProductionStep=null|false|true|EdgeProfilesImport|Code||
### 17.8.1 ProductionStepNull : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#CCD0D8|1|#000000|ProductionStepNull|ProductionStep=null|
## 17.9 EdgeGrooves : DetailListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DisplayName|DisplayOrder|Filter|IsSecondary|IsSortable|Name|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|Nuten|7||false|true|EdgeGrooves||
### 17.9.1 DisposeStateAlternative : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#E7CEEC|1|#000000|DisposeStateAlternative|ProductionStep != null  &&  ProductionStep.DisposeState="Alternative"|
### 17.9.2 ProductionStepNull : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#CCD0D8|2|#000000|ProductionStepNull|ProductionStep=null|
## 17.10 EdgeGroovesImport : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|ProductionOrderCode|Nuten (Importdaten)|8|EdgeGroove|ProductionStep=null|false|true|EdgeGroovesImport|Code||
### 17.10.1 ProductionStepNull : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#CCD0D8|1|#000000|ProductionStepNull|ProductionStep=null|
## 17.11 Beschläge : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|ProductionOrderCode|Beschläge|9|ViewFitting||false|true|Beschläge|Code||
## 17.12 ProcessingData : DetailListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DisplayName|DisplayOrder|Filter|IsSecondary|IsSortable|Name|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|Bearbeitungsdaten|10||false|true|ProcessingData||
### 17.12.1 DisposeStateAlternative : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#E7CEEC|1|#000000|DisposeStateAlternative|ProductionStep != null  &&  ProductionStep.DisposeState="Alternative"|
### 17.12.2 ProductionStepNull : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#CCD0D8|2|#000000|ProductionStepNull|ProductionStep=null|
## 17.13 Strukturanzeige : DetailTreeView
|DisplayName|DisplayOrder|Field|IsSecondary|Name|
|--|--|--|--|--|
|Baumansicht|11||false|Strukturanzeige|
## 17.14 Binaries : DetailImageView
|DisplayName|DisplayOrder|Field|IsSecondary|Name|
|--|--|--|--|--|
|Zeichnungen|12|Binaries|false|Binaries|
## 17.15 PossibleRoutes : DetailListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DisplayName|DisplayOrder|Filter|IsSecondary|IsSortable|Name|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|Alternative Fertigungswege|13||false|true|PossibleRoutes||
### 17.15.1 DisposeStateAlternative : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#E7CEEC|1|#000000|DisposeStateAlternative|DisposeState="Alternative"|
### 17.15.2 ChangeScheduledRouteCommand : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|None|EmptyTile.png|Fertigungsweg auf planmäßig ändern|Fertigungsweg auf planmäßig ändern|ChangeScheduledRouteCommand|
#### 17.15.2.1 ChangeScheduledRouteCommand : CommandUserExit
|Name|Order|
|--|--|
|ChangeScheduledRouteCommand|1|
## 17.16 EdgePasses : DetailListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DisplayName|DisplayOrder|Filter|IsSecondary|IsSortable|Name|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|Kantendurchläufe|14||false|true|EdgePasses||
### 17.16.1 DisposeStateAlternative : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#E7CEEC|1|#000000|DisposeStateAlternative|ProductionStep != null  &&  ProductionStep.DisposeState="Alternative"|
## 17.17 ViewExtensionData : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|ProductionOrderCode|Ergänzende Daten (Intern)|15|ViewExtensionData||false|false|ViewExtensionData|Code||
### 17.17.1  : DefaultSorting
|Field|SortDirection|
|--|--|
|InternSortOrder|Ascending|
## 17.18 ProductionOrderGeometryDetailView : GenericDetailView
|DisplayName|DisplayOrder|IsSecondary|Name|Parameter|View|ViewModel|
|--|--|--|--|--|--|--|
|Grafische Anzeige|16|false|ProductionOrderGeometryDetailView|MinimumAspectRatioForLeftInfoText=1.5;DisabledEdgeTexts=98\|99|ProductionOrderGeometryDetailView|ProductionOrderGeometryDetailViewModel|
## 17.19 FehlerBeimAuftrag : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CD5C5C|CustomErrorState|1|#FFFFFF|FehlerBeimAuftrag|CustomErrorState !="ValidData"|
## 17.20 AuftragGesperrt : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CD5C5C|ReleaseState|2|#FFFFFF|AuftragGesperrt|ReleaseState="NotReleased"|
## 17.21 StatusBeendet : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#A8FAC5|1|#199619|StatusBeendet|ProductionState="Finished"|
## 17.22 StatusInBearbeitung : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#EEE8AA|2|#199619|StatusInBearbeitung|ProductionState="Processing"|
# 18 EdgePass : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|Production|10205|EdgePass|ProductionStep.DisposeState="Scheduled"|Action|false|false|false||true|EdgePass||||EmptyTile.png|Medium|EmptyTileView|EmptyTileViewMode|0|
## 18.1 NavigateFromEdgePassesToProductionOrder : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|F6||Zum FA navigieren|Öffne Maske ProductionOrder mit Filter ProductionOrderCode.|NavigateFromEdgePassesToProductionOrder|
### 18.1.1 NavigateFromEdgePassesToProductionOrder : CommandUserExit
|Name|Order|
|--|--|
|NavigateFromEdgePassesToProductionOrder|10|
## 18.2 EdgePassesFilterByGroup : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|F3||Gruppe filtern (Arbeitsgang)|Filtert nach Gruppe/Arbeitsgang des selektierten Datensatzes|EdgePassesFilterByGroup|
### 18.2.1 EdgePassesFilterByGroup : CommandUserExit
|Name|Order|
|--|--|
|EdgePassesFilterByGroup|1|
#### 18.2.1.1 ShowAll : UserExitParameter
|Format|Name|PassNullValue|Value|
|--|--|--|--|
||ShowAll|false|False|
## 18.3 EdgePassFilterAll : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|F4||Gruppenfilter aufheben|Zeigt wieder alle Datensätze|EdgePassFilterAll|
### 18.3.1 EdgePassesFilterByGroup : CommandUserExit
|Name|Order|
|--|--|
|EdgePassesFilterByGroup|1|
#### 18.3.1.1 ShowAll : UserExitParameter
|Format|Name|PassNullValue|Value|
|--|--|--|--|
||ShowAll|false|True|
## 18.4 EdgePassesFilterNextPass : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|F7||Nächsten Durchlauf filtern|Filtert den nächsten Durchlauf der Gruppe/Arbeitsgang des selektierten Datensatzes|EdgePassesFilterNextPass|
### 18.4.1 EdgePassesFilterNextOrPreviousPass : CommandUserExit
|Name|Order|
|--|--|
|EdgePassesFilterNextOrPreviousPass|1|
#### 18.4.1.1 PreviousPass : UserExitParameter
|Format|Name|PassNullValue|Value|
|--|--|--|--|
||PreviousPass|false|False|
## 18.5 EdgePassesFilterPreviousPass : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|F8||Vorherigen Durchlauf filtern|Filtert den vorherigen Durchlauf der Gruppe/Arbeitsgang des selektierten Datensatzes|EdgePassesFilterPreviousPass|
### 18.5.1 EdgePassesFilterNextOrPreviousPass : CommandUserExit
|Name|Order|
|--|--|
|EdgePassesFilterNextOrPreviousPass|1|
#### 18.5.1.1 PreviousPass : UserExitParameter
|Format|Name|PassNullValue|Value|
|--|--|--|--|
||PreviousPass|false|True|
## 18.6 EdgePassGeometryDetailView : GenericDetailView
|DisplayName|DisplayOrder|IsSecondary|Name|Parameter|View|ViewModel|
|--|--|--|--|--|--|--|
|Grafische Anzeige|1|true|EdgePassGeometryDetailView|MinimumAspectRatioForLeftInfoText=5.0; CheckForValidThroughFeedSide=1;DisabledEdgeTexts=98\|99|EdgePassGeometryDetailView|EdgePassGeometryDetailViewModel|
# 19 WorkCenter : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|true|true|true|Production|10009|WorkCenter||Management|false|false|false||true|WorkCenter|||||Medium|||0|
# 20 WccStagingRecords : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|Production|10150|WccStagingRecord||Management|false|false|false||true|WccStagingRecords||||EmptyTile.png|Medium|EmptyTileView|EmptyTileViewMode|0|
## 20.1 NavigateToImportMessage : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|None|EmptyTile.png|Importmeldungen anzeigen|Importmeldungen anzeigen|NavigateToImportMessage|
### 20.1.1 NavigateToImportMessage : CommandUserExit
|Name|Order|
|--|--|
|NavigateToImportMessage|1|
## 20.2 Images : DetailImageView
|DisplayName|DisplayOrder|Field|IsSecondary|Name|
|--|--|--|--|--|
|Bilder|1|Binaries|false|Images|
## 20.3 WccStagingRecordsResources : DetailListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DisplayName|DisplayOrder|Filter|IsSecondary|IsSortable|Name|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|Bearbeitungs-Importdaten|2||false|true|WccStagingRecordsResources||
# 21 CtDStagingBomLineRecords : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|Production|10151|CtDStagingBomLineRecord||Management|false|false|false||true|CtDStagingBomLineRecords||||EmptyTile.png|Small|EmptyTileView|EmptyTileViewMode|0|
## 21.1 NavigateToImportMessage : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|None|EmptyTile.png|Importmeldungen anzeigen|Importmeldungen anzeigen|NavigateToImportMessage|
### 21.1.1 NavigateToImportMessage : CommandUserExit
|Name|Order|
|--|--|
|NavigateToImportMessage|1|
## 21.2 CtDStagingOrderRecords : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|OrderNo|Kundenauftrags-Importdaten|1|CtDStagingOrderRecord||false|true|CtDStagingOrderRecords|CustomerOrderCode||
## 21.3 CtDStagingEdgeRecords : DetailListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DisplayName|DisplayOrder|Filter|IsSecondary|IsSortable|Name|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|Kanten-Importdaten|2||false|true|CtDStagingEdgeRecords||
## 21.4 CtDStagingMachiningRecords : DetailListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DisplayName|DisplayOrder|Filter|IsSecondary|IsSortable|Name|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|Bearbeitungs-Importdaten|3||false|true|CtDStagingMachiningRecords||
## 21.5 ErrorImportErrorStateC3D : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CD5C5C|ImportErrorState|1|#FFFFFF|ErrorImportErrorStateC3D|ImportErrorState !="OK"|
## 21.6 ErrorTransferStateC3D : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CD5C5C|TransferState|2|#FFFFFF|ErrorTransferStateC3D|TransferState == "Error"|
# 22 CsvStagingRecords : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|Production|10162|CsvStagingRecord||Management|false|false|false||true|CsvStagingRecords||||EmptyTile.png|Medium|EmptyTileView|EmptyTileViewMode|0|
## 22.1 NavigateToImportMessage : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|None|EmptyTile.png|Importmeldungen anzeigen|Importmeldungen anzeigen|NavigateToImportMessage|
### 22.1.1 NavigateToImportMessage : CommandUserExit
|Name|Order|
|--|--|
|NavigateToImportMessage|1|
## 22.2 CsvStagingRecordsResources : DetailListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DisplayName|DisplayOrder|Filter|IsSecondary|IsSortable|Name|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|Bearbeitungs-Importdaten|1||false|true|CsvStagingRecordsResources||
## 22.3 ImportErrorState : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#FFCD5C5E|1|#000000|ImportErrorState|ImportErrorState!="OK"|
# 23 SapStagingRecords : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|Production|10167|SapStagingOrder||Management|false|false|false||true|SapStagingRecords||||EmptyTile.png|Small|EmptyTileView|EmptyTileViewMode|0|
## 23.1 NavigateToImportMessage : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|None|EmptyTile.png|Importmeldungen anzeigen|Importmeldungen anzeigen|NavigateToImportMessage|
### 23.1.1 NavigateToImportMessage : CommandUserExit
|Name|Order|
|--|--|
|NavigateToImportMessage|1|
## 23.2 SapStagingResources : DetailListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DisplayName|DisplayOrder|Filter|IsSecondary|IsSortable|Name|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|Bearbeitungs-Importdaten|1||false|true|SapStagingResources||
## 23.3 ImportErrorState : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#FFCD5C5E|1|#000000|ImportErrorState|ImportErrorState!="OK"|
# 24 McsStagingRecords : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|Production|10186|McsStagingProductionOrder||Management|false|false|false||true|McsStagingRecords||||EmptyTile.png|Small|EmptyTileView|EmptyTileViewMode|0|
## 24.1 NavigateToImportMessage : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|None|EmptyTile.png|Importmeldungen anzeigen|Importmeldungen anzeigen|NavigateToImportMessage|
### 24.1.1 NavigateToImportMessage : CommandUserExit
|Name|Order|
|--|--|
|NavigateToImportMessage|1|
## 24.2 McsStagingCustomerOrders : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|Code|Rohdaten Import Kundenauftrag|1|McsStagingCustomerOrder||false|true|McsStagingCustomerOrders|McsStagingCustomerOrderCode||
## 24.3 McsStagingProductionOrdersResources : DetailListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DisplayName|DisplayOrder|Filter|IsSecondary|IsSortable|Name|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|Rohdaten Import Ressourcen|2||false|true|McsStagingProductionOrdersResources||
## 24.4 McsStagingProductionSteps : DetailListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DisplayName|DisplayOrder|Filter|IsSecondary|IsSortable|Name|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|Rohdaten Import Arbeitsgänge|3||false|true|McsStagingProductionSteps||
## 24.5 McsStagingProductionItems : DetailListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DisplayName|DisplayOrder|Filter|IsSecondary|IsSortable|Name|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|Rohdaten Import Teile|4||false|true|McsStagingProductionItems||
## 24.6 McsStagingEdges : DetailListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DisplayName|DisplayOrder|Filter|IsSecondary|IsSortable|Name|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|Rohdaten Import Kanten|5||false|true|McsStagingEdges||
## 24.7 ImportErrorState : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#FFCD5C5E|1|#000000|ImportErrorState|ImportErrorState!="OK"|
# 25 Bulks : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|Production|10142|ManualBulk||Management|false|false|false||true|Bulks|||||Large|||0|
## 25.1 CancelManualBulk : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|None|CancelImage|Pulk auflösen|Pulk auflösen|CancelManualBulk|
### 25.1.1 CancelManualBulk : CommandUserExit
|Name|Order|
|--|--|
|CancelManualBulk|1|
## 25.2 ProductionOrders : DetailListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DisplayName|DisplayOrder|Filter|IsSecondary|IsSortable|Name|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false||1||false|true|ProductionOrders||
# 26 Routes : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|true|true|true|Production|10010|Route||Management|false|false|false||true|Routes|||||Medium|||0|
## 26.1 ProductionRouteSteps : DetailListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DisplayName|DisplayOrder|Filter|IsSecondary|IsSortable|Name|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|true|true|true|Arbeitsgänge|1||false|true|ProductionRouteSteps||
# 27 TemplateRouteSteps : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|true|true|true|Production|10026|ProductionRouteStep||Management|false|false|false||true|TemplateRouteSteps|||||Medium|||0|
# 28 ProductionStage : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|true|true|true|Production|10183|ProductionStage||Management|false|false|false||true|ProductionStage|||||Medium|||0|
# 29 WccFindRoute : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|true|true|true|Production|10148|WccFindRoute||DC|false|false|false||true|WccFindRoute|||||Medium|||0|
## 29.1 ErpWorkflow : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|ErpWorkflow|1|#000000|ErpWorkflow|true|
## 29.2 OrderType : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|OrderType|2|#000000|OrderType|true|
## 29.3 PartType : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|PartType|3|#000000|PartType|true|
## 29.4 CutFlag : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|CutFlag|4|#000000|CutFlag|true|
## 29.5 SurfaceFlag : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|SurfaceFlag|5|#000000|SurfaceFlag|true|
## 29.6 EdgeFlag : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|EdgeFlag|6|#000000|EdgeFlag|true|
## 29.7 CncFlag : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|CncFlag|7|#000000|CncFlag|true|
## 29.8 PartGeometry : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|PartGeometry|8|#000000|PartGeometry|true|
## 29.9 NarrowPartType : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|NarrowPartType|9|#000000|NarrowPartType|true|
## 29.10 ProductionRoute : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|ProductionRoute|10|#000000|ProductionRoute|true|
# 30 CtDFindRoute : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|true|true|true|Production|10149|CtDFindRoute||DC|false|false|false||true|CtDFindRoute|||||Small|||0|
## 30.1 ErpWorkflow : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|ErpWorkflow|1|#000000|ErpWorkflow|true|
## 30.2 OrderType : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|OrderType|2|#000000|OrderType|true|
## 30.3 CutFlag : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|CutFlag|3|#000000|CutFlag|true|
## 30.4 EdgeFlag : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|EdgeFlag|4|#000000|EdgeFlag|true|
## 30.5 CncFlag : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|CncFlag|5|#000000|CncFlag|true|
## 30.6 NarrowPartType : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|NarrowPartType|6|#000000|NarrowPartType|true|
## 30.7 CncWorkCenters : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|CncWorkCenters|7|#000000|CncWorkCenters|true|
## 30.8 ComponentType : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|ComponentType|8|#000000|ComponentType|true|
# 31 CtDHardwareToProductionOrders : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|true|true|true|Production|10160|CtDHardwareToProductionOrder||DC|false|false|false||true|CtDHardwareToProductionOrders|||||Small|||0|
## 31.1 HardwareComponentType : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|HardwareComponentType|1|#000000|HardwareComponentType|true|
# 32 CsvFindRoute : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|true|true|true|Production|10163|CsvFindRoute||DC|false|false|false||true|CsvFindRoute|||||Medium|||0|
## 32.1 ErpWorkflow : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|ErpWorkflow|1|#000000|ErpWorkflow|true|
## 32.2 OrderType : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|OrderType|2|#000000|OrderType|true|
## 32.3 CutFlag : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|CutFlag|3|#000000|CutFlag|true|
## 32.4 SurfaceFlag : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|SurfaceFlag|4|#000000|SurfaceFlag|true|
## 32.5 EdgeFlag : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|EdgeFlag|5|#000000|EdgeFlag|true|
## 32.6 ContourEdgeFlag : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|ContourEdgeFlag|6|#000000|ContourEdgeFlag|true|
## 32.7 CncFlag : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|CncFlag|7|#000000|CncFlag|true|
## 32.8 NarrowPartType : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|NarrowPartType|8|#000000|NarrowPartType|true|
## 32.9 ProductionRoute : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|ProductionRoute|9|#000000|ProductionRoute|true|
## 32.10 ComponentType : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|ComponentType|10|#000000|ComponentType|true|
# 33 EdgeProfileLibrary : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|true|true|true|Production|10198|EdgeProfileLibrary||DC|false|false|false||true|EdgeProfileLibrary|||||Small|||0|
## 33.1 EdgeId : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|EdgeId|1|#000000|EdgeId|true|
# 34 EdgeGrooveLibrary : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|true|true|true|Production|10196|EdgeGrooveLibrary||DC|false|false|false||true|EdgeGrooveLibrary|||||Medium|||0|
## 34.1 DistanceLeftCovered : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#E0E0E0|DistanceLeft|1|#BEBEBE|DistanceLeftCovered|DistanceTypeLeft == 0|
## 34.2 DistanceLeftThrough : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#E0E0E0|DistanceLeft|2|#BEBEBE|DistanceLeftThrough|DistanceTypeLeft == 1|
## 34.3 DistanceRightCovered : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#E0E0E0|DistanceRight|3|#BEBEBE|DistanceRightCovered|DistanceTypeRight == 0|
## 34.4 DistanceRightThrough : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#E0E0E0|DistanceRight|4|#BEBEBE|DistanceRightThrough|DistanceTypeRight == 1|
## 34.5 SearchValue : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|SearchValue|5|#000000|SearchValue|true|
# 35 EdgeInformationToEntityShapes : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|true|true|true|Production|10218|EdgeInformationToEntityShape||DataCompletion|false|false|false||true|EdgeInformationToEntityShapes|||||Small|||0|
## 35.1 FilterValue : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|FilterValue|1|#000000|FilterValue|true|
## 35.2 EdgeCode : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|EdgeCode|2|#000000|EdgeCode|true|
# 36 WorkerInformationSystem : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|Production|10048|ProductionOrder|OrderType = 1|Action|false|false|false||true|WorkerInformationSystem|FurnitureLabelPO|||Assembly|Medium|EmptyTileView|EdgePreviewTileViewModel|0|
## 36.1 GoFromProductionOrderToReproduction.3 : LinkContextMenu
|GenericName|Header|HelpText|Modul|Name|
|--|--|--|--|--|
|Reproduction|Post-production|Post-production|GenericManagement|GoFromProductionOrderToReproduction.3|
## 36.2 GoFromProductionOrderToReproduction.4 : LinkContextMenu
|GenericName|Header|HelpText|Modul|Name|
|--|--|--|--|--|
|Reproduction|Nachfertigung|Nachfertigung|GenericManagement|GoFromProductionOrderToReproduction.4|
## 36.3 CustomFeedbackSalesItem : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|F10|Assembly|Feedback|100064|CustomFeedbackSalesItem|
### 36.3.1 CustomFeedbackSalesItem : CommandUserExit
|Name|Order|
|--|--|
|CustomFeedbackSalesItem|0|
## 36.4 CustomPrintFurnitureLabel : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|F2|printer|Print Furniture Label|100064|CustomPrintFurnitureLabel|
### 36.4.1 CustomPrintFurnitureLabel : CommandUserExit
|Name|Order|
|--|--|
|CustomPrintFurnitureLabel|0|
## 36.5 CustomDataToPaqteq : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|F5|package|Transfer Data to Paqteq|100064|CustomDataToPaqteq|
### 36.5.1 CustomDataToPaqteq : CommandUserExit
|Name|Order|
|--|--|
|CustomDataToPaqteq|0|
## 36.6 Reproduction : LinkContextMenu
|GenericName|Header|HelpText|Modul|Name|
|--|--|--|--|--|
|Reproduction|Nachfertigung|Nachfertigung|Reproduction|Reproduction|
## 36.7 PartList : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|TopProductionOrderNumber|Teileliste|1|ProductionOrder|OrderType = 3|false|true|PartList|Code||
### 36.7.1 InProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|1|#199619|InProcessing|ProductionState = "Processing" and ReproductionType = "NoReproduction"|
### 36.7.2 Finished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|2|#199619|Finished|ProductionState = "Finished" and ReproductionType = "NoReproduction"|
### 36.7.3 RWNotStarted : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#FAC8C8|3|#961919|RWNotStarted|ProductionState = "New" and ReproductionType = "Standard"|
### 36.7.4 RWInProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|4|#961919|RWInProcessing|ProductionState = "Processing" and ReproductionType = "Standard"|
### 36.7.5 RWFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C8FAC8|5|#961919|RWFinished|ProductionState = "Finished" and ReproductionType = "Standard"|
## 36.8 GroupList : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|TopProductionOrderNumber|Baugruppen|2|ProductionOrder|OrderType = 2|false|true|GroupList|Code||
## 36.9 Binaries : DetailImageView
|DisplayName|DisplayOrder|Field|IsSecondary|Name|
|--|--|--|--|--|
|Zeichnungen|3|Binaries|true|Binaries|
## 36.10 StatusFinished : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#E1FAE1|1|Green|StatusFinished|ProductionState="Finished"|
## 36.11 StatusInProcessing : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|PaleGoldenrod|2|Green|StatusInProcessing|ProductionState="Processing"|
# 37 EdgeProcessings : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|true|true|true|Production|10096|EdgeProcessing||DataCompletion|false|false|false||true|EdgeProcessings|||||Medium|||0|
## 37.1 Binaries : DetailImageView
|DisplayName|DisplayOrder|Field|IsSecondary|Name|
|--|--|--|--|--|
|Zeichnung|1|Picture|false|Binaries|
## 37.2 WorkCenterCode : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|WorkCenterCode|1|#000000|WorkCenterCode|true|
## 37.3 EdgeThickness : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|EdgeThickness|2|#000000|EdgeThickness|true|
## 37.4 EdgeThicknessPartner : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|EdgeThicknessPartner|3|#000000|EdgeThicknessPartner|true|
## 37.5 Profile : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|Profile|4|#000000|Profile|true|
## 37.6 ProfilePartner : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|ProfilePartner|5|#000000|ProfilePartner|true|
## 37.7 CornerShaping : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|CornerShaping|6|#000000|CornerShaping|true|
## 37.8 AdditionalCornerOption : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|AdditionalCornerOption|7|#000000|AdditionalCornerOption|true|
# 38 EdgeTypes : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|true|true|true|Production|10097|EdgeType||DataCompletion|false|false|false||true|EdgeTypes|||||Medium|||0|
## 38.1 Binaries : DetailImageView
|DisplayName|DisplayOrder|Field|IsSecondary|Name|
|--|--|--|--|--|
|Zeichnung|1|Picture|false|Binaries|
## 38.2 WorkCenterCode : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|WorkCenterCode|1|#000000|WorkCenterCode|true|
## 38.3 CustomerEdgeType : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|CustomerEdgeType|2|#000000|CustomerEdgeType|true|
# 39 FindEdgeShapes : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|true|true|true|Production|10098|FindEdgeShape||DataCompletion|false|false|false||true|FindEdgeShapes|||||Medium|||0|
## 39.1 Binaries : DetailImageView
|DisplayName|DisplayOrder|Field|IsSecondary|Name|
|--|--|--|--|--|
|Zeichnung|1|Picture|false|Binaries|
## 39.2 EdgeNorth : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|EdgeNorth|1|#000000|EdgeNorth|true|
## 39.3 EdgeWest : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|EdgeWest|2|#000000|EdgeWest|true|
## 39.4 EdgeSouth : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|EdgeSouth|3|#000000|EdgeSouth|true|
## 39.5 EdgeEast : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|EdgeEast|4|#000000|EdgeEast|true|
## 39.6 CornerNorthWest : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|CornerNorthWest|5|#000000|CornerNorthWest|true|
## 39.7 CornerWestSouth : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|CornerWestSouth|6|#000000|CornerWestSouth|true|
## 39.8 CornerSouthEast : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|CornerSouthEast|7|#000000|CornerSouthEast|true|
## 39.9 CornerEastNorth : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|CornerEastNorth|8|#000000|CornerEastNorth|true|
# 40 GlueTypes : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|true|true|true|Production|10099|GlueType||DataCompletion|false|false|false||true|GlueTypes|||||Medium|||0|
## 40.1 Binaries : DetailImageView
|DisplayName|DisplayOrder|Field|IsSecondary|Name|
|--|--|--|--|--|
|Zeichnung|1|Picture|false|Binaries|
## 40.2 WorkCenterCode : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|WorkCenterCode|1|#000000|WorkCenterCode|true|
## 40.3 CustomerGlueType : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|CustomerGlueType|2|#000000|CustomerGlueType|true|
# 41 LabelInfoCncZeroLines : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|true|true|true|Production|10100|LabelInfoCncZeroLine||DataCompletion|false|false|false||true|LabelInfoCncZeroLines|||||Medium|||0|
## 41.1 Binaries : DetailImageView
|DisplayName|DisplayOrder|Field|IsSecondary|Name|
|--|--|--|--|--|
|Zeichnung|1|Picture|false|Binaries|
## 41.2 WorkCenterCode : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|WorkCenterCode|1|#000000|WorkCenterCode|true|
## 41.3 OrientationY : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|OrientationY|2|#000000|OrientationY|true|
## 41.4 OrientationZ : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|OrientationZ|3|#000000|OrientationZ|true|
# 42 PatternEvaluation : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|Production|10158|PatternEvaluationItem||Management|false|false|false||true|PatternEvaluation||||LotEvaluationImage|Medium|PatternEvaluationTileView|PatternEvaluationTileViewModel|0|
## 42.1 PatternView : GenericDetailView
|DisplayName|DisplayOrder|IsSecondary|Name|Parameter|View|ViewModel|
|--|--|--|--|--|--|--|
||1|false|PatternView||PatternDetailView|PatternDetailViewModel|
# 43 MakroGrooves : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|true|true|true|Production|10101|MakroGroove||DataCompletion|false|false|false||true|MakroGrooves|||||Medium|||0|
## 43.1 Binaries : DetailImageView
|DisplayName|DisplayOrder|Field|IsSecondary|Name|
|--|--|--|--|--|
|Zeichnung|1|Picture|false|Binaries|
## 43.2 WorkCenterCode : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|WorkCenterCode|1|#000000|WorkCenterCode|true|
## 43.3 GrooveType : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|GrooveType|2|#000000|GrooveType|true|
## 43.4 GrooveLayer : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|GrooveLayer|3|#000000|GrooveLayer|true|
## 43.5 GrooveDepth : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|GrooveDepth|4|#000000|GrooveDepth|true|
## 43.6 GrooveWidth : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|GrooveWidth|5|#000000|GrooveWidth|true|
## 43.7 GrooveDistance : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|GrooveDistance|6|#000000|GrooveDistance|true|
## 43.8 GrooveShape : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|GrooveShape|7|#000000|GrooveShape|true|
# 44 MakroSurfaces : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|true|true|true|Production|10107|MakroSurface||DataCompletion|false|false|false||true|MakroSurfaces|||||Medium|||0|
## 44.1 Binaries : DetailImageView
|DisplayName|DisplayOrder|Field|IsSecondary|Name|
|--|--|--|--|--|
|Zeichnung|1|Picture|false|Binaries|
## 44.2 WorkCenterCode : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|WorkCenterCode|1|#000000|WorkCenterCode|true|
## 44.3 Material : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|Material|2|#000000|Material|true|
## 44.4 MaterialGroupTop : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|MaterialGroupTop|3|#000000|MaterialGroupTop|true|
## 44.5 MaterialVarianteTop : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|MaterialVarianteTop|4|#000000|MaterialVarianteTop|true|
## 44.6 MaterialGroupBottom : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|MaterialGroupBottom|5|#000000|MaterialGroupBottom|true|
## 44.7 MaterialVarianteBottom : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|MaterialVarianteBottom|6|#000000|MaterialVarianteBottom|true|
# 45 OrientationConversions : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|true|true|true|Production|10159|OrientationConversion||DataCompletion|false|false|false||true|OrientationConversions|||||Medium|||0|
## 45.1 ConversionType : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|ConversionType|1|#000000|ConversionType|true|
## 45.2 WorkCenterCode : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|WorkCenterCode|2|#000000|WorkCenterCode|true|
## 45.3 OrientationInX : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|OrientationInX|3|#000000|OrientationInX|true|
## 45.4 OrientationInY : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|OrientationInY|4|#000000|OrientationInY|true|
## 45.5 OrientationInZ : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|OrientationInZ|5|#000000|OrientationInZ|true|
# 46 ProcessingDataOrientations : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|true|true|true|Production|10168|ProcessingDataOrientation||DataCompletion|false|false|false||true|ProcessingDataOrientations|||||Medium|||0|
## 46.1 CurrentWorkCenterCode : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|CurrentWorkCenterCode|1|#000000|CurrentWorkCenterCode|true|
## 46.2 MachiningSidesAtItem : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|MachiningSidesAtItem|2|#000000|MachiningSidesAtItem|true|
## 46.3 NextWorkCenterCode : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|NextWorkCenterCode|3|#000000|NextWorkCenterCode|true|
## 46.4 NextWorkCenterOrientationY : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|NextWorkCenterOrientationY|4|#000000|NextWorkCenterOrientationY|true|
# 47 ShapePasses : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|true|true|true|Production|10102|ShapePass||DataCompletion|false|false|false||true|ShapePasses|||||Medium|||0|
## 47.1 ShapePassFilterByGroup : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|F3||Gruppe filtern|Filtert nach Gruppe des selektierten Datensatzes|ShapePassFilterByGroup|
### 47.1.1 ShapePassFilterByGroup : CommandUserExit
|Name|Order|
|--|--|
|ShapePassFilterByGroup|1|
#### 47.1.1.1 ShowAll : UserExitParameter
|Format|Name|PassNullValue|Value|
|--|--|--|--|
||ShowAll|false|False|
## 47.2 ShapePassFilterAll : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|F4||Gruppenfilter aufheben|Zeigt wieder alle Datensätze|ShapePassFilterAll|
### 47.2.1 ShapePassFilterByGroup : CommandUserExit
|Name|Order|
|--|--|
|ShapePassFilterByGroup|1|
#### 47.2.1.1 ShowAll : UserExitParameter
|Format|Name|PassNullValue|Value|
|--|--|--|--|
||ShowAll|false|True|
## 47.3 ShapePassGeometryDetailView : GenericDetailView
|DisplayName|DisplayOrder|IsSecondary|Name|Parameter|View|ViewModel|
|--|--|--|--|--|--|--|
|Grafische Anzeige|1|true|ShapePassGeometryDetailView||ShapePassGeometryDetailView|ShapePassGeometryDetailViewModel|
## 47.4 GrooveInProcessWrong : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CD5C5C|GrooveInProcess|1|#FFFFFF|GrooveInProcessWrong|GrooveInProcess != "-" && GrooveShapeNorth == "0" && GrooveShapeWest == "0" && GrooveShapeSouth == "0" && GrooveShapeEast == "0"|
## 47.5 GrooveInProcess2Wrong : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CD5C5C|GrooveInProcess2|2|#FFFFFF|GrooveInProcess2Wrong|GrooveInProcess2 != "-" && GrooveShapeNorth == "0" && GrooveShapeWest == "0" && GrooveShapeSouth == "0" && GrooveShapeEast == "0"|
## 47.6 WorkCenterCode : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|WorkCenterCode|3|#000000|WorkCenterCode|true|
## 47.7 EdgeShape : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|EdgeShape|4|#000000|EdgeShape|true|
## 47.8 EdgeShapeAddOn : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|EdgeShapeAddOn|5|#000000|EdgeShapeAddOn|true|
## 47.9 GrooveShapeNorth : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|GrooveShapeNorth|6|#000000|GrooveShapeNorth|true|
## 47.10 GrooveShapeWest : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|GrooveShapeWest|7|#000000|GrooveShapeWest|true|
## 47.11 GrooveShapeSouth : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|GrooveShapeSouth|8|#000000|GrooveShapeSouth|true|
## 47.12 GrooveShapeEast : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|GrooveShapeEast|9|#000000|GrooveShapeEast|true|
## 47.13 IsLengthGreaterEqualWidth : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|IsLengthGreaterEqualWidth|10|#000000|IsLengthGreaterEqualWidth|true|
## 47.14 IsRatioPart : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|IsRatioPart|11|#000000|IsRatioPart|true|
## 47.15 IsSquarePart : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|IsSquarePart|12|#000000|IsSquarePart|true|
## 47.16 NarrowPartType : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|NarrowPartType|13|#000000|NarrowPartType|true|
## 47.17 Pass : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|Pass|14|#000000|Pass|true|
## 47.18 GrooveInProcessWrong : RowStyleCondition
|Background|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|
|#C0C0C0|1|#CD5C5C|GrooveInProcessWrong|(GrooveInProcess != "-" \|\| GrooveInProcess2 != "-") && GrooveShapeNorth == "0" && GrooveShapeWest == "0" && GrooveShapeSouth == "0" && GrooveShapeEast == "0"|
# 48 EdgeProfileConversions : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|true|true|true|Production|10194|EdgeProfileConversion||DC|false|false|false||true|EdgeProfileConversions|||||Medium|||0|
## 48.1 Profile : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|Profile|1|#000000|Profile|true|
# 49 WccEdgeInformationToEntityShapes : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|true|true|true|Production|10157|WccEdgeInformationToEntityShape||DataCompletion|false|false|false||true|WccEdgeInformationToEntityShapes|||||Medium|||0|
## 49.1 ShowResultEdgeTransition : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|F4||Ergebnis-Kantenbild anzeigen|Ergebnis-Kantenbild anzeigen|ShowResultEdgeTransition|
### 49.1.1 EdgeTransitionFromWccEdgeInformationToEntityShapeGeometryDialogCommand : CommandUserExit
|Name|Order|
|--|--|
|EdgeTransitionFromWccEdgeInformationToEntityShapeGeometryDialogCommand|1|
#### 49.1.1.1 MinimumAspectRatioForLeftInfoText : UserExitParameter
|Format|Name|PassNullValue|Value|
|--|--|--|--|
||MinimumAspectRatioForLeftInfoText|false|0|
#### 49.1.1.2 ShowResultEdgeTransition : UserExitParameter
|Format|Name|PassNullValue|Value|
|--|--|--|--|
||ShowResultEdgeTransition|false|true|
## 49.2 Binaries : DetailImageView
|DisplayName|DisplayOrder|Field|IsSecondary|Name|
|--|--|--|--|--|
|Zeichnung|1|Picture|false|Binaries|
## 49.3 GeoForm : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|GeoForm|1|#000000|GeoForm|true|
## 49.4 EdgeTransition : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|EdgeTransition|2|#000000|EdgeTransition|true|
## 49.5 DataChange : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|DataChange|3|#000000|DataChange|true|
## 49.6 EdgeIdInput : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|EdgeIdInput|4|#000000|EdgeIdInput|true|
# 50 CtDEdgeInformationToEntityShapes : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|true|true|true|Production|10161|CtDEdgeInformationToEntityShape||DataCompletion|false|false|false||true|CtDEdgeInformationToEntityShapes|||||Small|||0|
## 50.1 EndpointSouthLeft : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|EndpointSouthLeft|1|#000000|EndpointSouthLeft|true|
## 50.2 EndpointSouthRight : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|EndpointSouthRight|2|#000000|EndpointSouthRight|true|
## 50.3 EndpointEastLeft : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|EndpointEastLeft|3|#000000|EndpointEastLeft|true|
## 50.4 EndpointEastRight : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|EndpointEastRight|4|#000000|EndpointEastRight|true|
## 50.5 EndpointNorthLeft : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|EndpointNorthLeft|5|#000000|EndpointNorthLeft|true|
## 50.6 EndpointNorthRight : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|EndpointNorthRight|6|#000000|EndpointNorthRight|true|
## 50.7 EndpointWestLeft : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|EndpointWestLeft|7|#000000|EndpointWestLeft|true|
## 50.8 EndpointWestRight : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|EndpointWestRight|8|#000000|EndpointWestRight|true|
# 51 McsEdgeInformationToEntityShapes : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|true|true|true|Production|10197|McsEdgeInformationToEntityShape||DataCompletion|false|false|false||true|McsEdgeInformationToEntityShapes|||||Small|||0|
## 51.1 PartGeometry : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|PartGeometry|1|#000000|PartGeometry|true|
## 51.2 EdgeCode : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|EdgeCode|2|#000000|EdgeCode|true|
# 52 WorkCenterCodeFromCamInformations : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|true|true|true|Production|10106|WorkCenterCodeFromCamInformation||DC|false|false|false||true|WorkCenterCodeFromCamInformations|||||Medium|||0|
## 52.1 CamInformation : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|CamInformation|1|#000000|CamInformation|true|
# 53 WorkCenterCutting : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|true|true|true|Production|10212|WorkCenterCutting||DC|false|false|false||true|WorkCenterCutting|||||Medium|||0|
## 53.1 WorkCenterCode : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|WorkCenterCode|1|#000000|WorkCenterCode|true|
## 53.2 ProductionStepCode : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|ProductionStepCode|2|#000000|ProductionStepCode|true|
# 54 WorkCenterEdgePasses : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|true|true|true|Production|10213|WorkCenterEdgePass||DC|false|false|false||true|WorkCenterEdgePasses|||||Medium|||0|
## 54.1 WorkCenterCode : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|WorkCenterCode|1|#000000|WorkCenterCode|true|
## 54.2 Pass : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|Pass|2|#000000|Pass|true|
# 55 WorkCenterEdgeProfiles : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|true|true|true|Production|10185|WorkCenterEdgeProfile||DC|false|false|false||true|WorkCenterEdgeProfiles|||||Medium|||0|
## 55.1 WorkCenterCode : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|WorkCenterCode|1|#000000|WorkCenterCode|true|
## 55.2 ProductionStepCode : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|ProductionStepCode|2|#000000|ProductionStepCode|true|
# 56 WorkCenterGrooves : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|true|true|true|Production|10184|WorkCenterGroove||DC|false|false|false||true|WorkCenterGrooves|||||Medium|||0|
## 56.1 Rebate : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#E0E0E0|DistanceMax|1|#BEBEBE|Rebate|Type == 1|
## 56.2 HorizontalGroove : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#E0E0E0|DistanceMax|2|#BEBEBE|HorizontalGroove|Layer == 2|
## 56.3 WorkCenterCode : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|WorkCenterCode|3|#000000|WorkCenterCode|true|
## 56.4 ProductionStepCode : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|ProductionStepCode|4|#000000|ProductionStepCode|true|
## 56.5 Type : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|Type|5|#000000|Type|true|
## 56.6 Layer : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|Layer|6|#000000|Layer|true|
## 56.7 WidthMin : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|WidthMin|7|#000000|WidthMin|true|
## 56.8 WidthMax : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|WidthMax|8|#000000|WidthMax|true|
## 56.9 DistanceMin : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|DistanceMin|9|#000000|DistanceMin|true|
## 56.10 DistanceMax : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|DistanceMax|10|#000000|DistanceMax|true|
# 57 WorkCenterOversizeValues : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|true|true|true|Production|10214|WorkCenterOversizeValue||DC|false|false|false||true|WorkCenterOversizeValues|||||Medium|||0|
## 57.1 WorkCenterCode : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|WorkCenterCode|1|#000000|WorkCenterCode|true|
## 57.2 ProductionStepCode : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|ProductionStepCode|2|#000000|ProductionStepCode|true|
## 57.3 Material : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#CCD0D8|Material|3|#000000|Material|true|
# 58 ReorganizationDBTables : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|true|true|true|Production|10111|ReorganizationDBTable||Management|false|false|false||true|ReorganizationDBTables|||||Medium|||0|
# 59 ReorganizationFileSystems : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|true|true|true|Production|10112|ReorganizationFileSystem||Management|false|false|false||true|ReorganizationFileSystems|||||Medium|||0|
# 60 ProductionItemsValidationData : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|Production|10136|ProductionItemsValidation|ReproductionMode == 40|Action|false|false|false||true|ProductionItemsValidationData||||EmptyTile.png|Medium|EmptyTileView|EmptyTileViewMode|0|
## 60.1 ReleaseNewDataProductionItemsValidation : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|F7||Freigabe Datenfehler-Nachfertigungsaufträge|Nachfertigungsaufträge mit Datenfehlern freigeben.|ReleaseNewDataProductionItemsValidation|
### 60.1.1 ReleaseNewDataProductionItemsValidation : CommandUserExit
|Name|Order|
|--|--|
|ReleaseNewDataProductionItemsValidation|10|
## 60.2 DeleteNewDataProductionItemsValidation : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|None||Lösche Datenfehler-Nachfertigungsaufträge|Nachfertigungsaufträge mit Datenfehlern wieder löschen.|DeleteNewDataProductionItemsValidation|
### 60.2.1 DeleteNewDataProductionItemsValidation : CommandUserExit
|Name|Order|
|--|--|
|DeleteNewDataProductionItemsValidation|10|
## 60.3 ProductionOrder : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|Code|Org. Fertigungsaufträge|1|ProductionOrder||false|true|ProductionOrder|ProductionOrderCode||
## 60.4 ReproductionStateNew : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#FFA8FAC5|ReproductionState|4|#000000|ReproductionStateNew|ReproductionState="New"|
## 60.5 ReproductionStateReadyForValidation : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#FFEEE8AA|ReproductionState|2|#000000|ReproductionStateReadyForValidation|ReproductionState="ReadyForGeneration"|
## 60.6 ReproductionStateInProcessForValidation : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#FFEEE8AA|ReproductionState|3|#000000|ReproductionStateInProcessForValidation|ReproductionState="InProcessForGeneration"|
## 60.7 ReproductionStateNotApplicable : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#FFD2D2D2|ReproductionState|5|#000000|ReproductionStateNotApplicable|ReproductionState="NotApplicable"|
## 60.8 ReproductionStateError : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#FFCD5C5E|ReproductionState|6|#000000|ReproductionStateError|ReproductionState="Error"|
## 60.9 ReproductionModeNewPlanWithERP : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#FFD2D2D2|ReproductionMode|7|#000000|ReproductionModeNewPlanWithERP|ReproductionMode="NewPlanWithERP"|
## 60.10 ReproductionModeUnchangedPlan : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#FFEEE8AA|ReproductionMode|8|#000000|ReproductionModeUnchangedPlan|ReproductionMode="UnchangedPlan"|
## 60.11 ReproductionModeDataModification : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#FFA8FAC5|ReproductionMode|10|#000000|ReproductionModeDataModification|ReproductionMode="DataModification"|
# 61 ProductionItemsValidationAll : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|Production|10137|ProductionItemsValidation||Action|false|false|false||true|ProductionItemsValidationAll||||EmptyTile.png|Medium|EmptyTileView|EmptyTileViewMode|0|
## 61.1 ReleaseNewProductionItemsValidation : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|F7||Freigabe Nachfertigungsaufträge|Nachfertigungsaufträge freigeben.|ReleaseNewProductionItemsValidation|
### 61.1.1 ReleaseNewProductionItemsValidation : CommandUserExit
|Name|Order|
|--|--|
|ReleaseNewProductionItemsValidation|10|
## 61.2 DeleteNewProductionItemsValidation : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|None||Lösche Nachfertigungsaufträge|Nachfertigungsaufträge wieder löschen.|DeleteNewProductionItemsValidation|
### 61.2.1 DeleteNewProductionItemsValidation : CommandUserExit
|Name|Order|
|--|--|
|DeleteNewProductionItemsValidation|10|
## 61.3 ProductionOrder : DetailRelationListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DetailProperties|DisplayName|DisplayOrder|Entity|Filter|IsSecondary|IsSortable|Name|ParentProperties|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|Code|Org. Fertigungsaufträge|1|ProductionOrder||false|true|ProductionOrder|ProductionOrderCode||
## 61.4 ReproductionStateNew : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#FFA8FAC5|ReproductionState|4|#000000|ReproductionStateNew|ReproductionState="New"|
## 61.5 ReproductionStateReadyForValidation : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#FFEEE8AA|ReproductionState|2|#000000|ReproductionStateReadyForValidation|ReproductionState="ReadyForGeneration"|
## 61.6 ReproductionStateInProcessForValidation : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#FFEEE8AA|ReproductionState|3|#000000|ReproductionStateInProcessForValidation|ReproductionState="InProcessForGeneration"|
## 61.7 ReproductionStateNotApplicable : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#FFD2D2D2|ReproductionState|5|#000000|ReproductionStateNotApplicable|ReproductionState="NotApplicable"|
## 61.8 ReproductionStateError : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#FFCD5C5E|ReproductionState|6|#000000|ReproductionStateError|ReproductionState="Error"|
## 61.9 ReproductionModeNewPlanWithERP : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#FFD2D2D2|ReproductionMode|7|#000000|ReproductionModeNewPlanWithERP|ReproductionMode="NewPlanWithERP"|
## 61.10 ReproductionModeUnchangedPlan : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#FFEEE8AA|ReproductionMode|8|#000000|ReproductionModeUnchangedPlan|ReproductionMode="UnchangedPlan"|
## 61.11 ReproductionModeDataModification : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#FFA8FAC5|ReproductionMode|10|#000000|ReproductionModeDataModification|ReproductionMode="DataModification"|
# 62 ViewItemsProcessingData : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|Production|10169|ViewProcessingDataItem||Action|false|false|false||true|ViewItemsProcessingData|||||Medium|||0|
# 63 PartCarrierGroups : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|true|true|true|Production|10199|PartCarrierGroup||Management|false|false|false||true|PartCarrierGroups|||||Medium|||0|
## 63.1 Edit : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|None||Bearbeiten...|Editieren|Edit|
### 63.1.1 PartCarrierEditCommand : CommandUserExit
|Name|Order|
|--|--|
|PartCarrierEditCommand|1|
## 63.2 Copy : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|None||Kopieren|Kopieren|Copy|
### 63.2.1 PartCarrierCopyCommand : CommandUserExit
|Name|Order|
|--|--|
|PartCarrierCopyCommand|1|
## 63.3 Paste : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|None||Einfügen|Einfügen|Paste|
### 63.3.1 PartCarrierPasteCommand : CommandUserExit
|Name|Order|
|--|--|
|PartCarrierPasteCommand|1|
## 63.4 Delete : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|None||Löschen|Löschen|Delete|
### 63.4.1 PartCarrierDeleteCommand : CommandUserExit
|Name|Order|
|--|--|
|PartCarrierDeleteCommand|1|
## 63.5 Compartments : DetailListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DisplayName|DisplayOrder|Filter|IsSecondary|IsSortable|Name|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false||2||false|true|Compartments||
## 63.6 Vorschau : GenericDetailView
|DisplayName|DisplayOrder|IsSecondary|Name|Parameter|View|ViewModel|
|--|--|--|--|--|--|--|
|Vorschau|1|false|Vorschau||PartCarrierPreviewView|PartCarrierPreviewViewModel|
# 64 SortSteps : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|true|true|true|Production|10200|SortStep||Management|false|false|false||true|SortSteps|||||Small|||0|
## 64.1 Edit : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|None||Bearbeiten...|Editieren|Edit|
### 64.1.1 PartCarrierEditCommand : CommandUserExit
|Name|Order|
|--|--|
|PartCarrierEditCommand|1|
## 64.2 Copy : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|None||Kopieren|Kopieren|Copy|
### 64.2.1 PartCarrierCopyCommand : CommandUserExit
|Name|Order|
|--|--|
|PartCarrierCopyCommand|1|
## 64.3 Paste : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|None||Einfügen|Einfügen|Paste|
### 64.3.1 PartCarrierPasteCommand : CommandUserExit
|Name|Order|
|--|--|
|PartCarrierPasteCommand|1|
## 64.4 Delete : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|None||Löschen|Löschen|Delete|
### 64.4.1 PartCarrierDeleteCommand : CommandUserExit
|Name|Order|
|--|--|
|PartCarrierDeleteCommand|1|
## 64.5 Compartments : DetailListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DisplayName|DisplayOrder|Filter|IsSecondary|IsSortable|Name|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false||2||false|true|Compartments||
## 64.6 Vorschau : GenericDetailView
|DisplayName|DisplayOrder|IsSecondary|Name|Parameter|View|ViewModel|
|--|--|--|--|--|--|--|
|Vorschau|1|false|Vorschau||PartCarrierPreviewView|PartCarrierPreviewViewModel|
# 65 OptimizationRemovement : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|Production|10219|Optimization|OptimizationCuttingPlans.Any(CuttingPlanState != 5)|Action|false|false|false||true|OptimizationRemovement|||||Medium|||0|
## 65.1 RemoveOptimization : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|None||Optimierung löschen|Löscht die Optimierung mit Schnittplänen und zugehörigen Teilen|RemoveOptimization|
### 65.1.1 SetOptimizationStateForDelete : CommandUserExit
|Name|Order|
|--|--|
|SetOptimizationStateForDelete|1|
## 65.2 DeleteInvalidOptimizations : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|None|EmptyTile.png|Bereinigung Optimierungsdaten|Bereinigung Optimierungsdaten|DeleteInvalidOptimizations|
### 65.2.1 DeleteInvalidOptimizations : CommandUserExit
|Name|Order|
|--|--|
|DeleteInvalidOptimizations|1|
## 65.3 OptimizationCuttingPlans : DetailListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DisplayName|DisplayOrder|Filter|IsSecondary|IsSortable|Name|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|
|true|false|false|false|false|Optimierte Schnittpläne|1||false|true|OptimizationCuttingPlans||
### 65.3.1 RemoveOptimizationCuttingPlan : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|None||Schnittplan löschen|Löscht den Schnittplan mit zugehörigen Teilen|RemoveOptimizationCuttingPlan|
#### 65.3.1.1 SetCuttingPlanStateForDelete : CommandUserExit
|Name|Order|
|--|--|
|SetCuttingPlanStateForDelete|1|
## 65.4 OptimizationParts : DetailListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DisplayName|DisplayOrder|Filter|IsSecondary|IsSortable|Name|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false|Optimierte Teile|2||true|true|OptimizationParts||
# 66 Settings : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|true|true|true|Production|10093|Setting||Management|false|false|false||true|Settings|||||Small|||10000|
## 66.1 DeleteGrouping : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|None||Delete Grouping|Delete Grouping in View|DeleteGrouping|
### 66.1.1 DeleteGroupingCommand : CommandUserExit
|Name|Order|
|--|--|
|DeleteGroupingCommand|1|
## 66.2 DeleteAggregationCommand : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|None||Delete Aggregation|Delete Aggregation in View|DeleteAggregationCommand|
### 66.2.1 DeleteAggregationCommand : CommandUserExit
|Name|Order|
|--|--|
|DeleteAggregationCommand|1|
## 66.3 LogonId : Column
|Name|Style|
|--|--|
|LogonId||
## 66.4 Module : Column
|Name|Style|
|--|--|
|Module||
## 66.5 Component : Column
|Name|Style|
|--|--|
|Component||
## 66.6 Identifier : Column
|Name|Style|
|--|--|
|Identifier||
# 67 PrintOuts : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|Production|6|PrintOut||Management|false|false|false||true|PrintOuts|||||Medium|||10000|
## 67.1 Print : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|None||Druckdaten|Drucken|Print|
### 67.1.1 ShowWoodPrintPdf : CommandUserExit
|Name|Order|
|--|--|
|ShowWoodPrintPdf|1|
# 68 ScannerCommunication : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|true|false|false|Office|13|DeviceFunction||Management|true|false|false||true|ScannerCommunication||||ScannerImageBrush|Medium|||10000|
## 68.1 Scannerfunktion : GenericDetailView
|DisplayName|DisplayOrder|IsSecondary|Name|Parameter|View|ViewModel|
|--|--|--|--|--|--|--|
|Scannerfunktion|1|false|Scannerfunktion||ScannerFunctionDetailView|ScannerFunctionDetailViewModel|
## 68.2 Scanneranbindung : GenericDetailView
|DisplayName|DisplayOrder|IsSecondary|Name|Parameter|View|ViewModel|
|--|--|--|--|--|--|--|
|Datalogic Scanneranbindung (PowerScan)|2|false|Scanneranbindung||ScannerDetailView|ScannerDetailViewModel|
## 68.3 ProgloveScanneranbindung : GenericDetailView
|DisplayName|DisplayOrder|IsSecondary|Name|Parameter|View|ViewModel|
|--|--|--|--|--|--|--|
|Proglove Scanneranbindung|3|false|ProgloveScanneranbindung||ProgloveConnectionView|ProgloveConnectionViewModel|
# 69 Reproduction : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|Production|10138|ProductionItem|ProductionOrder.OrderType == 3|Action|false|false|false||true|Reproduction|||||Medium|EmptyTileView|EmptyTileViewMode|10000|
## 69.1 Nachfertigung : GenericDetailView
|DisplayName|DisplayOrder|IsSecondary|Name|Parameter|View|ViewModel|
|--|--|--|--|--|--|--|
||1|false|Nachfertigung||ReproductionDetailView|ReproductionDetailViewModel|
## 69.2 ProductionOrder.Binary : DetailImageView
|DisplayName|DisplayOrder|Field|IsSecondary|Name|
|--|--|--|--|--|
||2||false|ProductionOrder.Binary|
## 69.3 CreateValidationStateDetails : ModulePermission
|Description|Name|
|--|--|
|You are allowed to create ValidationStateDetails and -Sources in GUI|CreateValidationStateDetails|
# 70 PartCarriers : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|Production|15|PartCarrier||Management|false|false|false||true|PartCarriers|||||Medium|||10000|
## 70.1 Edit : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|None||Bearbeiten...|Editieren|Edit|
### 70.1.1 PartCarrierEditCommand : CommandUserExit
|Name|Order|
|--|--|
|PartCarrierEditCommand|1|
## 70.2 Copy : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|None||Kopieren|Kopieren|Copy|
### 70.2.1 PartCarrierCopyCommand : CommandUserExit
|Name|Order|
|--|--|
|PartCarrierCopyCommand|1|
## 70.3 Paste : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|None||Einfügen|Einfügen|Paste|
### 70.3.1 PartCarrierPasteCommand : CommandUserExit
|Name|Order|
|--|--|
|PartCarrierPasteCommand|1|
## 70.4 Delete : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|None||Löschen|Löschen|Delete|
### 70.4.1 PartCarrierDeleteCommand : CommandUserExit
|Name|Order|
|--|--|
|PartCarrierDeleteCommand|1|
## 70.5 Compartments : DetailListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DisplayName|DisplayOrder|Filter|IsSecondary|IsSortable|Name|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|false||2||false|true|Compartments||
## 70.6 Vorschau : GenericDetailView
|DisplayName|DisplayOrder|IsSecondary|Name|Parameter|View|ViewModel|
|--|--|--|--|--|--|--|
|Vorschau|1|false|Vorschau||PartCarrierPreviewView|PartCarrierPreviewViewModel|
# 71 Restrictions : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|true|true|true|Production|10030|Restriction||Management|false|false|false||true|Restrictions|||||Medium|||0|
## 71.1 RolesRestrictions : DetailListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DisplayName|DisplayOrder|Filter|IsSecondary|IsSortable|Name|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|true|false|true|Gesperrte Rollen|1||false|true|RolesRestrictions||
## 71.2 UserRestrictions : DetailListView
|CanMultiselect|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|DisplayName|DisplayOrder|Filter|IsSecondary|IsSortable|Name|RowStyle|
|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|true|false|true|Gesperrte Benutzer|2||true|true|UserRestrictions||
# 72 ConfirmedMessages : GenericView
|CanUserCopy|CanUserDelete|CanUserEdit|CanUserInsert|ClientType|Description|Entity|Filter|GroupName|HideFilterPanel|HideMasterPanel|HideUpdate|InitialUserExit|IsSortable|Name|PrintRowReport|PrintTableReport|RowStyle|TileIcon|TileSize|TileView|TileViewModel|UpdateInterval|
|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|--|
|false|false|false|false|Information|10033|ConfirmedMessage|ConfirmationUser == null && ((ReceiverType == "User" && Receiver == "{User}") \|\| (ReceiverType == "Role" && Receiver == "{Role}") \|\| (ReceiverType == "HostName" && Receiver == "{HostName}"))|Management|true|false|false||false|ConfirmedMessages|||||Small|CountOnlyTileView|CountOnlyTileViewModel|0|
## 72.1 Navigate : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|F5|LinkedImage|Gehe zu...|Verlinkte Daten anzeigen|Navigate|
### 72.1.1 NavigateUserExit : CommandUserExit
|Name|Order|
|--|--|
|NavigateUserExit|1|
## 72.2 Confirm : CommandUserExitContextMenu
|FunctionKey|FunctionKeyIcon|Header|HelpText|Name|
|--|--|--|--|--|
|F6|ConfirmImage|Bestätigen|Meldung bestätigen|Confirm|
### 72.2.1 ConfirmUserExit : CommandUserExit
|Name|Order|
|--|--|
|ConfirmUserExit|1|
## 72.3 Error : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#FF4500|Level|1|White|Error|Level = "Error"|
## 72.4 Warning : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#FFA500|Level|2|White|Warning|Level = "Warning"|
## 72.5 Success : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#32CD32|Level|3|White|Success|Level = "Success"|
## 72.6 Information : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#6495ED|Level|4|White|Information|Level = "Information"|
## 72.7 Notification : CellStyleCondition
|Background|Cell|ExecutionOrder|Foreground|Key|Rule|
|--|--|--|--|--|--|
|#444444|Level|5|White|Notification|Level = "Notification"|
## 72.8  : DefaultSorting
|Field|SortDirection|
|--|--|
|Date|Descending|
