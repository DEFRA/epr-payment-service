using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Data.DataModels.Migration;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationSubmission;

namespace EPR.Payment.Service.Services.Migration.RegistrationHistory
{
    /// <summary>
    /// Turns one submission's staged Synapse rows into the ordered handler calls the live Service Bus
    /// consumers would have made, or decides up front why the submission cannot be migrated.
    /// </summary>
    public static class RegistrationHistorySubmissionPlanner
    {
        public static SubmissionPlanResult Plan(
            IReadOnlyList<RegistrationHistoryStaging> rows,
            IReadOnlyList<SubmissionPeriod> periods,
            DateTime subsidiaryLateFeeEffectiveDate)
        {
            ArgumentNullException.ThrowIfNull(rows);
            ArgumentNullException.ThrowIfNull(periods);

            if (rows.Count == 0)
            {
                return SubmissionPlanResult.Failed(MigrationReasons.NoSubmittedFile, "No staged rows for submission.");
            }

            if (!Guid.TryParse(rows[0].SubmissionId, out var submissionId))
            {
                return SubmissionPlanResult.Failed(MigrationReasons.InvalidData, $"SubmissionId '{rows[0].SubmissionId}' is not a Guid.");
            }

            var ordered = rows
                .OrderBy(r => r.ReplayTs)
                .ThenBy(r => r.EventTypeOrder)
                .ThenBy(r => r.Id)
                .ToList();

            var submittedRows = ordered.Where(r => IsType(r, RegistrationHistoryEventTypes.Submitted)).ToList();
            if (submittedRows.Count == 0)
            {
                return SubmissionPlanResult.Failed(MigrationReasons.NoSubmittedFile, "No declared CompanyDetails file in the dataset.");
            }

            var first = submittedRows[0];
            Guid? complianceSchemeId = null;
            if (!string.IsNullOrWhiteSpace(first.ComplianceSchemeId))
            {
                if (!Guid.TryParse(first.ComplianceSchemeId, out var parsed))
                {
                    return SubmissionPlanResult.Failed(MigrationReasons.InvalidData, $"ComplianceSchemeId '{first.ComplianceSchemeId}' is not a Guid.");
                }

                complianceSchemeId = parsed;
            }

            var submissionPeriodId = SubmissionPeriodResolver.Resolve(first.SubmissionPeriod, first.RegistrationJourney, complianceSchemeId is not null, periods);
            if (submissionPeriodId is null)
            {
                return SubmissionPlanResult.NotRequired(
                    MigrationReasons.PeriodNotMapped,
                    $"No submission period for '{first.SubmissionPeriod}' / journey '{first.RegistrationJourney}' / compliance scheme {complianceSchemeId is not null}.");
            }

            var invalid = ordered.FirstOrDefault(r => r.ReplayTs is null || r.EventDate is null || string.IsNullOrWhiteSpace(r.FileId));
            if (invalid is not null)
            {
                return SubmissionPlanResult.Failed(MigrationReasons.InvalidData, $"{invalid.EventType} row {invalid.Id} is missing ReplayTs, EventDate or FileId.");
            }

            var postCutover = ordered.FirstOrDefault(r => IsType(r, RegistrationHistoryEventTypes.RegistrationApplicationSubmitted)
                                                          && r.EventDate!.Value >= subsidiaryLateFeeEffectiveDate);
            // Newly added subsidiaries are found by comparing against every earlier file, so once subsidiary
            // late fees apply the submission can only be replayed if all of its declared files are migrated.
            var migratedFiles = submittedRows.Select(r => r.FileId).Distinct(StringComparer.OrdinalIgnoreCase).Count();
            var totalFiles = ordered.Select(r => r.SubmissionFileCount).FirstOrDefault(c => c is not null);
            if (postCutover is not null && (totalFiles is null || migratedFiles < totalFiles))
            {
                return SubmissionPlanResult.Failed(
                    MigrationReasons.PostCutoverApproval,
                    $"Application submitted on {postCutover.EventDate:O} is on or after the subsidiary late-fee effective date {subsidiaryLateFeeEffectiveDate:O} and only {migratedFiles} of {totalFiles?.ToString() ?? "unknown"} declared files are migrated.");
            }

            var fallbackArn = ordered.Select(r => r.ApplicationReferenceNumber).FirstOrDefault(a => !string.IsNullOrWhiteSpace(a));
            var fileArns = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var warnings = new List<string>();
            var steps = new List<ReplayStep>();
            var seenEvents = new HashSet<(string FileId, string EventName, DateTime EventDate)>();
            var skipped = 0;

            foreach (var row in ordered)
            {
                var fileId = row.FileId!;
                var replayTs = AsUtc(row.ReplayTs!.Value);
                var eventDate = AsUtc(row.EventDate!.Value);

                if (IsType(row, RegistrationHistoryEventTypes.Submitted))
                {
                    if (string.IsNullOrWhiteSpace(row.BlobName))
                    {
                        return SubmissionPlanResult.Failed(MigrationReasons.BlobNotFound, $"File {fileId} has no BlobName.");
                    }

                    if (string.IsNullOrWhiteSpace(row.RegulatorNation))
                    {
                        return SubmissionPlanResult.Failed(MigrationReasons.NationMissing, $"File {fileId} has no regulator nation.");
                    }

                    var arn = FirstNonBlank(row.ApplicationReferenceNumber, ArnOfFirstApplicationSubmitted(ordered, fileId), fallbackArn);
                    if (arn is null)
                    {
                        return SubmissionPlanResult.Failed(MigrationReasons.ArnMissing, $"File {fileId} has no application reference number.");
                    }

                    fileArns[fileId] = arn;
                    steps.Add(new SubmittedStep(fileId, replayTs, new CreateRegistrationSubmissionDataRequest
                    {
                        SubmissionId = submissionId,
                        RegistrationBlobName = row.BlobName.Trim(),
                        ComplianceSchemeId = complianceSchemeId,
                        SubmissionDate = eventDate,
                        SubmissionPeriodId = submissionPeriodId.Value,
                        RegulatorNation = row.RegulatorNation.Trim(),
                        ApplicationReferenceNumber = arn,
                    }));
                    continue;
                }

                if (!fileArns.TryGetValue(fileId, out var fileArn))
                {
                    warnings.Add($"{row.EventType} at {replayTs:O} references file {fileId} that was not declared before it; skipped.");
                    skipped++;
                    continue;
                }

                if (IsType(row, RegistrationHistoryEventTypes.RegistrationApplicationSubmitted))
                {
                    if (!seenEvents.Add((fileId, Common.Constants.RegistrationEventNames.SubmittedForRegulatorApproval, eventDate)))
                    {
                        skipped++;
                        continue;
                    }

                    steps.Add(new ApplicationSubmittedStep(fileId, replayTs, new RegistrationSubmittedForRegulatorApprovalRequest
                    {
                        SubmissionId = submissionId,
                        ApplicationReferenceNumber = FirstNonBlank(row.ApplicationReferenceNumber, fileArn)!,
                        SubmissionDate = eventDate,
                    }));
                    continue;
                }

                if (IsType(row, RegistrationHistoryEventTypes.RegulatorRegistrationDecision))
                {
                    if (!RegulatorDecisionMapper.TryMap(row.Decision, out var eventName))
                    {
                        warnings.Add($"Decision '{row.Decision}' at {replayTs:O} on file {fileId} has no payment-service event; skipped.");
                        skipped++;
                        continue;
                    }

                    // A duplicate (row, event, date) would make the event repository leave a failed insert tracked.
                    if (!seenEvents.Add((fileId, eventName, eventDate)))
                    {
                        skipped++;
                        continue;
                    }

                    steps.Add(new DecisionStep(fileId, replayTs, new RegulatorRegistrationDecisionRequest
                    {
                        SubmissionId = submissionId,
                        EventName = eventName,
                        DecisionDate = eventDate,
                    }));
                    continue;
                }

                warnings.Add($"Unknown event type '{row.EventType}' at {replayTs:O}; skipped.");
                skipped++;
            }

            return SubmissionPlanResult.Replay(new SubmissionReplayPlan(submissionId, steps, warnings, skipped));
        }

        private static bool IsType(RegistrationHistoryStaging row, string eventType) =>
            string.Equals(row.EventType, eventType, StringComparison.OrdinalIgnoreCase);

        private static string? ArnOfFirstApplicationSubmitted(IEnumerable<RegistrationHistoryStaging> rows, string fileId) =>
            rows.Where(r => IsType(r, RegistrationHistoryEventTypes.RegistrationApplicationSubmitted)
                            && string.Equals(r.FileId, fileId, StringComparison.OrdinalIgnoreCase))
                .Select(r => r.ApplicationReferenceNumber)
                .FirstOrDefault(a => !string.IsNullOrWhiteSpace(a));

        private static string? FirstNonBlank(params string?[] values) =>
            values.FirstOrDefault(v => !string.IsNullOrWhiteSpace(v))?.Trim();

        private static DateTime AsUtc(DateTime value) => DateTime.SpecifyKind(value, DateTimeKind.Utc);
    }
}
