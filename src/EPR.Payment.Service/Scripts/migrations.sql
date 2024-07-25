IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240723085319_Initial'
)
BEGIN
    IF SCHEMA_ID(N'Lookup') IS NULL EXEC(N'CREATE SCHEMA [Lookup];');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240723085319_Initial'
)
BEGIN
    CREATE TABLE [Lookup].[AdditionalRegistrationFees] (
        [Id] int NOT NULL IDENTITY,
        [FeesSubType] nvarchar(255) NOT NULL,
        [Description] nvarchar(255) NOT NULL,
        [Regulator] nvarchar(255) NOT NULL,
        [Amount] decimal(19,4) NOT NULL,
        [EffectiveFrom] datetime2 NOT NULL,
        [EffectiveTo] datetime2 NOT NULL,
        CONSTRAINT [PK_AdditionalRegistrationFees] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240723085319_Initial'
)
BEGIN
    CREATE TABLE [Lookup].[ComplianceSchemeRegistrationFees] (
        [Id] int NOT NULL IDENTITY,
        [FeesType] nvarchar(255) NOT NULL,
        [Description] nvarchar(255) NOT NULL,
        [Regulator] nvarchar(255) NOT NULL,
        [Amount] decimal(19,4) NOT NULL,
        [EffectiveFrom] datetime2 NOT NULL,
        [EffectiveTo] datetime2 NOT NULL,
        CONSTRAINT [PK_ComplianceSchemeRegistrationFees] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240723085319_Initial'
)
BEGIN
    CREATE TABLE [Lookup].[PaymentStatus] (
        [Id] int NOT NULL,
        [Status] nvarchar(20) NOT NULL,
        CONSTRAINT [PK_PaymentStatus] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240723085319_Initial'
)
BEGIN
    CREATE TABLE [Lookup].[ProducerRegistrationFees] (
        [Id] int NOT NULL IDENTITY,
        [ProducerType] nvarchar(255) NOT NULL,
        [Description] nvarchar(255) NOT NULL,
        [Regulator] nvarchar(255) NOT NULL,
        [Amount] decimal(19,4) NOT NULL,
        [EffectiveFrom] datetime2 NOT NULL,
        [EffectiveTo] datetime2 NOT NULL,
        CONSTRAINT [PK_ProducerRegistrationFees] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240723085319_Initial'
)
BEGIN
    CREATE TABLE [Lookup].[SubsidiariesRegistrationFees] (
        [Id] int NOT NULL IDENTITY,
        [MinNumberOfSubsidiaries] int NOT NULL,
        [MaxNumberOfSubsidiaries] int NOT NULL,
        [Description] nvarchar(255) NOT NULL,
        [Regulator] nvarchar(255) NOT NULL,
        [Amount] decimal(19,4) NOT NULL,
        [EffectiveFrom] datetime2 NOT NULL,
        [EffectiveTo] datetime2 NOT NULL,
        CONSTRAINT [PK_SubsidiariesRegistrationFees] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240723085319_Initial'
)
BEGIN
    CREATE TABLE [Payment] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        [OrganisationId] uniqueidentifier NOT NULL,
        [ExternalPaymentId] uniqueidentifier NOT NULL DEFAULT (NEWID()),
        [GovpayPaymentId] nvarchar(50) NULL,
        [InternalStatusId] int NOT NULL,
        [Regulator] nvarchar(20) NOT NULL,
        [GovPayStatus] nvarchar(20) NULL,
        [ErrorCode] nvarchar(255) NULL,
        [ErrorMessage] nvarchar(255) NULL,
        [Reference] nvarchar(255) NOT NULL,
        [Amount] decimal(19,4) NOT NULL,
        [ReasonForPayment] nvarchar(255) NOT NULL,
        [CreatedDate] datetime2 NOT NULL,
        [UpdatedByUserId] uniqueidentifier NOT NULL,
        [UpdatedByOrganisationId] uniqueidentifier NOT NULL,
        [UpdatedDate] datetime2 NOT NULL,
        CONSTRAINT [PK_Payment] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Payment_PaymentStatus_InternalStatusId] FOREIGN KEY ([InternalStatusId]) REFERENCES [Lookup].[PaymentStatus] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240723085319_Initial'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Amount', N'Description', N'EffectiveFrom', N'EffectiveTo', N'FeesSubType', N'Regulator') AND [object_id] = OBJECT_ID(N'[Lookup].[AdditionalRegistrationFees]'))
        SET IDENTITY_INSERT [Lookup].[AdditionalRegistrationFees] ON;
    EXEC(N'INSERT INTO [Lookup].[AdditionalRegistrationFees] ([Id], [Amount], [Description], [EffectiveFrom], [EffectiveTo], [FeesSubType], [Regulator])
    VALUES (1, 714.0, N''Resubmission'', ''2024-01-01T00:00:00.0000000'', ''2025-01-01T00:00:00.0000000'', N''Resub'', N''GB-ENG''),
    (2, 714.0, N''Resubmission'', ''2024-01-01T00:00:00.0000000'', ''2025-01-01T00:00:00.0000000'', N''Resub'', N''GB-SCT''),
    (3, 714.0, N''Resubmission'', ''2024-01-01T00:00:00.0000000'', ''2025-01-01T00:00:00.0000000'', N''Resub'', N''GB-WLS''),
    (4, 714.0, N''Resubmission'', ''2024-01-01T00:00:00.0000000'', ''2025-01-01T00:00:00.0000000'', N''Resub'', N''GB-NIR''),
    (5, 332.0, N''Late'', ''2024-01-01T00:00:00.0000000'', ''2025-01-01T00:00:00.0000000'', N''Late'', N''GB-ENG''),
    (6, 332.0, N''Late'', ''2024-01-01T00:00:00.0000000'', ''2025-01-01T00:00:00.0000000'', N''Late'', N''GB-SCT''),
    (7, 332.0, N''Late'', ''2024-01-01T00:00:00.0000000'', ''2025-01-01T00:00:00.0000000'', N''Late'', N''GB-WLS''),
    (8, 332.0, N''Late'', ''2024-01-01T00:00:00.0000000'', ''2025-01-01T00:00:00.0000000'', N''Late'', N''GB-NIR'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Amount', N'Description', N'EffectiveFrom', N'EffectiveTo', N'FeesSubType', N'Regulator') AND [object_id] = OBJECT_ID(N'[Lookup].[AdditionalRegistrationFees]'))
        SET IDENTITY_INSERT [Lookup].[AdditionalRegistrationFees] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240723085319_Initial'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Amount', N'Description', N'EffectiveFrom', N'EffectiveTo', N'FeesType', N'Regulator') AND [object_id] = OBJECT_ID(N'[Lookup].[ComplianceSchemeRegistrationFees]'))
        SET IDENTITY_INSERT [Lookup].[ComplianceSchemeRegistrationFees] ON;
    EXEC(N'INSERT INTO [Lookup].[ComplianceSchemeRegistrationFees] ([Id], [Amount], [Description], [EffectiveFrom], [EffectiveTo], [FeesType], [Regulator])
    VALUES (1, 13804.0, N''Registration'', ''2024-01-01T00:00:00.0000000'', ''2025-01-01T00:00:00.0000000'', N''Reg'', N''GB-ENG''),
    (2, 13804.0, N''Registration'', ''2024-01-01T00:00:00.0000000'', ''2025-01-01T00:00:00.0000000'', N''Reg'', N''GB-SCT''),
    (3, 13804.0, N''Registration'', ''2024-01-01T00:00:00.0000000'', ''2025-01-01T00:00:00.0000000'', N''Reg'', N''GB-WLS''),
    (4, 13804.0, N''Registration'', ''2024-01-01T00:00:00.0000000'', ''2025-01-01T00:00:00.0000000'', N''Reg'', N''GB-NIR''),
    (5, 1658.0, N''Large Producer'', ''2024-01-01T00:00:00.0000000'', ''2025-01-01T00:00:00.0000000'', N''L'', N''GB-ENG''),
    (6, 1658.0, N''Large Producer'', ''2024-01-01T00:00:00.0000000'', ''2025-01-01T00:00:00.0000000'', N''L'', N''GB-SCT''),
    (7, 1658.0, N''Large Producer'', ''2024-01-01T00:00:00.0000000'', ''2025-01-01T00:00:00.0000000'', N''L'', N''GB-WLS''),
    (8, 1658.0, N''Large Producer'', ''2024-01-01T00:00:00.0000000'', ''2025-01-01T00:00:00.0000000'', N''L'', N''GB-NIR''),
    (9, 631.0, N''Small Producer'', ''2024-01-01T00:00:00.0000000'', ''2025-01-01T00:00:00.0000000'', N''S'', N''GB-ENG''),
    (10, 631.0, N''Small Producer'', ''2024-01-01T00:00:00.0000000'', ''2025-01-01T00:00:00.0000000'', N''S'', N''GB-SCT''),
    (11, 631.0, N''Small Producer'', ''2024-01-01T00:00:00.0000000'', ''2025-01-01T00:00:00.0000000'', N''S'', N''GB-WLS''),
    (12, 631.0, N''Small Producer'', ''2024-01-01T00:00:00.0000000'', ''2025-01-01T00:00:00.0000000'', N''S'', N''GB-NIR''),
    (13, 2579.0, N''Online Market'', ''2024-01-01T00:00:00.0000000'', ''2025-01-01T00:00:00.0000000'', N''On'', N''GB-ENG''),
    (14, 2579.0, N''Online Market'', ''2024-01-01T00:00:00.0000000'', ''2025-01-01T00:00:00.0000000'', N''On'', N''GB-SCT''),
    (15, 2579.0, N''Online Market'', ''2024-01-01T00:00:00.0000000'', ''2025-01-01T00:00:00.0000000'', N''On'', N''GB-WLS''),
    (16, 2579.0, N''Online Market'', ''2024-01-01T00:00:00.0000000'', ''2025-01-01T00:00:00.0000000'', N''On'', N''GB-NIR'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Amount', N'Description', N'EffectiveFrom', N'EffectiveTo', N'FeesType', N'Regulator') AND [object_id] = OBJECT_ID(N'[Lookup].[ComplianceSchemeRegistrationFees]'))
        SET IDENTITY_INSERT [Lookup].[ComplianceSchemeRegistrationFees] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240723085319_Initial'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Status') AND [object_id] = OBJECT_ID(N'[Lookup].[PaymentStatus]'))
        SET IDENTITY_INSERT [Lookup].[PaymentStatus] ON;
    EXEC(N'INSERT INTO [Lookup].[PaymentStatus] ([Id], [Status])
    VALUES (0, N''Initiated''),
    (1, N''InProgress''),
    (2, N''Success''),
    (3, N''Failed''),
    (4, N''Error''),
    (5, N''UserCancelled'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Status') AND [object_id] = OBJECT_ID(N'[Lookup].[PaymentStatus]'))
        SET IDENTITY_INSERT [Lookup].[PaymentStatus] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240723085319_Initial'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Amount', N'Description', N'EffectiveFrom', N'EffectiveTo', N'ProducerType', N'Regulator') AND [object_id] = OBJECT_ID(N'[Lookup].[ProducerRegistrationFees]'))
        SET IDENTITY_INSERT [Lookup].[ProducerRegistrationFees] ON;
    EXEC(N'INSERT INTO [Lookup].[ProducerRegistrationFees] ([Id], [Amount], [Description], [EffectiveFrom], [EffectiveTo], [ProducerType], [Regulator])
    VALUES (1, 2620.0, N''Large producer'', ''2024-01-01T00:00:00.0000000'', ''2025-01-01T00:00:00.0000000'', N''L'', N''GB-ENG''),
    (2, 2620.0, N''Large producer'', ''2024-01-01T00:00:00.0000000'', ''2025-01-01T00:00:00.0000000'', N''L'', N''GB-SCT''),
    (3, 2620.0, N''Large producer'', ''2024-01-01T00:00:00.0000000'', ''2025-01-01T00:00:00.0000000'', N''L'', N''GB-WLS''),
    (4, 2620.0, N''Large producer'', ''2024-01-01T00:00:00.0000000'', ''2025-01-01T00:00:00.0000000'', N''L'', N''GB-NIR''),
    (5, 1216.0, N''Small producer'', ''2024-01-01T00:00:00.0000000'', ''2025-01-01T00:00:00.0000000'', N''S'', N''GB-ENG''),
    (6, 1216.0, N''Small producer'', ''2024-01-01T00:00:00.0000000'', ''2025-01-01T00:00:00.0000000'', N''S'', N''GB-SCT''),
    (7, 1216.0, N''Small producer'', ''2024-01-01T00:00:00.0000000'', ''2025-01-01T00:00:00.0000000'', N''S'', N''GB-WLS''),
    (8, 1216.0, N''Small producer'', ''2024-01-01T00:00:00.0000000'', ''2025-01-01T00:00:00.0000000'', N''S'', N''GB-NIR'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Amount', N'Description', N'EffectiveFrom', N'EffectiveTo', N'ProducerType', N'Regulator') AND [object_id] = OBJECT_ID(N'[Lookup].[ProducerRegistrationFees]'))
        SET IDENTITY_INSERT [Lookup].[ProducerRegistrationFees] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240723085319_Initial'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Amount', N'Description', N'EffectiveFrom', N'EffectiveTo', N'MaxNumberOfSubsidiaries', N'MinNumberOfSubsidiaries', N'Regulator') AND [object_id] = OBJECT_ID(N'[Lookup].[SubsidiariesRegistrationFees]'))
        SET IDENTITY_INSERT [Lookup].[SubsidiariesRegistrationFees] ON;
    EXEC(N'INSERT INTO [Lookup].[SubsidiariesRegistrationFees] ([Id], [Amount], [Description], [EffectiveFrom], [EffectiveTo], [MaxNumberOfSubsidiaries], [MinNumberOfSubsidiaries], [Regulator])
    VALUES (1, 558.0, N''Up to 20'', ''2024-01-01T00:00:00.0000000'', ''2025-01-01T00:00:00.0000000'', 20, 1, N''GB-ENG''),
    (2, 558.0, N''Up to 20'', ''2024-01-01T00:00:00.0000000'', ''2025-01-01T00:00:00.0000000'', 20, 1, N''GB-SCT''),
    (3, 558.0, N''Up to 20'', ''2024-01-01T00:00:00.0000000'', ''2025-01-01T00:00:00.0000000'', 20, 1, N''GB-WLS''),
    (4, 558.0, N''Up to 20'', ''2024-01-01T00:00:00.0000000'', ''2025-01-01T00:00:00.0000000'', 20, 1, N''GB-NIR''),
    (5, 140.0, N''More then 20'', ''2024-01-01T00:00:00.0000000'', ''2025-01-01T00:00:00.0000000'', 100, 21, N''GB-ENG''),
    (6, 140.0, N''More then 20'', ''2024-01-01T00:00:00.0000000'', ''2025-01-01T00:00:00.0000000'', 100, 21, N''GB-SCT''),
    (7, 140.0, N''More then 20'', ''2024-01-01T00:00:00.0000000'', ''2025-01-01T00:00:00.0000000'', 100, 21, N''GB-WLS''),
    (8, 140.0, N''More then 20'', ''2024-01-01T00:00:00.0000000'', ''2025-01-01T00:00:00.0000000'', 100, 21, N''GB-NIR'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Amount', N'Description', N'EffectiveFrom', N'EffectiveTo', N'MaxNumberOfSubsidiaries', N'MinNumberOfSubsidiaries', N'Regulator') AND [object_id] = OBJECT_ID(N'[Lookup].[SubsidiariesRegistrationFees]'))
        SET IDENTITY_INSERT [Lookup].[SubsidiariesRegistrationFees] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240723085319_Initial'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Payment_ExternalPaymentId] ON [Payment] ([ExternalPaymentId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240723085319_Initial'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Payment_GovpayPaymentId] ON [Payment] ([GovpayPaymentId]) WHERE [GovpayPaymentId] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240723085319_Initial'
)
BEGIN
    CREATE INDEX [IX_Payment_InternalStatusId] ON [Payment] ([InternalStatusId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240723085319_Initial'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240723085319_Initial', N'8.0.4');
END;
GO

COMMIT;
GO

