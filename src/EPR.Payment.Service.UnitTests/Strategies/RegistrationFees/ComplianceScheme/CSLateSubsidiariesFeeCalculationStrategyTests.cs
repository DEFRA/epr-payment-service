using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Strategies.RegistrationFees.ComplianceScheme;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPR.Payment.Service.UnitTests.Strategies.RegistrationFees.ComplianceScheme
{
    [TestClass]
    public class CSLateSubsidiariesFeeCalculationStrategyTests
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
            IComplianceSchemeFeesRepository? nullRepository = null;

            // Act
            Action act = () => new CSLateSubsidiariesFeeCalculationStrategy(nullRepository!);

            // Assert
            act.Should().Throw<ArgumentNullException>()
               .WithMessage("Value cannot be null. (Parameter 'feesRepository')");
        }

        [TestMethod]
        public void Constructor_WhenFeesRepositoryIsNotNull_ShouldInitializeStrategy()
        {
            // Arrange
            var repoMock = _fixture.Create<Mock<IComplianceSchemeFeesRepository>>();

            // Act
            var sut = new CSLateSubsidiariesFeeCalculationStrategy(repoMock.Object);

            // Assert
            using (new AssertionScope())
            {
                sut.Should().NotBeNull();
                sut.Should().BeAssignableTo<ICSLateSubsidiariesFeeCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto, decimal>>();
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeeAsync_WhenRequestIsNull_ShouldThrowArgumentNullException(
            CSLateSubsidiariesFeeCalculationStrategy sut)
        {
            // Act
            Func<Task> act = async () => await sut.CalculateFeeAsync(null!, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>()
                     .WithMessage("Value cannot be null. (Parameter 'request')");
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeeAsync_WhenIsLateFeeApplicableTrue_ShouldReturnZero_AndNotHitRepo(
            [Frozen] Mock<IComplianceSchemeFeesRepository> repoMock,
            CSLateSubsidiariesFeeCalculationStrategy sut)
        {
            // Arrange
            var request = new ComplianceSchemeMemberWithRegulatorDto
            {
                MemberType = "small",
                IsLateFeeApplicable = true,
                NumberOfLateSubsidiaries = 5,
                Regulator = RegulatorType.Create("GB-ENG"),
                SubmissionDate = DateTime.UtcNow
            };

            // Act
            var result = await sut.CalculateFeeAsync(request, CancellationToken.None);

            // Assert
            result.Should().Be(0m);
            repoMock.Verify(r => r.GetLateFeeAsync(It.IsAny<RegulatorType>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeeAsync_WhenNumberOfLateSubsidiariesIsZero_ShouldReturnZero_AndNotHitRepo(
            [Frozen] Mock<IComplianceSchemeFeesRepository> repoMock,
            CSLateSubsidiariesFeeCalculationStrategy sut)
        {
            // Arrange
            var request = new ComplianceSchemeMemberWithRegulatorDto
            {
                MemberType = "small",
                IsLateFeeApplicable = false,
                NumberOfLateSubsidiaries = 0,
                Regulator = RegulatorType.Create("GB-ENG"),
                SubmissionDate = DateTime.UtcNow
            };

            // Act
            var result = await sut.CalculateFeeAsync(request, CancellationToken.None);

            // Assert
            result.Should().Be(0m);
            repoMock.Verify(r => r.GetLateFeeAsync(It.IsAny<RegulatorType>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeeAsync_WhenValidInput_ShouldReturnPerSubsidyFeeTimesCount_AndCallRepoOnce(
            [Frozen] Mock<IComplianceSchemeFeesRepository> repoMock,
            CSLateSubsidiariesFeeCalculationStrategy sut)
        {
            // Arrange
            var regulator = RegulatorType.Create("GB-ENG");
            var submissionDate = DateTime.UtcNow;
            const int lateSubsidiaryCount = 3;
            const decimal perSubsidiaryLateFee = 125.50m;

            var request = new ComplianceSchemeMemberWithRegulatorDto
            {
                MemberType = "small",
                IsLateFeeApplicable = false,
                NumberOfLateSubsidiaries = lateSubsidiaryCount,
                Regulator = regulator,
                SubmissionDate = submissionDate
            };

            var cts = new CancellationTokenSource();
            var token = cts.Token;

            repoMock
                .Setup(r => r.GetLateFeeAsync(regulator, submissionDate, token))
                .ReturnsAsync(perSubsidiaryLateFee);

            // Act
            var result = await sut.CalculateFeeAsync(request, token);

            // Assert
            result.Should().Be(perSubsidiaryLateFee * lateSubsidiaryCount);

            repoMock.Verify(r => r.GetLateFeeAsync(regulator, submissionDate, token), Times.Once);
            repoMock.VerifyNoOtherCalls();
        }
    }
}
