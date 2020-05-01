using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos
{
    public class ItemDto
    {
        public ItemDto()
        {
        }
        public ItemDto(ItemDto itemDto)
        {
            Id = itemDto.Id;
            Name = itemDto.Name;
            Image = itemDto.Image;
            Price = itemDto.Price;
            IsManaged = itemDto.IsManaged;
            MinQuantity = itemDto.MinQuantity;
            CurrentQuantity = itemDto.CurrentQuantity;
            CategoryId = itemDto.CategoryId;
            Category = itemDto.Category;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public bool IsManaged { get; set; }
        public double Price { get; set; }
        public int MinQuantity { get; set; }
        public int CurrentQuantity { get; set; }
        public Guid CategoryId { get; set; }
        public CategoryDto Category { get; set; }
    }
}
