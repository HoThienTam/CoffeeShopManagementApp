using Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Commands
{
    public class UpdateDiscountCommand : IRequest<bool>
    {
        public UpdateDiscountCommand(DiscountForCreateDto discount)
        {
            Discount = discount;
        }

        public DiscountForCreateDto Discount { get; set; }
    }
}
