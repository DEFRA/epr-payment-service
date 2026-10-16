namespace EPR.Payment.Service.Services.Migration.RegistrationHistory
{
    public static class RegistrationHistoryEventTypes
    {
        public const string Submitted = "Submitted";

        public const string RegistrationApplicationSubmitted = "RegistrationApplicationSubmitted";

        public const string RegulatorRegistrationDecision = "RegulatorRegistrationDecision";
    }

    public static class MigrationReasons
    {
        public const string AlreadyCaptured = "AlreadyCaptured";

        public const string PeriodNotMapped = "PeriodNotMapped";

        public const string InvalidData = "InvalidData";

        public const string NoSubmittedFile = "NoSubmittedFile";

        public const string NationMissing = "NationMissing";

        public const string ArnMissing = "ArnMissing";

        public const string BlobNotFound = "BlobNotFound";

        public const string CsvInvalid = "CsvInvalid";

        public const string PostCutoverApproval = "PostCutoverApproval";

        public const string HandlerError = "HandlerError";

        public const string KnownIssue = "KnownIssue";
    }
}
