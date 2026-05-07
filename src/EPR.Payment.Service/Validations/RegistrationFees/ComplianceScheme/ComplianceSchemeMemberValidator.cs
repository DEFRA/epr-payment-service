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

            RuleFor(x => x.NoOfSubsidiariesClosedLoopRecycling)
                .LessThanOrEqualTo(x => x.NumberOfSubsidiaries)
                .WithMessage(ValidationMessages.NumberOfClosedLoopRecyclingSubsidiariesLessThanOrEqualToNumberOfSubsidiaries);

            RuleFor(x => x.NoOfSubsidiariesClosedLoopRecycling)
                .GreaterThanOrEqualTo(0)
                .WithMessage(ValidationMessages.NoOfSubsidiariesClosedLoopRecyclingRange);

            RuleFor(x => x)
                .Must(x => (!x.IsClosedLoopRecycling && x.NoOfSubsidiariesClosedLoopRecycling == 0)
                           || string.Equals(x.MemberType, "LARGE", StringComparison.OrdinalIgnoreCase))
                .WithMessage(ValidationMessages.ClosedLoopRecyclingNotAllowedForSmall);
        }
    }
}
