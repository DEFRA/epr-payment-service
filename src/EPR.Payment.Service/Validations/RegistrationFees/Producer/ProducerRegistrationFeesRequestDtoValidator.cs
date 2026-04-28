using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Validations.Common;
using FluentValidation;

namespace EPR.Payment.Service.Validations.RegistrationFees.Producer
{
    public class ProducerRegistrationFeesRequestDtoValidator : AbstractValidator<ProducerRegistrationFeesRequestDto>
    {
        public ProducerRegistrationFeesRequestDtoValidator()
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

            RuleFor(x => x.NoOfSubsidiariesClosedLoopRecycling)
                .LessThanOrEqualTo(x => x.NumberOfSubsidiaries).WithMessage(ValidationMessages.NumberOfClosedLoopRecyclingSubsidiariesLessThanOrEqualToNumberOfSubsidiaries);

            RuleFor(x => x.NoOfSubsidiariesClosedLoopRecycling)
                .GreaterThanOrEqualTo(0).WithMessage(ValidationMessages.NoOfSubsidiariesClosedLoopRecyclingRange);

            RuleFor(x => x)
                .Must(x => (!x.IsClosedLoopRecycling && x.NoOfSubsidiariesClosedLoopRecycling == 0)
                           || string.Equals(x.ProducerType, "LARGE", StringComparison.OrdinalIgnoreCase))
                .WithMessage(ValidationMessages.ClosedLoopRecyclingNotAllowedForSmall);

            RuleFor(x => x.ApplicationReferenceNumber)
                .NotEmpty().WithMessage(ValidationMessages.ApplicationReferenceNumberRequired);

            RuleFor(x => x.SubmissionDate)
                 .Cascade(CascadeMode.Stop)
                 .MustBeValidSubmissionDate();
        }
    }
}
