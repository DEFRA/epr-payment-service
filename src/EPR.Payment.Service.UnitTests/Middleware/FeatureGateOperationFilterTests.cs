using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using EPR.Payment.Service.Helper;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;
using Microsoft.OpenApi.Models;
using Moq;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.UnitTests.Middleware
{
    [TestClass]
    public class FeatureGateOperationFilterTests
    {
        private Mock<IFeatureManager>? _featureManagerMock;
        private FeatureGateOperationFilter? _filter;

        [TestInitialize]
        public void Setup()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            _featureManagerMock = fixture.Freeze<Mock<IFeatureManager>>();
            _filter = new FeatureGateOperationFilter(_featureManagerMock.Object);
        }

        [TestMethod]
        public void Apply_SetsDeprecated_WhenFeatureIsDisabled()
        {
            // Arrange
            var operation = new OpenApiOperation();
            var methodInfo = typeof(TestController).GetMethod(nameof(TestController.FeatureGatedMethod));
            var context = new OperationFilterContext(
                new ApiDescription(),
                Mock.Of<ISchemaGenerator>(),
                new SchemaRepository(),
                methodInfo
            );

            _featureManagerMock?.Setup(x => x.IsEnabledAsync(It.IsAny<string>())).ReturnsAsync(false);

            // Act
            _filter?.Apply(operation, context);

            // Assert
            using (new AssertionScope())
            {
                operation.Deprecated.Should().BeTrue();
                operation.Description.Should().Contain("(This feature is currently disabled)");
            }
        }

        [TestMethod]
        public void Apply_DoesNotSetDeprecated_WhenFeatureIsEnabled()
        {
            // Arrange
            var operation = new OpenApiOperation();
            var methodInfo = typeof(TestController).GetMethod(nameof(TestController.FeatureGatedMethod));
            var context = new OperationFilterContext(
                new ApiDescription(),
                Mock.Of<ISchemaGenerator>(),
                new SchemaRepository(),
                methodInfo
            );

            _featureManagerMock?.Setup(x => x.IsEnabledAsync(It.IsAny<string>())).ReturnsAsync(true);

            // Act
            _filter?.Apply(operation, context);

            // Assert
            using (new AssertionScope())
            {
                operation.Deprecated.Should().BeFalse();
                operation.Description.Should().BeNullOrEmpty();
            }
        }

        [TestMethod]
        public void Apply_DoesNothing_WhenNoFeatureGateAttributes()
        {
            // Arrange
            var operation = new OpenApiOperation();
            var methodInfo = typeof(TestController).GetMethod(nameof(TestController.NoFeatureGateMethod));
            var context = new OperationFilterContext(
                new ApiDescription(),
                Mock.Of<ISchemaGenerator>(),
                new SchemaRepository(),
                methodInfo
            );

            // Act
            _filter?.Apply(operation, context);

            // Assert
            using (new AssertionScope())
            {
                operation.Deprecated.Should().BeFalse();
                operation.Description.Should().BeNullOrEmpty();
            }
        }

        [ExcludeFromCodeCoverage]
        public class TestController
        {
            [FeatureGate("TestFeature")]
            public void FeatureGatedMethod() { }

            public void NoFeatureGateMethod() { }
        }
    }
}
