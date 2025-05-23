using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Fees;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Payments;
using EPR.Payment.Service.Common.Dtos.Request.AccreditationFees;
using EPR.Payment.Service.Common.Dtos.Response.AccreditationFees;
using EPR.Payment.Service.Common.Enums;
using EPR.Payment.Service.Common.Extensions;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Helper;
using EPR.Payment.Service.Services.AccreditationFees;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;

namespace EPR.Payment.Service.UnitTests.Services.AccreditationFees
{
    [TestClass]
    public class AccreditationFeesCalculatorServiceTests
    {
        private readonly Mock<IAccreditationFeesRepository> _accreditationFeesRepositoryMock = new();
        private readonly Mock<IPaymentsRepository> _paymentsRepositoryMock = new();

        private AccreditationFeesCalculatorService? _accreditationFeesCalculatorServiceUnderTest = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _accreditationFeesCalculatorServiceUnderTest = new AccreditationFeesCalculatorService(
                _accreditationFeesRepositoryMock.Object,
                _paymentsRepositoryMock.Object);
        }

        [TestMethod]
        public async Task CalculateFeesAsync_ShouldCallRespositoryAndReturnNullResponse_WhenAccreditationFeeRecordNotFound()
        {
            // Arrange
            AccreditationFeesRequestDto accreditationFeesRequestDto = new()
            {
                Regulator = RegulatorConstants.GBENG,
                MaterialType = AccreditationFeesMaterialType.Plastic,
                RequestorType = AccreditationFeesRequestorType.Exporters,
                NumberOfOverseasSites = 10,
                TonnageBand = TonnageBand.Upto500,
                SubmissionDate = DateTime.UtcNow,
                ApplicationReferenceNumber = Guid.NewGuid().ToString()
            };
            using CancellationTokenSource cancellationTokenSource = new();

            AccreditationFee? accreditationFee = null;
            (int tonnesOver, int tonnesUpto) = TonnageHelper.GetTonnageBoundaryByTonnageBand(accreditationFeesRequestDto.TonnageBand);

            //Setup
            _accreditationFeesRepositoryMock.Setup(r =>
                r.GetFeeAsync(
                    (int)accreditationFeesRequestDto.RequestorType,
                    (int)accreditationFeesRequestDto.MaterialType,
                    tonnesOver,
                    tonnesUpto,
                    It.IsAny<RegulatorType>(),
                    accreditationFeesRequestDto.SubmissionDate,
                    cancellationTokenSource.Token))
                .ReturnsAsync(accreditationFee);

            // Act
            AccreditationFeesResponseDto? accreditationFeesResponseDto = await _accreditationFeesCalculatorServiceUnderTest!.CalculateFeesAsync(
                accreditationFeesRequestDto,
                cancellationTokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                accreditationFeesResponseDto.Should().BeNull();
           
                // Verify
                _accreditationFeesRepositoryMock.Verify(r =>
                    r.GetFeeAsync(
                        (int)accreditationFeesRequestDto.RequestorType,
                        (int)accreditationFeesRequestDto.MaterialType,
                        tonnesOver,
                        tonnesUpto,
                        It.IsAny<RegulatorType>(),
                        accreditationFeesRequestDto.SubmissionDate,
                        cancellationTokenSource.Token),
                    Times.Once());
                _paymentsRepositoryMock.Verify(r =>
                    r.GetPreviousPaymentIncludeChildrenByReferenceAsync(
                        It.IsAny<string>(),
                        cancellationTokenSource.Token),
                        Times.Never());
            }
        }

