using Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.InvoiceService
{
    public class GetInvoiceQuery : IRequest<InvoiceDto>
    {
        public GetInvoiceQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
