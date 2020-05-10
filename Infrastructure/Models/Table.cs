﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Models
{
    public class Table : BaseModel
    {
        public string Name { get; set; }
        public Guid ZoneId { get; set; }
        public Zone Zone { get; set; }
        public Guid InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
    }
}
