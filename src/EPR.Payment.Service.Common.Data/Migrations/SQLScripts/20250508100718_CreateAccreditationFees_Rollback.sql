BEGIN TRANSACTION;
GO

DROP TABLE [Lookup].[AccreditationFees];
GO

DELETE FROM [__EFMigrationsHistory]
WHERE [MigrationId] = N'20250508100718_CreateAccreditationFees';
GO

COMMIT;
GO
