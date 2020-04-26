using ApplicationCore.Queries;
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

namespace ApplicationCore.Handlers
{
    public class GetItemQueryHandler : IRequestHandler<GetItemQuery, ItemDto>
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public GetItemQueryHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ItemDto> Handle(GetItemQuery request, CancellationToken cancellationToken)
        {
            var itemFromDb = await _context.Items.AsNoTracking().FirstOrDefaultAsync(i => i.Id == request.Id);
            if (itemFromDb == null)
            {
                throw new Exception("Mặt hàng không tồn tại");
            }
            var itemDto = _mapper.Map<ItemDto>(itemFromDb);
            return itemDto;
        }
    }
}
