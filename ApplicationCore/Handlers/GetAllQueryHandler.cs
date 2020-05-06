using ApplicationCore.Queries;
using AutoMapper;
using Infrastructure.Data;
using Infrastructure.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationCore.Handlers
{
    public class GetAllQueryHandler<T, V> : IRequestHandler<GetAllQuery<T, V>, IEnumerable<T>> where T : BindableBase where V : BaseModel
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public GetAllQueryHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<T>> Handle(GetAllQuery<T, V> request, CancellationToken cancellationToken)
        {
            var listObj = _context.Set<V>().Where(c => c.IsDeleted == false).AsNoTracking();
            var listObjDto = _mapper.Map<IEnumerable<T>>(listObj);
            return listObjDto;
        }
    }
}
