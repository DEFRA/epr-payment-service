/*
    Manual clear-down of data created by the registration history migration (dev/test, or prod by DBA).
    Idempotent. Deletes only RegistrationSubmissionData rows recorded in [migration].[MigratedEntity] for the run;
    producers, subsidiaries, events, fee snapshots and line items go with them via ON DELETE CASCADE.
    Payments linked to those rows are unlinked first (FK is NO ACTION).
    The [migration] control, staging and log tables are kept for diagnosis.
    [migration].[ExcludedSubmission] is not run-scoped and is deliberately left alone.

    To run the migration again afterwards, deploy with a new Migration:RegistrationHistory:RunKey
    (or delete the [migration].[MigrationRun] row for @RunKey, which cascades its staging/status/audit rows).
*/
DECLARE @RunKey NVARCHAR(100) = N'registration-history-v1';

SET XACT_ABORT ON;
BEGIN TRANSACTION;

DECLARE @RunId UNIQUEIDENTIFIER = (SELECT [Id] FROM [migration].[MigrationRun] WHERE [RunKey] = @RunKey);

IF @RunId IS NULL
BEGIN
    PRINT CONCAT('No migration run found for RunKey ', @RunKey, '; nothing to clear down.');
END
ELSE
BEGIN
    SELECT p.[Id] AS UnlinkedPaymentId, p.[Reference], p.[RegistrationSubmissionDataId]
    FROM [dbo].[Payment] p
    WHERE p.[RegistrationSubmissionDataId] IN (SELECT [RegistrationSubmissionDataId] FROM [migration].[MigratedEntity] WHERE [RunId] = @RunId);

    UPDATE p
    SET p.[RegistrationSubmissionDataId] = NULL
    FROM [dbo].[Payment] p
    WHERE p.[RegistrationSubmissionDataId] IN (SELECT [RegistrationSubmissionDataId] FROM [migration].[MigratedEntity] WHERE [RunId] = @RunId);

    DELETE r
    FROM [registration].[RegistrationSubmissionData] r
    WHERE r.[Id] IN (SELECT [RegistrationSubmissionDataId] FROM [migration].[MigratedEntity] WHERE [RunId] = @RunId);

    PRINT CONCAT('Deleted ', @@ROWCOUNT, ' RegistrationSubmissionData rows for RunKey ', @RunKey, '.');

    UPDATE [migration].[RegistrationSubmissionMigration]
    SET [Status] = 'ClearedDown'
    WHERE [RunId] = @RunId AND [Status] = 'Succeeded';

    UPDATE [migration].[MigrationRun]
    SET [ClearDownApplied] = 1,
        [Status] = CASE WHEN [Status] = 'Running' THEN [Status] ELSE 'ClearedDown' END
    WHERE [Id] = @RunId;
END

COMMIT TRANSACTION;
