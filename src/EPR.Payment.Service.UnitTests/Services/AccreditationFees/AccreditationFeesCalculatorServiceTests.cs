using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Fees;
using EPR.Payment.Service.Common.Dtos.Request.AccreditationFees;
using EPR.Payment.Service.Common.Dtos.Response.AccreditationFees;
using EPR.Payment.Service.Common.Dtos.Response.Payments;
using EPR.Payment.Service.Common.Enums;
using EPR.Payment.Service.Common.Extensions;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Services.AccreditationFees;
using EPR.Payment.Service.Services.Interfaces.Payments;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;

namespace EPR.Payment.Service.UnitTests.Services.AccreditationFees
{
    [TestClass]
    public class AccreditationFeesCalculatorServiceTests
    {
        private readonly Mock<IAccreditationFeesRepository> _accreditationFeesRepositoryMock = new();
        private readonly Mock<IPreviousPaymentsHelper> _previousPaymentsHelperMock = new();

        private AccreditationFeesCalculatorService? _accreditationFeesCalculatorServiceUnderTest = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _accreditationFeesCalculatorServiceUnderTest = new AccreditationFeesCalculatorService(
                _accreditationFeesRepositoryMock.Object,
                _previousPaymentsHelperMock.Object);
        }

        [TestMethod]
        public async Task CalculateFeesAsync_ShouldCallRespositoryAndReturnNullResponse_WhenAccreditationFeeRecordNotFound()
        {
            // Arrange
            ReprocessorOrExporterAccreditationFeesRequestDto accreditationFeesRequestDto = new()
            {
                Regulator = RegulatorConstants.GBENG,
                MaterialType = MaterialTypes.Plastic,
                RequestorType = RequestorTypes.Exporters,
                NumberOfOverseasSites = 10,
                TonnageBand = TonnageBands.Upto500,
                SubmissionDate = DateTime.UtcNow,
                ApplicationReferenceNumber = Guid.NewGuid().ToString()
            };
            using CancellationTokenSource cancellationTokenSource = new();

            AccreditationFee? accreditationFee = null;
         
            //Setup
            _accreditationFeesRepositoryMock.Setup(r =>
                r.GetFeeAsync(
                    (int)accreditationFeesRequestDto.RequestorType,
                    (int)accreditationFeesRequestDto.MaterialType,
                    (int)accreditationFeesRequestDto.TonnageBand,
                    It.IsAny<RegulatorType>(),
                    accreditationFeesRequestDto.SubmissionDate,
                    cancellationTokenSource.Token))
                .ReturnsAsync(accreditationFee);

            // Act
            ReprocessorOrExporterAccreditationFeesResponseDto? accreditationFeesResponseDto = await _accreditationFeesCalculatorServiceUnderTest!.CalculateFeesAsync(
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
                        (int)accreditationFeesRequestDto.TonnageBand,
                        It.IsAny<RegulatorType>(),
                        accreditationFeesRequestDto.SubmissionDate,
                        cancellationTokenSource.Token),
                    Times.Once());

