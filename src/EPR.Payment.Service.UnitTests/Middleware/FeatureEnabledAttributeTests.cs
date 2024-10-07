using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Helper;
using FluentAssertions;
using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.UnitTests.Middleware
{
    [TestClass]
    public class FeatureEnabledAttributeTests
    {
        [TestMethod, AutoMoqData]
        public void Constructor_SetsFeatureNameProperty()
        {
            // Arrange
            var featureName = "TestFeature";

            // Act
            var attribute = new FeatureEnabledAttribute(featureName);

            // Assert
            attribute.FeatureName.Should().Be(featureName);
        }

        [TestMethod, AutoMoqData]
        public void AttributeUsage_ValidTargets()
        {
            // Arrange & Act
            var attributeUsage = Attribute.GetCustomAttribute(
                typeof(FeatureEnabledAttribute), typeof(AttributeUsageAttribute)
            ) as AttributeUsageAttribute;

            // Assert
            using (new FluentAssertions.Execution.AssertionScope())
            {
                attributeUsage.Should().NotBeNull();
                attributeUsage!.ValidOn.Should().Be(AttributeTargets.Class | AttributeTargets.Method);
                attributeUsage.AllowMultiple.Should().BeFalse();
            }
        }

        [TestMethod, AutoMoqData]
        public void FeatureEnabledAttribute_CanBeUsedOnClass()
        {
            // Arrange
            var type = typeof(ClassWithFeatureEnabledAttribute);

            // Act
            var attribute = Attribute.GetCustomAttribute(type, typeof(FeatureEnabledAttribute)) as FeatureEnabledAttribute;

            // Assert
            using (new FluentAssertions.Execution.AssertionScope())
            {
                attribute.Should().NotBeNull();
                attribute!.FeatureName.Should().Be("ClassFeature");
            }
        }

        [TestMethod, AutoMoqData]
        public void FeatureEnabledAttribute_CanBeUsedOnMethod()
        {
            // Arrange
            var method = typeof(ClassWithFeatureEnabledAttribute).GetMethod(nameof(ClassWithFeatureEnabledAttribute.MethodWithFeatureEnabledAttribute));

            // Act
            var attribute = Attribute.GetCustomAttribute(method!, typeof(FeatureEnabledAttribute)) as FeatureEnabledAttribute;

            // Assert
            using (new FluentAssertions.Execution.AssertionScope())
            {
                attribute.Should().NotBeNull();
                attribute!.FeatureName.Should().Be("MethodFeature");
            }
        }

        [ExcludeFromCodeCoverage]
        [FeatureEnabled("ClassFeature")]
        private class ClassWithFeatureEnabledAttribute
        {
            [FeatureEnabled("MethodFeature")]
            public static void MethodWithFeatureEnabledAttribute() { }
        }
    }
}
