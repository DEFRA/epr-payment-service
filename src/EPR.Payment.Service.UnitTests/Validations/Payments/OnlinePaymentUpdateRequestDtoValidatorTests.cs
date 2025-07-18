﻿using EPR.Payment.Service.Common.Dtos.Request.Payments;
using EPR.Payment.Service.Validations;
using FluentValidation.TestHelper;

namespace EPR.Payment.Service.UnitTests.Validations.Payments
{
    [TestClass]
    public class OnlinePaymentUpdateRequestDtoValidatorTests
    {
        private OnlinePaymentUpdateRequestDtoValidator _validator = null!;
        [TestInitialize]
        public void TestInitialize()
        {
            _validator = new OnlinePaymentUpdateRequestDtoValidator();
        }

        [TestMethod]
        public void Should_Have_Error_When_GovPayPaymentId_Is_Empty()
        {
            var paymentStatusUpdateRequestDto = new OnlinePaymentUpdateRequestDto { GovPayPaymentId = string.Empty };
            var result = _validator.TestValidate(paymentStatusUpdateRequestDto);
            result.ShouldHaveValidationErrorFor(x => x.GovPayPaymentId);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_GovPayPaymentId_Is_Valid()
        {
            var paymentStatusUpdateRequestDto = new OnlinePaymentUpdateRequestDto { GovPayPaymentId = "Test GovPayPaymentId" };
            var result = _validator.TestValidate(paymentStatusUpdateRequestDto);
            result.ShouldNotHaveValidationErrorFor(x => x.GovPayPaymentId);
        }

        [TestMethod]
        public void Should_Have_Error_When_UpdatedByUserId_Is_Null()
        {
            var paymentStatusUpdateRequestDto = new OnlinePaymentUpdateRequestDto { UpdatedByUserId = null };
            var result = _validator.TestValidate(paymentStatusUpdateRequestDto);
            result.ShouldHaveValidationErrorFor(x => x.UpdatedByUserId);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_UpdatedByUserId_Is_Valid()
        {
            var paymentStatusUpdateRequestDto = new OnlinePaymentUpdateRequestDto { UpdatedByUserId = Guid.NewGuid() };
            var result = _validator.TestValidate(paymentStatusUpdateRequestDto);
            result.ShouldNotHaveValidationErrorFor(x => x.UpdatedByUserId);
        }

        [TestMethod]
        public void Should_Have_Error_When_UpdatedByOrganisationId_Is_Null()
        {
            var paymentStatusUpdateRequestDto = new OnlinePaymentUpdateRequestDto { UpdatedByOrganisationId = null };
            var result = _validator.TestValidate(paymentStatusUpdateRequestDto);
            result.ShouldHaveValidationErrorFor(x => x.UpdatedByOrganisationId);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_UpdatedByOrganisationId_Is_Valid()
        {
            var paymentStatusUpdateRequestDto = new OnlinePaymentUpdateRequestDto { UpdatedByOrganisationId = Guid.NewGuid() };
            var result = _validator.TestValidate(paymentStatusUpdateRequestDto);
            result.ShouldNotHaveValidationErrorFor(x => x.UpdatedByOrganisationId);
        }

        [TestMethod]
        public void Should_Have_Error_When_Reference_Is_Empty()
        {
            var paymentStatusUpdateRequestDto = new OnlinePaymentUpdateRequestDto { Reference = string.Empty };
            var result = _validator.TestValidate(paymentStatusUpdateRequestDto);
            result.ShouldHaveValidationErrorFor(x => x.Reference);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_Reference_Is_Valid()
        {
            var paymentStatusUpdateRequestDto = new OnlinePaymentUpdateRequestDto { Reference = "Test Reference" };
            var result = _validator.TestValidate(paymentStatusUpdateRequestDto);
            result.ShouldNotHaveValidationErrorFor(x => x.Reference);
        }

        [TestMethod]
        public void Should_Have_Error_When_Status_Is_Not_InEnum()
        {
            var paymentStatusUpdateRequestDto = new OnlinePaymentUpdateRequestDto { Status = (Service.Common.Enums.Status)999 };
            var result = _validator.TestValidate(paymentStatusUpdateRequestDto);
            result.ShouldHaveValidationErrorFor(x => x.Status);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_Status_Is_Valid()
        {
            var paymentStatusUpdateRequestDto = new OnlinePaymentUpdateRequestDto { Status = Service.Common.Enums.Status.Initiated };
            var result = _validator.TestValidate(paymentStatusUpdateRequestDto);
            result.ShouldNotHaveValidationErrorFor(x => x.Status);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_ErrorCode_Is_Null()
        {
            var paymentStatusUpdateRequestDto = new OnlinePaymentUpdateRequestDto { ErrorCode = null };
            var result = _validator.TestValidate(paymentStatusUpdateRequestDto);
            result.ShouldNotHaveValidationErrorFor(x => x.ErrorCode);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_ErrorCode_Is_Empty()
        {
            var paymentStatusUpdateRequestDto = new OnlinePaymentUpdateRequestDto { ErrorCode = string.Empty };
            var result = _validator.TestValidate(paymentStatusUpdateRequestDto);
            result.ShouldNotHaveValidationErrorFor(x => x.ErrorCode);
        }
    }
}
