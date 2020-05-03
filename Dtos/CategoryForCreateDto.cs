using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos
{
    public class CategoryForCreateDto : BindableBase
    {
        public CategoryForCreateDto(CategoryDto categoryDto)
        {
            Id = categoryDto.Id;
            Name = categoryDto.Name;
            Icon = categoryDto.Icon;
        }
        public CategoryForCreateDto()
        {
        }
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Icon { get; set; }
    }
}
