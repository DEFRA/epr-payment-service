using System.Data.Entity;
using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.Data.Repositories.Fees;
using EPR.Payment.Service.Common.Enums;
using EPR.Payment.Service.Common.UnitTests.Mocks;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.EntityFrameworkCore;

namespace EPR.Payment.Service.Data.UnitTests.Repositories.Fees
{
    [TestClass]
    public class AccreditationFeesRepositoryTests
    {
        private Mock<DbSet<Common.Data.DataModels.Lookups.AccreditationFee>> _accreditationFeesMock = null!;
        private CancellationToken _cancellationToken;

        [TestInitialize]
        public void TestInitialize()
        {
            _cancellationToken = new CancellationToken();            
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetAccreditationFeeAsync_ValidInput_ShouldReturnFee(
        [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] AccreditationFeesRepository _accreditationFeesRepository)
        {
            // Arrange
            _accreditationFeesRepository = new AccreditationFeesRepository(_dataContextMock.Object);
            _accreditationFeesMock = MockIAccreditationFeeRepository.GetAccreditationFeesMock();
            _dataContextMock.Setup(i => i.AccreditationFees).ReturnsDbSet(_accreditationFeesMock.Object);

            var regulator = RegulatorType.Create("GB-ENG");
            var submissionDate = DateTime.Today;
            var groupId = (int)Group.Exporters;
            var subGroupId = (int)SubGroup.Aluminium;
            var tonnageBandId = (int)TonnageBands.Upto500;
            
            // Act
            var result = await _accreditationFeesRepository.GetFeeAsync(groupId, subGroupId, tonnageBandId, regulator, submissionDate, _cancellationToken);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetAccreditationFee_ValidInput_NoMatchCondition_ShouldReturnNull(
          [Frozen] Mock<IAppDbContext> _dataContextMock,
          [Greedy] AccreditationFeesRepository _accreditationFeesRepository)
        {
            // Arrange
            _accreditationFeesRepository = new AccreditationFeesRepository(_dataContextMock.Object);
            _accreditationFeesMock = MockIAccreditationFeeRepository.GetAccreditationFeesMock();
            _dataContextMock.Setup(i => i.AccreditationFees).ReturnsDbSet(_accreditationFeesMock.Object);

            var groupId = (int)Group.Exporters;
            var subGroupId = (int)SubGroup.Aluminium;
            var tonnageBandId = (int)TonnageBands.Upto500;

            // Act
            // Set submission date to mimimum value
            var result = await _accreditationFeesRepository.GetFeeAsync(groupId, subGroupId, tonnageBandId, RegulatorType.Create("GB-ENG"), DateTime.MinValue, _cancellationToken);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetAccreditationFeeAsync_ValidInput_EmptyData_ShouldReturnNull(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] AccreditationFeesRepository _accreditationFeesRepository)
        {
            // Arrange
            _accreditationFeesRepository = new AccreditationFeesRepository(_dataContextMock.Object);
            _accreditationFeesMock = MockIAccreditationFeeRepository.GetEmptyAccreditationFeesMock();
            _dataContextMock.Setup(i => i.AccreditationFees).ReturnsDbSet(_accreditationFeesMock.Object);

            var groupId = (int)Group.Exporters;
            var subGroupId = (int)SubGroup.Aluminium;
            var tonnageBandId = (int)TonnageBands.Upto500;

            // Act
            var result = await _accreditationFeesRepository.GetFeeAsync(groupId, subGroupId, tonnageBandId, RegulatorType.Create("GB-ENG"), DateTime.UtcNow, _cancellationToken);

            // Assert
            Assert.IsNull(result);
        }

    }
}