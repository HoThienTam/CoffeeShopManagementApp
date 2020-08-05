using Prism.Mvvm;
using System;

namespace Dtos
{
    public class TableDto : BaseDto
    {
        #region Name
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { SetProperty(ref _Name, value); }
        }
        #endregion

        #region IsBeingUsed
        private bool _IsBeingUsed;
        public bool IsBeingUsed
        {
            get { return _IsBeingUsed; }
            set { SetProperty(ref _IsBeingUsed, value); }
        }
        #endregion

    }
}