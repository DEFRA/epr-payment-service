using AutoFixture;
using AutoMapper;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Payments;
using EPR.Payment.Service.Common.Data.Profiles;
using EPR.Payment.Service.Common.Dtos.Enums;
using EPR.Payment.Service.Common.Dtos.Request.Payments;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Services.Interfaces.Payments;
using EPR.Payment.Service.Services.Payments;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;

namespace EPR.Payment.Service.UnitTests.Services.Payments
{
    [TestClass]
    public class OfflinePaymentsServiceTests
    {
        private Fixture? _fixture = null!;
        private Mock<IOfflinePaymentsRepository> _offlinePaymentsRepositoryMock = null!;
        private Mapper _mapper = null!;
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
            _service = new OfflinePaymentsService(_mapper, _offlinePaymentsRepositoryMock.Object);
        }

        private static MapperConfiguration SetupAutomapper()
        {
            var myProfile = new PaymentProfile();
            return new MapperConfiguration(c => c.AddProfile(myProfile));
        }

        [TestMethod]
        public void Constructor_WhenAllDependenciesAreNotNull_ShouldInitialize()
        {
            // Act
            var service = new OfflinePaymentsService(_mapper, _offlinePaymentsRepositoryMock.Object);

            // Assert
            using (new AssertionScope())
            {
                service.Should().NotBeNull();
                service.Should().BeAssignableTo<IOfflinePaymentsService>();
            }
        }

        [TestMethod]
        public void Constructor_WhenMapperIsNull_ShouldThrowArgumentNullException()
        {
            // Act
            Action act = () => new OfflinePaymentsService(null!, _offlinePaymentsRepositoryMock.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'mapper')");
        }

        [TestMethod]
        public void Constructor_WhenOfflinePaymentsRepositoryIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            IOfflinePaymentsRepository? offlinePaymentsRepositoryMock = null;

            // Act
            Action act = () => new OfflinePaymentsService(_mapper, offlinePaymentsRepositoryMock!);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'offlinePaymentRepository')");
        }

        [TestMethod]
        [AutoMoqData]
        public async Task InsertOfflinePaymentAsync_ValidInput_ShouldCallRespository()
        {
            // Arrange
            var request = _fixture!.Build<OfflinePaymentInsertRequestDto>().With(d => d.UserId, Guid.NewGuid()).Create();

            _offlinePaymentsRepositoryMock.Setup(r =>
               r.InsertOfflinePaymentAsync(It.IsAny<Common.Data.DataModels.Payment>(), _cancellationToken));

            // Act
            Func<Task> action = async () => await _service!.InsertOfflinePaymentAsync(request, _cancellationToken);

            // Assert
            await action.Should().NotThrowAsync();

        }

        [TestMethod]
        [AutoMoqData]
        public async Task InsertOfflinePaymentAsyncForV2_ValidInput_ShouldCallRespository()
        {
            // Arrange
            var request = _fixture!.Build<OfflinePaymentInsertRequestV2Dto>().With(d => d.UserId, Guid.NewGuid()).Create();
            request.PaymentMethod = OfflinePaymentMethodTypes.BankTransfer;

            _offlinePaymentsRepositoryMock.Setup(r =>
               r.InsertOfflinePaymentAsync(It.IsAny<Common.Data.DataModels.Payment>(), _cancellationToken));

            // Act
            Func<Task> action = async () => await _service!.InsertOfflinePaymentAsync(request, _cancellationToken);

            // Assert
            await action.Should().NotThrowAsync();

        }
    }
}
