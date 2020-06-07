using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Models
{
    public class ImportExportHistory : BaseModel
    {
        public int Quantity { get; set; }
        public string Reason { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
    }
}
    