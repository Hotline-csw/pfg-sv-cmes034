-- Printing Center Confi
-- SF - 2025-06-16

CREATE TABLE[cust].[PrintPDFs]
(
    Sequence                        bigint          not null identity(1,1),
    BinariesSequence                int             not null,                   -- Referenz to object in table Binaries
    Printer                         nvarchar(64)    not null,                   -- Printer
    Description                     nvarchar(256)   not null,                   -- Description
    WebPDFTokenID                   nvarchar(1024)  null,                       -- Token from web service for session
    WebPDFDocumentID                nvarchar(256)   null,                       -- Token from web service for document
    ProcessingState                 int             not null,                   -- Process state for the record
                                                                                --    0 = MES is actually writing the record
                                                                                --   10 = Record is ready for the consuming job
                                                                                --   15 = Record is read by the consuming job
                                                                                --   20 = Consuming job has done its work
    ErrorState                      int             not null,                   -- Error number, individual for the action
                                                                                --    0 = No error
    ErrorString                     nvarchar(max)   null,                       -- Error text, individual for the action
    CreationDate                    datetime2       not null default getdate(),
    CreationSource                  nvarchar(32)    null,
    ModificationDate                datetime2       not null default getdate(),
    ModificationSource              nvarchar(32)    null,
    Locked                          bit             not null default 0,
    LockSource                      nvarchar(32)    null,

    CONSTRAINT [PK_PrintPDFs] PRIMARY KEY ( Sequence )
)
GO


CREATE INDEX [IX_PrintPDFs_ProcessingState] ON [cust].[PrintPDFs]
(
    ProcessingState
)
GO