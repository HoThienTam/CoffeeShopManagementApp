using Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.CategoryService
{
    public class AddCategoryCommand : IRequest<CategoryDto>
    {
        public AddCategoryCommand(CategoryDto category)
        {
            Category = category;
        }
        public CategoryDto Category { get; set; }
    }
}
