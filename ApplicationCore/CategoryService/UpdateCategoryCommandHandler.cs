using AutoMapper;
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
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, bool>
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public UpdateCategoryCommandHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == request.CategoryDto.Id);

            _mapper.Map(request.CategoryDto, category);

            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }

            return false;
        }
    }
}
