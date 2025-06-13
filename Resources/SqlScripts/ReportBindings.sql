-- Printing Center Confi
-- SF - 2025-06-13

CREATE TABLE[cust].[ReportBindings]
(
    Sequence                        bigint          not null identity(1,1),
    DateReportRelevance             datetime2       not null,                   -- Datum für das der Report relevant ist
    PlanningGroup                   nvarchar(32)    not null,                   -- Dispogruppe
    WorkOrder                       nvarchar(32)    not null,                   -- Werkauftrag
    ProductionOrderCode             nvarchar(32)    null,                       -- Fertigungsauftrag
    ProductionItemCode              nvarchar(32)    null,                       -- Bauteilnummer
    CustomerOrderCode               nvarchar(32)    null,                       -- Kundenauftragsnummer
    CustomerOrderPosition           nvarchar(32)    null,                       -- Kundenauftragsposition
    OptimizationCode                nvarchar(32)    null,                       -- Optimierungsnummer
    VirtualCartCode                 nvarchar(64)    null,                       -- Virtuelle Wagennummer
    ReportLayout                    nvarchar(64)    not null,                   -- Name des Report Layout
    ReportEntity                    nvarchar(64)    not null,                   -- Tabelle, an der der Report logisch zugeordnet ist
    ReportSequence                  bigint          not null,                   -- Sequence zu ReportEntity
    BinariesSequence                int             null,                       -- Referenz auf das Objekt in der Tabelle Binaries
    CreationDate                    datetime2       not null default getdate(),
    CreationSource                  nvarchar(32)    null,
    ModificationDate                datetime2       not null default getdate(),
    ModificationSource              nvarchar(32)    null,
    Locked                          bit             not null default 0,
    LockSource                      nvarchar(32)    null,

    CONSTRAINT [PK_ReportBindings] PRIMARY KEY ( Sequence ),
    CONSTRAINT [FK_ReportBindings_ReportLayout] FOREIGN KEY ( ReportLayout ) REFERENCES [cust].[Reports] ( Layout ) ON DELETE CASCADE,
    CONSTRAINT [UQ_ReportBindings_ReportLayout] UNIQUE ( ReportEntity, ReportSequence, ReportLayout )
)
