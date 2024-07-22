using EPR.Payment.Service.Helper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;
using Moq;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace EPR.Payment.Service.UnitTests.Middleware
{
    [TestClass]
    public class ConditionalEndpointMiddlewareTests
    {
        private Mock<RequestDelegate> _nextMock = null!;
        private Mock<IFeatureManager> _featureManagerMock = null!;
        private Mock<ILogger<ConditionalEndpointMiddleware>> _loggerMock = null!;
        private ConditionalEndpointMiddleware _middleware = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _nextMock = new Mock<RequestDelegate>();
            _featureManagerMock = new Mock<IFeatureManager>();
            _loggerMock = new Mock<ILogger<ConditionalEndpointMiddleware>>();
            _middleware = new ConditionalEndpointMiddleware(_nextMock.Object, _featureManagerMock.Object, _loggerMock.Object);
        }

        private DefaultHttpContext CreateHttpContextWithEndpoint(Endpoint? endpoint)
        {
            var context = new DefaultHttpContext();
            context.SetEndpoint(endpoint);
            return context;
        }

        [TestMethod]
        public async Task InvokeAsync_NoEndpoint_CallsNextMiddleware()
        {
            // Arrange
            var context = new DefaultHttpContext();

            // Act
            await _middleware.InvokeAsync(context);

            // Assert
            _nextMock.Verify(next => next(context), Times.Once);
        }

        [TestMethod]
        public async Task InvokeAsync_NoFeatureGateAttributes_CallsNextMiddleware()
        {
            // Arrange
            var endpoint = new Endpoint(c => Task.CompletedTask, new EndpointMetadataCollection(), "TestEndpoint");
            var context = CreateHttpContextWithEndpoint(endpoint);

            // Act
            await _middleware.InvokeAsync(context);

            // Assert
            _nextMock.Verify(next => next(context), Times.Once);
        }

        [TestMethod]
        public async Task InvokeAsync_FeatureEnabled_CallsNextMiddleware()
        {
            // Arrange
            var featureName = "TestFeature";
            _featureManagerMock.Setup(fm => fm.IsEnabledAsync(featureName)).ReturnsAsync(true);

            var actionDescriptor = new ControllerActionDescriptor
            {
                ControllerTypeInfo = typeof(TestController).GetTypeInfo(),
                MethodInfo = typeof(TestController).GetMethod(nameof(TestController.TestAction))!
            };
            var endpoint = new Endpoint(c => Task.CompletedTask, new EndpointMetadataCollection(actionDescriptor), "TestEndpoint");
            var context = CreateHttpContextWithEndpoint(endpoint);

            // Act
            await _middleware.InvokeAsync(context);

            // Assert
            _nextMock.Verify(next => next(context), Times.Once);
        }

        [TestMethod]
        public async Task InvokeAsync_FeatureDisabled_Returns404()
        {
            // Arrange
            var featureName = "TestFeature";
            _featureManagerMock.Setup(fm => fm.IsEnabledAsync(featureName)).ReturnsAsync(false);

            var actionDescriptor = new ControllerActionDescriptor
            {
                ControllerTypeInfo = typeof(TestController).GetTypeInfo(),
                MethodInfo = typeof(TestController).GetMethod(nameof(TestController.TestAction))!
            };
            var endpoint = new Endpoint(c => Task.CompletedTask, new EndpointMetadataCollection(actionDescriptor), "TestEndpoint");
            var context = CreateHttpContextWithEndpoint(endpoint);

            // Act
            await _middleware.InvokeAsync(context);

            // Assert
            using (new FluentAssertions.Execution.AssertionScope())
            {
                context.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
                _nextMock.Verify(next => next(It.IsAny<HttpContext>()), Times.Never);
            }
        }

        // A test controller for creating action descriptors
        [ExcludeFromCodeCoverage]
        [FeatureGate("TestFeature")]
        private class TestController
        {
            public void TestAction() { }
        }
    }
}
