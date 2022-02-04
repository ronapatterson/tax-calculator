using System.Runtime.CompilerServices;

using Application.Interfaces;
using Application.Dtos.TaxJar;
using Application.Mappings.TaxJar;
using Application.Interfaces.TaxCalculators;

[assembly: InternalsVisibleTo("WebApi")]
namespace Application.Services
{
    internal class TaxService : ITaxService
    {
        private readonly ITaxJarHttpClient _taxJarHttpClient;
        
        public TaxService(ITaxJarHttpClient taxJarHttpClient)
        {
            _taxJarHttpClient = taxJarHttpClient;
        }

        public async Task<OrderTaxOutputDto> GetTaxJarOrderTax(OrderTaxParameterDto dto, CancellationToken cancellationToken)
        {
            var orderTax = await _taxJarHttpClient.GetOrderTax(dto, cancellationToken);
            return orderTax.ToOrderTax();
        }        

        public async Task<LocationTaxRateOutputDto> GetTaxJarLocationTaxRate(LocationTaxRateParameterDto dto, CancellationToken cancellationToken)
        {
            var locationTaxRate = await _taxJarHttpClient.GetLocationTaxRate(dto, cancellationToken);
            return locationTaxRate.ToLocationTaxRateDto();
        }
    }
}
