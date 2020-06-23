﻿using AutoMapper;
using Dtos;
using ImTools;
using Infrastructure.Data;
using Infrastructure.Models;
using MediatR;
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

        public AddInvoiceCommandHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<InvoiceDto> Handle(AddInvoiceCommand request, CancellationToken cancellationToken)
        {
            var invoice = _mapper.Map<Invoice>(request.Invoice);
            var items = _mapper.Map<IEnumerable<Item>>(request.Invoice.Items);
            var discounts = _mapper.Map<IEnumerable<Discount>>(request.Invoice.Discounts);

            await _context.Invoices.AddAsync(invoice);

            foreach (var item in request.Invoice.Items)
            {
                var invoiceItem = new InvoiceItem
                {
                    InvoiceId = invoice.Id,
                    ItemId = item.Id,
                    Quantity = item.Quantity
                };
                await _context.InvoiceItems.AddAsync(invoiceItem);
            }

            foreach (var discount in request.Invoice.Discounts)
            {
                var invoiceDiscount = new InvoiceDiscount
                {
                    InvoiceId = invoice.Id,
                    DiscountId = discount.Id,
                };
                await _context.InvoiceDiscounts.AddAsync(invoiceDiscount);
            }
            if (await _context.SaveChangesAsync() > 0)
            {
                var invoiceToReturn = _mapper.Map<InvoiceDto>(invoice);
                invoiceToReturn.Items = _mapper.Map<ObservableCollection<ItemForInvoiceDto>>(items);
                invoiceToReturn.Discounts = _mapper.Map<ObservableCollection<DiscountDto>>(discounts);
                return invoiceToReturn;
            }
            return null;
        }
    }
}
