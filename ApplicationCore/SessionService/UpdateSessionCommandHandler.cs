using AutoMapper;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationCore.SessionService
{
    public class UpdateSessionCommandHandler : IRequestHandler<UpdateSessionCommand, bool>
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public UpdateSessionCommandHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateSessionCommand request, CancellationToken cancellationToken)
        {
            var session = await _context.Sessions.AsNoTracking().FirstOrDefaultAsync(s => s.Status == 0);
            _mapper.Map(request.Session, session);

            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }

            return false;
        }
    }
}
