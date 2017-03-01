CREATE TABLE [Read].[SystemTag] (
    [SystemTagId]         INT              IDENTITY (1, 1) NOT NULL,
    [AccountId]           UNIQUEIDENTIFIER NOT NULL,
    [Name]                NVARCHAR (MAX)   NULL,
    [AppliesToExpenses]   BIT              NOT NULL,
    [AppliesToTimesheets] BIT              NOT NULL,
    CONSTRAINT [PK_Read.SystemTag] PRIMARY KEY CLUSTERED ([SystemTagId] ASC)
);

