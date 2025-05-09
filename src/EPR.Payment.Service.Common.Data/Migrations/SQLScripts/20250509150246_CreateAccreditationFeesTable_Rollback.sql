BEGIN TRANSACTION;
GO

DROP TABLE [Lookup].[AccreditationFees];
GO

DELETE FROM [__EFMigrationsHistory]
WHERE [MigrationId] = N'20250509150246_CreateAccreditationFeesTable';
GO

COMMIT;
GO

