using Domain.Entities.TaxJar;
using Application.Dtos.TaxJar;

namespace Application.Mappings.TaxJar
{
    internal static class LocationTaxRateOutputDtoMapping
    {
        public static LocationTaxRateOutputDto ToLocationTaxRateDto(this RateEntity dto) => new LocationTaxRateOutputDto
        {
            Zip = dto.Rate.Zip,
            City = dto.Rate.City,
            State = dto.Rate.State,
            County = dto.Rate.County,
            Country = dto.Rate.Country,
            CityRate = dto.Rate.CityRate,
            StateRate = dto.Rate.StateRate,
            CountyRate = dto.Rate.CountyRate,
            CountryRate = dto.Rate.CountryRate,
            FreightTaxable = dto.Rate.FreightTaxable,
            CombinedRate = dto.Rate.CombinedRate,            
            CombinedDistrictRate = dto.Rate.CombinedDistrictRate            
        };
    }
}
