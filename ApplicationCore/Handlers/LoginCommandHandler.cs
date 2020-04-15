using ApplicationCore.Commands;
using Infrastructure.IRepositories;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationCore.Handlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly IUserRepository _repo;
        private readonly IConfiguration _config;

        public LoginCommandHandler(IUserRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var dbUser = await _repo.LoginAsync(request.Username.ToLower(), request.Password);

            if (dbUser == null)
            {
                throw new ArgumentException($"Username or password is incorrect!");
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
    }
}
