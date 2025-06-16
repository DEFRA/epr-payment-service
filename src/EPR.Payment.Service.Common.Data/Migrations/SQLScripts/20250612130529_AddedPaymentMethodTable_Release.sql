BEGIN TRANSACTION;
GO

CREATE TABLE [Lookup].[PaymentMethod] (
    [Id] int NOT NULL IDENTITY,
    [Type] varchar(50) NOT NULL,
    [Description] varchar(255) NOT NULL,
    CONSTRAINT [PK_PaymentMethod] PRIMARY KEY ([Id])
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Type') AND [object_id] = OBJECT_ID(N'[Lookup].[PaymentMethod]'))
    SET IDENTITY_INSERT [Lookup].[PaymentMethod] ON;
INSERT INTO [Lookup].[PaymentMethod] ([Id], [Description], [Type])
VALUES (1, 'Not Applicable', 'NA'),
(2, 'Bank transfer', 'BankTransfer'),
(3, 'Credit or debit card', 'CreditOrDebitCard'),
(4, 'Cheque', 'Cheque'),
(5, 'Cash', 'Cash');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Type') AND [object_id] = OBJECT_ID(N'[Lookup].[PaymentMethod]'))
    SET IDENTITY_INSERT [Lookup].[PaymentMethod] OFF;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250612130529_AddedPaymentMethodTable', N'8.0.4');
GO

COMMIT;
GO

