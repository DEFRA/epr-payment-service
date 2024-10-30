using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.Payments;
using EPR.Payment.Service.Validations.Payments;
using FluentValidation.TestHelper;

namespace EPR.Payment.Service.UnitTests.Validations.Payments
{
    [TestClass]
    public class OfflinePaymentStatusInsertRequestDtoValidatorTests
    {
        private OfflinePaymentInsertRequestDtoValidator _validator = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _validator = new OfflinePaymentInsertRequestDtoValidator();
        }

        [TestMethod]
        public void Should_Have_Error_When_UserId_Is_Null()
        {
            var paymentStatusInsertRequestDto = new OfflinePaymentInsertRequestDto { UserId = null };
            var result = _validator.TestValidate(paymentStatusInsertRequestDto);
            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_UserId_Is_Valid()
        {
            var offlinePaymentStatusInsertRequestDto = new OfflinePaymentInsertRequestDto { UserId = Guid.NewGuid() };
            var result = _validator.TestValidate(offlinePaymentStatusInsertRequestDto);
            result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        }

        [TestMethod]
        public void Should_Have_Error_When_Reference_Is_Empty()
        {
            var offlinePaymentStatusInsertRequestDto = new OfflinePaymentInsertRequestDto { Reference = string.Empty };
            var result = _validator.TestValidate(offlinePaymentStatusInsertRequestDto);
            result.ShouldHaveValidationErrorFor(x => x.Reference);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_Reference_Is_Valid()
        {
            var offlinePaymentStatusInsertRequestDto = new OfflinePaymentInsertRequestDto { Reference = "Test Reference" };
            var result = _validator.TestValidate(offlinePaymentStatusInsertRequestDto);
            result.ShouldNotHaveValidationErrorFor(x => x.Reference);
        }

        [TestMethod]
        public void Should_Have_Error_When_Amount_Is_Null()
        {
            var offlinePaymentStatusInsertRequestDto = new OfflinePaymentInsertRequestDto { Amount = null };
            var result = _validator.TestValidate(offlinePaymentStatusInsertRequestDto);
            result.ShouldHaveValidationErrorFor(x => x.Amount);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_Amount_Is_Valid()
        {
            var offlinePaymentStatusInsertRequestDto = new OfflinePaymentInsertRequestDto { Amount = 10 };
            var result = _validator.TestValidate(offlinePaymentStatusInsertRequestDto);
            result.ShouldNotHaveValidationErrorFor(x => x.Amount);
        }

        [TestMethod]
        public void Should_Have_Error_When_Regulator_Is_Empty()
        {
            var paymentStatusInsertRequestDto = new OfflinePaymentInsertRequestDto { Regulator = string.Empty };
            var result = _validator.TestValidate(paymentStatusInsertRequestDto);
            result.ShouldHaveValidationErrorFor(x => x.Regulator);
        }

        [TestMethod]
        public void Should_Have_Error_When_Regulator_Is_NotSupported()
        {
            var paymentStatusInsertRequestDto = new OfflinePaymentInsertRequestDto { Regulator = RegulatorConstants.GBSCT };
            var result = _validator.TestValidate(paymentStatusInsertRequestDto);
            result.ShouldHaveValidationErrorFor(x => x.Regulator);
        }
    }
}
