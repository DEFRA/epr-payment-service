using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Payments;
using EPR.Payment.Service.Common.Dtos.Response.AccreditationFees;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ReprocessorOrExporter;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Services.Payments;
using FluentAssertions;
using Moq;

namespace EPR.Payment.Service.UnitTests.Services.Payments
{
    [TestClass]
    public class PreviousPaymentsHelperTests
    {
        [TestMethod]
        [AutoMoqData]
        public async Task GetPreviousPaymentAsync_WithNoExistingPayments_ShouldReturnNull(
            [Frozen] Mock<IPaymentsRepository> paymentsRepositoryMock,
            [Greedy] PreviousPaymentsHelper subjectUnderTest)
        {
            // Arrange
            var applicationReferenceNumber = "A12345";

            Common.Data.DataModels.Payment? paymentEntity = null;

            paymentsRepositoryMock.Setup(
                m => m.GetPreviousPaymentIncludeChildrenByReferenceAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(paymentEntity);

            // Act 
            var result = await subjectUnderTest.GetPreviousPaymentAsync<AccreditationFeesPreviousPayment>(applicationReferenceNumber, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetPreviousPaymentAsync_WithExistingOfflinePayment_ShouldReturnOfflinePayment(
            [Frozen] Mock<IPaymentsRepository> paymentsRepositoryMock,
            [Greedy] PreviousPaymentsHelper subjectUnderTest)
        {
            // Arrange

            var applicationReferenceNumber = "A12345";

            Common.Data.DataModels.Payment? paymentEntity = new()
            {
                Id = 1,
                UserId = Guid.NewGuid(),
                ExternalPaymentId = Guid.NewGuid(),
                InternalStatusId = Common.Data.Enums.Status.Success,
                Regulator = RegulatorConstants.GBENG,
                Reference = applicationReferenceNumber,
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

            paymentsRepositoryMock.Setup(
                m => m.GetPreviousPaymentIncludeChildrenByReferenceAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(paymentEntity);

            // Act 
            var result = await subjectUnderTest.GetPreviousPaymentAsync<AccreditationFeesPreviousPayment>(applicationReferenceNumber, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.PaymentMethod.Should().Be(paymentEntity.OfflinePayment.PaymentMethod);
            result!.PaymentDate.Should().Be(paymentEntity.OfflinePayment.PaymentDate);
            result!.PaymentAmount.Should().Be(paymentEntity.Amount);
            result!.PaymentMode.Should().Be("Offline");

        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetPreviousPaymentAsync_WithExistingOnlinePayment_ShouldReturnOnlinePayment(
            [Frozen] Mock<IPaymentsRepository> paymentsRepositoryMock,
            [Greedy] PreviousPaymentsHelper subjectUnderTest)
        {
            // Arrange

            var applicationReferenceNumber = "A12345";

            Common.Data.DataModels.Payment? paymentEntity = new()
            {
                Id = 1,
                UserId = Guid.NewGuid(),
                ExternalPaymentId = Guid.NewGuid(),
                InternalStatusId = Common.Data.Enums.Status.Success,
                Regulator = RegulatorConstants.GBENG,
                Reference = applicationReferenceNumber,
                Amount = 200,
                ReasonForPayment = "Registration Fees",
                CreatedDate = DateTime.UtcNow.AddDays(-1),
                UpdatedByUserId = Guid.NewGuid(),
                UpdatedDate = DateTime.UtcNow.AddDays(-1),
                OnlinePayment = new()
                {
                    Id = 12,
                    PaymentId = 1,
                }
            };

            paymentsRepositoryMock.Setup(
                m => m.GetPreviousPaymentIncludeChildrenByReferenceAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(paymentEntity);

            // Act 
            var result = await subjectUnderTest.GetPreviousPaymentAsync<PreviousPaymentDetailDto>(applicationReferenceNumber, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.PaymentMethod.Should().BeNull();
            result!.PaymentDate.Should().Be(paymentEntity.UpdatedDate);
            result!.PaymentAmount.Should().Be(paymentEntity.Amount);
            result!.PaymentMode.Should().Be("Online");
        }
    }
}
