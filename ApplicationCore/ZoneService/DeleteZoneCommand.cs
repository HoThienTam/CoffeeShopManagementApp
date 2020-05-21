using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.ZoneService
{
    public class DeleteZoneCommand : IRequest<bool>
    {
        public DeleteZoneCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
