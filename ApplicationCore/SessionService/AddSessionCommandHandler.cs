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

namespace ApplicationCore.SessionService
{
    public class AddSessionCommandHandler : IRequestHandler<AddSessionCommand, SessionDto>
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public AddSessionCommandHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<SessionDto> Handle(AddSessionCommand request, CancellationToken cancellationToken)
        {
            var session = new Session();
            session.InitMoney = request.InitMoney;
            session.ExpectedMoney = request.InitMoney;
            await _context.Sessions.AddAsync(session);

            if (await _context.SaveChangesAsync() > 0)
            {
                var sessionToReturn = _mapper.Map<SessionDto>(session);
                return sessionToReturn;
            }
            return null;
        }
    }
}
