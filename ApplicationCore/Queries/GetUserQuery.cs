using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Queries
{
    public class GetUserQuery : IRequest<bool>
    {
        public GetUserQuery(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
}
