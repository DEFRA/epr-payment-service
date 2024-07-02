using Asp.Versioning;
using EPR.Payment.Service.Common.Dtos.Requests;
using EPR.Payment.Service.Common.Dtos.Responses;
using EPR.Payment.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EPR.Payment.Service.Controllers.RegistrationFees
{
    [ApiVersion(1)]
    [ApiController]
    [Route("api/v{version:apiVersion}/producers")]
    public class ProducerFeesController : ControllerBase
    {
        private readonly IProducerFeesService _FeesService;

        public ProducerFeesController(IProducerFeesService feesService)
        {
            _FeesService = feesService ?? throw new ArgumentNullException(nameof(feesService));
        }

        [MapToApiVersion(1)]
        [HttpPost("calculateFees")]
        [ProducesResponseType(typeof(RegistrationFeeResponseDto), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RegistrationFeeResponseDto>> CalculateFees(ProducerRegistrationRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(request.ProducerType != "L" &&  request.ProducerType != "S") 
            {
                return BadRequest("ProducerType must be L or S");
            }
            if (request.NumberOfSubsidiaries < 0 || request.NumberOfSubsidiaries > 100)
            {
                return BadRequest("NumberOfSubsidiaries must be greater than 1 and less than 100");
            }

            var result = await _FeesService.CalculateFeesAsync(request);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}