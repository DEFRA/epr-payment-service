using AutoMapper;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories;
using EPR.Payment.Service.Common.Data.Profiles;
using EPR.Payment.Service.Common.Dtos.Request;
using EPR.Payment.Service.Services;
using EPR.Payment.Service.Services.Interfaces;
using FluentAssertions;
using Moq;

namespace EPR.Payment.Service.UnitTests.Services
{
    [TestClass]
    public class PaymentsServiceTests
    {
        private readonly Mock<IPaymentsRepository> _paymentsRepositoryMock;
        private readonly IMapper _mapper;
        private IPaymentsService _service;

        public PaymentsServiceTests() 
        {
            _paymentsRepositoryMock = new Mock<IPaymentsRepository>();
            var configuration = SetupAutomapper();
            _mapper = new Mapper(configuration);
            _service = new PaymentsService(_mapper, _paymentsRepositoryMock.Object);
        }

        private MapperConfiguration SetupAutomapper()
        {
            var myProfile = new PaymentProfile();
            return new MapperConfiguration(c => c.AddProfile(myProfile));
        }

        [TestMethod]
        public async Task InsertPaymentStatus_ValidRequest_ReturnsNewGuid()
        {
            // Arrange
            var request = new PaymentStatusInsertRequestDto
            {
                UserId = "88fb2f51-2f73-4b93-9894-8a39054cf6d2",
                OrganisationId = "88fb2f51-2f73-4b93-9894-8a39054cf6d2",
                ReferenceNumber = "123",
                Regulator = "Regulator",
                Amount = 20,
                ReasonForPayment = "Reason For Payment",
                Status = Common.Dtos.Enums.Status.Initiated
            };

            var expectedResult = new Guid();
            var entity = _mapper.Map<Common.Data.DataModels.Payment>(request);
            entity.ExternalPaymentId = expectedResult;

            _paymentsRepositoryMock.Setup(r =>
               r.InsertPaymentStatusAsync(entity)).ReturnsAsync(expectedResult); 


            // Act
            var result = await _service.InsertPaymentStatusAsync(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public async Task InsertPaymentStatus_NullRequest_ThrowsArgumentNullException()
        {
            // Act & Assert
            await _service.Invoking(async x => await x.InsertPaymentStatusAsync(null))
                .Should().ThrowAsync<ArgumentException>();
        }

        [TestMethod]
        public async Task UpdatePaymentStatus_WithCorrectParameters()
        {
            // Arrange
            var externalPaymentId = new Guid(); 
            var request = new PaymentStatusUpdateRequestDto
            {
                ExternalPaymentId = externalPaymentId,
                GovPayPaymentId = "123",
                UpdatedByUserId = "88fb2f51-2f73-4b93-9894-8a39054cf6d2",
                UpdatedByOrganisationId = "88fb2f51-2f73-4b93-9894-8a39054cf6d2",
                ReferenceNumber = "12345",
                Status = Common.Dtos.Enums.Status.InProgress
            };

            var entity = new Common.Data.DataModels.Payment();
            _paymentsRepositoryMock.Setup(r => r.GetPaymentByExternalPaymentIdAsync(externalPaymentId)).ReturnsAsync(entity);

            entity = _mapper.Map(request, entity);

            // Act
            _paymentsRepositoryMock.Setup(r =>
               r.UpdatePaymentStatusAsync(entity));

            Func<Task> action = async () => await _service.UpdatePaymentStatusAsync(externalPaymentId, request);

            // Assert
            await action.Should().NotThrowAsync();
        }

        [TestMethod]
        public async Task UpdatePaymentStatus_WhenInitialisedWithNullStatus_ThrowsArgumentNullException()
        {
            var externalPaymentId = new Guid();

            // Act & Assert
            await _service.Invoking(async x => await x.UpdatePaymentStatusAsync(externalPaymentId, null))
                .Should().ThrowAsync<ArgumentException>();
        }
    }
}
