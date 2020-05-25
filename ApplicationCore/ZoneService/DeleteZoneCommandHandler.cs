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
    public class DeleteZoneCommandHandler : IRequestHandler<DeleteZoneCommand, bool>
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public DeleteZoneCommandHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Handle(DeleteZoneCommand request, CancellationToken cancellationToken)
        {
            var zoneFromDb = await _context.Zones.FirstOrDefaultAsync(i => i.Id == request.Id);
            if (zoneFromDb == null)
            {
                return false;
            }
            zoneFromDb.IsDeleted = true;
            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
