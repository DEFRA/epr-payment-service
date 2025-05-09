BEGIN TRANSACTION;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Type') AND [object_id] = OBJECT_ID(N'[Lookup].[Group]'))
    SET IDENTITY_INSERT [Lookup].[Group] ON;
INSERT INTO [Lookup].[Group] ([Id], [Description], [Type])
VALUES (7, 'Exporters', 'Exporters'),
(8, 'Reprocessors', 'Reprocessors');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Type') AND [object_id] = OBJECT_ID(N'[Lookup].[Group]'))
    SET IDENTITY_INSERT [Lookup].[Group] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Type') AND [object_id] = OBJECT_ID(N'[Lookup].[SubGroup]'))
    SET IDENTITY_INSERT [Lookup].[SubGroup] ON;
INSERT INTO [Lookup].[SubGroup] ([Id], [Description], [Type])
VALUES (9, 'Aluminium', 'Aluminium'),
(10, 'Glass', 'Glass'),
(11, 'Paper, board or fibre-based composite material', 'PaperOrBoardOrFibreBasedCompositeMaterial'),
(12, 'Plastic', 'Plastic'),
(13, 'Steel', 'Steel'),
(14, 'Wood', 'Wood');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Type') AND [object_id] = OBJECT_ID(N'[Lookup].[SubGroup]'))
    SET IDENTITY_INSERT [Lookup].[SubGroup] OFF;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250508103111_SeedDataGroupandSubGroupTables', N'8.0.4');
GO

COMMIT;
GO