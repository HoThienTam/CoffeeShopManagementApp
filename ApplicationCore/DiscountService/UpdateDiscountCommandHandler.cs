using AutoMapper;
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
    public class UpdateDiscountCommandHandler : IRequestHandler<UpdateDiscountCommand, bool>
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public UpdateDiscountCommandHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateDiscountCommand request, CancellationToken cancellationToken)
        {
            var discount = await _context.Discounts.FirstOrDefaultAsync(c => c.Id == request.Discount.Id);

            _mapper.Map(request.Discount, discount);

            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }

            return false;
        }
    }
}
