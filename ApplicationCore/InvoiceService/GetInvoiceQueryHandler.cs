using AutoMapper;
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
    public class GetInvoiceQueryHandler : IRequestHandler<GetInvoiceQuery, InvoiceDto>
    {
        private DataContext _context;
        private readonly IMapper _mapper;
        private IMediator _mediator;

        public GetInvoiceQueryHandler(DataContext context, IMapper mapper, IMediator mediator)
        {
            _context = context;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<InvoiceDto> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
        {
            var invoice = await _context.Invoices.AsNoTracking()
                .Include(c => c.InvoiceItems)
                .ThenInclude(c => c.Item)
                .ThenInclude(c => c.ItemDiscounts)
                .ThenInclude(c => c.Discount)
                .Include(c => c.InvoiceDiscounts)
                .ThenInclude(c => c.Discount)
                .FirstOrDefaultAsync(c => c.Id == request.Id);

            if (invoice == null)
            {
                throw new Exception("Hóa đơn không tồn tại");
            }

            var invoiceDto = _mapper.Map<InvoiceDto>(invoice);

            var items = new List<ItemForInvoiceDto>();
            var discounts = new List<DiscountDto>();
            var itemDiscounts = new List<DiscountDto>();

            foreach (var item in invoice.InvoiceItems)
            {
                var itemForInvoice = _mapper.Map<ItemForInvoiceDto>(item.Item);
                itemForInvoice.Quantity = item.Quantity;
                itemForInvoice.Value = item.Value;
                foreach (var discount in item.Item.ItemDiscounts)
                {
                    var discountForItem = _mapper.Map<DiscountDto>(discount.Discount);
                    discountForItem.Value = discount.Value;
                    if (discount.InvoiceItemId == item.Id)
                    {
                        itemDiscounts.Add(discountForItem);
                    }
                }
                itemForInvoice.Discounts = new ObservableCollection<DiscountDto>(itemDiscounts);
                items.Add(itemForInvoice);
            }

            foreach (var discount in invoice.InvoiceDiscounts)
            {
                discounts.Add(_mapper.Map<DiscountDto>(discount.Discount));
            }

            invoiceDto.Items = new ObservableCollection<ItemForInvoiceDto>(items);
            invoiceDto.Discounts = new ObservableCollection<DiscountDto>(discounts);

            return invoiceDto;
        }
    }
}
