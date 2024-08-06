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

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240725154904_RegistrationFeesTablesUpdate'
)
BEGIN
    DROP TABLE [Lookup].[AdditionalRegistrationFees];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240725154904_RegistrationFeesTablesUpdate'
)
BEGIN
    DROP TABLE [Lookup].[ComplianceSchemeRegistrationFees];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240725154904_RegistrationFeesTablesUpdate'
)
BEGIN
    DROP TABLE [Lookup].[ProducerRegistrationFees];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240725154904_RegistrationFeesTablesUpdate'
)
BEGIN
    DROP TABLE [Lookup].[SubsidiariesRegistrationFees];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240725154904_RegistrationFeesTablesUpdate'
)
BEGIN
    CREATE TABLE [Lookup].[Group] (
        [Id] int NOT NULL IDENTITY,
        [Type] nvarchar(50) NOT NULL,
        [Description] nvarchar(255) NOT NULL,
        CONSTRAINT [PK_Group] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240725154904_RegistrationFeesTablesUpdate'
)
BEGIN
    CREATE TABLE [Lookup].[Regulator] (
        [Id] int NOT NULL IDENTITY,
        [Type] nvarchar(50) NOT NULL,
        [Description] nvarchar(255) NOT NULL,
        CONSTRAINT [PK_Regulator] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240725154904_RegistrationFeesTablesUpdate'
)
BEGIN
    CREATE TABLE [Lookup].[SubGroup] (
        [Id] int NOT NULL IDENTITY,
        [Type] nvarchar(50) NOT NULL,
        [Description] nvarchar(255) NOT NULL,
        CONSTRAINT [PK_SubGroup] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240725154904_RegistrationFeesTablesUpdate'
)
BEGIN
    CREATE TABLE [Lookup].[RegistrationFees] (
        [Id] int NOT NULL IDENTITY,
        [GroupId] int NOT NULL,
        [SubGroupId] int NOT NULL,
        [RegulatorId] int NOT NULL,
        [Amount] decimal(19,4) NOT NULL,
        [EffectiveFrom] datetime2 NOT NULL,
        [EffectiveTo] datetime2 NOT NULL,
        CONSTRAINT [PK_RegistrationFees] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RegistrationFees_Group_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [Lookup].[Group] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_RegistrationFees_Regulator_RegulatorId] FOREIGN KEY ([RegulatorId]) REFERENCES [Lookup].[Regulator] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_RegistrationFees_SubGroup_SubGroupId] FOREIGN KEY ([SubGroupId]) REFERENCES [Lookup].[SubGroup] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240725154904_RegistrationFeesTablesUpdate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Type') AND [object_id] = OBJECT_ID(N'[Lookup].[Group]'))
        SET IDENTITY_INSERT [Lookup].[Group] ON;
    EXEC(N'INSERT INTO [Lookup].[Group] ([Id], [Description], [Type])
    VALUES (1, N''Producer Type'', N''ProducerType''),
    (2, N''Compliance Scheme'', N''ComplianceScheme''),
    (3, N''Producer Subsidiaries'', N''ProducerSubsidiaries''),
    (4, N''Compliance Scheme Subsidiaries'', N''ComplianceSchemeSubsidiaries'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Type') AND [object_id] = OBJECT_ID(N'[Lookup].[Group]'))
        SET IDENTITY_INSERT [Lookup].[Group] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240725154904_RegistrationFeesTablesUpdate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Type') AND [object_id] = OBJECT_ID(N'[Lookup].[Regulator]'))
        SET IDENTITY_INSERT [Lookup].[Regulator] ON;
    EXEC(N'INSERT INTO [Lookup].[Regulator] ([Id], [Description], [Type])
    VALUES (1, N''England'', N''GB-ENG''),
    (2, N''Scotland'', N''GB-SCT''),
    (3, N''Wales'', N''GB-WLS''),
    (4, N''Northern Ireland'', N''GB-NIR'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Type') AND [object_id] = OBJECT_ID(N'[Lookup].[Regulator]'))
        SET IDENTITY_INSERT [Lookup].[Regulator] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240725154904_RegistrationFeesTablesUpdate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Type') AND [object_id] = OBJECT_ID(N'[Lookup].[SubGroup]'))
        SET IDENTITY_INSERT [Lookup].[SubGroup] ON;
    EXEC(N'INSERT INTO [Lookup].[SubGroup] ([Id], [Description], [Type])
    VALUES (1, N''Large producer'', N''Large''),
    (2, N''Small producer'', N''Small''),
    (3, N''Registration'', N''Registration''),
    (4, N''Online Market'', N''Online''),
    (5, N''Up to 20'', N''UpTo20''),
    (6, N''More than 20'', N''MoreThan20'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Type') AND [object_id] = OBJECT_ID(N'[Lookup].[SubGroup]'))
        SET IDENTITY_INSERT [Lookup].[SubGroup] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240725154904_RegistrationFeesTablesUpdate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Amount', N'EffectiveFrom', N'EffectiveTo', N'GroupId', N'RegulatorId', N'SubGroupId') AND [object_id] = OBJECT_ID(N'[Lookup].[RegistrationFees]'))
        SET IDENTITY_INSERT [Lookup].[RegistrationFees] ON;
    EXEC(N'INSERT INTO [Lookup].[RegistrationFees] ([Id], [Amount], [EffectiveFrom], [EffectiveTo], [GroupId], [RegulatorId], [SubGroupId])
    VALUES (1, 2620.0, ''2025-01-01T00:00:00.0000000'', ''2025-12-31T00:00:00.0000000'', 1, 1, 1),
    (2, 2620.0, ''2025-01-01T00:00:00.0000000'', ''2025-12-31T00:00:00.0000000'', 1, 2, 1),
    (3, 2620.0, ''2025-01-01T00:00:00.0000000'', ''2025-12-31T00:00:00.0000000'', 1, 3, 1),
    (4, 2620.0, ''2025-01-01T00:00:00.0000000'', ''2025-12-31T00:00:00.0000000'', 1, 4, 1),
    (5, 1216.0, ''2025-01-01T00:00:00.0000000'', ''2025-12-31T00:00:00.0000000'', 1, 1, 2),
    (6, 1216.0, ''2025-01-01T00:00:00.0000000'', ''2025-12-31T00:00:00.0000000'', 1, 2, 2),
    (7, 1216.0, ''2025-01-01T00:00:00.0000000'', ''2025-12-31T00:00:00.0000000'', 1, 3, 2),
    (8, 1216.0, ''2025-01-01T00:00:00.0000000'', ''2025-12-31T00:00:00.0000000'', 1, 4, 2),
    (9, 1658.0, ''2025-01-01T00:00:00.0000000'', ''2025-12-31T00:00:00.0000000'', 2, 1, 1),
    (10, 1658.0, ''2025-01-01T00:00:00.0000000'', ''2025-12-31T00:00:00.0000000'', 2, 2, 1),
    (11, 1658.0, ''2025-01-01T00:00:00.0000000'', ''2025-12-31T00:00:00.0000000'', 2, 3, 1),
    (12, 1658.0, ''2025-01-01T00:00:00.0000000'', ''2025-12-31T00:00:00.0000000'', 2, 4, 1),
    (13, 631.0, ''2025-01-01T00:00:00.0000000'', ''2025-12-31T00:00:00.0000000'', 2, 1, 2),
    (14, 631.0, ''2025-01-01T00:00:00.0000000'', ''2025-12-31T00:00:00.0000000'', 2, 2, 2),
    (15, 631.0, ''2025-01-01T00:00:00.0000000'', ''2025-12-31T00:00:00.0000000'', 2, 3, 2),
    (16, 631.0, ''2025-01-01T00:00:00.0000000'', ''2025-12-31T00:00:00.0000000'', 2, 4, 2),
    (17, 13804.0, ''2025-01-01T00:00:00.0000000'', ''2025-12-31T00:00:00.0000000'', 2, 1, 3),
    (18, 13804.0, ''2025-01-01T00:00:00.0000000'', ''2025-12-31T00:00:00.0000000'', 2, 2, 3),
    (19, 13804.0, ''2025-01-01T00:00:00.0000000'', ''2025-12-31T00:00:00.0000000'', 2, 3, 3),
    (20, 13804.0, ''2025-01-01T00:00:00.0000000'', ''2025-12-31T00:00:00.0000000'', 2, 4, 3),
    (21, 2579.0, ''2025-01-01T00:00:00.0000000'', ''2025-12-31T00:00:00.0000000'', 2, 1, 4),
    (22, 2579.0, ''2025-01-01T00:00:00.0000000'', ''2025-12-31T00:00:00.0000000'', 2, 2, 4),
    (23, 2579.0, ''2025-01-01T00:00:00.0000000'', ''2025-12-31T00:00:00.0000000'', 2, 3, 4),
    (24, 2579.0, ''2025-01-01T00:00:00.0000000'', ''2025-12-31T00:00:00.0000000'', 2, 4, 4),
    (25, 558.0, ''2025-01-01T00:00:00.0000000'', ''2025-12-31T00:00:00.0000000'', 3, 1, 5),
    (26, 558.0, ''2025-01-01T00:00:00.0000000'', ''2025-12-31T00:00:00.0000000'', 3, 2, 5),
    (27, 558.0, ''2025-01-01T00:00:00.0000000'', ''2025-12-31T00:00:00.0000000'', 3, 3, 5),
    (28, 558.0, ''2025-01-01T00:00:00.0000000'', ''2025-12-31T00:00:00.0000000'', 3, 4, 5),
    (29, 140.0, ''2025-01-01T00:00:00.0000000'', ''2025-12-31T00:00:00.0000000'', 3, 1, 6),
    (30, 140.0, ''2025-01-01T00:00:00.0000000'', ''2025-12-31T00:00:00.0000000'', 3, 2, 6),
    (31, 140.0, ''2025-01-01T00:00:00.0000000'', ''2025-12-31T00:00:00.0000000'', 3, 3, 6),
    (32, 140.0, ''2025-01-01T00:00:00.0000000'', ''2025-12-31T00:00:00.0000000'', 3, 4, 6),
    (33, 558.0, ''2025-01-01T00:00:00.0000000'', ''2025-12-31T00:00:00.0000000'', 4, 1, 5),
    (34, 558.0, ''2025-01-01T00:00:00.0000000'', ''2025-12-31T00:00:00.0000000'', 4, 2, 5),
    (35, 558.0, ''2025-01-01T00:00:00.0000000'', ''2025-12-31T00:00:00.0000000'', 4, 3, 5),
    (36, 558.0, ''2025-01-01T00:00:00.0000000'', ''2025-12-31T00:00:00.0000000'', 4, 4, 5),
    (37, 140.0, ''2025-01-01T00:00:00.0000000'', ''2025-12-31T00:00:00.0000000'', 4, 1, 6),
    (38, 140.0, ''2025-01-01T00:00:00.0000000'', ''2025-12-31T00:00:00.0000000'', 4, 2, 6),
    (39, 140.0, ''2025-01-01T00:00:00.0000000'', ''2025-12-31T00:00:00.0000000'', 4, 3, 6),
    (40, 140.0, ''2025-01-01T00:00:00.0000000'', ''2025-12-31T00:00:00.0000000'', 4, 4, 6)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Amount', N'EffectiveFrom', N'EffectiveTo', N'GroupId', N'RegulatorId', N'SubGroupId') AND [object_id] = OBJECT_ID(N'[Lookup].[RegistrationFees]'))
        SET IDENTITY_INSERT [Lookup].[RegistrationFees] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240725154904_RegistrationFeesTablesUpdate'
)
BEGIN
    CREATE INDEX [IX_RegistrationFees_GroupId] ON [Lookup].[RegistrationFees] ([GroupId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240725154904_RegistrationFeesTablesUpdate'
)
BEGIN
    CREATE INDEX [IX_RegistrationFees_RegulatorId] ON [Lookup].[RegistrationFees] ([RegulatorId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240725154904_RegistrationFeesTablesUpdate'
)
BEGIN
    CREATE INDEX [IX_RegistrationFees_SubGroupId] ON [Lookup].[RegistrationFees] ([SubGroupId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240725154904_RegistrationFeesTablesUpdate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240725154904_RegistrationFeesTablesUpdate', N'8.0.4');
END;
GO

COMMIT;
GO

