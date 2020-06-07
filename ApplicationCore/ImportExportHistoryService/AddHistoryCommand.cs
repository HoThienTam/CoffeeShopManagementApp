using Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.ImportExportHistoryService
{
    public class AddHistoryCommand : IRequest<ImportExportHistoryDto>
    {
        public AddHistoryCommand(ImportExportHistoryDto history)
        {
            History = history;
        }

        public ImportExportHistoryDto History { get; set; }
    }
}
