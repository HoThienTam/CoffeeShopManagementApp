using Infrastructure.Data;
using Infrastructure.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationCore.UserService
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly IConfiguration _config;
        private DataContext _context;

        public LoginCommandHandler(IConfiguration config, DataContext context)
        {
            _config = config;
            _context = context;
        }

        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var dbUser = await LoginAsync(request.Username.ToLower(), request.Password);

            if (dbUser == null)
            {
                return string.Empty;
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, dbUser.Id.ToString()),
                new Claim(ClaimTypes.Name, dbUser.Username),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["AppSettings:Token"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(4),
                SigningCredentials = creds,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);


            return tokenHandler.WriteToken(token);
        }

        public async Task<User> LoginAsync(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(h => h.Username == username);

            if (user == null)
            {
                return null;
            }

            if (!VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            return user;
        }

        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var hashedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                if (!hashedPassword.SequenceEqual(passwordHash))
                {
                    return false;
                }
            }
            return true;
        }

    }
}
