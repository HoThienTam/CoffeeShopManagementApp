using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos
{
    public class DiscountDto : BindableBase
    {
        public Guid Id { get; set; }

        #region Name
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { SetProperty(ref _Name, value); }
        }
        #endregion

        #region Value
        private double _Value;
        public double Value
        {
            get { return _Value; }
            set { SetProperty(ref _Value, value); }
        }
        #endregion

        #region MaxValue
        private double _MaxValue;
        public double MaxValue
        {
            get { return _MaxValue; }
            set { SetProperty(ref _MaxValue, value); }
        }
        #endregion

        #region IsPercentage
        private bool _IsPercentage;
        public bool IsPercentage
        {
            get { return _IsPercentage; }
            set { SetProperty(ref _IsPercentage, value); }
        }
        #endregion

    }
}
