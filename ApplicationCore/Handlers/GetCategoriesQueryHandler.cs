using ApplicationCore.Queries;
using AutoMapper;
using Dtos;
using Infrastructure.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationCore.Handlers
{
    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, IEnumerable<CategoryDto>>
    {
        private DataContext _context;
        private IMapper _mapper;

        public GetCategoriesQueryHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = _context.Categories;
            var cateDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);
            return cateDtos;
        }
    }
}
