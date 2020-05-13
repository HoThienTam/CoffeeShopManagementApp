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
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, bool>
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public DeleteCategoryCommandHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _context.Categories.Include(c => c.Items).FirstOrDefaultAsync(c => c.Id == request.Id);

            if (category == null)
            {
                return false;
            }

            category.IsDeleted = true;
            foreach (var item in category.Items)
            {
                item.IsDeleted = true;
            }
            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }

            return false;
        }
    }
}
