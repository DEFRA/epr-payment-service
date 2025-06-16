BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[OfflinePayment]') AND [c].[name] = N'PaymentMethod');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [OfflinePayment] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [OfflinePayment] DROP COLUMN [PaymentMethod];
GO

ALTER TABLE [OfflinePayment] ADD [PaymentMethodId] int NOT NULL DEFAULT 1;
GO

CREATE INDEX [IX_OfflinePayment_PaymentMethodId] ON [OfflinePayment] ([PaymentMethodId]);
GO

ALTER TABLE [OfflinePayment] ADD CONSTRAINT [FK_OfflinePayment_PaymentMethod_PaymentMethodId] FOREIGN KEY ([PaymentMethodId]) REFERENCES [Lookup].[PaymentMethod] ([Id]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250616080947_ChangedOfflinePaymentTable', N'8.0.4');
GO

COMMIT;
GO

