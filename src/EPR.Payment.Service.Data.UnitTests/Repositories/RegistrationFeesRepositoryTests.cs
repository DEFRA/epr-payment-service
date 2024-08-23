using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.Data.Repositories;
using EPR.Payment.Service.Common.UnitTests.Mocks;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.EntityFrameworkCore;
using System.Data.Entity;

namespace EPR.Payment.Service.Data.UnitTests.Repositories
{
    [TestClass]
    public class RegistrationFeesRepositoryTests
    {
        private Mock<DbSet<Common.Data.DataModels.Lookups.RegistrationFees>> _registrationFeesMock = null!;
        private CancellationToken _cancellationToken;

        [TestInitialize]
        public void TestInitialize()
        {
            _registrationFeesMock = MockIRegistrationFeesRepository.GetRegistrationFeesMock();
            _cancellationToken = new CancellationToken();
        }

        [TestMethod, AutoMoqData]
        public void Constructor_WhenAllDependenciesAreNotNull_ShouldCreateInstance(
             [Frozen] Mock<IAppDbContext> _dataContextMock
            )
        {
            // Act
            var repo = new RegistrationFeesRepository(
                _dataContextMock.Object);

            // Assert
            repo.Should().NotBeNull();
        }

        [TestMethod]
        public void Constructor_WhenDataContextIsNull_ShouldThrowArgumentNullException()
        {
            // Act
            Action act = () => new RegistrationFeesRepository(
                null!);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithParameterName("dataContext");
        }

        [TestMethod, AutoMoqData]
        public async Task GetProducerResubmissionAmountByRegulatorAsync_RegistrationFeesExist_ShouldReturnAmount(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] RegistrationFeesRepository _registrationFeesRepository)
        {
            //Arrange
            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(_registrationFeesMock.Object);
            _registrationFeesRepository = new RegistrationFeesRepository(_dataContextMock.Object);

            var regulator = "Test-Regulator-1";

            //Act
            var result = await _registrationFeesRepository.GetProducerResubmissionAmountByRegulatorAsync(regulator, _cancellationToken);

            //Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.Should().Be(100);
            }
        }

        [TestMethod, AutoMoqData]
        public async Task GetProducerResubmissionAmountByRegulatorAsync_RegistrationFeesDoesNotExist_ShouldThrowKeyNotFoundException(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] RegistrationFeesRepository _registrationFeesRepository)
        {
            //Arrange
            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(_registrationFeesMock.Object);
            _registrationFeesRepository = new RegistrationFeesRepository(_dataContextMock.Object);

            var regulator = "Test-Reg";

            //Act & Assert
            await _registrationFeesRepository.Invoking(async x => await x.GetProducerResubmissionAmountByRegulatorAsync(regulator, _cancellationToken))
                .Should().ThrowAsync<KeyNotFoundException>();
        }
    }
}
