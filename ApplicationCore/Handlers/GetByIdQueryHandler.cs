using ApplicationCore.Queries;
using AutoMapper;
using Infrastructure.Data;
using Infrastructure.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Prism.Mvvm;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationCore.Handlers
{
    public class GetByIdQueryHandler<T, V> : IRequestHandler<GetByIdQuery<T, V>, T> where T : BindableBase where V : BaseModel
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public GetByIdQueryHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<T> Handle(GetByIdQuery<T, V> request, CancellationToken cancellationToken)
        {
            var obj = await _context.Set<V>().AsNoTracking().FirstOrDefaultAsync(c => c.Id == request.Id);

            if (obj == null)
            {
                throw new Exception("Object is null");
            }

            var objDto = _mapper.Map<T>(obj);
            return objDto;
        }
    }
}
