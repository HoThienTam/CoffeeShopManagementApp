using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Models
{
    public class Discount : BaseModel
    {
        public string Name { get; set; }
        public double Value { get; set; }
        public double MaxValue { get; set; }
        public bool IsPercentage { get; set; }
    }
}
