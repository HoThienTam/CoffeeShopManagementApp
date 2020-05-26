using AutoMapper;
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
    public class UpdateZoneCommandHandler : IRequestHandler<UpdateZoneCommand, bool>
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public UpdateZoneCommandHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateZoneCommand request, CancellationToken cancellationToken)
        {
            var zoneFromDb = await _context.Zones.Include(t => t.Tables).FirstOrDefaultAsync(i => i.Id == request.ZoneDto.Id);

            _mapper.Map(request.ZoneDto, zoneFromDb);

            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }

            return false;
        }
    }
}
