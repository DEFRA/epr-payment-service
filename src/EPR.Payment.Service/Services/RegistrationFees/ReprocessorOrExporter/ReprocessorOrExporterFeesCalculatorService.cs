using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ReprocessorOrExporter;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ReprocessorOrExporter;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.ReprocessorOrExporter;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.ReprocessorOrExporter;
using FluentValidation;

namespace EPR.Payment.Service.Services.RegistrationFees.ReprocessorOrExporter
{
    public class ReprocessorOrExporterFeesCalculatorService: IReprocessorOrExporterFeesCalculatorService
    {
        private readonly IValidator<ReprocessorOrExporterRegistrationFeesRequestDto> _validator;
        private readonly IBaseFeeCalculationStrategy _feeCalculationStrategy;

        public ReprocessorOrExporterFeesCalculatorService(
            IValidator<ReprocessorOrExporterRegistrationFeesRequestDto> validator,
            IBaseFeeCalculationStrategy feeCalculationStrategy)
        {
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _feeCalculationStrategy = feeCalculationStrategy ?? throw new ArgumentNullException(nameof(feeCalculationStrategy));
        }

        public async Task<ReprocessorOrExporterRegistrationFeesResponseDto> CalculateFeesAsync(ReprocessorOrExporterRegistrationFeesRequestDto request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);
            var response = new ReprocessorOrExporterRegistrationFeesResponseDto
            {
                MaterialType = request.MaterialType
            };

            response.RegistrationFee = await _feeCalculationStrategy.CalculateFeeAsync(request, cancellationToken);

            return response;
        }

        private void ValidateRequest(ReprocessorOrExporterRegistrationFeesRequestDto request)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
        }
    }
}
