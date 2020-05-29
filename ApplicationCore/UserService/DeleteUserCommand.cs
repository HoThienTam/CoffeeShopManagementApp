using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.UserService
{
    public class DeleteUserCommand : IRequest<bool>
    {
        public DeleteUserCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
