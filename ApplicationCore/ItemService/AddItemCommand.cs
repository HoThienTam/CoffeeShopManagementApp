﻿using Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.ItemService
{
    public class AddItemCommand : IRequest<ItemDto>
    {
        public AddItemCommand(ItemForCreateDto itemDto)
        {
            ItemDto = itemDto;
        }

        public ItemForCreateDto ItemDto { get; set; }
    }
}
