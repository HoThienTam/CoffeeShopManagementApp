using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos
{
    public class ImportExportHistoryDto
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public string Reason { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
