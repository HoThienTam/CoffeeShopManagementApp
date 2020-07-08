using Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.SessionService
{
    public class GetSessionsQuery : IRequest<IEnumerable<SessionDto>>
    {
        public GetSessionsQuery()
        {
        }
    }
}
