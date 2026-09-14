using AutoFixture;
using AutoMapper;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Payments;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationSubmission;
using EPR.Payment.Service.Common.Data.Profiles;
using EPR.Payment.Service.Common.Dtos.Request.Payments;
using EPR.Payment.Service.Common.Enums;
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
        private Mock<IRegistrationSubmissionDataRepository> _rsdRepositoryMock = null!;
        private Mapper _mapper = null!;
        private OfflinePaymentsService? _service = null!;

        private CancellationToken _cancellationToken;

        [TestInitialize]
        public void TestInitialize()
        {
            _fixture = new Fixture();
            _offlinePaymentsRepositoryMock = new Mock<IOfflinePaymentsRepository>();
            _rsdRepositoryMock = new Mock<IRegistrationSubmissionDataRepository>();
            var configuration = SetupAutomapper();
            _mapper = new Mapper(configuration);
            _cancellationToken = new CancellationToken();
            _service = new OfflinePaymentsService(_mapper, _offlinePaymentsRepositoryMock.Object, _rsdRepositoryMock.Object);
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
            var service = new OfflinePaymentsService(_mapper, _offlinePaymentsRepositoryMock.Object, _rsdRepositoryMock.Object);

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
            Action act = () => { var unused = new OfflinePaymentsService(null!, _offlinePaymentsRepositoryMock.Object, _rsdRepositoryMock.Object); };

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
            Action act = () =>
            {
                // Assign the instance to a variable to avoid CA1806
                var unused = new OfflinePaymentsService(_mapper, offlinePaymentsRepositoryMock!, _rsdRepositoryMock.Object);
            };

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
        public void Constructor_WhenRsdRepositoryIsNull_ShouldThrowArgumentNullException()
        {
            Action act = () => { var unused = new OfflinePaymentsService(_mapper, _offlinePaymentsRepositoryMock.Object, null!); };

            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'registrationSubmissionDataRepository')");
        }

        [TestMethod]
        [AutoMoqData]
        public async Task InsertOfflinePaymentAsync_MatchingReference_StampsRegistrationSubmissionDataId()
        {
            var request = _fixture!.Build<OfflinePaymentInsertRequestDto>().With(d => d.UserId, Guid.NewGuid()).With(x => x.Reference, "PEPR2699999").Create();
            var expectedRsdId = Guid.NewGuid();
            Common.Data.DataModels.Payment? captured = null;

            _rsdRepositoryMock
                .Setup(r => r.GetLatestIdByApplicationReferenceNumberAsync("PEPR2699999", _cancellationToken))
                .ReturnsAsync(expectedRsdId);
            _offlinePaymentsRepositoryMock
                .Setup(r => r.InsertOfflinePaymentAsync(It.IsAny<Common.Data.DataModels.Payment>(), _cancellationToken))
                .Callback<Common.Data.DataModels.Payment, CancellationToken>((p, _) => captured = p)
                .Returns(Task.CompletedTask);

            await _service!.InsertOfflinePaymentAsync(request, _cancellationToken);

            captured!.RegistrationSubmissionDataId.Should().Be(expectedRsdId);
        }

        [TestMethod]
        [AutoMoqData]
        public async Task InsertOfflinePaymentAsync_NoMatchingReference_LeavesRegistrationSubmissionDataIdNull()
        {
            var request = _fixture!.Build<OfflinePaymentInsertRequestDto>().With(d => d.UserId, Guid.NewGuid()).Create();
            Common.Data.DataModels.Payment? captured = null;

            _rsdRepositoryMock
                .Setup(r => r.GetLatestIdByApplicationReferenceNumberAsync(It.IsAny<string>(), _cancellationToken))
                .ReturnsAsync((Guid?)null);
            _offlinePaymentsRepositoryMock
                .Setup(r => r.InsertOfflinePaymentAsync(It.IsAny<Common.Data.DataModels.Payment>(), _cancellationToken))
                .Callback<Common.Data.DataModels.Payment, CancellationToken>((p, _) => captured = p)
                .Returns(Task.CompletedTask);

            await _service!.InsertOfflinePaymentAsync(request, _cancellationToken);

            captured!.RegistrationSubmissionDataId.Should().BeNull();
        }
    }
}
