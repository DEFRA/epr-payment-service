BEGIN TRANSACTION;
GO

DELETE FROM [Lookup].[RegistrationFees]
WHERE [Id] = 85;
SELECT @@ROWCOUNT;

GO

DELETE FROM [Lookup].[RegistrationFees]
WHERE [Id] = 86;
SELECT @@ROWCOUNT;

GO

DELETE FROM [Lookup].[RegistrationFees]
WHERE [Id] = 87;
SELECT @@ROWCOUNT;

GO

DELETE FROM [Lookup].[RegistrationFees]
WHERE [Id] = 88;
SELECT @@ROWCOUNT;

GO

DELETE FROM [Lookup].[RegistrationFees]
WHERE [Id] = 89;
SELECT @@ROWCOUNT;

GO

DELETE FROM [Lookup].[RegistrationFees]
WHERE [Id] = 90;
SELECT @@ROWCOUNT;

GO

DELETE FROM [Lookup].[RegistrationFees]
WHERE [Id] = 91;
SELECT @@ROWCOUNT;

GO

DELETE FROM [Lookup].[RegistrationFees]
WHERE [Id] = 92;
SELECT @@ROWCOUNT;

GO

DELETE FROM [Lookup].[RegistrationFees]
WHERE [Id] = 93;
SELECT @@ROWCOUNT;

GO

DELETE FROM [Lookup].[RegistrationFees]
WHERE [Id] = 94;
SELECT @@ROWCOUNT;

GO

DELETE FROM [Lookup].[RegistrationFees]
WHERE [Id] = 95;
SELECT @@ROWCOUNT;

GO

DELETE FROM [Lookup].[RegistrationFees]
WHERE [Id] = 96;
SELECT @@ROWCOUNT;

GO

DELETE FROM [Lookup].[RegistrationFees]
WHERE [Id] = 97;
SELECT @@ROWCOUNT;

GO

DELETE FROM [Lookup].[RegistrationFees]
WHERE [Id] = 98;
SELECT @@ROWCOUNT;

GO

DELETE FROM [Lookup].[RegistrationFees]
WHERE [Id] = 99;
SELECT @@ROWCOUNT;

GO

DELETE FROM [Lookup].[RegistrationFees]
WHERE [Id] = 100;
SELECT @@ROWCOUNT;

GO

DELETE FROM [Lookup].[RegistrationFees]
WHERE [Id] = 101;
SELECT @@ROWCOUNT;

GO

DELETE FROM [Lookup].[RegistrationFees]
WHERE [Id] = 102;
SELECT @@ROWCOUNT;

GO

DELETE FROM [Lookup].[RegistrationFees]
WHERE [Id] = 103;
SELECT @@ROWCOUNT;

GO

DELETE FROM [Lookup].[RegistrationFees]
WHERE [Id] = 104;
SELECT @@ROWCOUNT;

GO

DELETE FROM [Lookup].[RegistrationFees]
WHERE [Id] = 105;
SELECT @@ROWCOUNT;

GO

DELETE FROM [Lookup].[RegistrationFees]
WHERE [Id] = 106;
SELECT @@ROWCOUNT;

GO

DELETE FROM [Lookup].[RegistrationFees]
WHERE [Id] = 107;
SELECT @@ROWCOUNT;

GO

DELETE FROM [Lookup].[RegistrationFees]
WHERE [Id] = 108;
SELECT @@ROWCOUNT;

GO

DELETE FROM [__EFMigrationsHistory]
WHERE [MigrationId] = N'20250509082826_SeedRegistrationFeesTablesReprocessorData';
GO

COMMIT;
GO

