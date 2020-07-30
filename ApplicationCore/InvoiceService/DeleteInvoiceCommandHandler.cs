using AutoMapper;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationCore.InvoiceService
{
    public class DeleteInvoiceCommandHandler : IRequestHandler<DeleteInvoiceCommand, bool>
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public DeleteInvoiceCommandHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> Handle(DeleteInvoiceCommand request, CancellationToken cancellationToken)
        {
            var invoice = await _context.Invoices
                .Include(c => c.InvoiceItems)
                .ThenInclude(c => c.Item)
                .ThenInclude(c => c.ItemDiscounts)
                .ThenInclude(c => c.Discount)
                .Include(c => c.InvoiceDiscounts)
                .ThenInclude(c => c.Discount)
                .Include(c => c.Table)
                .FirstOrDefaultAsync(c => c.Id == request.Id);

            if (invoice == null)
            {
                throw new Exception("Hóa đơn không tồn tại");
            }

            invoice.IsDeleted = true;

            foreach (var item in invoice.InvoiceItems)
            {
                item.IsDeleted = true;
                foreach (var discount in item.Item.ItemDiscounts)
                {
                    discount.IsDeleted = true;
                }
            }

            foreach (var discount in invoice.InvoiceDiscounts)
            {
                invoice.IsDeleted = true;
            }
            if (invoice.Table != null)
            {
                invoice.Table.IsBeingUsed = false;
            }

            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
