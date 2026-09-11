using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Common.Data.DataModels
{
    [ExcludeFromCodeCoverage]
    public class RegistrationFeeSnapshot
    {
        public Guid Id { get; set; }

        public Guid RegistrationSubmissionDataId { get; set; }

        public decimal TotalFee { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public virtual RegistrationSubmissionData RegistrationSubmissionData { get; set; } = null!;

        public ICollection<RegistrationFeeLineItem> LineItems { get; set; } = new List<RegistrationFeeLineItem>();
    }
}
