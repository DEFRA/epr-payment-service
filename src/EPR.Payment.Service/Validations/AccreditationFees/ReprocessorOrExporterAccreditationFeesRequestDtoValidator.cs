using EPR.Payment.Service.Common.Constants.Fees;
using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.AccreditationFees;
using EPR.Payment.Service.Common.Enums;
using EPR.Payment.Service.Validations.RegistrationFees.ReprocessorOrExporter;
using FluentValidation;

namespace EPR.Payment.Service.Validations.AccreditationFees
{
    public class ReprocessorOrExporterAccreditationFeesRequestDtoValidator : ReprocessorOrExporterFeesRequestDtoCommonValidator<ReprocessorOrExporterAccreditationFeesRequestDto>
    {
        public ReprocessorOrExporterAccreditationFeesRequestDtoValidator()
        {
            RuleFor(x => x.TonnageBand)
                .NotNull().WithMessage(ValidationMessages.EmptyTonnageBand)
                .IsInEnum()        
                .WithMessage(ValidationMessages.InvalidTonnageBand + string.Join(",", Enum.GetNames(typeof(TonnageBands))));

            RuleFor(x => x.NumberOfOverseasSites)
               .GreaterThan(0).When(x => x.RequestorType == RequestorTypes.Exporters)
               .LessThanOrEqualTo(ReprocessorExporterConstants.MaxNumberOfOverseasSitesAllowed).When(x => x.RequestorType == RequestorTypes.Exporters).WithMessage(ValidationMessages.InvalidNumberOfOverseasSiteForExporter);

            RuleFor(x => x.NumberOfOverseasSites)
               .Equal(0).When(x => x.RequestorType == RequestorTypes.Reprocessors).WithMessage(ValidationMessages.InvalidNumberOfOverseasSiteForReprocessor);
        }
    }
}
