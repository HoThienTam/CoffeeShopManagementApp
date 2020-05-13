using Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.InvoiceService
{
    public class AddInvoiceCommand : IRequest<InvoiceDto>
    {
        public AddInvoiceCommand(InvoiceForCreateDto invoice)
        {
            Invoice = invoice;
        }

        public InvoiceForCreateDto Invoice { get; set; }
    }
}
