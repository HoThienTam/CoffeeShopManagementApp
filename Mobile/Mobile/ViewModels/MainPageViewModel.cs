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
using System.Threading.Tasks;
using Telerik.XamarinForms.Input.Calendar;

namespace Mobile.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel(InitParams initParams) : base(initParams)
        {
            ListCategoryBindProp = new ObservableCollection<CategoryDto>();
            ListItemBindProp = new ObservableCollection<ItemDto>();
            ListDiscountBindProp = new ObservableCollection<DiscountDto>();
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

        #region ListDiscountBindProp
        private ObservableCollection<DiscountDto> _ListDiscountBindProp = null;
        public ObservableCollection<DiscountDto> ListDiscountBindProp
        {
            get { return _ListDiscountBindProp; }
            set { SetProperty(ref _ListDiscountBindProp, value); }
        }
        #endregion

        #region CurrentCategory
        private CategoryDto _CurrentCategory;
        public CategoryDto CurrentCategory
        {
            get { return _CurrentCategory; }
            set { SetProperty(ref _CurrentCategory, value); }
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

        #region BillBindProp
        private string _BillBindProp = null;
        public string BillBindProp
        {
            get { return _BillBindProp; }
            set { SetProperty(ref _BillBindProp, value); }
        }
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

                switch (settings)
                {
                    case "Category":
                        param.Add(nameof(ListCategoryBindProp), ListCategoryBindProp);
                        await NavigationService.NavigateAsync(nameof(CategoryPage), param);
                        break;
                    case "Item":
                        param.Add(nameof(ListCategoryBindProp), ListCategoryBindProp);
                        param.Add(nameof(ListItemBindProp), ListItemBindProp);
                        await NavigationService.NavigateAsync(nameof(ItemPage), param);
                        break;
                    case "Discount":
                        param.Add(nameof(ListDiscountBindProp), ListDiscountBindProp);
                        await NavigationService.NavigateAsync(nameof(DiscountPage), param);
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

        #region SelectCategoryCommand

        public DelegateCommand<CategoryDto> SelectCategoryCommand { get; private set; }
        private async void OnSelectCategory(CategoryDto obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                CurrentCategory = obj;
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
        private void InitSelectCategoryCommand()
        {
            SelectCategoryCommand = new DelegateCommand<CategoryDto>(OnSelectCategory);
            SelectCategoryCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region SelectItemCommand

        public DelegateCommand<ItemDto> SelectItemCommand { get; private set; }
        private async void OnSelectItem(ItemDto obj)
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
        private void InitSelectItemCommand()
        {
            SelectItemCommand = new DelegateCommand<ItemDto>(OnSelectItem);
            SelectItemCommand.ObservesCanExecute(() => IsNotBusy);
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
                        var categoryTask = await client.GetAsync(Properties.Resources.BaseUrl + "categories/");
                        var itemTask = await client.GetAsync(Properties.Resources.BaseUrl + "items/");
                        var discountTask = await client.GetAsync(Properties.Resources.BaseUrl + "discounts/");
                        if (categoryTask.IsSuccessStatusCode)
                        {
                            var categories = JsonConvert.DeserializeObject<IEnumerable<CategoryDto>>(await categoryTask.Content.ReadAsStringAsync());
                            foreach (var category in categories)
                            {
                                ListCategoryBindProp.Add(category);
                            }
                        }
                        else
                        {
                            await PageDialogService.DisplayAlertAsync("Lỗi", $"{await categoryTask.Content.ReadAsStringAsync()}", "Đóng");
                        }
                        if (itemTask.IsSuccessStatusCode)
                        {
                            var items = JsonConvert.DeserializeObject<IEnumerable<ItemDto>>(await itemTask.Content.ReadAsStringAsync());
                            foreach (var item in items)
                            {
                                ListItemBindProp.Add(item);
                            }
                        }
                        else
                        {
                            await PageDialogService.DisplayAlertAsync("Lỗi", $"{await itemTask.Content.ReadAsStringAsync()}", "Đóng");
                        }
                        if (discountTask.IsSuccessStatusCode)
                        {
                            var discounts = JsonConvert.DeserializeObject<IEnumerable<DiscountDto>>(await discountTask.Content.ReadAsStringAsync());
                            foreach (var discount in discounts)
                            {
                                ListDiscountBindProp.Add(discount);
                            }
                        }
                        else
                        {
                            await PageDialogService.DisplayAlertAsync("Lỗi", $"{await discountTask.Content.ReadAsStringAsync()}", "Đóng");
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
