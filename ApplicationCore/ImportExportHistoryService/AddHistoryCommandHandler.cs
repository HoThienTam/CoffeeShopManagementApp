using AutoMapper;
using Dtos;
using Infrastructure.Data;
using Infrastructure.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationCore.ImportExportHistoryService
{
    public class AddHistoryCommandHandler : IRequestHandler<AddHistoryCommand, ImportExportHistoryDto>
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public AddHistoryCommandHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ImportExportHistoryDto> Handle(AddHistoryCommand request, CancellationToken cancellationToken)
        {
            var history = _mapper.Map<ImportExportHistory>(request.History);
            await _context.ImportExportHistories.AddAsync(history);
            var item = await _context.Items.FirstOrDefaultAsync(i => i.Id == request.History.ItemId);
            item.CurrentQuantity += request.History.Quantity;
            try
            {
                if (await _context.SaveChangesAsync() > 0)
                {
                    var historyToReturn = _mapper.Map<ImportExportHistoryDto>(history);
                    return historyToReturn;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return null;
        }
    }
}
