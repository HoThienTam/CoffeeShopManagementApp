using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.TableService;
using Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TablesController : ControllerBase
    {
        private IMediator _mediator;

        public TablesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetTables()
        {
            var tables = await _mediator.Send(new GetTablesQuery());
            return Ok(tables);
        }
        [HttpGet("{id}", Name = nameof(GetTable))]
        public async Task<IActionResult> GetTable(Guid id)
        {
            var table = await _mediator.Send(new GetTableQuery(id));
            return Ok(table);
        }
        [HttpPost]
        public async Task<IActionResult> CreateDiscount(TableDto tableDto)
        {
            var table = await _mediator.Send(new AddTableCommand(tableDto));
            if (table != null)
            {
                return CreatedAtRoute(nameof(GetTable), new { id = table.Id }, table);
            }
            else
            {
                return BadRequest("Không thể tạo mới!");
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateDiscount(TableDto tableDto)
        {
            var ok = await _mediator.Send(new UpdateTableCommand(tableDto));
            if (ok)
            {
                return NoContent();
            }
            else
            {
                return BadRequest("Không thể cập nhật bàn!");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiscount(Guid id)
        {
            var ok = await _mediator.Send(new DeleteTableCommand(id));
            if (ok)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Không thể xóa bàn!");
            }
        }
    }
}