BEGIN TRANSACTION;
GO

DROP TABLE [Lookup].[AccreditationFees];
GO

DELETE FROM [__EFMigrationsHistory]
WHERE [MigrationId] = N'20250509154544_CreateAccreditationFeesTable';
GO

COMMIT;
GO

