using Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.DiscountService
{
    public class GetDiscountQuery : IRequest<DiscountDto>
    {
        public GetDiscountQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
