BEGIN TRANSACTION;
GO

CREATE TABLE [Lookup].[TonnageBand] (
    [Id] int NOT NULL IDENTITY,
    [Type] varchar(50) NOT NULL,
    [Description] varchar(255) NOT NULL,
    CONSTRAINT [PK_TonnageBand] PRIMARY KEY ([Id])
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Type') AND [object_id] = OBJECT_ID(N'[Lookup].[TonnageBand]'))
    SET IDENTITY_INSERT [Lookup].[TonnageBand] ON;
INSERT INTO [Lookup].[TonnageBand] ([Id], [Description], [Type])
VALUES (1, 'Tonnage upto 500 tonnes', 'Upto500'),
(2, 'Tonnage over 500 to 5000 tonnes', 'Over500To5000'),
(3, 'Tonnage over 5000 to 10000 tonnes', 'Over5000To10000'),
(4, 'Tonnage over 10000 tonnes', 'Over10000');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Type') AND [object_id] = OBJECT_ID(N'[Lookup].[TonnageBand]'))
    SET IDENTITY_INSERT [Lookup].[TonnageBand] OFF;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250605112455_AddingTonnageBandTable', N'8.0.4');
GO

COMMIT;
GO

