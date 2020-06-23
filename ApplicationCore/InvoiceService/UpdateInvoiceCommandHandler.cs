using AutoMapper;
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
            var invoiceItems = _context.InvoiceItems.Where(i => i.InvoiceId == request.Invoice.Id).ToList();
            foreach (var item in request.Invoice.Items)
            {
                if(!invoiceItems.Any(i => i.ItemId == item.Id))
                {
                    var invoiceItem = new InvoiceItem
                    {
                        InvoiceId = request.Invoice.Id,
                        ItemId = item.Id,
                        Quantity = item.Quantity
                    };
                    await _context.InvoiceItems.AddAsync(invoiceItem);
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
