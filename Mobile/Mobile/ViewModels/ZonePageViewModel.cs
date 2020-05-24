using Dtos;
using Mobile.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Mobile.ViewModels
{
    public class ZonePageViewModel : ViewModelBase
    {
        public ZonePageViewModel(InitParams initParams) : base(initParams)
        {
            ListZoneBindProp = new ObservableCollection<ZoneDto>();
        }
        #region ListZoneBindProp
        private ObservableCollection<ZoneDto> _ListZoneBindProp = null;
        public ObservableCollection<ZoneDto> ListZoneBindProp
        {
            get { return _ListZoneBindProp; }
            set { SetProperty(ref _ListZoneBindProp, value); }
        }
        #endregion

    }
}
