using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Models
{
    public class Category : BaseModel
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
