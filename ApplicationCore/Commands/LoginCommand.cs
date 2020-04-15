using Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Commands
{
    public class LoginCommand : IRequest<string>
    {
        public LoginCommand(LoginDto user)
        {
            Username = user.Username;
            Password = user.Password;
        }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
