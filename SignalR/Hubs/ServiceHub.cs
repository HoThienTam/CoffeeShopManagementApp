using Dtos;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Hubs
{
    public class ServiceHub : Hub
    {
        public async Task SendInvoice(InvoiceDto invoice)
        {
            await Clients.All.SendAsync("ReceiveInvoice", invoice);
        }
        public async Task UpdateInvoice(InvoiceDto invoice)
        {
            await Clients.All.SendAsync("UpdateInvoice", invoice);
        }
        public async Task DeleteInvoice(Guid invoiceId)
        {
            await Clients.All.SendAsync("DeleteInvoice", invoiceId);
        }
    }
}
