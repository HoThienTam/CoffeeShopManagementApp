using Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.ZoneService
{
    public class AddZoneCommand : IRequest<ZoneDto>
    {
        public AddZoneCommand(ZoneDto zoneDto)
        {
            ZoneDto = zoneDto;
        }

        public ZoneDto ZoneDto { get; set; }
    }
}
