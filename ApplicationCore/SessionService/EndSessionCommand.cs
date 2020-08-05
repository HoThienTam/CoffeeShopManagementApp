using Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.SessionService
{
    public class EndSessionCommand : IRequest<SessionDto>
    {
        public EndSessionCommand(double endMoney)
        {
            EndMoney = endMoney;
        }

        public double EndMoney { get; set; }
    }
}
