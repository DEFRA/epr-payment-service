﻿using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Constants.RegistrationFees.LookUps;
using EPR.Payment.Service.Common.Data.Helper;
using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.Data.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.UnitTests.Mocks;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using FluentAssertions;
using FluentAssertions.Execution;
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
            _registrationFeesMock = MockIRegistrationFeesRepository.GetRegistrationFeesMock();
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetBaseFeeAsync_ValidInput_ShouldReturnCurrentActiveBaseFee(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ProducerFeesRepository _producerFeesRepository)
        {
            // Arrange
            var feesMock = MockIRegistrationFeesRepository.GetRegistrationFeesMock();
            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(feesMock.Object);

            // Act
            var result = await _producerFeesRepository.GetBaseFeeAsync("Large", RegulatorType.Create("GB-ENG"), DateTime.UtcNow, _cancellationToken);

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
            Func<Task> act = async () => await _producerFeesRepository.GetBaseFeeAsync("InvalidType", RegulatorType.Create("GB-ENG"), DateTime.UtcNow, _cancellationToken);

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
            var result = await _producerFeesRepository.GetBaseFeeAsync("Small", RegulatorType.Create("GB-ENG"), DateTime.UtcNow, _cancellationToken);

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
            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(MockIRegistrationFeesRepository.GetEmptyRegistrationFeesMock().Object);

            // Act
            Func<Task> act = async () => await _producerFeesRepository.GetBaseFeeAsync("Large", RegulatorType.Create("GB-ENG"), DateTime.UtcNow, _cancellationToken);

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
            var feesMock = MockIRegistrationFeesRepository.GetRegistrationFeesMock();
            var oldFee = await feesMock.Object.FirstAsync(f => f.SubGroup.Type == "Large" && f.Amount == 262000m);
            var newFee = new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = oldFee!.Group,
                SubGroup = oldFee.SubGroup,
                Regulator = oldFee.Regulator,
                Amount = 300000m, // £3,000 represented in pence (300000 pence)
                EffectiveFrom = DateTime.UtcNow.AddDays(-5),
                EffectiveTo = DateTime.UtcNow.AddDays(5)
            };

            var updatedFees = await feesMock.Object.ToListAsync();
            updatedFees?.Add(newFee);

            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(updatedFees?.AsQueryable());

            // Act
            var result = await _producerFeesRepository.GetBaseFeeAsync("Large", RegulatorType.Create("GB-ENG"), DateTime.UtcNow, _cancellationToken);

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
            var result = await _producerFeesRepository.GetBaseFeeAsync("LARGE", RegulatorType.Create("GB-ENG"), DateTime.UtcNow, _cancellationToken);

            // Assert
            result.Should().Be(262000m); // £2,620 represented in pence (262000 pence)
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetBaseFeeAsync_DayBeforeEffectiveFromDate_ShouldThrowArgumentException(
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
            Func<Task> act = async () => await _producerFeesRepository.GetBaseFeeAsync("Large", RegulatorType.Create("GB-ENG"), DateTime.UtcNow, _cancellationToken);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage(ValidationMessages.SubmissionDateIsNotInRange);
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetBaseFeeAsync_InvalidSubmissionDate_ShouldThrowArgumentException(
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
            Func<Task> act = async () => await _producerFeesRepository.GetBaseFeeAsync("Large", RegulatorType.Create("GB-ENG"), DateTime.UtcNow, _cancellationToken);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage(ValidationMessages.SubmissionDateIsNotInRange);
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetOnlineMarketFeeAsync_ValidInput_ShouldReturnOnlineMarketFee(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ProducerFeesRepository _producerFeesRepository)
        {
            // Arrange
            var feesMock = MockIRegistrationFeesRepository.GetRegistrationFeesMock();
            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(feesMock.Object);

            // Act
            var result = await _producerFeesRepository.GetOnlineMarketFeeAsync(RegulatorType.Create("GB-ENG"), DateTime.UtcNow, _cancellationToken);

            // Assert
            result.Should().Be(257900m); // £2,579 represented in pence (257900 pence)
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetOnlineMarketFeeAsync_EmptyDatabase_ShouldThrowKeyNotFoundException(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ProducerFeesRepository _producerFeesRepository)
        {
            // Arrange
            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(MockIRegistrationFeesRepository.GetEmptyRegistrationFeesMock().Object);

            // Act
            Func<Task> act = async () => await _producerFeesRepository.GetOnlineMarketFeeAsync(RegulatorType.Create("GB-ENG"), DateTime.UtcNow, _cancellationToken);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage(string.Format(ProducerFeesRepositoryConstants.InvalidOnlineMarketRegulatorError, "GB-ENG"));
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetOnlineMarketFeeAsync_DayBeforeEffectiveFromDate_ShouldThrowArgumentException(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ProducerFeesRepository _producerFeesRepository)
        {
            // Arrange
            var futureFee = new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ProducerType, Description = "Producer Type" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type = SubGroupTypeConstants.OnlineMarket, Description = "Online Market" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type = "GB-ENG" },
                Amount = 257900m,
                EffectiveFrom = DateTime.UtcNow.AddDays(1),
                EffectiveTo = DateTime.UtcNow.AddDays(10)
            };
            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(new[] { futureFee }.AsQueryable());

            // Act
            Func<Task> act = async () => await _producerFeesRepository.GetOnlineMarketFeeAsync(RegulatorType.Create("GB-ENG"), DateTime.UtcNow, _cancellationToken);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage(ValidationMessages.SubmissionDateIsNotInRange);
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetOnlineMarketFeeAsync_DayAfterEffectiveToDate_ShouldThrowArgumentException(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ProducerFeesRepository _producerFeesRepository)
        {
            // Arrange

            var expiredFee = new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ProducerType, Description = "Producer Type" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type = SubGroupTypeConstants.OnlineMarket, Description = "Online Market" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type = "GB-ENG" },
                Amount = 257900m,
                EffectiveFrom = DateTime.UtcNow.AddDays(-10),
                EffectiveTo = DateTime.UtcNow.AddDays(-1)
            };

            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(new[] { expiredFee }.AsQueryable());

            // Act
            Func<Task> act = async () => await _producerFeesRepository.GetOnlineMarketFeeAsync(RegulatorType.Create("GB-ENG"), DateTime.UtcNow, _cancellationToken);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage(ValidationMessages.SubmissionDateIsNotInRange);
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetFirstBandFeeAsync_ValidInput_ShouldReturnFeePerSubsidiary(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ProducerFeesRepository _producerFeesRepository)
        {
            // Arrange
            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(_registrationFeesMock.Object);

            // Act
            var result = await _producerFeesRepository.GetFirstBandFeeAsync(RegulatorType.Create("GB-ENG"), DateTime.UtcNow, _cancellationToken);

            // Assert
            result.Should().Be(55800m); // £558 represented in pence per subsidiary (55800 pence)
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetFirstBandFeeAsync_FutureEffectiveDate_ShouldThrowArgumentException(
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
            Func<Task> act = async () => await _producerFeesRepository.GetFirstBandFeeAsync(RegulatorType.Create("GB-ENG"), DateTime.UtcNow, _cancellationToken);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage(ValidationMessages.SubmissionDateIsNotInRange);
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetFirstBandFeeAsync_ExpiredEffectiveDate_ShouldThrowArgumentException(
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
            Func<Task> act = async () => await _producerFeesRepository.GetFirstBandFeeAsync(RegulatorType.Create("GB-ENG"), DateTime.UtcNow, _cancellationToken);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage(ValidationMessages.SubmissionDateIsNotInRange);
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetFirstBandFeeAsync_OverlappingEffectiveDates_ShouldReturnCorrectFee(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ProducerFeesRepository _producerFeesRepository)
        {
            // Arrange
            var feesMock = MockIRegistrationFeesRepository.GetRegistrationFeesMock();
            var oldFee = await feesMock.Object.FirstAsync(f => f.SubGroup.Type == SubsidiariesConstants.UpTo20 && f.Amount == 55800m);
            var newFee = new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = oldFee.Group,
                SubGroup = oldFee.SubGroup,
                Regulator = oldFee.Regulator,
                Amount = 60000m, // Newer amount
                EffectiveFrom = DateTime.UtcNow.AddDays(-5), // More recent effective date
                EffectiveTo = DateTime.UtcNow.AddDays(5) // Still within the valid period
            };

            var updatedFees = await feesMock.Object.ToListAsync();
            updatedFees.Add(newFee);

            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(updatedFees.AsQueryable());

            // Act
            var result = await _producerFeesRepository.GetFirstBandFeeAsync(RegulatorType.Create("GB-ENG"), DateTime.UtcNow, _cancellationToken);

            // Assert
            result.Should().Be(60000m); // The most recent valid fee should be returned
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetSecondBandFeeAsync_ValidInput_ShouldReturnFeePerSubsidiary(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ProducerFeesRepository _producerFeesRepository)
        {
            // Arrange
            var feesMock = MockIRegistrationFeesRepository.GetRegistrationFeesMock();
            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(feesMock.Object);

            // Act
            var result = await _producerFeesRepository.GetSecondBandFeeAsync(RegulatorType.Create("GB-ENG"), DateTime.UtcNow, _cancellationToken);

            // Assert
            result.Should().Be(14000m); // £140 represented in pence (14000 pence)
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetSecondBandFeeAsync_NoValidRecord_ShouldThrowKeyNotFoundException(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ProducerFeesRepository _producerFeesRepository)
        {
            // Arrange
            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(MockIRegistrationFeesRepository.GetEmptyRegistrationFeesMock().Object);

            // Act
            Func<Task> act = async () => await _producerFeesRepository.GetSecondBandFeeAsync(RegulatorType.Create("GB-ENG"), DateTime.UtcNow, _cancellationToken);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Subsidiaries fee for 'MoreThan20' and regulator 'GB-ENG' not found.");
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetSecondBandFeeAsync_FutureEffectiveDate_ShouldThrowArgumentException(
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
            Func<Task> act = async () => await _producerFeesRepository.GetSecondBandFeeAsync(RegulatorType.Create("GB-ENG"), DateTime.UtcNow, _cancellationToken);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage(ValidationMessages.SubmissionDateIsNotInRange);
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetSecondBandFeeAsync_ExpiredEffectiveDate_ShouldThrowArgumentException(
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
            Func<Task> act = async () => await _producerFeesRepository.GetSecondBandFeeAsync(RegulatorType.Create("GB-ENG"), DateTime.UtcNow, _cancellationToken);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage(ValidationMessages.SubmissionDateIsNotInRange);
        }


        [TestMethod]
        [AutoMoqData]
        public async Task GetSecondBandFeeAsync_OverlappingEffectiveDates_ShouldReturnCorrectFee(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ProducerFeesRepository _producerFeesRepository)
        {
            // Arrange
            var feesMock = MockIRegistrationFeesRepository.GetRegistrationFeesMock();
            var oldFee = await feesMock.Object.FirstAsync(f => f.SubGroup.Type == SubsidiariesConstants.MoreThan20 && f.Amount == 14000m);
            var newFee = new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = oldFee.Group,
                SubGroup = oldFee.SubGroup,
                Regulator = oldFee.Regulator,
                Amount = 16000m, // Newer amount
                EffectiveFrom = DateTime.UtcNow.AddDays(-5), // More recent effective date
                EffectiveTo = DateTime.UtcNow.AddDays(5) // Still within the valid period
            };

            var updatedFees = await feesMock.Object.ToListAsync();
            updatedFees.Add(newFee);

            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(updatedFees.AsQueryable());

            // Act
            var result = await _producerFeesRepository.GetSecondBandFeeAsync(RegulatorType.Create("GB-ENG"), DateTime.UtcNow, _cancellationToken);

            // Assert
            result.Should().Be(16000m); // The most recent valid fee should be returned
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetThirdBandFeeAsync_ValidInput_ShouldReturnFeePerSubsidiary(
           [Frozen] Mock<IAppDbContext> _dataContextMock,
           [Greedy] ProducerFeesRepository _producerFeesRepository)
        {
            // Arrange
            var feesMock = MockIRegistrationFeesRepository.GetRegistrationFeesMock();
            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(feesMock.Object);

            // Act
            var result = await _producerFeesRepository.GetThirdBandFeeAsync(RegulatorType.Create("GB-ENG"), DateTime.UtcNow, _cancellationToken);

            // Assert
            result.Should().Be(1m);
        }


        [TestMethod]
        [AutoMoqData]
        public async Task GetThirdBandFeeAsync_OverlappingEffectiveDates_ShouldReturnCorrectFee(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ProducerFeesRepository _producerFeesRepository)
        {
            // Arrange
            var feesMock = MockIRegistrationFeesRepository.GetRegistrationFeesMock();
            var oldFee = await feesMock.Object.FirstAsync(f => f.SubGroup.Type == SubsidiariesConstants.MoreThan100 && f.Amount == 1m);
            var newFee = new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = oldFee.Group,
                SubGroup = oldFee.SubGroup,
                Regulator = oldFee.Regulator,
                Amount = 1m, // Newer amount
                EffectiveFrom = DateTime.UtcNow.AddDays(-5), // More recent effective date
                EffectiveTo = DateTime.UtcNow.AddDays(5) // Still within the valid period
            };

            var updatedFees = await feesMock.Object.ToListAsync();
            updatedFees.Add(newFee);

            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(updatedFees.AsQueryable());

            // Act
            var result = await _producerFeesRepository.GetThirdBandFeeAsync(RegulatorType.Create("GB-ENG"), DateTime.UtcNow, _cancellationToken);

            // Assert
            result.Should().Be(1m); // The most recent valid fee should be returned
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetBaseFeeAsync_OnEffectiveFromDate_ShouldReturnBaseFee(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ProducerFeesRepository _producerFeesRepository)
        {
            // Arrange
            var today = DateTime.UtcNow;
            var fee = new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ProducerType, Description = "Producer Type" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type = "Large", Description = "Large producer" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type = "GB-ENG", Description = "England" },
                Amount = 202000m, // Random new fee to ensure this is picked up
                EffectiveFrom = today, // Active starting today
                EffectiveTo = today.AddDays(10)
            };

            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(new[] { fee }.AsQueryable());

            // Act
            var result = await _producerFeesRepository.GetBaseFeeAsync("Large", RegulatorType.Create("GB-ENG"), DateTime.UtcNow, _cancellationToken);

            // Assert
            result.Should().Be(202000m); // Fee should be returned since today matches EffectiveFrom
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetBaseFeeAsync_OnEffectiveToDate_ShouldReturnBaseFee(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ProducerFeesRepository _producerFeesRepository)
        {
            // Arrange
            var today = DateTime.UtcNow;
            var fee = new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ProducerType, Description = "Producer Type" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type = "Large", Description = "Large producer" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type = "GB-ENG", Description = "England" },
                Amount = 302000m, // Random new fee to ensure this is picked up
                EffectiveFrom = today.AddDays(-10), // Became active 10 days ago
                EffectiveTo = today.AddDays(10) // Expires today
            };

            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(new[] { fee }.AsQueryable());

            // Act
            var result = await _producerFeesRepository.GetBaseFeeAsync("Large", RegulatorType.Create("GB-ENG"), today, _cancellationToken);

            // Assert
            result.Should().Be(302000m); // Fee should be returned since today matches EffectiveTo
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetBaseFeeAsync_FeeActiveForEntireDay_ShouldReturnBaseFee(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ProducerFeesRepository _producerFeesRepository)
        {
            // Arrange
            var today = DateTime.UtcNow;
            var fee = new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ProducerType, Description = "Producer Type" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type = "Large", Description = "Large producer" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type = "GB-ENG", Description = "England" },
                Amount = 102000m, // Random new fee to ensure this is picked up
                EffectiveFrom = today.AddDays(-10),
                EffectiveTo = today.AddDays(10)
            };

            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(new[] { fee }.AsQueryable());

            // Act
            var result = await _producerFeesRepository.GetBaseFeeAsync("Large", RegulatorType.Create("GB-ENG"), today, _cancellationToken);

            // Assert
            result.Should().Be(102000m); // Fee should be active for the entire day
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetFirstBandFeeAsync_OnEffectiveFromDate_ShouldReturnFee(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ProducerFeesRepository _producerFeesRepository)
        {
            // Arrange
            var today = DateTime.UtcNow;
            var fee = new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ProducerSubsidiaries, Description = "Producer Subsidiaries" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type = SubsidiariesConstants.UpTo20, Description = "Up to 20" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type = "GB-ENG", Description = "England" },
                Amount = 55000m, // New fee to ensure this is picked up
                EffectiveFrom = today, // Active starting today
                EffectiveTo = today.AddDays(10)
            };

            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(new[] { fee }.AsQueryable());

            // Act
            var result = await _producerFeesRepository.GetFirstBandFeeAsync(RegulatorType.Create("GB-ENG"), DateTime.UtcNow, _cancellationToken);

            // Assert
            result.Should().Be(55000m); // Fee should be returned since today matches EffectiveFrom
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetFirstBandFeeAsync_OnEffectiveToDate_ShouldReturnFee(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ProducerFeesRepository _producerFeesRepository)
        {
            // Arrange
            var today = DateTime.UtcNow;
            var fee = new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ProducerSubsidiaries, Description = "Producer Subsidiaries" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type = SubsidiariesConstants.UpTo20, Description = "Up to 20" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type = "GB-ENG", Description = "England" },
                Amount = 56000m, // New fee to ensure this is picked up
                EffectiveFrom = today.AddDays(-10), // Became active 10 days ago
                EffectiveTo = today.AddDays(10) // Expires today
            };

            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(new[] { fee }.AsQueryable());

            // Act
            var result = await _producerFeesRepository.GetFirstBandFeeAsync(RegulatorType.Create("GB-ENG"), today, _cancellationToken);

            // Assert
            result.Should().Be(56000m); // Fee should be returned since today matches EffectiveTo
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetFirstBandFeeAsync_FeeActiveForEntireDay_ShouldReturnFee(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ProducerFeesRepository _producerFeesRepository)
        {
            // Arrange
            var today = DateTime.UtcNow;
            var fee = new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ProducerSubsidiaries, Description = "Producer Subsidiaries" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type = SubsidiariesConstants.UpTo20, Description = "Up to 20" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type = "GB-ENG", Description = "England" },
                Amount = 57000m, // New fee to ensure this is picked up
                EffectiveFrom = today.AddDays(-10),
                EffectiveTo = today.AddDays(10)
            };

            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(new[] { fee }.AsQueryable());

            // Act
            var result = await _producerFeesRepository.GetFirstBandFeeAsync(RegulatorType.Create("GB-ENG"), today, _cancellationToken);

            // Assert
            result.Should().Be(57000m); // Fee should be active for the entire day
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetSecondBandFeeAsync_OnEffectiveFromDate_ShouldReturnFee(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ProducerFeesRepository _producerFeesRepository)
        {
            // Arrange
            var today = DateTime.UtcNow;
            var fee = new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ProducerSubsidiaries, Description = "Producer Subsidiaries" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type = SubsidiariesConstants.MoreThan20, Description = "More than 20" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type = "GB-ENG", Description = "England" },
                Amount = 15000m, // New fee to ensure this is picked up
                EffectiveFrom = today, // Active starting today
                EffectiveTo = today.AddDays(10)
            };

            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(new[] { fee }.AsQueryable());

            // Act
            var result = await _producerFeesRepository.GetSecondBandFeeAsync(RegulatorType.Create("GB-ENG"), DateTime.UtcNow, _cancellationToken);

            // Assert
            result.Should().Be(15000m); // Fee should be returned since today matches EffectiveFrom
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetSecondBandFeeAsync_OnEffectiveToDate_ShouldReturnFee(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ProducerFeesRepository _producerFeesRepository)
        {
            // Arrange
            var today = DateTime.UtcNow;
            var fee = new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ProducerSubsidiaries, Description = "Producer Subsidiaries" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type = SubsidiariesConstants.MoreThan20, Description = "More than 20" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type = "GB-ENG", Description = "England" },
                Amount = 16000m, // New fee to ensure this is picked up
                EffectiveFrom = today.AddDays(-10), // Became active 10 days ago
                EffectiveTo = today.AddDays(10) // Expires today
            };

            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(new[] { fee }.AsQueryable());

            // Act
            var result = await _producerFeesRepository.GetSecondBandFeeAsync(RegulatorType.Create("GB-ENG"), today, _cancellationToken);

            // Assert
            result.Should().Be(16000m); // Fee should be returned since today matches EffectiveTo
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetSecondBandFeeAsync_FeeActiveForEntireDay_ShouldReturnFee(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ProducerFeesRepository _producerFeesRepository)
        {
            // Arrange
            var today = DateTime.UtcNow;
            var fee = new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ProducerSubsidiaries, Description = "Producer Subsidiaries" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type = SubsidiariesConstants.MoreThan20, Description = "More than 20" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type = "GB-ENG", Description = "England" },
                Amount = 17000m, // New fee to ensure this is picked up
                EffectiveFrom = today.AddDays(-10),
                EffectiveTo = today.AddDays(10)
            };

            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(new[] { fee }.AsQueryable());

            // Act
            var result = await _producerFeesRepository.GetSecondBandFeeAsync(RegulatorType.Create("GB-ENG"), today, _cancellationToken);

            // Assert
            result.Should().Be(17000m); // Fee should be active for the entire day
        }

        [TestMethod, AutoMoqData]
        public async Task GetResubmissionAsync_RegistrationFeesExist_ShouldReturnAmount(
           [Frozen] Mock<IAppDbContext> _dataContextMock,
           [Frozen] Mock<FeesKeyValueStore> _keyValueStore,
           [Greedy] ProducerFeesRepository _producerFeesRepository)
        {
            //Arrange
            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(_registrationFeesMock.Object);
            _producerFeesRepository = new ProducerFeesRepository(_dataContextMock.Object, _keyValueStore.Object);

            var regulator = RegulatorType.Create("GB-ENG");
            var submissionDate = DateTime.Today;

            //Act
            var result = await _producerFeesRepository.GetResubmissionAsync(regulator, submissionDate, _cancellationToken);

            //Assert
            using (new AssertionScope())
            {
                result.Should().Be(100);
            }
        }

        [TestMethod, AutoMoqData]
        public async Task GetResubmissionAsync_RegistrationFeesDoesNotExist_ShouldThrowKeyNotFoundException(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Frozen] Mock<FeesKeyValueStore> _keyValueStore,
            [Greedy] ProducerFeesRepository _producerFeesRepository)
        {
            //Arrange
            var regulator = RegulatorType.Create("GB-SCT");
            var submissionDate = DateTime.Today;
            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(_registrationFeesMock.Object);
            _producerFeesRepository = new ProducerFeesRepository(_dataContextMock.Object, _keyValueStore.Object);

            //Act & Assert
            await _producerFeesRepository.Invoking(async x => await x.GetResubmissionAsync(regulator, submissionDate, _cancellationToken))
                .Should().ThrowAsync<KeyNotFoundException>();
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetLateFeeAsync_ValidInput_ShouldReturnLateFee(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ProducerFeesRepository _producerFeesRepository)
        {
            // Arrange
            var feesMock = MockIRegistrationFeesRepository.GetRegistrationFeesMock();
            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(feesMock.Object);

            // Act
            var result = await _producerFeesRepository.GetLateFeeAsync(RegulatorType.Create("GB-ENG"), DateTime.UtcNow, _cancellationToken);

            // Assert
            result.Should().Be(33200m); // £332 represented in pence (33200 pence)
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetLateFeeAsync_EmptyDatabase_ShouldThrowKeyNotFoundException(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ProducerFeesRepository _producerFeesRepository)
        {
            // Arrange
            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(MockIRegistrationFeesRepository.GetEmptyRegistrationFeesMock().Object);

            // Act
            Func<Task> act = async () => await _producerFeesRepository.GetLateFeeAsync(RegulatorType.Create("GB-ENG"), DateTime.UtcNow, _cancellationToken);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Producer Late Fee record not found for regulator: 'GB-ENG'");
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetLateFeeAsync_DayBeforeEffectiveFromDate_ShouldThrowArgumentException(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ProducerFeesRepository _producerFeesRepository)
        {
            // Arrange
            var futureFee = new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ProducerType, Description = "Producer Type" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type = SubGroupTypeConstants.LateFee, Description = "Late Fee" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type = "GB-ENG" },
                Amount = 257900m,
                EffectiveFrom = DateTime.UtcNow.AddDays(1),
                EffectiveTo = DateTime.UtcNow.AddDays(10)
            };
            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(new[] { futureFee }.AsQueryable());

            // Act
            Func<Task> act = async () => await _producerFeesRepository.GetLateFeeAsync(RegulatorType.Create("GB-ENG"), DateTime.UtcNow, _cancellationToken);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage(ValidationMessages.SubmissionDateIsNotInRange);
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetLateFeeAsync_DayAfterEffectiveToDate_ShouldThrowArgumentException(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] ProducerFeesRepository _producerFeesRepository)
        {
            // Arrange

            var expiredFee = new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ProducerType, Description = "Producer Type" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type = SubGroupTypeConstants.LateFee, Description = "Late Fee" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type = "GB-ENG" },
                Amount = 257900m,
                EffectiveFrom = DateTime.UtcNow.AddDays(-10),
                EffectiveTo = DateTime.UtcNow.AddDays(-1)
            };

            _dataContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(new[] { expiredFee }.AsQueryable());

            // Act
            Func<Task> act = async () => await _producerFeesRepository.GetLateFeeAsync(RegulatorType.Create("GB-ENG"), DateTime.UtcNow, _cancellationToken);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage(ValidationMessages.SubmissionDateIsNotInRange);
        }

    }
}