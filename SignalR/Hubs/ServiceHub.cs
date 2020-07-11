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
        public async Task SendSession(SessionDto session)
        {
            await Clients.All.SendAsync("ReceiveSession", session);
        }
    }
}
