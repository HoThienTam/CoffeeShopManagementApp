using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Models
{
    public class Table : BaseModel
    {
        public string Name { get; set; }
        public string ZoneId { get; set; }
        public Zone Zone { get; set; }
    }
}
