using Dtos;
using Mobile.Models;
using Mobile.Views;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;

namespace Mobile.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel(InitParams initParams) : base(initParams)
        {
            ListCategoryBindProp = new ObservableCollection<CategoryDto>();
            ListItemBindProp = new ObservableCollection<ItemDto>();
        }

        #region ListZoneBindProp
        private ObservableCollection<string> _ListZoneBindProp = null;
        public ObservableCollection<string> ListZoneBindProp
        {
            get { return _ListZoneBindProp; }
            set { SetProperty(ref _ListZoneBindProp, value); }
        }
        #endregion

        #region ListInvoiceBindProp
        private ObservableCollection<string> _ListInvoiceBindProp = null;
        public ObservableCollection<string> ListInvoiceBindProp
        {
            get { return _ListInvoiceBindProp; }
            set { SetProperty(ref _ListInvoiceBindProp, value); }
        }
        #endregion

        #region ListCategoryBindProp
        private ObservableCollection<CategoryDto> _ListCategoryBindProp = null;
        public ObservableCollection<CategoryDto> ListCategoryBindProp
        {
            get { return _ListCategoryBindProp; }
            set { SetProperty(ref _ListCategoryBindProp, value); }
        }
        #endregion

        #region ListItemBindProp
        private ObservableCollection<ItemDto> _ListItemBindProp = null;
        public ObservableCollection<ItemDto> ListItemBindProp
        {
            get { return _ListItemBindProp; }
            set { SetProperty(ref _ListItemBindProp, value); }
        }
        #endregion

        #region IsEditing
        private bool _IsEditing = false;
        public bool IsEditing
        {
            get { return _IsEditing; }
            set
            {
                SetProperty(ref _IsEditing, value);
                RaisePropertyChanged(nameof(IsNotEditting));
            }
        }


        public bool IsNotEditting { get { return !_IsEditing; } }
        #endregion

        #region ChangeEditModeCommand

        public DelegateCommand<object> ChangeEditModeCommand { get; private set; }
        private async void OnChangeEditMode(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                if (IsEditing)
                {
                    IsEditing = false;
                }
                else
                {
                    IsEditing = true;
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
        private void InitChangeEditModeCommand()
        {
            ChangeEditModeCommand = new DelegateCommand<object>(OnChangeEditMode);
            ChangeEditModeCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region SelectSettingCommand

        public DelegateCommand<string> SelectSettingCommand { get; private set; }
        private async void OnSelectSetting(string settings)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                var param = new NavigationParameters();
                param.Add(nameof(ListCategoryBindProp), ListCategoryBindProp);
                switch (settings)
                {
                    case "Category":                      
                        await NavigationService.NavigateAsync(nameof(CategoryPage), param);
                        break;
                    case "Item":
                        await NavigationService.NavigateAsync(nameof(ItemPage), param);
                        break;
                    default:
                        break;
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
        private void InitSelectSettingCommand()
        {
            SelectSettingCommand = new DelegateCommand<string>(OnSelectSetting);
            SelectSettingCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion


        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            switch (parameters.GetNavigationMode())
            {
                case NavigationMode.Back:
                    break;
                case NavigationMode.New:
                    using (var client = new HttpClient())
                    {
                        var response = await client.GetAsync(Properties.Resources.BaseUrl + "categories/");
                        if (response.IsSuccessStatusCode)
                        {
                            var categories = JsonConvert.DeserializeObject<IEnumerable<CategoryDto>>(await response.Content.ReadAsStringAsync());
                            foreach (var category in categories)
                            {
                                ListCategoryBindProp.Add(category);
                            }
                        }
                        else
                        {
                            await PageDialogService.DisplayAlertAsync("Lỗi", $"{await response.Content.ReadAsStringAsync()}", "Đóng");
                        }
                    }
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
