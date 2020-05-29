﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.UserService;
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

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _mediator.Send(new GetUsersQuery());
            return Ok(users);
        }
        [HttpGet("{id}", Name = nameof(GetUser))]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await _mediator.Send(new GetUserQuery(id));
            return Ok(user);
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
        [HttpPut]
        public async Task<IActionResult> UpdateUser(UserDto userDto)
        {
            var ok = await _mediator.Send(new UpdateUserCommand(userDto));
            if (ok)
            {
                return NoContent();
            }
            else
            {
                return BadRequest("Không thể cập nhật nhân viên!");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var ok = await _mediator.Send(new DeleteUserCommand(id));
            if (ok)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Không thể xóa nhân viên!");
            }
        }
    }
}