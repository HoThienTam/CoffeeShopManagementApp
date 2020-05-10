using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Models
{
    public class InvoiceItem : BaseModel
    {
        public long Quantity { get; set; }
        public Guid ItemId { get; set; }
        public Item Item { get; set; }
        public Guid InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
    }
}
