using Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.UserService
{
    public class GetUsersQuery : IRequest<IEnumerable<UserDto>>
    {
    }
}
