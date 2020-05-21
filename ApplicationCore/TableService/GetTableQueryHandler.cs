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

namespace ApplicationCore.TableService
{
    public class GetTableQueryHandler : IRequestHandler<GetTableQuery, TableDto>
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public GetTableQueryHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TableDto> Handle(GetTableQuery request, CancellationToken cancellationToken)
        {
            var tableFromDb = await _context.Tables.FirstOrDefaultAsync(i => i.Id == request.Id);
            if (tableFromDb == null)
            {
                throw new Exception("Bàn không tồn tại");
            }
            var tableDto = _mapper.Map<TableDto>(tableFromDb);
            return tableDto;
        }
    }
}
