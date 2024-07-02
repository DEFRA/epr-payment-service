using Asp.Versioning;
using EPR.Payment.Service.Common.Dtos.Requests;
using EPR.Payment.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EPR.Payment.Service.Controllers.RegistrationFees
{
    [ApiVersion(1)]
    [ApiController]
    [Route("api/v{version:apiVersion}/complianceschemes")]
    public class ComplianceSchemeController : ControllerBase
    {
        private readonly IComplianceSchemeFeesService _feesService;
        public ComplianceSchemeController(IComplianceSchemeFeesService feesService)
        {
            _feesService = feesService;
        }

        [MapToApiVersion(1)]
        [HttpPost("calculateFees")]
        [ProducesResponseType(typeof(ComplianceSchemeRegistrationRequestDto), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CalculateFeesAsync(ComplianceSchemeRegistrationRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Fees = await _feesService.CalculateFeesAsync(request);

            if (Fees == null)
                return NotFound();

            return Ok(Fees);
        }
    }
}
