using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Commands
{
    public class DeleteItemCommand : IRequest<bool>
    {
        public DeleteItemCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
