namespace Application.Dtos.TaxJar
{
    public class OrderTaxOutputDto
    {
        public double AmountToCollect { get; set; }
        public double OrderTotalAmount { get; set; }
        public double Rate { get; set; }
        public double Shipping { get; set; }
        public string TaxSource { get; set; }
        public double TaxableAmount { get; set; }
    }
}
