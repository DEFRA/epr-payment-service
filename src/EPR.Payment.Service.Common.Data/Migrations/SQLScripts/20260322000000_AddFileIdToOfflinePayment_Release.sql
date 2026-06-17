BEGIN TRANSACTION;
GO

ALTER TABLE [Payment] ADD [FileId] uniqueidentifier NULL;
GO

ALTER TABLE [OfflinePayment] ADD [FileId] uniqueidentifier NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260322000000_AddFileIdToOfflinePayment', N'8.0.4');
GO

COMMIT;
GO

