using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Commands
{
    public class DeleteDiscountCommand : IRequest<bool>
    {
        public DeleteDiscountCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
