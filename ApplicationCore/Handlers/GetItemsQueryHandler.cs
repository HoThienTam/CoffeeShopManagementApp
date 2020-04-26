using ApplicationCore.Queries;
using AutoMapper;
using Dtos;
using Infrastructure.Data;
using Infrastructure.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationCore.Handlers
{
    public class GetItemsQueryHandler : IRequestHandler<GetItemsQuery, IEnumerable<ItemDto>>
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public GetItemsQueryHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ItemDto>> Handle(GetItemsQuery request, CancellationToken cancellationToken)
        {
            var items = _context.Items.Where(c => c.IsDeleted == false).AsNoTracking();
            var itemDtos = _mapper.Map<IEnumerable<ItemDto>>(items);
            return itemDtos;
        }
    }
}
