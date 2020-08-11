using AutoMapper;
using Dtos;
using Infrastructure.Data;
using Infrastructure.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationCore.CategoryService
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
            var categories = _context.Categories
                .Where(c => c.IsDeleted == false)
                .Include(c => c.Items)
                .Select(c => new Category
                {
                    Id = c.Id, Name = c.Name, Icon = c.Icon, Items = c.Items.Where(i => !i.IsDeleted && !i.IsOutOfStock).ToList()
                })
                .AsNoTracking();
            var cateDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);
            return cateDtos;
        }
    }
}
