using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos
{
    public class DiscountForCreateDto
    {
        public DiscountForCreateDto(DiscountDto discountDto)
        {
            Id = discountDto.Id;
            Name = discountDto.Name;
            Value = discountDto.Value;
            MaxValue = discountDto.MaxValue;
            IsPercentage = discountDto.IsPercentage;
        }
        public DiscountForCreateDto()
        {
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public double MaxValue { get; set; }
        public bool IsPercentage { get; set; }
    }
}
