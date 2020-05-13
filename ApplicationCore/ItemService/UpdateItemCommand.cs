﻿using Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.ItemService
{
    public class UpdateItemCommand : IRequest<bool>
    {
        public UpdateItemCommand(ItemForCreateDto itemDto)
        {
            ItemDto = itemDto;
        }

        public ItemForCreateDto ItemDto { get; set; }
    }
}
