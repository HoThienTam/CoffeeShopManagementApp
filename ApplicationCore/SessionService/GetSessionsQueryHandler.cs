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

namespace ApplicationCore.SessionService
{
    public class GetSessionsQueryHandler : IRequestHandler<GetSessionsQuery, IEnumerable<SessionDto>>
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public GetSessionsQueryHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<SessionDto>> Handle(GetSessionsQuery request, CancellationToken cancellationToken)
        {
            var sessions = _context.Sessions.Where(c => c.IsClosed).AsNoTracking().OrderBy(c => c.ClosedAt);
            var sessionDtos = _mapper.Map<IEnumerable<SessionDto>>(sessions);
            return sessionDtos;
        }
    }
}
