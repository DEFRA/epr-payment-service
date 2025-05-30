using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Fees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ReprocessorOrExporter;
using EPR.Payment.Service.Common.Dtos.Response.Payments;
using EPR.Payment.Service.Common.Enums;
using EPR.Payment.Service.Common.Extensions;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Services.Interfaces.Payments;
using EPR.Payment.Service.Services.RegistrationFees.ReprocessorOrExporter;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;

namespace EPR.Payment.Service.UnitTests.Services.RegistrationFees.ReprocessorOrExporter
{
    [TestClass]
    public class ReprocessorOrExporterFeesCalculatorServiceTests
    {
        private readonly Mock<IReprocessorOrExporterFeeRepository> _reprocessorOrExporterFeeRepositoryMock = new();
        private readonly Mock<IPreviousPaymentsHelper> _previousPaymentsHelperMock = new();

        private ReprocessorOrExporterFeesCalculatorService? _reprocessorOrExporterFeeCalculatorServiceUnderTest = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _reprocessorOrExporterFeeCalculatorServiceUnderTest = new ReprocessorOrExporterFeesCalculatorService(
                _reprocessorOrExporterFeeRepositoryMock.Object,
                _previousPaymentsHelperMock.Object);
        }

        [TestMethod]
        public async Task CalculateFeesAsync_ShouldCallRespositoryAndReturnNullResponse_WhenAccreditationFeeRecordNotFound()
        {
            // Arrange
            var request = new ReprocessorOrExporterRegistrationFeesRequestDto
            {
                Regulator = RegulatorConstants.GBENG,
                MaterialType = MaterialTypes.Plastic,
                RequestorType = RequestorTypes.Exporters,
                SubmissionDate = DateTime.UtcNow,
                ApplicationReferenceNumber = Guid.NewGuid().ToString()
            };
            using CancellationTokenSource cancellationTokenSource = new();

            Common.Data.DataModels.Lookups.RegistrationFees? registrationFeeEntity = null;

            //Setup
            _reprocessorOrExporterFeeRepositoryMock.Setup(r =>
                r.GetFeeAsync(
                    (int)request.RequestorType,
                    (int)request.MaterialType,
                    It.IsAny<RegulatorType>(),
                    request.SubmissionDate,
                    cancellationTokenSource.Token))
                .ReturnsAsync(registrationFeeEntity);

            // Act
            var response = await _reprocessorOrExporterFeeCalculatorServiceUnderTest!.CalculateFeesAsync(
                request,
                cancellationTokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                response.Should().BeNull();
           
                // Verify
                _reprocessorOrExporterFeeRepositoryMock.Verify(r =>
                r.GetFeeAsync(
                    (int)request.RequestorType,
                    (int)request.MaterialType,
                    It.IsAny<RegulatorType>(),
                    request.SubmissionDate,
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
        public async Task CalculateFeesAsync_ShouldCallRespositoryAndReturnResponseWithoutPreviousPayment_WhenNoApplicationReferenceNumberSupplied()
        {
            // Arrange
            var request = new ReprocessorOrExporterRegistrationFeesRequestDto
            {
                Regulator = RegulatorConstants.GBENG,
                MaterialType = MaterialTypes.Plastic,
                RequestorType = RequestorTypes.Exporters,
                SubmissionDate = DateTime.UtcNow,
            };
            using CancellationTokenSource cancellationTokenSource = new();

            var registrationFeeEntity = new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Id = 1,
                RegulatorId = 1,
                GroupId = (int)Group.Exporters,
                SubGroupId = (int)SubGroup.Plastic,
                Amount = 100,
                EffectiveFrom = DateTime.UtcNow.AddDays(-1),
                EffectiveTo = DateTime.UtcNow.AddDays(1),
            };

            //Setup
            _reprocessorOrExporterFeeRepositoryMock.Setup(r =>
                r.GetFeeAsync(
                    (int)request.RequestorType,
                    (int)request.MaterialType,
                    It.IsAny<RegulatorType>(),
                    request.SubmissionDate,
                    cancellationTokenSource.Token))
                .ReturnsAsync(registrationFeeEntity);

            // Act
            var response = await _reprocessorOrExporterFeeCalculatorServiceUnderTest!.CalculateFeesAsync(
                request,
                cancellationTokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                response.Should().NotBeNull();
                response!.RegistrationFee.Should().Be(registrationFeeEntity.Amount);
                
                // Verify
                _reprocessorOrExporterFeeRepositoryMock.Verify(r =>
                    r.GetFeeAsync(
                        (int)request.RequestorType,
                        (int)request.MaterialType,
                        It.IsAny<RegulatorType>(),
                        request.SubmissionDate,
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
            var request = new ReprocessorOrExporterRegistrationFeesRequestDto
            {
                Regulator = RegulatorConstants.GBENG,
                MaterialType = MaterialTypes.Plastic,
                RequestorType = RequestorTypes.Exporters,
                SubmissionDate = DateTime.UtcNow,
                ApplicationReferenceNumber = Guid.NewGuid().ToString()
            };
            using CancellationTokenSource cancellationTokenSource = new();

            var registrationFeeEntity = new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Id = 1,
                RegulatorId = 1,
                GroupId = (int)Group.Exporters,
                SubGroupId = (int)SubGroup.Plastic,
                Amount = 100,
                EffectiveFrom = DateTime.UtcNow.AddDays(-1),
                EffectiveTo = DateTime.UtcNow.AddDays(1),
            };

            PreviousPaymentDetailResponseDto? previousPayment = null;

            //Setup
            _reprocessorOrExporterFeeRepositoryMock.Setup(r =>
                r.GetFeeAsync(
                    (int)request.RequestorType,
                    (int)request.MaterialType,
                    It.IsAny<RegulatorType>(),
                    request.SubmissionDate,
                    cancellationTokenSource.Token))
                .ReturnsAsync(registrationFeeEntity);

            _previousPaymentsHelperMock.Setup(r =>
                r.GetPreviousPaymentAsync(
                        request.ApplicationReferenceNumber,
                        cancellationTokenSource.Token))
                .ReturnsAsync(previousPayment);

            // Act
            var response = await _reprocessorOrExporterFeeCalculatorServiceUnderTest!.CalculateFeesAsync(
                request,
                cancellationTokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                response.Should().NotBeNull();
                response!.RegistrationFee.Should().Be(registrationFeeEntity.Amount);
                response!.PreviousPaymentDetail.Should().BeNull();

                // Verify
                _reprocessorOrExporterFeeRepositoryMock.Verify(r =>
                    r.GetFeeAsync(
                        (int)request.RequestorType,
                        (int)request.MaterialType,
                        It.IsAny<RegulatorType>(),
                        request.SubmissionDate,
                        cancellationTokenSource.Token),
                    Times.Once());

                _previousPaymentsHelperMock.Verify(r =>
                    r.GetPreviousPaymentAsync(
                        request.ApplicationReferenceNumber,
                        cancellationTokenSource.Token),
                        Times.Once());
            }
        }

        [TestMethod]
        public async Task CalculateFeesAsync_ShouldCallHelperAndReturnResponse()
        {
            // Arrange
            var request = new ReprocessorOrExporterRegistrationFeesRequestDto
            {
                Regulator = RegulatorConstants.GBENG,
                MaterialType = MaterialTypes.Plastic,
                RequestorType = RequestorTypes.Exporters,
                SubmissionDate = DateTime.UtcNow,
                ApplicationReferenceNumber = Guid.NewGuid().ToString()
            };
            using CancellationTokenSource cancellationTokenSource = new();

            var registrationFeeEntity = new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Id = 1,
                RegulatorId = 1,
                GroupId = (int)Group.Exporters,
                SubGroupId = (int)SubGroup.Plastic,
                Amount = 100,
                EffectiveFrom = DateTime.UtcNow.AddDays(-1),
                EffectiveTo = DateTime.UtcNow.AddDays(1),
            };

            PreviousPaymentDetailResponseDto? previousPayment = new()
            {
                PaymentMode = PaymentTypes.Offline.GetDescription(),
                PaymentAmount = 200,
                PaymentDate = DateTime.UtcNow.AddDays(-1),
                PaymentMethod = "Cheque"
            };

            //Setup
            _reprocessorOrExporterFeeRepositoryMock.Setup(r =>
                r.GetFeeAsync(
                    (int)request.RequestorType,
                    (int)request.MaterialType,
                    It.IsAny<RegulatorType>(),
                    request.SubmissionDate,
                    cancellationTokenSource.Token))
                .ReturnsAsync(registrationFeeEntity);

            _previousPaymentsHelperMock.Setup(r =>
                r.GetPreviousPaymentAsync(
                        request.ApplicationReferenceNumber,
                        cancellationTokenSource.Token))
                .ReturnsAsync(previousPayment);

            // Act
            var response = await _reprocessorOrExporterFeeCalculatorServiceUnderTest!.CalculateFeesAsync(
                request,
                cancellationTokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                response.Should().NotBeNull();
                response!.RegistrationFee.Should().Be(registrationFeeEntity.Amount);
                response!.PreviousPaymentDetail.Should().Be(previousPayment);

                // Verify
                _reprocessorOrExporterFeeRepositoryMock.Verify(r =>
                    r.GetFeeAsync(
                        (int)request.RequestorType,
                        (int)request.MaterialType,
                        It.IsAny<RegulatorType>(),
                        request.SubmissionDate,
                        cancellationTokenSource.Token),
                    Times.Once());

                _previousPaymentsHelperMock.Verify(r =>
                    r.GetPreviousPaymentAsync(
                        request.ApplicationReferenceNumber,
                        cancellationTokenSource.Token),
                        Times.Once());
            }
        }
    }
}
