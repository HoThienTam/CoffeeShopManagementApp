using ApplicationCore.Commands;
using AutoMapper;
using Dtos;
using Infrastructure.Data;
using Infrastructure.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationCore.Handlers
{
    public class AddDiscountCommandHandler : IRequestHandler<AddDiscountCommand, DiscountDto>
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public AddDiscountCommandHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<DiscountDto> Handle(AddDiscountCommand request, CancellationToken cancellationToken)
        {
            var discount = _mapper.Map<Discount>(request.Discount);

            await _context.Discounts.AddAsync(discount);

            if (await _context.SaveChangesAsync() > 0)
            {
                var discountToReturn = _mapper.Map<DiscountDto>(discount);
                return discountToReturn;
            }
            return null;
        }
    }
}
