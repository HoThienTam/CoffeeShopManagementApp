using Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.TableService
{
    public class AddTableCommand : IRequest<TableDto>
    {
        public AddTableCommand(TableDto tableDto)
        {
            TableDto = tableDto;
        }

        public TableDto TableDto { get; set; }
    }
}
