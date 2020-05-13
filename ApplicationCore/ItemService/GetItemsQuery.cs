using Dtos;
using Infrastructure.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.ItemService
{
    public class GetItemsQuery : IRequest<IEnumerable<ItemDto>>
    {
    }
}
