using AutoMapper;
using Dtos;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationCore.ImportExportHistoryService
{
    public class GetHistoriesQueryHandler : IRequestHandler<GetHistoriesQuery, IEnumerable<ImportExportHistoryDto>>
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public GetHistoriesQueryHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ImportExportHistoryDto>> Handle(GetHistoriesQuery request, CancellationToken cancellationToken)
        {
            var histories = _context.ImportExportHistories.Where(h => !h.IsDeleted)
                .OrderByDescending(h => h.CreatedAt).AsNoTracking();
            var historyDtos = _mapper.Map<IEnumerable<ImportExportHistoryDto>>(histories);
            return historyDtos;
        }
    }
}
