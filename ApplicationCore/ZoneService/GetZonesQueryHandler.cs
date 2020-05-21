using AutoMapper;
using Dtos;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationCore.ZoneService
{
    public class GetZonesQueryHandler : IRequestHandler<GetZonesQuery, IEnumerable<ZoneDto>>
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public GetZonesQueryHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ZoneDto>> Handle(GetZonesQuery request, CancellationToken cancellationToken)
        {
            var zones = _context.Zones.Where(c => c.IsDeleted == false).Include(i => i.Tables).AsNoTracking();
            var zoneDtos = _mapper.Map<IEnumerable<ZoneDto>>(zones);
            return zoneDtos;
        }
    }
}
