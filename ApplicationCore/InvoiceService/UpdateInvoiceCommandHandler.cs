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
            _mapper.Map(request.Invoice, invoice);
            var session = await _context.Sessions.FirstOrDefaultAsync(s => !s.IsClosed);
            var table = await _context.Tables.FirstOrDefaultAsync(s => s.Id == invoice.TableId);

            if (!invoice.IsPaid)
            {
                table.IsBeingUsed = true;
            }
            else
            {
                table.IsBeingUsed = false;
            }

            var invoiceItems = _context.InvoiceItems.Where(i => i.InvoiceId == request.Invoice.Id).ToList();
            foreach (var item in request.Invoice.Items)
            {
                if (invoice.IsPaid)
                {
                    var itemDb = await _context.Items.FirstOrDefaultAsync(i => i.Id == item.Id);
                    itemDb.CurrentQuantity -= item.Quantity;
                }
                if (!item.IsAdded)
                {
                    var invoiceItem = new InvoiceItem
                    {
                        InvoiceId = invoice.Id,
                        ItemId = item.Id,
                        Quantity = item.Quantity,
                        Value = item.Value
                    };

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
                    await _context.InvoiceItems.AddAsync(invoiceItem);
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

            if (invoice.IsPaid)
            {
                session.Revenue += invoice.TotalPrice;
                session.ExpectedMoney += invoice.TotalPrice + invoice.Tip;
                session.Tip += invoice.Tip;
            }

            if (await _context.SaveChangesAsync() > 0)
            {

                return true;
            }

            return false;
        }
    }
}
