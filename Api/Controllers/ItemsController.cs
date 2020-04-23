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
            return Ok();
        }
        [HttpGet("{id}", Name = nameof(GetItem))]
        public async Task<IActionResult> GetItem(Guid id)
        {
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> CreateItem(ItemDto itemDto)
        {
            return CreatedAtRoute(nameof(GetItem), new { id = itemDto.Id }, itemDto);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateItem(ItemDto itemDto)
        {
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(Guid id)
        {
            return Ok();
        }
    }
}
