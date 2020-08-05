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
    public class EndSessionCommandHandler : IRequestHandler<EndSessionCommand, SessionDto>
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public EndSessionCommandHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<SessionDto> Handle(EndSessionCommand request, CancellationToken cancellationToken)
        {
            var session = await _context.Sessions.FirstOrDefaultAsync(s => !s.IsClosed);
            session.RealMoney = request.EndMoney;
            session.Difference = request.EndMoney - session.ExpectedMoney;
            session.IsClosed = true;
            session.ClosedAt = DateTime.Now;

            if (await _context.SaveChangesAsync() > 0)
            {
                var sessionToReturn = _mapper.Map<SessionDto>(session);
                return sessionToReturn;
            }

            return null;
        }
    }
}
