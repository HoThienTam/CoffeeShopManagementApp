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
        public bool IsImported { get; set; }
        public Guid ItemId { get; set; }
        public ItemDto Item { get; set; }
    }
}
