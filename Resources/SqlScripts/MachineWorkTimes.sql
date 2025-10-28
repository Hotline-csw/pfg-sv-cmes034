-- Table für Arbeitszeiten
-- The schema name has to be 'cust'
CREATE TABLE[cust].[MachineWorkTimes]
(
	[Sequence] 			[bigint] IDENTITY(1,1) NOT NULL,
	[Day] 				[datetime2](7) NOT NULL,
	[Start] 			[datetime2](7) NOT NULL,
	[End] 				[datetime2](7) NOT NULL,
	[Shift]             NVARCHAR(32) NULL,
	[WorkCenterCode] 	NVARCHAR(32) NULL,
	[CreationDate] 		[datetime2](7) NOT NULL default getdate(),
	[CreationSource] 	[nvarchar](32) NULL,
	[ModificationDate] 	[datetime2](7) NOT NULL default getdate(),
	[ModificationSource] [nvarchar](32) NULL,
	[Locked] 			[bit]NOT NULL default 0,
	[LockSource] 		[nvarchar](32) NULL

	CONSTRAINT [PK_MachineWorkTimes] PRIMARY KEY CLUSTERED (Sequence)
	CONSTRAINT [FK_MachineWorkTimes_WorkCenter] FOREIGN KEY ( [WorkCenterCode] ) REFERENCES [base].WorkCenters ( [Code] ) ON DELETE SET NULL
)
GO

CREATE INDEX [IX_MachineWorkTimes_ModificationDate] ON [cust].[MachineWorkTimes]
(
	[Day]
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
