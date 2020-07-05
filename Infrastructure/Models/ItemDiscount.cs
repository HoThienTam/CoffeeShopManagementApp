
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Models
{
    public class ItemDiscount : BaseModel
    {
        public Guid ItemId { get; set; }
        public Item Item { get; set; }
        public Guid DiscountId { get; set; }
        public Discount Discount { get; set; }
        public Guid InvoiceItemId { get; set; }
        public double Value { get; set; }
    }
}
