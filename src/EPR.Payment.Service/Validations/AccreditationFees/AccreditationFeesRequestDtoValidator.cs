using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Enums;
using EPR.Payment.Service.Common.Dtos.Request.AccreditationFees;
using EPR.Payment.Service.Validations.Common;
using EPR.Payment.Service.Common.Constants.Fees;
using FluentValidation;

namespace EPR.Payment.Service.Validations.AccreditationFees
{
    public class AccreditationFeesRequestDtoValidator : AbstractValidator<AccreditationFeesRequestDto>
    {
        public AccreditationFeesRequestDtoValidator()
        {
            RuleFor(x => x.Regulator)
                .NotEmpty().WithMessage(ValidationMessages.ReferenceRequired)
                .Must(RegulatorValidationHelper.IsValidRegulator).WithMessage(ValidationMessages.RegulatorInvalid);

            RuleFor(x => x.SubmissionDate)
                .Cascade(CascadeMode.Stop)
                .MustBeValidSubmissionDate();

            RuleFor(x => x.ApplicationReferenceNumber)
                .NotNull()
                .NotEmpty().WithMessage(ValidationMessages.ApplicationReferenceNumberRequired);

            RuleFor(x => x.TonnageBand)
                .NotNull().WithMessage(ValidationMessages.EmptyTonnageBand)
                .IsInEnum()        
                .WithMessage(ValidationMessages.InvalidTonnageBand + string.Join(",", Enum.GetNames(typeof(TonnageBands))));

            RuleFor(x => x.RequestorType)
                .NotNull().WithMessage(ValidationMessages.EmptyRequestorType)
                .IsInEnum()
                .WithMessage(ValidationMessages.InvalidRequestorType + string.Join(",", Enum.GetNames(typeof(RequestorTypes))));

            RuleFor(x => x.MaterialType)
               .NotNull().WithMessage(ValidationMessages.EmptyMaterialType)
               .IsInEnum()
               .WithMessage(ValidationMessages.InvalidMaterialType + string.Join(",", Enum.GetNames(typeof(MaterialTypes))));

            RuleFor(x => x.NumberOfOverseasSites)
               .GreaterThan(0).When(x => x.RequestorType == RequestorTypes.Exporters)
               .LessThanOrEqualTo(ReprocessorExporterConstants.MaxNumberOfOverseasSitesAllowed).When(x => x.RequestorType == RequestorTypes.Exporters).WithMessage(ValidationMessages.InvalidNumberOfOverseasSiteForExporter);

            RuleFor(x => x.NumberOfOverseasSites)
               .Equal(0).When(x => x.RequestorType == RequestorTypes.Reprocessors).WithMessage(ValidationMessages.InvalidNumberOfOverseasSiteForReprocessor);
        }
    }
}
