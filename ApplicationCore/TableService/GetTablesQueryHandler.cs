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

namespace ApplicationCore.TableService
{
    public class GetTablesQueryHandler : IRequestHandler<GetTablesQuery, IEnumerable<TableDto>>
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public GetTablesQueryHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<IEnumerable<TableDto>> Handle(GetTablesQuery request, CancellationToken cancellationToken)
        {
            var tables = _context.Tables.Where(c => c.IsDeleted == false).AsNoTracking();
            var tableDtos = _mapper.Map<IEnumerable<TableDto>>(tables);
            return tableDtos;
        }
    }
}
