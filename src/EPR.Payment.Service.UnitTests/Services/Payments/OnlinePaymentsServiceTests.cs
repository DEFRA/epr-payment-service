using AutoFixture;
using AutoFixture.MSTest;
using AutoMapper;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Payments;
using EPR.Payment.Service.Common.Data.Profiles;
using EPR.Payment.Service.Common.Dtos.Request.Payments;
using EPR.Payment.Service.Common.Dtos.Response.Payments;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Services.Interfaces.Payments;
using EPR.Payment.Service.Services.Payments;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;

namespace EPR.Payment.Service.UnitTests.Services.Payments
{
    [TestClass]
    public class OnlinePaymentsServiceTests
    {
        private Fixture? _fixture = null!;
        private Mock<IOnlinePaymentsRepository> _onlinePaymentsRepositoryMock = null!;
        private Mapper _mapper = null!;
        private OnlinePaymentsService? _service = null!;

        private CancellationToken _cancellationToken;

        [TestInitialize]
        public void TestInitialize()
        {
            _fixture = new Fixture();
            _onlinePaymentsRepositoryMock = new Mock<IOnlinePaymentsRepository>();
            MapperConfiguration configuration = SetupAutomapper();
            _mapper = new Mapper(configuration);
            _cancellationToken = new CancellationToken();
            _service = new OnlinePaymentsService(_mapper, _onlinePaymentsRepositoryMock.Object);
        }

        private static MapperConfiguration SetupAutomapper()
        {
            PaymentProfile myProfile = new PaymentProfile();
            return new MapperConfiguration(c => c.AddProfile(myProfile));
        }

        [TestMethod]
        public void Constructor_WhenAllDependenciesAreNotNull_ShouldInitialize()
        {
            // Act
            OnlinePaymentsService service = new OnlinePaymentsService(_mapper, _onlinePaymentsRepositoryMock.Object);

            // Assert
            using (new AssertionScope())
            {
                service.Should().NotBeNull();
                service.Should().BeAssignableTo<IOnlinePaymentsService>();
            }
        }

        [TestMethod]
        public void Constructor_WhenMapperIsNull_ShouldThrowArgumentNullException()
        {
            // Act
            Action act = () => new OnlinePaymentsService(null!, _onlinePaymentsRepositoryMock.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'mapper')");
        }

        [TestMethod]
        public void Constructor_WhenonlinePaymentsRepositoryIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            IOnlinePaymentsRepository? onlinePaymentsRepository = null;

