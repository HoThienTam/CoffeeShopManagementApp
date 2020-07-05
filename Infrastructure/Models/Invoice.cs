using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Models
{
    public class Invoice : BaseModel
    {
        public string InvoiceNumber { get; set; }
        public bool IsPaid { get; set; }
        public DateTime ClosedAt { get; set; }
        public double TotalPrice { get; set; }
        public double PaidAmount { get; set; }
        public double Tip { get; set; }
        public Guid TableId { get; set; }
        public string TableName { get; set; }
        public ICollection<InvoiceItem> InvoiceItems { get; set; }
        public ICollection<InvoiceDiscount> InvoiceDiscounts { get; set; }
    }
}
