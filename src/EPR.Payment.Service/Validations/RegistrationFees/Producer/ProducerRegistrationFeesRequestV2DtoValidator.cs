using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Validations.Common;
using FluentValidation;

namespace EPR.Payment.Service.Validations.RegistrationFees.Producer
{
    public class ProducerRegistrationFeesRequestV2DtoValidator : AbstractValidator<ProducerRegistrationFeesRequestV2Dto>
    {
        public ProducerRegistrationFeesRequestV2DtoValidator()
        {

            var validProducerTypes = new List<string> { "LARGE", "SMALL" };

            RuleFor(x => x.ProducerType)
                .Must(pt => validProducerTypes.Contains(pt.ToUpper()))
                .WithMessage(ValidationMessages.ProducerTypeInvalid + string.Join(", ", validProducerTypes));

            RuleFor(x => x.NumberOfSubsidiaries)
                .GreaterThanOrEqualTo(0).WithMessage(ValidationMessages.NumberOfSubsidiariesRange);

            RuleFor(x => x.NoOfSubsidiariesOnlineMarketplace)
                .LessThanOrEqualTo(x => x.NumberOfSubsidiaries).WithMessage(ValidationMessages.NumberOfOMPSubsidiariesLessThanOrEqualToNumberOfSubsidiaries);

            RuleFor(x => x.Regulator)
                .NotEmpty().WithMessage(ValidationMessages.RegulatorRequired)
                .Must(RegulatorValidationHelper.IsValidRegulator).WithMessage(ValidationMessages.RegulatorInvalid);

            RuleFor(x => x.NoOfSubsidiariesOnlineMarketplace)
                .GreaterThanOrEqualTo(0).WithMessage(ValidationMessages.NoOfSubsidiariesOnlineMarketplaceRange);

            RuleFor(x => x.ApplicationReferenceNumber)
                .NotEmpty().WithMessage(ValidationMessages.ApplicationReferenceNumberRequired);

            RuleFor(x => x.SubmissionDate)
                .Cascade(CascadeMode.Stop)
                .MustBeValidSubmissionDate();

            RuleFor(x => x.FileId)
                    .NotEmpty().WithMessage(ValidationMessages.FileIdRequired);

            RuleFor(x => x.ExternalId)
                    .NotEmpty().WithMessage(ValidationMessages.ExternalIdRequired);

            RuleFor(x => x.InvoicePeriod)
                    .NotEmpty().WithMessage(ValidationMessages.InvoicePeriodRequired);

            RuleFor(x => x.PayerTypeId)
                    .LessThan(1).WithMessage(ValidationMessages.PayerTypeIdRequired);

            RuleFor(x => x.PayerId)
                    .LessThan(1).WithMessage(ValidationMessages.PayerIdRequired);
        }
    }
}
