using Asp.Versioning;
using EPR.Payment.Service.Common.Dtos.Responses;
using EPR.Payment.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EPR.Payment.Service.Controllers
{
    [ApiVersion(1)]
    [ApiController]
    [Route("/api/[controller]")]
    public class AccreditationFeesController : ControllerBase
    {
        private readonly IAccreditationFeesService _accreditationFeesService;

        public AccreditationFeesController(IAccreditationFeesService feesService)
        {
            _accreditationFeesService = feesService ?? throw new ArgumentNullException(nameof(feesService));
        }

        // TODO : MA - This endpoint can be removed later. Remove this endpoint during clean up activity as this may not be needed.
        [MapToApiVersion(1)]
        [HttpGet]
        [Route("GetFees")]
        [ProducesResponseType(typeof(GetAccreditationFeesResponse), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFees(bool isLarge, string regulator)
        {
            var accreditationFees = await _accreditationFeesService.GetFees(isLarge, regulator);

            if (accreditationFees == null)
                return NotFound();

            return Ok(accreditationFees);
        }

        [MapToApiVersion(1)]
        [HttpGet]
        [Route("GetFeesAmount")]
        [ProducesResponseType(typeof(decimal), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFeesAmount(bool isLarge, string regulator)
        {
            var fees = await _accreditationFeesService.GetFeesAmount(isLarge, regulator);

            if (fees == null)
                return NotFound();

            return Ok(fees);
        }
    }
}