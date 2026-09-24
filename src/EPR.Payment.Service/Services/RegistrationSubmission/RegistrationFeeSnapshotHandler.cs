using EPR.Payment.Service.Common.Data.DataModels;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationSubmission;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.Producer;
using EPR.Payment.Service.Common.Enums;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.Producer;
using EPR.Payment.Service.Services.Interfaces.RegistrationSubmission;
using Microsoft.Extensions.Logging;

namespace EPR.Payment.Service.Services.RegistrationSubmission
{
    public class RegistrationFeeSnapshotHandler : IRegistrationFeeSnapshotHandler
    {
        private readonly IRegistrationFeeSnapshotRepository _snapshotRepository;
        private readonly IProducerFeesCalculatorService _producerCalculator;
        private readonly IComplianceSchemeCalculatorService _complianceSchemeCalculator;
        private readonly TimeProvider _timeProvider;
        private readonly ILogger<RegistrationFeeSnapshotHandler> _logger;

        public RegistrationFeeSnapshotHandler(
            IRegistrationFeeSnapshotRepository snapshotRepository,
            IProducerFeesCalculatorService producerCalculator,
            IComplianceSchemeCalculatorService complianceSchemeCalculator,
            TimeProvider timeProvider,
            ILogger<RegistrationFeeSnapshotHandler> logger)
        {
            _snapshotRepository = snapshotRepository ?? throw new ArgumentNullException(nameof(snapshotRepository));
            _producerCalculator = producerCalculator ?? throw new ArgumentNullException(nameof(producerCalculator));
            _complianceSchemeCalculator = complianceSchemeCalculator ?? throw new ArgumentNullException(nameof(complianceSchemeCalculator));
            _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task HandleAsync(
            RegistrationSubmissionData latestRecord,
            IReadOnlyList<RegistrationSubmissionData> allRecords,
            DateTime submissionDate,
            SubmissionLifecycle lifecycle,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(latestRecord);
            ArgumentNullException.ThrowIfNull(allRecords);
            ArgumentNullException.ThrowIfNull(lifecycle);

            var existing = await _snapshotRepository.GetByRegistrationSubmissionDataIdAsync(latestRecord.Id, cancellationToken);
            if (existing is not null)
            {
                _logger.LogInformation(
                    "RegistrationFeeSnapshot already exists for RegistrationSubmissionDataId {RegistrationSubmissionDataId}; skipping.",
                    latestRecord.Id);
                return;
            }

            if (latestRecord.Producers.Count == 0)
            {
                _logger.LogWarning(
                    "RegistrationSubmissionData {RegistrationSubmissionDataId} has no producers; skipping fee snapshot creation.",
                    latestRecord.Id);
                return;
            }

            var snapshot = new RegistrationFeeSnapshot
            {
                RegistrationSubmissionDataId = latestRecord.Id,
                CreatedDate = _timeProvider.GetUtcNow(),
            };

            var today = _timeProvider.GetUtcNow().UtcDateTime;
            var newlyAddedSubs = NewlyAddedSubsidiaryFinder.Find(
                latestRecord,
                allRecords,
                latestRecord.SubmissionPeriodWindow.DeadlineDate);

            if (latestRecord.ComplianceSchemeId is null)
            {
                var producer = latestRecord.Producers.First();
                var request = RegistrationFeeRequestBuilder.BuildProducerRequest(latestRecord, producer, lifecycle, today, newlyAddedSubs);
                var response = await _producerCalculator.CalculateFeesAsync(request, cancellationToken);

                snapshot.TotalFee = response.TotalFee;
                AddProducerLineItems(snapshot, response);
            }
            else
            {
                var request = RegistrationFeeRequestBuilder.BuildComplianceSchemeRequest(latestRecord, lifecycle, today, newlyAddedSubs);
                var response = await _complianceSchemeCalculator.CalculateFeesAsync(request, cancellationToken);

                snapshot.TotalFee = response.TotalFee;
                AddComplianceSchemeLineItems(snapshot, response);
            }

            await _snapshotRepository.CreateAsync(snapshot, cancellationToken);

            _logger.LogInformation(
                "Created RegistrationFeeSnapshot for RegistrationSubmissionDataId {RegistrationSubmissionDataId} with {LineItemCount} line items and TotalFee {TotalFee} (submissionDate {SubmissionDate}).",
                latestRecord.Id,
                snapshot.LineItems.Count,
                snapshot.TotalFee,
                submissionDate);
        }

        private static void AddProducerLineItems(RegistrationFeeSnapshot snapshot, RegistrationFeesResponseDto response)
        {
            AddIfPositive(snapshot, FeeTypeIds.ProducerRegistrationFee, response.ProducerRegistrationFee);
            AddIfPositive(snapshot, FeeTypeIds.ProducerOnlineMarketplaceFee, response.ProducerOnlineMarketPlaceFee);
            AddIfPositive(snapshot, FeeTypeIds.ProducerClosedLoopRecyclingFee, response.ProducerClosedLoopRecyclingFee);
            AddIfPositive(snapshot, FeeTypeIds.ProducerLateRegistrationFee, response.ProducerLateRegistrationFee);
            AddSubsidiaryLineItems(snapshot, response.SubsidiariesFeeBreakdown, memberId: null);
        }

        private static void AddComplianceSchemeLineItems(RegistrationFeeSnapshot snapshot, ComplianceSchemeFeesResponseDto response)
        {
            AddIfPositive(snapshot, FeeTypeIds.ComplianceSchemeRegistrationFee, response.ComplianceSchemeRegistrationFee);

            foreach (var member in response.ComplianceSchemeMembersWithFees)
            {
                AddIfPositive(snapshot, FeeTypeIds.MemberRegistrationFee, member.MemberRegistrationFee, member.MemberId);
                AddIfPositive(snapshot, FeeTypeIds.MemberOnlineMarketplaceFee, member.MemberOnlineMarketPlaceFee, member.MemberId);
                AddIfPositive(snapshot, FeeTypeIds.MemberClosedLoopRecyclingFee, member.MemberClosedLoopRecyclingFee, member.MemberId);
                AddIfPositive(snapshot, FeeTypeIds.MemberLateRegistrationFee, member.MemberLateRegistrationFee, member.MemberId);
                AddSubsidiaryLineItems(snapshot, member.SubsidiariesFeeBreakdown, member.MemberId);
            }
        }

        private static void AddSubsidiaryLineItems(
            RegistrationFeeSnapshot snapshot,
            Common.Dtos.Response.RegistrationFees.SubsidiariesFeeBreakdown breakdown,
            string? memberId)
        {
            if (breakdown is null)
            {
                return;
            }

            foreach (var band in breakdown.FeeBreakdowns)
            {
                if (band.UnitCount <= 0 && band.TotalPrice <= 0m)
                {
                    continue;
                }

                snapshot.LineItems.Add(new RegistrationFeeLineItem
                {
                    FeeTypeId = FeeTypeIds.SubsidiaryFee,
                    FeeTypeName = nameof(FeeTypeIds.SubsidiaryFee),
                    UnitPrice = band.UnitPrice,
                    Quantity = band.UnitCount,
                    Amount = band.TotalPrice,
                    MemberId = memberId,
                    BandNumber = band.BandNumber,
                });
            }

            if (breakdown.TotalSubsidiariesOMPFees > 0)
            {
                snapshot.LineItems.Add(new RegistrationFeeLineItem
                {
                    FeeTypeId = FeeTypeIds.SubsidiaryOnlineMarketplaceFee,
                    FeeTypeName = nameof(FeeTypeIds.SubsidiaryOnlineMarketplaceFee),
                    UnitPrice = breakdown.UnitOMPFees,
                    Quantity = breakdown.CountOfOMPSubsidiaries,
                    Amount = breakdown.TotalSubsidiariesOMPFees,
                    MemberId = memberId,
                });
            }

            if (breakdown.TotalSubsidiariesClosedLoopRecyclingFees > 0)
            {
                snapshot.LineItems.Add(new RegistrationFeeLineItem
                {
                    FeeTypeId = FeeTypeIds.SubsidiaryClosedLoopRecyclingFee,
                    FeeTypeName = nameof(FeeTypeIds.SubsidiaryClosedLoopRecyclingFee),
                    UnitPrice = breakdown.UnitClosedLoopRecyclingFees,
                    Quantity = breakdown.CountOfClosedLoopRecyclingSubsidiaries,
                    Amount = breakdown.TotalSubsidiariesClosedLoopRecyclingFees,
                    MemberId = memberId,
                });
            }

            if (breakdown.TotalSubsidiariesLateFees > 0)
            {
                snapshot.LineItems.Add(new RegistrationFeeLineItem
                {
                    FeeTypeId = FeeTypeIds.SubsidiaryLateFee,
                    FeeTypeName = nameof(FeeTypeIds.SubsidiaryLateFee),
                    UnitPrice = breakdown.UnitSubsidiaryLateFee,
                    Quantity = breakdown.CountOfLateSubsidiaries,
                    Amount = breakdown.TotalSubsidiariesLateFees,
                    MemberId = memberId,
                });
            }
        }

        private static void AddIfPositive(RegistrationFeeSnapshot snapshot, FeeTypeIds feeTypeId, decimal amount, string? memberId = null)
        {
            if (amount <= 0m)
            {
                return;
            }

            snapshot.LineItems.Add(new RegistrationFeeLineItem
            {
                FeeTypeId = feeTypeId,
                FeeTypeName = feeTypeId.ToString(),
                Amount = amount,
                MemberId = memberId,
            });
        }
    }
}
