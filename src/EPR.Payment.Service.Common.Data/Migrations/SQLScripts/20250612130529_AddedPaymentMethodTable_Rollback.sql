BEGIN TRANSACTION;
GO

DROP TABLE [Lookup].[PaymentMethod];
GO

DELETE FROM [__EFMigrationsHistory]
WHERE [MigrationId] = N'20250612130529_AddedPaymentMethodTable';
GO

COMMIT;
GO

