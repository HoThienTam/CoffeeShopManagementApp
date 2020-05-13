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

namespace ApplicationCore.CategoryService
{
    public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, CategoryDto>
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public GetCategoryQueryHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CategoryDto> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            var category = await _context.Categories.AsNoTracking().Include(c => c.Items).FirstOrDefaultAsync(c => c.Id == request.Id);

            if (category == null)
            {
                throw new Exception("Danh mục không tồn tại");
            }

            var categoryDto = _mapper.Map<CategoryDto>(category);
            return categoryDto;
        }
    }
}
