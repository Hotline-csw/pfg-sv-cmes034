-- Printing Center Confi
-- SF - 2025-06-13

CREATE TABLE[cust].[ReportViews]
(

    Sequence                        bigint          not null identity(1,1),
    ReportView                      nvarchar(64)    not null,                   -- Name des Datenbank-View für den Report, z.B. ViewReportProducerLabels
    CreationDate                    datetime2       not null default getdate(),
    CreationSource                  nvarchar(32)    null,
    ModificationDate                datetime2       not null default getdate(),
    ModificationSource              nvarchar(32)    null,
    Locked                          bit             not null default 0,
    LockSource                      nvarchar(32)    null,

    CONSTRAINT [PK_ReportViews] PRIMARY KEY ( ReportView )

)
