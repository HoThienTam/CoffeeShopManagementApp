using Dtos;
using Mobile.Models;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Mobile.ViewModels
{
    public class ItemDiscountPageViewModel : ViewModelBase
    {
        public ItemDiscountPageViewModel(InitParams initParams) : base(initParams)
        {

        }

        #region ListDiscountBindProp
        private ObservableCollection<DiscountDto> _ListDiscountBindProp;
        public ObservableCollection<DiscountDto> ListDiscountBindProp
        {
            get { return _ListDiscountBindProp; }
            set { SetProperty(ref _ListDiscountBindProp, value); }
        }
        #endregion

        #region ItemBindProp
        private ItemDto _ItemBindProp;
        public ItemDto ItemBindProp
        {
            get { return _ItemBindProp; }
            set { SetProperty(ref _ItemBindProp, value); }
        }
        #endregion

        #region ItemForInvoiceBindProp
        private ItemForInvoiceDto _ItemForInvoiceBindProp;
        public ItemForInvoiceDto ItemForInvoiceBindProp
        {
            get { return _ItemForInvoiceBindProp; }
            set { SetProperty(ref _ItemForInvoiceBindProp, value); }
        }
        #endregion

        #region SaveCommand

        public DelegateCommand<object> SaveCommand { get; private set; }
        private async void OnSave(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                ItemForInvoiceBindProp.SubItems.Add(ItemBindProp);
                var param = new NavigationParameters();
                param.Add("item", ItemForInvoiceBindProp);
                await NavigationService.GoBackAsync(param);
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
        private void InitSaveCommand()
        {
            SaveCommand = new DelegateCommand<object>(OnSave);
            SaveCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region SelectDiscountCommand

        public DelegateCommand<DiscountDto> SelectDiscountCommand { get; private set; }
        private async void OnSelectDiscount(DiscountDto obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
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
        private void InitSelectDiscountCommand()
        {
            SelectDiscountCommand = new DelegateCommand<DiscountDto>(OnSelectDiscount);
            SelectDiscountCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            switch (parameters.GetNavigationMode())
            {
                case NavigationMode.Back:
                    break;
                case NavigationMode.New:
                    ItemForInvoiceBindProp = new ItemForInvoiceDto(ItemBindProp);
                    break;
                case NavigationMode.Forward:
                    break;
                case NavigationMode.Refresh:
                    break;
                default:
                    break;
            }
        }
    }
}
