using Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Commands
{
    public class AddItemCommand : IRequest<ItemDto>
    {
        public AddItemCommand(ItemDto itemDto)
        {
            ItemDto = itemDto;
        }

        public ItemDto ItemDto { get; set; }
    }
}
