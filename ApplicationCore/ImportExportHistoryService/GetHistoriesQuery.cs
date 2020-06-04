using Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.ImportExportHistoryService
{
    public class GetHistoriesQuery : IRequest<IEnumerable<ImportExportHistoryDto>>
    {
    }
}
