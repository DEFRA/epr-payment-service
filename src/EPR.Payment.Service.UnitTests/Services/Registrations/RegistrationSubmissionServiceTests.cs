using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Registrations;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Services.Interfaces.Registrations;
using EPR.Payment.Service.Services.Registrations;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;

namespace EPR.Payment.Service.UnitTests.Services.Registrations
{
    [TestClass]
    public class RegistrationSubmissionServiceTests
    {
        private Mock<IRegistrationSubmissionRepository> _repositoryMock = null!;
        private RegistrationSubmissionService _service = null!;
        private CancellationToken _cancellationToken;

        [TestInitialize]
        public void TestInitialize()
        {
            _repositoryMock = new Mock<IRegistrationSubmissionRepository>();
            _cancellationToken = new CancellationToken();
            _service = new RegistrationSubmissionService(_repositoryMock.Object);
        }

        [TestMethod]
        public void Constructor_WhenRepositoryIsNotNull_ShouldInitialise()
        {
            // Act
            var service = new RegistrationSubmissionService(_repositoryMock.Object);

            // Assert
            using (new AssertionScope())
            {
                service.Should().NotBeNull();
                service.Should().BeAssignableTo<IRegistrationSubmissionService>();
            }
        }

        [TestMethod]
        public void Constructor_WhenRepositoryIsNull_ShouldThrowArgumentNullException()
        {
            // Act
            Action act = () => new RegistrationSubmissionService(null!);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'repository')");
        }

        [TestMethod, AutoMoqData]
        public async Task SubmissionExistsAsync_RepositoryReturnsTrue_ShouldReturnTrue(Guid submissionId)
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.SubmissionExistsAsync(submissionId, _cancellationToken))
                .ReturnsAsync(true);

            // Act
            var result = await _service.SubmissionExistsAsync(submissionId, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                _repositoryMock.Verify(r => r.SubmissionExistsAsync(submissionId, _cancellationToken), Times.Once);
            }
        }

        [TestMethod, AutoMoqData]
        public async Task SubmissionExistsAsync_RepositoryReturnsFalse_ShouldReturnFalse(Guid submissionId)
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.SubmissionExistsAsync(submissionId, _cancellationToken))
                .ReturnsAsync(false);

            // Act
            var result = await _service.SubmissionExistsAsync(submissionId, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeFalse();
                _repositoryMock.Verify(r => r.SubmissionExistsAsync(submissionId, _cancellationToken), Times.Once);
            }
        }
    }
}
