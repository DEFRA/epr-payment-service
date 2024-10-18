namespace EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions
{
    public static class ProducerResubmissionExceptions
    {
        public const string RecordNotFoundProducerResubmissionFeeError = "Producer Resubmission Registration Fee record not found for this regulator: '{0}'";
        public const string RegulatorCanNotBeNullOrEmpty = "regulator cannot be null or empty";
        public const string Status500InternalServerError = "Internal server error";
    }
}