using Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.ZoneService
{
    public class GetZoneQuery : IRequest<ZoneDto>
    {
        public GetZoneQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
