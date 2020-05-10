using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Models
{
    public class InvoiceDiscount : BaseModel
    {
        public double Value { get; set; }
        public Guid InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
        public Guid DiscountId { get; set; }
        public Discount Discount { get; set; }
    }
}
