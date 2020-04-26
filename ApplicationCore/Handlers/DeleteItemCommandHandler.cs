using ApplicationCore.Commands;
using AutoMapper;
using Infrastructure.Data;
using Infrastructure.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationCore.Handlers
{
    public class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand, bool>
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public DeleteItemCommandHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
        {
            var itemFromDb = await _context.Items.FirstOrDefaultAsync(i => i.Id == request.Id);
            if (itemFromDb == null)
            {
                return false;
            }
            itemFromDb.IsDeleted = true;
            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
