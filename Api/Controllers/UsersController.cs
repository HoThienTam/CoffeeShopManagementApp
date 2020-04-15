using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Commands;
using Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto user)
        {
            var token = await _mediator.Send(new LoginCommand(user));
            return Ok(token);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            var user = await _mediator.Send(new RegisterCommand(userDto));

            return Ok(user);
        }

    }
}