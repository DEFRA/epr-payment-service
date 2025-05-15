BEGIN TRANSACTION;
GO

ALTER TABLE [OnlinePayment] ADD [RequestorType] nvarchar(50) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250515115823_AddingRequestorTypeColumn', N'8.0.4');
GO

COMMIT;
GO

