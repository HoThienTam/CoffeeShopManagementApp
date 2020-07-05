using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.InvoiceService;
using Dtos;
using Infrastructure.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private IMediator _mediator;

        public InvoicesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetInvoices()
        {
            var invoices = await _mediator.Send(new GetInvoicesQuery(false));
            return Ok(invoices);
        }
        [HttpGet("GetPaidInvoices")]
        public async Task<IActionResult> GetPaidInvoices()
        {
            var invoices = await _mediator.Send(new GetInvoicesQuery(true));
            return Ok(invoices);
        }
        [HttpGet("{id}", Name = nameof(GetInvoice))]
        public async Task<IActionResult> GetInvoice(Guid id)
        {
            var invoice = await _mediator.Send(new GetInvoiceQuery(id));
            return Ok(invoice);
        }
        [HttpPost]
        public async Task<IActionResult> CreateInvoice(InvoiceForCreateDto invoiceDto)
        {
            var invoice = await _mediator.Send(new AddInvoiceCommand(invoiceDto));
            if (invoice != null)
            {
                return CreatedAtRoute(nameof(GetInvoice), new { id = invoice.Id }, invoice);
            }
            else
            {
                return BadRequest("Không thể tạo mới!");
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateInvoice(InvoiceForCreateDto invoiceDto)
        {
            var ok = await _mediator.Send(new UpdateInvoiceCommand(invoiceDto));
            if (ok)
            {
                return NoContent();
            }
            else
            {
                return BadRequest("Không thể cập nhật danh mục!");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(Guid id)
        {
            var ok = await _mediator.Send(new DeleteInvoiceCommand(id));
            if (ok)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Không thể xóa hóa đơn!");
            }

        }
    }
}