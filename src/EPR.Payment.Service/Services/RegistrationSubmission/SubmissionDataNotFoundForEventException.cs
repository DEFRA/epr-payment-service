namespace EPR.Payment.Service.Services.RegistrationSubmission
{
    public class SubmissionDataNotFoundForEventException : Exception
    {
        public SubmissionDataNotFoundForEventException(Guid submissionId, string eventName)
            : base($"No RegistrationSubmissionData row found for SubmissionId '{submissionId}' when recording '{eventName}' event.")
        {
            SubmissionId = submissionId;
            EventName = eventName;
        }

        public Guid SubmissionId { get; }

        public string EventName { get; }
    }
}
