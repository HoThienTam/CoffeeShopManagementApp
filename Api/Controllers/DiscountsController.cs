using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.DiscountService;using Dtos;
using Infrastructure.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController : ControllerBase
    {
        private IMediator _mediator;

        public DiscountsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetDiscounts()
        {
            var discounts = await _mediator.Send(new GetDiscountsQuery());
            return Ok(discounts);
        }
        [HttpGet("{id}", Name = nameof(GetDiscount))]
        public async Task<IActionResult> GetDiscount(Guid id)
        {
            var discount = await _mediator.Send(new GetDiscountQuery(id));
            return Ok(discount);
        }
        [HttpPost]
        public async Task<IActionResult> CreateDiscount(DiscountForCreateDto discountDto)
        {
            var discount = await _mediator.Send(new AddDiscountCommand(discountDto));
            if (discount != null)
            {
                return CreatedAtRoute(nameof(GetDiscount), new { id = discountDto.Id }, discountDto);
            }
            else
            {
                return BadRequest("Không thể tạo mới!");
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateDiscount(DiscountForCreateDto discountDto)
        {
            var ok = await _mediator.Send(new UpdateDiscountCommand(discountDto));
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
        public async Task<IActionResult> DeleteDiscount(Guid id)
        {
            var ok = await _mediator.Send(new DeleteDiscountCommand(id));
            if (ok)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Không thể xóa danh mục!");
            }
        }
    }
}