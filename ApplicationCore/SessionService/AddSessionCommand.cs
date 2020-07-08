using Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.SessionService
{
    public class AddSessionCommand : IRequest<SessionDto>
    {
        public AddSessionCommand(double initMoney)
        {
            InitMoney = initMoney;
        }

        public double InitMoney { get; set; }
    }
}
