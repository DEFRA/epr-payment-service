using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Constants.RegistrationFees.LookUps;
using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.Data.Repositories.Fees;
using EPR.Payment.Service.Common.UnitTests.Mocks;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.EntityFrameworkCore;
using System.Data.Entity;
using EPR.Payment.Service.Common.Enums;

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
        public async Task GetAccreditationFeeAsync_ValidInput_ShouldReturnFee(
            [Frozen] Mock<IAppDbContext> dataContextMock,
            [Greedy] ReprocessorOrExporterFeeRepository subjectUnderTest)
        {
            // Arrange
            // dataContextMock.Setup(i => i.RegistrationFees)
            //    .ReturnsDbSet(_reprocessorOrExporterRegistrationFeesMock.Object);

            var regulator = RegulatorType.Create("GB-ENG");
            var submissionDate = DateTime.Today;
            var groupId = (int)Group.Exporters;
            var subGroupId = (int)SubGroup.Aluminium;

            // Act
            var result = await subjectUnderTest.GetFeeAsync(groupId, subGroupId, regulator, submissionDate, _cancellationToken);

            // Assert
            Assert.IsNotNull(result);
        }
       
        /*
        [TestMethod]
        [AutoMoqData]
        public async Task GetAccreditationFee_ValidInput_NoMatchCondition_ShouldReturnNull(
          [Frozen] Mock<IAppDbContext> _dataContextMock,
          [Greedy] ReprocessorOrExporterFeeRepository _ReprocessorOrExporterFeeRepository)
        {
            // Arrange
            _ReprocessorOrExporterFeeRepository = new ReprocessorOrExporterFeeRepository(_dataContextMock.Object);
            _reprocessorOrExporterRegistrationFeesMock = MockIAccreditationFeeRepository.GetReprocessorOrExporterRegistrationFeesMock();
            _dataContextMock.Setup(i => i.ReprocessorOrExporterRegistrationFees).ReturnsDbSet(_reprocessorOrExporterRegistrationFeesMock.Object);

            var groupId = (int)Group.Exporters;
            var subGroupId = (int)SubGroup.Aluminium;

            // Act
            // Set submission date to mimimum value
            var result = await _ReprocessorOrExporterFeeRepository.GetFeeAsync(groupId, subGroupId, 0, 500, RegulatorType.Create("GB-ENG"), DateTime.MinValue, _cancellationToken);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetAccreditationFeeAsync_ValidInput_EmptyData_ShouldReturnNull(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ReprocessorOrExporterFeeRepository _ReprocessorOrExporterFeeRepository)
        {
            // Arrange
            _ReprocessorOrExporterFeeRepository = new ReprocessorOrExporterFeeRepository(_dataContextMock.Object);
            _reprocessorOrExporterRegistrationFeesMock = MockIAccreditationFeeRepository.GetEmptyReprocessorOrExporterRegistrationFeesMock();
            _dataContextMock.Setup(i => i.ReprocessorOrExporterRegistrationFees).ReturnsDbSet(_reprocessorOrExporterRegistrationFeesMock.Object);

            var groupId = (int)Group.Exporters;
            var subGroupId = (int)SubGroup.Aluminium;

            // Act
            var result = await _ReprocessorOrExporterFeeRepository.GetFeeAsync(groupId, subGroupId, 0, 500, RegulatorType.Create("GB-ENG"), DateTime.UtcNow, _cancellationToken);

            // Assert
            Assert.IsNull(result);
        }
        */
    }
}