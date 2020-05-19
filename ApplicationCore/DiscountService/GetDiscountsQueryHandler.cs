using AutoMapper;
using Dtos;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationCore.DiscountService
{
    public class GetDiscountsQueryHandler : IRequestHandler<GetDiscountsQuery, IEnumerable<DiscountDto>>
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public GetDiscountsQueryHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DiscountDto>> Handle(GetDiscountsQuery request, CancellationToken cancellationToken)
        {
            var discounts = _context.Discounts.Where(c => c.IsDeleted == false).OrderBy(c => c.CreatedAt).AsNoTracking();
            var discountDtos = _mapper.Map<IEnumerable<DiscountDto>>(discounts);
            return discountDtos;
        }
    }
}
