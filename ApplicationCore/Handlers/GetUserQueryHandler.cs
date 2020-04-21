using ApplicationCore.Queries;
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
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, bool>
    {
        private DataContext _context;

        public GetUserQueryHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == request.Id);

            if (user == null)
            {
                return false;
            }
            return true;
        }
    }
}
