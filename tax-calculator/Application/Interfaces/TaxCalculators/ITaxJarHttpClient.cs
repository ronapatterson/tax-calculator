using Domain.Entities.TaxJar;
using Application.Dtos.TaxJar;

namespace Application.Interfaces.TaxCalculators
{
    public interface ITaxJarHttpClient
    {
        Task<TaxEntity> GetOrderTax(OrderTaxParameterDto dto, CancellationToken cancellationToken);
        Task<RateEntity> GetLocationTaxRate(LocationTaxRateParameterDto dto, CancellationToken cancellationToken);
    }
}
