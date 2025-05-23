﻿using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Dtos.Request.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Services.Interfaces.Payments;
using EPR.Payment.Service.Services.Interfaces.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Services.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Strategies.Interfaces.ResubmissionFees.ComplianceScheme;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;

namespace EPR.Payment.Service.UnitTests.Services.ResubmissionFees.ComplianceScheme
{
    [TestClass]
    public class ComplianceSchemeResubmissionServiceTests
    {
        private IFixture _fixture = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
        }

        [TestMethod, AutoMoqData]
        public void Constructor_WhenResubmissionFeeStrategyIsNull_ShouldThrowArgumentNullException([Frozen] Mock<IPaymentsService> paymentsServiceMock)
        {
            // Arrange
            IComplianceSchemeResubmissionStrategy<ComplianceSchemeResubmissionFeeRequestDto, decimal>? nullStrategy = null;

            // Act
            Action act = () => new ComplianceSchemeResubmissionService(nullStrategy!, paymentsServiceMock.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'resubmissionFeeStrategy')");
        }

        [TestMethod]
        public void Constructor_WhenPaymentsServiceIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var resubmissionFeeStrategyMock = _fixture.Create<Mock<IComplianceSchemeResubmissionStrategy<ComplianceSchemeResubmissionFeeRequestDto, decimal>>>();
            IPaymentsService? paymentsService = null;

