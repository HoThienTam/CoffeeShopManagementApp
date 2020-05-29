using Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.UserService
{
    public class UpdateUserCommand : IRequest<bool>
    {
        public UpdateUserCommand(UserDto user)
        {
            User = user;
        }

        public UserDto User { get; set; }
    }
}
