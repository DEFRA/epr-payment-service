using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Fees;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Payments;
using EPR.Payment.Service.Common.Dtos.Request.AccreditationFees;
using EPR.Payment.Service.Common.Dtos.Response.AccreditationFees;
using EPR.Payment.Service.Common.Enums;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Helper;
using EPR.Payment.Service.Services.AccreditationFees;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;

namespace EPR.Payment.Service.UnitTests.Services.AccreditationFees
{
    [TestClass]
    public class AccreditationFeesCalculatorServiceTests
    {
        private readonly Mock<IAccreditationFeesRepository> _accreditationFeesRepositoryMock = new();
        private readonly Mock<IPaymentsRepository> _paymentsRepositoryMock = new();

        private AccreditationFeesCalculatorService? _accreditationFeesCalculatorServiceUnderTest = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _accreditationFeesCalculatorServiceUnderTest = new AccreditationFeesCalculatorService(
                _accreditationFeesRepositoryMock.Object,
                _paymentsRepositoryMock.Object);
        }

        [TestMethod]
        [AutoMoqData]
        public async Task CalculateFeesAsync_ShouldCallRespositoryAndReturnResponseWithoutPreviousPyament()
        {
            // Arrange
            AccreditationFeesRequestDto accreditationFeesRequestDto = new()
            {
                Regulator = RegulatorConstants.GBENG,
                MaterialType = AccreditationFeesMaterialType.Plastic,
                RequestorType = AccreditationFeesRequestorType.Exporters,
                NumberOfOverseasSites = 10,
                TonnageBand = TonnageBand.Upto500,
                SubmissionDate = DateTime.UtcNow,
                ApplicationReferenceNumber = Guid.NewGuid().ToString()
            };
            using CancellationTokenSource cancellationTokenSource = new();

            AccreditationFee accreditationFee = new()
            {
                Id = 1,
                RegulatorId = 1,
                GroupId = (int)Common.Enums.Group.Exporters,
                SubGroupId = (int)Common.Enums.SubGroup.Plastic,
                Amount = 100,
                FeesPerSite = 10,
                EffectiveFrom = DateTime.UtcNow.AddDays(-1),
                EffectiveTo = DateTime.UtcNow.AddDays(1),
            };
            (int tonnesOver, int tonnesUpto) = TonnageHelper.GetTonnageBoundaryByTonnageBand(accreditationFeesRequestDto.TonnageBand);

            //Setup
            _accreditationFeesRepositoryMock.Setup(r =>
                r.GetFeeAsync(
                    (int)accreditationFeesRequestDto.RequestorType,
                    (int)accreditationFeesRequestDto.MaterialType,
                    tonnesOver,
                    tonnesUpto,
                    It.IsAny<RegulatorType>(),
                    accreditationFeesRequestDto.SubmissionDate,
                    cancellationTokenSource.Token))
                .ReturnsAsync(accreditationFee);

            // Act
            AccreditationFeesResponseDto? accreditationFeesResponseDto = await _accreditationFeesCalculatorServiceUnderTest!.CalculateFeesAsync(
                accreditationFeesRequestDto,
                cancellationTokenSource.Token);

            // Assert
            // Assert
            using (new AssertionScope())
            {
                accreditationFeesResponseDto.Should().NotBeNull();
                accreditationFeesResponseDto!.TonnageBandCharge.Should().Be(accreditationFee.Amount);
                accreditationFeesResponseDto!.OverseasSiteChargePerSite.Should().Be(accreditationFee.FeesPerSite);
                accreditationFeesResponseDto!.TotalOverseasSitesCharges.Should().Be(accreditationFee.FeesPerSite * accreditationFeesRequestDto.NumberOfOverseasSites);
                accreditationFeesResponseDto!.TotalAccreditationFees.Should().Be((accreditationFee.FeesPerSite * accreditationFeesRequestDto.NumberOfOverseasSites) + accreditationFee.Amount);
                
                // Verify
                _accreditationFeesRepositoryMock.Verify(r =>
                    r.GetFeeAsync(
                        (int)accreditationFeesRequestDto.RequestorType,
                        (int)accreditationFeesRequestDto.MaterialType,
                        tonnesOver,
                        tonnesUpto,
                        It.IsAny<RegulatorType>(),
                        accreditationFeesRequestDto.SubmissionDate,
                        cancellationTokenSource.Token),
                    Times.Once());
                _paymentsRepositoryMock.Verify(r =>
                    r.GetPreviousPaymentsByReferenceAsync(
                        accreditationFeesRequestDto.ApplicationReferenceNumber,
                        cancellationTokenSource.Token),
                        Times.Never());
            }
        }
    }
}
