using AutoMapper;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationCore.ItemService
{
    public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, bool>
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public UpdateItemCommandHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
        {
            var itemFromDb = await _context.Items.FirstOrDefaultAsync(i => i.Id == request.ItemDto.Id);

            _mapper.Map(request.ItemDto, itemFromDb);

            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }

            return false;
        }
    }
}
