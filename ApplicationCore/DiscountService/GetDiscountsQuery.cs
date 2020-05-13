using Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.DiscountService
{
    public class GetDiscountsQuery : IRequest<IEnumerable<DiscountDto>>
    {
    }
}
