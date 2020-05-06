using Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Commands
{
    public class AddDiscountCommand : IRequest<DiscountDto>
    {
        public AddDiscountCommand(DiscountForCreateDto discount)
        {
            Discount = discount;
        }
        public DiscountForCreateDto Discount { get; set; }
    }
}
