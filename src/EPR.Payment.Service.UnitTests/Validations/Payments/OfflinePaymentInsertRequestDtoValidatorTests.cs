using EPR.Payment.Service.Common.Constants.Payments;
using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.Payments;
using EPR.Payment.Service.Validations.Payments;
using FluentValidation.TestHelper;

namespace EPR.Payment.Service.UnitTests.Validations.Payments
{
    [TestClass]
    public class OfflinePaymentInsertRequestDtoValidatorTests
    {
        private OfflinePaymentInsertRequestDtoValidator _validator = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _validator = new OfflinePaymentInsertRequestDtoValidator();
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_UserId_Is_Valid()
        {
            var offlinePaymentStatusInsertRequestDto = new OfflinePaymentInsertRequestDto { UserId = Guid.NewGuid(), Reference = "Test Reference", Amount = 100, Description = OfflinePayDescConstants.RegistrationFee, Regulator = RegulatorConstants.GBENG };
            var result = _validator.TestValidate(offlinePaymentStatusInsertRequestDto);
            result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        }

        [TestMethod]
        public void Should_Have_Error_When_Reference_Is_Empty()
        {
            var offlinePaymentStatusInsertRequestDto = new OfflinePaymentInsertRequestDto { Reference = string.Empty, UserId = Guid.NewGuid(), Amount = 100, Description = OfflinePayDescConstants.RegistrationFee, Regulator = RegulatorConstants.GBENG };
            var result = _validator.TestValidate(offlinePaymentStatusInsertRequestDto);
            result.ShouldHaveValidationErrorFor(x => x.Reference);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_Reference_Is_Valid()
        {
            var offlinePaymentStatusInsertRequestDto = new OfflinePaymentInsertRequestDto { Reference = "Test Reference", UserId = Guid.NewGuid(), Amount = 100, Description = OfflinePayDescConstants.RegistrationFee, Regulator = RegulatorConstants.GBENG };
            var result = _validator.TestValidate(offlinePaymentStatusInsertRequestDto);
            result.ShouldNotHaveValidationErrorFor(x => x.Reference);
        }

        [TestMethod]
        public void Should_Have_Error_When_Amount_Is_Zero()
        {
            var offlinePaymentStatusInsertRequestDto = new OfflinePaymentInsertRequestDto { Amount = 0, Reference = "Test Reference", UserId = Guid.NewGuid(), Description = OfflinePayDescConstants.RegistrationFee, Regulator = RegulatorConstants.GBENG };
            var result = _validator.TestValidate(offlinePaymentStatusInsertRequestDto);
            result.ShouldHaveValidationErrorFor(x => x.Amount);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_Amount_Is_Valid()
        {
            var offlinePaymentStatusInsertRequestDto = new OfflinePaymentInsertRequestDto { Amount = 10, Reference = "Test Reference", UserId = Guid.NewGuid(), Description = OfflinePayDescConstants.RegistrationFee, Regulator = RegulatorConstants.GBENG };
            var result = _validator.TestValidate(offlinePaymentStatusInsertRequestDto);
            result.ShouldNotHaveValidationErrorFor(x => x.Amount);
        }

        [TestMethod]
        public void Should_Have_Error_When_Regulator_Is_Empty()
        {
            var paymentStatusInsertRequestDto = new OfflinePaymentInsertRequestDto { Regulator = string.Empty, Amount = 10, Reference = "Test Reference", UserId = Guid.NewGuid(), Description = OfflinePayDescConstants.RegistrationFee };
            var result = _validator.TestValidate(paymentStatusInsertRequestDto);
            result.ShouldHaveValidationErrorFor(x => x.Regulator);
        }

        [TestMethod]
        public void Should_Have_Error_When_Regulator_Is_NotSupported()
        {
            var paymentStatusInsertRequestDto = new OfflinePaymentInsertRequestDto { Regulator = "Test Regulator", Amount = 10, Reference = "Test Reference", UserId = Guid.NewGuid(), Description = OfflinePayDescConstants.RegistrationFee };
            var result = _validator.TestValidate(paymentStatusInsertRequestDto);
            result.ShouldHaveValidationErrorFor(x => x.Regulator);
        }

        [TestMethod]
        public void Should_Have_Error_When_Regulator_Is_Valid()
        {
            var paymentStatusInsertRequestDto = new OfflinePaymentInsertRequestDto { Regulator = RegulatorConstants.GBENG, Amount = 10, Reference = "Test Reference", UserId = Guid.NewGuid(), Description = OfflinePayDescConstants.RegistrationFee };
            var result = _validator.TestValidate(paymentStatusInsertRequestDto);
            result.ShouldNotHaveValidationErrorFor(x => x.Regulator);
        }

        [TestMethod]
        public void Should_Have_Error_When_Description_Is_Empty()
        {
            var paymentStatusInsertRequestDto = new OfflinePaymentInsertRequestDto { Regulator = RegulatorConstants.GBENG, Amount = 10, Reference = "Test Reference", UserId = Guid.NewGuid(), Description = string.Empty };
            var result = _validator.TestValidate(paymentStatusInsertRequestDto);
            result.ShouldHaveValidationErrorFor(x => x.Description);
        }

        [TestMethod]
        public void Should_Have_Error_When_Description_Is_NotSupported()
        {
            var paymentStatusInsertRequestDto = new OfflinePaymentInsertRequestDto { Regulator = RegulatorConstants.GBSCT, Amount = 10, Reference = "Test Reference", UserId = Guid.NewGuid(), Description = "Test Description" };
            var result = _validator.TestValidate(paymentStatusInsertRequestDto);
            result.ShouldHaveValidationErrorFor(x => x.Description);
        }
    }
}
