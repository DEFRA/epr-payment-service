﻿using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Constants.RegistrationFees;
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
    public class ProducerFeesRepositoryTests
    {
        private Mock<DbSet<Common.Data.DataModels.Lookups.RegistrationFees>> _registrationFeesMock = null!;
        private CancellationToken _cancellationToken;

        [TestInitialize]
        public void TestInitialize()
        {
            _cancellationToken = new CancellationToken();
            _registrationFeesMock = MockIPaymentRepository.GetRegistrationFeesMock();
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetBaseFeeAsync_ValidInput_ShouldReturnCurrentActiveBaseFee(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ProducerFeesRepository _producerFeesRepository)
        {
            // Arrange
            var feesMock = MockIPaymentRepository.GetRegistrationFeesMock();
            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(feesMock.Object);

            // Act
            var result = await _producerFeesRepository.GetBaseFeeAsync("Large", RegulatorType.Create("GB-ENG"), _cancellationToken);

            // Assert
            result.Should().Be(262000m); // £2,620 represented in pence (262000 pence)
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetBaseFeeAsync_InvalidInput_ShouldThrowKeyNotFoundException(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ProducerFeesRepository _producerFeesRepository)
        {
            // Arrange
            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(_registrationFeesMock.Object);

            // Act
            Func<Task> act = async () => await _producerFeesRepository.GetBaseFeeAsync("InvalidType", RegulatorType.Create("GB-ENG"), _cancellationToken);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Base fee for producer type 'InvalidType' and regulator 'GB-ENG' not found.");
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetBaseFeeAsync_SmallProducer_ShouldReturnBaseFee(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ProducerFeesRepository _producerFeesRepository)
        {
            // Arrange
            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(_registrationFeesMock.Object);

            // Act
            var result = await _producerFeesRepository.GetBaseFeeAsync("Small", RegulatorType.Create("GB-ENG"), _cancellationToken);

            // Assert
            result.Should().Be(121600m); // £1,216 represented in pence (121600 pence)
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetBaseFeeAsync_EmptyDatabase_ShouldThrowKeyNotFoundException(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ProducerFeesRepository _producerFeesRepository)
        {
            // Arrange
            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(MockIPaymentRepository.GetEmptyRegistrationFeesMock().Object);

            // Act
            Func<Task> act = async () => await _producerFeesRepository.GetBaseFeeAsync("Large", RegulatorType.Create("GB-ENG"), _cancellationToken);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Base fee for producer type 'Large' and regulator 'GB-ENG' not found.");
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetBaseFeeAsync_OverlappingEffectiveDates_ShouldReturnCorrectFee(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ProducerFeesRepository _producerFeesRepository)
        {
            // Arrange
            var feesMock = MockIPaymentRepository.GetRegistrationFeesMock();
            var oldFee = feesMock?.Object?.First(f => f.SubGroup.Type == "Large" && f.Amount == 262000m);
            var newFee = new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = oldFee!.Group,
                SubGroup = oldFee.SubGroup,
                Regulator = oldFee.Regulator,
                Amount = 300000m, // £3,000 represented in pence (300000 pence)
                EffectiveFrom = DateTime.UtcNow.AddDays(-5),
                EffectiveTo = DateTime.UtcNow.AddDays(5)
            };

            var updatedFees = feesMock?.Object?.ToList();
            updatedFees?.Add(newFee);

            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(updatedFees?.AsQueryable());

            // Act
            var result = await _producerFeesRepository.GetBaseFeeAsync("Large", RegulatorType.Create("GB-ENG"), _cancellationToken);

            // Assert
            result.Should().Be(300000m); // The most recent valid fee should be returned
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetBaseFeeAsync_InputInDifferentCase_ShouldReturnBaseFee(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ProducerFeesRepository _producerFeesRepository)
        {
            // Arrange
            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(_registrationFeesMock.Object);

            // Act
            var result = await _producerFeesRepository.GetBaseFeeAsync("LARGE", RegulatorType.Create("GB-ENG"), _cancellationToken);

            // Assert
            result.Should().Be(262000m); // £2,620 represented in pence (262000 pence)
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetBaseFeeAsync_DayBeforeEffectiveFromDate_ShouldThrowKeyNotFoundException(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ProducerFeesRepository _producerFeesRepository)
        {
            // Arrange
            var futureFee = new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ProducerType, Description = "large" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type = "Large", Description = "large" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type = "GB-ENG" },
                Amount = 262000m,
                EffectiveFrom = DateTime.UtcNow.AddDays(1),
                EffectiveTo = DateTime.UtcNow.AddDays(10)
            };
            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(new[] { futureFee }.AsQueryable());

            // Act
            Func<Task> act = async () => await _producerFeesRepository.GetBaseFeeAsync("Large", RegulatorType.Create("GB-ENG"), _cancellationToken);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Base fee for producer type 'Large' and regulator 'GB-ENG' not found.");
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetBaseFeeAsync_DayAfterEffectiveToDate_ShouldThrowKeyNotFoundException(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ProducerFeesRepository _producerFeesRepository)
        {
            // Arrange
            var expiredFee = new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ProducerType, Description = "large" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type = "Large", Description = "large" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type = "GB-ENG" },
                Amount = 262000m,
                EffectiveFrom = DateTime.UtcNow.AddDays(-10),
                EffectiveTo = DateTime.UtcNow.AddDays(-1)
            };
            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(new[] { expiredFee }.AsQueryable());

            // Act
            Func<Task> act = async () => await _producerFeesRepository.GetBaseFeeAsync("Large", RegulatorType.Create("GB-ENG"), _cancellationToken);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Base fee for producer type 'Large' and regulator 'GB-ENG' not found.");
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetFirst20SubsidiariesFeeAsync_ValidInput_ShouldReturnFeePerSubsidiary(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ProducerFeesRepository _producerFeesRepository)
        {
            // Arrange
            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(_registrationFeesMock.Object);

            // Act
            var result = await _producerFeesRepository.GetFirst20SubsidiariesFeeAsync(RegulatorType.Create("GB-ENG"), _cancellationToken);

            // Assert
            result.Should().Be(55800m); // £558 represented in pence per subsidiary (55800 pence)
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetFirst20SubsidiariesFeeAsync_FutureEffectiveDate_ShouldThrowKeyNotFoundException(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ProducerFeesRepository _producerFeesRepository)
        {
            // Arrange
            var futureFee = new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ProducerSubsidiaries, Description = "ProducerSubsidiaries" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type = SubsidiariesConstants.UpTo20, Description = "Up to 20" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type = "GB-ENG" },
                Amount = 60000m, // £600 represented in pence (60000 pence)
                EffectiveFrom = DateTime.UtcNow.AddDays(5), // Not yet effective
                EffectiveTo = DateTime.UtcNow.AddDays(15)
            };
            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(new[] { futureFee }.AsQueryable());

            // Act
            Func<Task> act = async () => await _producerFeesRepository.GetFirst20SubsidiariesFeeAsync(RegulatorType.Create("GB-ENG"), _cancellationToken);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Subsidiaries fee for 'UpTo20' and regulator 'GB-ENG' not found.");
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetFirst20SubsidiariesFeeAsync_ExpiredEffectiveDate_ShouldThrowKeyNotFoundException(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ProducerFeesRepository _producerFeesRepository)
        {
            // Arrange
            var expiredFee = new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ProducerSubsidiaries, Description = "ProducerSubsidiaries" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type = SubsidiariesConstants.UpTo20, Description = "Up to 20" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type = "GB-ENG" },
                Amount = 55800m, // £558 represented in pence (55800 pence)
                EffectiveFrom = DateTime.UtcNow.AddDays(-20), // Expired
                EffectiveTo = DateTime.UtcNow.AddDays(-10)
            };
            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(new[] { expiredFee }.AsQueryable());

            // Act
            Func<Task> act = async () => await _producerFeesRepository.GetFirst20SubsidiariesFeeAsync(RegulatorType.Create("GB-ENG"), _cancellationToken);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Subsidiaries fee for 'UpTo20' and regulator 'GB-ENG' not found.");
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetFirst20SubsidiariesFeeAsync_OverlappingEffectiveDates_ShouldReturnCorrectFee(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ProducerFeesRepository _producerFeesRepository)
        {
            // Arrange
            var feesMock = MockIPaymentRepository.GetRegistrationFeesMock();
            var oldFee = feesMock.Object.First(f => f.SubGroup.Type == SubsidiariesConstants.UpTo20 && f.Amount == 55800m);
            var newFee = new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = oldFee.Group,
                SubGroup = oldFee.SubGroup,
                Regulator = oldFee.Regulator,
                Amount = 60000m, // Newer amount
                EffectiveFrom = DateTime.UtcNow.AddDays(-5), // More recent effective date
                EffectiveTo = DateTime.UtcNow.AddDays(5) // Still within the valid period
            };

            var updatedFees = feesMock.Object.ToList();
            updatedFees.Add(newFee);

            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(updatedFees.AsQueryable());

            // Act
            var result = await _producerFeesRepository.GetFirst20SubsidiariesFeeAsync(RegulatorType.Create("GB-ENG"), _cancellationToken);

            // Assert
            result.Should().Be(60000m); // The most recent valid fee should be returned
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetAdditionalSubsidiariesFeeAsync_ValidInput_ShouldReturnFeePerSubsidiary(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ProducerFeesRepository _producerFeesRepository)
        {
            // Arrange
            var feesMock = MockIPaymentRepository.GetRegistrationFeesMock();
            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(feesMock.Object);

            // Act
            var result = await _producerFeesRepository.GetAdditionalSubsidiariesFeeAsync(RegulatorType.Create("GB-ENG"), _cancellationToken);

            // Assert
            result.Should().Be(14000m); // £140 represented in pence (14000 pence)
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetAdditionalSubsidiariesFeeAsync_NoValidRecord_ShouldThrowKeyNotFoundException(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ProducerFeesRepository _producerFeesRepository)
        {
            // Arrange
            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(MockIPaymentRepository.GetEmptyRegistrationFeesMock().Object);

            // Act
            Func<Task> act = async () => await _producerFeesRepository.GetAdditionalSubsidiariesFeeAsync(RegulatorType.Create("GB-ENG"), _cancellationToken);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Subsidiaries fee for 'MoreThan20' and regulator 'GB-ENG' not found.");
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetAdditionalSubsidiariesFeeAsync_FutureEffectiveDate_ShouldThrowKeyNotFoundException(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ProducerFeesRepository _producerFeesRepository)
        {
            // Arrange
            var futureFee = new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ProducerSubsidiaries, Description = "ProducerSubsidiaries" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type = SubsidiariesConstants.MoreThan20, Description = "More than 20" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type = "GB-ENG" },
                Amount = 16000m, // £160 represented in pence (16000 pence), future fee
                EffectiveFrom = DateTime.UtcNow.AddDays(5), // Not yet effective
                EffectiveTo = DateTime.UtcNow.AddDays(15)
            };
            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(new[] { futureFee }.AsQueryable());

            // Act
            Func<Task> act = async () => await _producerFeesRepository.GetAdditionalSubsidiariesFeeAsync(RegulatorType.Create("GB-ENG"), _cancellationToken);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Subsidiaries fee for 'MoreThan20' and regulator 'GB-ENG' not found.");
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetAdditionalSubsidiariesFeeAsync_ExpiredEffectiveDate_ShouldThrowKeyNotFoundException(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ProducerFeesRepository _producerFeesRepository)
        {
            // Arrange
            var expiredFee = new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ProducerSubsidiaries, Description = "ProducerSubsidiaries" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type = SubsidiariesConstants.MoreThan20, Description = "More than 20" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type = "GB-ENG" },
                Amount = 14000m, // £140 represented in pence (14000 pence), expired fee
                EffectiveFrom = DateTime.UtcNow.AddDays(-20), // Expired 20 days ago
                EffectiveTo = DateTime.UtcNow.AddDays(-10) // Expired 10 days ago
            };

            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(new[] { expiredFee }.AsQueryable());

            // Act
            Func<Task> act = async () => await _producerFeesRepository.GetAdditionalSubsidiariesFeeAsync(RegulatorType.Create("GB-ENG"), _cancellationToken);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Subsidiaries fee for 'MoreThan20' and regulator 'GB-ENG' not found.");
        }


        [TestMethod]
        [AutoMoqData]
        public async Task GetAdditionalSubsidiariesFeeAsync_OverlappingEffectiveDates_ShouldReturnCorrectFee(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ProducerFeesRepository _producerFeesRepository)
        {
            // Arrange
            var feesMock = MockIPaymentRepository.GetRegistrationFeesMock();
            var oldFee = feesMock.Object.First(f => f.SubGroup.Type == SubsidiariesConstants.MoreThan20 && f.Amount == 14000m);
            var newFee = new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = oldFee.Group,
                SubGroup = oldFee.SubGroup,
                Regulator = oldFee.Regulator,
                Amount = 16000m, // Newer amount
                EffectiveFrom = DateTime.UtcNow.AddDays(-5), // More recent effective date
                EffectiveTo = DateTime.UtcNow.AddDays(5) // Still within the valid period
            };

            var updatedFees = feesMock.Object.ToList();
            updatedFees.Add(newFee);

            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(updatedFees.AsQueryable());

            // Act
            var result = await _producerFeesRepository.GetAdditionalSubsidiariesFeeAsync(RegulatorType.Create("GB-ENG"), _cancellationToken);

            // Assert
            result.Should().Be(16000m); // The most recent valid fee should be returned
        }
    }
}