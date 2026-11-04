/*
    Backup of the [registration] schema taken before the registration history migration clears it.
    Run automatically by the migration (RegistrationHistoryMigrationRepository.BackupAndClearRegistrationDataAsync),
    in the same transaction as the clear-down, every time a new run has staged its dataset. Can also be run by hand.

    Data changes only: the [registration_pre_migration] schema and tables are created by the EF Core migration
    20261104151355_SUB-365-add-registration-pre-migration-schema (run by the pipeline), because the service's login
    cannot create objects. It needs SELECT on [registration] and [dbo].[Payment], and SELECT, INSERT and DELETE on
    [registration_pre_migration].

    Steps:
      - empties every [registration_pre_migration] table (an earlier backup);
      - copies every [registration] table into its namesake (columns common to both);
      - fills [payments_unlinked] with every [dbo].[Payment] linked to a RegistrationSubmissionData row,
        i.e. the payments the clear-down unlinks;
      - fails, rolling everything back, if a backup table is missing or a row count does not match its source.
    Restore with Scripts/Migration/rollback-registration-migration.sql.
*/
SET XACT_ABORT ON;
SET NOCOUNT ON;

IF OBJECT_ID(N'[registration_pre_migration].[payments_unlinked]') IS NULL
   OR EXISTS (SELECT 1 FROM sys.tables t
              WHERE t.schema_id = SCHEMA_ID(N'registration')
                AND OBJECT_ID(N'[registration_pre_migration].' + QUOTENAME(t.name)) IS NULL)
    THROW 50000, N'[registration_pre_migration] is missing a backup table; apply the EF Core migrations first. Nothing has been changed.', 1;

BEGIN TRANSACTION;

-- Empty the earlier backup
DECLARE @empty NVARCHAR(MAX) = N'';
SELECT @empty += N'DELETE FROM [registration_pre_migration].' + QUOTENAME(t.name) + N';' + CHAR(10)
FROM sys.tables t
WHERE t.schema_id = SCHEMA_ID(N'registration_pre_migration');

EXEC sp_executesql @empty;

-- Copy every table in [registration]
DECLARE @copy NVARCHAR(MAX) = N'';
SELECT @copy += N'INSERT INTO [registration_pre_migration].' + QUOTENAME(t.name) + N' (' + c.Cols + N') '
              + N'SELECT ' + c.Cols + N' FROM [registration].' + QUOTENAME(t.name) + N';' + CHAR(10)
FROM sys.tables t
CROSS APPLY (
    SELECT STRING_AGG(CAST(QUOTENAME(sc.name) AS NVARCHAR(MAX)), N', ') WITHIN GROUP (ORDER BY sc.column_id) AS Cols
    FROM sys.columns sc
    WHERE sc.object_id = t.object_id
      AND sc.is_computed = 0
      AND EXISTS (SELECT 1 FROM sys.columns bc
                  WHERE bc.object_id = OBJECT_ID(N'[registration_pre_migration].' + QUOTENAME(t.name))
                    AND bc.name = sc.name)
) c
WHERE t.schema_id = SCHEMA_ID(N'registration');

EXEC sp_executesql @copy;

-- Payments the clear-down unlinks
INSERT INTO [registration_pre_migration].[payments_unlinked]
       ([PaymentId], [ExternalPaymentId], [Reference], [Amount], [ReasonForPayment], [Regulator],
        [InternalStatusId], [CreatedDate], [UpdatedDate], [RegistrationSubmissionDataId], [BackedUpAt])
SELECT p.[Id], p.[ExternalPaymentId], p.[Reference], p.[Amount], p.[ReasonForPayment], p.[Regulator],
       p.[InternalStatusId], p.[CreatedDate], p.[UpdatedDate], p.[RegistrationSubmissionDataId], SYSUTCDATETIME()
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
