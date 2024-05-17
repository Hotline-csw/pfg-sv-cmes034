-- CREATING NEW TABLE
-- The schema name has to be 'cust'
CREATE TABLE[cust].[FredPictures]
 ([Sequence] [bigint] IDENTITY(1,1) NOT NULL,
  [Picture1]					  INT			  NULL,
   [Picture2]					  INT			  NULL,
[CreationDate] [datetime2](7) NOT NULL default getdate(),
[CreationSource] [nvarchar](32) NULL,
[ModificationDate] [datetime2](7) NOT NULL default getdate(),
[ModificationSource] [nvarchar](32) NULL,
[Locked] [bit]NOT NULL default 0,
[LockSource] [nvarchar](32) NULL)
go
ALTER TABLE [cust].[FredPictures]  WITH CHECK ADD  CONSTRAINT [FK_FredPictures_Binaries] FOREIGN KEY ( [Picture1] ) REFERENCES [base].[Binaries] ( [Sequence] ) ON DELETE CASCADE

 go
ALTER TABLE [cust].[FredPictures]  WITH CHECK ADD  CONSTRAINT [FK1_FredPictures_Binaries] FOREIGN KEY ( [Picture2] ) REFERENCES [base].[Binaries] ( [Sequence] ) 

 go

