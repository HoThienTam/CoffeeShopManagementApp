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

namespace ApplicationCore.SessionService
{
    public class GetSessionQueryHandler : IRequestHandler<GetSessionQuery, SessionDto>
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public GetSessionQueryHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<SessionDto> Handle(GetSessionQuery request, CancellationToken cancellationToken)
        {
            var session = await _context.Sessions.AsNoTracking().FirstOrDefaultAsync(c => c.Id == request.Id);

            if (session == null)
            {
                throw new Exception("Phiên làm việc không tồn tại");
            }

            var sessionDto = _mapper.Map<SessionDto>(session);
            return sessionDto;
        }
    }
}
