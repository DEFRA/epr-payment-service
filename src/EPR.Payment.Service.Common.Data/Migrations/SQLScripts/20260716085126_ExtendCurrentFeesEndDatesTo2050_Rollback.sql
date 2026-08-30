BEGIN TRANSACTION;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000000;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000001;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000002;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000003;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000004;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000005;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000006;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000007;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000008;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000009;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000010;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000011;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000012;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000013;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000014;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000015;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000016;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000017;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000018;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000019;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000020;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000021;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000022;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000023;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000024;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000025;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000026;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000027;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000028;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000029;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000030;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000031;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000032;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000033;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000034;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000035;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000036;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000037;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000038;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000039;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000040;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000041;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000042;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000043;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000044;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000045;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000046;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000047;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000048;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000049;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000050;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000051;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000052;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000053;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000054;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000055;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000056;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000057;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000058;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000059;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000060;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000061;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000062;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000063;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000064;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000065;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000066;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    EXEC(N'UPDATE [Lookup].[RegistrationFees] SET [EffectiveTo] = ''2026-12-31T23:59:59.0000000Z''
    WHERE [Id] = 26000067;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050'
)
BEGIN
    DELETE FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716085126_ExtendCurrentFeesEndDatesTo2050';
END;
GO

COMMIT;
GO

