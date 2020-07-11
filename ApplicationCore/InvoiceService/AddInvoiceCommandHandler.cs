﻿using AutoMapper;
using Dtos;
using ImTools;
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
            var session = await _context.Sessions.FirstOrDefaultAsync(s => s.Status == 0);

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
            session.Revenue += invoice.TotalPrice;
            session.ExpectedMoney += invoice.TotalPrice + invoice.Tip;
            session.Tip += invoice.Tip;
            if (await _context.SaveChangesAsync() > 0)
            {
                var invoiceToReturn = await _mediator.Send(new GetInvoiceQuery(invoice.Id));
                return invoiceToReturn;
            }
            return null;
        }
    }
}
