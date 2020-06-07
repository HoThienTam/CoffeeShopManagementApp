using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.Models
{
    public class SelectionModel : BindableBase
    {
        public string Name { get; set; }

        #region IsSelected
        private bool _IsSelected;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set { SetProperty(ref _IsSelected, value); }
        }
        #endregion

    }
}
