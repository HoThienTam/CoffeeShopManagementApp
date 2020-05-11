using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos
{
    public class InvoiceForCreateDto
    {
        public InvoiceForCreateDto()
        {
            Items = new List<ItemDto>();
            Discounts = new List<DiscountDto>();
        }

        public Guid Id { get; set; }
        public string InvoiceNumber { get; set; }
        public int Status { get; set; }
        public DateTime ClosedAt { get; set; }
        public double TotalPrice { get; set; }
        public double PaidAmount { get; set; }
        public double Tip { get; set; }
        public Guid TableId { get; set; }
        public TableDto Table { get; set; }
        public ICollection<ItemDto> Items { get; set; }
        public ICollection<DiscountDto> Discounts { get; set; }
    }
}
