using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees;
using EPR.Payment.Service.Services.Interfaces.Payments;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Services.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Strategies.RegistrationFees;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;

namespace EPR.Payment.Service.UnitTests.Services.RegistrationFees.ComplianceScheme
{
    [TestClass]
    public class ComplianceSchemeCalculatorServiceTests
    {
        private Mock<ICSBaseFeeCalculationStrategy<ComplianceSchemeFeesRequestDto, decimal>> _baseFeeCalculationStrategyMock = null!;
        private Mock<ICSOnlineMarketCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto, decimal>> _complianceSchemeOnlineMarketStrategyMock = null!;
        private Mock<ICSLateFeeCalculationStrategy<ComplianceSchemeLateFeeRequestDto, decimal>> _complianceSchemeLateFeeStrategyMock = null!;
        private Mock<ICSMemberFeeCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto, decimal>> _complianceSchemeMemberStrategyMock = null!;
        private Mock<IBaseSubsidiariesFeeCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto, SubsidiariesFeeBreakdown>> _subsidiariesFeeCalculationStrategyMock = null!;
        private Mock<IPaymentsService> _paymentsServiceMock = null!;
        private ComplianceSchemeCalculatorService _service = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _baseFeeCalculationStrategyMock = new Mock<ICSBaseFeeCalculationStrategy<ComplianceSchemeFeesRequestDto, decimal>>();
            _complianceSchemeOnlineMarketStrategyMock = new Mock<ICSOnlineMarketCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto, decimal>>();
            _complianceSchemeLateFeeStrategyMock = new Mock<ICSLateFeeCalculationStrategy<ComplianceSchemeLateFeeRequestDto, decimal>>();
            _complianceSchemeMemberStrategyMock = new Mock<ICSMemberFeeCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto, decimal>>();
            _subsidiariesFeeCalculationStrategyMock = new Mock<IBaseSubsidiariesFeeCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto, SubsidiariesFeeBreakdown>>();
            _paymentsServiceMock = new Mock<IPaymentsService>();
            _service = new ComplianceSchemeCalculatorService(
                _baseFeeCalculationStrategyMock.Object,
                _complianceSchemeOnlineMarketStrategyMock.Object, 
                _complianceSchemeLateFeeStrategyMock.Object, 
                _complianceSchemeMemberStrategyMock.Object, 
                _subsidiariesFeeCalculationStrategyMock.Object,
                _paymentsServiceMock.Object);
        }

        [TestMethod]
        public void Constructor_WhenBaseFeeCalculationStrategyIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            ICSBaseFeeCalculationStrategy<ComplianceSchemeFeesRequestDto, decimal>? baseFeeCalculationStrategy = null;

