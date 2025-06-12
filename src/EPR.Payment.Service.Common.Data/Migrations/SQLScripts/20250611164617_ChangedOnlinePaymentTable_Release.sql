BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[OnlinePayment]') AND [c].[name] = N'RequestorType');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [OnlinePayment] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [OnlinePayment] DROP COLUMN [RequestorType];
GO

ALTER TABLE [OnlinePayment] ADD [RequestorTypeId] int NOT NULL DEFAULT 1;
GO

ALTER TABLE [OnlinePayment] ADD CONSTRAINT [FK_OnlinePayment_RequestorType_RequestorTypeId] FOREIGN KEY ([RequestorTypeId]) REFERENCES [Lookup].[RequestorType] ([Id]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250611164617_ChangedOnlinePaymentTable', N'8.0.4');
GO

COMMIT;
GO

