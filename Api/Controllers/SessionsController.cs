using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.SessionService;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionsController : ControllerBase
    {
        private IMediator _mediator;

        public SessionsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetSessions()
        {
            var sessions = await _mediator.Send(new GetSessionsQuery());
            return Ok(sessions);
        }
        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentSession()
        {
            var session = await _mediator.Send(new GetCurrentSessionQuery());
            return Ok(session);
        }
        [HttpGet("{id}", Name = nameof(GetSession))]
        public async Task<IActionResult> GetSession(Guid id)
        {
            var session = await _mediator.Send(new GetSessionQuery(id));
            return Ok(session);
        }
        [HttpPost]
        public async Task<IActionResult> CreateSession([FromBody]double initMoney)
        {
            var session = await _mediator.Send(new AddSessionCommand(initMoney));
            if (session != null)
            {
                return CreatedAtRoute(nameof(GetSession), new { id = session.Id }, session);
            }
            else
            {
                return BadRequest("Không thể tạo mới!");
            }
        }
    }
}
