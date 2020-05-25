using AutoMapper;
using Dtos;
using Infrastructure.Data;
using Infrastructure.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationCore.ZoneService
{
    public class AddZoneCommandHandler : IRequestHandler<AddZoneCommand, ZoneDto>
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public AddZoneCommandHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ZoneDto> Handle(AddZoneCommand request, CancellationToken cancellationToken)
        {
            var zone = _mapper.Map<Zone>(request.ZoneDto);

            _context.Zones.Add(zone);

            foreach (var table in zone.Tables)
            {
                await _context.Tables.AddAsync(table);
            }

            if (await _context.SaveChangesAsync() > 0)
            {
                var zoneDto = _mapper.Map<ZoneDto>(zone);
                return zoneDto;
            }

            return null;
        }
    }
}
