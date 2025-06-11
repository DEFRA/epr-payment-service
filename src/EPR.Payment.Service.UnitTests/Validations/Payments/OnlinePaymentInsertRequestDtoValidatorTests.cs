using EPR.Payment.Service.Common.Constants.Payments;
using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.Payments;
using EPR.Payment.Service.Validations.Payments;
using FluentValidation.TestHelper;

namespace EPR.Payment.Service.UnitTests.Validations.Payments
{
    [TestClass]
    public class OnlinePaymentInsertRequestDtoValidatorTests
    {
        private OnlinePaymentInsertRequestDtoValidator _validator = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _validator = new OnlinePaymentInsertRequestDtoValidator();
        }

        [TestMethod]
        public void Should_Have_Error_When_UserId_Is_Null()
        {
            var paymentStatusInsertRequestDto = new OnlinePaymentInsertRequestDto { UserId = null };
            var result = _validator.TestValidate(paymentStatusInsertRequestDto);
            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_UserId_Is_Valid()
        {
            var paymentStatusInsertRequestDto = new OnlinePaymentInsertRequestDto { UserId = Guid.NewGuid() };
            var result = _validator.TestValidate(paymentStatusInsertRequestDto);
            result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        }

        [TestMethod]
        public void Should_Have_Error_When_OrganisationId_Is_Null()
        {
            var paymentStatusInsertRequestDto = new OnlinePaymentInsertRequestDto { OrganisationId = null };
            var result = _validator.TestValidate(paymentStatusInsertRequestDto);
            result.ShouldHaveValidationErrorFor(x => x.OrganisationId);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_OrganisationId_Is_Valid()
        {
            var paymentStatusInsertRequestDto = new OnlinePaymentInsertRequestDto { OrganisationId = Guid.NewGuid() };
            var result = _validator.TestValidate(paymentStatusInsertRequestDto);
            result.ShouldNotHaveValidationErrorFor(x => x.OrganisationId);
        }

        [TestMethod]
        public void Should_Have_Error_When_Reference_Is_Empty()
        {
            var paymentStatusInsertRequestDto = new OnlinePaymentInsertRequestDto { Reference = string.Empty };
            var result = _validator.TestValidate(paymentStatusInsertRequestDto);
            result.ShouldHaveValidationErrorFor(x => x.Reference);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_Reference_Is_Valid()
        {
            var paymentStatusInsertRequestDto = new OnlinePaymentInsertRequestDto { Reference = "Test Reference" };
            var result = _validator.TestValidate(paymentStatusInsertRequestDto);
            result.ShouldNotHaveValidationErrorFor(x => x.Reference);
        }

        [TestMethod]
        public void Should_Have_Error_When_ReasonForPayment_Is_Empty()
        {
            var paymentStatusInsertRequestDto = new OnlinePaymentInsertRequestDto { ReasonForPayment = string.Empty };
            var result = _validator.TestValidate(paymentStatusInsertRequestDto);
            result.ShouldHaveValidationErrorFor(x => x.ReasonForPayment);
        }

        [TestMethod]
        [DataRow(ReasonForPaymentConstants.RegistrationFee)]
        [DataRow(ReasonForPaymentConstants.PackagingResubmissionFee)]
        public void Should_Not_Have_Error_When_ReasonForPayment_Is_Valid(string reasonForPayment)
        {
            var paymentStatusInsertRequestDto = new OnlinePaymentInsertRequestDto { ReasonForPayment = reasonForPayment };
            var result = _validator.TestValidate(paymentStatusInsertRequestDto);
            result.ShouldNotHaveValidationErrorFor(x => x.ReasonForPayment);
        }

        [TestMethod]
        public void Should_Have_Error_When_Amount_Is_Null()
        {
            var paymentStatusInsertRequestDto = new OnlinePaymentInsertRequestDto { Amount = null };
            var result = _validator.TestValidate(paymentStatusInsertRequestDto);
            result.ShouldHaveValidationErrorFor(x => x.Amount);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_Amount_Is_Valid()
        {
            var paymentStatusInsertRequestDto = new OnlinePaymentInsertRequestDto { Amount = 10 };
            var result = _validator.TestValidate(paymentStatusInsertRequestDto);
            result.ShouldNotHaveValidationErrorFor(x => x.Amount);
        }

        [TestMethod]
        public void Should_Have_Error_When_Status_Is_Not_InEnum()
        {
            var paymentStatusInsertRequestDto = new OnlinePaymentInsertRequestDto { Status = (Service.Common.Dtos.Enums.Status)999 };
            var result = _validator.TestValidate(paymentStatusInsertRequestDto);
            result.ShouldHaveValidationErrorFor(x => x.Status);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_Status_Is_Valid()
        {
            var paymentStatusInsertRequestDto = new OnlinePaymentInsertRequestDto { Status = Service.Common.Dtos.Enums.Status.Initiated };
            var result = _validator.TestValidate(paymentStatusInsertRequestDto);
            result.ShouldNotHaveValidationErrorFor(x => x.Status);
        }

        [TestMethod]
        public void Should_Have_Error_When_Regulator_Is_Empty()
        {
            var paymentStatusInsertRequestDto = new OnlinePaymentInsertRequestDto { Regulator = string.Empty };
            var result = _validator.TestValidate(paymentStatusInsertRequestDto);
            result.ShouldHaveValidationErrorFor(x => x.Regulator);
        }

        [TestMethod]
        public void Should_Have_Error_When_Regulator_Is_NotSupported()
        {
            var paymentStatusInsertRequestDto = new OnlinePaymentInsertRequestDto { Regulator = RegulatorConstants.GBSCT };
            var result = _validator.TestValidate(paymentStatusInsertRequestDto);
            result.ShouldHaveValidationErrorFor(x => x.Regulator);
        }
    }
}
