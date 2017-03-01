CREATE TABLE [Write].[AccountSnapshot] (
    [SnapshotId]        UNIQUEIDENTIFIER NOT NULL,
    [AggregateRootId]   UNIQUEIDENTIFIER NOT NULL,
    [LastEventSequence] INT              NOT NULL,
    [Payload]           NVARCHAR (4000)  NULL,
    CONSTRAINT [PK_Write.AccountSnapshot] PRIMARY KEY CLUSTERED ([SnapshotId] ASC)
);

