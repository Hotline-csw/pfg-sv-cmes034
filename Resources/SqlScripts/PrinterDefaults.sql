-- Printing Center Confi
-- SF - 2025-06-16

CREATE TABLE [cust].[PrinterDefaults]
(
    Sequence                        bigint          not null identity(1,1),
    UserName                        nvarchar(32)    not null,                   -- Benutzername
    ReportLayout                    nvarchar(64)    not null,                   -- Name des Report Layout
    Dialogue                        nvarchar(64)    not null,                   -- Name der aufrufenden Maske
    Printer                         nvarchar(64)    not null,                   -- Name des Druckers
    IsDefault                       int             not null default 0,                   -- Defaultkennzeichen
                                                                                --   0 = kein Default
                                                                                --   1 = Printer ist Default für diesen Benutzer,
                                                                                --       diesen Report und diese Maske
    Description                     nvarchar(256)   null,                       -- Beschreibung
    CreationDate                    datetime2       not null default getdate(),
    CreationSource                  nvarchar(32)    null,
    ModificationDate                datetime2       not null default getdate(),
    ModificationSource              nvarchar(32)    null,
    Locked                          bit             not null default 0,
    LockSource                      nvarchar(32)    null,

    CONSTRAINT [PK_PrinterDefaults] PRIMARY KEY ( UserName, ReportLayout, Dialogue, Printer ),
    CONSTRAINT [FK_PrinterDefaults_Printers] FOREIGN KEY ( Printer ) REFERENCES [cust].[Printers] ( Printer ) ON DELETE CASCADE
)
