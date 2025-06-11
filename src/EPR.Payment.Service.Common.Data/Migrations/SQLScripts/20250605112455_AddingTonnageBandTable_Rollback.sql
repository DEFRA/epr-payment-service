BEGIN TRANSACTION;
GO

DROP TABLE [Lookup].[TonnageBand];
GO

DELETE FROM [__EFMigrationsHistory]
WHERE [MigrationId] = N'20250605112455_AddingTonnageBandTable';
GO

COMMIT;
GO

