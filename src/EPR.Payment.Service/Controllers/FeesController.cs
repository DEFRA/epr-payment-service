using Asp.Versioning;
using EPR.Payment.Service.Common.Dtos.Responses;
using EPR.Payment.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EPR.Payment.Service.Controllers
{
    [ApiVersion(1)]
    [ApiController]
    [Route("/api/[controller]")]
    public class FeesController : ControllerBase
    {
        private readonly IFeesService _feesService;

        public FeesController(IFeesService feesService)
        {
            _feesService = feesService ?? throw new ArgumentNullException(nameof(feesService));
        }

        // TODO : MA - This endpoint can be removed later. Remove this endpoint during clean up activity as this may not be needed.
        [MapToApiVersion(1)]
        [HttpGet]
        [Route("GetFees")]
        [ProducesResponseType(typeof(GetFeesResponse), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFees(bool isLarge, string regulator)
        {
            var fees = await _feesService.GetFees(isLarge, regulator);

            if (fees == null)
                return NotFound();

            return Ok(fees);
        }

        [MapToApiVersion(1)]
        [HttpGet]
        [Route("GetFeesAmount")]
        [ProducesResponseType(typeof(decimal), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFeesAmount(bool isLarge, string regulator)
        {
            var fees = await _feesService.GetFeesAmount(isLarge, regulator);

            if (fees == null)
                return NotFound();

            return Ok(fees);
        }
    }
}