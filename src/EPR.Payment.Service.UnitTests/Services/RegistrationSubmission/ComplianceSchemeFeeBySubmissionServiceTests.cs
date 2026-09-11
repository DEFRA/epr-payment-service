using EPR.Payment.Service.Common.Constants;
using EPR.Payment.Service.Common.Data.DataModels;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationSubmission;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Enums;
using EPR.Payment.Service.Services.Interfaces.Payments;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Services.RegistrationSubmission;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPR.Payment.Service.UnitTests.Services.RegistrationSubmission
{
    [TestClass]
    public class ComplianceSchemeFeeBySubmissionServiceTests
    {
        private static readonly DateTime Today = new(2026, 7, 24, 0, 0, 0, DateTimeKind.Utc);
        private static readonly DateTime Deadline = new(2026, 4, 1, 0, 0, 0, DateTimeKind.Utc);

        private Mock<IRegistrationSubmissionDataRepository> _repositoryMock = null!;
        private Mock<IRegistrationFeeSnapshotRepository> _snapshotRepositoryMock = null!;
        private Mock<IComplianceSchemeCalculatorService> _calculatorMock = null!;
        private Mock<IPaymentsService> _paymentsServiceMock = null!;
        private Mock<TimeProvider> _timeProviderMock = null!;
        private Mock<ILogger<ComplianceSchemeFeeBySubmissionService>> _loggerMock = null!;
        private ComplianceSchemeFeeBySubmissionService _sut = null!;
        private ComplianceSchemeFeesResponseDto _calculatorResponse = null!;
        private ComplianceSchemeFeesRequestDto? _capturedRequest;

        [TestInitialize]
        public void Init()
        {
            _repositoryMock = new Mock<IRegistrationSubmissionDataRepository>();
            _snapshotRepositoryMock = new Mock<IRegistrationFeeSnapshotRepository>();
            _calculatorMock = new Mock<IComplianceSchemeCalculatorService>();
            _paymentsServiceMock = new Mock<IPaymentsService>();
            _timeProviderMock = new Mock<TimeProvider>();
            _timeProviderMock.Setup(t => t.GetUtcNow()).Returns(new DateTimeOffset(Today, TimeSpan.Zero));
            _loggerMock = new Mock<ILogger<ComplianceSchemeFeeBySubmissionService>>();
            _calculatorResponse = new ComplianceSchemeFeesResponseDto();
            _capturedRequest = null;
            _calculatorMock
                .Setup(c => c.CalculateFeesAsync(It.IsAny<ComplianceSchemeFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .Callback<ComplianceSchemeFeesRequestDto, CancellationToken>((r, _) => _capturedRequest = r)
                .ReturnsAsync(_calculatorResponse);

            _sut = new ComplianceSchemeFeeBySubmissionService(
                _repositoryMock.Object,
                _snapshotRepositoryMock.Object,
                _calculatorMock.Object,
                _paymentsServiceMock.Object,
                _timeProviderMock.Object,
                _loggerMock.Object);
        }

        [TestMethod]
        public void Constructor_NullDependency_Throws()
        {
            using (new AssertionScope())
            {
                ((Action)(() => new ComplianceSchemeFeeBySubmissionService(null!, _snapshotRepositoryMock.Object, _calculatorMock.Object, _paymentsServiceMock.Object, _timeProviderMock.Object, _loggerMock.Object)))
                    .Should().Throw<ArgumentNullException>();
                ((Action)(() => new ComplianceSchemeFeeBySubmissionService(_repositoryMock.Object, null!, _calculatorMock.Object, _paymentsServiceMock.Object, _timeProviderMock.Object, _loggerMock.Object)))
                    .Should().Throw<ArgumentNullException>();
                ((Action)(() => new ComplianceSchemeFeeBySubmissionService(_repositoryMock.Object, _snapshotRepositoryMock.Object, null!, _paymentsServiceMock.Object, _timeProviderMock.Object, _loggerMock.Object)))
                    .Should().Throw<ArgumentNullException>();
                ((Action)(() => new ComplianceSchemeFeeBySubmissionService(_repositoryMock.Object, _snapshotRepositoryMock.Object, _calculatorMock.Object, null!, _timeProviderMock.Object, _loggerMock.Object)))
                    .Should().Throw<ArgumentNullException>();
                ((Action)(() => new ComplianceSchemeFeeBySubmissionService(_repositoryMock.Object, _snapshotRepositoryMock.Object, _calculatorMock.Object, _paymentsServiceMock.Object, null!, _loggerMock.Object)))
                    .Should().Throw<ArgumentNullException>();
                ((Action)(() => new ComplianceSchemeFeeBySubmissionService(_repositoryMock.Object, _snapshotRepositoryMock.Object, _calculatorMock.Object, _paymentsServiceMock.Object, _timeProviderMock.Object, null!)))
                    .Should().Throw<ArgumentNullException>();
            }
        }

        [TestMethod]
        public async Task GetFeesAsync_SnapshotExists_ReturnsProjectedResponseAndSkipsCalculator()
        {
            var record = BuildRecord(created: Today.AddDays(-1), producers: new[] { Producer() });
            SetupRepo(record);

            var snapshot = new RegistrationFeeSnapshot
            {
                Id = Guid.NewGuid(),
                RegistrationSubmissionDataId = record.Id,
                TotalFee = 3000m,
                LineItems = new List<RegistrationFeeLineItem>
                {
                    new()
                    {
                        FeeTypeId = FeeTypeIds.ComplianceSchemeRegistrationFee,
                        FeeTypeName = nameof(FeeTypeIds.ComplianceSchemeRegistrationFee),
                        Amount = 1000m,
                    },
                    new()
                    {
                        FeeTypeId = FeeTypeIds.MemberRegistrationFee,
                        FeeTypeName = nameof(FeeTypeIds.MemberRegistrationFee),
                        Amount = 2000m,
                        MemberId = "ORG-1",
                    },
                },
            };
            _snapshotRepositoryMock
                .Setup(s => s.GetByRegistrationSubmissionDataIdAsync(record.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(snapshot);
            _paymentsServiceMock
                .Setup(p => p.GetPreviousPaymentsByReferenceAsync(record.ApplicationReferenceNumber, It.IsAny<CancellationToken>()))
                .ReturnsAsync(750m);

            var result = await _sut.GetFeesAsync(record.SubmissionId, false, CancellationToken.None);

            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result!.TotalFee.Should().Be(3000m);
                result.ComplianceSchemeRegistrationFee.Should().Be(1000m);
                result.PreviousPayment.Should().Be(750m);
                result.OutstandingPayment.Should().Be(2250m);
                result.RegistrationBlobName.Should().Be(record.RegistrationBlobName);
                result.ComplianceSchemeMembersWithFees.Should().ContainSingle(m => m.MemberId == "ORG-1" && m.MemberRegistrationFee == 2000m);
                _calculatorMock.Verify(c => c.CalculateFeesAsync(It.IsAny<ComplianceSchemeFeesRequestDto>(), It.IsAny<CancellationToken>()), Times.Never);
            }
        }

        [TestMethod]
        public async Task GetFeesAsync_SnapshotWithMemberSubsidiaryOmpRows_SubsidiariesFeeIncludesOmpAndTotalMemberFeeNotDouble()
        {
            // Regression: matches ComplianceSchemeCalculatorService.cs:80-94.
            // member.SubsidiariesFee = band + OMP + CLR; TotalMemberFee then adds SubsidiariesFee
            // (not OMP/CLR again). Prior projector under-filled SubsidiariesFee to band only and
            // added OMP/CLR back on TotalMemberFee — total was right, subtotal was wrong.
            var record = BuildRecord(created: Today.AddDays(-1), producers: new[] { Producer() });
            SetupRepo(record);

            var snapshot = new RegistrationFeeSnapshot
            {
                Id = Guid.NewGuid(),
                RegistrationSubmissionDataId = record.Id,
                TotalFee = 12030m,
                LineItems = new List<RegistrationFeeLineItem>
                {
                    new()
                    {
                        FeeTypeId = FeeTypeIds.MemberRegistrationFee,
                        FeeTypeName = nameof(FeeTypeIds.MemberRegistrationFee),
                        Amount = 1803m,
                        MemberId = "173503",
                    },
                    new()
                    {
                        FeeTypeId = FeeTypeIds.SubsidiaryFee,
                        FeeTypeName = nameof(FeeTypeIds.SubsidiaryFee),
                        BandNumber = 1,
                        Quantity = 1,
                        UnitPrice = 690m,
                        Amount = 690m,
                        MemberId = "173503",
                    },
                    new()
                    {
                        FeeTypeId = FeeTypeIds.SubsidiaryOnlineMarketplaceFee,
                        FeeTypeName = nameof(FeeTypeIds.SubsidiaryOnlineMarketplaceFee),
                        Quantity = 1,
                        UnitPrice = 2885m,
                        Amount = 2885m,
                        MemberId = "173503",
                    },
                },
            };
            _snapshotRepositoryMock
                .Setup(s => s.GetByRegistrationSubmissionDataIdAsync(record.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(snapshot);

            var result = await _sut.GetFeesAsync(record.SubmissionId, false, CancellationToken.None);

            var member = result!.ComplianceSchemeMembersWithFees.Single(m => m.MemberId == "173503");
            using (new AssertionScope())
            {
                member.SubsidiariesFee.Should().Be(3575m); // band 690 + OMP 2885
                member.TotalMemberFee.Should().Be(5378m);  // reg 1803 + subsidiariesFee 3575 (NOT double-counting OMP)
                member.SubsidiariesFeeBreakdown.TotalSubsidiariesOMPFees.Should().Be(2885m);
            }
        }

        [TestMethod]
        public async Task GetFeesAsync_NoRecords_ReturnsNullAndDoesNotCallCalculator()
        {
            _repositoryMock.Setup(r => r.GetAllForSubmissionAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Array.Empty<RegistrationSubmissionData>());

            var result = await _sut.GetFeesAsync(Guid.NewGuid(), false, CancellationToken.None);

            using (new AssertionScope())
            {
                result.Should().BeNull();
                _calculatorMock.Verify(c => c.CalculateFeesAsync(It.IsAny<ComplianceSchemeFeesRequestDto>(), It.IsAny<CancellationToken>()), Times.Never);
            }
        }

        [TestMethod]
        public async Task GetFeesAsync_AllRejected_ReturnsNull()
        {
            var rejected = BuildRecord(
                created: Today.AddMonths(-1),
                events: new[]
                {
                    (RegistrationEventNames.SubmittedForRegulatorApproval, Today.AddMonths(-1)),
                    (RegistrationEventNames.RejectedByRegulator, Today.AddDays(-15)),
                });
            SetupRepo(rejected);

            var result = await _sut.GetFeesAsync(Guid.NewGuid(), false, CancellationToken.None);

            result.Should().BeNull();
        }

        [TestMethod]
        public async Task GetFeesAsync_NotYetSubmittedAndTodayAfterDeadline_SubmissionLevelLateAppliedToAllMembers()
        {
            // Latest non-rejected has NO SubmittedForRegulatorApproval event, so compare-date = today.
            // Today (2026-07-24) > Deadline (2026-04-01) → submissionLevelLate = true.
            // No first submission → noFirstSubmission = true → cascade fires → every member gets IsLateFeeApplicable=true.
            var record = BuildRecord(
                created: Today.AddDays(-1),
                producers: new[] { Producer(newJoiner: false), Producer(newJoiner: false) });
            SetupRepo(record);

            await _sut.GetFeesAsync(Guid.NewGuid(), false, CancellationToken.None);
            var captured = CapturedRequest();

            captured.ComplianceSchemeMembers.Should().OnlyContain(m => m.IsLateFeeApplicable);
        }

        [TestMethod]
        public async Task GetFeesAsync_OriginalCsoLate_LateFeeCascadesToAllMembersEvenNonNewJoiners()
        {
            // First submitted-for-approval date is after the deadline → isOriginalCsoLate = true.
            // All members get IsLateFeeApplicable regardless of new-joiner status.
            var lateFirstSubmit = Deadline.AddDays(5);
            var record = BuildRecord(
                created: Today.AddMonths(-2),
                events: new[] { (RegistrationEventNames.SubmittedForRegulatorApproval, lateFirstSubmit) },
                producers: new[] { Producer(newJoiner: false), Producer(newJoiner: false) });
            SetupRepo(record);

            await _sut.GetFeesAsync(Guid.NewGuid(), false, CancellationToken.None);
            var captured = CapturedRequest();

            captured.ComplianceSchemeMembers.Should().OnlyContain(m => m.IsLateFeeApplicable);
        }

        [TestMethod]
        public async Task GetFeesAsync_OriginalNotLateButCurrentlyLateAndMemberIsNewJoiner_OnlyNewJoinerGetsLateFee()
        {
            // First submitted-for-approval on time → isOriginalCsoLate = false, noFirstSubmission = false.
            // Latest not yet submitted → submissionLevelLate = today >= deadline → true.
            // Per-member: only new-joiners get IsLateFeeApplicable.
            var onTimeFirstSubmit = Deadline.AddDays(-10);
            var first = BuildRecord(
                created: onTimeFirstSubmit.AddDays(-1),
                events: new[] { (RegistrationEventNames.SubmittedForRegulatorApproval, onTimeFirstSubmit) });
            var latest = BuildRecord(
                created: Today.AddDays(-1),
                producers: new[] { Producer("A", newJoiner: false), Producer("B", newJoiner: true) });
            SetupRepo(first, latest);

            await _sut.GetFeesAsync(Guid.NewGuid(), false, CancellationToken.None);
            var captured = CapturedRequest();

            using (new AssertionScope())
            {
                captured.ComplianceSchemeMembers.Should().HaveCount(2);
                captured.ComplianceSchemeMembers.Single(m => m.MemberId == "A").IsLateFeeApplicable.Should().BeFalse();
                captured.ComplianceSchemeMembers.Single(m => m.MemberId == "B").IsLateFeeApplicable.Should().BeTrue();
            }
        }

        [TestMethod]
        public async Task GetFeesAsync_OriginalOnTimeAndLatestOnTime_NoLateFeesForAnyone()
        {
            var onTime = Deadline.AddDays(-10);
            var first = BuildRecord(
                created: onTime.AddDays(-1),
                events: new[] { (RegistrationEventNames.SubmittedForRegulatorApproval, onTime) });
            var latest = BuildRecord(
                created: onTime.AddDays(1),
                events: new[] { (RegistrationEventNames.SubmittedForRegulatorApproval, onTime.AddDays(2)) },
                producers: new[] { Producer("A", newJoiner: false), Producer("B", newJoiner: true) });
            SetupRepo(first, latest);

            await _sut.GetFeesAsync(Guid.NewGuid(), false, CancellationToken.None);
            var captured = CapturedRequest();

            captured.ComplianceSchemeMembers.Should().OnlyContain(m => !m.IsLateFeeApplicable);
        }

        [TestMethod]
        public async Task GetFeesAsync_CalcDateIsFirstSubmittedForApproval_WhenPresent()
        {
            var firstApproval = new DateTime(2026, 6, 15, 0, 0, 0, DateTimeKind.Utc);
            var record = BuildRecord(
                created: firstApproval.AddDays(-1),
                events: new[] { (RegistrationEventNames.SubmittedForRegulatorApproval, firstApproval) });
            SetupRepo(record);

            await _sut.GetFeesAsync(Guid.NewGuid(), false, CancellationToken.None);
            var captured = CapturedRequest();

            captured.SubmissionDate.Should().Be(firstApproval);
        }

        [TestMethod]
        public async Task GetFeesAsync_CalcDateIsToday_WhenNoFirstSubmissionYet()
        {
            var record = BuildRecord(created: Today.AddDays(-1));
            SetupRepo(record);

            await _sut.GetFeesAsync(Guid.NewGuid(), false, CancellationToken.None);
            var captured = CapturedRequest();

            captured.SubmissionDate.Should().Be(Today);
        }

        [TestMethod]
        public async Task GetFeesAsync_IncludeRegistrationFee_FalseWhenCsoSmallProducerWindow()
        {
            var record = BuildRecord(created: Today.AddDays(-1), windowType: "CsoSmallProducer");
            SetupRepo(record);

            await _sut.GetFeesAsync(Guid.NewGuid(), false, CancellationToken.None);
            var captured = CapturedRequest();

            captured.IncludeRegistrationFee.Should().BeFalse();
        }

        [TestMethod]
        public async Task GetFeesAsync_IncludeRegistrationFee_TrueForOtherWindowTypes()
        {
            var record = BuildRecord(created: Today.AddDays(-1), windowType: "CsoLargeProducer");
            SetupRepo(record);

            await _sut.GetFeesAsync(Guid.NewGuid(), false, CancellationToken.None);
            var captured = CapturedRequest();

            captured.IncludeRegistrationFee.Should().BeTrue();
        }

        [TestMethod]
        public async Task GetFeesAsync_RegulatorAndAppRefTakenFromLatestNonRejectedRecord()
        {
            var first = BuildRecord(
                created: Today.AddDays(-30),
                regulatorNation: "GB-SCT",
                applicationReferenceNumber: "OLD-REF");
            var latest = BuildRecord(
                created: Today.AddDays(-1),
                regulatorNation: "GB-ENG",
                applicationReferenceNumber: "NEW-REF");
            SetupRepo(first, latest);

            await _sut.GetFeesAsync(Guid.NewGuid(), false, CancellationToken.None);
            var captured = CapturedRequest();

            using (new AssertionScope())
            {
                captured.Regulator.Should().Be("GB-ENG");
                captured.ApplicationReferenceNumber.Should().Be("NEW-REF");
            }
        }

        [TestMethod]
        public async Task GetFeesAsync_ReturnsCalculatorResponse()
        {
            var record = BuildRecord(created: Today.AddDays(-1));
            SetupRepo(record);

            var result = await _sut.GetFeesAsync(Guid.NewGuid(), false, CancellationToken.None);

            result.Should().BeSameAs(_calculatorResponse);
        }

        [TestMethod]
        public async Task GetFeesAsync_MembersMappedWithSubsidiaryCounts()
        {
            var producer = new RegistrationSubmissionProducer
            {
                OrganisationId = "ORG-1",
                OrganisationSize = "Large",
                IsOnlineMarketplace = true,
                IsClosedLoopRecycling = false,
                IsNewJoiner = false,
                Subsidiaries = new List<RegistrationSubmissionSubsidiary>
                {
                    new() { SubsidiaryId = "S1", IsOnlineMarketplace = true, IsClosedLoopRecycling = false },
                    new() { SubsidiaryId = "S2", IsOnlineMarketplace = false, IsClosedLoopRecycling = true },
                    new() { SubsidiaryId = "S3", IsOnlineMarketplace = false, IsClosedLoopRecycling = false },
                },
            };
            var record = BuildRecord(created: Today.AddDays(-1), producers: new[] { producer });
            SetupRepo(record);

            await _sut.GetFeesAsync(Guid.NewGuid(), false, CancellationToken.None);
            var captured = CapturedRequest();

            var member = captured.ComplianceSchemeMembers.Single();
            using (new AssertionScope())
            {
                member.MemberId.Should().Be("ORG-1");
                member.MemberType.Should().Be("Large");
                member.IsOnlineMarketplace.Should().BeTrue();
                member.IsClosedLoopRecycling.Should().BeFalse();
                member.NumberOfSubsidiaries.Should().Be(3);
                member.NoOfSubsidiariesOnlineMarketplace.Should().Be(1);
                member.NoOfSubsidiariesClosedLoopRecycling.Should().Be(1);
            }
        }

        [TestMethod]
        public async Task GetFeesAsync_EchoesRegistrationBlobNameFromLatestNonRejectedRecord()
        {
            var record = BuildRecord(created: Today.AddDays(-1), producers: new[] { Producer() });
            record.RegistrationBlobName = "cso-blob-under-test.csv";
            SetupRepo(record);

            var result = await _sut.GetFeesAsync(Guid.NewGuid(), false, CancellationToken.None);

            result.Should().NotBeNull();
            result!.RegistrationBlobName.Should().Be("cso-blob-under-test.csv");
        }

        [TestMethod]
        public async Task GetFeesAsync_CalculatorPath_EnrichesResponseMembersWithMemberTypeAndSubsidiaryCount()
        {
            var producer = new RegistrationSubmissionProducer
            {
                OrganisationId = "ORG-1",
                OrganisationSize = "Large",
                Subsidiaries = new List<RegistrationSubmissionSubsidiary>
                {
                    new() { SubsidiaryId = "S1" },
                    new() { SubsidiaryId = "S2" },
                },
            };
            var record = BuildRecord(created: Today.AddDays(-1), producers: new[] { producer });
            SetupRepo(record);
            _calculatorResponse.ComplianceSchemeMembersWithFees.Add(new ComplianceSchemeMembersWithFeesDto
            {
                MemberId = "ORG-1",
                SubsidiariesFeeBreakdown = new SubsidiariesFeeBreakdown(),
            });

            var result = await _sut.GetFeesAsync(Guid.NewGuid(), false, CancellationToken.None);

            result.Should().NotBeNull();
            var enrichedMember = result!.ComplianceSchemeMembersWithFees.Single();
            using (new AssertionScope())
            {
                enrichedMember.MemberType.Should().Be("Large");
                enrichedMember.NumberOfSubsidiaries.Should().Be(2);
            }
        }

        [TestMethod]
        public async Task GetFeesAsync_RequireSubmittedForApproval_WipResubmissionAfterAcceptedCycle_ReturnsAcceptedCycleFees()
        {
            var acceptedCycle = BuildRecord(
                created: Today.AddMonths(-1),
                applicationReferenceNumber: "ACCEPTED-REF",
                events: new[] { (RegistrationEventNames.SubmittedForRegulatorApproval, Today.AddMonths(-1)) },
                producers: new[] { new RegistrationSubmissionProducer { OrganisationId = "ORG-1", OrganisationSize = "Large" } });
            var wipResubmission = BuildRecord(
                created: Today.AddDays(-1),
                applicationReferenceNumber: "WIP-REF",
                producers: new[] { new RegistrationSubmissionProducer { OrganisationId = "ORG-1", OrganisationSize = "Large" } });
            SetupRepo(acceptedCycle, wipResubmission);

            await _sut.GetFeesAsync(Guid.NewGuid(), true, CancellationToken.None);
            var captured = CapturedRequest();

            captured.ApplicationReferenceNumber.Should().Be("ACCEPTED-REF");
        }

        [TestMethod]
        public async Task GetFeesAsync_RequireSubmittedForApproval_NoSubmittedCycle_ReturnsNull()
        {
            var wipOnly = BuildRecord(
                created: Today.AddDays(-1),
                producers: new[] { new RegistrationSubmissionProducer { OrganisationId = "ORG-1", OrganisationSize = "Large" } });
            SetupRepo(wipOnly);

            var result = await _sut.GetFeesAsync(Guid.NewGuid(), true, CancellationToken.None);

            using (new AssertionScope())
            {
                result.Should().BeNull();
                _calculatorMock.Verify(c => c.CalculateFeesAsync(It.IsAny<ComplianceSchemeFeesRequestDto>(), It.IsAny<CancellationToken>()), Times.Never);
            }
        }

        // -------- helpers --------

        private void SetupRepo(params RegistrationSubmissionData[] records)
        {
            _repositoryMock
                .Setup(r => r.GetAllForSubmissionAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(records);
        }

        private ComplianceSchemeFeesRequestDto CapturedRequest()
        {
            _capturedRequest.Should().NotBeNull("calculator should have been invoked");
            return _capturedRequest!;
        }

        private static RegistrationSubmissionData BuildRecord(
            DateTime created,
            string windowType = "CsoLargeProducer",
            string regulatorNation = "GB-ENG",
            string applicationReferenceNumber = "PEPR2601234",
            (string EventName, DateTime EventDate)[]? events = null,
            RegistrationSubmissionProducer[]? producers = null)
        {
            return new RegistrationSubmissionData
            {
                Id = Guid.NewGuid(),
                SubmissionId = Guid.NewGuid(),
                RegistrationBlobName = $"blob-{Guid.NewGuid()}",
                SubmissionPeriodId = 1,
                SubmissionDate = created,
                CreatedDate = new DateTimeOffset(created, TimeSpan.Zero),
                RegulatorNation = regulatorNation,
                ApplicationReferenceNumber = applicationReferenceNumber,
                Events = (events ?? Array.Empty<(string, DateTime)>())
                    .Select(e => new RegistrationSubmissionDataEvent
                    {
                        EventName = e.EventName,
                        EventDate = e.EventDate,
                    })
                    .ToList(),
                Producers = (producers ?? Array.Empty<RegistrationSubmissionProducer>()).ToList(),
                SubmissionPeriodWindow = new SubmissionPeriod
                {
                    Id = 1,
                    WindowType = windowType,
                    DeadlineDate = Deadline,
                    OpeningDate = Deadline.AddMonths(-6),
                    ClosingDate = Deadline.AddMonths(6),
                    RegistrationYear = 2026,
                },
            };
        }

        private static RegistrationSubmissionProducer Producer(string organisationId = "ORG-1", bool newJoiner = false)
        {
            return new RegistrationSubmissionProducer
            {
                OrganisationId = organisationId,
                OrganisationSize = "Large",
                IsNewJoiner = newJoiner,
                Subsidiaries = new List<RegistrationSubmissionSubsidiary>(),
            };
        }
    }
}
