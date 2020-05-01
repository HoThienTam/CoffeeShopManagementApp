using Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Commands
{
    public class UpdateCategoryCommand : IRequest<bool>
    {
        public UpdateCategoryCommand(CategoryForCreateDto categoryDto)
        {
            CategoryDto = categoryDto;
        }

        public CategoryForCreateDto CategoryDto { get; set; }
    }
}
