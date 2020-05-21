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

namespace ApplicationCore.TableService
{
    public class AddTableCommandHandler : IRequestHandler<AddTableCommand, TableDto>
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public AddTableCommandHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TableDto> Handle(AddTableCommand request, CancellationToken cancellationToken)
        {
            var table = _mapper.Map<Table>(request.TableDto);

            await _context.Tables.AddAsync(table);

            if (await _context.SaveChangesAsync() > 0)
            {
                var tableDto = _mapper.Map<TableDto>(table);
                return tableDto;
            }

            return null;
        }
    }
}
