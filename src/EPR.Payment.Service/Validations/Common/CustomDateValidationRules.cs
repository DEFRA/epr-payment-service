using EPR.Payment.Service.Common.Constants.RegistrationFees;
using FluentValidation;

namespace EPR.Payment.Service.Validations.Common
{
    public static class CustomDateValidationRules
    {

        public static IRuleBuilderOptions<T, DateTime> MustBeValidSubmissionDate<T>(this IRuleBuilder<T, DateTime> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage(ValidationMessages.InvalidSubmissionDate)
                .Must(BeInUtc).WithMessage(ValidationMessages.SubmissionDateMustBeUtc);
        }


        public static IRuleBuilderOptions<T, DateTime> MustBeValidResubmissionDate<T>(this IRuleBuilder<T, DateTime> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage(ValidationMessages.ResubmissionDateRequired)
                .Must(BeInUtc).WithMessage(ValidationMessages.ResubmissionDateMustBeUtc);
        }

        private static bool BeInUtc(DateTime dateTime)
        {
            return dateTime.Kind == DateTimeKind.Utc;
        }
    }
}