                _previousPaymentsHelperMock.Verify(r =>
                    r.GetPreviousPaymentAsync(
                        It.IsAny<string>(),
                        cancellationTokenSource.Token),
                        Times.Never());
            }
        }

        [TestMethod]
        [DataRow(TonnageBands.Upto500)]
        [DataRow(TonnageBands.Over500To5000)]
        [DataRow(TonnageBands.Over5000To10000)]
        [DataRow(TonnageBands.Over10000)]
        public async Task CalculateFeesAsync_ShouldCallRespositoryAndReturnResponseWithoutPreviousPayment_WhenNoApplicationReferenceNumberSupplied(
            TonnageBands tonnageBand)
        {
            // Arrange
            ReprocessorOrExporterAccreditationFeesRequestDto accreditationFeesRequestDto = new()
            {
                Regulator = RegulatorConstants.GBENG,
                MaterialType = MaterialTypes.Plastic,
                RequestorType = RequestorTypes.Exporters,
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
           
            //Setup
            _accreditationFeesRepositoryMock.Setup(r =>
                r.GetFeeAsync(
                    (int)accreditationFeesRequestDto.RequestorType,
                    (int)accreditationFeesRequestDto.MaterialType,
                    (int)accreditationFeesRequestDto.TonnageBand,
                    It.IsAny<RegulatorType>(),
                    accreditationFeesRequestDto.SubmissionDate,
                    cancellationTokenSource.Token))
                .ReturnsAsync(accreditationFee);

            // Act
            ReprocessorOrExporterAccreditationFeesResponseDto? accreditationFeesResponseDto = await _accreditationFeesCalculatorServiceUnderTest!.CalculateFeesAsync(
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
                        (int)accreditationFeesRequestDto.TonnageBand,
                        It.IsAny<RegulatorType>(),
                        accreditationFeesRequestDto.SubmissionDate,
                        cancellationTokenSource.Token),
                    Times.Once());

                _previousPaymentsHelperMock.Verify(r =>
                    r.GetPreviousPaymentAsync(
                        It.IsAny<string>(),
                        cancellationTokenSource.Token),
                        Times.Never());
            }
        }

        [TestMethod]
        public async Task CalculateFeesAsync_ShouldCallRespositoryAndReturnResponseWithoutPreviousPayment_WhenApplicationReferenceNumberIsSuppliedButPaymentRecordNotFound()
        {
            // Arrange
            ReprocessorOrExporterAccreditationFeesRequestDto accreditationFeesRequestDto = new()
            {
                Regulator = RegulatorConstants.GBENG,
                MaterialType = MaterialTypes.Plastic,
                RequestorType = RequestorTypes.Exporters,
                NumberOfOverseasSites = 10,
                TonnageBand = TonnageBands.Upto500,
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

            PreviousPaymentDetailResponseDto? previousPayment = null;
         
            //Setup
            _accreditationFeesRepositoryMock.Setup(r =>
                r.GetFeeAsync(
                    (int)accreditationFeesRequestDto.RequestorType,
                    (int)accreditationFeesRequestDto.MaterialType,
                    (int)accreditationFeesRequestDto.TonnageBand,
                    It.IsAny<RegulatorType>(),
                    accreditationFeesRequestDto.SubmissionDate,
                    cancellationTokenSource.Token))
                .ReturnsAsync(accreditationFee);

            _previousPaymentsHelperMock.Setup(r =>
                    r.GetPreviousPaymentAsync(
                        accreditationFeesRequestDto.ApplicationReferenceNumber,
                        cancellationTokenSource.Token))
                .ReturnsAsync(previousPayment);

            // Act
            ReprocessorOrExporterAccreditationFeesResponseDto? accreditationFeesResponseDto = await _accreditationFeesCalculatorServiceUnderTest!.CalculateFeesAsync(
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
                        (int)accreditationFeesRequestDto.TonnageBand,
                        It.IsAny<RegulatorType>(),
                        accreditationFeesRequestDto.SubmissionDate,
                        cancellationTokenSource.Token),
                    Times.Once());

                _previousPaymentsHelperMock.Verify(r =>
                    r.GetPreviousPaymentAsync(
                        accreditationFeesRequestDto.ApplicationReferenceNumber,
                        cancellationTokenSource.Token),
                        Times.Once());
            }
        }

        [TestMethod]
        public async Task CalculateFeesAsync_ShouldCallHelperAndReturnResponse()
        {
            // Arrange
            ReprocessorOrExporterAccreditationFeesRequestDto accreditationFeesRequestDto = new()
            {
                Regulator = RegulatorConstants.GBENG,
                MaterialType = MaterialTypes.Plastic,
                RequestorType = RequestorTypes.Exporters,
                NumberOfOverseasSites = 10,
                TonnageBand = TonnageBands.Upto500,
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

            PreviousPaymentDetailResponseDto? previousPayment = new()
            {
                PaymentMode = PaymentTypes.Offline.GetDescription(),
                PaymentAmount = 200,
                PaymentDate = DateTime.UtcNow.AddDays(-1),
                PaymentMethod = "Bank Transfer"
            };
           
            //Setup
            _accreditationFeesRepositoryMock.Setup(r =>
                r.GetFeeAsync(
                    (int)accreditationFeesRequestDto.RequestorType,
                    (int)accreditationFeesRequestDto.MaterialType,
                    (int)accreditationFeesRequestDto.TonnageBand,
                    It.IsAny<RegulatorType>(),
                    accreditationFeesRequestDto.SubmissionDate,
                    cancellationTokenSource.Token))
                .ReturnsAsync(accreditationFee);

            _previousPaymentsHelperMock.Setup(r =>
                    r.GetPreviousPaymentAsync(
                        accreditationFeesRequestDto.ApplicationReferenceNumber,
                        cancellationTokenSource.Token))
                .ReturnsAsync(previousPayment);

            // Act
            ReprocessorOrExporterAccreditationFeesResponseDto? accreditationFeesResponseDto = await _accreditationFeesCalculatorServiceUnderTest!.CalculateFeesAsync(
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
                accreditationFeesResponseDto.PreviousPaymentDetail.Should().Be(previousPayment);

                // Verify
                _accreditationFeesRepositoryMock.Verify(r =>
                    r.GetFeeAsync(
                        (int)accreditationFeesRequestDto.RequestorType,
                        (int)accreditationFeesRequestDto.MaterialType,
                        (int)accreditationFeesRequestDto.TonnageBand,
                        It.IsAny<RegulatorType>(),
                        accreditationFeesRequestDto.SubmissionDate,
                        cancellationTokenSource.Token),
                    Times.Once());

                _previousPaymentsHelperMock.Verify(r =>
                    r.GetPreviousPaymentAsync(
                        accreditationFeesRequestDto.ApplicationReferenceNumber,
                        cancellationTokenSource.Token),
                        Times.Once());
            }
        }
    }
}
