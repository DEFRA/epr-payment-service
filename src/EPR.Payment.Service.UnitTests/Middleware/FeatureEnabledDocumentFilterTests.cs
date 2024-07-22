using AutoFixture;
using AutoFixture.AutoMoq;
using EPR.Payment.Service.Helper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;
using Microsoft.OpenApi.Models;
using Moq;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace EPR.Payment.Service.UnitTests.Middleware
{
    [TestClass]
    public class FeatureEnabledDocumentFilterTests
    {
        private Mock<IFeatureManager> _featureManagerMock = null!;
        private FeatureEnabledDocumentFilter _filter = null!;
        private StringWriter _output = null!;

        [TestInitialize]
        public void Setup()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            _featureManagerMock = fixture.Freeze<Mock<IFeatureManager>>();
            var loggerMock = fixture.Freeze<Mock<ILogger<FeatureEnabledDocumentFilter>>>();
            _output = new StringWriter();
            Console.SetOut(_output);
            _filter = new FeatureEnabledDocumentFilter(_featureManagerMock.Object, loggerMock.Object);
        }

        [TestMethod]
        public void Apply_RemovesPaths_WhenActionFeatureIsDisabled()
        {
            // Arrange
            var swaggerDoc = new OpenApiDocument
            {
                Paths = new OpenApiPaths
                {
                    ["/test"] = new OpenApiPathItem
                    {
                        Operations = new Dictionary<OperationType, OpenApiOperation>
                        {
                            [OperationType.Get] = new OpenApiOperation()
                        }
                    }
                }
            };

            var apiDescription = new ApiDescription
            {
                RelativePath = "test",
                ActionDescriptor = new ControllerActionDescriptor
                {
                    ControllerTypeInfo = typeof(TestController).GetTypeInfo(),
                    EndpointMetadata = new List<object> { new FeatureGateAttribute("TestFeature") }
                }
            };

            var apiDescriptions = new List<ApiDescription> { apiDescription };
            var schemaRepository = new SchemaRepository();
            var schemaGeneratorMock = new Mock<ISchemaGenerator>();
            var context = new DocumentFilterContext(apiDescriptions, schemaGeneratorMock.Object, schemaRepository);

            _featureManagerMock.Setup(x => x.IsEnabledAsync("TestFeature")).ReturnsAsync(false);

            // Act
            _filter.Apply(swaggerDoc, context);

            // Assert
            using (new FluentAssertions.Execution.AssertionScope())
            {
                swaggerDoc.Paths.Should().NotContainKey("/test");
                _output.ToString().Should().Contain("Removing path '/test' from Swagger documentation because the feature gate is disabled.");
            }
        }

        [TestMethod]
        public void Apply_DoesNotRemovePaths_WhenActionFeatureIsEnabled()
        {
            // Arrange
            var swaggerDoc = new OpenApiDocument
            {
                Paths = new OpenApiPaths
                {
                    ["/test"] = new OpenApiPathItem
                    {
                        Operations = new Dictionary<OperationType, OpenApiOperation>
                        {
                            [OperationType.Get] = new OpenApiOperation()
                        }
                    }
                }
            };

            var apiDescription = new ApiDescription
            {
                RelativePath = "test",
                ActionDescriptor = new ControllerActionDescriptor
                {
                    ControllerTypeInfo = typeof(TestController).GetTypeInfo(),
                    EndpointMetadata = new List<object> { new FeatureGateAttribute("TestFeature") }
                }
            };

            var apiDescriptions = new List<ApiDescription> { apiDescription };
            var schemaRepository = new SchemaRepository();
            var schemaGeneratorMock = new Mock<ISchemaGenerator>();
            var context = new DocumentFilterContext(apiDescriptions, schemaGeneratorMock.Object, schemaRepository);

            _featureManagerMock.Setup(x => x.IsEnabledAsync("TestFeature")).ReturnsAsync(true);

            // Act
            _filter.Apply(swaggerDoc, context);

            // Assert
            using (new FluentAssertions.Execution.AssertionScope())
            {
                swaggerDoc.Paths.Should().ContainKey("/test");
                _output.ToString().Should().NotContain("Removing path '/test' from Swagger documentation because the feature gate is disabled.");
            }
        }

        [TestMethod]
        public void Apply_RemovesPaths_WhenControllerFeatureIsDisabled()
        {
            // Arrange
            var swaggerDoc = new OpenApiDocument
            {
                Paths = new OpenApiPaths
                {
                    ["/test"] = new OpenApiPathItem
                    {
                        Operations = new Dictionary<OperationType, OpenApiOperation>
                        {
                            [OperationType.Get] = new OpenApiOperation()
                        }
                    }
                }
            };

            var apiDescription = new ApiDescription
            {
                RelativePath = "test",
                ActionDescriptor = new ControllerActionDescriptor
                {
                    ControllerTypeInfo = typeof(TestControllerWithFeatureGate).GetTypeInfo(),
                    EndpointMetadata = new List<object>()
                }
            };

            var apiDescriptions = new List<ApiDescription> { apiDescription };
            var schemaRepository = new SchemaRepository();
            var schemaGeneratorMock = new Mock<ISchemaGenerator>();
            var context = new DocumentFilterContext(apiDescriptions, schemaGeneratorMock.Object, schemaRepository);

            _featureManagerMock.Setup(x => x.IsEnabledAsync("ControllerFeature")).ReturnsAsync(false);

            // Act
            _filter.Apply(swaggerDoc, context);

            // Assert
            using (new FluentAssertions.Execution.AssertionScope())
            {
                swaggerDoc.Paths.Should().NotContainKey("/test");
                _output.ToString().Should().Contain("Removing path '/test' from Swagger documentation because the feature gate is disabled.");
            }
        }

        [TestMethod]
        public void Apply_DoesNotRemovePaths_WhenNoFeatureGateAttributes()
        {
            // Arrange
            var swaggerDoc = new OpenApiDocument
            {
                Paths = new OpenApiPaths
                {
                    ["/test"] = new OpenApiPathItem
                    {
                        Operations = new Dictionary<OperationType, OpenApiOperation>
                        {
                            [OperationType.Get] = new OpenApiOperation()
                        }
                    }
                }
            };

            var apiDescription = new ApiDescription
            {
                RelativePath = "test",
                ActionDescriptor = new ControllerActionDescriptor
                {
                    ControllerTypeInfo = typeof(TestController).GetTypeInfo(),
                    EndpointMetadata = new List<object>()
                }
            };

            var apiDescriptions = new List<ApiDescription> { apiDescription };
            var schemaRepository = new SchemaRepository();
            var schemaGeneratorMock = new Mock<ISchemaGenerator>();
            var context = new DocumentFilterContext(apiDescriptions, schemaGeneratorMock.Object, schemaRepository);

            // Act
            _filter.Apply(swaggerDoc, context);

            // Assert
            using (new FluentAssertions.Execution.AssertionScope())
            {
                swaggerDoc.Paths.Should().ContainKey("/test");
                _output.ToString().Should().NotContain("Removing path '/test' from Swagger documentation because the feature gate is disabled.");
            }
        }

        [TestMethod]
        public void Apply_DoesNotRemovePaths_WhenAllFeaturesAreEnabled()
        {
            // Arrange
            var swaggerDoc = new OpenApiDocument
            {
                Paths = new OpenApiPaths
                {
                    ["/test"] = new OpenApiPathItem
                    {
                        Operations = new Dictionary<OperationType, OpenApiOperation>
                        {
                            [OperationType.Get] = new OpenApiOperation()
                        }
                    }
                }
            };

            var apiDescription = new ApiDescription
            {
                RelativePath = "test",
                ActionDescriptor = new ControllerActionDescriptor
                {
                    ControllerTypeInfo = typeof(TestControllerWithFeatureGate).GetTypeInfo(),
                    EndpointMetadata = new List<object> { new FeatureGateAttribute("ControllerFeature") }
                }
            };

            var apiDescriptions = new List<ApiDescription> { apiDescription };
            var schemaRepository = new SchemaRepository();
            var schemaGeneratorMock = new Mock<ISchemaGenerator>();
            var context = new DocumentFilterContext(apiDescriptions, schemaGeneratorMock.Object, schemaRepository);

            _featureManagerMock.Setup(x => x.IsEnabledAsync("ControllerFeature")).ReturnsAsync(true);

            // Act
            _filter.Apply(swaggerDoc, context);

            // Assert
            using (new FluentAssertions.Execution.AssertionScope())
            {
                swaggerDoc.Paths.Should().ContainKey("/test");
                _output.ToString().Should().NotContain("Removing path '/test' from Swagger documentation because the feature gate is disabled.");
            }
        }

        private class TestController : ControllerBase
        {
        }

        [ExcludeFromCodeCoverage]
        [FeatureGate("ControllerFeature")]
        private class TestControllerWithFeatureGate : ControllerBase
        {
        }
    }
}
