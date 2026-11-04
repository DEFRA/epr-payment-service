/*
    Manual rollback of the registration history migration: restores the [registration] schema and the payment links
    from [registration_pre_migration], which the migration fills (Scripts/Migration/backup-registration-pre-migration.sql)
    immediately before it clears [registration]. Each new run replaces that backup, so roll back before starting another run.

    BEFORE RUNNING: stop the migration from running again - set Migration:RegistrationHistory:Enabled to false
    (a run left in Running status resumes on startup and would replay on top of the restored data).

    Steps (one transaction; any error or count mismatch rolls everything back):
      1. Lists rows that exist now but not in the backup (e.g. captured live since the backup) - these are removed.
      2. Unlinks every payment from the current RegistrationSubmissionData rows (FK is NO ACTION).
      3. Deletes [registration].[RegistrationSubmissionData]; producers, subsidiaries, events, fee snapshots and
         line items go with it via ON DELETE CASCADE.
      4. Re-inserts every table from [registration_pre_migration] in foreign key order (columns common to both).
      5. Re-links the payments recorded in [registration_pre_migration].[payments_unlinked].
      6. Checks every table and the payment links against the backup.

    @Execute = 0 (default) runs everything and then rolls back, so you can review the output first.
    Set @Execute = 1 to commit.
*/
DECLARE @Execute BIT = 0;

SET XACT_ABORT ON;
SET NOCOUNT ON;

-- Tables in foreign key order (parents first)
DECLARE @Tables TABLE ([Seq] INT NOT NULL, [Name] SYSNAME NOT NULL);
INSERT INTO @Tables ([Seq], [Name]) VALUES
    (1, N'RegistrationSubmissionData'),
    (2, N'RegistrationSubmissionProducer'),
    (3, N'RegistrationSubmissionSubsidiary'),
    (4, N'RegistrationSubmissionDataEvents'),
    (5, N'RegistrationFeeSnapshot'),
    (6, N'RegistrationFeeLineItem');

-- Pre-checks: the backup is complete and no [registration] table is unaccounted for
IF EXISTS (SELECT 1 FROM @Tables t WHERE OBJECT_ID(N'[registration_pre_migration].' + QUOTENAME(t.[Name])) IS NULL)
   OR OBJECT_ID(N'[registration_pre_migration].[payments_unlinked]') IS NULL
    THROW 50000, N'The [registration_pre_migration] backup is incomplete; nothing has been changed.', 1;

IF EXISTS (SELECT 1 FROM sys.tables st
           WHERE st.schema_id = SCHEMA_ID(N'registration')
             AND st.name NOT IN (SELECT [Name] FROM @Tables))
    THROW 50000, N'[registration] has a table this script does not restore; add it to @Tables in foreign key order.', 1;

BEGIN TRANSACTION;

-- 1. Rows that will be lost (present now, not in the backup)
SELECT n.[SubmissionId], n.[RegistrationBlobName], n.[ApplicationReferenceNumber], n.[CreatedDate]
FROM [registration].[RegistrationSubmissionData] n
WHERE NOT EXISTS (SELECT 1 FROM [registration_pre_migration].[RegistrationSubmissionData] b
                  WHERE b.[RegistrationBlobName] = n.[RegistrationBlobName])
ORDER BY n.[CreatedDate];

-- 2. Unlink payments from the current rows
UPDATE [dbo].[Payment]
SET [RegistrationSubmissionDataId] = NULL
WHERE [RegistrationSubmissionDataId] IS NOT NULL;
PRINT CONCAT('Unlinked ', @@ROWCOUNT, ' payments from the current registration data.');

-- 3. Clear the schema (cascades to the child tables)
DELETE FROM [registration].[RegistrationSubmissionData];
PRINT CONCAT('Deleted ', @@ROWCOUNT, ' RegistrationSubmissionData rows.');

