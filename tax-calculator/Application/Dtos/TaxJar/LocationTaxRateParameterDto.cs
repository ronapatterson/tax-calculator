using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.TaxJar
{
    public class LocationTaxRateParameterDto
    {        
        public string Country { get; set; }
        public string City { get; set; }
        public string? Street { get; set; }

        [Required]
        public string Zip { get; set; }
    }
}
