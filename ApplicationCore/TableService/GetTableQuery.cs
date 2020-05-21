using Dtos;
using Infrastructure.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.TableService
{
    public class GetTableQuery : IRequest<TableDto>
    {
        public GetTableQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
