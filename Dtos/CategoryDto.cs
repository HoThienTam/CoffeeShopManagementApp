using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos
{
    public class CategoryDto
    {
        public CategoryDto()
        {
        }
        public CategoryDto(CategoryDto categoryDto)
        {
            Id = categoryDto.Id;
            Name = categoryDto.Name;
            Icon = categoryDto.Icon;
        }
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Icon { get; set; }
        public ICollection<ItemDto> Items { get; set; }
    }
}
