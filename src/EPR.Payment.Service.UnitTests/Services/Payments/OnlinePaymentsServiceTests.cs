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
    public class OnlinePaymentsServiceTests
    {
        private Fixture? _fixture = null!;
        private Mock<IOnlinePaymentsRepository> _onlinePaymentsRepositoryMock = null!;
        private Mapper? _mapper = null!;
        private Mock<IValidator<OnlinePaymentStatusInsertRequestDto>> _onlinePaymentStatusInsertRequestDtoMock = null!;
        private Mock<IValidator<OnlinePaymentStatusUpdateRequestDto>> _onlinePaymentStatusUpdateRequestDtoMock = null!;
        private OnlinePaymentsService? _service = null!;

        private CancellationToken _cancellationToken;

        [TestInitialize]
        public void TestInitialize()
        {
            _fixture = new Fixture();
            _onlinePaymentsRepositoryMock = new Mock<IOnlinePaymentsRepository>();
            var configuration = SetupAutomapper();
            _mapper = new Mapper(configuration);
            _cancellationToken = new CancellationToken();
            _onlinePaymentStatusInsertRequestDtoMock = new Mock<IValidator<OnlinePaymentStatusInsertRequestDto>>();
            _onlinePaymentStatusUpdateRequestDtoMock = new Mock<IValidator<OnlinePaymentStatusUpdateRequestDto>>();
            _service = new OnlinePaymentsService(_mapper, _onlinePaymentsRepositoryMock.Object, _onlinePaymentStatusInsertRequestDtoMock.Object, _onlinePaymentStatusUpdateRequestDtoMock.Object);
        }

        private static MapperConfiguration SetupAutomapper()
        {
            var myProfile = new PaymentProfile();
            return new MapperConfiguration(c => c.AddProfile(myProfile));
        }

        [TestMethod]
        [AutoMoqData]
        public async Task InsertOnlinePaymentStatusAsync_ValidInput_ShouldReturnGuid([Frozen] Guid expectedResult)
        {
            // Arrange
            var request = _fixture!.Build<OnlinePaymentStatusInsertRequestDto>().With(d => d.UserId, Guid.NewGuid()).With(x => x.OrganisationId, Guid.NewGuid()).Create();

            _onlinePaymentStatusInsertRequestDtoMock.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(new ValidationResult());

            _onlinePaymentsRepositoryMock.Setup(r =>
               r.InsertOnlinePaymentAsync(It.IsAny<Common.Data.DataModels.OnlinePayment>(), _cancellationToken)).ReturnsAsync(expectedResult);

            // Act
            var result = await _service!.InsertOnlinePaymentStatusAsync(request, _cancellationToken);

            // Assert
            result.Should().Be(expectedResult);
        }

        [TestMethod]
        public async Task InsertOnlinePaymentStatusAsync_ValiditonFails_ShouldThrowValidationException()
        {
            // Arrange
            var request = _fixture!.Build<OnlinePaymentStatusInsertRequestDto>().With(d => d.UserId, (Guid?)null).With(d => d.OrganisationId, (Guid?)null).Create();

            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure(nameof(request.UserId), "User ID cannot be null or empty."),
                new ValidationFailure(nameof(request.OrganisationId), "Organisation ID cannot be null or empty.")
            };

            _onlinePaymentStatusInsertRequestDtoMock.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(new ValidationResult(validationFailures));

            // Act & Assert
            await _service.Invoking(async x => await x!.InsertOnlinePaymentStatusAsync(request, _cancellationToken))
                .Should().ThrowAsync<ValidationException>();
        }

        [TestMethod]
        [AutoMoqData]
        public async Task UpdateOnlinePaymentStatusAsync_ValidInput_NotThrowException([Frozen] Guid id)
        {
            // Arrange
            var request = _fixture!.Build<OnlinePaymentStatusUpdateRequestDto>().With(d => d.UpdatedByUserId, Guid.NewGuid()).With(x => x.UpdatedByOrganisationId, Guid.NewGuid()).Create();

            var entity = new Common.Data.DataModels.OnlinePayment();
            _onlinePaymentsRepositoryMock.Setup(r => r.GetOnlinePaymentByExternalPaymentIdAsync(id, _cancellationToken)).ReturnsAsync(entity);

            entity = _mapper!.Map(request, entity);

            // Act
            _onlinePaymentsRepositoryMock.Setup(r =>
               r.UpdateOnlinePaymentAsync(entity, _cancellationToken));

            _onlinePaymentStatusUpdateRequestDtoMock.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(new ValidationResult());

            Func<Task> action = async () => await _service!.UpdateOnlinePaymentStatusAsync(id, request, _cancellationToken);

            // Assert
            await action.Should().NotThrowAsync();
        }

        [TestMethod]
        [AutoMoqData]
        public async Task UpdateOnlinePaymentStatusAsync_ValiditonFails_ShouldThrowValidationException([Frozen] Guid id)
        {
            // Arrange
            var request = _fixture!.Build<OnlinePaymentStatusUpdateRequestDto>().With(d => d.UpdatedByUserId, (Guid?)null).With(d => d.UpdatedByOrganisationId, (Guid?)null).Create();

            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure(nameof(request.UpdatedByUserId), "Updated User ID cannot be null or empty."),
                new ValidationFailure(nameof(request.UpdatedByOrganisationId), "Updated Organisation ID cannot be null or empty.")
            };

            _onlinePaymentStatusUpdateRequestDtoMock.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(new ValidationResult(validationFailures));

            // Act & Assert
            await _service.Invoking(async x => await x!.UpdateOnlinePaymentStatusAsync(id, request, _cancellationToken))
                .Should().ThrowAsync<ValidationException>();
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetOnlinePaymentStatusCountAsync_RepositoryReturnsAResult_ShouldReturnNotNullInteger([Frozen] int onlinePaymentStatusCountResult)
        {
            //Arrange
            _onlinePaymentsRepositoryMock.Setup(i => i.GetPaymentStatusCount(_cancellationToken)).ReturnsAsync(onlinePaymentStatusCountResult);

            //Act
            var result = await _service!.GetOnlinePaymentStatusCountAsync(_cancellationToken);

            //Assert
            result.Should().Be(onlinePaymentStatusCountResult);
        }

        [TestMethod]
        public async Task GetOnlinePaymentStatusCountAsync_RepositoryReturnsNoResult_ShouldReturnsNoRecords()
        {
            //Arrange
            _onlinePaymentsRepositoryMock.Setup(i => i.GetPaymentStatusCount(_cancellationToken)).ReturnsAsync(0);

            //Act
            var result = await _service!.GetOnlinePaymentStatusCountAsync(_cancellationToken);

            //Assert
            result.Should().Be(0);
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetOnlinePaymentByExternalPaymentIdAsync_RepositoryReturnsAResult_ShouldReturnMappedObject(
            [Frozen] Common.Data.DataModels.OnlinePayment onlinePaymentEntity,
            Guid externalPaymentId
            )
        {
            //Arrange
            _onlinePaymentsRepositoryMock.Setup(i => i.GetOnlinePaymentByExternalPaymentIdAsync(externalPaymentId, _cancellationToken)).ReturnsAsync(onlinePaymentEntity);

            var expectedResult = _mapper!.Map<OnlinePaymentResponseDto>(onlinePaymentEntity);

            //Act
            var result = await _service!.GetOnlinePaymentByExternalPaymentIdAsync(externalPaymentId, _cancellationToken);

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
        public async Task GetOnlinePaymentByExternalPaymentIdAsync_RepositoryReturnsAResult_ShouldReturnNullMappedObject(
            Guid externalPaymentId
            )
        {
            //Arrange
            _onlinePaymentsRepositoryMock.Setup(i => i.GetOnlinePaymentByExternalPaymentIdAsync(externalPaymentId, _cancellationToken)).ReturnsAsync((Common.Data.DataModels.OnlinePayment?)null);

            var expectedResult = _mapper!.Map<OnlinePaymentResponseDto>(null);

            //Act
            var result = await _service!.GetOnlinePaymentByExternalPaymentIdAsync(externalPaymentId, _cancellationToken);

            //Assert
            result.Should().Be(expectedResult);
        }
    }
}
