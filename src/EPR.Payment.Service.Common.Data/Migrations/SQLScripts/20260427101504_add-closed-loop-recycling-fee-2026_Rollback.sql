BEGIN TRANSACTION;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260427101504_add-closed-loop-recycling-fee-2026'
)
BEGIN
    EXEC(N'DELETE FROM [Lookup].[RegistrationFees]
    WHERE [Id] = 26000060;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260427101504_add-closed-loop-recycling-fee-2026'
)
BEGIN
    EXEC(N'DELETE FROM [Lookup].[RegistrationFees]
    WHERE [Id] = 26000061;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260427101504_add-closed-loop-recycling-fee-2026'
)
BEGIN
    EXEC(N'DELETE FROM [Lookup].[RegistrationFees]
    WHERE [Id] = 26000062;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260427101504_add-closed-loop-recycling-fee-2026'
)
BEGIN
    EXEC(N'DELETE FROM [Lookup].[RegistrationFees]
    WHERE [Id] = 26000063;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260427101504_add-closed-loop-recycling-fee-2026'
)
BEGIN
    EXEC(N'DELETE FROM [Lookup].[RegistrationFees]
    WHERE [Id] = 26000064;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260427101504_add-closed-loop-recycling-fee-2026'
)
BEGIN
    EXEC(N'DELETE FROM [Lookup].[RegistrationFees]
    WHERE [Id] = 26000065;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260427101504_add-closed-loop-recycling-fee-2026'
)
BEGIN
    EXEC(N'DELETE FROM [Lookup].[RegistrationFees]
    WHERE [Id] = 26000066;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260427101504_add-closed-loop-recycling-fee-2026'
)
BEGIN
    EXEC(N'DELETE FROM [Lookup].[RegistrationFees]
    WHERE [Id] = 26000067;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260427101504_add-closed-loop-recycling-fee-2026'
)
BEGIN
    EXEC(N'DELETE FROM [Lookup].[SubGroup]
    WHERE [Id] = 15;
    SELECT @@ROWCOUNT');
END;
GO

IF EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260427101504_add-closed-loop-recycling-fee-2026'
)
BEGIN
    DELETE FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260427101504_add-closed-loop-recycling-fee-2026';
END;
GO

COMMIT;
GO

