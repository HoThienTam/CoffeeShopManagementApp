using Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.ZoneService
{
    public class UpdateZoneCommand : IRequest<bool>
    {
        public UpdateZoneCommand(ZoneDto zoneDto)
        {
            ZoneDto = zoneDto;
        }

        public ZoneDto ZoneDto { get; set; }
    }
}
