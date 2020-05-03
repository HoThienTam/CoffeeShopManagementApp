using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos
{
    public class ItemForCreateDto : BindableBase
    {
        public ItemForCreateDto(ItemDto itemDto)
        {
            Id = itemDto.Id;
            Name = itemDto.Name;
            Price = itemDto.Price;
            CategoryId = itemDto.CategoryId;
        }

        public ItemForCreateDto()
        {
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public Guid CategoryId { get; set; }
    }
}
