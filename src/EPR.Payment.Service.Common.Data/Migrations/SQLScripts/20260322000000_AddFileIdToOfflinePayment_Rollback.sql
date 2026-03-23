BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Payment]') AND [c].[name] = N'FileId');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Payment] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Payment] DROP COLUMN [FileId];
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[OfflinePayment]') AND [c].[name] = N'FileId');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [OfflinePayment] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [OfflinePayment] DROP COLUMN [FileId];
GO

DELETE FROM [__EFMigrationsHistory]
WHERE [MigrationId] = N'20260322000000_AddFileIdToOfflinePayment';
GO

COMMIT;
GO

