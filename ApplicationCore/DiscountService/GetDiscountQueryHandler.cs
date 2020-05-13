using AutoMapper;
using Dtos;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationCore.DiscountService
{
    public class GetDiscountQueryHandler : IRequestHandler<GetDiscountQuery ,DiscountDto>
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public GetDiscountQueryHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<DiscountDto> Handle(GetDiscountQuery request, CancellationToken cancellationToken)
        {
            var discount = await _context.Discounts.AsNoTracking().FirstOrDefaultAsync(c => c.Id == request.Id);

            if (discount == null)
            {
                throw new Exception("Giảm giá không tồn tại");
            }

            var discountDto = _mapper.Map<DiscountDto>(discount);
            return discountDto;
        }
    }
}
