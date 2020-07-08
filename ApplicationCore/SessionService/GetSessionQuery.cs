using Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.SessionService
{
    public class GetSessionQuery : IRequest<SessionDto>
    {
        public GetSessionQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
