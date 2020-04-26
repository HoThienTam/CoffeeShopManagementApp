using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos
{
    public class ItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public bool IsManaged { get; set; }
        public double Price { get; set; }
        public int MinQuantity { get; set; }
        public int CurrentQuantity { get; set; }
        public CategoryDto Category { get; set; }
    }
}
