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

        [TestMethod]
        [AutoMoqData]
        public async Task GetFees_ServiceReturnsNullResult_ReturnsNotFoundResponse(
            [Frozen] Mock<IAccreditationFeesService> _accreditationFeesServiceMock,
            [Greedy] AccreditationFeesController sut,
            bool isLarge,
            string regulator)
        {
            //Arrange
            GetAccreditationFeesResponse? getAccreditationFeesResponse = null;
            _accreditationFeesServiceMock.Setup(i => i.GetFees(It.IsAny<bool>(), It.IsAny<string>())).ReturnsAsync(getAccreditationFeesResponse);

            //Act
            var result = await sut.GetFees(isLarge, regulator);

            //Assert
            result.Should().BeOfType<NotFoundResult>();
            result.As<NotFoundResult>().Should().NotBeNull();
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetFees_ServiceReturnsAResult_ReturnsOkResponse(
            [Frozen] Mock<IAccreditationFeesService> _accreditationFeesServiceMock,
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
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Should().NotBeNull();
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetFeesAmount_ServiceReturnsNullResult_ReturnsNotFoundResponse(
            [Frozen] Mock<IAccreditationFeesService> _accreditationFeesServiceMock,
            [Greedy] AccreditationFeesController sut,
            bool isLarge,
            string regulator)
        {
            //Arrange
            _accreditationFeesServiceMock.Setup(i => i.GetFeesAmount(It.IsAny<bool>(), It.IsAny<string>())).ReturnsAsync((decimal?)null);

            //Act
            var result = await sut.GetFeesAmount(isLarge, regulator);

            //Assert
            result.Should().BeOfType<NotFoundResult>();
            result.As<NotFoundResult>().Should().NotBeNull();
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetFeesAmount_ServiceReturnsAResult_ReturnsOkResponse(
            [Frozen] Mock<IAccreditationFeesService> _accreditationFeesServiceMock,
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
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Should().NotBeNull();
        }
    }
}