﻿using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.TaxJar
{
    public class OrderTaxParameterDto
    {
        public string FromCountry { get; set; }
        public string FromZipCode { get; set; }
        public string FromState { get; set; }

        [Required]
        public string ToCountry { get; set; }
        public string ToZip { get; set; }
        public string ToState { get; set; }
        public double Amount { get; set; }

        [Required]
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
