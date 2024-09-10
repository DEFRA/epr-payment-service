using EPR.Payment.Service.Common.Constants.RegistrationFees;

namespace EPR.Payment.Service.Validations.RegistrationFees
{
    public static class RegulatorValidationHelper
    {
        private static readonly List<string> ValidRegulators = new List<string>
        {
            RegulatorConstants.GBENG,
            RegulatorConstants.GBSCT,
            RegulatorConstants.GBWLS,
            RegulatorConstants.GBNIR
        };

        public static bool IsValidRegulator(string regulator)
        {
            return ValidRegulators.Contains(regulator);
        }
    }
}
