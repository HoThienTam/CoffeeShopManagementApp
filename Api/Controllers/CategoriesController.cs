using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Commands;
using ApplicationCore.Queries;
using Dtos;
using Infrastructure.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _mediator.Send(new GetCategoriesQuery());
            return Ok(categories);
        }
        [HttpGet("{id}", Name = nameof(GetCategory))]
        public async Task<IActionResult> GetCategory(Guid id)
        {
            var category = await _mediator.Send(new GetCategoryQuery(id));
            return Ok(category);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryDto categoryDto)
        {
            var category = await _mediator.Send(new AddCategoryCommand(categoryDto));
            if (category != null)
            {
                return CreatedAtRoute(nameof(GetCategory), new { id = category.Id }, category);
            }
            else
            {
                return BadRequest("Không thể tạo mới!");
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateCategory(CategoryDto categoryDto)
        {
            var ok = await _mediator.Send(new UpdateCategoryCommand(categoryDto));
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
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var ok = await _mediator.Send(new DeleteCategoryCommand(id));
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