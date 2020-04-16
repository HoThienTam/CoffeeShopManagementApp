using ApplicationCore.Commands;
using AutoMapper;
using Dtos;
using Infrastructure.Data;
using Infrastructure.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationCore.Handlers
{
    class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, bool>
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public AddCategoryCommandHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<Category>(request.Category);

            await _context.Categories.AddAsync(category);

            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