        [TestMethod]
        [DataRow(TonnageBand.Upto500)]
        [DataRow(TonnageBand.Over500To5000)]
        [DataRow(TonnageBand.Over5000To10000)]
        [DataRow(TonnageBand.Over10000)]
        public async Task CalculateFeesAsync_ShouldCallRespositoryAndReturnResponseWithoutPreviousPayment_WhenNoApplicationReferenceNumberSupplied(
            TonnageBand tonnageBand)
        {
            // Arrange
            AccreditationFeesRequestDto accreditationFeesRequestDto = new()
            {
                Regulator = RegulatorConstants.GBENG,
                MaterialType = AccreditationFeesMaterialType.Plastic,
                RequestorType = AccreditationFeesRequestorType.Exporters,
                NumberOfOverseasSites = 10,
                TonnageBand = tonnageBand,
                SubmissionDate = DateTime.UtcNow,
            };
            using CancellationTokenSource cancellationTokenSource = new();

            AccreditationFee accreditationFee = new()
            {
                Id = 1,
                RegulatorId = 1,
                GroupId = (int)Common.Enums.Group.Exporters,
                SubGroupId = (int)Common.Enums.SubGroup.Plastic,
                Amount = 100,
                FeesPerSite = 10,
                EffectiveFrom = DateTime.UtcNow.AddDays(-1),
                EffectiveTo = DateTime.UtcNow.AddDays(1),
            };
            (int tonnesOver, int tonnesUpto) = TonnageHelper.GetTonnageBoundaryByTonnageBand(accreditationFeesRequestDto.TonnageBand);

            //Setup
            _accreditationFeesRepositoryMock.Setup(r =>
                r.GetFeeAsync(
                    (int)accreditationFeesRequestDto.RequestorType,
                    (int)accreditationFeesRequestDto.MaterialType,
                    tonnesOver,
                    tonnesUpto,
                    It.IsAny<RegulatorType>(),
                    accreditationFeesRequestDto.SubmissionDate,
                    cancellationTokenSource.Token))
                .ReturnsAsync(accreditationFee);

            // Act
            AccreditationFeesResponseDto? accreditationFeesResponseDto = await _accreditationFeesCalculatorServiceUnderTest!.CalculateFeesAsync(
                accreditationFeesRequestDto,
                cancellationTokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                accreditationFeesResponseDto.Should().NotBeNull();
                accreditationFeesResponseDto!.TonnageBandCharge.Should().Be(accreditationFee.Amount);
                accreditationFeesResponseDto!.OverseasSiteChargePerSite.Should().Be(accreditationFee.FeesPerSite);
                accreditationFeesResponseDto!.TotalOverseasSitesCharges.Should().Be(accreditationFee.FeesPerSite * accreditationFeesRequestDto.NumberOfOverseasSites);
                accreditationFeesResponseDto!.TotalAccreditationFees.Should().Be((accreditationFee.FeesPerSite * accreditationFeesRequestDto.NumberOfOverseasSites) + accreditationFee.Amount);
                
                // Verify
                _accreditationFeesRepositoryMock.Verify(r =>
                    r.GetFeeAsync(
                        (int)accreditationFeesRequestDto.RequestorType,
                        (int)accreditationFeesRequestDto.MaterialType,
                        tonnesOver,
                        tonnesUpto,
                        It.IsAny<RegulatorType>(),
                        accreditationFeesRequestDto.SubmissionDate,
                        cancellationTokenSource.Token),
                    Times.Once());
                _paymentsRepositoryMock.Verify(r =>
                    r.GetPreviousPaymentIncludeChildrenByReferenceAsync(
                        It.IsAny<string>(),
                        cancellationTokenSource.Token),
                        Times.Never());
            }
        }

