using AutoFixture.MSTest;
using AutoMapper;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories;
using EPR.Payment.Service.Common.Dtos.Responses;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Services;
using FluentAssertions;
using Moq;

namespace EPR.Payment.Service.UnitTests.Services
{
    [TestClass]
    public class AccreditationFeesServiceTests
    {
        [TestMethod]
        [AutoMoqData]
        public void AccreditationFeesService_WhenInitialisedWithNullMapper_ThrowsArgumentNullException(
            [Frozen] Mock<IAccreditationFeesRepository> _accreditationFeesRepositoryMock,
            bool isLarge,
            string regulator
            )
        {
            //Arrange
            Mock<IMapper> mapperMock = new Mock<IMapper>();

            //Act
            var sut = new AccreditationFeesService(mapperMock.Object, _accreditationFeesRepositoryMock.Object);

            //Assert
            Assert.ThrowsExceptionAsync<ArgumentNullException>(() => sut.GetFees(isLarge, regulator));
        }

        [TestMethod]
        [AutoMoqData]
        public void AccreditationFeesService_WhenInitialisedWithNullAccreditationFeesRepository_ThrowsArgumentNullException(
            [Frozen] Mock<IMapper> _mapperMock,
            bool isLarge,
            string regulator
            )
        {
            //Arrange
            Mock<IAccreditationFeesRepository> accreditationFeesRepositoryMock = new Mock<IAccreditationFeesRepository>();

            //Act
            var sut = new AccreditationFeesService(_mapperMock.Object, accreditationFeesRepositoryMock.Object);

            //Assert
            Assert.ThrowsExceptionAsync<ArgumentNullException>(() => sut.GetFees(isLarge, regulator));
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetFees_RepositoryReturnsAResult_ReturnsNotNullMappedObject(
            [Frozen] Mock<IAccreditationFeesRepository> _accreditationFeesRepositoryMock,
            [Frozen] Mock<IMapper> _mapperMock,
            [Frozen] AccreditationFees _accreditationFees,
            [Frozen] GetAccreditationFeesResponse _getAccreditationFeesResponse,
            [Greedy] AccreditationFeesService sut,
            bool isLarge,
            string regulator
            )
        {
            //Arrange
            _accreditationFeesRepositoryMock.Setup(i => i.GetFeesAsync(It.IsAny<bool>(), It.IsAny<string>())).ReturnsAsync(_accreditationFees);
            _mapperMock.Setup(i => i.Map<GetAccreditationFeesResponse>(_accreditationFees)).Returns(_getAccreditationFeesResponse);

            //Act
            var result = await sut.GetFees(isLarge, regulator);

            //Assert
            result.Should().Be(_getAccreditationFeesResponse);
            _mapperMock.Verify(i => i.Map<GetAccreditationFeesResponse>(_accreditationFees), Times.Once);
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetFees_RepositoryReturnsAResult_ReturnsNullMappedObject(
            [Frozen] Mock<IAccreditationFeesRepository> _accreditationFeesRepositoryMock,
            [Frozen] Mock<IMapper> _mapperMock,
            [Frozen] GetAccreditationFeesResponse _getAccreditationFeesResponse,
            [Greedy] AccreditationFeesService sut,
            bool isLarge,
            string regulator
            )
        {
            //Arrange
            _accreditationFeesRepositoryMock.Setup(i => i.GetFeesAsync(It.IsAny<bool>(), It.IsAny<string>())).ReturnsAsync((AccreditationFees?) null);
            _mapperMock.Setup(i => i.Map<GetAccreditationFeesResponse>(null)).Returns(_getAccreditationFeesResponse);

            //Act
            var result = await sut.GetFees(isLarge, regulator);

            //Assert
            result.Should().Be(_getAccreditationFeesResponse);
            _mapperMock.Verify(i => i.Map<GetAccreditationFeesResponse>(null), Times.Once);
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetFeesAmount_RepositoryReturnsAResult_ReturnsNotNullDecimal(
            [Frozen] Mock<IAccreditationFeesRepository> _accreditationFeesRepositoryMock,
            [Greedy] AccreditationFeesService sut,
            bool isLarge,
            string regulator,
            decimal feesResult
            )
        {
            //Arrange
            _accreditationFeesRepositoryMock.Setup(i => i.GetFeesAmountAsync(It.IsAny<bool>(), It.IsAny<string>())).ReturnsAsync(feesResult);

            //Act
            var result = await sut.GetFeesAmount(isLarge, regulator);

            //Assert
            result.Should().Be(feesResult);
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetFeesAmount_RepositoryReturnsAResult_ReturnsNullDecimal(
            [Frozen] Mock<IAccreditationFeesRepository> _accreditationFeesRepositoryMock,
            [Greedy] AccreditationFeesService sut,
            bool isLarge,
            string regulator
            )
        {
            //Arrange
            _accreditationFeesRepositoryMock.Setup(i => i.GetFeesAmountAsync(It.IsAny<bool>(), It.IsAny<string>())).ReturnsAsync((decimal?)null);

            //Act
            var result = await sut.GetFeesAmount(isLarge, regulator);

            //Assert
            result.Should().Be(null);
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetFeesCount_RepositoryReturnsAResult_ReturnsNotNullInteger(
            [Frozen] Mock<IAccreditationFeesRepository> _accreditationFeesRepositoryMock,
            [Greedy] AccreditationFeesService sut,
            int feesCountResult
            )
        {
            //Arrange
            _accreditationFeesRepositoryMock.Setup(i => i.GetFeesCount()).ReturnsAsync(feesCountResult);

            //Act
            var result = await sut.GetFeesCount();

            //Assert
            result.Should().Be(feesCountResult);
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetFeesCount_RepositoryReturnsNoResult_ReturnsNoRecords(
            [Frozen] Mock<IAccreditationFeesRepository> _accreditationFeesRepositoryMock,
            [Greedy] AccreditationFeesService sut
            )
        {
            //Arrange
            _accreditationFeesRepositoryMock.Setup(i => i.GetFeesCount()).ReturnsAsync(0);

            //Act
            var result = await sut.GetFeesCount();

            //Assert
            result.Should().Be(0);
        }

    }
}
