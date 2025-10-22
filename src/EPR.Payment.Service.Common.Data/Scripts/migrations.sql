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

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512155019_SeedAccreditationFeesTablesExporterData'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Amount', N'EffectiveFrom', N'EffectiveTo', N'FeesPerSite', N'GroupId', N'RegulatorId', N'SubGroupId', N'TonnesOver', N'TonnesUpTo') AND [object_id] = OBJECT_ID(N'[Lookup].[AccreditationFees]'))
        SET IDENTITY_INSERT [Lookup].[AccreditationFees] ON;
    EXEC(N'INSERT INTO [Lookup].[AccreditationFees] ([Id], [Amount], [EffectiveFrom], [EffectiveTo], [FeesPerSite], [GroupId], [RegulatorId], [SubGroupId], [TonnesOver], [TonnesUpTo])
    VALUES (1, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 1, 9, 0, 500),
    (2, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 1, 9, 500, 5000),
    (3, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 1, 9, 5000, 10000),
    (4, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 1, 9, 10000, 99999999),
    (5, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 1, 10, 0, 500),
    (6, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 1, 10, 500, 5000),
    (7, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 1, 10, 5000, 10000),
    (8, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 1, 10, 10000, 99999999),
    (9, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 1, 11, 0, 500),
    (10, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 1, 11, 500, 5000),
    (11, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 1, 11, 5000, 10000),
    (12, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 1, 11, 10000, 99999999),
    (13, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 1, 12, 0, 500),
    (14, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 1, 12, 500, 5000),
    (15, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 1, 12, 5000, 10000),
    (16, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 1, 12, 10000, 99999999),
    (17, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 1, 13, 0, 500),
    (18, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 1, 13, 500, 5000),
    (19, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 1, 13, 5000, 10000),
    (20, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 1, 13, 10000, 99999999),
    (21, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 1, 14, 0, 500),
    (22, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 1, 14, 500, 5000),
    (23, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 1, 14, 5000, 10000),
    (24, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 1, 14, 10000, 99999999),
    (25, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 4, 9, 0, 500),
    (26, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 4, 9, 500, 5000),
    (27, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 4, 9, 5000, 10000),
    (28, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 4, 9, 10000, 99999999),
    (29, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 4, 10, 0, 500),
    (30, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 4, 10, 500, 5000),
    (31, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 4, 10, 5000, 10000),
    (32, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 4, 10, 10000, 99999999),
    (33, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 4, 11, 0, 500),
    (34, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 4, 11, 500, 5000),
    (35, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 4, 11, 5000, 10000),
    (36, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 4, 11, 10000, 99999999),
    (37, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 4, 12, 0, 500),
    (38, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 4, 12, 500, 5000),
    (39, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 4, 12, 5000, 10000),
    (40, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 4, 12, 10000, 99999999),
    (41, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 4, 13, 0, 500),
    (42, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 4, 13, 500, 5000);
    INSERT INTO [Lookup].[AccreditationFees] ([Id], [Amount], [EffectiveFrom], [EffectiveTo], [FeesPerSite], [GroupId], [RegulatorId], [SubGroupId], [TonnesOver], [TonnesUpTo])
    VALUES (43, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 4, 13, 5000, 10000),
    (44, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 4, 13, 10000, 99999999),
    (45, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 4, 14, 0, 500),
    (46, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 4, 14, 500, 5000),
    (47, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 4, 14, 5000, 10000),
    (48, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 4, 14, 10000, 99999999),
    (49, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 2, 9, 0, 500),
    (50, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 2, 9, 500, 5000),
    (51, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 2, 9, 5000, 10000),
    (52, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 2, 9, 10000, 99999999),
    (53, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 2, 10, 0, 500),
    (54, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 2, 10, 500, 5000),
    (55, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 2, 10, 5000, 10000),
    (56, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 2, 10, 10000, 99999999),
    (57, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 2, 11, 0, 500),
    (58, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 2, 11, 500, 5000),
    (59, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 2, 11, 5000, 10000),
    (60, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 2, 11, 10000, 99999999),
    (61, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 2, 12, 0, 500),
    (62, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 2, 12, 500, 5000),
    (63, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 2, 12, 5000, 10000),
    (64, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 2, 12, 10000, 99999999),
    (65, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 2, 13, 0, 500),
    (66, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 2, 13, 500, 5000),
    (67, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 2, 13, 5000, 10000),
    (68, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 2, 13, 10000, 99999999),
    (69, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 2, 14, 0, 500),
    (70, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 2, 14, 500, 5000),
    (71, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 2, 14, 5000, 10000),
    (72, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 2, 14, 10000, 99999999),
    (73, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 3, 9, 0, 500),
    (74, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 3, 9, 500, 5000),
    (75, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 3, 9, 5000, 10000),
    (76, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 3, 9, 10000, 99999999),
    (77, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 3, 10, 0, 500),
    (78, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 3, 10, 500, 5000),
    (79, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 3, 10, 5000, 10000),
    (80, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 3, 10, 10000, 99999999),
    (81, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 3, 11, 0, 500),
    (82, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 3, 11, 500, 5000),
    (83, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 3, 11, 5000, 10000),
    (84, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 3, 11, 10000, 99999999);
    INSERT INTO [Lookup].[AccreditationFees] ([Id], [Amount], [EffectiveFrom], [EffectiveTo], [FeesPerSite], [GroupId], [RegulatorId], [SubGroupId], [TonnesOver], [TonnesUpTo])
    VALUES (85, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 3, 12, 0, 500),
    (86, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 3, 12, 500, 5000),
    (87, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 3, 12, 5000, 10000),
    (88, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 3, 12, 10000, 99999999),
    (89, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 3, 13, 0, 500),
    (90, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 3, 13, 500, 5000),
    (91, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 3, 13, 5000, 10000),
    (92, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 3, 13, 10000, 99999999),
    (93, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 3, 14, 0, 500),
    (94, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 3, 14, 500, 5000),
    (95, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 3, 14, 5000, 10000),
    (96, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 216.0, 7, 3, 14, 10000, 99999999)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Amount', N'EffectiveFrom', N'EffectiveTo', N'FeesPerSite', N'GroupId', N'RegulatorId', N'SubGroupId', N'TonnesOver', N'TonnesUpTo') AND [object_id] = OBJECT_ID(N'[Lookup].[AccreditationFees]'))
        SET IDENTITY_INSERT [Lookup].[AccreditationFees] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512155019_SeedAccreditationFeesTablesExporterData'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250512155019_SeedAccreditationFeesTablesExporterData', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250513003609_SeedAccreditationFeesTablesReprocessors'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Amount', N'EffectiveFrom', N'EffectiveTo', N'FeesPerSite', N'GroupId', N'RegulatorId', N'SubGroupId', N'TonnesOver', N'TonnesUpTo') AND [object_id] = OBJECT_ID(N'[Lookup].[AccreditationFees]'))
        SET IDENTITY_INSERT [Lookup].[AccreditationFees] ON;
    EXEC(N'INSERT INTO [Lookup].[AccreditationFees] ([Id], [Amount], [EffectiveFrom], [EffectiveTo], [FeesPerSite], [GroupId], [RegulatorId], [SubGroupId], [TonnesOver], [TonnesUpTo])
    VALUES (97, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 1, 9, 0, 500),
    (98, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 1, 9, 500, 5000),
    (99, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 1, 9, 5000, 10000),
    (100, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 1, 9, 10000, 99999999),
    (101, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 1, 10, 0, 500),
    (102, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 1, 10, 500, 5000),
    (103, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 1, 10, 5000, 10000),
    (104, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 1, 10, 10000, 99999999),
    (105, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 1, 11, 0, 500),
    (106, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 1, 11, 500, 5000),
    (107, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 1, 11, 5000, 10000),
    (108, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 1, 11, 10000, 99999999),
    (109, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 1, 12, 0, 500),
    (110, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 1, 12, 500, 5000),
    (111, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 1, 12, 5000, 10000),
    (112, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 1, 12, 10000, 99999999),
    (113, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 1, 13, 0, 500),
    (114, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 1, 13, 500, 5000),
    (115, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 1, 13, 5000, 10000),
    (116, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 1, 13, 10000, 99999999),
    (117, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 1, 14, 0, 500),
    (118, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 1, 14, 500, 5000),
    (119, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 1, 14, 5000, 10000),
    (120, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 1, 14, 10000, 99999999),
    (121, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 4, 9, 0, 500),
    (122, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 4, 9, 500, 5000),
    (123, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 4, 9, 5000, 10000),
    (124, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 4, 9, 10000, 99999999),
    (125, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 4, 10, 0, 500),
    (126, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 4, 10, 500, 5000),
    (127, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 4, 10, 5000, 10000),
    (128, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 4, 10, 10000, 99999999),
    (129, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 4, 11, 0, 500),
    (130, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 4, 11, 500, 5000),
    (131, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 4, 11, 5000, 10000),
    (132, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 4, 11, 10000, 99999999),
    (133, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 4, 12, 0, 500),
    (134, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 4, 12, 500, 5000),
    (135, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 4, 12, 5000, 10000),
    (136, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 4, 12, 10000, 99999999),
    (137, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 4, 13, 0, 500),
    (138, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 4, 13, 500, 5000);
    INSERT INTO [Lookup].[AccreditationFees] ([Id], [Amount], [EffectiveFrom], [EffectiveTo], [FeesPerSite], [GroupId], [RegulatorId], [SubGroupId], [TonnesOver], [TonnesUpTo])
    VALUES (139, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 4, 13, 5000, 10000),
    (140, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 4, 13, 10000, 99999999),
    (141, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 4, 14, 0, 500),
    (142, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 4, 14, 500, 5000),
    (143, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 4, 14, 5000, 10000),
    (144, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 4, 14, 10000, 99999999),
    (145, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 2, 9, 0, 500),
    (146, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 2, 9, 500, 5000),
    (147, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 2, 9, 5000, 10000),
    (148, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 2, 9, 10000, 99999999),
    (149, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 2, 10, 0, 500),
    (150, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 2, 10, 500, 5000),
    (151, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 2, 10, 5000, 10000),
    (152, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 2, 10, 10000, 99999999),
    (153, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 2, 11, 0, 500),
    (154, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 2, 11, 500, 5000),
    (155, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 2, 11, 5000, 10000),
    (156, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 2, 11, 10000, 99999999),
    (157, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 2, 12, 0, 500),
    (158, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 2, 12, 500, 5000),
    (159, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 2, 12, 5000, 10000),
    (160, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 2, 12, 10000, 99999999),
    (161, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 2, 13, 0, 500),
    (162, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 2, 13, 500, 5000),
    (163, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 2, 13, 5000, 10000),
    (164, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 2, 13, 10000, 99999999),
    (165, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 2, 14, 0, 500),
    (166, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 2, 14, 500, 5000),
    (167, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 2, 14, 5000, 10000),
    (168, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 2, 14, 10000, 99999999),
    (169, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 3, 9, 0, 500),
    (170, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 3, 9, 500, 5000),
    (171, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 3, 9, 5000, 10000),
    (172, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 3, 9, 10000, 99999999),
    (173, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 3, 10, 0, 500),
    (174, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 3, 10, 500, 5000),
    (175, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 3, 10, 5000, 10000),
    (176, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 3, 10, 10000, 99999999),
    (177, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 3, 11, 0, 500),
    (178, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 3, 11, 500, 5000),
    (179, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 3, 11, 5000, 10000),
    (180, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 3, 11, 10000, 99999999);
    INSERT INTO [Lookup].[AccreditationFees] ([Id], [Amount], [EffectiveFrom], [EffectiveTo], [FeesPerSite], [GroupId], [RegulatorId], [SubGroupId], [TonnesOver], [TonnesUpTo])
    VALUES (181, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 3, 12, 0, 500),
    (182, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 3, 12, 500, 5000),
    (183, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 3, 12, 5000, 10000),
    (184, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 3, 12, 10000, 99999999),
    (185, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 3, 13, 0, 500),
    (186, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 3, 13, 500, 5000),
    (187, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 3, 13, 5000, 10000),
    (188, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 3, 13, 10000, 99999999),
    (189, 500.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 3, 14, 0, 500),
    (190, 2000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 3, 14, 500, 5000),
    (191, 3000.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 3, 14, 5000, 10000),
    (192, 3631.0, ''2024-09-01T00:00:00.0000000Z'', ''9999-08-31T23:59:59.0000000Z'', 0.0, 8, 3, 14, 10000, 99999999)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Amount', N'EffectiveFrom', N'EffectiveTo', N'FeesPerSite', N'GroupId', N'RegulatorId', N'SubGroupId', N'TonnesOver', N'TonnesUpTo') AND [object_id] = OBJECT_ID(N'[Lookup].[AccreditationFees]'))
        SET IDENTITY_INSERT [Lookup].[AccreditationFees] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250513003609_SeedAccreditationFeesTablesReprocessors'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250513003609_SeedAccreditationFeesTablesReprocessors', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250515105845_AddingPaymentMethodColumn'
)
BEGIN
    ALTER TABLE [OfflinePayment] ADD [PaymentMethod] nvarchar(20) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250515105845_AddingPaymentMethodColumn'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250515105845_AddingPaymentMethodColumn', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250515115823_AddingRequestorTypeColumn'
)
BEGIN
    ALTER TABLE [OnlinePayment] ADD [RequestorType] nvarchar(50) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250515115823_AddingRequestorTypeColumn'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250515115823_AddingRequestorTypeColumn', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250605112455_AddingTonnageBandTable'
)
BEGIN
    CREATE TABLE [Lookup].[TonnageBand] (
        [Id] int NOT NULL IDENTITY,
        [Type] varchar(50) NOT NULL,
        [Description] varchar(255) NOT NULL,
        CONSTRAINT [PK_TonnageBand] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250605112455_AddingTonnageBandTable'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Type') AND [object_id] = OBJECT_ID(N'[Lookup].[TonnageBand]'))
        SET IDENTITY_INSERT [Lookup].[TonnageBand] ON;
    EXEC(N'INSERT INTO [Lookup].[TonnageBand] ([Id], [Description], [Type])
    VALUES (1, ''Tonnage upto 500 tonnes'', ''Upto500''),
    (2, ''Tonnage over 500 to 5000 tonnes'', ''Over500To5000''),
    (3, ''Tonnage over 5000 to 10000 tonnes'', ''Over5000To10000''),
    (4, ''Tonnage over 10000 tonnes'', ''Over10000'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Type') AND [object_id] = OBJECT_ID(N'[Lookup].[TonnageBand]'))
        SET IDENTITY_INSERT [Lookup].[TonnageBand] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250605112455_AddingTonnageBandTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250605112455_AddingTonnageBandTable', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    DECLARE @var24 sysname;
    SELECT @var24 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Lookup].[AccreditationFees]') AND [c].[name] = N'TonnesOver');
    IF @var24 IS NOT NULL EXEC(N'ALTER TABLE [Lookup].[AccreditationFees] DROP CONSTRAINT [' + @var24 + '];');
    ALTER TABLE [Lookup].[AccreditationFees] DROP COLUMN [TonnesOver];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    DECLARE @var25 sysname;
    SELECT @var25 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Lookup].[AccreditationFees]') AND [c].[name] = N'TonnesUpTo');
    IF @var25 IS NOT NULL EXEC(N'ALTER TABLE [Lookup].[AccreditationFees] DROP CONSTRAINT [' + @var25 + '];');
    ALTER TABLE [Lookup].[AccreditationFees] DROP COLUMN [TonnesUpTo];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    ALTER TABLE [Lookup].[AccreditationFees] ADD [TonnageBandId] int NOT NULL DEFAULT 1;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 1;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 2;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 3;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 4;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 5;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 6;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 7;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 8;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 9;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 10;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 11;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 12;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 13;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 14;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 15;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 16;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 17;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 18;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 19;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 20;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 21;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 22;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 23;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 24;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 25;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 26;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 27;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 28;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 29;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 30;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 31;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 32;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 33;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 34;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 35;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 36;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 37;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 38;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 39;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 40;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 41;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 42;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 43;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 44;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 45;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 46;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 47;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 48;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 49;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 50;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 51;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 52;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 53;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 54;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 55;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 56;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 57;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 58;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 59;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 60;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 61;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 62;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 63;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 64;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 65;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 66;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 67;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 68;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 69;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 70;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 71;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 72;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 73;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 74;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 75;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 76;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 77;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 78;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 79;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 80;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 81;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 82;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 83;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 84;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 85;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 86;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 87;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 88;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 89;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 90;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 91;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 92;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 93;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 94;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 95;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 96;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 97;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 98;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 99;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 100;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 101;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 102;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 103;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 104;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 105;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 106;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 107;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 108;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 109;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 110;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 111;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 112;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 113;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 114;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 115;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 116;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 117;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 118;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 119;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 120;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 121;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 122;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 123;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 124;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 125;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 126;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 127;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 128;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 129;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 130;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 131;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 132;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 133;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 134;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 135;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 136;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 137;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 138;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 139;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 140;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 141;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 142;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 143;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 144;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 145;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 146;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 147;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 148;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 149;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 150;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 151;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 152;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 153;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 154;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 155;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 156;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 157;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 158;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 159;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 160;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 161;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 162;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 163;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 164;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 165;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 166;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 167;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 168;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 169;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 170;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 171;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 172;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 173;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 174;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 175;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 176;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 177;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 178;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 179;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 180;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 181;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 182;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 183;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 184;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 185;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 186;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 187;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 188;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 1
    WHERE [Id] = 189;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 2
    WHERE [Id] = 190;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 3
    WHERE [Id] = 191;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[AccreditationFees] SET [TonnageBandId] = 4
    WHERE [Id] = 192;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    CREATE INDEX [IX_AccreditationFees_TonnageBandId] ON [Lookup].[AccreditationFees] ([TonnageBandId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    ALTER TABLE [Lookup].[AccreditationFees] ADD CONSTRAINT [FK_AccreditationFees_TonnageBand_TonnageBandId] FOREIGN KEY ([TonnageBandId]) REFERENCES [Lookup].[TonnageBand] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250606151503_AccreditationFeeTableDesignChanges'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250606151503_AccreditationFeeTableDesignChanges', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250611100506_AddingRequestorTypeTable'
)
BEGIN
    CREATE TABLE [Lookup].[RequestorType] (
        [Id] int NOT NULL IDENTITY,
        [Type] varchar(50) NOT NULL,
        [Description] varchar(255) NOT NULL,
        CONSTRAINT [PK_RequestorType] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250611100506_AddingRequestorTypeTable'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Type') AND [object_id] = OBJECT_ID(N'[Lookup].[RequestorType]'))
        SET IDENTITY_INSERT [Lookup].[RequestorType] ON;
    EXEC(N'INSERT INTO [Lookup].[RequestorType] ([Id], [Description], [Type])
    VALUES (1, ''Not Applicable'', ''NA''),
    (2, ''Producers'', ''Producers''),
    (3, ''Compliance Schemes'', ''ComplianceSchemes''),
    (4, ''Exporters'', ''Exporters''),
    (5, ''Reprocessors'', ''Reprocessors'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Type') AND [object_id] = OBJECT_ID(N'[Lookup].[RequestorType]'))
        SET IDENTITY_INSERT [Lookup].[RequestorType] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250611100506_AddingRequestorTypeTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250611100506_AddingRequestorTypeTable', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250611164617_ChangedOnlinePaymentTable'
)
BEGIN
    DECLARE @var26 sysname;
    SELECT @var26 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[OnlinePayment]') AND [c].[name] = N'RequestorType');
    IF @var26 IS NOT NULL EXEC(N'ALTER TABLE [OnlinePayment] DROP CONSTRAINT [' + @var26 + '];');
    ALTER TABLE [OnlinePayment] DROP COLUMN [RequestorType];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250611164617_ChangedOnlinePaymentTable'
)
BEGIN
    ALTER TABLE [OnlinePayment] ADD [RequestorTypeId] int NOT NULL DEFAULT 1;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250611164617_ChangedOnlinePaymentTable'
)
BEGIN
    CREATE INDEX [IX_OnlinePayment_RequestorTypeId] ON [OnlinePayment] ([RequestorTypeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250611164617_ChangedOnlinePaymentTable'
)
BEGIN
    ALTER TABLE [OnlinePayment] ADD CONSTRAINT [FK_OnlinePayment_RequestorType_RequestorTypeId] FOREIGN KEY ([RequestorTypeId]) REFERENCES [Lookup].[RequestorType] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250611164617_ChangedOnlinePaymentTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250611164617_ChangedOnlinePaymentTable', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612130529_AddedPaymentMethodTable'
)
BEGIN
    CREATE TABLE [Lookup].[PaymentMethod] (
        [Id] int NOT NULL IDENTITY,
        [Type] varchar(50) NOT NULL,
        [Description] varchar(255) NOT NULL,
        CONSTRAINT [PK_PaymentMethod] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612130529_AddedPaymentMethodTable'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Type') AND [object_id] = OBJECT_ID(N'[Lookup].[PaymentMethod]'))
        SET IDENTITY_INSERT [Lookup].[PaymentMethod] ON;
    EXEC(N'INSERT INTO [Lookup].[PaymentMethod] ([Id], [Description], [Type])
    VALUES (1, ''Not Applicable'', ''NA''),
    (2, ''Bank transfer'', ''BankTransfer''),
    (3, ''Credit or debit card'', ''CreditOrDebitCard''),
    (4, ''Cheque'', ''Cheque''),
    (5, ''Cash'', ''Cash'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Type') AND [object_id] = OBJECT_ID(N'[Lookup].[PaymentMethod]'))
        SET IDENTITY_INSERT [Lookup].[PaymentMethod] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250612130529_AddedPaymentMethodTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250612130529_AddedPaymentMethodTable', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250616080947_ChangedOfflinePaymentTable'
)
BEGIN
    DECLARE @var27 sysname;
    SELECT @var27 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[OfflinePayment]') AND [c].[name] = N'PaymentMethod');
    IF @var27 IS NOT NULL EXEC(N'ALTER TABLE [OfflinePayment] DROP CONSTRAINT [' + @var27 + '];');
    ALTER TABLE [OfflinePayment] DROP COLUMN [PaymentMethod];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250616080947_ChangedOfflinePaymentTable'
)
BEGIN
    ALTER TABLE [OfflinePayment] ADD [PaymentMethodId] int NOT NULL DEFAULT 1;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250616080947_ChangedOfflinePaymentTable'
)
BEGIN
    CREATE INDEX [IX_OfflinePayment_PaymentMethodId] ON [OfflinePayment] ([PaymentMethodId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250616080947_ChangedOfflinePaymentTable'
)
BEGIN
    ALTER TABLE [OfflinePayment] ADD CONSTRAINT [FK_OfflinePayment_PaymentMethod_PaymentMethodId] FOREIGN KEY ([PaymentMethodId]) REFERENCES [Lookup].[PaymentMethod] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250616080947_ChangedOfflinePaymentTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250616080947_ChangedOfflinePaymentTable', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250702083006_AddingOrganisationIdColumn'
)
BEGIN
    ALTER TABLE [OfflinePayment] ADD [OrganisationId] uniqueidentifier NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250702083006_AddingOrganisationIdColumn'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250702083006_AddingOrganisationIdColumn', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250718150533_AddFeeSummaryTables'
)
BEGIN
    CREATE TABLE [Lookup].[FeeTypes] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        CONSTRAINT [PK_FeeTypes] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250718150533_AddFeeSummaryTables'
)
BEGIN
    CREATE TABLE [Lookup].[PayerTypes] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(50) NOT NULL,
        CONSTRAINT [PK_PayerTypes] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250718150533_AddFeeSummaryTables'
)
BEGIN
    CREATE TABLE [FeeSummaries] (
        [Id] int NOT NULL IDENTITY,
        [ExternalId] uniqueidentifier NOT NULL,
        [AppRefNo] nvarchar(50) NOT NULL,
        [InvoiceDate] datetimeoffset NOT NULL,
        [InvoicePeriod] datetimeoffset NOT NULL,
        [PayerTypeId] int NOT NULL,
        [PayerId] int NOT NULL,
        [FeeTypeId] int NOT NULL,
        [UnitPrice] decimal(18,2) NULL,
        [Quantity] int NOT NULL DEFAULT 0,
        [Amount] decimal(18,2) NOT NULL,
        [CreatedDate] datetimeoffset NOT NULL,
        [UpdatedDate] datetimeoffset NULL,
        CONSTRAINT [PK_FeeSummaries] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_FeeSummaries_FeeTypes_FeeTypeId] FOREIGN KEY ([FeeTypeId]) REFERENCES [Lookup].[FeeTypes] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_FeeSummaries_PayerTypes_PayerTypeId] FOREIGN KEY ([PayerTypeId]) REFERENCES [Lookup].[PayerTypes] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250718150533_AddFeeSummaryTables'
)
BEGIN
    CREATE TABLE [FileFeeSummaryConnections] (
        [Id] int NOT NULL IDENTITY,
        [FileId] uniqueidentifier NOT NULL,
        [FeeSummaryId] int NOT NULL,
        CONSTRAINT [PK_FileFeeSummaryConnections] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_FileFeeSummaryConnections_FeeSummaries_FeeSummaryId] FOREIGN KEY ([FeeSummaryId]) REFERENCES [FeeSummaries] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250718150533_AddFeeSummaryTables'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup].[FeeTypes]'))
        SET IDENTITY_INSERT [Lookup].[FeeTypes] ON;
    EXEC(N'INSERT INTO [Lookup].[FeeTypes] ([Id], [Name])
    VALUES (1, N''Producer Registration Fee''),
    (2, N''Compliance Scheme Registration Fee''),
    (3, N''Producer OnlineMarketPlace Fee''),
    (4, N''Member Registration Fee''),
    (5, N''Member Late Registration Fee''),
    (6, N''UnitOMP Fee''),
    (7, N''Subsidiary Fee''),
    (8, N''Late Registration Fee'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup].[FeeTypes]'))
        SET IDENTITY_INSERT [Lookup].[FeeTypes] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250718150533_AddFeeSummaryTables'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup].[PayerTypes]'))
        SET IDENTITY_INSERT [Lookup].[PayerTypes] ON;
    EXEC(N'INSERT INTO [Lookup].[PayerTypes] ([Id], [Name])
    VALUES (1, N''Direct Producer''),
    (2, N''Compliance Scheme''),
    (3, N''Reprocessor''),
    (4, N''Exporter'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup].[PayerTypes]'))
        SET IDENTITY_INSERT [Lookup].[PayerTypes] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250718150533_AddFeeSummaryTables'
)
BEGIN
    CREATE INDEX [IX_FeeSummaries_FeeTypeId] ON [FeeSummaries] ([FeeTypeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250718150533_AddFeeSummaryTables'
)
BEGIN
    CREATE INDEX [IX_FeeSummaries_PayerTypeId] ON [FeeSummaries] ([PayerTypeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250718150533_AddFeeSummaryTables'
)
BEGIN
    CREATE INDEX [IX_FileFeeSummaryConnections_FeeSummaryId] ON [FileFeeSummaryConnections] ([FeeSummaryId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250718150533_AddFeeSummaryTables'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250718150533_AddFeeSummaryTables', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250921121924_AddResubmissionFeeTypes'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup].[FeeTypes]'))
        SET IDENTITY_INSERT [Lookup].[FeeTypes] ON;
    EXEC(N'INSERT INTO [Lookup].[FeeTypes] ([Id], [Name])
    VALUES (9, N''Producer Resubmission Fee''),
    (10, N''Compliance Scheme Resubmission Fee'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Lookup].[FeeTypes]'))
        SET IDENTITY_INSERT [Lookup].[FeeTypes] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250921121924_AddResubmissionFeeTypes'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250921121924_AddResubmissionFeeTypes', N'8.0.4');
END;
GO

COMMIT;
GO

