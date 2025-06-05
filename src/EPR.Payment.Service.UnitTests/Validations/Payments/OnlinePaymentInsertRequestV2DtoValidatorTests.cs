using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Enums;
using EPR.Payment.Service.Common.Dtos.Request.Payments;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Validations.Payments;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace EPR.Payment.Service.UnitTests.Validations.Payments
{
    [TestClass]
    public class OnlinePaymentInsertRequestV2DtoValidatorTests
    {
        [TestMethod, AutoMoqData]
        public void Should_Have_Error_WhenAmount_Is_Invalid(OnlinePaymentInsertRequestV2DtoValidator validatorUnderTest)
        {
            OnlinePaymentInsertRequestV2Dto request = new OnlinePaymentInsertRequestV2Dto
            {
                Amount = null,
                OrganisationId = Guid.NewGuid(),
                ReasonForPayment = "Registration Fees",
                Reference = Guid.NewGuid().ToString(),
                Regulator = RegulatorConstants.GBENG,
                RequestorType = OnlinePaymentRequestorTypes.Reprocessors,
                UserId = Guid.NewGuid(),
            };

            TestValidationResult<OnlinePaymentInsertRequestV2Dto> result = validatorUnderTest.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Amount);
        }

        [TestMethod, AutoMoqData]
        public void Should_Have_Error_WhenOrganisationId_Is_Invalid(OnlinePaymentInsertRequestV2DtoValidator validatorUnderTest)
        {
            OnlinePaymentInsertRequestV2Dto request = new OnlinePaymentInsertRequestV2Dto
            {
                Amount = 100,
                OrganisationId = null,
                ReasonForPayment = "Registration Fees",
                Reference = Guid.NewGuid().ToString(),
                Regulator = RegulatorConstants.GBENG,
                RequestorType = OnlinePaymentRequestorTypes.Reprocessors,
                UserId = Guid.NewGuid(),
            };

            TestValidationResult<OnlinePaymentInsertRequestV2Dto> result = validatorUnderTest.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.OrganisationId);
        }

        [TestMethod, AutoMoqData]
        public void Should_Have_Error_WhenReasonForPayment_Is_Invalid(OnlinePaymentInsertRequestV2DtoValidator validatorUnderTest)
        {
            OnlinePaymentInsertRequestV2Dto request = new OnlinePaymentInsertRequestV2Dto
            {
                Amount = 100,
                OrganisationId = Guid.NewGuid(),
                ReasonForPayment = string.Empty,
                Reference = Guid.NewGuid().ToString(),
                Regulator = RegulatorConstants.GBENG,
                RequestorType = OnlinePaymentRequestorTypes.Reprocessors,
                UserId = Guid.NewGuid(),
            };

            TestValidationResult<OnlinePaymentInsertRequestV2Dto> result = validatorUnderTest.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.ReasonForPayment);
        }

        [TestMethod, AutoMoqData]
        public void Should_Have_Error_When_Reference_Is_Invalid(OnlinePaymentInsertRequestV2DtoValidator validatorUnderTest)
        {
            OnlinePaymentInsertRequestV2Dto request = new OnlinePaymentInsertRequestV2Dto 
            { 
                Amount = 100,
                OrganisationId = Guid.NewGuid(),
                ReasonForPayment = "Registration Fees",
                Reference = string.Empty, 
                Regulator = RegulatorConstants.GBENG,
                RequestorType = OnlinePaymentRequestorTypes.Reprocessors,
                UserId = Guid.NewGuid(), 
            };

            TestValidationResult<OnlinePaymentInsertRequestV2Dto> result = validatorUnderTest.TestValidate(request);
            
            result.ShouldHaveValidationErrorFor(x => x.Reference);
        }

        [TestMethod, AutoMoqData]
        public void Should_Have_Error_WhenRegulator_Is_Invalid(OnlinePaymentInsertRequestV2DtoValidator validatorUnderTest)
        {
            OnlinePaymentInsertRequestV2Dto request = new OnlinePaymentInsertRequestV2Dto
            {
                Amount = 100,
                OrganisationId = Guid.NewGuid(),
                ReasonForPayment = "Registration Fees",
                Reference = Guid.NewGuid().ToString(),
                Regulator = null,
                RequestorType = OnlinePaymentRequestorTypes.Reprocessors,
                UserId = Guid.NewGuid(),
            };

            TestValidationResult<OnlinePaymentInsertRequestV2Dto> result = validatorUnderTest.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Regulator);
        }

        [TestMethod, AutoMoqData]
        public void Should_Have_Error_WhenRequestorType_Is_Invalid(OnlinePaymentInsertRequestV2DtoValidator validatorUnderTest)
        {
            OnlinePaymentInsertRequestV2Dto request = new OnlinePaymentInsertRequestV2Dto
            {
                Amount = 100,
                OrganisationId = Guid.NewGuid(),
                ReasonForPayment = "Registration Fees",
                Reference = Guid.NewGuid().ToString(),
                Regulator = RegulatorConstants.GBENG,
                RequestorType = null,
                UserId = Guid.NewGuid(),
            };

            TestValidationResult<OnlinePaymentInsertRequestV2Dto> result = validatorUnderTest.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.RequestorType);
        }

        [TestMethod, AutoMoqData]
        public void Should_Have_Error_WhenUserId_Is_Invalid(OnlinePaymentInsertRequestV2DtoValidator validatorUnderTest)
        {
            OnlinePaymentInsertRequestV2Dto request = new OnlinePaymentInsertRequestV2Dto
            {
                Amount = 100,
                OrganisationId = Guid.NewGuid(),
                ReasonForPayment = "Registration Fees",
                Reference = Guid.NewGuid().ToString(),
                Regulator = RegulatorConstants.GBENG,
                RequestorType = OnlinePaymentRequestorTypes.Reprocessors,
                UserId = null,
            };

            TestValidationResult<OnlinePaymentInsertRequestV2Dto> result = validatorUnderTest.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [TestMethod, AutoMoqData]
        public void Should_Not_Have_Error_When_Request_Is_Valid(OnlinePaymentInsertRequestV2DtoValidator validatorUnderTest)
        {
            OnlinePaymentInsertRequestV2Dto request = new OnlinePaymentInsertRequestV2Dto
            {
                Amount = 100,
                OrganisationId = Guid.NewGuid(),
                ReasonForPayment = "Registration Fees",
                Reference = Guid.NewGuid().ToString(),
                Regulator = RegulatorConstants.GBENG,
                RequestorType = OnlinePaymentRequestorTypes.Reprocessors,
                UserId = Guid.NewGuid(),
            };

            TestValidationResult<OnlinePaymentInsertRequestV2Dto> result = validatorUnderTest.TestValidate(request);

            result.IsValid.Should().BeTrue();
        }
    }
}
