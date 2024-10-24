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
    public class OfflinePaymentsServiceTests
    {
        private Fixture? _fixture = null!;
        private Mock<IOfflinePaymentsRepository> _offlinePaymentsRepositoryMock = null!;
        private Mapper? _mapper = null!;
        private Mock<IValidator<OfflinePaymentStatusInsertRequestDto>> _offlinePaymentStatusInsertRequestDtoMock = null!;
        private OfflinePaymentsService? _service = null!;

        private CancellationToken _cancellationToken;

        [TestInitialize]
        public void TestInitialize()
        {
            _fixture = new Fixture();
            _offlinePaymentsRepositoryMock = new Mock<IOfflinePaymentsRepository>();
            var configuration = SetupAutomapper();
            _mapper = new Mapper(configuration);
            _cancellationToken = new CancellationToken();
            _offlinePaymentStatusInsertRequestDtoMock = new Mock<IValidator<OfflinePaymentStatusInsertRequestDto>>();
            _service = new OfflinePaymentsService(_mapper, _offlinePaymentsRepositoryMock.Object, _offlinePaymentStatusInsertRequestDtoMock.Object);
        }

        private static MapperConfiguration SetupAutomapper()
        {
            var myProfile = new OfflinePaymentProfile();
            return new MapperConfiguration(c => c.AddProfile(myProfile));
        }

        [TestMethod]
        [AutoMoqData]
        public async Task InsertOfflinePaymentStatusAsync_ValidInput_ShouldReturnGuid([Frozen] Guid expectedResult)
        {
            // Arrange
            var request = _fixture!.Build<OfflinePaymentStatusInsertRequestDto>().With(d => d.UserId, Guid.NewGuid()).Create();

            _offlinePaymentStatusInsertRequestDtoMock.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(new ValidationResult());

            _offlinePaymentsRepositoryMock.Setup(r =>
               r.InsertOfflinePaymentAsync(It.IsAny<Common.Data.DataModels.OfflinePayment>(), _cancellationToken)).ReturnsAsync(expectedResult);

            // Act
            var result = await _service!.InsertOfflinePaymentAsync(request, _cancellationToken);

            // Assert
            result.Should().Be(expectedResult);
        }

        [TestMethod]
        public async Task InsertOfflinePaymentStatusAsync_ValiditonFails_ShouldThrowValidationException()
        {
            // Arrange
            var request = _fixture!.Build<OfflinePaymentStatusInsertRequestDto>().With(d => d.UserId, (Guid?)null).Create();

            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure(nameof(request.UserId), "User ID cannot be null or empty.")
            };

            _offlinePaymentStatusInsertRequestDtoMock.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(new ValidationResult(validationFailures));

            // Act & Assert
            await _service.Invoking(async x => await x!.InsertOfflinePaymentAsync(request, _cancellationToken))
                .Should().ThrowAsync<ValidationException>();
        }
    }
}
