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

namespace ApplicationCore.UserService
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public GetUserQueryHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == request.Id);
            if (user == null)
            {
                return null;
            }
            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }
    }
}
