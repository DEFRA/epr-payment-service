using EPR.Payment.Service.Common.Dtos.Request.RegistrationSubmission;
using FluentValidation;

namespace EPR.Payment.Service.Validations.RegistrationSubmission
{
    public class CreateRegistrationSubmissionDataRequestValidator : AbstractValidator<CreateRegistrationSubmissionDataRequest>
    {
        public CreateRegistrationSubmissionDataRequestValidator()
        {
            RuleFor(x => x.SubmissionId).NotEqual(Guid.Empty).WithMessage("SubmissionId is required.");
            RuleFor(x => x.FileId).NotEqual(Guid.Empty).WithMessage("FileId is required.");
            RuleFor(x => x.RegistrationBlobName).NotEmpty().WithMessage("RegistrationBlobName is required.");
            RuleFor(x => x.SubmissionPeriod).NotEmpty().WithMessage("SubmissionPeriod is required.");
            RuleFor(x => x.SubmissionDate).NotEqual(default(DateTime)).WithMessage("SubmissionDate is required.");
        }
    }
}
