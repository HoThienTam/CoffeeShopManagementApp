using Mobile.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.ViewModels
{
    public class NewItemPageViewModel : ViewModelBase
    {
        public NewItemPageViewModel(InitParams initParams) : base(initParams)
        {
            Title = "Mặt hàng mới";
        }
    }
}
