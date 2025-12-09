using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using FluentValidation;

namespace EPR.Payment.Service.Validations.RegistrationFees.ComplianceScheme
{
    public class ComplianceSchemeMemberDtoValidator : AbstractValidator<ComplianceSchemeMemberDto>
    {
        public ComplianceSchemeMemberDtoValidator()
        {
            var validMemberTypes = new List<string> { "LARGE", "SMALL" };

            RuleFor(x => x.MemberId)
                .NotEmpty()
                .WithMessage(ValidationMessages.InvalidMemberId);

            RuleFor(x => x.MemberType)
                .NotEmpty()
                .WithMessage(ValidationMessages.MemberTypeRequired)
                .Must(pt => validMemberTypes.Contains(pt.ToUpper()))
                .WithMessage(ValidationMessages.InvalidMemberType + string.Join(", ", validMemberTypes));

            RuleFor(x => x.NumberOfSubsidiaries)
                .GreaterThanOrEqualTo(0)
                .WithMessage(ValidationMessages.NumberOfSubsidiariesRange);

            RuleFor(x => x.NoOfSubsidiariesOnlineMarketplace)
                .LessThanOrEqualTo(x => x.NumberOfSubsidiaries).WithMessage(ValidationMessages.NumberOfOMPSubsidiariesLessThanOrEqualToNumberOfSubsidiaries);

            RuleFor(x => x.NoOfSubsidiariesOnlineMarketplace)
                .GreaterThanOrEqualTo(0)
                .WithMessage(ValidationMessages.NoOfSubsidiariesOnlineMarketplaceRange);

            When(x => x.IsLateFeeApplicable, () =>
            {
                RuleFor(x => x.NumberOfLateSubsidiaries)
                    .Equal(0)
                    .WithMessage(ValidationMessages.LateFeeMutualExclusion);
            })
            .Otherwise(() =>
            {
                RuleFor(x => x.NumberOfLateSubsidiaries)
                    .GreaterThanOrEqualTo(0)
                    .WithMessage(ValidationMessages.NumberOfLateSubsidiariesRange);
            });
        }
    }
}
