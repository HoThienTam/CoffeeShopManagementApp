using Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Commands
{
    public class AddCategoryCommand : IRequest<bool>
    {
        public AddCategoryCommand(CategoryDto category)
        {
            Category = category;
        }
        public CategoryDto Category { get; set; }
    }
}
