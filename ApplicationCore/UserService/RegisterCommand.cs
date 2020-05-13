using Dtos;
using MediatR;

namespace ApplicationCore.UserService
{
    public class RegisterCommand : IRequest<UserDto>
    {
        public RegisterCommand(UserDto user)
        {
            User = user;
        }
        public UserDto User { get; set; }
    }
}
