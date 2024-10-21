﻿using EPR.Payment.Service.Common.Dtos.Request.Payments;
using EPR.Payment.Service.Validations.Payments;
using FluentValidation.TestHelper;

namespace EPR.Payment.Service.UnitTests.Validations.Payments
{
    [TestClass]
    public class PaymentStatusInsertRequestDtoValidatorTests
    {
        private OnlinePaymentStatusInsertRequestDtoValidator _validator = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _validator = new OnlinePaymentStatusInsertRequestDtoValidator();
        }

        [TestMethod]
        public void Should_Have_Error_When_UserId_Is_Null()
        {
            var paymentStatusInsertRequestDto = new OnlinePaymentStatusInsertRequestDto { UserId = null };
            var result = _validator.TestValidate(paymentStatusInsertRequestDto);
            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_UserId_Is_Valid()
        {
            var paymentStatusInsertRequestDto = new OnlinePaymentStatusInsertRequestDto { UserId = Guid.NewGuid() };
            var result = _validator.TestValidate(paymentStatusInsertRequestDto);
            result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        }

        [TestMethod]
        public void Should_Have_Error_When_OrganisationId_Is_Null()
        {
            var paymentStatusInsertRequestDto = new OnlinePaymentStatusInsertRequestDto { OrganisationId = null };
            var result = _validator.TestValidate(paymentStatusInsertRequestDto);
            result.ShouldHaveValidationErrorFor(x => x.OrganisationId);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_OrganisationId_Is_Valid()
        {
            var paymentStatusInsertRequestDto = new OnlinePaymentStatusInsertRequestDto { OrganisationId = Guid.NewGuid() };
            var result = _validator.TestValidate(paymentStatusInsertRequestDto);
            result.ShouldNotHaveValidationErrorFor(x => x.OrganisationId);
        }

        [TestMethod]
        public void Should_Have_Error_When_Reference_Is_Empty()
        {
            var paymentStatusInsertRequestDto = new OnlinePaymentStatusInsertRequestDto { Reference = string.Empty };
            var result = _validator.TestValidate(paymentStatusInsertRequestDto);
            result.ShouldHaveValidationErrorFor(x => x.Reference);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_Reference_Is_Valid()
        {
            var paymentStatusInsertRequestDto = new OnlinePaymentStatusInsertRequestDto { Reference = "Test Reference" };
            var result = _validator.TestValidate(paymentStatusInsertRequestDto);
            result.ShouldNotHaveValidationErrorFor(x => x.Reference);
        }

        [TestMethod]
        public void Should_Have_Error_When_ReasonForPayment_Is_Empty()
        {
            var paymentStatusInsertRequestDto = new OnlinePaymentStatusInsertRequestDto { ReasonForPayment = string.Empty };
            var result = _validator.TestValidate(paymentStatusInsertRequestDto);
            result.ShouldHaveValidationErrorFor(x => x.ReasonForPayment);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_ReasonForPayment_Is_Valid()
        {
            var paymentStatusInsertRequestDto = new OnlinePaymentStatusInsertRequestDto { ReasonForPayment = "Test ReasonForPayment" };
            var result = _validator.TestValidate(paymentStatusInsertRequestDto);
            result.ShouldNotHaveValidationErrorFor(x => x.ReasonForPayment);
        }

        [TestMethod]
        public void Should_Have_Error_When_Amount_Is_Null()
        {
            var paymentStatusInsertRequestDto = new OnlinePaymentStatusInsertRequestDto { Amount = null };
            var result = _validator.TestValidate(paymentStatusInsertRequestDto);
            result.ShouldHaveValidationErrorFor(x => x.Amount);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_Amount_Is_Valid()
        {
            var paymentStatusInsertRequestDto = new OnlinePaymentStatusInsertRequestDto { Amount = 10 };
            var result = _validator.TestValidate(paymentStatusInsertRequestDto);
            result.ShouldNotHaveValidationErrorFor(x => x.Amount);
        }

        [TestMethod]
        public void Should_Have_Error_When_Status_Is_Not_InEnum()
        {
            var paymentStatusInsertRequestDto = new OnlinePaymentStatusInsertRequestDto { Status = (Service.Common.Dtos.Enums.Status)999 };
            var result = _validator.TestValidate(paymentStatusInsertRequestDto);
            result.ShouldHaveValidationErrorFor(x => x.Status);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_Status_Is_Valid()
        {
            var paymentStatusInsertRequestDto = new OnlinePaymentStatusInsertRequestDto { Status = Service.Common.Dtos.Enums.Status.Initiated };
            var result = _validator.TestValidate(paymentStatusInsertRequestDto);
            result.ShouldNotHaveValidationErrorFor(x => x.Status);
        }
    }
}
