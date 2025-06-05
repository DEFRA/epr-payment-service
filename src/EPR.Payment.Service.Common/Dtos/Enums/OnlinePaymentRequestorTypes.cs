using EPR.Payment.Service.Common.Enums;

namespace EPR.Payment.Service.Common.Dtos.Enums
{
    public enum OnlinePaymentRequestorTypes
    {
        ProducerType = Group.ProducerType,
        ComplianceScheme = Group.ComplianceScheme,
        Exporters = Group.Exporters,
        Reprocessors = Group.Reprocessors,
    }
}