            // Act
            Action act = () => new ComplianceSchemeResubmissionService(resubmissionFeeStrategyMock.Object, paymentsService!);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'paymentsService')");
        }

        [TestMethod, AutoMoqData]
        public void Constructor_WhenResubmissionFeeStrategyIsNotNull_ShouldInitializeService([Frozen] Mock<IPaymentsService> paymentsServiceMock)
        {
            // Arrange
            var resubmissionFeeStrategyMock = _fixture.Create<Mock<IComplianceSchemeResubmissionStrategy<ComplianceSchemeResubmissionFeeRequestDto, decimal>>>();

            // Act
            var service = new ComplianceSchemeResubmissionService(resubmissionFeeStrategyMock.Object, paymentsServiceMock.Object);

            // Assert
            using (new AssertionScope())
            {
                service.Should().NotBeNull();
                service.Should().BeAssignableTo<IComplianceSchemeResubmissionService>();
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateResubmissionFeeAsync_ValidRequest_ShouldReturnResubmissionFeeResult(
            [Frozen] Mock<IComplianceSchemeResubmissionStrategy<ComplianceSchemeResubmissionFeeRequestDto, decimal>> strategyMock,
            [Frozen] Mock<IPaymentsService> paymentsServiceMock,
            [Greedy] ComplianceSchemeResubmissionService service,
            [Frozen] decimal baseFee, 
            [Frozen] decimal previousPayments)
        {
            // Arrange
            var request = new ComplianceSchemeResubmissionFeeRequestDto
            {
                Regulator = "GB-ENG",
                MemberCount = 5,
                ReferenceNumber = "REF12345" // Set the required ReferenceNumber
            };

            strategyMock.Setup(x => x.CalculateFeeAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(baseFee);
            paymentsServiceMock.Setup(x => x.GetPreviousPaymentsByReferenceAsync(request.ReferenceNumber, It.IsAny<CancellationToken>())).ReturnsAsync(previousPayments);

            // Act
            var result = await service.CalculateResubmissionFeeAsync(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                var expectedTotalFee = baseFee * request.MemberCount;
                result.TotalResubmissionFee.Should().Be(expectedTotalFee);
                result.PreviousPayments.Should().Be(previousPayments);
                // OutstandingPayment should be equal to totalFee - previousPayments
                var outstandingPayment = expectedTotalFee - previousPayments;
                result.OutstandingPayment.Should().Be(outstandingPayment);
                result.MemberCount.Should().Be(request.MemberCount);
            }            
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateResubmissionFeeAsync_MemberCountLessThan1_ShouldThrowArgumentException(
            [Greedy] ComplianceSchemeResubmissionService service)
        {
            // Arrange
            var request = new ComplianceSchemeResubmissionFeeRequestDto
            {
                Regulator = "GB-ENG",
                MemberCount = 0,
                ReferenceNumber = "REF12345" // Set the required ReferenceNumber
            };

            // Act
            Func<Task> act = async () => await service.CalculateResubmissionFeeAsync(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage(string.Format(ComplianceSchemeFeeCalculationExceptions.InvalidMemberCountError, request.MemberCount));
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateResubmissionFeeAsync_CalculationStrategyThrowsException_ShouldThrowException(
            [Frozen] Mock<IComplianceSchemeResubmissionStrategy<ComplianceSchemeResubmissionFeeRequestDto, decimal>> strategyMock,
            [Greedy] ComplianceSchemeResubmissionService service)
        {
            // Arrange
            var request = new ComplianceSchemeResubmissionFeeRequestDto
            {
                Regulator = "GB-ENG",
                MemberCount = 5,
                ReferenceNumber = "REF12345" // Set the required ReferenceNumber
            };

            strategyMock.Setup(x => x.CalculateFeeAsync(request, It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Test Exception"));

            // Act
            Func<Task> act = async () => await service.CalculateResubmissionFeeAsync(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Test Exception");
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateResubmissionFeeAsync_ValidRequest_ShouldCorrectlyCalculateFeeComponents(
            [Frozen] Mock<IComplianceSchemeResubmissionStrategy<ComplianceSchemeResubmissionFeeRequestDto, decimal>> strategyMock,
            [Frozen] Mock<IPaymentsService> paymentsServiceMock,
            [Greedy] ComplianceSchemeResubmissionService service,
            [Frozen] decimal baseFee, 
            [Frozen] decimal previousPayments)
        {
            // Arrange
            var request = new ComplianceSchemeResubmissionFeeRequestDto
            {
                Regulator = "GB-ENG",
                MemberCount = 3, // Set a valid MemberCount to test the calculations
                ReferenceNumber = "REF12345" // Set the required ReferenceNumber
            };

            strategyMock.Setup(x => x.CalculateFeeAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(baseFee);
            paymentsServiceMock.Setup(x => x.GetPreviousPaymentsByReferenceAsync(request.ReferenceNumber, It.IsAny<CancellationToken>())).ReturnsAsync(previousPayments);

            // Act
            var result = await service.CalculateResubmissionFeeAsync(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();

                // Ensure that baseFee * MemberCount is correctly calculated as totalFee
                var expectedTotalFee = baseFee * request.MemberCount;
                result.TotalResubmissionFee.Should().Be(expectedTotalFee);

                result.PreviousPayments.Should().Be(previousPayments);

                // OutstandingPayment should be equal to totalFee - previousPayments
                var outstandingPayment = expectedTotalFee - previousPayments;
                result.OutstandingPayment.Should().Be(outstandingPayment);
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateResubmissionFeeAsync_WithNoPreviousPayment_ShouldCorrectlyCalculateFeeComponents(
            [Frozen] Mock<IComplianceSchemeResubmissionStrategy<ComplianceSchemeResubmissionFeeRequestDto, decimal>> strategyMock,
            [Frozen] Mock<IPaymentsService> paymentsServiceMock,
            [Greedy] ComplianceSchemeResubmissionService service)
        {
            // Arrange
            var request = new ComplianceSchemeResubmissionFeeRequestDto
            {
                Regulator = "GB-ENG",
                MemberCount = 1, // Set a valid MemberCount to test the calculations
                ReferenceNumber = "REF12345" // Set the required ReferenceNumber
            };

            decimal baseFee = 10000m;
            decimal previousPayments = 0m;

            strategyMock.Setup(x => x.CalculateFeeAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(baseFee);
            paymentsServiceMock.Setup(x => x.GetPreviousPaymentsByReferenceAsync(request.ReferenceNumber, It.IsAny<CancellationToken>())).ReturnsAsync(previousPayments);

            // Act
            var result = await service.CalculateResubmissionFeeAsync(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();

                // Ensure that baseFee * MemberCount is correctly calculated as totalFee
                var expectedTotalFee = baseFee * request.MemberCount;
                result.TotalResubmissionFee.Should().Be(expectedTotalFee);

                result.PreviousPayments.Should().Be(previousPayments);

                // OutstandingPayment should be equal to totalFee - previousPayments
                var outstandingPayment = expectedTotalFee - previousPayments;
                result.OutstandingPayment.Should().Be(outstandingPayment);
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateResubmissionFeeAsync_WithPartialPreviousPayment_ShouldCorrectlyCalculateFeeComponents(
            [Frozen] Mock<IComplianceSchemeResubmissionStrategy<ComplianceSchemeResubmissionFeeRequestDto, decimal>> strategyMock,
            [Frozen] Mock<IPaymentsService> paymentsServiceMock,
            [Greedy] ComplianceSchemeResubmissionService service)
        {
            // Arrange
            var request = new ComplianceSchemeResubmissionFeeRequestDto
            {
                Regulator = "GB-ENG",
                MemberCount = 1, // Set a valid MemberCount to test the calculations
                ReferenceNumber = "REF12345" // Set the required ReferenceNumber
            };

            decimal baseFee = 10000m;
            decimal previousPayments = 5000m;

            strategyMock.Setup(x => x.CalculateFeeAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(baseFee);
            paymentsServiceMock.Setup(x => x.GetPreviousPaymentsByReferenceAsync(request.ReferenceNumber, It.IsAny<CancellationToken>())).ReturnsAsync(previousPayments);

            // Act
            var result = await service.CalculateResubmissionFeeAsync(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();

                // Ensure that baseFee * MemberCount is correctly calculated as totalFee
                var expectedTotalFee = baseFee * request.MemberCount;
                result.TotalResubmissionFee.Should().Be(expectedTotalFee);

                result.PreviousPayments.Should().Be(previousPayments);

                // OutstandingPayment should be equal to totalFee - previousPayments
                var outstandingPayment = expectedTotalFee - previousPayments;
                result.OutstandingPayment.Should().Be(outstandingPayment);
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateResubmissionFeeAsync_WithFullPreviousPayment_ShouldCorrectlyCalculateFeeComponents(
            [Frozen] Mock<IComplianceSchemeResubmissionStrategy<ComplianceSchemeResubmissionFeeRequestDto, decimal>> strategyMock,
            [Frozen] Mock<IPaymentsService> paymentsServiceMock,
            [Greedy] ComplianceSchemeResubmissionService service)
        {
            // Arrange
            var request = new ComplianceSchemeResubmissionFeeRequestDto
            {
                Regulator = "GB-ENG",
                MemberCount = 1, // Set a valid MemberCount to test the calculations
                ReferenceNumber = "REF12345" // Set the required ReferenceNumber
            };

            decimal baseFee = 10000m;
            decimal previousPayments = 10000m;

            strategyMock.Setup(x => x.CalculateFeeAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(baseFee);
            paymentsServiceMock.Setup(x => x.GetPreviousPaymentsByReferenceAsync(request.ReferenceNumber, It.IsAny<CancellationToken>())).ReturnsAsync(previousPayments);

            // Act
            var result = await service.CalculateResubmissionFeeAsync(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();

                // Ensure that baseFee * MemberCount is correctly calculated as totalFee
                var expectedTotalFee = baseFee * request.MemberCount;
                result.TotalResubmissionFee.Should().Be(expectedTotalFee);

                result.PreviousPayments.Should().Be(previousPayments);

                // OutstandingPayment should be equal to totalFee - previousPayments
                var outstandingPayment = expectedTotalFee - previousPayments;
                result.OutstandingPayment.Should().Be(outstandingPayment);
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateResubmissionFeeAsync_WithOverPreviousPayment_ShouldCorrectlyCalculateFeeComponents(
            [Frozen] Mock<IComplianceSchemeResubmissionStrategy<ComplianceSchemeResubmissionFeeRequestDto, decimal>> strategyMock,
            [Frozen] Mock<IPaymentsService> paymentsServiceMock,
            [Greedy] ComplianceSchemeResubmissionService service)
        {
            // Arrange
            var request = new ComplianceSchemeResubmissionFeeRequestDto
            {
                Regulator = "GB-ENG",
                MemberCount = 1, // Set a valid MemberCount to test the calculations
                ReferenceNumber = "REF12345" // Set the required ReferenceNumber
            };

            decimal baseFee = 10000m;
            decimal previousPayments = 15000m;

            strategyMock.Setup(x => x.CalculateFeeAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(baseFee);
            paymentsServiceMock.Setup(x => x.GetPreviousPaymentsByReferenceAsync(request.ReferenceNumber, It.IsAny<CancellationToken>())).ReturnsAsync(previousPayments);

            // Act
            var result = await service.CalculateResubmissionFeeAsync(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();

                // Ensure that baseFee * MemberCount is correctly calculated as totalFee
                var expectedTotalFee = baseFee * request.MemberCount;
                result.TotalResubmissionFee.Should().Be(expectedTotalFee);

                result.PreviousPayments.Should().Be(previousPayments);

                // OutstandingPayment should be equal to totalFee - previousPayments
                var outstandingPayment = expectedTotalFee - previousPayments;
                result.OutstandingPayment.Should().Be(-5000M);
            }
        }
    }
}
