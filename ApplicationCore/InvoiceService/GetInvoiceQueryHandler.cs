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

        public GetInvoiceQueryHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<InvoiceDto> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
        {
            var invoice = await _context.Invoices.AsNoTracking()
                .Include(c => c.InvoiceItems)
                .Include(c => c.InvoiceDiscounts)
                .FirstOrDefaultAsync(c => c.Id == request.Id);

            if (invoice == null)
            {
                throw new Exception("Hóa đơn không tồn tại");
            }

            var invoiceDto = _mapper.Map<InvoiceDto>(invoice);

            var items = new List<ItemForInvoiceDto>();
            var discounts = new List<DiscountDto>();

            foreach (var item in invoice.InvoiceItems)
            {
                items.Add(_mapper.Map<ItemForInvoiceDto>(item.Item));
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
