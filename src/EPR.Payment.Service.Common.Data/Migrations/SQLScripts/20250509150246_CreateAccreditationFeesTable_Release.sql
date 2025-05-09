BEGIN TRANSACTION;
GO

CREATE TABLE [Lookup].[AccreditationFees] (
    [Id] int NOT NULL IDENTITY,
    [GroupId] int NOT NULL,
    [SubGroupId] int NOT NULL,
    [RegulatorId] int NOT NULL,
    [TonnesUpTo] int NOT NULL,
    [TonnesOver] int NOT NULL,
    [Amount] decimal(19,4) NOT NULL,
    [FeesPerSite] int NOT NULL,
    [EffectiveFrom] datetime2 NOT NULL,
    [EffectiveTo] datetime2 NOT NULL,
    CONSTRAINT [PK_AccreditationFees] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AccreditationFees_Group_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [Lookup].[Group] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AccreditationFees_Regulator_RegulatorId] FOREIGN KEY ([RegulatorId]) REFERENCES [Lookup].[Regulator] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AccreditationFees_SubGroup_SubGroupId] FOREIGN KEY ([SubGroupId]) REFERENCES [Lookup].[SubGroup] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_AccreditationFees_GroupId] ON [Lookup].[AccreditationFees] ([GroupId]);
GO

CREATE INDEX [IX_AccreditationFees_RegulatorId] ON [Lookup].[AccreditationFees] ([RegulatorId]);
GO

CREATE INDEX [IX_AccreditationFees_SubGroupId] ON [Lookup].[AccreditationFees] ([SubGroupId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250509150246_CreateAccreditationFeesTable', N'8.0.4');
GO

COMMIT;
GO

