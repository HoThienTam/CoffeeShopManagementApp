using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.DiscountService
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