        [TestMethod]
        public async Task CalculateFeesAsync_ShouldCallRespositoryAndReturnResponseWithoutPreviousPayment_WhenApplicationReferenceNumberIsSuppliedButPaymentRecordNotFound()
        {
            // Arrange
            AccreditationFeesRequestDto accreditationFeesRequestDto = new()
            {
                Regulator = RegulatorConstants.GBENG,
                MaterialType = AccreditationFeesMaterialType.Plastic,
                RequestorType = AccreditationFeesRequestorType.Exporters,
                NumberOfOverseasSites = 10,
                TonnageBand = TonnageBand.Upto500,
                SubmissionDate = DateTime.UtcNow,
                ApplicationReferenceNumber = Guid.NewGuid().ToString()
            };
            using CancellationTokenSource cancellationTokenSource = new();

            AccreditationFee accreditationFee = new()
            {
                Id = 1,
                RegulatorId = 1,
                GroupId = (int)Common.Enums.Group.Exporters,
                SubGroupId = (int)Common.Enums.SubGroup.Plastic,
                Amount = 100,
                FeesPerSite = 10,
                EffectiveFrom = DateTime.UtcNow.AddDays(-1),
                EffectiveTo = DateTime.UtcNow.AddDays(1),
            };
            Common.Data.DataModels.Payment? payment = null;

            (int tonnesOver, int tonnesUpto) = TonnageHelper.GetTonnageBoundaryByTonnageBand(accreditationFeesRequestDto.TonnageBand);

            //Setup
            _accreditationFeesRepositoryMock.Setup(r =>
                r.GetFeeAsync(
                    (int)accreditationFeesRequestDto.RequestorType,
                    (int)accreditationFeesRequestDto.MaterialType,
                    tonnesOver,
                    tonnesUpto,
                    It.IsAny<RegulatorType>(),
                    accreditationFeesRequestDto.SubmissionDate,
                    cancellationTokenSource.Token))
                .ReturnsAsync(accreditationFee);
            _paymentsRepositoryMock.Setup(r =>
                    r.GetPreviousPaymentIncludeChildrenByReferenceAsync(
                        accreditationFeesRequestDto.ApplicationReferenceNumber,
                        cancellationTokenSource.Token))
                .ReturnsAsync(payment);

            // Act
            AccreditationFeesResponseDto? accreditationFeesResponseDto = await _accreditationFeesCalculatorServiceUnderTest!.CalculateFeesAsync(
                accreditationFeesRequestDto,
                cancellationTokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                accreditationFeesResponseDto.Should().NotBeNull();
                accreditationFeesResponseDto!.TonnageBandCharge.Should().Be(accreditationFee.Amount);
                accreditationFeesResponseDto!.OverseasSiteChargePerSite.Should().Be(accreditationFee.FeesPerSite);
                accreditationFeesResponseDto!.TotalOverseasSitesCharges.Should().Be(accreditationFee.FeesPerSite * accreditationFeesRequestDto.NumberOfOverseasSites);
                accreditationFeesResponseDto!.TotalAccreditationFees.Should().Be((accreditationFee.FeesPerSite * accreditationFeesRequestDto.NumberOfOverseasSites) + accreditationFee.Amount);

                // Verify
                _accreditationFeesRepositoryMock.Verify(r =>
                    r.GetFeeAsync(
                        (int)accreditationFeesRequestDto.RequestorType,
                        (int)accreditationFeesRequestDto.MaterialType,
                        tonnesOver,
                        tonnesUpto,
                        It.IsAny<RegulatorType>(),
                        accreditationFeesRequestDto.SubmissionDate,
                        cancellationTokenSource.Token),
                    Times.Once());
                _paymentsRepositoryMock.Verify(r =>
                    r.GetPreviousPaymentIncludeChildrenByReferenceAsync(
                        accreditationFeesRequestDto.ApplicationReferenceNumber,
                        cancellationTokenSource.Token),
                        Times.Once());
            }
        }

