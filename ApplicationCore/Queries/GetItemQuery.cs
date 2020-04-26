﻿using Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Queries
{
    public class GetItemQuery : IRequest<ItemDto>
    {
        public GetItemQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
