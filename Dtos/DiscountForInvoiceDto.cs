using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos
{
    public class DiscountForInvoiceDto
    {
        public DiscountForInvoiceDto()
        {
        }

        public DiscountForInvoiceDto(DiscountDto discount)
        {
            Id = discount.Id;
            Name = discount.Name;
            Value = discount.Value;
        }

        public Guid Id { get; set; }
        public bool IsAdded { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
    }
}
