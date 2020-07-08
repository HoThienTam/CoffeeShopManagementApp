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
    public class GetCurrentSessonQueryHandler : IRequestHandler<GetCurrentSessionQuery, SessionDto>
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public GetCurrentSessonQueryHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<SessionDto> Handle(GetCurrentSessionQuery request, CancellationToken cancellationToken)
        {
            var session = await _context.Sessions.AsNoTracking().OrderByDescending(c => c.CreatedAt).FirstOrDefaultAsync();

            if (session == null)
            {
                throw new Exception("Phiên làm việc không tồn tại");
            }

            var sessionDto = _mapper.Map<SessionDto>(session);
            return sessionDto;
        }
    }
}
