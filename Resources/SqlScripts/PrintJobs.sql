-- Printing Center Confi
-- SF - 2025-06-13

CREATE TABLE[cust].[PrintJobs]
(
    Sequence                        bigint          not null identity(1,1),
    ProductionItemCode              nvarchar(32)    null,                       -- Part identifier
    ProductionOrderCode             nvarchar(32)    null,                       -- Production order
    CustomerOrderCode               nvarchar(32)    null,                       -- Customer order
    CustomerOrderPosition           nvarchar(32)    null,                       -- Customer order position
    //StackCode                       nvarchar(32)    null,                       -- Stapelnummer
    OptimizationCode                nvarchar(32)    null,                       -- Optimierungsnummer
    WorkOrder                       nvarchar(32)    null,                       -- Werksauftrag
    Quantity                        int             null,                       -- Menge
    TransferState                   int             not null,                   -- Transfer state
                                                                                --    0 = Record not ready for the processing job
                                                                                --   10 = Record is ready for the processing job
                                                                                --   15 = Record will be actually read by the processing job
                                                                                --   20 = Record is finished by the processing job
    JobName                         nvarchar(64)    not null,                   -- Name des Report Jobs
                                                                                -- z.B. Print'name of report'-'name of printer'
    BinariesSequence                bigint          null,                       -- Referenz auf das Objekt in der Tabelle Binaries
    ReportBindingsSequence          bigint          null,                       -- Sequence zu ReportEntity
    CreationDate                    datetime2       not null default getdate(),
    CreationSource                  nvarchar(32)    null,
    ModificationDate                datetime2       not null default getdate(),
    ModificationSource              nvarchar(32)    null,
    Locked                          bit             not null default 0,
    LockSource                      nvarchar(32)    null,

    CONSTRAINT [PK_PrintJobs] PRIMARY KEY ( Sequence )
)
GO


CREATE INDEX [IX_PrintJobs_TransferState] ON [cust].[PrintJobs]
(
    TransferState, JobName
)
GO
