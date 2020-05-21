using Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.TableService
{
    public class UpdateTableCommand : IRequest<bool>
    {
        public UpdateTableCommand(TableDto tableDto)
        {
            TableDto = tableDto;
        }

        public TableDto TableDto { get; set; }
    }
}
