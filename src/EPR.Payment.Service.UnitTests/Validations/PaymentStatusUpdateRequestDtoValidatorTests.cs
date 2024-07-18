using EPR.Payment.Service.Common.Dtos.Request;
using EPR.Payment.Service.Validations;
using FluentValidation.TestHelper;

namespace EPR.Payment.Service.UnitTests.Validations
{
    [TestClass]
    public class PaymentStatusUpdateRequestDtoValidatorTests
    {
        private PaymentStatusUpdateRequestDtoValidator _validator = null!;
        [TestInitialize]
        public void TestInitialize()
        {
            _validator = new PaymentStatusUpdateRequestDtoValidator();
        }

        [TestMethod]
        public void Should_Have_Error_When_GovPayPaymentId_Is_Empty()
        {
            var paymentStatusUpdateRequestDto = new PaymentStatusUpdateRequestDto { GovPayPaymentId = string.Empty };
            var result = _validator.TestValidate(paymentStatusUpdateRequestDto);
            result.ShouldHaveValidationErrorFor(x => x.GovPayPaymentId);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_GovPayPaymentId_Is_Valid()
        {
            var paymentStatusUpdateRequestDto = new PaymentStatusUpdateRequestDto { GovPayPaymentId = "Test GovPayPaymentId" };
            var result = _validator.TestValidate(paymentStatusUpdateRequestDto);
            result.ShouldNotHaveValidationErrorFor(x => x.GovPayPaymentId);
        }

        [TestMethod]
        public void Should_Have_Error_When_UpdatedByUserId_Is_Null()
        {
            var paymentStatusUpdateRequestDto = new PaymentStatusUpdateRequestDto { UpdatedByUserId = null };
            var result = _validator.TestValidate(paymentStatusUpdateRequestDto);
            result.ShouldHaveValidationErrorFor(x => x.UpdatedByUserId);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_UpdatedByUserId_Is_Valid()
        {
            var paymentStatusUpdateRequestDto = new PaymentStatusUpdateRequestDto { UpdatedByUserId = Guid.NewGuid() };
            var result = _validator.TestValidate(paymentStatusUpdateRequestDto);
            result.ShouldNotHaveValidationErrorFor(x => x.UpdatedByUserId);
        }

        [TestMethod]
        public void Should_Have_Error_When_UpdatedByOrganisationId_Is_Null()
        {
            var paymentStatusUpdateRequestDto = new PaymentStatusUpdateRequestDto { UpdatedByOrganisationId = null };
            var result = _validator.TestValidate(paymentStatusUpdateRequestDto);
            result.ShouldHaveValidationErrorFor(x => x.UpdatedByOrganisationId);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_UpdatedByOrganisationId_Is_Valid()
        {
            var paymentStatusUpdateRequestDto = new PaymentStatusUpdateRequestDto { UpdatedByOrganisationId = Guid.NewGuid() };
            var result = _validator.TestValidate(paymentStatusUpdateRequestDto);
            result.ShouldNotHaveValidationErrorFor(x => x.UpdatedByOrganisationId);
        }

        [TestMethod]
        public void Should_Have_Error_When_Reference_Is_Empty()
        {
            var paymentStatusUpdateRequestDto = new PaymentStatusUpdateRequestDto { Reference = string.Empty };
            var result = _validator.TestValidate(paymentStatusUpdateRequestDto);
            result.ShouldHaveValidationErrorFor(x => x.Reference);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_Reference_Is_Valid()
        {
            var paymentStatusUpdateRequestDto = new PaymentStatusUpdateRequestDto { Reference = "Test Reference" };
            var result = _validator.TestValidate(paymentStatusUpdateRequestDto);
            result.ShouldNotHaveValidationErrorFor(x => x.Reference);
        }

        [TestMethod]
        public void Should_Have_Error_When_Status_Is_Not_InEnum()
        {
            var paymentStatusUpdateRequestDto = new PaymentStatusUpdateRequestDto { Status = (Common.Dtos.Enums.Status)999 };
            var result = _validator.TestValidate(paymentStatusUpdateRequestDto);
            result.ShouldHaveValidationErrorFor(x => x.Status);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_Status_Is_Valid()
        {
            var paymentStatusUpdateRequestDto = new PaymentStatusUpdateRequestDto { Status = Common.Dtos.Enums.Status.Initiated };
            var result = _validator.TestValidate(paymentStatusUpdateRequestDto);
            result.ShouldNotHaveValidationErrorFor(x => x.Status);
        }

        [TestMethod]
        public void Should_Have_Error_When_ErrorCode_InValid()
        {
            var paymentStatusUpdateRequestDto = new PaymentStatusUpdateRequestDto { ErrorCode = "X" };
            var result = _validator.TestValidate(paymentStatusUpdateRequestDto);
            result.ShouldHaveValidationErrorFor(x => x.ErrorCode);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_ErrorCode_Is_Null()
        {
            var paymentStatusUpdateRequestDto = new PaymentStatusUpdateRequestDto { ErrorCode = null };
            var result = _validator.TestValidate(paymentStatusUpdateRequestDto);
            result.ShouldNotHaveValidationErrorFor(x => x.ErrorCode);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_ErrorCode_Is_Empty()
        {
            var paymentStatusUpdateRequestDto = new PaymentStatusUpdateRequestDto { ErrorCode = string.Empty };
            var result = _validator.TestValidate(paymentStatusUpdateRequestDto);
            result.ShouldNotHaveValidationErrorFor(x => x.ErrorCode);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_ErrorCode_Valid()
        {
            var paymentStatusUpdateRequestDto = new PaymentStatusUpdateRequestDto { ErrorCode = "A" };
            var result = _validator.TestValidate(paymentStatusUpdateRequestDto);
            result.ShouldNotHaveValidationErrorFor(x => x.ErrorCode);
        }
    }
}
