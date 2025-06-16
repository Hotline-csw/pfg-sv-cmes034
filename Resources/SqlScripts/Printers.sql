-- Printing Center Confi
-- SF - 2025-06-16

CREATE TABLE[cust].[Printers]
(
    Sequence                        bigint          not null identity(1,1),
    Printer                         nvarchar(64)    not null,                   -- Name des Druckers
    PrinterType                     int             not null default 0,         -- Typisierung des Druckers
                                                                                --   0 = nicht bekannt
                                                                                --   1 = DIN A4
                                                                                --   2 = Etikett
    Description                     nvarchar(256)   null,                       -- Beschreibung
    CreationDate                    datetime2       not null default getdate(),
    CreationSource                  nvarchar(32)    null,
    ModificationDate                datetime2       not null default getdate(),
    ModificationSource              nvarchar(32)    null,
    Locked                          bit             not null default 0,
    LockSource                      nvarchar(32)    null,

    CONSTRAINT [PK_Printers] PRIMARY KEY ( Printer )
)