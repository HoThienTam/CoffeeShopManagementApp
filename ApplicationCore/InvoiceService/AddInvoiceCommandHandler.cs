﻿using AutoMapper;
using Dtos;
using Infrastructure.Data;
using Infrastructure.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationCore.InvoiceService
{
    class AddInvoiceCommandHandler : IRequestHandler<AddInvoiceCommand, InvoiceDto>
    {
        private DataContext _context;
        private readonly IMapper _mapper;
        private IMediator _mediator;
        public AddInvoiceCommandHandler(DataContext context, IMapper mapper, IMediator mediator)
        {
            _context = context;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<InvoiceDto> Handle(AddInvoiceCommand request, CancellationToken cancellationToken)
        {
            var invoice = _mapper.Map<Invoice>(request.Invoice);
            var session = await _context.Sessions.FirstOrDefaultAsync(s => !s.IsClosed);

            if (invoice.Table != null && !invoice.IsPaid)
            {
                var table = await _context.Tables.FirstOrDefaultAsync(s => s.Id == invoice.TableId);
                table.IsBeingUsed = true;
            }

            await _context.Invoices.AddAsync(invoice);

            foreach (var item in request.Invoice.Items)
            {
                var invoiceItem = new InvoiceItem
                {
                    InvoiceId = invoice.Id,
                    ItemId = item.Id,
                    Quantity = item.Quantity,
                    Value = item.Value
                };
                await _context.InvoiceItems.AddAsync(invoiceItem);

                if (invoice.IsPaid)
                {
                    var itemDb = await _context.Items.FirstOrDefaultAsync(i => i.Id == item.Id);
                    itemDb.CurrentQuantity -= item.Quantity;
                    if (itemDb.CurrentQuantity <= 0)
                    {
                        itemDb.IsOutOfStock = true;
                    }
                }

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

            foreach (var discount in request.Invoice.Discounts)
            {
                var invoiceDiscount = new InvoiceDiscount
                {
                    InvoiceId = invoice.Id,
                    DiscountId = discount.Id,
                    Value = discount.Value
                };
                await _context.InvoiceDiscounts.AddAsync(invoiceDiscount);
            }

            if (invoice.IsPaid)
            {
                session.Revenue += invoice.TotalPrice;
                session.ExpectedMoney += invoice.TotalPrice + invoice.Tip;
                session.Tip += invoice.Tip;
            }

            if (await _context.SaveChangesAsync() > 0)
            {
                var invoiceToReturn = await _mediator.Send(new GetInvoiceQuery(invoice.Id));
                return invoiceToReturn;
            }
            return null;
        }
    }
}
