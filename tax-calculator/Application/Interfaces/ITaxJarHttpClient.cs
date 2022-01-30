using Domain.Entities.TaxJar;
using Application.Dtos.TaxJar;

namespace Application.Interfaces
{
    public interface ITaxJarHttpClient
    {
        Task<Tax> GetOrderTax(TaxForOrderDto dto, CancellationToken cancellationToken);
    }
}
