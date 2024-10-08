using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
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
        private Mock<ICSBaseFeeCalculationStrategy<RegulatorType, decimal>> _baseFeeCalculationStrategyMock = null!;
        private Mock<ICSOMPFeeCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto, decimal>> _complianceSchemeOnlineMarketStrategyMock = null!;
        private Mock<ICSMemberFeeCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto, decimal>> _complianceSchemeMemberStrategyMock = null!;
        private Mock<IBaseSubsidiariesFeeCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto, SubsidiariesFeeBreakdown>> _subsidiariesFeeCalculationStrategyMock = null!;
        private ComplianceSchemeCalculatorService _service = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _baseFeeCalculationStrategyMock = new Mock<ICSBaseFeeCalculationStrategy<RegulatorType, decimal>>();
            _complianceSchemeOnlineMarketStrategyMock = new Mock<ICSOMPFeeCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto, decimal>>();
            _complianceSchemeMemberStrategyMock = new Mock<ICSMemberFeeCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto, decimal>>();
            _subsidiariesFeeCalculationStrategyMock = new Mock<IBaseSubsidiariesFeeCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto, SubsidiariesFeeBreakdown>>();
            _service = new ComplianceSchemeCalculatorService(_baseFeeCalculationStrategyMock.Object, _complianceSchemeOnlineMarketStrategyMock.Object, _complianceSchemeMemberStrategyMock.Object, _subsidiariesFeeCalculationStrategyMock.Object);
        }

        [TestMethod]
        public void Constructor_WhenBaseFeeCalculationStrategyIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            ICSBaseFeeCalculationStrategy<RegulatorType, decimal>? baseFeeCalculationStrategy = null;

            // Act
            Action act = () => new ComplianceSchemeCalculatorService(baseFeeCalculationStrategy!, _complianceSchemeOnlineMarketStrategyMock.Object, _complianceSchemeMemberStrategyMock.Object, _subsidiariesFeeCalculationStrategyMock.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'baseFeeCalculationStrategy')");
        }

        [TestMethod]
        public void Constructor_WhenComplianceSchemeOnlineMarketStrategyIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            ICSOMPFeeCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto, decimal>? complianceSchemeOnlineMarketStrategy = null;

            // Act
            Action act = () => new ComplianceSchemeCalculatorService(_baseFeeCalculationStrategyMock.Object, complianceSchemeOnlineMarketStrategy!, _complianceSchemeMemberStrategyMock.Object, _subsidiariesFeeCalculationStrategyMock.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'complianceSchemeOnlineMarketStrategy')");
        }

        [TestMethod]
        public void Constructor_WhenComplianceSchemeMemberStrategyIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            ICSMemberFeeCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto, decimal>? complianceSchemeMemberStrategy = null;

            // Act
            Action act = () => new ComplianceSchemeCalculatorService(_baseFeeCalculationStrategyMock.Object, _complianceSchemeOnlineMarketStrategyMock.Object, complianceSchemeMemberStrategy!, _subsidiariesFeeCalculationStrategyMock.Object);

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
            Action act = () => new ComplianceSchemeCalculatorService(_baseFeeCalculationStrategyMock.Object, _complianceSchemeOnlineMarketStrategyMock.Object, _complianceSchemeMemberStrategyMock.Object, subsidiariesFeeCalculationStrategy!);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'subsidiariesFeeCalculationStrategy')");
        }

        [TestMethod]
        public void Constructor_WhenAllDependenciesAreNotNull_ShouldInitializeComplianceSchemeBaseFeeService()
        {
            // Act
            var service = new ComplianceSchemeCalculatorService(_baseFeeCalculationStrategyMock.Object, _complianceSchemeOnlineMarketStrategyMock.Object, _complianceSchemeMemberStrategyMock.Object, _subsidiariesFeeCalculationStrategyMock.Object);

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
                ComplianceSchemeMembers = new List<ComplianceSchemeMemberDto>
            {
                new ComplianceSchemeMemberDto
                {
                    MemberId = 12345,
                    MemberType = "Small",
                    IsOnlineMarketplace = false,
                    NumberOfSubsidiaries = 5,
                    NoOfSubsidiariesOnlineMarketplace = 0
                }
            }
            };

            _baseFeeCalculationStrategyMock
                .Setup(s => s.CalculateFeeAsync(It.IsAny<RegulatorType>(), It.IsAny<CancellationToken>()))
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
                member.SubsidiariesFee.Should().Be(279000M);
                member.TotalMemberFee.Should().Be(342100M);

                result.TotalFee.Should().Be(1722500M);
                result.PreviousPayment.Should().Be(0);
                result.OutstandingPayment.Should().Be(1722500M);
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
                ComplianceSchemeMembers = new List<ComplianceSchemeMemberDto>
            {
                new ComplianceSchemeMemberDto
                {
                    MemberId = 12345,
                    MemberType = "Large",
                    IsOnlineMarketplace = true,
                    NumberOfSubsidiaries = 105,
                    NoOfSubsidiariesOnlineMarketplace = 0
                }
            }
            };

            _baseFeeCalculationStrategyMock
                .Setup(s => s.CalculateFeeAsync(It.IsAny<RegulatorType>(), It.IsAny<CancellationToken>()))
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
                member.SubsidiariesFee.Should().Be(2236000M);
                member.TotalMemberFee.Should().Be(2659700M);

                result.TotalFee.Should().Be(4040100M);
                result.PreviousPayment.Should().Be(0);
                result.OutstandingPayment.Should().Be(4040100M);
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
                ComplianceSchemeMembers = new List<ComplianceSchemeMemberDto>
                {
                    new ComplianceSchemeMemberDto
                    {
                        MemberId = 12345,
                        MemberType = "Small",
                        IsOnlineMarketplace = false,
                        NumberOfSubsidiaries = 5,
                        NoOfSubsidiariesOnlineMarketplace = 0
                    },
                    new ComplianceSchemeMemberDto
                    {
                        MemberId = 67890,
                        MemberType = "Large",
                        IsOnlineMarketplace = true,
                        NumberOfSubsidiaries = 105,
                        NoOfSubsidiariesOnlineMarketplace = 0
                    }
                }
            };
            var cancellationToken = CancellationToken.None;

            _baseFeeCalculationStrategyMock
                .Setup(s => s.CalculateFeeAsync(It.IsAny<RegulatorType>(), cancellationToken))
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

            // Act
            var result = await _service.CalculateFeesAsync(request, cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                result.ComplianceSchemeRegistrationFee.Should().Be(1380400M);

                result.ComplianceSchemeMembersWithFees.Should().HaveCount(2);

                // member 1
                var member1 = result.ComplianceSchemeMembersWithFees.First();
                member1.MemberId.Should().Be(12345);
                member1.MemberRegistrationFee.Should().Be(63100M);
                member1.MemberOnlineMarketPlaceFee.Should().Be(0);
                member1.SubsidiariesFee.Should().Be(279000M);
                member1.TotalMemberFee.Should().Be(342100M);

                // member 2
                var member2 = result.ComplianceSchemeMembersWithFees.Last();
                member2.MemberId.Should().Be(67890);
                member2.MemberRegistrationFee.Should().Be(165800M);
                member2.MemberOnlineMarketPlaceFee.Should().Be(257900M);
                member2.SubsidiariesFee.Should().Be(2236000M);
                member2.TotalMemberFee.Should().Be(2659700M);

                // Total fee calculation
                result.TotalFee.Should().Be(4382200M);
                result.PreviousPayment.Should().Be(0);
                result.OutstandingPayment.Should().Be(4382200M);
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
                ComplianceSchemeMembers = new List<ComplianceSchemeMemberDto>
                {
                    new ComplianceSchemeMemberDto
                    {
                        MemberId = 12345,
                        MemberType = "Small",
                        IsOnlineMarketplace = false,
                        NumberOfSubsidiaries = 5,
                        NoOfSubsidiariesOnlineMarketplace = 0
                    },
                    new ComplianceSchemeMemberDto
                    {
                        MemberId = 67890,
                        MemberType = "Large",
                        IsOnlineMarketplace = true,
                        NumberOfSubsidiaries = 105,
                        NoOfSubsidiariesOnlineMarketplace = 10
                    }
                }
            };
            var cancellationToken = CancellationToken.None;

            _baseFeeCalculationStrategyMock
                .Setup(s => s.CalculateFeeAsync(It.IsAny<RegulatorType>(), cancellationToken))
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

            // Act
            var result = await _service.CalculateFeesAsync(request, cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                result.ComplianceSchemeRegistrationFee.Should().Be(1380400);

                result.ComplianceSchemeMembersWithFees.Should().HaveCount(2);

                // member 1
                var member1 = result.ComplianceSchemeMembersWithFees.First();
                member1.MemberId.Should().Be(12345);
                member1.MemberRegistrationFee.Should().Be(63100);
                member1.MemberOnlineMarketPlaceFee.Should().Be(0);
                member1.SubsidiariesFee.Should().Be(279000);
                member1.TotalMemberFee.Should().Be(342100);

                // member 2
                var member2 = result.ComplianceSchemeMembersWithFees.Last();
                member2.MemberId.Should().Be(67890);
                member2.MemberRegistrationFee.Should().Be(165800);
                member2.MemberOnlineMarketPlaceFee.Should().Be(257900);
                member2.SubsidiariesFee.Should().Be(4815000);
                member2.TotalMemberFee.Should().Be(5238700);

                // Total fee calculation
                result.TotalFee.Should().Be(6961200);
                result.PreviousPayment.Should().Be(0);
                result.OutstandingPayment.Should().Be(6961200);
            }
        }

        [TestMethod]
        public async Task CalculateFeesAsync_WithNoMembers_ReturnsOnlyRegistrationFee()
        {
            // Arrange
            var request = new ComplianceSchemeFeesRequestDto
            {
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "A123"
            };

            // Mock base fee calculation
            _baseFeeCalculationStrategyMock
                .Setup(s => s.CalculateFeeAsync(It.IsAny<RegulatorType>(), It.IsAny<CancellationToken>()))
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
                ApplicationReferenceNumber = "A123"
            };
            var cancellationToken = CancellationToken.None;

            // Act
            Func<Task> act = async () => await _service.CalculateFeesAsync(request, cancellationToken);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage(ComplianceSchemeFeeCalculationExceptions.CalculationError);
        }
    }
}