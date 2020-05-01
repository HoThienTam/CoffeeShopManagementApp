using Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Commands
{
    public class AddCategoryCommand : IRequest<CategoryDto>
    {
        public AddCategoryCommand(CategoryForCreateDto category)
        {
            Category = category;
        }
        public CategoryForCreateDto Category { get; set; }
    }
}
