using EPR.Payment.Service.Common.Extensions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EPR.Payment.Service.Common.UnitTests.Extensions
{
    [TestClass]
    public class EnumExtensionsTests
    {
        [TestMethod]
        public void GetDescription_ShouldReturnDescription_WhenDescriptionAttributeExists()
        {
            // Arrange
            var enumValue = Test.FirstValue;

            // Act
            var description = enumValue.GetDescription();

            // Assert
            description.Should().Be("First Value Description");
        }

        [TestMethod]
        public void GetDescription_ShouldReturnEnumName_WhenDescriptionAttributeDoesNotExist()
        {
            // Arrange
            var enumValue = Test.NoDescriptionValue;

            // Act
            var description = enumValue.GetDescription();

            // Assert
            description.Should().Be("NoDescriptionValue");
        }

        [TestMethod]
        public void GetDescription_ShouldReturnDescription_ForAnotherEnumValue()
        {
            // Arrange
            var enumValue = Test.SecondValue;

            // Act
            var description = enumValue.GetDescription();

            // Assert
            description.Should().Be("Second Value Description");
        }
    }
}
