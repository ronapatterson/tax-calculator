using Newtonsoft.Json;

namespace Domain.Entities.TaxJar
{
    public class RateEntity
    {
        public Rate Rate { get; set; }
    }

    public class Rate
    {
        public string City { get; set; }
        
        [JsonProperty("city_rate")]
        public string CityRate { get; set; }

        [JsonProperty("combined_district_rate")]
        public string CombinedDistrictRate { get; set; }

        [JsonProperty("combined_rate")]
        public string CombinedRate { get; set; }

        public string Country { get; set; }

        [JsonProperty("country_rate")]
        public string CountryRate { get; set; }

        public string County { get; set; }

        [JsonProperty("County_Rate")]
        public string CountyRate { get; set; }

        [JsonProperty("freight_taxable")]
        public bool? FreightTaxable { get; set; }

        public string State { get; set; }

        [JsonProperty("state_rate")]
        public string StateRate { get; set; }

        public string Zip { get; set; }
    }
}
