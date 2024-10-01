using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Constants.RegistrationFees.LookUps;
using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.Data.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.UnitTests.Mocks;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.EntityFrameworkCore;
using System.Data.Entity;

namespace EPR.Payment.Service.Data.UnitTests.Repositories.RegistrationFees
{
    [TestClass]
    public class ComplianceSchemeFeesRepositoryTests
    {
        private Mock<DbSet<Common.Data.DataModels.Lookups.RegistrationFees>> _registrationFeesMock = null!;
        private CancellationToken _cancellationToken;

        [TestInitialize]
        public void TestInitialize()
        {
            _cancellationToken = new CancellationToken();
            _registrationFeesMock = MockIRegistrationFeesRepository.GetRegistrationFeesMock();
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetBaseFeeAsync_ValidInput_ShouldReturnCurrentActiveBaseFee(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ComplianceSchemeFeesRepository _complianceSchemeFeesRepository)
        {
            // Arrange
            var feesMock = MockIRegistrationFeesRepository.GetRegistrationFeesMock();
            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(feesMock.Object);

            // Act
            var result = await _complianceSchemeFeesRepository.GetBaseFeeAsync(RegulatorType.Create("GB-ENG"), _cancellationToken);

            // Assert
            result.Should().Be(1500000m); // £13,804 represented in pence (1380400 pence)
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetBaseFeeAsync_InvalidInput_ShouldThrowArgumentException(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ComplianceSchemeFeesRepository _complianceSchemeFeesRepository)
        {
            // Arrange
            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(_registrationFeesMock.Object);

            // Act
            Func<Task> act = async () => await _complianceSchemeFeesRepository.GetBaseFeeAsync(RegulatorType.Create("GB-INVALID"), _cancellationToken);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Invalid regulator type: GB-INVALID. (Parameter 'regulator')"); // Include the parameter name in the expected message
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetBaseFeeAsync_EmptyDatabase_ShouldThrowKeyNotFoundException(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ComplianceSchemeFeesRepository _complianceSchemeFeesRepository)
        {
            // Arrange
            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(MockIRegistrationFeesRepository.GetEmptyRegistrationFeesMock().Object);

            // Act
            Func<Task> act = async () => await _complianceSchemeFeesRepository.GetBaseFeeAsync(RegulatorType.Create("GB-ENG"), _cancellationToken);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Base fee for compliance scheme with regulator 'GB-ENG' not found.");
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetBaseFeeAsync_OverlappingEffectiveDates_ShouldReturnCorrectFee(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ComplianceSchemeFeesRepository _complianceSchemeFeesRepository)
        {
            // Arrange
            var feesMock = MockIRegistrationFeesRepository.GetRegistrationFeesMock();
            var oldFee = await feesMock.Object.FirstAsync(f => f.SubGroup.Type == ComplianceSchemeConstants.Registration && f.Amount == 1380400m);
            var newFee = new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = oldFee.Group,
                SubGroup = oldFee.SubGroup,
                Regulator = oldFee.Regulator,
                Amount = 1500000m, // £15,000 represented in pence (1500000 pence)
                EffectiveFrom = DateTime.UtcNow.AddDays(-5),
                EffectiveTo = DateTime.UtcNow.AddDays(5)
            };

            var updatedFees = await feesMock.Object.ToListAsync();
            updatedFees.Add(newFee);

            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(updatedFees.AsQueryable());

            // Act
            var result = await _complianceSchemeFeesRepository.GetBaseFeeAsync(RegulatorType.Create("GB-ENG"), _cancellationToken);

            // Assert
            result.Should().Be(1500000m); // The most recent valid fee should be returned
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetBaseFeeAsync_InputInDifferentCase_ShouldReturnBaseFee(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ComplianceSchemeFeesRepository _complianceSchemeFeesRepository)
        {
            // Arrange
            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(_registrationFeesMock.Object);

            // Act
            var result = await _complianceSchemeFeesRepository.GetBaseFeeAsync(RegulatorType.Create("gb-eng"), _cancellationToken);

            // Assert
            result.Should().Be(1500000m); // Assuming this is the expected fee in pence for 'gb-eng'.
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetBaseFeeAsync_DayBeforeEffectiveFromDate_ShouldThrowKeyNotFoundException(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ComplianceSchemeFeesRepository _complianceSchemeFeesRepository)
        {
            // Arrange
            var futureFee = new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ComplianceScheme, Description = "Compliance Scheme" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type = ComplianceSchemeConstants.Registration, Description = "Registration" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type = "GB-ENG" },
                Amount = 1380400m,
                EffectiveFrom = DateTime.UtcNow.AddDays(1),
                EffectiveTo = DateTime.UtcNow.AddDays(10)
            };
            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(new[] { futureFee }.AsQueryable());

            // Act
            Func<Task> act = async () => await _complianceSchemeFeesRepository.GetBaseFeeAsync(RegulatorType.Create("GB-ENG"), _cancellationToken);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Base fee for compliance scheme with regulator 'GB-ENG' not found.");
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetBaseFeeAsync_DayAfterEffectiveToDate_ShouldThrowKeyNotFoundException(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ComplianceSchemeFeesRepository _complianceSchemeFeesRepository)
        {
            // Arrange
            var expiredFee = new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ComplianceScheme, Description = "Compliance Scheme" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type = ComplianceSchemeConstants.Registration, Description = "Registration" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type = "GB-ENG" },
                Amount = 1380400m,
                EffectiveFrom = DateTime.UtcNow.AddDays(-10),
                EffectiveTo = DateTime.UtcNow.AddDays(-1)
            };
            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(new[] { expiredFee }.AsQueryable());

            // Act
            Func<Task> act = async () => await _complianceSchemeFeesRepository.GetBaseFeeAsync(RegulatorType.Create("GB-ENG"), _cancellationToken);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Base fee for compliance scheme with regulator 'GB-ENG' not found.");
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetBaseFeeAsync_OnEffectiveFromDate_ShouldReturnBaseFee(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ComplianceSchemeFeesRepository _complianceSchemeFeesRepository)
        {
            // Arrange
            var today = DateTime.UtcNow.Date;
            var fee = new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ComplianceScheme, Description = "Compliance Scheme" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type = ComplianceSchemeConstants.Registration, Description = "Registration" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type = "GB-ENG", Description = "England" },
                Amount = 1380400m, // Random new fee to ensure this is picked up
                EffectiveFrom = today, // Active starting today
                EffectiveTo = today.AddDays(10)
            };

            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(new[] { fee }.AsQueryable());

            // Act
            var result = await _complianceSchemeFeesRepository.GetBaseFeeAsync(RegulatorType.Create("GB-ENG"), _cancellationToken);

            // Assert
            result.Should().Be(1380400m); // Fee should be returned since today matches EffectiveFrom
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetBaseFeeAsync_OnEffectiveToDate_ShouldReturnBaseFee(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ComplianceSchemeFeesRepository _complianceSchemeFeesRepository)
        {
            // Arrange
            var today = DateTime.UtcNow.Date;
            var fee = new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ComplianceScheme, Description = "Compliance Scheme" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type = ComplianceSchemeConstants.Registration, Description = "Registration" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type = "GB-ENG", Description = "England" },
                Amount = 1380400m, // Random new fee to ensure this is picked up
                EffectiveFrom = today.AddDays(-10), // Became active 10 days ago
                EffectiveTo = today // Expires today
            };

            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(new[] { fee }.AsQueryable());

            // Act
            var result = await _complianceSchemeFeesRepository.GetBaseFeeAsync(RegulatorType.Create("GB-ENG"), _cancellationToken);

            // Assert
            result.Should().Be(1380400m); // Fee should be returned since today matches EffectiveTo
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetBaseFeeAsync_FeeActiveForEntireDay_ShouldReturnBaseFee(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ComplianceSchemeFeesRepository _complianceSchemeFeesRepository)
        {
            // Arrange
            var today = DateTime.UtcNow.Date;
            var fee = new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ComplianceScheme, Description = "Compliance Scheme" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type = ComplianceSchemeConstants.Registration, Description = "Registration" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type = "GB-ENG", Description = "England" },
                Amount = 1380400m, // Random new fee to ensure this is picked up
                EffectiveFrom = today,
                EffectiveTo = today
            };

            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(new[] { fee }.AsQueryable());

            // Act
            var result = await _complianceSchemeFeesRepository.GetBaseFeeAsync(RegulatorType.Create("GB-ENG"), _cancellationToken);

            // Assert
            result.Should().Be(1380400m); // Fee should be active for the entire day
        }
    }
}
