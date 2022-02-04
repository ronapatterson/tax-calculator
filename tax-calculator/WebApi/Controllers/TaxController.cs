using Microsoft.AspNetCore.Mvc;

using Application.Interfaces;
using Application.Dtos.TaxJar;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaxController : ControllerBase
    {
        private readonly ITaxService _taxService;

        public TaxController(ITaxService taxService)
        {
            _taxService = taxService;
        }

        [HttpPost("ordertax")]
        [ProducesResponseType(200, Type = typeof(OrderTaxOutputDto))]
        public async Task<IActionResult> Get(OrderTaxParameterDto orderTaxParameter)
        {
            var tax = await _taxService.GetTaxJarOrderTax(orderTaxParameter, HttpContext.RequestAborted);
            if (tax == null) return NotFound();

            return Ok(tax);
        }

        [HttpGet("rate")]
        [ProducesResponseType(200, Type = typeof(LocationTaxRateOutputDto))]
        public async Task<IActionResult> GetLocationTaxRate([FromQuery] LocationTaxRateParameterDto locationTaxRateParameter)
        {
            var locationTaxRate = await _taxService.GetTaxJarLocationTaxRate(locationTaxRateParameter, HttpContext.RequestAborted);
            if (locationTaxRate == null) return NotFound();

            return Ok(locationTaxRate);
        }
    }
}