using Domain.Entities.TaxJar;
using Application.Dtos.TaxJar;

namespace Application.Mappings.TaxJar
{
    internal static class LocationTaxRateOutputDtoMapping
    {
        public static LocationTaxRateOutputDto ToLocationTaxRateDto(this RateEntity model) => new LocationTaxRateOutputDto
        {
            Zip = model?.Rate?.Zip ?? null,
            City = model?.Rate?.City ?? null,
            State = model?.Rate?.State ?? null,
            County = model?.Rate?.County ?? null,
            Country = model?.Rate?.Country ?? null,
            CityRate = model?.Rate?.CityRate ?? null,
            StateRate = model?.Rate?.StateRate ?? null,
            CountyRate = model?.Rate?.CountyRate ?? null,
            CountryRate = model?.Rate?.CountryRate ?? null,
            FreightTaxable = model?.Rate?.FreightTaxable ?? null,
            CombinedRate = model?.Rate?.CombinedRate ?? null,            
            CombinedDistrictRate = model?.Rate?.CombinedDistrictRate ?? null
        };
    }
}
