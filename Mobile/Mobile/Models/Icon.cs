using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.Models
{
    public class Icon : BindableBase
    {
        public string Name { get; set; }
        #region IsSelected
        private bool _IsSelected = false;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set { SetProperty(ref _IsSelected, value); }
        }
        #endregion

    }
}
