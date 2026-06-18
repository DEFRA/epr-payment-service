using EPR.Payment.Service.Common.Data.DataModels;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationSubmission;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationSubmission;
using EPR.Payment.Service.Common.Services.Interfaces.RegistrationSubmission;
using EPR.Payment.Service.Services.RegistrationSubmission.Csv;
using EPR.Payment.Service.Services.RegistrationSubmission.Csv.Maps;
using EPR.Payment.Service.Services.RegistrationSubmission.Csv.Models;
using EPR.Payment.Service.Services.RegistrationSubmission.Storage;
using Microsoft.Extensions.Logging;

namespace EPR.Payment.Service.Services.RegistrationSubmission
{
    public class RegistrationSubmissionDataHandler : IRegistrationSubmissionDataHandler
    {
        private readonly IRegistrationSubmissionDataRepository _repository;
        private readonly IBlobReader _blobReader;
        private readonly ICsvStreamParser _csvStreamParser;
        private readonly TimeProvider _timeProvider;
        private readonly ILogger<RegistrationSubmissionDataHandler> _logger;

        public RegistrationSubmissionDataHandler(
            IRegistrationSubmissionDataRepository repository,
            IBlobReader blobReader,
            ICsvStreamParser csvStreamParser,
            TimeProvider timeProvider,
            ILogger<RegistrationSubmissionDataHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _blobReader = blobReader ?? throw new ArgumentNullException(nameof(blobReader));
            _csvStreamParser = csvStreamParser ?? throw new ArgumentNullException(nameof(csvStreamParser));
            _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Guid> HandleAsync(CreateRegistrationSubmissionDataRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            _logger.LogInformation(
                "Processing registration submission data for SubmissionId {SubmissionId} RegistrationBlobName {RegistrationBlobName} ComplianceSchemeId {ComplianceSchemeId}.",
                request.SubmissionId,
                request.RegistrationBlobName,
                request.ComplianceSchemeId);

            try
            {
                var existing = await _repository.GetByRegistrationBlobNameAsync(request.RegistrationBlobName, cancellationToken);
                if (existing is not null)
                {
                    return existing.Id;
                }

                var rows = await ReadCsvAsync(request.RegistrationBlobName, cancellationToken);

                var now = _timeProvider.GetUtcNow();
                var entity = BuildEntity(request, rows, now);

                return await _repository.CreateAsync(entity, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to process registration submission data for SubmissionId {SubmissionId} RegistrationBlobName {RegistrationBlobName}.",
                    request.SubmissionId,
                    request.RegistrationBlobName);
                throw;
            }
        }

        private async Task<List<RegistrationCsvRow>> ReadCsvAsync(string registrationBlobName, CancellationToken cancellationToken)
        {
            await using var stream = await _blobReader.DownloadAsync(registrationBlobName, cancellationToken);

            var rows = new List<RegistrationCsvRow>();
            await foreach (var row in _csvStreamParser.ParseAsync(stream, new RegistrationCsvRowMap(), cancellationToken))
            {
                rows.Add(row);
            }

            return rows;
        }

        private static RegistrationSubmissionData BuildEntity(
            CreateRegistrationSubmissionDataRequest request,
            IEnumerable<RegistrationCsvRow> rows,
            DateTimeOffset now)
        {
            var entity = new RegistrationSubmissionData
            {
                SubmissionId = request.SubmissionId,
                RegistrationBlobName = request.RegistrationBlobName,
                ComplianceSchemeId = request.ComplianceSchemeId,
                SubmissionPeriod = request.SubmissionPeriod,
                SubmissionDate = request.SubmissionDate,
                CreatedDate = now,
            };

            var groups = rows
                .Where(r => !string.IsNullOrWhiteSpace(r.OrganisationId))
                .GroupBy(r => r.OrganisationId.Trim(), StringComparer.OrdinalIgnoreCase);

            foreach (var group in groups)
            {
                var producerRow = group.FirstOrDefault(r => string.IsNullOrWhiteSpace(r.SubsidiaryId));
                if (producerRow is null)
                {
                    throw new InvalidOperationException(
                        $"No producer row found for OrganisationId '{group.Key}'.");
                }

                var producer = MapProducer(producerRow, now);

                foreach (var subsidiaryRow in group.Where(r => !string.IsNullOrWhiteSpace(r.SubsidiaryId)))
                {
                    producer.Subsidiaries.Add(MapSubsidiary(subsidiaryRow, now));
                }

                entity.Producers.Add(producer);
            }

            return entity;
        }

        private static RegistrationSubmissionProducer MapProducer(RegistrationCsvRow row, DateTimeOffset now) => new()
        {
            OrganisationId = row.OrganisationId.Trim(),
            OrganisationSize = MapOrganisationSize(row.OrganisationSize),
            NationId = MapNationId(row.HomeNationCode),
            IsOnlineMarketplace = IsOnlineMarketplace(row.PackagingActivityOm),
            IsClosedLoopRecycling = IsClosedLoopRecycling(row.ClosedLoopRegistration),
            IsNewJoiner = IsNewJoiner(row.JoinerDate),
            CreatedDate = now,
        };

        private static RegistrationSubmissionSubsidiary MapSubsidiary(RegistrationCsvRow row, DateTimeOffset now) => new()
        {
            SubsidiaryId = row.SubsidiaryId.Trim(),
            IsOnlineMarketplace = IsOnlineMarketplace(row.PackagingActivityOm),
            IsClosedLoopRecycling = IsClosedLoopRecycling(row.ClosedLoopRegistration),
            IsNewJoiner = IsNewJoiner(row.JoinerDate),
            CreatedDate = now,
        };

        private static int MapNationId(string? homeNationCode) => homeNationCode?.Trim().ToUpperInvariant() switch
        {
            "EN" => 1,
            "NI" => 2,
            "SC" => 3,
            "WS" or "WA" => 4,
            _ => 0,
        };

        // CSV contains single-letter codes (`L` / `S`) per OrganisationSizeCodes; downstream consumers
        // (facade fee validator, regulator service, etc.) expect the domain values "Large" / "Small".
        private static string MapOrganisationSize(string? organisationSize) => organisationSize?.Trim().ToUpperInvariant() switch
        {
            "L" or "LARGE" => "Large",
            "S" or "SMALL" => "Small",
            _ => organisationSize?.Trim() ?? string.Empty,
        };

        private static bool IsOnlineMarketplace(string? packagingActivityOm)
        {
            var value = packagingActivityOm?.Trim();
            return string.Equals(value, "Primary", StringComparison.OrdinalIgnoreCase)
                || string.Equals(value, "Secondary", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsClosedLoopRecycling(string? closedLoopRegistration) =>
            string.Equals(closedLoopRegistration?.Trim(), "YES", StringComparison.OrdinalIgnoreCase);

        private static bool IsNewJoiner(string? joinerDate) => !string.IsNullOrWhiteSpace(joinerDate);
    }
}
