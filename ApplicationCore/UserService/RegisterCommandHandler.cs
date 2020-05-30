using AutoMapper;
using Dtos;
using Infrastructure.Data;
using Infrastructure.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationCore.UserService
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, UserDto>
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public RegisterCommandHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (await CheckUserExistsAsync(request.User.Username))
            {
                throw new ArgumentException($"This username {request.User.Username} is already existed!", nameof(request.User.Username));
            }
            var user = _mapper.Map<User>(request.User);

            await RegisterAsync(user, request.User.Password);

            return request.User;
        }

        public async Task<User> RegisterAsync(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            HashPassword(password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private void HashPassword(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> CheckUserExistsAsync(string username)
        {
            if (await _context.Users.AnyAsync(h => h.Username == username))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
