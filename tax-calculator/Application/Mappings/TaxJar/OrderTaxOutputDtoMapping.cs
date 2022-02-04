using Domain.Entities.TaxJar;
using Application.Dtos.TaxJar;

namespace Application.Mappings.TaxJar
{
    internal static class OrderTaxOutputDtoMapping
    {
        public static OrderTaxOutputDto ToOrderTax(this TaxEntity dto) => new OrderTaxOutputDto
        {
            Rate = dto.Tax.Rate,
            Shipping = dto.Tax.Shipping,
            TaxSource = dto.Tax.TaxSource,            
            AmountToCollect = dto.Tax.AmountToCollect,
            OrderTotalAmount = dto.Tax.OrderTotalAmount,
            TaxableAmount = dto.Tax.TaxableAmount
        };
    }
}
