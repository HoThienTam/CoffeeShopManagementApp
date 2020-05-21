using Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.TableService
{
    public class GetTablesQuery : IRequest<IEnumerable<TableDto>>
    {
    }
}
