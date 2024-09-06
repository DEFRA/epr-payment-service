using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.ComplianceScheme;
using FluentValidation;

namespace EPR.Payment.Service.Services.RegistrationFees.ComplianceScheme
{
    public class ComplianceSchemeBaseFeeService : IComplianceSchemeBaseFeeService
    {
        private readonly IComplianceSchemeBaseFeeCalculationStrategy _baseFeeCalculationStrategy;
        private readonly IValidator<string> _regulatorValidator;

        public ComplianceSchemeBaseFeeService(
            IComplianceSchemeBaseFeeCalculationStrategy baseFeeCalculationStrategy,
            IValidator<string> regulatorValidator)
        {
            _baseFeeCalculationStrategy = baseFeeCalculationStrategy ?? throw new ArgumentNullException(nameof(baseFeeCalculationStrategy));
            _regulatorValidator = regulatorValidator ?? throw new ArgumentNullException(nameof(regulatorValidator));
        }

        public async Task<decimal> GetComplianceSchemeBaseFeeAsync(string regulator, CancellationToken cancellationToken)
        {
            ValidateRequest(regulator);

            try
            {
                return await _baseFeeCalculationStrategy.CalculateFeeAsync(regulator, cancellationToken);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException(ComplianceSchemeFeeCalculationExceptions.BaseFeeCalculationInvalidOperation, ex);
            }
        }

        private void ValidateRequest(string regulator)
        {
            var validationResult = _regulatorValidator.Validate(regulator);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
        }
    }
}