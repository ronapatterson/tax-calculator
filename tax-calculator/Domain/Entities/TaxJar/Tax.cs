using Newtonsoft.Json;

namespace Domain.Entities.TaxJar
{
    public class Tax
    {
        [JsonProperty("amount_to_collect")]
        public double AmountToCollect { get; set; }
        
        [JsonProperty("order_total_amount")]
        public double OrderTotalAmount { get; set; }
        public double Rate { get; set; }
        public double Shipping { get; set; }
        
        [JsonProperty("tax_source")]
        public string TaxSource { get; set; }

        [JsonProperty("taxable_amount")]
        public double TaxableAmount { get; set; }
    }
}
