using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.InvoiceService
{
    public class DeleteInvoiceCommand : IRequest<bool>
    {
        public DeleteInvoiceCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
