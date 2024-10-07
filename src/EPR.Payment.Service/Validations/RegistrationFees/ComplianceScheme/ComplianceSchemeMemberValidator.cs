using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using FluentValidation;

namespace EPR.Payment.Service.Validations.RegistrationFees.ComplianceScheme
{
    public class ComplianceSchemeMemberDtoValidator : AbstractValidator<ComplianceSchemeMemberDto>
    {
        public ComplianceSchemeMemberDtoValidator()
        {
            RuleFor(x => x.MemberId)
                .GreaterThan(0)
                .WithMessage(ValidationMessages.InvalidMemberId);

            RuleFor(x => x.MemberType)
                .NotEmpty()
                .WithMessage(ValidationMessages.MemberTypeRequired)
                .Must(type => type == "Large" || type == "Small")
                .WithMessage(ValidationMessages.InvalidMemberType);

            RuleFor(x => x.NumberOfSubsidiaries)
                .GreaterThanOrEqualTo(0)
                .WithMessage(ValidationMessages.NumberOfSubsidiariesRange);

            RuleFor(x => x.NoOfSubsidiariesOnlineMarketplace)
                .GreaterThanOrEqualTo(0)
                .WithMessage(ValidationMessages.NoOfSubsidiariesOnlineMarketplaceRange);
        }
    }
}