        [TestMethod]
        public async Task CalculateFeesAsync_ShouldCallRespositoryAndReturnResponseWithOfflincePreviousPayment()
        {
            // Arrange
            AccreditationFeesRequestDto accreditationFeesRequestDto = new()
            {
                Regulator = RegulatorConstants.GBENG,
                MaterialType = AccreditationFeesMaterialType.Plastic,
                RequestorType = AccreditationFeesRequestorType.Exporters,
                NumberOfOverseasSites = 10,
                TonnageBand = TonnageBand.Upto500,
                SubmissionDate = DateTime.UtcNow,
                ApplicationReferenceNumber = Guid.NewGuid().ToString()
            };
            using CancellationTokenSource cancellationTokenSource = new();

            AccreditationFee accreditationFee = new()
            {
                Id = 1,
                RegulatorId = 1,
                GroupId = (int)Common.Enums.Group.Exporters,
                SubGroupId = (int)Common.Enums.SubGroup.Plastic,
                Amount = 100,
                FeesPerSite = 10,
                EffectiveFrom = DateTime.UtcNow.AddDays(-1),
                EffectiveTo = DateTime.UtcNow.AddDays(1),
            };
            Common.Data.DataModels.Payment? payment = new()
            {
                Id = 1,
                UserId = Guid.NewGuid(),
                ExternalPaymentId = Guid.NewGuid(),
                InternalStatusId = Common.Data.Enums.Status.Success,
                Regulator = RegulatorConstants.GBENG,
                Reference = accreditationFeesRequestDto.ApplicationReferenceNumber,
                Amount = 200,
                ReasonForPayment = "Accreditation Fees",
                CreatedDate = DateTime.UtcNow.AddDays(-1),
                UpdatedByUserId = Guid.NewGuid(),
                UpdatedDate = DateTime.UtcNow.AddDays(-1),
                OfflinePayment = new()
                {
                    Id = 11,
                    PaymentId = 1,
                    PaymentDate = DateTime.UtcNow.AddDays(-1),
                    Comments = "Accreditation Fees Payment",
                    PaymentMethod = "Bank Transfer",
                }
            };

            (int tonnesOver, int tonnesUpto) = TonnageHelper.GetTonnageBoundaryByTonnageBand(accreditationFeesRequestDto.TonnageBand);

            //Setup
            _accreditationFeesRepositoryMock.Setup(r =>
                r.GetFeeAsync(
                    (int)accreditationFeesRequestDto.RequestorType,
                    (int)accreditationFeesRequestDto.MaterialType,
                    tonnesOver,
                    tonnesUpto,
                    It.IsAny<RegulatorType>(),
                    accreditationFeesRequestDto.SubmissionDate,
                    cancellationTokenSource.Token))
                .ReturnsAsync(accreditationFee);
            _paymentsRepositoryMock.Setup(r =>
                    r.GetPreviousPaymentIncludeChildrenByReferenceAsync(
                        accreditationFeesRequestDto.ApplicationReferenceNumber,
                        cancellationTokenSource.Token))
                .ReturnsAsync(payment);

            // Act
            AccreditationFeesResponseDto? accreditationFeesResponseDto = await _accreditationFeesCalculatorServiceUnderTest!.CalculateFeesAsync(
                accreditationFeesRequestDto,
                cancellationTokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                accreditationFeesResponseDto.Should().NotBeNull();
                accreditationFeesResponseDto!.TonnageBandCharge.Should().Be(accreditationFee.Amount);
                accreditationFeesResponseDto!.OverseasSiteChargePerSite.Should().Be(accreditationFee.FeesPerSite);
                accreditationFeesResponseDto!.TotalOverseasSitesCharges.Should().Be(accreditationFee.FeesPerSite * accreditationFeesRequestDto.NumberOfOverseasSites);
                accreditationFeesResponseDto!.TotalAccreditationFees.Should().Be((accreditationFee.FeesPerSite * accreditationFeesRequestDto.NumberOfOverseasSites) + accreditationFee.Amount);
                accreditationFeesResponseDto.PreviousPaymentDetail.Should().NotBeNull();
                accreditationFeesResponseDto.PreviousPaymentDetail!.PaymentMode.Should().Be(PaymentType.Offline.GetDescription());
                accreditationFeesResponseDto.PreviousPaymentDetail!.PaymentAmount.Should().Be(payment.Amount);
                accreditationFeesResponseDto.PreviousPaymentDetail!.PaymentDate.Should().Be(payment.OfflinePayment.PaymentDate);
                accreditationFeesResponseDto.PreviousPaymentDetail!.PaymentMethod.Should().Be(payment.OfflinePayment.PaymentMethod);

                // Verify
                _accreditationFeesRepositoryMock.Verify(r =>
                    r.GetFeeAsync(
                        (int)accreditationFeesRequestDto.RequestorType,
                        (int)accreditationFeesRequestDto.MaterialType,
                        tonnesOver,
                        tonnesUpto,
                        It.IsAny<RegulatorType>(),
                        accreditationFeesRequestDto.SubmissionDate,
                        cancellationTokenSource.Token),
                    Times.Once());
                _paymentsRepositoryMock.Verify(r =>
                    r.GetPreviousPaymentIncludeChildrenByReferenceAsync(
                        accreditationFeesRequestDto.ApplicationReferenceNumber,
                        cancellationTokenSource.Token),
                        Times.Once());
            }
        }

