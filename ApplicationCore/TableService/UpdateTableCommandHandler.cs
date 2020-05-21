using AutoMapper;
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
    public class UpdateTableCommandHandler : IRequestHandler<UpdateTableCommand, bool>
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public UpdateTableCommandHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateTableCommand request, CancellationToken cancellationToken)
        {
            var tableFromDb = await _context.Tables.FirstOrDefaultAsync(i => i.Id == request.TableDto.Id);

            _mapper.Map(request.TableDto, tableFromDb);

            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }

            return false;
        }
    }
    {
    }
}
