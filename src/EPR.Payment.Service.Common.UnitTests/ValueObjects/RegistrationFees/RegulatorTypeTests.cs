using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EPR.Payment.Service.Common.UnitTests.ValueObjects.RegistrationFees
{
    [TestClass]
    public class RegulatorTypeTests
    {
        [TestMethod]
        public void Create_ValidRegulator_ShouldReturnRegulatorType()
        {
            // Act
            var regulator = RegulatorType.Create("GB-ENG");

            // Assert
            regulator.Should().NotBeNull();
            regulator.Value.Should().Be("GB-ENG");
        }

        [TestMethod]
        public void Create_InvalidRegulator_ShouldThrowArgumentException()
        {
            // Act
            Action act = () => RegulatorType.Create("Invalid-Regulator");

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Invalid regulator type: Invalid-Regulator.*");
        }

        [TestMethod]
        public void Create_EmptyRegulator_ShouldThrowArgumentException()
        {
            // Act
            Action act = () => RegulatorType.Create(string.Empty);

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Regulator cannot be null or empty.*");
        }

        [TestMethod]
        public void Create_NullRegulator_ShouldThrowArgumentException()
        {
            // Act
            Action act = () => RegulatorType.Create(null);

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Regulator cannot be null or empty.*");
        }

        [TestMethod]
        public void Equals_SameValue_ShouldReturnTrue()
        {
            // Arrange
            var regulator1 = RegulatorType.Create("GB-ENG");
            var regulator2 = RegulatorType.Create("GB-ENG");

            // Act & Assert
            regulator1.Equals(regulator2).Should().BeTrue();
            regulator1.Should().Be(regulator2);
        }

        [TestMethod]
        public void Equals_DifferentValue_ShouldReturnFalse()
        {
            // Arrange
            var regulator1 = RegulatorType.Create("GB-ENG");
            var regulator2 = RegulatorType.Create("GB-SCT");

            // Act & Assert
            regulator1.Equals(regulator2).Should().BeFalse();
            regulator1.Should().NotBe(regulator2);
        }

        [TestMethod]
        public void GetHashCode_SameValue_ShouldReturnSameHashCode()
        {
            // Arrange
            var regulator1 = RegulatorType.Create("GB-ENG");
            var regulator2 = RegulatorType.Create("GB-ENG");

            // Act & Assert
            regulator1.GetHashCode().Should().Be(regulator2.GetHashCode());
        }

        [TestMethod]
        public void GetHashCode_DifferentValue_ShouldReturnDifferentHashCode()
        {
            // Arrange
            var regulator1 = RegulatorType.Create("GB-ENG");
            var regulator2 = RegulatorType.Create("GB-SCT");

            // Act & Assert
            regulator1.GetHashCode().Should().NotBe(regulator2.GetHashCode());
        }

        [TestMethod]
        public void ToString_ShouldReturnValue()
        {
            // Arrange
            var regulator = RegulatorType.Create("GB-ENG");

            // Act
            var result = regulator.ToString();

            // Assert
            result.Should().Be("GB-ENG");
        }

        [TestMethod]
        public void PredefinedProperties_ShouldReturnCorrectValues()
        {
            // Assert
            RegulatorType.GBEng.Value.Should().Be("GB-ENG");
            RegulatorType.GBSct.Value.Should().Be("GB-SCT");
            RegulatorType.GBWls.Value.Should().Be("GB-WLS");
            RegulatorType.GBNir.Value.Should().Be("GB-NIR");
        }
    }
}
