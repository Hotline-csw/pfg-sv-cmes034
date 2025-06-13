-- "Reports" TABLE
-- Printing Center Confi
-- SF - 2025-06-13

CREATE TABLE[cust].[Reports]
(
    Sequence                        bigint          not null identity(1,1),
    Layout                          nvarchar(64)    not null,                   -- Name des Report Layout
    Description                     nvarchar(256)   null,                       -- Beschreibung
    Entity                          nvarchar(64)    null,                       -- Tabelle, an der der Report logisch zugeordnet ist,
                                                                                -- notwendig für eine automatische Generierung
    JobName                         nvarchar(64)    not null,                   -- Name des Report Jobs
    ReportView                      nvarchar(64)    null,                       -- Name des Datenbank-View für den Report, z.B. ViewReportProducerLabels
    CreationDate                    datetime2       not null default getdate(),
    CreationSource                  nvarchar(32)    null,
    ModificationDate                datetime2       not null default getdate(),
    ModificationSource              nvarchar(32)    null,
    Locked                          bit             not null default 0,
    LockSource                      nvarchar(32)    null,

    CONSTRAINT [PK_Reports] PRIMARY KEY ( Layout ),
    CONSTRAINT [FK_Reports_ReportViews] FOREIGN KEY ( ReportView ) REFERENCES [cust].[ReportViews] ( ReportView ) ON DELETE CASCADE
)
