-- CREATING NEW TABLE
-- The schema name has to be 'cust'
CREATE TABLE[cust].[ReportBindings]
 ([Sequence] [bigint] IDENTITY(1,1) NOT NULL,
[CreationDate] [datetime2](7) NOT NULL default getdate(),
[CreationSource] [nvarchar](32) NULL,
[ModificationDate] [datetime2](7) NOT NULL default getdate(),
[ModificationSource] [nvarchar](32) NULL,
[Locked] [bit]NOT NULL default 0,
[LockSource] [nvarchar](32) NULL)
