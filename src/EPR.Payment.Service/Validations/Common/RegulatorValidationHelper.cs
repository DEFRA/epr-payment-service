﻿using EPR.Payment.Service.Common.Constants.RegistrationFees;

namespace EPR.Payment.Service.Validations.Common
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

        public static bool IsValidRegulator(string? regulator)
        {
            return regulator != null && ValidRegulators.Contains(regulator);
        }
    }
}
