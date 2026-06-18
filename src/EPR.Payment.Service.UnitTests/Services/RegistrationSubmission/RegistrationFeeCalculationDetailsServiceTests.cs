using EPR.Payment.Service.Common.Data.DataModels;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationSubmission;
using EPR.Payment.Service.Services.RegistrationSubmission;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;

namespace EPR.Payment.Service.UnitTests.Services.RegistrationSubmission
{
    [TestClass]
    public class RegistrationFeeCalculationDetailsServiceTests
    {
        private Mock<IRegistrationSubmissionDataRepository> _repositoryMock = null!;
        private RegistrationFeeCalculationDetailsService _sut = null!;
        private CancellationToken _ct;

        [TestInitialize]
        public void Init()
        {
            _repositoryMock = new Mock<IRegistrationSubmissionDataRepository>();
            _sut = new RegistrationFeeCalculationDetailsService(_repositoryMock.Object);
            _ct = CancellationToken.None;
        }

        [TestMethod]
        public void Constructor_NullRepository_Throws()
        {
            Action act = () => new RegistrationFeeCalculationDetailsService(null!);
            act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public async Task GetAsync_NoSnapshot_ReturnsEmpty()
        {
            _repositoryMock
                .Setup(r => r.GetLatestWithProducersAndSubsidiariesAsync(It.IsAny<Guid>(), _ct))
                .ReturnsAsync((RegistrationSubmissionData?)null);

            var result = await _sut.GetAsync(Guid.NewGuid(), _ct);

            result.Should().BeEmpty();
        }

        [TestMethod]
        public async Task GetAsync_SnapshotWithNoProducers_ReturnsEmpty()
        {
            _repositoryMock
                .Setup(r => r.GetLatestWithProducersAndSubsidiariesAsync(It.IsAny<Guid>(), _ct))
                .ReturnsAsync(new RegistrationSubmissionData { Producers = new List<RegistrationSubmissionProducer>() });

            var result = await _sut.GetAsync(Guid.NewGuid(), _ct);

            result.Should().BeEmpty();
        }

        [TestMethod]
        public async Task GetAsync_SingleProducerWithMixedSubsidiaries_MapsCorrectly()
        {
            var producer = new RegistrationSubmissionProducer
            {
                OrganisationId = "ORG-1",
                OrganisationSize = "Large",
                NationId = 1,
                IsOnlineMarketplace = true,
                IsClosedLoopRecycling = true,
                IsNewJoiner = true,
                Subsidiaries = new List<RegistrationSubmissionSubsidiary>
                {
                    new() { SubsidiaryId = "SUB-1", IsOnlineMarketplace = true, IsClosedLoopRecycling = false },
                    new() { SubsidiaryId = "SUB-2", IsOnlineMarketplace = true, IsClosedLoopRecycling = true },
                    new() { SubsidiaryId = "SUB-3", IsOnlineMarketplace = false, IsClosedLoopRecycling = true },
                    new() { SubsidiaryId = "SUB-4", IsOnlineMarketplace = false, IsClosedLoopRecycling = false },
                },
            };
            var snapshot = new RegistrationSubmissionData { Producers = new List<RegistrationSubmissionProducer> { producer } };
            _repositoryMock
                .Setup(r => r.GetLatestWithProducersAndSubsidiariesAsync(It.IsAny<Guid>(), _ct))
                .ReturnsAsync(snapshot);

            var result = await _sut.GetAsync(Guid.NewGuid(), _ct);

            using (new AssertionScope())
            {
                result.Should().HaveCount(1);
                var item = result[0];
                item.OrganisationId.Should().Be("ORG-1");
                item.OrganisationSize.Should().Be("Large");
                item.NationId.Should().Be(1);
                item.IsOnlineMarketplace.Should().BeTrue();
                item.IsClosedLoopRecycling.Should().BeTrue();
                item.IsNewJoiner.Should().BeTrue();
                item.NumberOfSubsidiaries.Should().Be(4);
                item.NumberOfSubsidiariesBeingOnlineMarketPlace.Should().Be(2);
                item.NumberOfSubsidiariesBeingClosedLoopRecycling.Should().Be(2);
            }
        }

        [TestMethod]
        public async Task GetAsync_ComplianceSchemeSnapshot_ReturnsOneRowPerProducer()
        {
            var snapshot = new RegistrationSubmissionData
            {
                Producers = new List<RegistrationSubmissionProducer>
                {
                    new()
                    {
                        OrganisationId = "ORG-A",
                        OrganisationSize = "Large",
                        NationId = 2,
                        IsOnlineMarketplace = false,
                        IsClosedLoopRecycling = false,
                        IsNewJoiner = false,
                        Subsidiaries = new List<RegistrationSubmissionSubsidiary> { new() { SubsidiaryId = "A-1" } },
                    },
                    new()
                    {
                        OrganisationId = "ORG-B",
                        OrganisationSize = "Small",
                        NationId = 3,
                        IsOnlineMarketplace = true,
                        IsClosedLoopRecycling = false,
                        IsNewJoiner = false,
                        Subsidiaries = new List<RegistrationSubmissionSubsidiary>(),
                    },
                    new()
                    {
                        OrganisationId = "ORG-C",
                        OrganisationSize = "Large",
                        NationId = 4,
                        IsOnlineMarketplace = false,
                        IsClosedLoopRecycling = true,
                        IsNewJoiner = true,
                        Subsidiaries = new List<RegistrationSubmissionSubsidiary>
                        {
                            new() { IsOnlineMarketplace = true },
                            new() { IsOnlineMarketplace = true },
                            new() { IsClosedLoopRecycling = true },
                        },
                    },
                },
            };
            _repositoryMock
                .Setup(r => r.GetLatestWithProducersAndSubsidiariesAsync(It.IsAny<Guid>(), _ct))
                .ReturnsAsync(snapshot);

            var result = await _sut.GetAsync(Guid.NewGuid(), _ct);

            using (new AssertionScope())
            {
                result.Should().HaveCount(3);
                result.Single(r => r.OrganisationId == "ORG-A").NumberOfSubsidiaries.Should().Be(1);
                result.Single(r => r.OrganisationId == "ORG-B").NumberOfSubsidiaries.Should().Be(0);
                var c = result.Single(r => r.OrganisationId == "ORG-C");
                c.NumberOfSubsidiaries.Should().Be(3);
                c.NumberOfSubsidiariesBeingOnlineMarketPlace.Should().Be(2);
                c.NumberOfSubsidiariesBeingClosedLoopRecycling.Should().Be(1);
            }
        }

        [TestMethod]
        public async Task GetAsync_QueriesRepositoryWithGivenSubmissionId()
        {
            var submissionId = Guid.NewGuid();
            _repositoryMock
                .Setup(r => r.GetLatestWithProducersAndSubsidiariesAsync(submissionId, _ct))
                .ReturnsAsync((RegistrationSubmissionData?)null);

            await _sut.GetAsync(submissionId, _ct);

            _repositoryMock.Verify(r => r.GetLatestWithProducersAndSubsidiariesAsync(submissionId, _ct), Times.Once);
        }
    }
}
