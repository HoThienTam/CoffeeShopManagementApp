using Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.InvoiceService
{
    public class GetInvoicesQuery : IRequest<IEnumerable<InvoiceDto>>
    {
    }
}
