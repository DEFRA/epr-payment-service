BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260427101504_add-closed-loop-recycling-fee-2026'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Type') AND [object_id] = OBJECT_ID(N'[Lookup].[SubGroup]'))
        SET IDENTITY_INSERT [Lookup].[SubGroup] ON;
    EXEC(N'INSERT INTO [Lookup].[SubGroup] ([Id], [Description], [Type])
    VALUES (15, ''Closed Loop Recycling'', ''ClosedLoop'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Type') AND [object_id] = OBJECT_ID(N'[Lookup].[SubGroup]'))
        SET IDENTITY_INSERT [Lookup].[SubGroup] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260427101504_add-closed-loop-recycling-fee-2026'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Amount', N'EffectiveFrom', N'EffectiveTo', N'GroupId', N'RegulatorId', N'SubGroupId') AND [object_id] = OBJECT_ID(N'[Lookup].[RegistrationFees]'))
        SET IDENTITY_INSERT [Lookup].[RegistrationFees] ON;
    EXEC(N'INSERT INTO [Lookup].[RegistrationFees] ([Id], [Amount], [EffectiveFrom], [EffectiveTo], [GroupId], [RegulatorId], [SubGroupId])
    VALUES (26000060, 254800.0, ''2026-01-01T00:00:00.0000000Z'', ''2026-12-31T23:59:59.0000000Z'', 2, 1, 15),
    (26000061, 254800.0, ''2026-01-01T00:00:00.0000000Z'', ''2026-12-31T23:59:59.0000000Z'', 2, 2, 15),
    (26000062, 254800.0, ''2026-01-01T00:00:00.0000000Z'', ''2026-12-31T23:59:59.0000000Z'', 2, 3, 15),
    (26000063, 254800.0, ''2026-01-01T00:00:00.0000000Z'', ''2026-12-31T23:59:59.0000000Z'', 2, 4, 15),
    (26000064, 254800.0, ''2026-01-01T00:00:00.0000000Z'', ''2026-12-31T23:59:59.0000000Z'', 1, 1, 15),
    (26000065, 254800.0, ''2026-01-01T00:00:00.0000000Z'', ''2026-12-31T23:59:59.0000000Z'', 1, 2, 15),
    (26000066, 254800.0, ''2026-01-01T00:00:00.0000000Z'', ''2026-12-31T23:59:59.0000000Z'', 1, 3, 15),
    (26000067, 254800.0, ''2026-01-01T00:00:00.0000000Z'', ''2026-12-31T23:59:59.0000000Z'', 1, 4, 15)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Amount', N'EffectiveFrom', N'EffectiveTo', N'GroupId', N'RegulatorId', N'SubGroupId') AND [object_id] = OBJECT_ID(N'[Lookup].[RegistrationFees]'))
        SET IDENTITY_INSERT [Lookup].[RegistrationFees] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260427101504_add-closed-loop-recycling-fee-2026'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260427101504_add-closed-loop-recycling-fee-2026', N'8.0.4');
END;
GO

COMMIT;
GO

