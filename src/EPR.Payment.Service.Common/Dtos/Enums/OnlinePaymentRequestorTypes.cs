using EPR.Payment.Service.Common.Enums;

namespace EPR.Payment.Service.Common.Dtos.Enums
{
    public enum OnlinePaymentRequestorTypes
    {
        ProducerType = Group.ProducerType,
        ComplianceScheme = Group.ComplianceScheme,
        ProducerSubsidiaries = Group.ProducerSubsidiaries,
        ComplianceSchemeSubsidiaries = Group.ComplianceSchemeSubsidiaries,
        ProducerResubmission = Group.ProducerResubmission,
        ComplianceSchemeResubmission = Group.ComplianceSchemeResubmission,
        Exporters = Group.Exporters,
        Reprocessors = Group.Reprocessors,
    }
}
