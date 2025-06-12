BEGIN TRANSACTION;
GO

ALTER TABLE [OnlinePayment] DROP CONSTRAINT [FK_OnlinePayment_RequestorType_RequestorTypeId];
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[OnlinePayment]') AND [c].[name] = N'RequestorTypeId');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [OnlinePayment] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [OnlinePayment] DROP COLUMN [RequestorTypeId];
GO

ALTER TABLE [OnlinePayment] ADD [RequestorType] nvarchar(50) NULL;
GO

DELETE FROM [__EFMigrationsHistory]
WHERE [MigrationId] = N'20250611164617_ChangedOnlinePaymentTable';
GO

COMMIT;
GO