            // Act
            var act = () => new ComplianceSchemeCalculatorService(
                baseFeeCalculationStrategy!,
                _complianceSchemeOnlineMarketStrategyMock.Object,
                _complianceSchemeLateFeeStrategyMock.Object,
                _complianceSchemeMemberStrategyMock.Object,
                _subsidiariesFeeCalculationStrategyMock.Object,
                _paymentsServiceMock.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'baseFeeCalculationStrategy')");
        }

        [TestMethod]
        public void Constructor_WhenComplianceSchemeOnlineMarketStrategyIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            ICSOnlineMarketCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto, decimal>? complianceSchemeOnlineMarketStrategy = null;

            // Act
            Action act = () =>
            {
                var unused = new ComplianceSchemeCalculatorService(
                    _baseFeeCalculationStrategyMock.Object,
                    complianceSchemeOnlineMarketStrategy!,
                    _complianceSchemeLateFeeStrategyMock.Object,
                    _complianceSchemeMemberStrategyMock.Object,
                    _subsidiariesFeeCalculationStrategyMock.Object,
                    _paymentsServiceMock.Object);
            };

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'complianceSchemeOnlineMarketStrategy')");
        }

        [TestMethod]
        public void Constructor_WhenComplianceSchemeLateFeeStrategyIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            ICSLateFeeCalculationStrategy<ComplianceSchemeLateFeeRequestDto, decimal>? complianceSchemeLateFeeStrategy = null;

            // Act
            Action act = () =>
            {
                var unused = new ComplianceSchemeCalculatorService(
                    _baseFeeCalculationStrategyMock.Object,
                    _complianceSchemeOnlineMarketStrategyMock.Object,
                    complianceSchemeLateFeeStrategy!,
                    _complianceSchemeMemberStrategyMock.Object,
                    _subsidiariesFeeCalculationStrategyMock.Object,
                    _paymentsServiceMock.Object);
            };

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'complianceSchemeLateFeeStrategy')");
        }

        [TestMethod]
        public void Constructor_WhenComplianceSchemeMemberStrategyIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            ICSMemberFeeCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto, decimal>? complianceSchemeMemberStrategy = null;

            // Act
            Action act = () =>
            {
                var unused = new ComplianceSchemeCalculatorService(
                    _baseFeeCalculationStrategyMock.Object,
                    _complianceSchemeOnlineMarketStrategyMock.Object,
                    _complianceSchemeLateFeeStrategyMock.Object,
                    complianceSchemeMemberStrategy!,
                    _subsidiariesFeeCalculationStrategyMock.Object,
                    _paymentsServiceMock.Object);
            };

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'complianceSchemeMemberStrategy')");
        }

        [TestMethod]
        public void Constructor_WhenSubsidiariesFeeCalculationStrategyIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            BaseSubsidiariesFeeCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto>? subsidiariesFeeCalculationStrategy = null;

            // Act
            Action act = () =>
            {
                var unused = new ComplianceSchemeCalculatorService(
                    _baseFeeCalculationStrategyMock.Object,
                    _complianceSchemeOnlineMarketStrategyMock.Object,
                    _complianceSchemeLateFeeStrategyMock.Object,
                    _complianceSchemeMemberStrategyMock.Object,
                    subsidiariesFeeCalculationStrategy!,
                    _paymentsServiceMock.Object);
            };

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'subsidiariesFeeCalculationStrategy')");
        }


        [TestMethod]
        public void Constructor_WhenPaymentServiceIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            IPaymentsService? paymentsService = null;

            // Act
            Action act = () =>
            {
                var unused = new ComplianceSchemeCalculatorService(
                    _baseFeeCalculationStrategyMock.Object,
                    _complianceSchemeOnlineMarketStrategyMock.Object,
                    _complianceSchemeLateFeeStrategyMock.Object,
                    _complianceSchemeMemberStrategyMock.Object,
                    _subsidiariesFeeCalculationStrategyMock.Object,
                    paymentsService!);
            };

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'paymentsService')");
        }

        [TestMethod]
        public void Constructor_WhenAllDependenciesAreNotNull_ShouldInitializeComplianceSchemeBaseFeeService()
        {
            // Act
            var service = new ComplianceSchemeCalculatorService(
                _baseFeeCalculationStrategyMock.Object,
                _complianceSchemeOnlineMarketStrategyMock.Object, 
                _complianceSchemeLateFeeStrategyMock.Object, 
                _complianceSchemeMemberStrategyMock.Object, 
                _subsidiariesFeeCalculationStrategyMock.Object,
                _paymentsServiceMock.Object);

            // Assert
            using (new AssertionScope())
            {
                service.Should().NotBeNull();
                service.Should().BeAssignableTo<IComplianceSchemeCalculatorService>();
            }
        }

        [TestMethod]
        public async Task CalculateFeesAsync_WithSmallMemberTypeAnd5Subsidiaries_ReturnsCorrectFees()
        {
            // Arrange
            var request = new ComplianceSchemeFeesRequestDto
            {
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "ABC123",
                SubmissionDate = DateTime.UtcNow,
                ComplianceSchemeMembers = new List<ComplianceSchemeMemberDto>
            {
                new ComplianceSchemeMemberDto
                {
                    MemberId = "12345",
                    MemberType = "Small",
                    IsOnlineMarketplace = false,
                    IsLateFeeApplicable = false,
                    NumberOfSubsidiaries = 5,
                    NoOfSubsidiariesOnlineMarketplace = 0
                }
            }
            };

            _baseFeeCalculationStrategyMock
                .Setup(s => s.CalculateFeeAsync(It.IsAny<ComplianceSchemeFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1380400);

            _complianceSchemeMemberStrategyMock
                .Setup(s => s.CalculateFeeAsync(It.IsAny<ComplianceSchemeMemberWithRegulatorDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(63100);

            _complianceSchemeOnlineMarketStrategyMock
                .Setup(s => s.CalculateFeeAsync(It.IsAny<ComplianceSchemeMemberWithRegulatorDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            _subsidiariesFeeCalculationStrategyMock
                .Setup(s => s.CalculateFeeAsync(It.IsAny<ComplianceSchemeMemberWithRegulatorDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SubsidiariesFeeBreakdown
                {
                    TotalSubsidiariesOMPFees = 0,
                    FeeBreakdowns = new List<FeeBreakdown>
                    {
                        new FeeBreakdown { TotalPrice = 279000 },
                        new FeeBreakdown { TotalPrice = 0 },
                        new FeeBreakdown { TotalPrice = 0 }
                    }
                });

            _paymentsServiceMock.Setup(s => s.GetPreviousPaymentsByReferenceAsync(request.ApplicationReferenceNumber, It.IsAny<CancellationToken>()))
                .ReturnsAsync(100M);

             // Act
             var result = await _service.CalculateFeesAsync(request, It.IsAny<CancellationToken>());

            // Assert
            using (new AssertionScope())
            {
                result.ComplianceSchemeRegistrationFee.Should().Be(1380400M);
                result.ComplianceSchemeMembersWithFees.Should().HaveCount(1);
                var member = result.ComplianceSchemeMembersWithFees.First();
                member.MemberRegistrationFee.Should().Be(63100M);
                member.MemberOnlineMarketPlaceFee.Should().Be(0);
                member.MemberLateRegistrationFee.Should().Be(0);
                member.SubsidiariesFee.Should().Be(279000M);
                member.TotalMemberFee.Should().Be(342100M);

                result.TotalFee.Should().Be(1722500M);
                result.PreviousPayment.Should().Be(100M);
                result.OutstandingPayment.Should().Be(1722400M);
            }
        }

        [TestMethod]
        public async Task CalculateFeesAsync_WithLargeMemberTypeAnd105SubsidiariesWithOMP_ReturnsCorrectFees()
        {
            // Arrange
            var request = new ComplianceSchemeFeesRequestDto
            {
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "ABC123",
                SubmissionDate = DateTime.UtcNow,
                ComplianceSchemeMembers = new List<ComplianceSchemeMemberDto>
            {
                new ComplianceSchemeMemberDto
                {
                    MemberId = "12345",
                    MemberType = "Large",
                    IsOnlineMarketplace = true,
                    IsLateFeeApplicable = false,
                    NumberOfSubsidiaries = 105,
                    NoOfSubsidiariesOnlineMarketplace = 0
                }
            }
            };

            _baseFeeCalculationStrategyMock
                .Setup(s => s.CalculateFeeAsync(It.IsAny<ComplianceSchemeFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1380400);

            _complianceSchemeMemberStrategyMock
                .Setup(s => s.CalculateFeeAsync(It.IsAny<ComplianceSchemeMemberWithRegulatorDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(165800);

            _complianceSchemeOnlineMarketStrategyMock
                .Setup(s => s.CalculateFeeAsync(It.IsAny<ComplianceSchemeMemberWithRegulatorDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(257900);

            _subsidiariesFeeCalculationStrategyMock
                .Setup(s => s.CalculateFeeAsync(It.IsAny<ComplianceSchemeMemberWithRegulatorDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SubsidiariesFeeBreakdown
                {
                    TotalSubsidiariesOMPFees = 0,
                    FeeBreakdowns = new List<FeeBreakdown>
                    {
                        new FeeBreakdown { TotalPrice = 1116000 },
                        new FeeBreakdown { TotalPrice = 1120000 },
                        new FeeBreakdown { TotalPrice = 0 }
                    }
                });

            _paymentsServiceMock.Setup(s => s.GetPreviousPaymentsByReferenceAsync(request.ApplicationReferenceNumber, It.IsAny<CancellationToken>()))
                .ReturnsAsync(100M);

            // Act
            var result = await _service.CalculateFeesAsync(request, It.IsAny<CancellationToken>());

            // Assert
            using (new AssertionScope())
            {
                result.ComplianceSchemeRegistrationFee.Should().Be(1380400M);
                result.ComplianceSchemeMembersWithFees.Should().HaveCount(1);
                var member = result.ComplianceSchemeMembersWithFees.First();
                member.MemberRegistrationFee.Should().Be(165800M);
                member.MemberOnlineMarketPlaceFee.Should().Be(257900);
                member.MemberLateRegistrationFee.Should().Be(0);
                member.SubsidiariesFee.Should().Be(2236000M);
                member.TotalMemberFee.Should().Be(2659700M);

                result.TotalFee.Should().Be(4040100M);
                result.PreviousPayment.Should().Be(100M);
                result.OutstandingPayment.Should().Be(4040000M);
            }
        }

        [TestMethod]
        public async Task CalculateFeesAsync_WithTwoMembersOneSmallOneLargeWithMemberOMP_ReturnsCorrectAggregatedFees()
        {
            // Arrange
            var request = new ComplianceSchemeFeesRequestDto
            {
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "ABC123",
                SubmissionDate = DateTime.UtcNow,
                ComplianceSchemeMembers = new List<ComplianceSchemeMemberDto>
                {
                    new ComplianceSchemeMemberDto
                    {
                        MemberId = "12345",
                        MemberType = "Small",
                        IsOnlineMarketplace = false,
                        IsLateFeeApplicable = false,
                        NumberOfSubsidiaries = 5,
                        NoOfSubsidiariesOnlineMarketplace = 0
                    },
                    new ComplianceSchemeMemberDto
                    {
                        MemberId = "67890",
                        MemberType = "Large",
                        IsOnlineMarketplace = true,
                        IsLateFeeApplicable = false,
                        NumberOfSubsidiaries = 105,
                        NoOfSubsidiariesOnlineMarketplace = 0
                    }
                }
            };
            var cancellationToken = CancellationToken.None;

            _baseFeeCalculationStrategyMock
                .Setup(s => s.CalculateFeeAsync(It.IsAny<ComplianceSchemeFeesRequestDto>(), cancellationToken))
                .ReturnsAsync(1380400);

            _complianceSchemeMemberStrategyMock
                .SetupSequence(s => s.CalculateFeeAsync(It.IsAny<ComplianceSchemeMemberWithRegulatorDto>(), cancellationToken))
                .ReturnsAsync(63100)
                .ReturnsAsync(165800);

            _complianceSchemeOnlineMarketStrategyMock
                .SetupSequence(s => s.CalculateFeeAsync(It.IsAny<ComplianceSchemeMemberWithRegulatorDto>(), cancellationToken))
                .ReturnsAsync(0)
                .ReturnsAsync(257900);

            _subsidiariesFeeCalculationStrategyMock
                .SetupSequence(s => s.CalculateFeeAsync(It.IsAny<ComplianceSchemeMemberWithRegulatorDto>(), cancellationToken))
                .ReturnsAsync(new SubsidiariesFeeBreakdown
                {
                    TotalSubsidiariesOMPFees = 0,
                    FeeBreakdowns = new List<FeeBreakdown>
                    {
                        new FeeBreakdown { TotalPrice = 279000 },
                        new FeeBreakdown { TotalPrice = 0 },
                        new FeeBreakdown { TotalPrice = 0 }
                    }
                })
                .ReturnsAsync(new SubsidiariesFeeBreakdown
                {
                    TotalSubsidiariesOMPFees = 0,
                    FeeBreakdowns = new List<FeeBreakdown>
                    {
                         new FeeBreakdown { TotalPrice = 1116000 },
                         new FeeBreakdown { TotalPrice = 1120000 },
                         new FeeBreakdown { TotalPrice = 0 }
                    }
                });

            _paymentsServiceMock.Setup(s => s.GetPreviousPaymentsByReferenceAsync(request.ApplicationReferenceNumber, It.IsAny<CancellationToken>()))
                .ReturnsAsync(100M);

            // Act
            var result = await _service.CalculateFeesAsync(request, cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                result.ComplianceSchemeRegistrationFee.Should().Be(1380400M);

                result.ComplianceSchemeMembersWithFees.Should().HaveCount(2);

                // member 1
                var member1 = result.ComplianceSchemeMembersWithFees.First();
                member1.MemberId.Should().Be("12345");
                member1.MemberRegistrationFee.Should().Be(63100M);
                member1.MemberOnlineMarketPlaceFee.Should().Be(0);
                member1.MemberLateRegistrationFee.Should().Be(0);
                member1.SubsidiariesFee.Should().Be(279000M);
                member1.TotalMemberFee.Should().Be(342100M);

                // member 2
                var member2 = result.ComplianceSchemeMembersWithFees.Last();
                member2.MemberId.Should().Be("67890");
                member2.MemberRegistrationFee.Should().Be(165800M);
                member2.MemberOnlineMarketPlaceFee.Should().Be(257900M);
                member2.MemberLateRegistrationFee.Should().Be(0);
                member2.SubsidiariesFee.Should().Be(2236000M);
                member2.TotalMemberFee.Should().Be(2659700M);

                // Total fee calculation
                result.TotalFee.Should().Be(4382200M);
                result.PreviousPayment.Should().Be(100M);
                result.OutstandingPayment.Should().Be(4382100M);
            }
        }

        [TestMethod]
        public async Task CalculateFeesAsync_WithTwoMembersOneSmallOneLargeWithMemberAndSubsidiariesOMP_ReturnsCorrectAggregatedFees()
        {
            // Arrange
            var request = new ComplianceSchemeFeesRequestDto
            {
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "ABC123",
                SubmissionDate = DateTime.UtcNow,
                ComplianceSchemeMembers = new List<ComplianceSchemeMemberDto>
                {
                    new ComplianceSchemeMemberDto
                    {
                        MemberId = "12345",
                        MemberType = "Small",
                        IsOnlineMarketplace = false,
                        IsLateFeeApplicable = false,
                        NumberOfSubsidiaries = 5,
                        NoOfSubsidiariesOnlineMarketplace = 0
                    },
                    new ComplianceSchemeMemberDto
                    {
                        MemberId = "67890",
                        MemberType = "Large",
                        IsOnlineMarketplace = true,
                        IsLateFeeApplicable = false,
                        NumberOfSubsidiaries = 105,
                        NoOfSubsidiariesOnlineMarketplace = 10
                    }
                }
            };
            var cancellationToken = CancellationToken.None;

            _baseFeeCalculationStrategyMock
                .Setup(s => s.CalculateFeeAsync(It.IsAny<ComplianceSchemeFeesRequestDto>(), cancellationToken))
                .ReturnsAsync(1380400);

            _complianceSchemeMemberStrategyMock
                .SetupSequence(s => s.CalculateFeeAsync(It.IsAny<ComplianceSchemeMemberWithRegulatorDto>(), cancellationToken))
                .ReturnsAsync(63100)
                .ReturnsAsync(165800);

            _complianceSchemeOnlineMarketStrategyMock
                .SetupSequence(s => s.CalculateFeeAsync(It.IsAny<ComplianceSchemeMemberWithRegulatorDto>(), cancellationToken))
                .ReturnsAsync(0)
                .ReturnsAsync(257900);

            _subsidiariesFeeCalculationStrategyMock
                .SetupSequence(s => s.CalculateFeeAsync(It.IsAny<ComplianceSchemeMemberWithRegulatorDto>(), cancellationToken))
                .ReturnsAsync(new SubsidiariesFeeBreakdown
                {
                    TotalSubsidiariesOMPFees = 0,
                    FeeBreakdowns = new List<FeeBreakdown>
                    {
                        new FeeBreakdown { TotalPrice = 279000 },
                        new FeeBreakdown { TotalPrice = 0 },
                        new FeeBreakdown { TotalPrice = 0 }
                    }
                })
                .ReturnsAsync(new SubsidiariesFeeBreakdown
                {
                    TotalSubsidiariesOMPFees = 2579000,
                    FeeBreakdowns = new List<FeeBreakdown>
                    {
                         new FeeBreakdown { TotalPrice = 1116000 },
                         new FeeBreakdown { TotalPrice = 1120000 },
                         new FeeBreakdown { TotalPrice = 0 }
                    }
                });

            _paymentsServiceMock.Setup(s => s.GetPreviousPaymentsByReferenceAsync(request.ApplicationReferenceNumber, It.IsAny<CancellationToken>()))
                .ReturnsAsync(100M);

            // Act
            var result = await _service.CalculateFeesAsync(request, cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                result.ComplianceSchemeRegistrationFee.Should().Be(1380400);

                result.ComplianceSchemeMembersWithFees.Should().HaveCount(2);

                // member 1
                var member1 = result.ComplianceSchemeMembersWithFees.First();
                member1.MemberId.Should().Be("12345");
                member1.MemberRegistrationFee.Should().Be(63100);
                member1.MemberOnlineMarketPlaceFee.Should().Be(0);
                member1.MemberLateRegistrationFee.Should().Be(0);
                member1.SubsidiariesFee.Should().Be(279000);
                member1.TotalMemberFee.Should().Be(342100);

                // member 2
                var member2 = result.ComplianceSchemeMembersWithFees.Last();
                member2.MemberId.Should().Be("67890");
                member2.MemberRegistrationFee.Should().Be(165800);
                member2.MemberOnlineMarketPlaceFee.Should().Be(257900);
                member2.MemberLateRegistrationFee.Should().Be(0);
                member2.SubsidiariesFee.Should().Be(4815000);
                member2.TotalMemberFee.Should().Be(5238700);

                // Total fee calculation
                result.TotalFee.Should().Be(6961200);
                result.PreviousPayment.Should().Be(100M);
                result.OutstandingPayment.Should().Be(6961100);
            }
        }

        [TestMethod]
        public async Task CalculateFeesAsync_WithNoMembers_ReturnsOnlyRegistrationFee()
        {
            // Arrange
            var request = new ComplianceSchemeFeesRequestDto
            {
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.UtcNow
            };

            // Mock base fee calculation
            _baseFeeCalculationStrategyMock
                .Setup(s => s.CalculateFeeAsync(It.IsAny<ComplianceSchemeFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(100);

            // Act
            var result = await _service.CalculateFeesAsync(request, It.IsAny<CancellationToken>());

            // Assert
            using (new AssertionScope())
            {
                result.ComplianceSchemeRegistrationFee.Should().Be(100);
                result.ComplianceSchemeMembersWithFees.Should().BeEmpty();
                result.TotalFee.Should().Be(100);
                result.PreviousPayment.Should().Be(0);
                result.OutstandingPayment.Should().Be(100);
            }
        }

        [TestMethod]
        public async Task CalculateFeesAsync_InvalidRegulator_ThrowsInvalidOperationException()
        {
            // Arrange
            var request = new ComplianceSchemeFeesRequestDto
            {
                Regulator = "InvalidRegulator",
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.UtcNow
            };
            var cancellationToken = CancellationToken.None;

            // Act
            Func<Task> act = async () => await _service.CalculateFeesAsync(request, cancellationToken);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Invalid regulator type: InvalidRegulator. (Parameter 'regulator')");
        }

        [TestMethod]
        public async Task CalculateFeesAsync_WhenIncludeRegistrationFeeIsFalse_DoesNotChargeRegistrationFee()
        {
            // Arrange
            var request = new ComplianceSchemeFeesRequestDto
            {
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "REF123",
                SubmissionDate = DateTime.UtcNow,
                IncludeRegistrationFee = false,
                ComplianceSchemeMembers = new List<ComplianceSchemeMemberDto>
                {
                    new ComplianceSchemeMemberDto
                    {
                        MemberId = "M1",
                        MemberType = "Small",
                        IsOnlineMarketplace = false,
                        IsLateFeeApplicable = false,
                        NumberOfSubsidiaries = 5,
                        NoOfSubsidiariesOnlineMarketplace = 0
                    }
                }
            };

            _baseFeeCalculationStrategyMock
                .Setup(s => s.CalculateFeeAsync(It.IsAny<ComplianceSchemeFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("Base fee should not be called when IncludeRegistrationFee is false"));

            _complianceSchemeMemberStrategyMock
                .Setup(s => s.CalculateFeeAsync(It.IsAny<ComplianceSchemeMemberWithRegulatorDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(63100);

            _complianceSchemeOnlineMarketStrategyMock
                .Setup(s => s.CalculateFeeAsync(It.IsAny<ComplianceSchemeMemberWithRegulatorDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            _subsidiariesFeeCalculationStrategyMock
                .Setup(s => s.CalculateFeeAsync(It.IsAny<ComplianceSchemeMemberWithRegulatorDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SubsidiariesFeeBreakdown
                {
                    TotalSubsidiariesOMPFees = 0,
                    FeeBreakdowns = new List<FeeBreakdown>
                    {
                        new FeeBreakdown { TotalPrice = 279000 }
                    }
                });

            _paymentsServiceMock.Setup(s => s.GetPreviousPaymentsByReferenceAsync(request.ApplicationReferenceNumber, It.IsAny<CancellationToken>()))
                .ReturnsAsync(50M);

            // Act
            var result = await _service.CalculateFeesAsync(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.ComplianceSchemeRegistrationFee.Should().Be(0M);

                result.ComplianceSchemeMembersWithFees.Should().HaveCount(1);
                var member = result.ComplianceSchemeMembersWithFees.First();
                member.MemberId.Should().Be("M1");
                member.MemberRegistrationFee.Should().Be(63100M);
                member.MemberOnlineMarketPlaceFee.Should().Be(0M);
                member.MemberLateRegistrationFee.Should().Be(0M);
                member.SubsidiariesFee.Should().Be(279000M);
                member.TotalMemberFee.Should().Be(63100M + 0M + 279000M + 0M);

                result.TotalFee.Should().Be(member.TotalMemberFee);
                result.PreviousPayment.Should().Be(50M);
                result.OutstandingPayment.Should().Be(result.TotalFee - 50M);
            }
        }

        [TestMethod]
        public async Task CalculateFeesAsync_CSWith1MemberWithLateFee_ReturnsCorrectFees()
        {
            // Arrange
            var request = new ComplianceSchemeFeesRequestDto
            {
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "ABC123",
                SubmissionDate = DateTime.UtcNow,
                ComplianceSchemeMembers = new List<ComplianceSchemeMemberDto>
            {
                new ComplianceSchemeMemberDto
                {
                    MemberId = "12345",
                    MemberType = "Small",
                    IsOnlineMarketplace = false,
                    IsLateFeeApplicable = true,
                    NumberOfSubsidiaries = 5,
                    NoOfSubsidiariesOnlineMarketplace = 0
                }
            }
            };

            _baseFeeCalculationStrategyMock
                .Setup(s => s.CalculateFeeAsync(It.IsAny<ComplianceSchemeFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1380400);

            _complianceSchemeMemberStrategyMock
                .Setup(s => s.CalculateFeeAsync(It.IsAny<ComplianceSchemeMemberWithRegulatorDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(63100);

            _complianceSchemeOnlineMarketStrategyMock
                .Setup(s => s.CalculateFeeAsync(It.IsAny<ComplianceSchemeMemberWithRegulatorDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            _complianceSchemeLateFeeStrategyMock
                .Setup(s => s.CalculateFeeAsync(It.IsAny<ComplianceSchemeLateFeeRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(33200);

            _subsidiariesFeeCalculationStrategyMock
                .Setup(s => s.CalculateFeeAsync(It.IsAny<ComplianceSchemeMemberWithRegulatorDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SubsidiariesFeeBreakdown
                {
                    TotalSubsidiariesOMPFees = 0,
                    FeeBreakdowns = new List<FeeBreakdown>
                    {
                        new FeeBreakdown { TotalPrice = 279000 },
                        new FeeBreakdown { TotalPrice = 0 },
                        new FeeBreakdown { TotalPrice = 0 }
                    }
                });

            _paymentsServiceMock.Setup(s => s.GetPreviousPaymentsByReferenceAsync(request.ApplicationReferenceNumber, It.IsAny<CancellationToken>()))
                .ReturnsAsync(100M);

            // Act
            var result = await _service.CalculateFeesAsync(request, It.IsAny<CancellationToken>());

            // Assert
            using (new AssertionScope())
            {
                result.ComplianceSchemeRegistrationFee.Should().Be(1380400M);
                result.ComplianceSchemeMembersWithFees.Should().HaveCount(1);
                var member = result.ComplianceSchemeMembersWithFees.First();
                member.MemberRegistrationFee.Should().Be(63100M);
                member.MemberOnlineMarketPlaceFee.Should().Be(0);

                // Update for late fee with subsidiaries: Member late fee + (5 subsidiaries * late fee per subsidiary)
                var expectedMemberLateRegistrationFee = 33200M + (5 * 33200M);
                member.MemberLateRegistrationFee.Should().Be(expectedMemberLateRegistrationFee);

                member.SubsidiariesFee.Should().Be(279000M);

                // Update total member fee to include adjusted late fee
                member.TotalMemberFee.Should().Be(63100M + 0 + 279000M + expectedMemberLateRegistrationFee);

                var expectedTotalFee = 1380400M + member.TotalMemberFee;
                result.TotalFee.Should().Be(expectedTotalFee);
                result.PreviousPayment.Should().Be(100M);
                result.OutstandingPayment.Should().Be(expectedTotalFee - 100M);
            }
        }

        [TestMethod]
        public async Task CalculateFeesAsync_CSWith2MembersWithLateFee_ReturnsCorrectAggregatedFees()
        {
            // Arrange
            var request = new ComplianceSchemeFeesRequestDto
            {
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "ABC123",
                SubmissionDate = DateTime.UtcNow,
                ComplianceSchemeMembers = new List<ComplianceSchemeMemberDto>
        {
            new ComplianceSchemeMemberDto
            {
                MemberId = "12345",
                MemberType = "Small",
                IsOnlineMarketplace = false,
                IsLateFeeApplicable = true,
                NumberOfSubsidiaries = 5,
                NoOfSubsidiariesOnlineMarketplace = 0
            },
            new ComplianceSchemeMemberDto
            {
                MemberId = "67890",
                MemberType = "Large",
                IsOnlineMarketplace = true,
                IsLateFeeApplicable = true,
                NumberOfSubsidiaries = 105,
                NoOfSubsidiariesOnlineMarketplace = 0
            }
        }
            };
            var cancellationToken = CancellationToken.None;

            _baseFeeCalculationStrategyMock
                .Setup(s => s.CalculateFeeAsync(It.IsAny<ComplianceSchemeFeesRequestDto>(), cancellationToken))
                .ReturnsAsync(1380400);

            _complianceSchemeMemberStrategyMock
                .SetupSequence(s => s.CalculateFeeAsync(It.IsAny<ComplianceSchemeMemberWithRegulatorDto>(), cancellationToken))
                .ReturnsAsync(63100)
                .ReturnsAsync(165800);

            _complianceSchemeOnlineMarketStrategyMock
                .SetupSequence(s => s.CalculateFeeAsync(It.IsAny<ComplianceSchemeMemberWithRegulatorDto>(), cancellationToken))
                .ReturnsAsync(0)
                .ReturnsAsync(257900);

            _complianceSchemeLateFeeStrategyMock
                .Setup(s => s.CalculateFeeAsync(It.IsAny<ComplianceSchemeLateFeeRequestDto>(), cancellationToken))
                .ReturnsAsync(33200);

            _subsidiariesFeeCalculationStrategyMock
                .SetupSequence(s => s.CalculateFeeAsync(It.IsAny<ComplianceSchemeMemberWithRegulatorDto>(), cancellationToken))
                .ReturnsAsync(new SubsidiariesFeeBreakdown
                {
                    TotalSubsidiariesOMPFees = 0,
                    FeeBreakdowns = new List<FeeBreakdown>
                    {
                new FeeBreakdown { TotalPrice = 279000 },
                new FeeBreakdown { TotalPrice = 0 },
                new FeeBreakdown { TotalPrice = 0 }
                    }
                })
                .ReturnsAsync(new SubsidiariesFeeBreakdown
                {
                    TotalSubsidiariesOMPFees = 0,
                    FeeBreakdowns = new List<FeeBreakdown>
                    {
                new FeeBreakdown { TotalPrice = 1116000 },
                new FeeBreakdown { TotalPrice = 1120000 },
                new FeeBreakdown { TotalPrice = 0 }
                    }
                });

            _paymentsServiceMock.Setup(s => s.GetPreviousPaymentsByReferenceAsync(request.ApplicationReferenceNumber, It.IsAny<CancellationToken>()))
                .ReturnsAsync(100M);

            // Act
            var result = await _service.CalculateFeesAsync(request, cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                result.ComplianceSchemeRegistrationFee.Should().Be(1380400M);

                result.ComplianceSchemeMembersWithFees.Should().HaveCount(2);

                // Member 1
                var member1 = result.ComplianceSchemeMembersWithFees.First();
                member1.MemberId.Should().Be("12345");
                member1.MemberRegistrationFee.Should().Be(63100M);
                member1.MemberOnlineMarketPlaceFee.Should().Be(0);

                // Calculate the expected late fee for member 1
                var expectedMember1LateRegistrationFee = 33200M + (5 * 33200M);
                member1.MemberLateRegistrationFee.Should().Be(expectedMember1LateRegistrationFee);

                member1.SubsidiariesFee.Should().Be(279000M);
                member1.TotalMemberFee.Should().Be(63100M + 0 + 279000M + expectedMember1LateRegistrationFee);

                // Member 2
                var member2 = result.ComplianceSchemeMembersWithFees.Last();
                member2.MemberId.Should().Be("67890");
                member2.MemberRegistrationFee.Should().Be(165800M);
                member2.MemberOnlineMarketPlaceFee.Should().Be(257900M);

                // Calculate the expected late fee for member 2
                var expectedMember2LateRegistrationFee = 33200M + (105 * 33200M);
                member2.MemberLateRegistrationFee.Should().Be(expectedMember2LateRegistrationFee);

                member2.SubsidiariesFee.Should().Be(2236000M);
                member2.TotalMemberFee.Should().Be(165800M + 257900M + 2236000M + expectedMember2LateRegistrationFee);

                // Total fee calculation including both members
                var expectedTotalFee = 1380400M + member1.TotalMemberFee + member2.TotalMemberFee;
                result.TotalFee.Should().Be(expectedTotalFee);
                result.PreviousPayment.Should().Be(100M);
                result.OutstandingPayment.Should().Be(expectedTotalFee - 100M);
            }
        }

        [TestMethod]
        public async Task CalculateFeesAsync_CSWith2MembersWith1LateMemberRegstrationFee_ReturnsCorrectAggregatedFees()
        {
            // Arrange
            var request = new ComplianceSchemeFeesRequestDto
            {
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "ABC123",
                SubmissionDate = DateTime.UtcNow,
                ComplianceSchemeMembers = new List<ComplianceSchemeMemberDto>
                {
                    new ComplianceSchemeMemberDto
                    {
                        MemberId = "12345",
                        MemberType = "Small",
                        IsOnlineMarketplace = false,
                        IsLateFeeApplicable = false,
                        NumberOfSubsidiaries = 5,
                        NoOfSubsidiariesOnlineMarketplace = 0
                    },
                    new ComplianceSchemeMemberDto
                    {
                        MemberId = "67890",
                        MemberType = "Large",
                        IsOnlineMarketplace = true,
                        IsLateFeeApplicable = true,
                        NumberOfSubsidiaries = 105,
                        NoOfSubsidiariesOnlineMarketplace = 0
                    }
                }
            };
            var cancellationToken = CancellationToken.None;

            _baseFeeCalculationStrategyMock
                .Setup(s => s.CalculateFeeAsync(It.IsAny<ComplianceSchemeFeesRequestDto>(), cancellationToken))
                .ReturnsAsync(1380400);

            _complianceSchemeMemberStrategyMock
                .SetupSequence(s => s.CalculateFeeAsync(It.IsAny<ComplianceSchemeMemberWithRegulatorDto>(), cancellationToken))
                .ReturnsAsync(63100)
                .ReturnsAsync(165800);

            _complianceSchemeOnlineMarketStrategyMock
                .SetupSequence(s => s.CalculateFeeAsync(It.IsAny<ComplianceSchemeMemberWithRegulatorDto>(), cancellationToken))
                .ReturnsAsync(0)
                .ReturnsAsync(257900);

            _complianceSchemeLateFeeStrategyMock
                .SetupSequence(s => s.CalculateFeeAsync(It.IsAny<ComplianceSchemeLateFeeRequestDto>(), cancellationToken))
                .ReturnsAsync(33200);

            _subsidiariesFeeCalculationStrategyMock
                .SetupSequence(s => s.CalculateFeeAsync(It.IsAny<ComplianceSchemeMemberWithRegulatorDto>(), cancellationToken))
                .ReturnsAsync(new SubsidiariesFeeBreakdown
                {
                    TotalSubsidiariesOMPFees = 0,
                    FeeBreakdowns = new List<FeeBreakdown>
                    {
                        new FeeBreakdown { TotalPrice = 279000 },
                        new FeeBreakdown { TotalPrice = 0 },
                        new FeeBreakdown { TotalPrice = 0 }
                    }
                })
                .ReturnsAsync(new SubsidiariesFeeBreakdown
                {
                    TotalSubsidiariesOMPFees = 0,
                    FeeBreakdowns = new List<FeeBreakdown>
                    {
                         new FeeBreakdown { TotalPrice = 1116000 },
                         new FeeBreakdown { TotalPrice = 1120000 },
                         new FeeBreakdown { TotalPrice = 0 }
                    }
                });

            _paymentsServiceMock.Setup(s => s.GetPreviousPaymentsByReferenceAsync(request.ApplicationReferenceNumber, It.IsAny<CancellationToken>()))
                .ReturnsAsync(100M);

            // Act
            var result = await _service.CalculateFeesAsync(request, cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                result.ComplianceSchemeRegistrationFee.Should().Be(1380400M);
                result.ComplianceSchemeMembersWithFees.Should().HaveCount(2);

                // Member 1 (no late fee)
                var member1 = result.ComplianceSchemeMembersWithFees.First();
                member1.MemberId.Should().Be("12345");
                member1.MemberRegistrationFee.Should().Be(63100M);
                member1.MemberOnlineMarketPlaceFee.Should().Be(0);
                member1.MemberLateRegistrationFee.Should().Be(0);
                member1.SubsidiariesFee.Should().Be(279000M);
                member1.TotalMemberFee.Should().Be(342100M);

                // Member 2 (with late fee)
                var member2 = result.ComplianceSchemeMembersWithFees.Last();
                member2.MemberId.Should().Be("67890");
                member2.MemberRegistrationFee.Should().Be(165800M);
                member2.MemberOnlineMarketPlaceFee.Should().Be(257900M);

                // Expected late fee: base late fee + (number of subsidiaries * late fee)
                var expectedMemberLateRegistrationFee = 33200M + (105 * 33200M);
                member2.MemberLateRegistrationFee.Should().Be(expectedMemberLateRegistrationFee);

                member2.SubsidiariesFee.Should().Be(2236000M);

                // Total member fee including adjusted late fee
                member2.TotalMemberFee.Should().Be(165800M + 257900M + 2236000M + expectedMemberLateRegistrationFee);

                // Total fee calculation including both members
                var expectedTotalFee = 1380400M + member1.TotalMemberFee + member2.TotalMemberFee;
                result.TotalFee.Should().Be(expectedTotalFee);
                result.PreviousPayment.Should().Be(100M);
                result.OutstandingPayment.Should().Be(expectedTotalFee - 100M);
            }
        }
    }
}