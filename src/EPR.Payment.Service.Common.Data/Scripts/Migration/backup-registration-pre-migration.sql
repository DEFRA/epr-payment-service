/*
    Backup of the [registration] schema taken before the registration history migration clears it.
    Run automatically by the migration (RegistrationHistoryMigrationRepository.BackupAndClearRegistrationDataAsync),
    in the same transaction as the clear-down, every time a new run has staged its dataset. Can also be run by hand.

    Recreates [registration_pre_migration] with:
      - a copy of every table in [registration] (same name, columns and data; no keys or indexes);
      - [payments_unlinked]: every [dbo].[Payment] currently linked to a RegistrationSubmissionData row,
        i.e. the payments the clear-down unlinks.

    Any tables already in [registration_pre_migration] (an earlier backup) are dropped first.
    Fails, rolling everything back, if a copied row count does not match its source.
    Restore with Scripts/Migration/rollback-registration-migration.sql.
*/
SET XACT_ABORT ON;
SET NOCOUNT ON;

IF SCHEMA_ID(N'registration_pre_migration') IS NULL
    EXEC (N'CREATE SCHEMA [registration_pre_migration]');

BEGIN TRANSACTION;

-- Drop an earlier backup
DECLARE @drop NVARCHAR(MAX) = N'';
SELECT @drop += N'DROP TABLE [registration_pre_migration].' + QUOTENAME(t.name) + N';' + CHAR(10)
FROM sys.tables t
WHERE t.schema_id = SCHEMA_ID(N'registration_pre_migration');

IF @drop <> N''
    EXEC sp_executesql @drop;

-- Copy every table in [registration]
DECLARE @copy NVARCHAR(MAX) = N'';
SELECT @copy += N'SELECT * INTO [registration_pre_migration].' + QUOTENAME(t.name)
              + N' FROM [registration].' + QUOTENAME(t.name) + N';' + CHAR(10)
FROM sys.tables t
WHERE t.schema_id = SCHEMA_ID(N'registration');

EXEC sp_executesql @copy;

-- Payments the clear-down unlinks
SELECT p.[Id] AS [PaymentId],
       p.[ExternalPaymentId],
       p.[Reference],
       p.[Amount],
       p.[ReasonForPayment],
       p.[Regulator],
       p.[InternalStatusId],
       p.[CreatedDate],
       p.[UpdatedDate],
       p.[RegistrationSubmissionDataId],
       SYSUTCDATETIME() AS [BackedUpAt]
INTO [registration_pre_migration].[payments_unlinked]
FROM [dbo].[Payment] p
WHERE p.[RegistrationSubmissionDataId] IS NOT NULL;

-- Verify: every row count must match its source
DECLARE @check NVARCHAR(MAX) = N'';
SELECT @check += CASE WHEN @check = N'' THEN N'' ELSE N' UNION ALL ' END
               + N'SELECT N''' + t.name + N''' AS [Table], '
               + N'(SELECT COUNT_BIG(*) FROM [registration].' + QUOTENAME(t.name) + N') AS [Source], '
               + N'(SELECT COUNT_BIG(*) FROM [registration_pre_migration].' + QUOTENAME(t.name) + N') AS [Backup]'
FROM sys.tables t
WHERE t.schema_id = SCHEMA_ID(N'registration');

SET @check += CASE WHEN @check = N'' THEN N'' ELSE N' UNION ALL ' END
            + N'SELECT N''payments_unlinked'', '
            + N'(SELECT COUNT_BIG(*) FROM [dbo].[Payment] WHERE [RegistrationSubmissionDataId] IS NOT NULL), '
            + N'(SELECT COUNT_BIG(*) FROM [registration_pre_migration].[payments_unlinked])';

DECLARE @counts TABLE ([Table] SYSNAME NOT NULL, [Source] BIGINT NOT NULL, [Backup] BIGINT NOT NULL);
INSERT INTO @counts EXEC sp_executesql @check;

IF EXISTS (SELECT 1 FROM @counts WHERE [Source] <> [Backup])
    THROW 50000, N'[registration_pre_migration] backup row counts do not match [registration]; nothing has been changed.', 1;

COMMIT TRANSACTION;

SELECT [Table], [Source], [Backup] FROM @counts ORDER BY [Table];
