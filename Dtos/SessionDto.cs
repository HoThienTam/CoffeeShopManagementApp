using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos
{
    public class SessionDto : BindableBase
    {
        public SessionDto()
        {
        }

        public SessionDto(SessionDto session)
        {
            Id = session.Id;
            CreatedAt = session.CreatedAt;
            InitMoney = session.InitMoney;
            ExpectedMoney = session.ExpectedMoney;
        }
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ClosedAt { get; set; }

        #region InitMoney
        private double _InitMoney;
        public double InitMoney
        {
            get { return _InitMoney; }
            set { SetProperty(ref _InitMoney, value); }
        }
        #endregion

        #region Revenue
        private double _Revenue;
        public double Revenue
        {
            get { return _Revenue; }
            set { SetProperty(ref _Revenue, value); }
        }
        #endregion

        #region Tip
        private double _Tip;
        public double Tip
        {
            get { return _Tip; }
            set { SetProperty(ref _Tip, value); }
        }
        #endregion

        #region ExpectedMoney
        private double _ExpectedMoney;
        public double ExpectedMoney
        {
            get { return _ExpectedMoney; }
            set { SetProperty(ref _ExpectedMoney, value); }
        }
        #endregion

        public double RealMoney { get; set; }
        public double Difference { get; set; }

        #region Status
        private int _Status;
        public int Status
        {
            get { return _Status; }
            set { SetProperty(ref _Status, value); }
        }
        #endregion

    }
}
