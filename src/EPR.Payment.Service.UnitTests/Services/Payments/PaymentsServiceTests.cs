using AutoFixture;
using AutoMapper;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Payments;
using EPR.Payment.Service.Services.Interfaces.Payments;
using EPR.Payment.Service.Services.Payments;
using FluentAssertions.Execution;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Dtos.Response.Payments;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;

namespace EPR.Payment.Service.UnitTests.Services.Payments
{
    [TestClass]
    public class PaymentsServiceTests
    {
        private Mock<IPaymentsRepository> _paymentsRepositoryMock = null!;
        private PaymentsService? _service = null!;

        private CancellationToken _cancellationToken;

        [TestInitialize]
        public void TestInitialize()
        {
            _paymentsRepositoryMock = new Mock<IPaymentsRepository>();
            _cancellationToken = new CancellationToken();
            _service = new PaymentsService(_paymentsRepositoryMock.Object);
        }

        [TestMethod]
        public void Constructor_WhenAllDependenciesAreNotNull_ShouldInitialize()
        {
            // Act
            var service = new PaymentsService(_paymentsRepositoryMock.Object);

            // Assert
            using (new AssertionScope())
            {
                service.Should().NotBeNull();
                service.Should().BeAssignableTo<IPaymentsService>();
            }
        }

        [TestMethod]
        public void Constructor_WhenPaymentsRepositoryIsNull_ShouldThrowArgumentNullException()
        {
            // Act
            IPaymentsRepository? paymentsRepository = null;

            Action act = () => new PaymentsService(paymentsRepository!);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'paymentsRepository')");
        }

        [TestMethod, AutoMoqData]
        public async Task GetPreviousPaymentsByReferenceAsync_RepositoryReturnsAResult_ShouldReturnPreviousPayments(
            [Frozen] decimal expectedPreviousPayments,
            string reference
            )
        {
            //Arrange
            _paymentsRepositoryMock.Setup(i => i.GetPreviousPaymentsByReferenceAsync(reference, _cancellationToken)).ReturnsAsync(expectedPreviousPayments);

            //Act
            var result = await _service!.GetPreviousPaymentsByReferenceAsync(reference, _cancellationToken);

            //Assert
            result.Should().Be(expectedPreviousPayments);
        }
    }
}
