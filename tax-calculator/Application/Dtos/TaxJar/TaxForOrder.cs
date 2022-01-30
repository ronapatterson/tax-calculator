namespace Application.Dtos.TaxJar
{
    public class TaxForOrderDto
    {
        public string FromCountry { get; set; }
        public string FromZipCode { get; set; }
        public string FromState { get; set; }
        public string ToCountry { get; set; }
        public string ToZip { get; set; }
        public string ToState { get; set; }
        public double Amount { get; set; }
        public double Shipping { get; set; }
        public IEnumerable<LineItemDto> LineItems { get; set; }
    }

    public class LineItemDto
    {
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public string ProductTaxCode { get; set; }
    }
}
