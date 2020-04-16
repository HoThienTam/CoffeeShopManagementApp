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
        public async Task<IActionResult> Get()
        {
            var categories = await _mediator.Send(new GetCategoriesQuery());
            return Ok(categories);
        }
        [HttpPost]
        public async Task<IActionResult> Post(CategoryDto category)
        {
            var ok = await _mediator.Send(new AddCategoryCommand(category));
            if (ok)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}