using EPR.Payment.Service.Common.Data.DataModels;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationSubmission;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.Producer;
using EPR.Payment.Service.Common.Enums;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.Producer;
using EPR.Payment.Service.Services.RegistrationSubmission;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPR.Payment.Service.UnitTests.Services.RegistrationSubmission
{
    [TestClass]
    public class RegistrationFeeSnapshotHandlerTests
    {
        private static readonly DateTime Today = new(2026, 7, 24, 0, 0, 0, DateTimeKind.Utc);
        private static readonly DateTime Deadline = new(2026, 4, 1, 0, 0, 0, DateTimeKind.Utc);

        private Mock<IRegistrationFeeSnapshotRepository> _snapshotRepositoryMock = null!;
        private Mock<IProducerFeesCalculatorService> _producerCalculatorMock = null!;
        private Mock<IComplianceSchemeCalculatorService> _complianceSchemeCalculatorMock = null!;
        private Mock<TimeProvider> _timeProviderMock = null!;
        private Mock<ILogger<RegistrationFeeSnapshotHandler>> _loggerMock = null!;
        private RegistrationFeeSnapshotHandler _sut = null!;
        private CancellationToken _ct;

        [TestInitialize]
        public void Init()
        {
            _snapshotRepositoryMock = new Mock<IRegistrationFeeSnapshotRepository>();
            _producerCalculatorMock = new Mock<IProducerFeesCalculatorService>();
            _complianceSchemeCalculatorMock = new Mock<IComplianceSchemeCalculatorService>();
            _timeProviderMock = new Mock<TimeProvider>();
            _timeProviderMock.Setup(t => t.GetUtcNow()).Returns(new DateTimeOffset(Today, TimeSpan.Zero));
            _loggerMock = new Mock<ILogger<RegistrationFeeSnapshotHandler>>();
            _ct = CancellationToken.None;
            _sut = new RegistrationFeeSnapshotHandler(
                _snapshotRepositoryMock.Object,
                _producerCalculatorMock.Object,
                _complianceSchemeCalculatorMock.Object,
                _timeProviderMock.Object,
                _loggerMock.Object);
        }

        [TestMethod]
        public void Constructor_NullDependency_Throws()
        {
            using (new AssertionScope())
            {
                ((Action)(() => new RegistrationFeeSnapshotHandler(null!, _producerCalculatorMock.Object, _complianceSchemeCalculatorMock.Object, _timeProviderMock.Object, _loggerMock.Object)))
                    .Should().Throw<ArgumentNullException>();
                ((Action)(() => new RegistrationFeeSnapshotHandler(_snapshotRepositoryMock.Object, null!, _complianceSchemeCalculatorMock.Object, _timeProviderMock.Object, _loggerMock.Object)))
                    .Should().Throw<ArgumentNullException>();
                ((Action)(() => new RegistrationFeeSnapshotHandler(_snapshotRepositoryMock.Object, _producerCalculatorMock.Object, null!, _timeProviderMock.Object, _loggerMock.Object)))
                    .Should().Throw<ArgumentNullException>();
                ((Action)(() => new RegistrationFeeSnapshotHandler(_snapshotRepositoryMock.Object, _producerCalculatorMock.Object, _complianceSchemeCalculatorMock.Object, null!, _loggerMock.Object)))
                    .Should().Throw<ArgumentNullException>();
                ((Action)(() => new RegistrationFeeSnapshotHandler(_snapshotRepositoryMock.Object, _producerCalculatorMock.Object, _complianceSchemeCalculatorMock.Object, _timeProviderMock.Object, null!)))
                    .Should().Throw<ArgumentNullException>();
            }
        }

        [TestMethod]
        public async Task HandleAsync_NullLatestRecord_Throws()
        {
            Func<Task> act = () => _sut.HandleAsync(null!, Today, NewLifecycle(), _ct);
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [TestMethod]
        public async Task HandleAsync_ExistingSnapshot_ShortCircuits()
        {
            var rsd = BuildProducerRsd();
            _snapshotRepositoryMock
                .Setup(s => s.GetByRegistrationSubmissionDataIdAsync(rsd.Id, _ct))
                .ReturnsAsync(new RegistrationFeeSnapshot { Id = Guid.NewGuid(), RegistrationSubmissionDataId = rsd.Id });

            await _sut.HandleAsync(rsd, Today, NewLifecycle(rsd), _ct);

            using (new AssertionScope())
            {
                _producerCalculatorMock.Verify(c => c.CalculateFeesAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), _ct), Times.Never);
                _complianceSchemeCalculatorMock.Verify(c => c.CalculateFeesAsync(It.IsAny<ComplianceSchemeFeesRequestDto>(), _ct), Times.Never);
                _snapshotRepositoryMock.Verify(s => s.CreateAsync(It.IsAny<RegistrationFeeSnapshot>(), _ct), Times.Never);
            }
        }

        [TestMethod]
        public async Task HandleAsync_ProducerPath_ProjectsLineItemsAndPersists()
        {
            var rsd = BuildProducerRsd();
            _producerCalculatorMock
                .Setup(c => c.CalculateFeesAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), _ct))
                .ReturnsAsync(new RegistrationFeesResponseDto
                {
                    ProducerRegistrationFee = 2000m,
                    ProducerOnlineMarketPlaceFee = 50m,
                    ProducerClosedLoopRecyclingFee = 0m,
                    ProducerLateRegistrationFee = 250m,
                    TotalFee = 2300m,
                    SubsidiariesFeeBreakdown = new SubsidiariesFeeBreakdown
                    {
                        FeeBreakdowns = new List<FeeBreakdown>
                        {
                            new() { BandNumber = 1, UnitCount = 3, UnitPrice = 100m, TotalPrice = 300m },
                        },
                        CountOfOMPSubsidiaries = 2,
                        UnitOMPFees = 25m,
                        TotalSubsidiariesOMPFees = 50m,
                    },
                });

            RegistrationFeeSnapshot? captured = null;
            _snapshotRepositoryMock
                .Setup(s => s.CreateAsync(It.IsAny<RegistrationFeeSnapshot>(), _ct))
                .Callback<RegistrationFeeSnapshot, CancellationToken>((s, _) => captured = s)
                .ReturnsAsync(Guid.NewGuid());

            await _sut.HandleAsync(rsd, Today, NewLifecycle(rsd), _ct);

            captured.Should().NotBeNull();
            using (new AssertionScope())
            {
                captured!.RegistrationSubmissionDataId.Should().Be(rsd.Id);
                captured.TotalFee.Should().Be(2300m);
                captured.LineItems.Should().Contain(l => l.FeeTypeId == FeeTypeIds.ProducerRegistrationFee && l.Amount == 2000m);
                captured.LineItems.Should().Contain(l => l.FeeTypeId == FeeTypeIds.ProducerOnlineMarketplaceFee && l.Amount == 50m);
                captured.LineItems.Should().NotContain(l => l.FeeTypeId == FeeTypeIds.ProducerClosedLoopRecyclingFee);
                captured.LineItems.Should().Contain(l => l.FeeTypeId == FeeTypeIds.ProducerLateRegistrationFee && l.Amount == 250m);
                captured.LineItems.Should().Contain(l => l.FeeTypeId == FeeTypeIds.SubsidiaryFee && l.BandNumber == 1 && l.Amount == 300m);
                captured.LineItems.Should().Contain(l => l.FeeTypeId == FeeTypeIds.SubsidiaryOnlineMarketplaceFee && l.Amount == 50m && l.Quantity == 2);
            }
        }

        [TestMethod]
        public async Task HandleAsync_ComplianceSchemePath_ProjectsPerMemberAndPersists()
        {
            var rsd = BuildComplianceSchemeRsd();
            _complianceSchemeCalculatorMock
                .Setup(c => c.CalculateFeesAsync(It.IsAny<ComplianceSchemeFeesRequestDto>(), _ct))
                .ReturnsAsync(new ComplianceSchemeFeesResponseDto
                {
                    TotalFee = 4500m,
                    ComplianceSchemeRegistrationFee = 1000m,
                    ComplianceSchemeMembersWithFees = new List<ComplianceSchemeMembersWithFeesDto>
                    {
                        new()
                        {
                            MemberId = "ORG-1",
                            MemberRegistrationFee = 2000m,
                            MemberLateRegistrationFee = 150m,
                            SubsidiariesFeeBreakdown = new SubsidiariesFeeBreakdown(),
                        },
                        new()
                        {
                            MemberId = "ORG-2",
                            MemberRegistrationFee = 1000m,
                            MemberClosedLoopRecyclingFee = 350m,
                            SubsidiariesFeeBreakdown = new SubsidiariesFeeBreakdown(),
                        },
                    },
                });

            RegistrationFeeSnapshot? captured = null;
            _snapshotRepositoryMock
                .Setup(s => s.CreateAsync(It.IsAny<RegistrationFeeSnapshot>(), _ct))
                .Callback<RegistrationFeeSnapshot, CancellationToken>((s, _) => captured = s)
                .ReturnsAsync(Guid.NewGuid());

            await _sut.HandleAsync(rsd, Today, NewLifecycle(rsd), _ct);

            captured.Should().NotBeNull();
            using (new AssertionScope())
            {
                captured!.RegistrationSubmissionDataId.Should().Be(rsd.Id);
                captured.TotalFee.Should().Be(4500m);
                captured.LineItems.Should().Contain(l => l.FeeTypeId == FeeTypeIds.ComplianceSchemeRegistrationFee && l.MemberId == null && l.Amount == 1000m);
                captured.LineItems.Should().Contain(l => l.FeeTypeId == FeeTypeIds.MemberRegistrationFee && l.MemberId == "ORG-1" && l.Amount == 2000m);
                captured.LineItems.Should().Contain(l => l.FeeTypeId == FeeTypeIds.MemberLateRegistrationFee && l.MemberId == "ORG-1" && l.Amount == 150m);
                captured.LineItems.Should().Contain(l => l.FeeTypeId == FeeTypeIds.MemberRegistrationFee && l.MemberId == "ORG-2" && l.Amount == 1000m);
                captured.LineItems.Should().Contain(l => l.FeeTypeId == FeeTypeIds.MemberClosedLoopRecyclingFee && l.MemberId == "ORG-2" && l.Amount == 350m);
            }
        }

        [TestMethod]
        public async Task HandleAsync_ProducerPath_SkipsEmptySubsidiaryBands()
        {
            var rsd = BuildProducerRsd();
            _producerCalculatorMock
                .Setup(c => c.CalculateFeesAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), _ct))
                .ReturnsAsync(new RegistrationFeesResponseDto
                {
                    ProducerRegistrationFee = 1000m,
                    TotalFee = 1690m,
                    SubsidiariesFeeBreakdown = new SubsidiariesFeeBreakdown
                    {
                        FeeBreakdowns = new List<FeeBreakdown>
                        {
                            new() { BandNumber = 1, UnitCount = 1, UnitPrice = 690m, TotalPrice = 690m },
                            new() { BandNumber = 2, UnitCount = 0, UnitPrice = 172m, TotalPrice = 0m },
                            new() { BandNumber = 3, UnitCount = 0, UnitPrice = 0m, TotalPrice = 0m },
                        },
                    },
                });

            RegistrationFeeSnapshot? captured = null;
            _snapshotRepositoryMock
                .Setup(s => s.CreateAsync(It.IsAny<RegistrationFeeSnapshot>(), _ct))
                .Callback<RegistrationFeeSnapshot, CancellationToken>((s, _) => captured = s)
                .ReturnsAsync(Guid.NewGuid());

            await _sut.HandleAsync(rsd, Today, NewLifecycle(rsd), _ct);

            using (new AssertionScope())
            {
                captured!.LineItems.Where(l => l.FeeTypeId == FeeTypeIds.SubsidiaryFee).Should().ContainSingle()
                    .Which.BandNumber.Should().Be(1);
            }
        }

        [TestMethod]
        public async Task HandleAsync_EmptyProducers_SkipsWithoutError()
        {
            var rsd = BuildProducerRsd();
            rsd.Producers.Clear();

            await _sut.HandleAsync(rsd, Today, NewLifecycle(rsd), _ct);

            using (new AssertionScope())
            {
                _producerCalculatorMock.Verify(c => c.CalculateFeesAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), _ct), Times.Never);
                _snapshotRepositoryMock.Verify(s => s.CreateAsync(It.IsAny<RegistrationFeeSnapshot>(), _ct), Times.Never);
            }
        }

        private static SubmissionLifecycle NewLifecycle(RegistrationSubmissionData? rsd = null) =>
            new(
                FirstNonRejected: rsd,
                LatestNonRejected: rsd,
                FirstSubmittedForApprovalDate: null,
                LatestSubmittedForApprovalDate: null,
                CalcDate: Today);

        private static RegistrationSubmissionData BuildProducerRsd()
        {
            return new RegistrationSubmissionData
            {
                Id = Guid.NewGuid(),
                SubmissionId = Guid.NewGuid(),
                RegistrationBlobName = "producer-blob",
                ComplianceSchemeId = null,
                CreatedDate = new DateTimeOffset(Today.AddDays(-1), TimeSpan.Zero),
                RegulatorNation = "GB-ENG",
                ApplicationReferenceNumber = "PEPR2601111",
                SubmissionPeriodId = 1,
                Producers = new List<RegistrationSubmissionProducer>
                {
                    new() { OrganisationId = "ORG-1", OrganisationSize = "Large", Subsidiaries = new List<RegistrationSubmissionSubsidiary>() },
                },
                SubmissionPeriodWindow = new SubmissionPeriod
                {
                    Id = 1,
                    WindowType = "LargeProducer",
                    DeadlineDate = Deadline,
                    OpeningDate = Deadline.AddMonths(-6),
                    ClosingDate = Deadline.AddMonths(6),
                    RegistrationYear = 2026,
                },
                Events = new List<RegistrationSubmissionDataEvent>(),
            };
        }

        private static RegistrationSubmissionData BuildComplianceSchemeRsd()
        {
            var rsd = BuildProducerRsd();
            rsd.ComplianceSchemeId = Guid.NewGuid();
            rsd.RegistrationBlobName = "cso-blob";
            rsd.Producers = new List<RegistrationSubmissionProducer>
            {
                new() { OrganisationId = "ORG-1", OrganisationSize = "Large", Subsidiaries = new List<RegistrationSubmissionSubsidiary>() },
                new() { OrganisationId = "ORG-2", OrganisationSize = "Small", Subsidiaries = new List<RegistrationSubmissionSubsidiary>() },
            };
            rsd.SubmissionPeriodWindow.WindowType = "CsoLargeProducer";
            return rsd;
        }
    }
}
