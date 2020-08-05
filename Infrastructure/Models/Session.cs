using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Models
{
    public class Session : BaseModel
    {
        public DateTime ClosedAt { get; set; }
        public double InitMoney { get; set; }
        public double Revenue { get; set; }
        public double Tip { get; set; }
        public double ExpectedMoney { get; set; }
        public double RealMoney { get; set; }
        public double Difference { get; set; }
        public bool IsClosed { get; set; }
    }
}
