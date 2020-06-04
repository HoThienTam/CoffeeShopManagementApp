using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.ImportExportHistoryService;
using Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HistoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetHistories()
        {
            var histories = await _mediator.Send(new GetHistoriesQuery());
            return Ok(histories);
        }
        [HttpPost]
        public async Task<IActionResult> CreateHistory(ImportExportHistoryDto historyDto)
        {
            var history = await _mediator.Send(new AddHistoryCommand(historyDto));
            if (history != null)
            {
                return Ok(history);
            }
            else
            {
                return BadRequest("Không thể tạo mới!");
            }
        }
    }
}
