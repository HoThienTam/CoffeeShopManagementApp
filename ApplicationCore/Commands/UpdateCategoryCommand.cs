﻿using Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Commands
{
    public class UpdateCategoryCommand : IRequest<bool>
    {
        public UpdateCategoryCommand(CategoryDto categoryDto)
        {
            CategoryDto = categoryDto;
        }

        public CategoryDto CategoryDto { get; set; }
    }
}
