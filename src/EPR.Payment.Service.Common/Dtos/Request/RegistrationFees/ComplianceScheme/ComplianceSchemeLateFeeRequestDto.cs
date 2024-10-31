using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;

namespace EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme
{
    public class ComplianceSchemeLateFeeRequestDto
    {
        public bool IsLateFeeApplicable { get; set; }
        public required RegulatorType Regulator { get; set; }
        public required DateTime SubmissionDate { get; set; }
    }
}