using Domain.Entities.TaxJar;
using Application.Dtos.TaxJar;

namespace Application.Mappings.TaxJar
{
    internal static class LocationTaxRateOutputDtoMapping
    {
        public static LocationTaxRateOutputDto ToLocationTaxRateDto(this Rate dto) => new LocationTaxRateOutputDto
        {
            Zip = dto.Zip,
            City = dto.City,
            State = dto.State,
            County = dto.County,
            Country = dto.Country,
            CityRate = dto.CityRate,
            StateRate = dto.StateRate,
            CountyRate = dto.CountyRate,
            CountryRate = dto.CountryRate,
            FreightTaxable = dto.FreightTaxable,
            CombinedRate = dto.CombinedRate,            
            CombinedDistrictRate = dto.CombinedDistrictRate            
        };
    }
}
