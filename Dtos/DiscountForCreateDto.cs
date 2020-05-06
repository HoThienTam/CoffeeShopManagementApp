using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos
{
    public class DiscountForCreateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public double MaxValue { get; set; }
        public bool IsPercentage { get; set; }
    }
}
