using ApplicationCore.Commands;
using AutoMapper;
using Dtos;
using Infrastructure.IRepositories;
using Infrastructure.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationCore.Handlers
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, UserDto>
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;

        public RegisterCommandHandler(IUserRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (await _repo.CheckUserExistsAsync(request.User.Username))
            {
                throw new ArgumentException($"This username {request.User.Username} is already existed!", nameof(request.User.Username));
            }

            var user = _mapper.Map<User>(request.User);

            await _repo.RegisterAsync(user, request.User.Password);

            return request.User;
        }
    }
}
