BEGIN TRANSACTION;
GO

DROP TABLE [Lookup].[RequestorType];
GO

DELETE FROM [__EFMigrationsHistory]
WHERE [MigrationId] = N'20250611100506_AddingRequestorTypeTable';
GO

COMMIT;
GO

