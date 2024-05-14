using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Dtos.Responses;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Controllers;
using EPR.Payment.Service.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EPR.Payment.Service.UnitTests.Controllers
{
    [TestClass]
    public class AccreditationFeesControllerTests
    {
        private Mock<IAccreditationFeesService> _accreditationFeesServiceMock = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _accreditationFeesServiceMock = new Mock<IAccreditationFeesService>();
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetFees_ServiceReturnsNullResult_ReturnsNotFoundResponse(
            [Greedy] AccreditationFeesController sut,
            bool isLarge,
            string regulator)
        {
            //Arrange
            _accreditationFeesServiceMock.Setup(i => i.GetFees(It.IsAny<bool>(), It.IsAny<string>())).ReturnsAsync((GetAccreditationFeesResponse?)null);
            //Act
            var result = await sut.GetFees(isLarge, regulator);
            //Assert
            result.As<NotFoundResult>().Should().BeNull();
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetFees_ServiceReturnsAResult_ReturnsOkResponse(
            [Frozen] GetAccreditationFeesResponse getAccreditationFeesResponse,
            [Greedy] AccreditationFeesController sut,
            bool isLarge,
            string regulator)
        {
            //Arrange
            _accreditationFeesServiceMock.Setup(i => i.GetFees(It.IsAny<bool>(), It.IsAny<string>())).ReturnsAsync(getAccreditationFeesResponse);
            //Act
            var result = await sut.GetFees(isLarge, regulator);
            //Assert
            result.As<OkObjectResult>().Should().NotBeNull();
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetFeesAmount_ServiceReturnsNullResult_ReturnsNotFoundResponse(
            [Greedy] AccreditationFeesController sut,
            bool isLarge,
            string regulator)
        {
            //Arrange
            _accreditationFeesServiceMock.Setup(i => i.GetFeesAmount(It.IsAny<bool>(), It.IsAny<string>())).ReturnsAsync((decimal?)null);
            //Act
            var result = await sut.GetFeesAmount(isLarge, regulator);
            //Assert
            result.As<NotFoundResult>().Should().BeNull();
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetFeesAmount_ServiceReturnsAResult_ReturnsOkResponse(
            [Greedy] AccreditationFeesController sut,
            decimal value,
            bool isLarge,
            string regulator)
        {
            //Arrange
            _accreditationFeesServiceMock.Setup(i => i.GetFeesAmount(It.IsAny<bool>(), It.IsAny<string>())).ReturnsAsync(value);
            //Act
            var result = await sut.GetFeesAmount(isLarge, regulator);
            //Assert
            result.As<OkObjectResult>().Should().NotBeNull();
        }
    }
}