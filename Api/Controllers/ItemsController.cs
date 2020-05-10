using ApplicationCore.Commands;
using ApplicationCore.Queries;
using Dtos;
using Infrastructure.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetItems()
        {
            var items = await _mediator.Send(new GetAllQuery<ItemDto, Item>(x => x.Category));
            return Ok(items);
        }
        [HttpGet("{id}", Name = nameof(GetItem))]
        public async Task<IActionResult> GetItem(Guid id)
        {
            var item = await _mediator.Send(new GetByIdQuery<ItemDto, Item>(id, x => x.Category));
            return Ok(item);
        }
        [HttpPost]
        public async Task<IActionResult> CreateItem(ItemForCreateDto itemDto)
        {
            var itemToReturn = await _mediator.Send(new AddItemCommand(itemDto));
            return CreatedAtRoute(nameof(GetItem), new { id = itemToReturn.Id }, itemToReturn);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateItem(ItemForCreateDto itemDto)
        {
            var ok = await _mediator.Send(new UpdateItemCommand(itemDto));
            if (ok)
            {
                return NoContent();
            }
            else
            {
                return BadRequest("Không thể cập nhật mặt hàng!");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(Guid id)
        {
            var ok = await _mediator.Send(new DeleteItemCommand(id));
            if (ok)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Không thể xóa mặt hàng!");
            }
        }
    }
}
