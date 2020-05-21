using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.TableService
{
    public class DeleteTableCommand : IRequest<bool>
    {
        public DeleteTableCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