            // Act
            Action act = () => new OnlinePaymentsService(_mapper, onlinePaymentsRepository!);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'onlinePaymentRepository')");
        }

        [TestMethod]
        [AutoMoqData]
        public async Task InsertOnlinePaymentStatusAsync_ValidInput_ShouldReturnGuid([Frozen] Guid expectedResult)
        {
            // Arrange
            OnlinePaymentInsertRequestDto request = _fixture!.Build<OnlinePaymentInsertRequestDto>().With(d => d.UserId, Guid.NewGuid()).With(x => x.OrganisationId, Guid.NewGuid()).Create();

            _onlinePaymentsRepositoryMock.Setup(r =>
               r.InsertOnlinePaymentAsync(It.IsAny<Common.Data.DataModels.Payment>(), _cancellationToken)).ReturnsAsync(expectedResult);

            // Act
            Guid result = await _service!.InsertOnlinePaymentAsync(request, _cancellationToken);

            // Assert
            result.Should().Be(expectedResult);
        }

        [TestMethod]
        [AutoMoqData]
        public async Task InsertOnlinePaymentV2StatusAsync_ValidInput_ShouldReturnGuid(Guid expectedResult)
        {
            // Arrange
            OnlinePaymentInsertRequestV2Dto request = _fixture!
                .Build<OnlinePaymentInsertRequestV2Dto>()
                .With(d => d.UserId, Guid.NewGuid())
                .With(x => x.OrganisationId, Guid.NewGuid())
                .Create();

            _onlinePaymentsRepositoryMock
                .Setup(r => r.InsertOnlinePaymentAsync(It.IsAny<Common.Data.DataModels.Payment>(), _cancellationToken))
                .ReturnsAsync(expectedResult);

            // Act
            Guid result = await _service!.InsertOnlinePaymentAsync(request, _cancellationToken);

            // Assert
            result.Should().Be(expectedResult);
        }

        [TestMethod]
        [AutoMoqData]
        public async Task UpdateOnlinePaymentStatusAsync_ValidInput_NotThrowException([Frozen] Guid id)
        {
            // Arrange
            OnlinePaymentUpdateRequestDto request = _fixture!.Build<OnlinePaymentUpdateRequestDto>().With(d => d.UpdatedByUserId, Guid.NewGuid()).With(x => x.UpdatedByOrganisationId, Guid.NewGuid()).Create();

            Common.Data.DataModels.Payment entity = new Common.Data.DataModels.Payment();
            _onlinePaymentsRepositoryMock.Setup(r => r.GetOnlinePaymentByExternalPaymentIdAsync(id, _cancellationToken)).ReturnsAsync(entity);

            entity = _mapper!.Map(request, entity);
            entity.OnlinePayment = _mapper!.Map(request, entity.OnlinePayment);

            // Act
            _onlinePaymentsRepositoryMock.Setup(r =>
               r.UpdateOnlinePayment(entity, _cancellationToken));

            Func<Task> action = async () => await _service!.UpdateOnlinePaymentAsync(id, request, _cancellationToken);

            // Assert
            await action.Should().NotThrowAsync();
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetOnlinePaymentStatusCountAsync_RepositoryReturnsAResult_ShouldReturnNotNullInteger([Frozen] int onlinePaymentStatusCountResult)
        {
            //Arrange
            _onlinePaymentsRepositoryMock.Setup(i => i.GetPaymentStatusCount(_cancellationToken)).ReturnsAsync(onlinePaymentStatusCountResult);

            //Act
            int result = await _service!.GetOnlinePaymentStatusCountAsync(_cancellationToken);

            //Assert
            result.Should().Be(onlinePaymentStatusCountResult);
        }

        [TestMethod]
        public async Task GetOnlinePaymentStatusCountAsync_RepositoryReturnsNoResult_ShouldReturnsNoRecords()
        {
            //Arrange
            _onlinePaymentsRepositoryMock.Setup(i => i.GetPaymentStatusCount(_cancellationToken)).ReturnsAsync(0);

            //Act
            int result = await _service!.GetOnlinePaymentStatusCountAsync(_cancellationToken);

            //Assert
            result.Should().Be(0);
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetOnlinePaymentByExternalPaymentIdAsync_RepositoryReturnsAResult_ShouldReturnMappedObject(
            [Frozen] Common.Data.DataModels.Payment onlinePaymentEntity,
            Guid externalPaymentId
            )
        {
            //Arrange
            _onlinePaymentsRepositoryMock.Setup(i => i.GetOnlinePaymentByExternalPaymentIdAsync(externalPaymentId, _cancellationToken)).ReturnsAsync(onlinePaymentEntity);

            OnlinePaymentResponseDto expectedResult = _mapper!.Map<OnlinePaymentResponseDto>(onlinePaymentEntity);

            //Act
            OnlinePaymentResponseDto result = await _service!.GetOnlinePaymentByExternalPaymentIdAsync(externalPaymentId, _cancellationToken);

            //Assert
            using (new AssertionScope())
            {
                result.GovPayPaymentId.Should().Be(expectedResult.GovPayPaymentId);
                result.UpdatedByOrganisationId.Should().Be(expectedResult.UpdatedByOrganisationId);
                result.UpdatedByUserId.Should().Be(expectedResult.UpdatedByUserId);
                result.Description.Should().Be(expectedResult.Description);
                result.RequestorType.Should().Be(expectedResult.RequestorType);
            }
        }        
    }
}
