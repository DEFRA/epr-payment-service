using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Fees;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Payments;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ReprocessorOrExporter;
using EPR.Payment.Service.Common.Enums;
using EPR.Payment.Service.Common.Extensions;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
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
        private readonly Mock<IPaymentsRepository> _paymentsRepositoryMock = new();

        private ReprocessorOrExporterFeesCalculatorService? _reprocessorOrExporterFeeCalculatorServiceUnderTest = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _reprocessorOrExporterFeeCalculatorServiceUnderTest = new ReprocessorOrExporterFeesCalculatorService(
                _reprocessorOrExporterFeeRepositoryMock.Object,
                _paymentsRepositoryMock.Object);
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

                _paymentsRepositoryMock.Verify(r =>
                    r.GetPreviousPaymentIncludeChildrenByReferenceAsync(
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
                response!.MaterialType.Should().Be((MaterialTypes)registrationFeeEntity.SubGroupId);
                
                // Verify
                _reprocessorOrExporterFeeRepositoryMock.Verify(r =>
                    r.GetFeeAsync(
                        (int)request.RequestorType,
                        (int)request.MaterialType,
                        It.IsAny<RegulatorType>(),
                        request.SubmissionDate,
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

            Common.Data.DataModels.Payment? paymentEntity = null;

            //Setup
            _reprocessorOrExporterFeeRepositoryMock.Setup(r =>
                r.GetFeeAsync(
                    (int)request.RequestorType,
                    (int)request.MaterialType,
                    It.IsAny<RegulatorType>(),
                    request.SubmissionDate,
                    cancellationTokenSource.Token))
                .ReturnsAsync(registrationFeeEntity);

            _paymentsRepositoryMock.Setup(r =>
                    r.GetPreviousPaymentIncludeChildrenByReferenceAsync(
                        request.ApplicationReferenceNumber,
                        cancellationTokenSource.Token))
                .ReturnsAsync(paymentEntity);

            // Act
            var response = await _reprocessorOrExporterFeeCalculatorServiceUnderTest!.CalculateFeesAsync(
                request,
                cancellationTokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                response.Should().NotBeNull();
                response!.RegistrationFee.Should().Be(registrationFeeEntity.Amount);
                response!.MaterialType.Should().Be((MaterialTypes)registrationFeeEntity.SubGroupId);
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

                _paymentsRepositoryMock.Verify(r =>
                    r.GetPreviousPaymentIncludeChildrenByReferenceAsync(
                        request.ApplicationReferenceNumber,
                        cancellationTokenSource.Token),
                        Times.Once());
            }
        }

        [TestMethod]
        public async Task CalculateFeesAsync_ShouldCallRespositoryAndReturnResponseWithOfflincePreviousPayment()
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

            var paymentEntity = new Common.Data.DataModels.Payment
            {
                Id = 1,
                UserId = Guid.NewGuid(),
                ExternalPaymentId = Guid.NewGuid(),
                InternalStatusId = Common.Data.Enums.Status.Success,
                Regulator = RegulatorConstants.GBENG,
                Reference = request.ApplicationReferenceNumber,
                Amount = 200,
                ReasonForPayment = "Registration Fees",
                CreatedDate = DateTime.UtcNow.AddDays(-1),
                UpdatedByUserId = Guid.NewGuid(),
                UpdatedDate = DateTime.UtcNow.AddDays(-1),
                OfflinePayment = new()
                {
                    Id = 11,
                    PaymentId = 1,
                    PaymentDate = DateTime.UtcNow.AddDays(-1),
                    Comments = "Registration Fees Payment",
                    PaymentMethod = "Bank Transfer",
                }
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

            _paymentsRepositoryMock.Setup(r =>
                    r.GetPreviousPaymentIncludeChildrenByReferenceAsync(
                        request.ApplicationReferenceNumber,
                        cancellationTokenSource.Token))
                .ReturnsAsync(paymentEntity);

            // Act
            var response = await _reprocessorOrExporterFeeCalculatorServiceUnderTest!.CalculateFeesAsync(
                request,
                cancellationTokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                response.Should().NotBeNull();
                response!.RegistrationFee.Should().Be(registrationFeeEntity.Amount);
                response!.MaterialType.Should().Be((MaterialTypes)registrationFeeEntity.SubGroupId);

                response!.PreviousPaymentDetail.Should().NotBeNull();
                response.PreviousPaymentDetail!.PaymentMode.Should().Be(PaymentTypes.Offline.GetDescription());
                response.PreviousPaymentDetail!.PaymentAmount.Should().Be(paymentEntity.Amount);
                response.PreviousPaymentDetail!.PaymentDate.Should().Be(paymentEntity.OfflinePayment.PaymentDate);
                response.PreviousPaymentDetail!.PaymentMethod.Should().Be(paymentEntity.OfflinePayment.PaymentMethod);

                // Verify
                _reprocessorOrExporterFeeRepositoryMock.Verify(r =>
                    r.GetFeeAsync(
                        (int)request.RequestorType,
                        (int)request.MaterialType,
                        It.IsAny<RegulatorType>(),
                        request.SubmissionDate,
                        cancellationTokenSource.Token),
                    Times.Once());

                _paymentsRepositoryMock.Verify(r =>
                    r.GetPreviousPaymentIncludeChildrenByReferenceAsync(
                        request.ApplicationReferenceNumber,
                        cancellationTokenSource.Token),
                        Times.Once());
            }
        }

        [TestMethod]
        public async Task CalculateFeesAsync_ShouldCallRespositoryAndReturnResponseWithOnlinePreviousPayment()
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

            var paymentEntity = new Common.Data.DataModels.Payment
            {
                Id = 1,
                UserId = Guid.NewGuid(),
                ExternalPaymentId = Guid.NewGuid(),
                InternalStatusId = Common.Data.Enums.Status.Success,
                Regulator = RegulatorConstants.GBENG,
                Reference = request.ApplicationReferenceNumber,
                Amount = 200,
                ReasonForPayment = "Registration Fees",
                CreatedDate = DateTime.UtcNow.AddDays(-1),
                UpdatedByUserId = Guid.NewGuid(),
                UpdatedDate = DateTime.UtcNow.AddDays(-1),
                OnlinePayment = new()
                {
                    Id = 11,
                    PaymentId = 1,
                }
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

            _paymentsRepositoryMock.Setup(r =>
                    r.GetPreviousPaymentIncludeChildrenByReferenceAsync(
                        request.ApplicationReferenceNumber,
                        cancellationTokenSource.Token))
                .ReturnsAsync(paymentEntity);

            // Act
            var response = await _reprocessorOrExporterFeeCalculatorServiceUnderTest!.CalculateFeesAsync(
                request,
                cancellationTokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                response.Should().NotBeNull();
                response!.RegistrationFee.Should().Be(registrationFeeEntity.Amount);
                response!.MaterialType.Should().Be((MaterialTypes)registrationFeeEntity.SubGroupId);

                response.PreviousPaymentDetail.Should().NotBeNull();
                response.PreviousPaymentDetail!.PaymentMode.Should().Be(PaymentTypes.Online.GetDescription());
                response.PreviousPaymentDetail!.PaymentAmount.Should().Be(paymentEntity.Amount);
                response.PreviousPaymentDetail!.PaymentDate.Should().Be(paymentEntity.UpdatedDate);
                response.PreviousPaymentDetail!.PaymentMethod.Should().BeNullOrEmpty();

                // Verify
                _reprocessorOrExporterFeeRepositoryMock.Verify(r =>
                    r.GetFeeAsync(
                        (int)request.RequestorType,
                        (int)request.MaterialType,
                        It.IsAny<RegulatorType>(),
                        request.SubmissionDate,
                        cancellationTokenSource.Token),
                    Times.Once());

                _paymentsRepositoryMock.Verify(r =>
                    r.GetPreviousPaymentIncludeChildrenByReferenceAsync(
                        request.ApplicationReferenceNumber,
                        cancellationTokenSource.Token),
                        Times.Once());
            }
        }
    }
}
