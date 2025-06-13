BEGIN TRANSACTION;
GO

CREATE TABLE [Lookup].[RequestorType] (
    [Id] int NOT NULL IDENTITY,
    [Type] varchar(50) NOT NULL,
    [Description] varchar(255) NOT NULL,
    CONSTRAINT [PK_RequestorType] PRIMARY KEY ([Id])
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Type') AND [object_id] = OBJECT_ID(N'[Lookup].[RequestorType]'))
    SET IDENTITY_INSERT [Lookup].[RequestorType] ON;
INSERT INTO [Lookup].[RequestorType] ([Id], [Description], [Type])
VALUES (1, 'Not Applicable', 'NA'),
(2, 'Producers', 'Producers'),
(3, 'Compliance Schemes', 'ComplianceSchemes'),
(4, 'Exporters', 'Exporters'),
(5, 'Reprocessors', 'Reprocessors');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Type') AND [object_id] = OBJECT_ID(N'[Lookup].[RequestorType]'))
    SET IDENTITY_INSERT [Lookup].[RequestorType] OFF;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250611100506_AddingRequestorTypeTable', N'8.0.4');
GO

COMMIT;
GO

