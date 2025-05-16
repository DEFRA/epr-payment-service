BEGIN TRANSACTION;
GO

ALTER TABLE [OfflinePayment] ADD [PaymentMethod] nvarchar(20) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250515105845_AddingPaymentMethodColumn', N'8.0.4');
GO

COMMIT;
GO