        [TestMethod]
        public async Task CalculateFeesAsync_ShouldCallRespositoryAndReturnResponseWithOnlinePreviousPayment()
        {
            // Arrange
            AccreditationFeesRequestDto accreditationFeesRequestDto = new()
            {
                Regulator = RegulatorConstants.GBENG,
                MaterialType = AccreditationFeesMaterialType.Plastic,
                RequestorType = AccreditationFeesRequestorType.Exporters,
                NumberOfOverseasSites = 10,
                TonnageBand = TonnageBand.Upto500,
                SubmissionDate = DateTime.UtcNow,
                ApplicationReferenceNumber = Guid.NewGuid().ToString()
            };
            using CancellationTokenSource cancellationTokenSource = new();

            AccreditationFee accreditationFee = new()
            {
                Id = 1,
                RegulatorId = 1,
                GroupId = (int)Common.Enums.Group.Exporters,
                SubGroupId = (int)Common.Enums.SubGroup.Plastic,
                Amount = 100,
                FeesPerSite = 10,
                EffectiveFrom = DateTime.UtcNow.AddDays(-1),
                EffectiveTo = DateTime.UtcNow.AddDays(1),
            };
            Common.Data.DataModels.Payment? payment = new()
            {
                Id = 1,
                UserId = Guid.NewGuid(),
                ExternalPaymentId = Guid.NewGuid(),
                InternalStatusId = Common.Data.Enums.Status.Success,
                Regulator = RegulatorConstants.GBENG,
                Reference = accreditationFeesRequestDto.ApplicationReferenceNumber,
                Amount = 200,
                ReasonForPayment = "Accreditation Fees",
                CreatedDate = DateTime.UtcNow.AddDays(-1),
                UpdatedByUserId = Guid.NewGuid(),
                UpdatedDate = DateTime.UtcNow.AddDays(-1),
                OnlinePayment = new()
                {
                    Id = 11,
                    PaymentId = 1,
                }
            };

            (int tonnesOver, int tonnesUpto) = TonnageHelper.GetTonnageBoundaryByTonnageBand(accreditationFeesRequestDto.TonnageBand);

            //Setup
            _accreditationFeesRepositoryMock.Setup(r =>
                r.GetFeeAsync(
                    (int)accreditationFeesRequestDto.RequestorType,
                    (int)accreditationFeesRequestDto.MaterialType,
                    tonnesOver,
                    tonnesUpto,
                    It.IsAny<RegulatorType>(),
                    accreditationFeesRequestDto.SubmissionDate,
                    cancellationTokenSource.Token))
                .ReturnsAsync(accreditationFee);
            _paymentsRepositoryMock.Setup(r =>
                    r.GetPreviousPaymentIncludeChildrenByReferenceAsync(
                        accreditationFeesRequestDto.ApplicationReferenceNumber,
                        cancellationTokenSource.Token))
                .ReturnsAsync(payment);

            // Act
            AccreditationFeesResponseDto? accreditationFeesResponseDto = await _accreditationFeesCalculatorServiceUnderTest!.CalculateFeesAsync(
                accreditationFeesRequestDto,
                cancellationTokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                accreditationFeesResponseDto.Should().NotBeNull();
                accreditationFeesResponseDto!.TonnageBandCharge.Should().Be(accreditationFee.Amount);
                accreditationFeesResponseDto!.OverseasSiteChargePerSite.Should().Be(accreditationFee.FeesPerSite);
                accreditationFeesResponseDto!.TotalOverseasSitesCharges.Should().Be(accreditationFee.FeesPerSite * accreditationFeesRequestDto.NumberOfOverseasSites);
                accreditationFeesResponseDto!.TotalAccreditationFees.Should().Be((accreditationFee.FeesPerSite * accreditationFeesRequestDto.NumberOfOverseasSites) + accreditationFee.Amount);
                accreditationFeesResponseDto.PreviousPaymentDetail.Should().NotBeNull();
                accreditationFeesResponseDto.PreviousPaymentDetail!.PaymentMode.Should().Be(PaymentType.Online.GetDescription());
                accreditationFeesResponseDto.PreviousPaymentDetail!.PaymentAmount.Should().Be(payment.Amount);
                accreditationFeesResponseDto.PreviousPaymentDetail!.PaymentDate.Should().Be(payment.UpdatedDate);
                accreditationFeesResponseDto.PreviousPaymentDetail!.PaymentMethod.Should().BeNullOrEmpty();

                // Verify
                _accreditationFeesRepositoryMock.Verify(r =>
                    r.GetFeeAsync(
                        (int)accreditationFeesRequestDto.RequestorType,
                        (int)accreditationFeesRequestDto.MaterialType,
                        tonnesOver,
                        tonnesUpto,
                        It.IsAny<RegulatorType>(),
                        accreditationFeesRequestDto.SubmissionDate,
                        cancellationTokenSource.Token),
                    Times.Once());
                _paymentsRepositoryMock.Verify(r =>
                    r.GetPreviousPaymentIncludeChildrenByReferenceAsync(
                        accreditationFeesRequestDto.ApplicationReferenceNumber,
                        cancellationTokenSource.Token),
                        Times.Once());
            }
        }
    }
}
