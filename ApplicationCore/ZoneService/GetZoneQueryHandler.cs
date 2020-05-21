using AutoMapper;
using Dtos;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationCore.ZoneService
{
    public class GetZoneQueryHandler : IRequestHandler<GetZoneQuery, ZoneDto>
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public GetZoneQueryHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ZoneDto> Handle(GetZoneQuery request, CancellationToken cancellationToken)
        {
            var zoneFromDb = await _context.Zones.AsNoTracking().Include(i => i.Tables).FirstOrDefaultAsync(i => i.Id == request.Id);
            if (zoneFromDb == null)
            {
                throw new Exception("Mặt hàng không tồn tại");
            }
            var zoneDto = _mapper.Map<ZoneDto>(zoneFromDb);
            return zoneDto;
        }
    }
}
