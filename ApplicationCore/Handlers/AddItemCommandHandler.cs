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
    public class AddItemCommandHandler : IRequestHandler<AddItemCommand, ItemDto>
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public AddItemCommandHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ItemDto> Handle(AddItemCommand request, CancellationToken cancellationToken)
        {
            var item = _mapper.Map<Item>(request.ItemDto);

            await _context.Items.AddAsync(item);

            if (await _context.SaveChangesAsync() > 0)
            {
                var itemDto = _mapper.Map<ItemDto>(item);
                return itemDto;
            }

            return null;
        }
    }
}
