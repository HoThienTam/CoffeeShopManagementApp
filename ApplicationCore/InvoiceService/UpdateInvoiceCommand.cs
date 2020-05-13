using Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.InvoiceService
{
    public class UpdateInvoiceCommand : IRequest<bool>
    {
        public UpdateInvoiceCommand(InvoiceForCreateDto invoice)
        {
            Invoice = invoice;
        }

        public InvoiceForCreateDto Invoice { get; set; }
    }
}
