-- CREATING NEW TABLE HandlingItemsSignals
-- The schema name has to be 'cust'
CREATE TABLE [cust].[HandlingItemsSignals](
    [Sequence] [bigint] IDENTITY(1,1) NOT NULL,
    [ProductionItemCode] [nvarchar](32) NOT NULL,
    [ProductionOrderCode] [nvarchar](32) NOT NULL,
    [HandlingUnitPosition] [nvarchar](32) NOT NULL,
    [HandlingUnitLocation] [int] NOT NULL,
    [Timestamp] [datetime2](7) NOT NULL,
    [Remark] [nvarchar](256) NULL,
    [EBCurrentPass] [int] NULL,
    [EBMaxPass] [int] NULL,
    [EBReporterMachine] [int] NULL,
    [EBCustomerEdge] [nvarchar](64) NULL,
    [EBProductionStep] [int] NULL,
    [EBQualityInfoFred] [int] NULL,
    [EBMonitoringEdgeOverhang] [int] NULL,
    [EBFeedStopGluingUnit] [int] NULL,
    [EBFeedStopSnippingUnit] [int] NULL,
    [EBFeedStopContourTrimmingUnit] [int] NULL,
    [EBFeedStopProfileScrapingUnit] [int] NULL,
    [EBTemperatureGluingUnit] [numeric](18, 2) NULL,
    [CreationDate] [datetime2](7) NOT NULL,
    [CreationSource] [nvarchar](32) NULL,
    [ModificationDate] [datetime2](7) NOT NULL,
    [ModificationSource] [nvarchar](32) NULL,
    [Locked] [bit] NOT NULL,
    [LockSource] [nvarchar](32) NULL,
 CONSTRAINT [PK_HandlingItemsSignals] PRIMARY KEY CLUSTERED
(

       [Sequence] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [cust].[HandlingItemsSignals] ADD DEFAULT (getdate()) FOR [CreationDate]
GO

ALTER TABLE [cust].[HandlingItemsSignals] ADD DEFAULT (getdate()) FOR [ModificationDate]
GO

ALTER TABLE [cust].[HandlingItemsSignals] ADD DEFAULT ((0)) FOR [Locked]
GO

ALTER TABLE [cust].[HandlingItemsSignals] WITH CHECK ADD CONSTRAINT [FK_HandlingItemsSignals_ProductionItems] FOREIGN KEY([ProductionOrderCode], [ProductionItemCode])
REFERENCES [base].[ProductionItems] ([ProductionOrderCode], [Code])
ON DELETE CASCADE
GO

ALTER TABLE [cust].[HandlingItemsSignals] CHECK CONSTRAINT [FK_HandlingItemsSignals_ProductionItems]
GO
