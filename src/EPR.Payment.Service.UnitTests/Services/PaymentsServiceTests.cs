using AutoFixture;
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
        private readonly IFixture _fixture;
        private readonly Mock<IPaymentsRepository> _paymentsRepositoryMock;
        private readonly IMapper _mapper;
        private IPaymentsService _service;

        public PaymentsServiceTests() 
        {
            _fixture = new Fixture();
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
        public async Task InsertPaymentStatus_ReturnsWithGuid_WithValidRequest()
        {
            // Arrange
            var request = _fixture.Build<PaymentStatusInsertRequestDto>().With(d => d.UserId, new Guid()).With(x => x.OrganisationId, new Guid()).Create();

            var expectedResult = new Guid();
            var entity = _mapper.Map<Common.Data.DataModels.Payment>(request);

            _paymentsRepositoryMock.Setup(r =>
               r.InsertPaymentStatusAsync(entity)).ReturnsAsync(expectedResult); 


            // Act
            var result = await _service.InsertPaymentStatusAsync(request);

            // Assert
            result.Should().Be(expectedResult);
        }

        [TestMethod]
        public async Task InsertPaymentStatus_ReturnsArgumentException_WithNullUserId()
        {
            // Act & Assert
            var request = _fixture.Build<PaymentStatusInsertRequestDto>().With(d => d.UserId, (Guid?)null).Create();

            await _service.Invoking(async x => await x.InsertPaymentStatusAsync(request))
                .Should().ThrowAsync<ArgumentException>();
        }

        [TestMethod]
        public async Task InsertPaymentStatus_ReturnsArgumentException_WithNullOrganisationId()
        {
            // Act & Assert
            var request = _fixture.Build<PaymentStatusInsertRequestDto>().With(d => d.OrganisationId, (Guid?)null).Create();

            await _service.Invoking(async x => await x.InsertPaymentStatusAsync(request))
                .Should().ThrowAsync<ArgumentException>();
        }

        [TestMethod]
        public async Task UpdatePaymentStatus_WithValidRequest()
        {
            // Arrange
            var Id = new Guid();
            var request = _fixture.Build<PaymentStatusUpdateRequestDto>().With(d => d.UpdatedByUserId, new Guid()).With(x => x.UpdatedByOrganisationId, new Guid()).With(k => k.ErrorCode, "").Create();

            var entity = new Common.Data.DataModels.Payment();
            _paymentsRepositoryMock.Setup(r => r.GetPaymentByIdAsync(Id)).ReturnsAsync(entity);

            entity = _mapper.Map(request, entity);

            // Act
            _paymentsRepositoryMock.Setup(r =>
               r.UpdatePaymentStatusAsync(entity));

            Func<Task> action = async () => await _service.UpdatePaymentStatusAsync(Id, request);

            // Assert
            await action.Should().NotThrowAsync();
        }

        [TestMethod]
        public async Task UpdatePaymentStatus_ThrowArgumentException_WithInValidErrorCode()
        {
            // Arrange
            var Id = new Guid();
            var request = _fixture.Build<PaymentStatusUpdateRequestDto>().With(d => d.UpdatedByUserId, new Guid()).With(x => x.UpdatedByOrganisationId, new Guid()).With(k => k.ErrorCode, "X").Create();

            //Assert
            await _service.Invoking(async x => await x.UpdatePaymentStatusAsync(Id, request))
                .Should().ThrowAsync<ArgumentException>();
        }

        [TestMethod]
        public async Task UpdatePaymentStatus_ReturnsArgumentException_WithNullUserId()
        {
            // Act & Assert
            var id = new Guid();
            var request = _fixture.Build<PaymentStatusUpdateRequestDto>().With(d => d.UpdatedByUserId, (Guid?)null).Create();

            await _service.Invoking(async x => await x.UpdatePaymentStatusAsync(id, request))
                .Should().ThrowAsync<ArgumentException>();
        }

        [TestMethod]
        public async Task UpdatePaymentStatus_ReturnsArgumentException_WithNullOrganisationId()
        {
            // Act & Assert
            var id = new Guid();
            var request = _fixture.Build<PaymentStatusUpdateRequestDto>().With(d => d.UpdatedByOrganisationId, (Guid?)null).Create();

            await _service.Invoking(async x => await x.UpdatePaymentStatusAsync(id, request))
                .Should().ThrowAsync<ArgumentException>();
        }
    }
}
