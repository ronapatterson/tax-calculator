using Domain.Entities.TaxJar;
using Application.Dtos.TaxJar;

namespace Application.Mappings.TaxJar
{
    internal static class OrderTaxOutputDtoMapping
    {
        public static OrderTaxOutputDto ToOrderTax(this TaxEntity model) => new OrderTaxOutputDto()
        {
            Rate = model?.Tax?.Rate ?? 0,
            Shipping = model?.Tax?.Shipping ?? 0,
            TaxSource = model?.Tax?.TaxSource ?? null,
            AmountToCollect = model?.Tax?.AmountToCollect ?? 0,
            OrderTotalAmount = model?.Tax?.OrderTotalAmount ?? 0,
            TaxableAmount = model?.Tax?.TaxableAmount ?? 0
        };
    }
}
