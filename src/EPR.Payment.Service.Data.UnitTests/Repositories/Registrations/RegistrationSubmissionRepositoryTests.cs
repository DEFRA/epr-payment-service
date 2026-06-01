using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Data.DataModels;
using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.Data.Repositories.Registrations;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.EntityFrameworkCore;

namespace EPR.Payment.Service.Data.UnitTests.Repositories.Registrations
{
    [TestClass]
    public class RegistrationSubmissionRepositoryTests
    {
        [TestMethod]
        public void Constructor_WhenDataContextIsNull_ShouldThrowArgumentNullException()
        {
            // Act
            Action act = () => new RegistrationSubmissionRepository(null!);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'dataContext')");
        }

        [TestMethod, AutoMoqData]
        public async Task SubmissionExistsAsync_WhenSubmissionExists_ShouldReturnTrue(
            [Frozen] Mock<IAppDbContext> dbMock)
        {
            // Arrange
            var submissionId = Guid.NewGuid();

            var data = new List<RegistrationSubmissionData>
            {
                new() { Id = Guid.NewGuid(), SubmissionId = submissionId }
            };

            dbMock.Setup(d => d.RegistrationSubmissionData).ReturnsDbSet(data);

            var repository = new RegistrationSubmissionRepository(dbMock.Object);

            // Act
            var result = await repository.SubmissionExistsAsync(submissionId, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod, AutoMoqData]
        public async Task SubmissionExistsAsync_WhenSubmissionDoesNotExist_ShouldReturnFalse(
            [Frozen] Mock<IAppDbContext> dbMock)
        {
            // Arrange
            var submissionId = Guid.NewGuid();

            dbMock.Setup(d => d.RegistrationSubmissionData).ReturnsDbSet(new List<RegistrationSubmissionData>());

            var repository = new RegistrationSubmissionRepository(dbMock.Object);

            // Act
            var result = await repository.SubmissionExistsAsync(submissionId, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeFalse();
            }
        }
    }
}
