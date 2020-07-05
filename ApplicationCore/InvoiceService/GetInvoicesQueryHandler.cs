using AutoMapper;
using Dtos;
using Infrastructure.Data;
using Infrastructure.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationCore.InvoiceService
{
    public class GetInvoicesQueryHandler : IRequestHandler<GetInvoicesQuery, IEnumerable<InvoiceDto>>
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public GetInvoicesQueryHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<InvoiceDto>> Handle(GetInvoicesQuery request, CancellationToken cancellationToken)
        {
            var invoices = _context.Invoices.Where(c => !c.IsDeleted && c.IsPaid == request.IsPaid)
                .OrderBy(c => c.CreatedAt)
                .Include(c => c.InvoiceDiscounts)
                .ThenInclude(c => c.Discount)
                .Include(c => c.InvoiceItems)
                .ThenInclude(c => c.Item)
                .ThenInclude(c => c.ItemDiscounts)
                .ThenInclude(c => c.Discount)
                .AsNoTracking();

            var invoiceDtos = new List<InvoiceDto>();

            foreach (var invoice in invoices)
            {
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

                invoiceDtos.Add(invoiceDto);
            }
            
            return invoiceDtos;
        }
    }
}
