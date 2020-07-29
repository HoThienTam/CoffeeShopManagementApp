using ImTools;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos
{
    public class InvoiceForCreateDto
    {
        public InvoiceForCreateDto()
        {
        }
        public InvoiceForCreateDto(InvoiceDto invoiceDto)
        {
            Id = invoiceDto.Id;
            TotalPrice = invoiceDto.TotalPrice;
            Items = invoiceDto.Items;
            Discounts = invoiceDto.Discounts;
            TableId = invoiceDto.TableId;
        }

        public Guid Id { get; set; }
        public string InvoiceNumber { get; set; }
        public bool IsPaid { get; set; }
        public DateTime ClosedAt { get; set; }
        public double TotalPrice { get; set; }
        public double PaidAmount { get; set; }
        public double Tip { get; set; }
        public Guid? TableId { get; set; }
        public ICollection<ItemForInvoiceDto> Items { get; set; }
        public ICollection<DiscountForInvoiceDto> Discounts { get; set; }
    }
}
