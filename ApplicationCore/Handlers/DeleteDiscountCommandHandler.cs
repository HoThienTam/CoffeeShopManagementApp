using ApplicationCore.Commands;
using AutoMapper;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationCore.Handlers
{
    public class DeleteDiscountCommandHandler : IRequestHandler<DeleteDiscountCommand, bool>
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public DeleteDiscountCommandHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
        {
            var discountFromDb = await _context.Discounts.FirstOrDefaultAsync(i => i.Id == request.Id);
            if (discountFromDb == null)
            {
                return false;
            }
            discountFromDb.IsDeleted = true;
            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
