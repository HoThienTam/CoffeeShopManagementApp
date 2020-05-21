using Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.ZoneService
{
    public class GetZonesQuery : IRequest<IEnumerable<ZoneDto>>
    {
    }
}
