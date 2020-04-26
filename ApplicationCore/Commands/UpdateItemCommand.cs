using Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Commands
{
    public class UpdateItemCommand : IRequest<bool>
    {
        public UpdateItemCommand(ItemDto itemDto)
        {
            ItemDto = itemDto;
        }

        public ItemDto ItemDto { get; set; }
    }
}
