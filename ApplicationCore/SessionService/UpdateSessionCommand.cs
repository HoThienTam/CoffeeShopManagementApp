using Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.SessionService
{
    public class UpdateSessionCommand : IRequest<bool>
    {
        public UpdateSessionCommand(SessionDto session)
        {
            Session = session;
        }

        public SessionDto Session { get; set; }
    }
}
