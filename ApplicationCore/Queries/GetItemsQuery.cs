using Dtos;
using Infrastructure.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Queries
{
    public class GetItemsQuery : IRequest<IEnumerable<ItemDto>>
    {
    }
}
