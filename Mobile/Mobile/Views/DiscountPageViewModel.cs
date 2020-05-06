using Dtos;
using Mobile.Models;
using Mobile.ViewModels;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.Views
{
    public class DiscountPageViewModel : ViewModelBase
    {
        public DiscountPageViewModel(InitParams initParams) : base(initParams)
        {
        }

        #region IsOpen
        private bool _IsOpen = false;
        public bool IsOpen
        {
            get { return _IsOpen; }
            set { SetProperty(ref _IsOpen, value); }
        }
        #endregion

        #region DiscountBindProp
        private DiscountDto _DiscountBindProp = null;
        public DiscountDto DiscountBindProp
        {
            get { return _DiscountBindProp; }
            set { SetProperty(ref _DiscountBindProp, value); }
        }
        #endregion

        #region ShowPopUpCommand

        public DelegateCommand<object> ShowPopUpCommand { get; private set; }
        private async void OnShowPopUp(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                if (IsOpen)
                {
                    IsOpen = false;
                }
                else
                {
                    IsOpen = true;
                    DiscountBindProp = new DiscountDto();
                }
            }
            catch (Exception e)
            {
                await ShowErrorAsync(e);
            }
            finally
            {
                IsBusy = false;
            }

        }
        [Initialize]
        private void InitShowPopUpCommand()
        {
            ShowPopUpCommand = new DelegateCommand<object>(OnShowPopUp);
            ShowPopUpCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion
    }
}
