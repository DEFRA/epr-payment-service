using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Enums;
using EPR.Payment.Service.Common.Dtos.Request.AccreditationFees;
using EPR.Payment.Service.Validations.Common;
using FluentValidation;
using EPR.Payment.Service.Helper;

namespace EPR.Payment.Service.Validations.AccreditationFees
{
    public class AccreditationFeesRequestDtoValidator : AbstractValidator<AccreditationFeesRequestDto>
    {
        public AccreditationFeesRequestDtoValidator()
        {
            RuleFor(x => x.Regulator.ToString())
                .NotNull()
                .NotEmpty().WithMessage(ValidationMessages.RegulatorRequired)
                .Must(RegulatorValidationHelper.IsValidRegulator).WithMessage(ValidationMessages.RegulatorInvalid);

            RuleFor(x => x.SubmissionDate)
                .Cascade(CascadeMode.Stop)
                .MustBeValidSubmissionDate();

            RuleFor(x => x.ApplicationReferenceNumber)
                .NotEmpty().WithMessage(ValidationMessages.ApplicationReferenceNumberRequired);

            RuleFor(x => x.TonnageBand)
                .NotNull()
                .IsInEnum()        
                .WithMessage(ValidationMessages.InvalidTonnageBand + string.Join(",", Enum.GetNames(typeof(TonnageBand))));

            RuleFor(x => x.RequestorType)
                .NotNull()                
                .WithMessage(ValidationMessages.InvalidRequestType + string.Join(",", Enum.GetNames(typeof(AccreditationFeesMaterialType))));

            RuleFor(x => x.MaterialType)
               .NotNull()
               .IsInEnum()
               .WithMessage(ValidationMessages.InvalidMaterialType + string.Join(",", Enum.GetNames(typeof(AccreditationFeesMaterialType))));

            RuleFor(x => x.NumberOfOverseasSites)                
               .GreaterThan(0).When(x => x.RequestorType == AccreditationFeesRequestorType.Exporters).WithMessage(ValidationMessages.InvalidNumberOfOverseasSiteForExporter);

            RuleFor(x => x.NumberOfOverseasSites)
               .Equal(0).When(x => x.RequestorType == AccreditationFeesRequestorType.Reprocessors).WithMessage(ValidationMessages.InvalidNumberOfOverseasSiteForReprocessor);
        }
    }
}
