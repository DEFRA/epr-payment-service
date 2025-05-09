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

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240821075038_RemoveTable'
)
BEGIN
    DROP TABLE [Payment];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240821075038_RemoveTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240821075038_RemoveTable', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240821075639_ChangeColumnType'
)
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Lookup].[SubGroup]') AND [c].[name] = N'Type');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Lookup].[SubGroup] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [Lookup].[SubGroup] ALTER COLUMN [Type] varchar(50) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240821075639_ChangeColumnType'
)
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Lookup].[SubGroup]') AND [c].[name] = N'Description');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Lookup].[SubGroup] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [Lookup].[SubGroup] ALTER COLUMN [Description] varchar(255) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240821075639_ChangeColumnType'
)
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Lookup].[Regulator]') AND [c].[name] = N'Type');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Lookup].[Regulator] DROP CONSTRAINT [' + @var2 + '];');
    ALTER TABLE [Lookup].[Regulator] ALTER COLUMN [Type] varchar(50) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240821075639_ChangeColumnType'
)
BEGIN
    DECLARE @var3 sysname;
    SELECT @var3 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Lookup].[Regulator]') AND [c].[name] = N'Description');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Lookup].[Regulator] DROP CONSTRAINT [' + @var3 + '];');
    ALTER TABLE [Lookup].[Regulator] ALTER COLUMN [Description] varchar(255) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240821075639_ChangeColumnType'
)
BEGIN
    DECLARE @var4 sysname;
    SELECT @var4 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Lookup].[PaymentStatus]') AND [c].[name] = N'Status');
    IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Lookup].[PaymentStatus] DROP CONSTRAINT [' + @var4 + '];');
    ALTER TABLE [Lookup].[PaymentStatus] ALTER COLUMN [Status] varchar(20) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240821075639_ChangeColumnType'
)
BEGIN
    DECLARE @var5 sysname;
    SELECT @var5 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Lookup].[Group]') AND [c].[name] = N'Type');
    IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [Lookup].[Group] DROP CONSTRAINT [' + @var5 + '];');
    ALTER TABLE [Lookup].[Group] ALTER COLUMN [Type] varchar(50) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240821075639_ChangeColumnType'
)
BEGIN
    DECLARE @var6 sysname;
    SELECT @var6 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Lookup].[Group]') AND [c].[name] = N'Description');
    IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [Lookup].[Group] DROP CONSTRAINT [' + @var6 + '];');
    ALTER TABLE [Lookup].[Group] ALTER COLUMN [Description] varchar(255) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240821075639_ChangeColumnType'
)
BEGIN
    CREATE TABLE [Payment] (
        [Id] int NOT NULL IDENTITY,
        [UserId] uniqueidentifier NOT NULL,
        [OrganisationId] uniqueidentifier NOT NULL,
        [ExternalPaymentId] uniqueidentifier NOT NULL DEFAULT (NEWID()),
        [GovpayPaymentId] varchar(50) NULL,
        [InternalStatusId] int NOT NULL,
        [Regulator] varchar(20) NOT NULL,
        [GovPayStatus] varchar(20) NULL,
        [ErrorCode] varchar(255) NULL,
        [ErrorMessage] varchar(255) NULL,
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
    WHERE [MigrationId] = N'20240821075639_ChangeColumnType'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Payment_ExternalPaymentId] ON [Payment] ([ExternalPaymentId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240821075639_ChangeColumnType'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Payment_GovpayPaymentId] ON [Payment] ([GovpayPaymentId]) WHERE [GovpayPaymentId] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240821075639_ChangeColumnType'
)
BEGIN
    CREATE INDEX [IX_Payment_InternalStatusId] ON [Payment] ([InternalStatusId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240821075639_ChangeColumnType'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240821075639_ChangeColumnType', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240822124821_ProducerResubmission'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Type') AND [object_id] = OBJECT_ID(N'[Lookup].[Group]'))
        SET IDENTITY_INSERT [Lookup].[Group] ON;
    EXEC(N'INSERT INTO [Lookup].[Group] ([Id], [Description], [Type])
    VALUES (5, ''Producer re-submitting a report'', ''ProducerResubmission'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Type') AND [object_id] = OBJECT_ID(N'[Lookup].[Group]'))
        SET IDENTITY_INSERT [Lookup].[Group] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240822124821_ProducerResubmission'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 262000.0
    WHERE [Id] = 1;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240822124821_ProducerResubmission'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 262000.0
    WHERE [Id] = 2;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240822124821_ProducerResubmission'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 262000.0
    WHERE [Id] = 3;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240822124821_ProducerResubmission'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 262000.0
    WHERE [Id] = 4;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240822124821_ProducerResubmission'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 121600.0
    WHERE [Id] = 5;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240822124821_ProducerResubmission'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 121600.0
    WHERE [Id] = 6;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240822124821_ProducerResubmission'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 121600.0
    WHERE [Id] = 7;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240822124821_ProducerResubmission'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 121600.0
    WHERE [Id] = 8;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240822124821_ProducerResubmission'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 165800.0
    WHERE [Id] = 9;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240822124821_ProducerResubmission'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 165800.0
    WHERE [Id] = 10;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240822124821_ProducerResubmission'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 165800.0
    WHERE [Id] = 11;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240822124821_ProducerResubmission'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 165800.0
    WHERE [Id] = 12;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240822124821_ProducerResubmission'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 63100.0
    WHERE [Id] = 13;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240822124821_ProducerResubmission'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 63100.0
    WHERE [Id] = 14;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240822124821_ProducerResubmission'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 63100.0
    WHERE [Id] = 15;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240822124821_ProducerResubmission'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 63100.0
    WHERE [Id] = 16;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240822124821_ProducerResubmission'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 1380400.0
    WHERE [Id] = 17;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240822124821_ProducerResubmission'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 1380400.0
    WHERE [Id] = 18;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240822124821_ProducerResubmission'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 1380400.0
    WHERE [Id] = 19;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240822124821_ProducerResubmission'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 1380400.0
    WHERE [Id] = 20;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240822124821_ProducerResubmission'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 257900.0
    WHERE [Id] = 21;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240822124821_ProducerResubmission'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 257900.0
    WHERE [Id] = 22;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240822124821_ProducerResubmission'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 257900.0
    WHERE [Id] = 23;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240822124821_ProducerResubmission'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 257900.0
    WHERE [Id] = 24;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240822124821_ProducerResubmission'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 55800.0
    WHERE [Id] = 25;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240822124821_ProducerResubmission'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 55800.0
    WHERE [Id] = 26;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240822124821_ProducerResubmission'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 55800.0
    WHERE [Id] = 27;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240822124821_ProducerResubmission'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 55800.0
    WHERE [Id] = 28;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240822124821_ProducerResubmission'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 14000.0
    WHERE [Id] = 29;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240822124821_ProducerResubmission'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 14000.0
    WHERE [Id] = 30;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240822124821_ProducerResubmission'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 14000.0
    WHERE [Id] = 31;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240822124821_ProducerResubmission'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 14000.0
    WHERE [Id] = 32;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240822124821_ProducerResubmission'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 55800.0
    WHERE [Id] = 33;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240822124821_ProducerResubmission'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 55800.0
    WHERE [Id] = 34;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240822124821_ProducerResubmission'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 55800.0
    WHERE [Id] = 35;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240822124821_ProducerResubmission'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 55800.0
    WHERE [Id] = 36;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240822124821_ProducerResubmission'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 14000.0
    WHERE [Id] = 37;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240822124821_ProducerResubmission'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 14000.0
    WHERE [Id] = 38;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240822124821_ProducerResubmission'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 14000.0
    WHERE [Id] = 39;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240822124821_ProducerResubmission'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 14000.0
    WHERE [Id] = 40;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240822124821_ProducerResubmission'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Type') AND [object_id] = OBJECT_ID(N'[Lookup].[SubGroup]'))
        SET IDENTITY_INSERT [Lookup].[SubGroup] ON;
    EXEC(N'INSERT INTO [Lookup].[SubGroup] ([Id], [Description], [Type])
    VALUES (7, ''Re-submitting a report'', ''ReSubmitting'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Type') AND [object_id] = OBJECT_ID(N'[Lookup].[SubGroup]'))
        SET IDENTITY_INSERT [Lookup].[SubGroup] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240822124821_ProducerResubmission'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Amount', N'EffectiveFrom', N'EffectiveTo', N'GroupId', N'RegulatorId', N'SubGroupId') AND [object_id] = OBJECT_ID(N'[Lookup].[RegistrationFees]'))
        SET IDENTITY_INSERT [Lookup].[RegistrationFees] ON;
    EXEC(N'INSERT INTO [Lookup].[RegistrationFees] ([Id], [Amount], [EffectiveFrom], [EffectiveTo], [GroupId], [RegulatorId], [SubGroupId])
    VALUES (41, 71400.0, ''2025-01-01T00:00:00.0000000'', ''2025-12-31T00:00:00.0000000'', 5, 1, 7),
    (42, 71400.0, ''2025-01-01T00:00:00.0000000'', ''2025-12-31T00:00:00.0000000'', 5, 2, 7),
    (43, 71400.0, ''2025-01-01T00:00:00.0000000'', ''2025-12-31T00:00:00.0000000'', 5, 3, 7),
    (44, 71400.0, ''2025-01-01T00:00:00.0000000'', ''2025-12-31T00:00:00.0000000'', 5, 4, 7)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Amount', N'EffectiveFrom', N'EffectiveTo', N'GroupId', N'RegulatorId', N'SubGroupId') AND [object_id] = OBJECT_ID(N'[Lookup].[RegistrationFees]'))
        SET IDENTITY_INSERT [Lookup].[RegistrationFees] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240822124821_ProducerResubmission'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240822124821_ProducerResubmission', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveFrom] = ''2024-01-01T00:00:00.0000000Z'', [EffectiveTo] = ''2025-12-31T23:59:59.0000000Z''
    WHERE [Id] = 1;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveFrom] = ''2024-01-01T00:00:00.0000000Z'', [EffectiveTo] = ''2025-12-31T23:59:59.0000000Z''
    WHERE [Id] = 2;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveFrom] = ''2024-01-01T00:00:00.0000000Z'', [EffectiveTo] = ''2025-12-31T23:59:59.0000000Z''
    WHERE [Id] = 3;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveFrom] = ''2024-01-01T00:00:00.0000000Z'', [EffectiveTo] = ''2025-12-31T23:59:59.0000000Z''
    WHERE [Id] = 4;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveFrom] = ''2024-01-01T00:00:00.0000000Z'', [EffectiveTo] = ''2025-12-31T23:59:59.0000000Z''
    WHERE [Id] = 5;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveFrom] = ''2024-01-01T00:00:00.0000000Z'', [EffectiveTo] = ''2025-12-31T23:59:59.0000000Z''
    WHERE [Id] = 6;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveFrom] = ''2024-01-01T00:00:00.0000000Z'', [EffectiveTo] = ''2025-12-31T23:59:59.0000000Z''
    WHERE [Id] = 7;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveFrom] = ''2024-01-01T00:00:00.0000000Z'', [EffectiveTo] = ''2025-12-31T23:59:59.0000000Z''
    WHERE [Id] = 8;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveFrom] = ''2024-01-01T00:00:00.0000000Z'', [EffectiveTo] = ''2025-12-31T23:59:59.0000000Z''
    WHERE [Id] = 9;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveFrom] = ''2024-01-01T00:00:00.0000000Z'', [EffectiveTo] = ''2025-12-31T23:59:59.0000000Z''
    WHERE [Id] = 10;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveFrom] = ''2024-01-01T00:00:00.0000000Z'', [EffectiveTo] = ''2025-12-31T23:59:59.0000000Z''
    WHERE [Id] = 11;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveFrom] = ''2024-01-01T00:00:00.0000000Z'', [EffectiveTo] = ''2025-12-31T23:59:59.0000000Z''
    WHERE [Id] = 12;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveFrom] = ''2024-01-01T00:00:00.0000000Z'', [EffectiveTo] = ''2025-12-31T23:59:59.0000000Z''
    WHERE [Id] = 13;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveFrom] = ''2024-01-01T00:00:00.0000000Z'', [EffectiveTo] = ''2025-12-31T23:59:59.0000000Z''
    WHERE [Id] = 14;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveFrom] = ''2024-01-01T00:00:00.0000000Z'', [EffectiveTo] = ''2025-12-31T23:59:59.0000000Z''
    WHERE [Id] = 15;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveFrom] = ''2024-01-01T00:00:00.0000000Z'', [EffectiveTo] = ''2025-12-31T23:59:59.0000000Z''
    WHERE [Id] = 16;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveFrom] = ''2024-01-01T00:00:00.0000000Z'', [EffectiveTo] = ''2025-12-31T23:59:59.0000000Z''
    WHERE [Id] = 17;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveFrom] = ''2024-01-01T00:00:00.0000000Z'', [EffectiveTo] = ''2025-12-31T23:59:59.0000000Z''
    WHERE [Id] = 18;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveFrom] = ''2024-01-01T00:00:00.0000000Z'', [EffectiveTo] = ''2025-12-31T23:59:59.0000000Z''
    WHERE [Id] = 19;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveFrom] = ''2024-01-01T00:00:00.0000000Z'', [EffectiveTo] = ''2025-12-31T23:59:59.0000000Z''
    WHERE [Id] = 20;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveFrom] = ''2024-01-01T00:00:00.0000000Z'', [EffectiveTo] = ''2025-12-31T23:59:59.0000000Z''
    WHERE [Id] = 21;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveFrom] = ''2024-01-01T00:00:00.0000000Z'', [EffectiveTo] = ''2025-12-31T23:59:59.0000000Z''
    WHERE [Id] = 22;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveFrom] = ''2024-01-01T00:00:00.0000000Z'', [EffectiveTo] = ''2025-12-31T23:59:59.0000000Z''
    WHERE [Id] = 23;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveFrom] = ''2024-01-01T00:00:00.0000000Z'', [EffectiveTo] = ''2025-12-31T23:59:59.0000000Z''
    WHERE [Id] = 24;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveFrom] = ''2024-01-01T00:00:00.0000000Z'', [EffectiveTo] = ''2025-12-31T23:59:59.0000000Z''
    WHERE [Id] = 25;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveFrom] = ''2024-01-01T00:00:00.0000000Z'', [EffectiveTo] = ''2025-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveFrom] = ''2024-01-01T00:00:00.0000000Z'', [EffectiveTo] = ''2025-12-31T23:59:59.0000000Z''
    WHERE [Id] = 27;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveFrom] = ''2024-01-01T00:00:00.0000000Z'', [EffectiveTo] = ''2025-12-31T23:59:59.0000000Z''
    WHERE [Id] = 28;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveFrom] = ''2024-01-01T00:00:00.0000000Z'', [EffectiveTo] = ''2025-12-31T23:59:59.0000000Z''
    WHERE [Id] = 29;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveFrom] = ''2024-01-01T00:00:00.0000000Z'', [EffectiveTo] = ''2025-12-31T23:59:59.0000000Z''
    WHERE [Id] = 30;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveFrom] = ''2024-01-01T00:00:00.0000000Z'', [EffectiveTo] = ''2025-12-31T23:59:59.0000000Z''
    WHERE [Id] = 31;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveFrom] = ''2024-01-01T00:00:00.0000000Z'', [EffectiveTo] = ''2025-12-31T23:59:59.0000000Z''
    WHERE [Id] = 32;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveFrom] = ''2024-01-01T00:00:00.0000000Z'', [EffectiveTo] = ''2025-12-31T23:59:59.0000000Z''
    WHERE [Id] = 33;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveFrom] = ''2024-01-01T00:00:00.0000000Z'', [EffectiveTo] = ''2025-12-31T23:59:59.0000000Z''
    WHERE [Id] = 34;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveFrom] = ''2024-01-01T00:00:00.0000000Z'', [EffectiveTo] = ''2025-12-31T23:59:59.0000000Z''
    WHERE [Id] = 35;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveFrom] = ''2024-01-01T00:00:00.0000000Z'', [EffectiveTo] = ''2025-12-31T23:59:59.0000000Z''
    WHERE [Id] = 36;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveFrom] = ''2024-01-01T00:00:00.0000000Z'', [EffectiveTo] = ''2025-12-31T23:59:59.0000000Z''
    WHERE [Id] = 37;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveFrom] = ''2024-01-01T00:00:00.0000000Z'', [EffectiveTo] = ''2025-12-31T23:59:59.0000000Z''
    WHERE [Id] = 38;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveFrom] = ''2024-01-01T00:00:00.0000000Z'', [EffectiveTo] = ''2025-12-31T23:59:59.0000000Z''
    WHERE [Id] = 39;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveFrom] = ''2024-01-01T00:00:00.0000000Z'', [EffectiveTo] = ''2025-12-31T23:59:59.0000000Z''
    WHERE [Id] = 40;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveFrom] = ''2024-01-01T00:00:00.0000000Z'', [EffectiveTo] = ''2025-12-31T23:59:59.0000000Z''
    WHERE [Id] = 41;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveFrom] = ''2024-01-01T00:00:00.0000000Z'', [EffectiveTo] = ''2025-12-31T23:59:59.0000000Z''
    WHERE [Id] = 42;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveFrom] = ''2024-01-01T00:00:00.0000000Z'', [EffectiveTo] = ''2025-12-31T23:59:59.0000000Z''
    WHERE [Id] = 43;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveFrom] = ''2024-01-01T00:00:00.0000000Z'', [EffectiveTo] = ''2025-12-31T23:59:59.0000000Z''
    WHERE [Id] = 44;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240903101544_UpdateRegistrationFeesDates'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240903101544_UpdateRegistrationFeesDates', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240905115928_AddOnlineMarketToProducer'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 257900.0, [GroupId] = 1, [SubGroupId] = 4
    WHERE [Id] = 9;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240905115928_AddOnlineMarketToProducer'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 257900.0, [GroupId] = 1, [SubGroupId] = 4
    WHERE [Id] = 10;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240905115928_AddOnlineMarketToProducer'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 257900.0, [GroupId] = 1, [SubGroupId] = 4
    WHERE [Id] = 11;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240905115928_AddOnlineMarketToProducer'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 257900.0, [GroupId] = 1, [SubGroupId] = 4
    WHERE [Id] = 12;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240905115928_AddOnlineMarketToProducer'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 165800.0, [SubGroupId] = 1
    WHERE [Id] = 13;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240905115928_AddOnlineMarketToProducer'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 165800.0, [SubGroupId] = 1
    WHERE [Id] = 14;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240905115928_AddOnlineMarketToProducer'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 165800.0, [SubGroupId] = 1
    WHERE [Id] = 15;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240905115928_AddOnlineMarketToProducer'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 165800.0, [SubGroupId] = 1
    WHERE [Id] = 16;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240905115928_AddOnlineMarketToProducer'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 63100.0, [SubGroupId] = 2
    WHERE [Id] = 17;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240905115928_AddOnlineMarketToProducer'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 63100.0, [SubGroupId] = 2
    WHERE [Id] = 18;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240905115928_AddOnlineMarketToProducer'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 63100.0, [SubGroupId] = 2
    WHERE [Id] = 19;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240905115928_AddOnlineMarketToProducer'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 63100.0, [SubGroupId] = 2
    WHERE [Id] = 20;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240905115928_AddOnlineMarketToProducer'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 1380400.0, [SubGroupId] = 3
    WHERE [Id] = 21;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240905115928_AddOnlineMarketToProducer'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 1380400.0, [SubGroupId] = 3
    WHERE [Id] = 22;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240905115928_AddOnlineMarketToProducer'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 1380400.0, [SubGroupId] = 3
    WHERE [Id] = 23;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240905115928_AddOnlineMarketToProducer'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 1380400.0, [SubGroupId] = 3
    WHERE [Id] = 24;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240905115928_AddOnlineMarketToProducer'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 257900.0, [GroupId] = 2, [SubGroupId] = 4
    WHERE [Id] = 25;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240905115928_AddOnlineMarketToProducer'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 257900.0, [GroupId] = 2, [SubGroupId] = 4
    WHERE [Id] = 26;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240905115928_AddOnlineMarketToProducer'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 257900.0, [GroupId] = 2, [SubGroupId] = 4
    WHERE [Id] = 27;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240905115928_AddOnlineMarketToProducer'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 257900.0, [GroupId] = 2, [SubGroupId] = 4
    WHERE [Id] = 28;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240905115928_AddOnlineMarketToProducer'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 55800.0, [SubGroupId] = 5
    WHERE [Id] = 29;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240905115928_AddOnlineMarketToProducer'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 55800.0, [SubGroupId] = 5
    WHERE [Id] = 30;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240905115928_AddOnlineMarketToProducer'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 55800.0, [SubGroupId] = 5
    WHERE [Id] = 31;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240905115928_AddOnlineMarketToProducer'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 55800.0, [SubGroupId] = 5
    WHERE [Id] = 32;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240905115928_AddOnlineMarketToProducer'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 14000.0, [GroupId] = 3, [SubGroupId] = 6
    WHERE [Id] = 33;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240905115928_AddOnlineMarketToProducer'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 14000.0, [GroupId] = 3, [SubGroupId] = 6
    WHERE [Id] = 34;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240905115928_AddOnlineMarketToProducer'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 14000.0, [GroupId] = 3, [SubGroupId] = 6
    WHERE [Id] = 35;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240905115928_AddOnlineMarketToProducer'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 14000.0, [GroupId] = 3, [SubGroupId] = 6
    WHERE [Id] = 36;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240905115928_AddOnlineMarketToProducer'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 55800.0, [SubGroupId] = 5
    WHERE [Id] = 37;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240905115928_AddOnlineMarketToProducer'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 55800.0, [SubGroupId] = 5
    WHERE [Id] = 38;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240905115928_AddOnlineMarketToProducer'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 55800.0, [SubGroupId] = 5
    WHERE [Id] = 39;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240905115928_AddOnlineMarketToProducer'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 55800.0, [SubGroupId] = 5
    WHERE [Id] = 40;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240905115928_AddOnlineMarketToProducer'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 14000.0, [GroupId] = 4, [SubGroupId] = 6
    WHERE [Id] = 41;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240905115928_AddOnlineMarketToProducer'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 14000.0, [GroupId] = 4, [SubGroupId] = 6
    WHERE [Id] = 42;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240905115928_AddOnlineMarketToProducer'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 14000.0, [GroupId] = 4, [SubGroupId] = 6
    WHERE [Id] = 43;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240905115928_AddOnlineMarketToProducer'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 14000.0, [GroupId] = 4, [SubGroupId] = 6
    WHERE [Id] = 44;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240905115928_AddOnlineMarketToProducer'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Amount', N'EffectiveFrom', N'EffectiveTo', N'GroupId', N'RegulatorId', N'SubGroupId') AND [object_id] = OBJECT_ID(N'[Lookup].[RegistrationFees]'))
        SET IDENTITY_INSERT [Lookup].[RegistrationFees] ON;
    EXEC(N'INSERT INTO [Lookup].[RegistrationFees] ([Id], [Amount], [EffectiveFrom], [EffectiveTo], [GroupId], [RegulatorId], [SubGroupId])
    VALUES (45, 71400.0, ''2024-01-01T00:00:00.0000000Z'', ''2025-12-31T23:59:59.0000000Z'', 5, 1, 7),
    (46, 71400.0, ''2024-01-01T00:00:00.0000000Z'', ''2025-12-31T23:59:59.0000000Z'', 5, 2, 7),
    (47, 71400.0, ''2024-01-01T00:00:00.0000000Z'', ''2025-12-31T23:59:59.0000000Z'', 5, 3, 7),
    (48, 71400.0, ''2024-01-01T00:00:00.0000000Z'', ''2025-12-31T23:59:59.0000000Z'', 5, 4, 7)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Amount', N'EffectiveFrom', N'EffectiveTo', N'GroupId', N'RegulatorId', N'SubGroupId') AND [object_id] = OBJECT_ID(N'[Lookup].[RegistrationFees]'))
        SET IDENTITY_INSERT [Lookup].[RegistrationFees] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240905115928_AddOnlineMarketToProducer'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240905115928_AddOnlineMarketToProducer', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241011074943_LateFeeData'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Type') AND [object_id] = OBJECT_ID(N'[Lookup].[SubGroup]'))
        SET IDENTITY_INSERT [Lookup].[SubGroup] ON;
    EXEC(N'INSERT INTO [Lookup].[SubGroup] ([Id], [Description], [Type])
    VALUES (8, ''Late Fee'', ''LateFee'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Type') AND [object_id] = OBJECT_ID(N'[Lookup].[SubGroup]'))
        SET IDENTITY_INSERT [Lookup].[SubGroup] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241011074943_LateFeeData'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Amount', N'EffectiveFrom', N'EffectiveTo', N'GroupId', N'RegulatorId', N'SubGroupId') AND [object_id] = OBJECT_ID(N'[Lookup].[RegistrationFees]'))
        SET IDENTITY_INSERT [Lookup].[RegistrationFees] ON;
    EXEC(N'INSERT INTO [Lookup].[RegistrationFees] ([Id], [Amount], [EffectiveFrom], [EffectiveTo], [GroupId], [RegulatorId], [SubGroupId])
    VALUES (49, 33200.0, ''2024-01-01T00:00:00.0000000Z'', ''2025-12-31T23:59:59.0000000Z'', 1, 1, 8),
    (50, 33200.0, ''2024-01-01T00:00:00.0000000Z'', ''2025-12-31T23:59:59.0000000Z'', 1, 2, 8),
    (51, 33200.0, ''2024-01-01T00:00:00.0000000Z'', ''2025-12-31T23:59:59.0000000Z'', 1, 3, 8),
    (52, 33200.0, ''2024-01-01T00:00:00.0000000Z'', ''2025-12-31T23:59:59.0000000Z'', 1, 4, 8)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Amount', N'EffectiveFrom', N'EffectiveTo', N'GroupId', N'RegulatorId', N'SubGroupId') AND [object_id] = OBJECT_ID(N'[Lookup].[RegistrationFees]'))
        SET IDENTITY_INSERT [Lookup].[RegistrationFees] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241011074943_LateFeeData'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241011074943_LateFeeData', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241016152809_ComplianceLateFeeData'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Amount', N'EffectiveFrom', N'EffectiveTo', N'GroupId', N'RegulatorId', N'SubGroupId') AND [object_id] = OBJECT_ID(N'[Lookup].[RegistrationFees]'))
        SET IDENTITY_INSERT [Lookup].[RegistrationFees] ON;
    EXEC(N'INSERT INTO [Lookup].[RegistrationFees] ([Id], [Amount], [EffectiveFrom], [EffectiveTo], [GroupId], [RegulatorId], [SubGroupId])
    VALUES (53, 33200.0, ''2024-01-01T00:00:00.0000000Z'', ''2025-12-31T23:59:59.0000000Z'', 2, 1, 8),
    (54, 33200.0, ''2024-01-01T00:00:00.0000000Z'', ''2025-12-31T23:59:59.0000000Z'', 2, 2, 8),
    (55, 33200.0, ''2024-01-01T00:00:00.0000000Z'', ''2025-12-31T23:59:59.0000000Z'', 2, 3, 8),
    (56, 33200.0, ''2024-01-01T00:00:00.0000000Z'', ''2025-12-31T23:59:59.0000000Z'', 2, 4, 8)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Amount', N'EffectiveFrom', N'EffectiveTo', N'GroupId', N'RegulatorId', N'SubGroupId') AND [object_id] = OBJECT_ID(N'[Lookup].[RegistrationFees]'))
        SET IDENTITY_INSERT [Lookup].[RegistrationFees] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241016152809_ComplianceLateFeeData'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241016152809_ComplianceLateFeeData', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241018135341_ComplianceSchemeResubmission'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Type') AND [object_id] = OBJECT_ID(N'[Lookup].[Group]'))
        SET IDENTITY_INSERT [Lookup].[Group] ON;
    EXEC(N'INSERT INTO [Lookup].[Group] ([Id], [Description], [Type])
    VALUES (6, ''Compliance Scheme re-submitting a report'', ''ComplianceSchemeResubmission'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Type') AND [object_id] = OBJECT_ID(N'[Lookup].[Group]'))
        SET IDENTITY_INSERT [Lookup].[Group] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241018135341_ComplianceSchemeResubmission'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Amount', N'EffectiveFrom', N'EffectiveTo', N'GroupId', N'RegulatorId', N'SubGroupId') AND [object_id] = OBJECT_ID(N'[Lookup].[RegistrationFees]'))
        SET IDENTITY_INSERT [Lookup].[RegistrationFees] ON;
    EXEC(N'INSERT INTO [Lookup].[RegistrationFees] ([Id], [Amount], [EffectiveFrom], [EffectiveTo], [GroupId], [RegulatorId], [SubGroupId])
    VALUES (57, 43000.0, ''2024-01-01T00:00:00.0000000Z'', ''2025-12-31T23:59:59.0000000Z'', 6, 1, 7),
    (58, 43000.0, ''2024-01-01T00:00:00.0000000Z'', ''2025-12-31T23:59:59.0000000Z'', 6, 2, 7),
    (59, 43000.0, ''2024-01-01T00:00:00.0000000Z'', ''2025-12-31T23:59:59.0000000Z'', 6, 3, 7),
    (60, 43000.0, ''2024-01-01T00:00:00.0000000Z'', ''2025-12-31T23:59:59.0000000Z'', 6, 4, 7)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Amount', N'EffectiveFrom', N'EffectiveTo', N'GroupId', N'RegulatorId', N'SubGroupId') AND [object_id] = OBJECT_ID(N'[Lookup].[RegistrationFees]'))
        SET IDENTITY_INSERT [Lookup].[RegistrationFees] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241018135341_ComplianceSchemeResubmission'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241018135341_ComplianceSchemeResubmission', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241028134619_RefactorPaymentTables'
)
BEGIN
    DROP INDEX [IX_Payment_GovpayPaymentId] ON [Payment];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241028134619_RefactorPaymentTables'
)
BEGIN
    DECLARE @var7 sysname;
    SELECT @var7 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Payment]') AND [c].[name] = N'ErrorCode');
    IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [Payment] DROP CONSTRAINT [' + @var7 + '];');
    ALTER TABLE [Payment] DROP COLUMN [ErrorCode];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241028134619_RefactorPaymentTables'
)
BEGIN
    DECLARE @var8 sysname;
    SELECT @var8 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Payment]') AND [c].[name] = N'ErrorMessage');
    IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [Payment] DROP CONSTRAINT [' + @var8 + '];');
    ALTER TABLE [Payment] DROP COLUMN [ErrorMessage];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241028134619_RefactorPaymentTables'
)
BEGIN
    DECLARE @var9 sysname;
    SELECT @var9 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Payment]') AND [c].[name] = N'GovPayStatus');
    IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [Payment] DROP CONSTRAINT [' + @var9 + '];');
    ALTER TABLE [Payment] DROP COLUMN [GovPayStatus];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241028134619_RefactorPaymentTables'
)
BEGIN
    DECLARE @var10 sysname;
    SELECT @var10 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Payment]') AND [c].[name] = N'GovpayPaymentId');
    IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [Payment] DROP CONSTRAINT [' + @var10 + '];');
    ALTER TABLE [Payment] DROP COLUMN [GovpayPaymentId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241028134619_RefactorPaymentTables'
)
BEGIN
    DECLARE @var11 sysname;
    SELECT @var11 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Payment]') AND [c].[name] = N'OrganisationId');
    IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [Payment] DROP CONSTRAINT [' + @var11 + '];');
    ALTER TABLE [Payment] DROP COLUMN [OrganisationId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241028134619_RefactorPaymentTables'
)
BEGIN
    DECLARE @var12 sysname;
    SELECT @var12 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Payment]') AND [c].[name] = N'UpdatedByOrganisationId');
    IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [Payment] DROP CONSTRAINT [' + @var12 + '];');
    ALTER TABLE [Payment] DROP COLUMN [UpdatedByOrganisationId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241028134619_RefactorPaymentTables'
)
BEGIN
    DECLARE @var13 sysname;
    SELECT @var13 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Payment]') AND [c].[name] = N'UpdatedDate');
    IF @var13 IS NOT NULL EXEC(N'ALTER TABLE [Payment] DROP CONSTRAINT [' + @var13 + '];');
    ALTER TABLE [Payment] ALTER COLUMN [UpdatedDate] datetime2 NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241028134619_RefactorPaymentTables'
)
BEGIN
    DECLARE @var14 sysname;
    SELECT @var14 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Payment]') AND [c].[name] = N'UpdatedByUserId');
    IF @var14 IS NOT NULL EXEC(N'ALTER TABLE [Payment] DROP CONSTRAINT [' + @var14 + '];');
    ALTER TABLE [Payment] ALTER COLUMN [UpdatedByUserId] uniqueidentifier NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241028134619_RefactorPaymentTables'
)
BEGIN
    DECLARE @var15 sysname;
    SELECT @var15 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Payment]') AND [c].[name] = N'Regulator');
    IF @var15 IS NOT NULL EXEC(N'ALTER TABLE [Payment] DROP CONSTRAINT [' + @var15 + '];');
    ALTER TABLE [Payment] ALTER COLUMN [Regulator] varchar(20) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241028134619_RefactorPaymentTables'
)
BEGIN
    DECLARE @var16 sysname;
    SELECT @var16 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Payment]') AND [c].[name] = N'Reference');
    IF @var16 IS NOT NULL EXEC(N'ALTER TABLE [Payment] DROP CONSTRAINT [' + @var16 + '];');
    ALTER TABLE [Payment] ALTER COLUMN [Reference] nvarchar(255) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241028134619_RefactorPaymentTables'
)
BEGIN
    DECLARE @var17 sysname;
    SELECT @var17 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Payment]') AND [c].[name] = N'ReasonForPayment');
    IF @var17 IS NOT NULL EXEC(N'ALTER TABLE [Payment] DROP CONSTRAINT [' + @var17 + '];');
    ALTER TABLE [Payment] ALTER COLUMN [ReasonForPayment] nvarchar(255) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241028134619_RefactorPaymentTables'
)
BEGIN
    DECLARE @var18 sysname;
    SELECT @var18 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Payment]') AND [c].[name] = N'InternalStatusId');
    IF @var18 IS NOT NULL EXEC(N'ALTER TABLE [Payment] DROP CONSTRAINT [' + @var18 + '];');
    ALTER TABLE [Payment] ALTER COLUMN [InternalStatusId] int NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241028134619_RefactorPaymentTables'
)
BEGIN
    DECLARE @var19 sysname;
    SELECT @var19 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Payment]') AND [c].[name] = N'ExternalPaymentId');
    IF @var19 IS NOT NULL EXEC(N'ALTER TABLE [Payment] DROP CONSTRAINT [' + @var19 + '];');
    ALTER TABLE [Payment] ALTER COLUMN [ExternalPaymentId] uniqueidentifier NOT NULL;
    ALTER TABLE [Payment] ADD DEFAULT (NEWID()) FOR [ExternalPaymentId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241028134619_RefactorPaymentTables'
)
BEGIN
    DECLARE @var20 sysname;
    SELECT @var20 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Payment]') AND [c].[name] = N'CreatedDate');
    IF @var20 IS NOT NULL EXEC(N'ALTER TABLE [Payment] DROP CONSTRAINT [' + @var20 + '];');
    ALTER TABLE [Payment] ALTER COLUMN [CreatedDate] datetime2 NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241028134619_RefactorPaymentTables'
)
BEGIN
    DECLARE @var21 sysname;
    SELECT @var21 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Payment]') AND [c].[name] = N'Amount');
    IF @var21 IS NOT NULL EXEC(N'ALTER TABLE [Payment] DROP CONSTRAINT [' + @var21 + '];');
    ALTER TABLE [Payment] ALTER COLUMN [Amount] decimal(19,4) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241028134619_RefactorPaymentTables'
)
BEGIN
    CREATE TABLE [OfflinePayment] (
        [Id] int NOT NULL IDENTITY,
        [PaymentId] int NOT NULL,
        [PaymentDate] datetime2 NOT NULL,
        [Comments] nvarchar(255) NOT NULL,
        CONSTRAINT [PK_OfflinePayment] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_OfflinePayment_Payment_PaymentId] FOREIGN KEY ([PaymentId]) REFERENCES [Payment] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241028134619_RefactorPaymentTables'
)
BEGIN
    CREATE TABLE [OnlinePayment] (
        [Id] int NOT NULL IDENTITY,
        [PaymentId] int NOT NULL,
        [OrganisationId] uniqueidentifier NOT NULL,
        [GovPayPaymentId] varchar(50) NULL,
        [GovPayStatus] varchar(20) NULL,
        [ErrorCode] varchar(255) NULL,
        [ErrorMessage] varchar(255) NULL,
        [UpdatedByOrgId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_OnlinePayment] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_OnlinePayment_Payment_PaymentId] FOREIGN KEY ([PaymentId]) REFERENCES [Payment] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241028134619_RefactorPaymentTables'
)
BEGIN
    CREATE UNIQUE INDEX [IX_OfflinePayment_PaymentId] ON [OfflinePayment] ([PaymentId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241028134619_RefactorPaymentTables'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_OnlinePayment_GovPayPaymentId] ON [OnlinePayment] ([GovPayPaymentId]) WHERE [GovPayPaymentId] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241028134619_RefactorPaymentTables'
)
BEGIN
    CREATE UNIQUE INDEX [IX_OnlinePayment_PaymentId] ON [OnlinePayment] ([PaymentId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241028134619_RefactorPaymentTables'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241028134619_RefactorPaymentTables', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241108152208_CSUpdateLargeFee'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 168500.0
    WHERE [Id] = 13;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241108152208_CSUpdateLargeFee'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 168500.0
    WHERE [Id] = 14;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241108152208_CSUpdateLargeFee'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 168500.0
    WHERE [Id] = 15;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241108152208_CSUpdateLargeFee'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [Amount] = 168500.0
    WHERE [Id] = 16;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241108152208_CSUpdateLargeFee'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241108152208_CSUpdateLargeFee', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241114105610_ChangeNullableFields'
)
BEGIN
    DECLARE @var22 sysname;
    SELECT @var22 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[OfflinePayment]') AND [c].[name] = N'PaymentDate');
    IF @var22 IS NOT NULL EXEC(N'ALTER TABLE [OfflinePayment] DROP CONSTRAINT [' + @var22 + '];');
    ALTER TABLE [OfflinePayment] ALTER COLUMN [PaymentDate] datetime2 NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241114105610_ChangeNullableFields'
)
BEGIN
    DECLARE @var23 sysname;
    SELECT @var23 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[OfflinePayment]') AND [c].[name] = N'Comments');
    IF @var23 IS NOT NULL EXEC(N'ALTER TABLE [OfflinePayment] DROP CONSTRAINT [' + @var23 + '];');
    ALTER TABLE [OfflinePayment] ALTER COLUMN [Comments] nvarchar(255) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241114105610_ChangeNullableFields'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241114105610_ChangeNullableFields', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250508103111_SeedDataGroupandSubGroupTables'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Type') AND [object_id] = OBJECT_ID(N'[Lookup].[Group]'))
        SET IDENTITY_INSERT [Lookup].[Group] ON;
    EXEC(N'INSERT INTO [Lookup].[Group] ([Id], [Description], [Type])
    VALUES (7, ''Exporters'', ''Exporters''),
    (8, ''Reprocessors'', ''Reprocessors'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Type') AND [object_id] = OBJECT_ID(N'[Lookup].[Group]'))
        SET IDENTITY_INSERT [Lookup].[Group] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250508103111_SeedDataGroupandSubGroupTables'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Type') AND [object_id] = OBJECT_ID(N'[Lookup].[SubGroup]'))
        SET IDENTITY_INSERT [Lookup].[SubGroup] ON;
    EXEC(N'INSERT INTO [Lookup].[SubGroup] ([Id], [Description], [Type])
    VALUES (9, ''Aluminium'', ''Aluminium''),
    (10, ''Glass'', ''Glass''),
    (11, ''Paper, board or fibre-based composite material'', ''PaperOrBoardOrFibreBasedCompositeMaterial''),
    (12, ''Plastic'', ''Plastic''),
    (13, ''Steel'', ''Steel''),
    (14, ''Wood'', ''Wood'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Type') AND [object_id] = OBJECT_ID(N'[Lookup].[SubGroup]'))
        SET IDENTITY_INSERT [Lookup].[SubGroup] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250508103111_SeedDataGroupandSubGroupTables'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250508103111_SeedDataGroupandSubGroupTables', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250508114330_SeedRegistrationFeesTablesExporterData'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Amount', N'EffectiveFrom', N'EffectiveTo', N'GroupId', N'RegulatorId', N'SubGroupId') AND [object_id] = OBJECT_ID(N'[Lookup].[RegistrationFees]'))
        SET IDENTITY_INSERT [Lookup].[RegistrationFees] ON;
    EXEC(N'INSERT INTO [Lookup].[RegistrationFees] ([Id], [Amount], [EffectiveFrom], [EffectiveTo], [GroupId], [RegulatorId], [SubGroupId])
    VALUES (61, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 7, 1, 9),
    (62, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 7, 1, 10),
    (63, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 7, 1, 11),
    (64, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 7, 1, 12),
    (65, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 7, 1, 13),
    (66, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 7, 1, 14),
    (67, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 7, 2, 9),
    (68, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 7, 2, 10),
    (69, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 7, 2, 11),
    (70, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 7, 2, 12),
    (71, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 7, 2, 13),
    (72, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 7, 2, 14),
    (73, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 7, 3, 9),
    (74, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 7, 3, 10),
    (75, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 7, 3, 11),
    (76, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 7, 3, 12),
    (77, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 7, 3, 13),
    (78, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 7, 3, 14),
    (79, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 7, 4, 9),
    (80, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 7, 4, 10),
    (81, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 7, 4, 11),
    (82, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 7, 4, 12),
    (83, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 7, 4, 13),
    (84, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 7, 4, 14)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Amount', N'EffectiveFrom', N'EffectiveTo', N'GroupId', N'RegulatorId', N'SubGroupId') AND [object_id] = OBJECT_ID(N'[Lookup].[RegistrationFees]'))
        SET IDENTITY_INSERT [Lookup].[RegistrationFees] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250508114330_SeedRegistrationFeesTablesExporterData'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250508114330_SeedRegistrationFeesTablesExporterData', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250509082826_SeedRegistrationFeesTablesReprocessorData'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Amount', N'EffectiveFrom', N'EffectiveTo', N'GroupId', N'RegulatorId', N'SubGroupId') AND [object_id] = OBJECT_ID(N'[Lookup].[RegistrationFees]'))
        SET IDENTITY_INSERT [Lookup].[RegistrationFees] ON;
    EXEC(N'INSERT INTO [Lookup].[RegistrationFees] ([Id], [Amount], [EffectiveFrom], [EffectiveTo], [GroupId], [RegulatorId], [SubGroupId])
    VALUES (85, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 8, 1, 9),
    (86, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 8, 1, 10),
    (87, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 8, 1, 11),
    (88, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 8, 1, 12),
    (89, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 8, 1, 13),
    (90, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 8, 1, 14),
    (91, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 8, 2, 9),
    (92, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 8, 2, 10),
    (93, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 8, 2, 11),
    (94, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 8, 2, 12),
    (95, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 8, 2, 13),
    (96, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 8, 2, 14),
    (97, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 8, 3, 9),
    (98, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 8, 3, 10),
    (99, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 8, 3, 11),
    (100, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 8, 3, 12),
    (101, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 8, 3, 13),
    (102, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 8, 3, 14),
    (103, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 8, 4, 9),
    (104, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 8, 4, 10),
    (105, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 8, 4, 11),
    (106, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 8, 4, 12),
    (107, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 8, 4, 13),
    (108, 2921.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 8, 4, 14)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Amount', N'EffectiveFrom', N'EffectiveTo', N'GroupId', N'RegulatorId', N'SubGroupId') AND [object_id] = OBJECT_ID(N'[Lookup].[RegistrationFees]'))
        SET IDENTITY_INSERT [Lookup].[RegistrationFees] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250509082826_SeedRegistrationFeesTablesReprocessorData'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250509082826_SeedRegistrationFeesTablesReprocessorData', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250509154544_CreateAccreditationFeesTable'
)
BEGIN
    CREATE TABLE [Lookup].[AccreditationFees] (
        [Id] int NOT NULL IDENTITY,
        [GroupId] int NOT NULL,
        [SubGroupId] int NOT NULL,
        [RegulatorId] int NOT NULL,
        [TonnesUpTo] int NOT NULL,
        [TonnesOver] int NOT NULL,
        [Amount] decimal(19,4) NOT NULL,
        [FeesPerSite] decimal(19,4) NOT NULL,
        [EffectiveFrom] datetime2 NOT NULL,
        [EffectiveTo] datetime2 NOT NULL,
        CONSTRAINT [PK_AccreditationFees] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AccreditationFees_Group_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [Lookup].[Group] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AccreditationFees_Regulator_RegulatorId] FOREIGN KEY ([RegulatorId]) REFERENCES [Lookup].[Regulator] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AccreditationFees_SubGroup_SubGroupId] FOREIGN KEY ([SubGroupId]) REFERENCES [Lookup].[SubGroup] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250509154544_CreateAccreditationFeesTable'
)
BEGIN
    CREATE INDEX [IX_AccreditationFees_GroupId] ON [Lookup].[AccreditationFees] ([GroupId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250509154544_CreateAccreditationFeesTable'
)
BEGIN
    CREATE INDEX [IX_AccreditationFees_RegulatorId] ON [Lookup].[AccreditationFees] ([RegulatorId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250509154544_CreateAccreditationFeesTable'
)
BEGIN
    CREATE INDEX [IX_AccreditationFees_SubGroupId] ON [Lookup].[AccreditationFees] ([SubGroupId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250509154544_CreateAccreditationFeesTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250509154544_CreateAccreditationFeesTable', N'8.0.4');
END;
GO

COMMIT;
GO

