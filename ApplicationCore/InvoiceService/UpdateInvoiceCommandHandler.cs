using AutoMapper;
using ImTools;
using Infrastructure.Data;
using Infrastructure.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationCore.InvoiceService
{
    public class UpdateInvoiceCommandHandler : IRequestHandler<UpdateInvoiceCommand, bool>
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public UpdateInvoiceCommandHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateInvoiceCommand request, CancellationToken cancellationToken)
        {
            var invoice = await _context.Invoices.FirstOrDefaultAsync(i => i.Id == request.Invoice.Id);
            invoice.TotalPrice = request.Invoice.TotalPrice;
            var invoiceItems = _context.InvoiceItems.Where(i => i.InvoiceId == request.Invoice.Id).ToList();
            foreach (var item in request.Invoice.Items)
            {
                if (!item.IsAdded)
                {
                    var invoiceItem = new InvoiceItem
                    {
                        InvoiceId = invoice.Id,
                        ItemId = item.Id,
                        Quantity = item.Quantity,
                        Value = item.Value
                    };
                    await _context.InvoiceItems.AddAsync(invoiceItem);

                    foreach (var discount in item.Discounts)
                    {
                        var itemDiscount = new ItemDiscount
                        {
                            ItemId = item.Id,
                            DiscountId = discount.Id,
                            Value = discount.Value,
                            InvoiceItemId = invoiceItem.Id
                        };
                        await _context.ItemDiscounts.AddAsync(itemDiscount);
                    }
                }
            }

            foreach (var discount in request.Invoice.Discounts)
            {
                if (!discount.IsAdded)
                {
                    var invoiceDiscount = new InvoiceDiscount
                    {
                        InvoiceId = invoice.Id,
                        DiscountId = discount.Id,
                        Value = discount.Value
                    };
                    await _context.InvoiceDiscounts.AddAsync(invoiceDiscount);
                }
            }
            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
