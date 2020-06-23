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
    }
}