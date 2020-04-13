using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.Models
{
    public class InitParams
    {
        public InitParams(INavigationService navigationService, IPageDialogService pageDialogService)
        {
            NavigationService = navigationService;
            PageDialogService = pageDialogService;
        }

        public INavigationService NavigationService { get; set; }
        public IPageDialogService PageDialogService { get; set; }
    }
}
