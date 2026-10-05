using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Common.Data.DataModels.Migration
{
    [ExcludeFromCodeCoverage]
    public sealed record MigrationStatusCounts(int Total, int Pending, int Succeeded, int Failed, int NotRequired);

    [ExcludeFromCodeCoverage]
    public sealed record ClearDownResult(int DeletedRegistrationSubmissionDataCount, IReadOnlyList<int> UnlinkedPaymentIds);
}
