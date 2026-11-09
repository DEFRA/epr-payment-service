/*
    Exclude submissions from the registration history migration as known issues (e.g. bad source data).
    Idempotent: a SubmissionId already excluded is left as it is.

    Every later run records an excluded submission in [migration].[RegistrationSubmissionMigration] with
    Status 'Excluded', Reason 'KnownIssue' and Error '<Reason>: <Note>'. It is not replayed, and it does not
    count as Failed, so it does not trigger clear-down or CompletedWithErrors.

    Exclusions are read when the run processes its pending submissions:
      - a finished run is not re-run, so deploy/restart with a new Migration:RegistrationHistory:RunKey;
      - a run that is still Running (interrupted) picks them up for submissions that are still Pending.

    Usage: add one row per submission to @Exclusions (Reason is usually the migration Reason it failed with).
*/
DECLARE @Exclusions TABLE ([SubmissionId] NVARCHAR(100) NOT NULL, [Reason] NVARCHAR(50) NOT NULL, [Note] NVARCHAR(1000) NULL);

INSERT INTO @Exclusions ([SubmissionId], [Reason], [Note])
VALUES
    (N'00000000-0000-0000-0000-000000000000', N'CsvInvalid', N'Describe why this submission cannot be migrated.');

SET XACT_ABORT ON;
BEGIN TRANSACTION;

INSERT INTO [migration].[ExcludedSubmission] ([SubmissionId], [Reason], [Note])
SELECT LOWER(LTRIM(RTRIM(e.[SubmissionId]))), e.[Reason], e.[Note]
FROM @Exclusions e
WHERE NOT EXISTS (SELECT 1 FROM [migration].[ExcludedSubmission] x WHERE x.[SubmissionId] = LOWER(LTRIM(RTRIM(e.[SubmissionId]))));

PRINT CONCAT('Excluded ', @@ROWCOUNT, ' new submissions.');

COMMIT TRANSACTION;

SELECT [SubmissionId], [Reason], [Note], [CreatedAt]
FROM [migration].[ExcludedSubmission]
ORDER BY [CreatedAt], [SubmissionId];
