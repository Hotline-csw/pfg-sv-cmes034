-- Create Table: Stammdaten Eckprinzip

CREATE TABLE [cust].[DGCornerPrinciples]
(
    Sequence                    bigint IDENTITY(1,1)        not null,                       -- Eindeutige Nummer
    CornerPrinciple             nvarchar(8)                 not null,                       -- Eckprinzip
    TypeNewEdge                 nvarchar(3)                 not null,                       -- Art der neuen Kante
    TypeFinishEdge              nvarchar(3)                 not null,                       -- Art der Fertigkante
    CornerDesign                nvarchar(3)                 not null,                       -- Eckausführung
    Description                 nvarchar(32)				not null,
     Description1                nvarchar(32)				not null,
       Description2               nvarchar(32)				not null,
         Description3               nvarchar(32)				not null,
         TestDsc nvarchar(32)				null,
          TestDscFred nvarchar(32)				null,
          TestDscNeu nvarchar(32)				null,
           TestDscNeuNoch nvarchar(32)				null,
    CreationDate                datetime2(7)                not null default getdate(),
    CreationSource              nvarchar(32)                null,
    ModificationDate            datetime2(7)                not null default getdate(),
    ModificationSource          nvarchar(32)                null,
    Locked                      bit                         not null default 0,
    LockSource                  nvarchar(32)                null,

 CONSTRAINT [PK_DGCornerPrinciples] PRIMARY KEY CLUSTERED (Sequence)
 )