CREATE TABLE [Write].[AccountNumber] (
    [AccountNumberId] INT           IDENTITY (1, 1) NOT NULL,
    [CreatedOnUtc]    DATETIME2 (7) NOT NULL,
    CONSTRAINT [PK_Write.AccountNumber] PRIMARY KEY CLUSTERED ([AccountNumberId] ASC)
);

