using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Models
{
    public class Zone : BaseModel
    {
        public string Name { get; set; }
        public ICollection<Table> Tables { get; set; }
    }
}
