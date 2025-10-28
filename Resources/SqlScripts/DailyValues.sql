-- Creating Table for historical data of daily values

/****** Object:  Table [cust].[DailyValues]    Script Date: 24.02.2021 10:01:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [cust].[DailyValues](
    [Sequence] [int] IDENTITY(1,1) NOT NULL,
    [DateOfDay] [datetime2](7) NOT NULL,
    [Position] [nvarchar](max) NOT NULL,
    [Location] [int] NOT NULL,
    [Value01Int] [int] NULL,
    [Value02Int] [int] NULL,
    [Value03Int] [int] NULL,
    [Value04Int] [int] NULL,
    [Value05Int] [int] NULL,
    [Value06Int] [int] NULL,
    [Value07Int] [int] NULL,
    [Value08Int] [int] NULL,
    [Value09Int] [int] NULL,
    [Value01Float] [float] NULL,
    [Value02Float] [float] NULL,
    [Value03Float] [float] NULL,
    [Value04Float] [float] NULL,
    [Value05Float] [float] NULL,
    [Value06Float] [float] NULL,
    [Value07Float] [float] NULL,
    [Value08Float] [float] NULL,
    [Value09Float] [float] NULL,
    [Value01String] [nvarchar](max) NULL,
    [Value02String] [nvarchar](max) NULL,
    [Value03String] [nvarchar](max) NULL,
    [Value04String] [nvarchar](max) NULL,
    [Value05String] [nvarchar](max) NULL,
    [Value06String] [nvarchar](max) NULL,
    [Value07String] [nvarchar](max) NULL,
    [Value08String] [nvarchar](max) NULL,
    [Value09String] [nvarchar](max) NULL,
    [CreationDate] [datetime2](7) NOT NULL,
    [CreationSource] [nvarchar](32) NULL,
    [ModificationDate] [datetime2](7) NOT NULL,
    [ModificationSource] [nvarchar](32) NULL,
    [Locked] [bit] NOT NULL,
    [LockSource] [nvarchar](32) NULL,
 CONSTRAINT [PK_DailyValues] PRIMARY KEY CLUSTERED 
(
    [Sequence] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [cust].[DailyValues] ADD  CONSTRAINT [DF_DailyValues_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO

ALTER TABLE [cust].[DailyValues] ADD  CONSTRAINT [DF_DailyValues_ModificationDate]  DEFAULT (getdate()) FOR [ModificationDate]
GO

ALTER TABLE [cust].[DailyValues] ADD  CONSTRAINT [DF_DailyValues_Locked]  DEFAULT ((0)) FOR [Locked]
GO