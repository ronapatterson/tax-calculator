using Domain.Entities.TaxJar;
using Application.Dtos.TaxJar;

namespace Application.Mappings.TaxJar
{
    public static class OrderTaxOutputDtoMapping
    {
        public static OrderTaxOutputDto ToOrderTax(this Tax dto) => new OrderTaxOutputDto
        {
            Rate = dto.Rate,
            Shipping = dto.Shipping,
            TaxSource = dto.TaxSource,            
            AmountToCollect = dto.AmountToCollect,
            OrderTotalAmount = dto.OrderTotalAmount,
            TaxableAmount = dto.TaxableAmount
        };
    }
}
