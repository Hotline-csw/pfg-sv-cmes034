CREATE TABLE [cust].[DummyIndexes]
(
    Sequence                        bigint identity(1,1) not null,
    CreationDate                    datetime2       not null default getdate(),
    CreationSource                  nvarchar(32)    null,
    ModificationDate                datetime2       not null default getdate(),
    ModificationSource              nvarchar(32)    null,
    Locked                          bit             not null default 0,
    LockSource                      nvarchar(32)    null,

    CONSTRAINT [PK_DummyIndexes] PRIMARY KEY CLUSTERED ( Sequence )
)
GO


CREATE INDEX [CustomIX_DummyIndexes_CreationSource] ON [cust].[DummyIndexes]
(
    CreationSource
)
GO

