using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.ZoneService;
using Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZonesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ZonesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetZones()
        {
            var zones = await _mediator.Send(new GetZonesQuery());
            return Ok(zones);
        }
        [HttpGet("{id}", Name = nameof(GetZone))]
        public async Task<IActionResult> GetZone(Guid id)
        {
            var zone = await _mediator.Send(new GetZoneQuery(id));
            return Ok(zone);
        }
        [HttpPost]
        public async Task<IActionResult> CreateZone(ZoneDto zoneDto)
        {
            var zoneToReturn = await _mediator.Send(new AddZoneCommand(zoneDto));
            if (zoneToReturn != null)
            {
                return CreatedAtRoute(nameof(GetZone), new { id = zoneToReturn.Id }, zoneToReturn);
            }
            else
            {
                return BadRequest("Không thể tạo mới!");
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateItem(ZoneDto zoneDto)
        {
            var ok = await _mediator.Send(new UpdateZoneCommand(zoneDto));
            if (ok)
            {
                return NoContent();
            }
            else
            {
                return BadRequest("Không thể cập nhật khu vực!");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteZone(Guid id)
        {
            var ok = await _mediator.Send(new DeleteZoneCommand(id));
            if (ok)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Không thể xóa khu vực!");
            }
        }
    }
}