CREATE TABLE [Write].[AccountEvents] (
    [EventId]          UNIQUEIDENTIFIER NOT NULL,
    [Name]             NVARCHAR (200)   NOT NULL,
    [AggregateId]      UNIQUEIDENTIFIER NOT NULL,
    [Payload]          NVARCHAR (4000)  NOT NULL,
    [EventSequence]    INT              NOT NULL,
    [CreatedOnUtc]     DATETIME         NOT NULL,
    [AggregateVersion] INT              NOT NULL,
    CONSTRAINT [PK_Write.AccountEvents] PRIMARY KEY CLUSTERED ([EventId] ASC)
);

