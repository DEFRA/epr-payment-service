
using EPR.Payment.Service.Common.Dtos.Request.Payments;
using EPR.Payment.Service.Validations.Payments;
using FluentValidation.TestHelper;

namespace EPR.Payment.Service.UnitTests.Validations.Payments
{
    [TestClass]
    public class OfflinePaymentStatusInsertRequestDtoValidatorTests
    {
        private OfflinePaymentStatusInsertRequestDtoValidator _validator = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _validator = new OfflinePaymentStatusInsertRequestDtoValidator();
        }

        [TestMethod]
        public void Should_Have_Error_When_UserId_Is_Null()
        {
            var offlinePaymentStatusInsertRequestDto = new OfflinePaymentStatusInsertRequestDto { UserId = null };
            var result = _validator.TestValidate(offlinePaymentStatusInsertRequestDto);
            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_UserId_Is_Valid()
        {
            var offlinePaymentStatusInsertRequestDto = new OfflinePaymentStatusInsertRequestDto { UserId = Guid.NewGuid() };
            var result = _validator.TestValidate(offlinePaymentStatusInsertRequestDto);
            result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        }

        [TestMethod]
        public void Should_Have_Error_When_Reference_Is_Empty()
        {
            var offlinePaymentStatusInsertRequestDto = new OfflinePaymentStatusInsertRequestDto { Reference = string.Empty };
            var result = _validator.TestValidate(offlinePaymentStatusInsertRequestDto);
            result.ShouldHaveValidationErrorFor(x => x.Reference);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_Reference_Is_Valid()
        {
            var offlinePaymentStatusInsertRequestDto = new OfflinePaymentStatusInsertRequestDto { Reference = "Test Reference" };
            var result = _validator.TestValidate(offlinePaymentStatusInsertRequestDto);
            result.ShouldNotHaveValidationErrorFor(x => x.Reference);
        }

        [TestMethod]
        public void Should_Have_Error_When_Amount_Is_Null()
        {
            var offlinePaymentStatusInsertRequestDto = new OfflinePaymentStatusInsertRequestDto { Amount = null };
            var result = _validator.TestValidate(offlinePaymentStatusInsertRequestDto);
            result.ShouldHaveValidationErrorFor(x => x.Amount);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_Amount_Is_Valid()
        {
            var offlinePaymentStatusInsertRequestDto = new OfflinePaymentStatusInsertRequestDto { Amount = 10 };
            var result = _validator.TestValidate(offlinePaymentStatusInsertRequestDto);
            result.ShouldNotHaveValidationErrorFor(x => x.Amount);
        }
    }
}
