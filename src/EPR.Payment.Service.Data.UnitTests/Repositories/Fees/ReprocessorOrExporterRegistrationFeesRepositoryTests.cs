using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.Data.Repositories.Fees;
using EPR.Payment.Service.Common.UnitTests.Mocks;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.EntityFrameworkCore;
using EPR.Payment.Service.Common.Enums;
using FluentAssertions;

namespace EPR.Payment.Service.Data.UnitTests.Repositories.Fees
{
    [TestClass]
    public class ReprocessorOrExporterFeeRepositoryTests
    {
        private CancellationToken _cancellationToken;

        [TestInitialize]
        public void TestInitialize()
        {
            _cancellationToken = new CancellationToken();            
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetFeeAsync_ValidInput_ShouldReturnFee(
            [Frozen] Mock<IAppDbContext> dataContextMock,
            [Greedy] ReprocessorOrExporterFeeRepository subjectUnderTest)
        {
            // Arrange
            var registrationFeesMock = MockIRegistrationFeesRepository.GetRegistrationFeesMock();
            
            dataContextMock.Setup(i => i.RegistrationFees)
                .ReturnsDbSet(registrationFeesMock.Object);

            var regulator = RegulatorType.Create("GB-ENG");
            var submissionDate = DateTime.Today;
            var groupId = (int)Group.Exporters;
            var subGroupId = (int)SubGroup.Aluminium;

            // Act
            var result = await subjectUnderTest.GetFeeAsync(groupId, subGroupId, regulator, submissionDate, _cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetFeeAsync_ValidInput_NoMatchCondition_ShouldReturnNull(
            [Frozen] Mock<IAppDbContext> dataContextMock,
            [Greedy] ReprocessorOrExporterFeeRepository subjectUnderTest)
        {
            // Arrange
            var registrationFeesMock = MockIRegistrationFeesRepository.GetRegistrationFeesMock();

            dataContextMock.Setup(i => i.RegistrationFees)
                .ReturnsDbSet(registrationFeesMock.Object);

            var regulator = RegulatorType.Create("GB-ENG");

            // Set submission date to mimimum value
            var submissionDate = DateTime.MinValue;
            var groupId = (int)Group.Exporters;
            var subGroupId = (int)SubGroup.Aluminium;

            // Act
            var result = await subjectUnderTest.GetFeeAsync(groupId, subGroupId, regulator, submissionDate, _cancellationToken);

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetFeeAsync_ValidInput_EmptyData_ShouldReturnNull(
            [Frozen] Mock<IAppDbContext> dataContextMock,
            [Greedy] ReprocessorOrExporterFeeRepository subjectUnderTest)
        {
            // Arrange
            var registrationFeesMock = MockIRegistrationFeesRepository.GetEmptyRegistrationFeesMock();

            dataContextMock.Setup(i => i.RegistrationFees)
                .ReturnsDbSet(registrationFeesMock.Object);

            var regulator = RegulatorType.Create("GB-ENG");
            var submissionDate = DateTime.MinValue;
            var groupId = (int)Group.Exporters;
            var subGroupId = (int)SubGroup.Aluminium;

            // Act
            var result = await subjectUnderTest.GetFeeAsync(groupId, subGroupId, regulator, submissionDate, _cancellationToken);

            // Assert
            result.Should().BeNull();
        }
    }
}