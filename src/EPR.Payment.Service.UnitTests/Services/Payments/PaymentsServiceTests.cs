using AutoFixture;
using AutoFixture.MSTest;
using AutoMapper;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Payments;
using EPR.Payment.Service.Common.Data.Profiles;
using EPR.Payment.Service.Common.Dtos.Request.Payments;
using EPR.Payment.Service.Common.Dtos.Response.Payments;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Services.Payments;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace EPR.Payment.Service.UnitTests.Services.Payments
{
    [TestClass]
    public class PaymentsServiceTests
    {
        private Fixture? _fixture = null!;
        private Mock<IPaymentsRepository> _paymentsRepositoryMock = null!;
        private Mapper? _mapper = null!;
        private Mock<IValidator<PaymentStatusInsertRequestDto>> _paymentStatusInsertRequestDtoMock = null!;
        private Mock<IValidator<PaymentStatusUpdateRequestDto>> _paymentStatusUpdateRequestDtoMock = null!;
        private PaymentsService? _service = null!;

        private CancellationToken _cancellationToken;

        [TestInitialize]
        public void TestInitialize()
        {
            _fixture = new Fixture();
            _paymentsRepositoryMock = new Mock<IPaymentsRepository>();
            var configuration = SetupAutomapper();
            _mapper = new Mapper(configuration);
            _cancellationToken = new CancellationToken();
            _paymentStatusInsertRequestDtoMock = new Mock<IValidator<PaymentStatusInsertRequestDto>>();
            _paymentStatusUpdateRequestDtoMock = new Mock<IValidator<PaymentStatusUpdateRequestDto>>();
            _service = new PaymentsService(_mapper, _paymentsRepositoryMock.Object, _paymentStatusInsertRequestDtoMock.Object, _paymentStatusUpdateRequestDtoMock.Object);
        }

        private static MapperConfiguration SetupAutomapper()
        {
            var myProfile = new PaymentProfile();
            return new MapperConfiguration(c => c.AddProfile(myProfile));
        }

        [TestMethod]
        [AutoMoqData]
        public async Task InsertPaymentStatusAsync_ValidInput_ShouldReturnGuid([Frozen] Guid expectedResult)
        {
            // Arrange
            var request = _fixture!.Build<PaymentStatusInsertRequestDto>().With(d => d.UserId, new Guid()).With(x => x.OrganisationId, new Guid()).Create();

            _paymentStatusInsertRequestDtoMock.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(new ValidationResult());

            _paymentsRepositoryMock.Setup(r =>
               r.InsertPaymentStatusAsync(It.IsAny<Common.Data.DataModels.Payment>(), _cancellationToken)).ReturnsAsync(expectedResult);

            // Act
            var result = await _service!.InsertPaymentStatusAsync(request, _cancellationToken);

            // Assert
            result.Should().Be(expectedResult);
        }

        [TestMethod]
        public async Task InsertPaymentStatusAsync_ValiditonFails_ShouldThrowValidationException()
        {
            // Arrange
            var request = _fixture!.Build<PaymentStatusInsertRequestDto>().With(d => d.UserId, (Guid?)null).With(d => d.OrganisationId, (Guid?)null).Create();

            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure(nameof(request.UserId), "User ID cannot be null or empty."),
                new ValidationFailure(nameof(request.OrganisationId), "Organisation ID cannot be null or empty.")
            };

            _paymentStatusInsertRequestDtoMock.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(new ValidationResult(validationFailures));

            // Act & Assert
            await _service.Invoking(async x => await x!.InsertPaymentStatusAsync(request, _cancellationToken))
                .Should().ThrowAsync<ValidationException>();
        }

        [TestMethod]
        [AutoMoqData]
        public async Task UpdatePaymentStatusAsync_ValidInput_NotThrowException([Frozen] Guid id)
        {
            // Arrange
            var request = _fixture!.Build<PaymentStatusUpdateRequestDto>().With(d => d.UpdatedByUserId, new Guid()).With(x => x.UpdatedByOrganisationId, new Guid()).Create();

            var entity = new Common.Data.DataModels.Payment();
            _paymentsRepositoryMock.Setup(r => r.GetPaymentByExternalPaymentIdAsync(id, _cancellationToken)).ReturnsAsync(entity);

            entity = _mapper!.Map(request, entity);

            // Act
            _paymentsRepositoryMock.Setup(r =>
               r.UpdatePaymentStatusAsync(entity, _cancellationToken));

            _paymentStatusUpdateRequestDtoMock.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(new ValidationResult());

            Func<Task> action = async () => await _service!.UpdatePaymentStatusAsync(id, request, _cancellationToken);

            // Assert
            await action.Should().NotThrowAsync();
        }

        [TestMethod]
        [AutoMoqData]
        public async Task UpdatePaymentStatusAsync_ValiditonFails_ShouldThrowValidationException([Frozen] Guid id)
        {
            // Arrange
            var request = _fixture!.Build<PaymentStatusUpdateRequestDto>().With(d => d.UpdatedByUserId, (Guid?)null).With(d => d.UpdatedByOrganisationId, (Guid?)null).Create();

            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure(nameof(request.UpdatedByUserId), "Updated User ID cannot be null or empty."),
                new ValidationFailure(nameof(request.UpdatedByOrganisationId), "Updated Organisation ID cannot be null or empty.")
            };

            _paymentStatusUpdateRequestDtoMock.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(new ValidationResult(validationFailures));

            // Act & Assert
            await _service.Invoking(async x => await x!.UpdatePaymentStatusAsync(id, request, _cancellationToken))
                .Should().ThrowAsync<ValidationException>();
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetPaymentStatusCountAsync_RepositoryReturnsAResult_ShouldReturnNotNullInteger([Frozen] int paymentStatusCountResult)
        {
            //Arrange
            _paymentsRepositoryMock.Setup(i => i.GetPaymentStatusCount(_cancellationToken)).ReturnsAsync(paymentStatusCountResult);

            //Act
            var result = await _service!.GetPaymentStatusCountAsync(_cancellationToken);

            //Assert
            result.Should().Be(paymentStatusCountResult);
        }

        [TestMethod]
        public async Task GetPaymentStatusCountAsync_RepositoryReturnsNoResult_ShouldReturnsNoRecords()
        {
            //Arrange
            _paymentsRepositoryMock.Setup(i => i.GetPaymentStatusCount(_cancellationToken)).ReturnsAsync(0);

            //Act
            var result = await _service!.GetPaymentStatusCountAsync(_cancellationToken);

            //Assert
            result.Should().Be(0);
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetPaymentByExternalPaymentIdAsync_RepositoryReturnsAResult_ShouldReturnMappedObject(
            [Frozen] Common.Data.DataModels.Payment paymentEntity,
            Guid externalPaymentId
            )
        {
            //Arrange
            _paymentsRepositoryMock.Setup(i => i.GetPaymentByExternalPaymentIdAsync(externalPaymentId, _cancellationToken)).ReturnsAsync(paymentEntity);

            var expectedResult = _mapper!.Map<PaymentResponseDto>(paymentEntity);

            //Act
            var result = await _service!.GetPaymentByExternalPaymentIdAsync(externalPaymentId, _cancellationToken);

            //Assert
            using (new AssertionScope())
            {
                result.GovPayPaymentId.Should().Be(expectedResult.GovPayPaymentId);
                result.UpdatedByOrganisationId.Should().Be(expectedResult.UpdatedByOrganisationId);
                result.UpdatedByUserId.Should().Be(expectedResult.UpdatedByUserId);
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetPaymentByExternalPaymentIdAsync_RepositoryReturnsAResult_ShouldReturnNullMappedObject(
            Guid externalPaymentId
            )
        {
            //Arrange
            _paymentsRepositoryMock.Setup(i => i.GetPaymentByExternalPaymentIdAsync(externalPaymentId, _cancellationToken)).ReturnsAsync((Common.Data.DataModels.Payment?)null);

            var expectedResult = _mapper!.Map<PaymentResponseDto>(null);

            //Act
            var result = await _service!.GetPaymentByExternalPaymentIdAsync(externalPaymentId, _cancellationToken);

            //Assert
            result.Should().Be(expectedResult);
        }
    }
}
