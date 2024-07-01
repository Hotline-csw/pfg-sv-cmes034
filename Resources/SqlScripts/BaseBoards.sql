-- intelliStack table for base boards (Schonerplatten)
CREATE TABLE [cust].[BaseBoards]
(
    Sequence                        bigint          not null identity(1,1),
    BaseBoardCode                   nvarchar(32)    not null,                   -- Name der Schonplatte
                                                                                -- z.B. SP1, SP2
    Description                     nvarchar(256)   null,                       -- Beschreibung
    Length                          numeric(18,2)   not null,                   -- Länge der Schonplatte
    Width                           numeric(18,2)   not null,                   -- Breite der Schonplatte
    Thickness                       numeric(18,2)   not null,                   -- Dicke der Schonplatte
    CreationDate                    datetime2       not null default getdate(),
    CreationSource                  nvarchar(32)    null,
    ModificationDate                datetime2       not null default getdate(),
    ModificationSource              nvarchar(32)    null,
    Locked                          bit             not null default 0,
    LockSource                      nvarchar(32)    null,

    CONSTRAINT [PK_BaseBoards] PRIMARY KEY ( BaseBoardCode )
)