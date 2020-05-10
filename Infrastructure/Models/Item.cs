using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Models
{
    public class Item : BaseModel
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public bool IsManaged { get; set; }
        public double Price { get; set; }
        public int MinQuantity { get; set; }
        public int CurrentQuantity { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<ItemDiscount> ItemDiscounts { get; set; }
        public ICollection<InvoiceItem> InvoiceItems { get; set; }
    }
}
