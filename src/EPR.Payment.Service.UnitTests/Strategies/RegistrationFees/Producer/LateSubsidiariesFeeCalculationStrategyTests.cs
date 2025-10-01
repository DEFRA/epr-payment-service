using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.Producer;
using EPR.Payment.Service.Strategies.RegistrationFees.Producer;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPR.Payment.Service.UnitTests.Strategies.RegistrationFees.Producer
{
    [TestClass]
    public class LateSubsidiariesFeeCalculationStrategyTests
    {
        private IFixture _fixture = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
        }

        [TestMethod]
        public void Constructor_WhenFeesRepositoryIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            IProducerFeesRepository? nullRepository = null;

            // Act
            Action act = () => new LateSubsidiariesFeeCalculationStrategy(nullRepository!);

            // Assert
            act.Should().Throw<ArgumentNullException>()
               .WithMessage("Value cannot be null. (Parameter 'feesRepository')");
        }

        [TestMethod]
        public void Constructor_WhenFeesRepositoryProvided_ShouldCreateInstance()
        {
            // Arrange
            var repo = _fixture.Create<Mock<IProducerFeesRepository>>();

            // Act
            var sut = new LateSubsidiariesFeeCalculationStrategy(repo.Object);

            // Assert
            using (new AssertionScope())
            {
                sut.Should().NotBeNull();
                sut.Should().BeAssignableTo<ILateSubsidiariesFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal>>();
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeeAsync_WhenRequestIsNull_ShouldThrowArgumentNullException(
            LateSubsidiariesFeeCalculationStrategy sut)
        {
            // Act
            var act = async () => await sut.CalculateFeeAsync(null!, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>()
                     .WithParameterName("request");
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeeAsync_WhenIsLateFeeApplicableTrue_ShouldReturnZero_AndNotCallRepo(
            [Frozen] Mock<IProducerFeesRepository> repo,
            LateSubsidiariesFeeCalculationStrategy sut)
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "Large",
                IsLateFeeApplicable = true,
                NumberOfLateSubsidiaries = 5, // ignored
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "A1",
                SubmissionDate = DateTime.UtcNow
            };

            // Act
            var result = await sut.CalculateFeeAsync(request, CancellationToken.None);

            // Assert
            result.Should().Be(0m);
            repo.Verify(r => r.GetLateFeeAsync(It.IsAny<RegulatorType>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeeAsync_WhenLateSubsidiaryCountIsZeroOrLess_ShouldReturnZero_AndNotCallRepo(
            [Frozen] Mock<IProducerFeesRepository> repo,
            LateSubsidiariesFeeCalculationStrategy sut)
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "Small",
                IsLateFeeApplicable = false,
                NumberOfLateSubsidiaries = 0,
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "A2",
                SubmissionDate = DateTime.UtcNow
            };

            // Act
            var result = await sut.CalculateFeeAsync(request, CancellationToken.None);

            // Assert
            result.Should().Be(0m);
            repo.Verify(r => r.GetLateFeeAsync(It.IsAny<RegulatorType>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeeAsync_WhenRegulatorIsNullOrWhitespace_ShouldThrowArgumentException(
            LateSubsidiariesFeeCalculationStrategy sut)
        {
            // Arrange
            var badRequests = new[]
            {
                new ProducerRegistrationFeesRequestDto
                {
                    ProducerType = "Large",
                    IsLateFeeApplicable = false,
                    NumberOfLateSubsidiaries = 2,
                    Regulator = null!,
                    ApplicationReferenceNumber = "A3",
                    SubmissionDate = DateTime.UtcNow
                },
                new ProducerRegistrationFeesRequestDto
                {
                    ProducerType = "Large",
                    IsLateFeeApplicable = false,
                    NumberOfLateSubsidiaries = 2,
                    Regulator = "   ",
                    ApplicationReferenceNumber = "A4",
                    SubmissionDate = DateTime.UtcNow
                }
            };

            foreach (var req in badRequests)
            {
                await Assert.ThrowsExceptionAsync<ArgumentException>(
                    () => sut.CalculateFeeAsync(req, CancellationToken.None));
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeeAsync_WhenValidInput_ShouldReturnPerSubsidiaryFeeTimesCount_AndCallRepoOnce(
            [Frozen] Mock<IProducerFeesRepository> repo,
            LateSubsidiariesFeeCalculationStrategy sut)
        {
            // Arrange
            var perFee = 125m;
            var count = 3;

            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "Large",
                IsLateFeeApplicable = false,
                NumberOfLateSubsidiaries = count,
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "A5",
                SubmissionDate = DateTime.UtcNow
            };

            var regulator = RegulatorType.Create(request.Regulator);
            var token = new CancellationTokenSource().Token;

            repo.Setup(r => r.GetLateFeeAsync(
                            regulator,
                            request.SubmissionDate,
                            It.Is<CancellationToken>(ct => ct == token)))
                .ReturnsAsync(perFee);

            // Act
            var result = await sut.CalculateFeeAsync(request, token);

            // Assert
            result.Should().Be(perFee * count);
            repo.Verify(r => r.GetLateFeeAsync(
                            It.Is<RegulatorType>(rt => rt.Equals(regulator)),
                            request.SubmissionDate,
                            token),
                        Times.Once);
            repo.VerifyNoOtherCalls();
        }
    }
}
