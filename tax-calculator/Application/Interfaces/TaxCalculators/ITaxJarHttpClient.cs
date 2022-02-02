using Domain.Entities.TaxJar;
using Application.Dtos.TaxJar;

namespace Application.Interfaces.TaxCalculators
{
    internal interface ITaxJarHttpClient
    {
        Task<Tax> GetOrderTax(OrderTaxParameterDto dto, CancellationToken cancellationToken);
        Task<Rate> GetLocationTaxRate(LocationTaxRateParameterDto dto, CancellationToken cancellationToken);
    }
}
