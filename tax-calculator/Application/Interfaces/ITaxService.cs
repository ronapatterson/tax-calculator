using Application.Dtos.TaxJar;

namespace Application.Interfaces
{
    public interface ITaxService
    {
        Task<OrderTaxOutputDto> GetTaxJarOrderTax(OrderTaxParameterDto dto, CancellationToken cancellationToken);
        Task<LocationTaxRateOutputDto> GetTaxJarLocationTaxRate(LocationTaxRateParameterDto dto, CancellationToken cancellationToken);
    }
}
