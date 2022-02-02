using Application.Dtos.TaxJar;

namespace Application.Interfaces
{
    public interface ITaxService
    {
        Task<OrderTaxOutputDto> GetOrderTax(OrderTaxParameterDto dto, CancellationToken cancellationToken);
        Task<LocationTaxRateOutputDto> GetLocationTaxRate(LocationTaxRateParameterDto dto, CancellationToken cancellationToken);
    }
}
