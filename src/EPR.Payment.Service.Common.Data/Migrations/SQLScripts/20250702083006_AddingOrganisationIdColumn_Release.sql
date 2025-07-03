BEGIN TRANSACTION;
GO

ALTER TABLE [OfflinePayment] ADD [OrganisationId] uniqueidentifier NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250702083006_AddingOrganisationIdColumn', N'8.0.4');
GO

COMMIT;
GO

