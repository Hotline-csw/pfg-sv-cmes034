-- PrintJobItem-Table for Stack lable
-- Required due to in the original table ProductionItemCode needed, but in case of stack is not available
-- S.Feist - 2025-05-26

CREATE TABLE[cust].[StackPrintJobItems]
 ([Sequence] [bigint] IDENTITY(1,1) NOT NULL,
[StackCode] [nvarchar](32) NOT NULL,
[ProcessingState] [int] NOT NULL,
[JobName] [nvarchar](64) NOT NULL,
[CreationDate] [datetime2](7) NOT NULL default getdate(),
[CreationSource] [nvarchar](32) NULL,
[ModificationDate] [datetime2](7) NOT NULL default getdate(),
[ModificationSource] [nvarchar](32) NULL,
[Locked] [bit]NOT NULL default 0,
[LockSource] [nvarchar](32) NULL,
 CONSTRAINT [PK_StackPrintJobItems] PRIMARY KEY CLUSTERED 
(
	[Sequence] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [cust].[StackPrintJobItems]  WITH CHECK ADD  CONSTRAINT [FK_StackPrintJobItems_Stacks] FOREIGN KEY([StackCode])
REFERENCES [base].[Stacks] ([StackCode])
ON DELETE CASCADE
GO