-- 4. Restore from the backup, parents first
DECLARE @Name SYSNAME, @Cols NVARCHAR(MAX), @Sql NVARCHAR(MAX), @HasIdentity BIT;
DECLARE table_cursor CURSOR LOCAL FAST_FORWARD FOR SELECT [Name] FROM @Tables ORDER BY [Seq];
OPEN table_cursor;
FETCH NEXT FROM table_cursor INTO @Name;
WHILE @@FETCH_STATUS = 0
BEGIN
    SELECT @Cols = STRING_AGG(CAST(QUOTENAME(c.name) AS NVARCHAR(MAX)), N', ') WITHIN GROUP (ORDER BY c.column_id)
    FROM sys.columns c
    WHERE c.object_id = OBJECT_ID(N'[registration].' + QUOTENAME(@Name))
      AND c.is_computed = 0
      AND EXISTS (SELECT 1 FROM sys.columns b
                  WHERE b.object_id = OBJECT_ID(N'[registration_pre_migration].' + QUOTENAME(@Name))
                    AND b.name = c.name);

    SET @HasIdentity = CASE WHEN EXISTS (SELECT 1 FROM sys.identity_columns
                                         WHERE object_id = OBJECT_ID(N'[registration].' + QUOTENAME(@Name)))
                            THEN 1 ELSE 0 END;

    SET @Sql = CASE WHEN @HasIdentity = 1 THEN N'SET IDENTITY_INSERT [registration].' + QUOTENAME(@Name) + N' ON; ' ELSE N'' END
             + N'INSERT INTO [registration].' + QUOTENAME(@Name) + N' (' + @Cols + N') '
             + N'SELECT ' + @Cols + N' FROM [registration_pre_migration].' + QUOTENAME(@Name) + N'; '
             + N'PRINT CONCAT(''Restored '', @@ROWCOUNT, '' rows into ' + @Name + N'.''); '
             + CASE WHEN @HasIdentity = 1 THEN N'SET IDENTITY_INSERT [registration].' + QUOTENAME(@Name) + N' OFF;' ELSE N'' END;

    EXEC sp_executesql @Sql;

    FETCH NEXT FROM table_cursor INTO @Name;
END
CLOSE table_cursor;
DEALLOCATE table_cursor;

-- 5. Re-link the payments that were linked at backup time
UPDATE p
SET p.[RegistrationSubmissionDataId] = u.[RegistrationSubmissionDataId]
FROM [dbo].[Payment] p
INNER JOIN [registration_pre_migration].[payments_unlinked] u ON u.[PaymentId] = p.[Id];
PRINT CONCAT('Re-linked ', @@ROWCOUNT, ' payments.');

-- 6. Verify against the backup
DECLARE @Check NVARCHAR(MAX) = N'';
SELECT @Check += CASE WHEN @Check = N'' THEN N'' ELSE N' UNION ALL ' END
               + N'SELECT N''' + t.[Name] + N''' AS [Table], '
               + N'(SELECT COUNT_BIG(*) FROM [registration_pre_migration].' + QUOTENAME(t.[Name]) + N') AS [Backup], '
               + N'(SELECT COUNT_BIG(*) FROM [registration].' + QUOTENAME(t.[Name]) + N') AS [Restored]'
FROM @Tables t;

SET @Check += N' UNION ALL SELECT N''payments_unlinked'', '
            + N'(SELECT COUNT_BIG(*) FROM [registration_pre_migration].[payments_unlinked]), '
            + N'(SELECT COUNT_BIG(*) FROM [dbo].[Payment] p INNER JOIN [registration_pre_migration].[payments_unlinked] u '
            + N'ON u.[PaymentId] = p.[Id] AND p.[RegistrationSubmissionDataId] = u.[RegistrationSubmissionDataId])';

DECLARE @Results TABLE ([Table] SYSNAME, [Backup] BIGINT, [Restored] BIGINT);
INSERT INTO @Results EXEC sp_executesql @Check;

SELECT *, CASE WHEN [Backup] = [Restored] THEN N'OK' ELSE N'MISMATCH' END AS [Check]
FROM @Results
ORDER BY [Table];

IF EXISTS (SELECT 1 FROM @Results WHERE [Backup] <> [Restored])
BEGIN
    ROLLBACK TRANSACTION;
    THROW 50000, N'Restored counts do not match the backup; everything has been rolled back.', 1;
END

IF @Execute = 1
BEGIN
    COMMIT TRANSACTION;
    PRINT 'Rollback committed: [registration] and payment links restored from [registration_pre_migration].';
END
ELSE
BEGIN
    ROLLBACK TRANSACTION;
    PRINT 'Dry run (@Execute = 0): all changes rolled back. Review the output, then set @Execute = 1 to commit.';
END
