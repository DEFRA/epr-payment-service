using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Fees;
using EPR.Payment.Service.Common.Dtos.Request.AccreditationFees;
using EPR.Payment.Service.Common.Dtos.Response.AccreditationFees;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Helper;
using EPR.Payment.Service.Services.Interfaces.AccreditationFees;

namespace EPR.Payment.Service.Services.AccreditationFees
{
    public class AccreditationFeesCalculatorService : IAccreditationFeesCalculatorService
    {   
        private readonly IAccreditationFeesRepository _accreditationFeesRepository;

        public AccreditationFeesCalculatorService(IAccreditationFeesRepository accreditationFeesRepository)
        {
            _accreditationFeesRepository = accreditationFeesRepository ?? throw new ArgumentNullException(nameof(accreditationFeesRepository));       
        }

        public async Task<AccreditationFeesResponseDto> CalculateFeesAsync(AccreditationFeesRequestDto request, CancellationToken cancellationToken)
        {   
            var regulatorType = RegulatorType.Create(request.Regulator);
            var tonnageValues = TonnageHelper.GetTonnageBoundaryByTonnageBand(request.TonnageBand);

            var entity = await _accreditationFeesRepository.GetFeeAsync(
                (int)request.RequestType,
                (int)request.MaterialType,
                tonnageValues.Item1,
                tonnageValues.Item2,
                regulatorType,
                request.SubmissionDate,
                cancellationToken
             );

            var response = new AccreditationFeesResponseDto
            {
               OverseasSiteChargePerSite  = entity.FeesPerSite,


            };

            return response;
        }
    }
}