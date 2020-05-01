using Dtos;
using Mobile.Models;
using Mobile.Views;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Text;
using Telerik.XamarinForms.Input.Calendar;

namespace Mobile.ViewModels
{
    public class ItemPageViewModel : ViewModelBase
    {
        public ItemPageViewModel(InitParams initParams) : base(initParams)
        {
        }

        #region ItemBindProp
        private ItemDto _ItemBindProp = null;
        public ItemDto ItemBindProp
        {
            get { return _ItemBindProp; }
            set { SetProperty(ref _ItemBindProp, value); }
        }
        #endregion

        #region TempItem
        private ItemDto _TempItem = null;
        public ItemDto TempItem
        {
            get { return _TempItem; }
            set { SetProperty(ref _TempItem, value); }
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

        #region CategoryBindProp
        private CategoryDto _CategoryBindProp = null;
        public CategoryDto CategoryBindProp
        {
            get { return _CategoryBindProp; }
            set { SetProperty(ref _CategoryBindProp, value); }
        }
        #endregion

        #region IsOpen
        private bool _IsOpen = false;
        public bool IsOpen
        {
            get { return _IsOpen; }
            set { SetProperty(ref _IsOpen, value); }
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
                    ItemBindProp = new ItemDto();
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

        #region SaveCommand

        public DelegateCommand<object> SaveCommand { get; private set; }
        private bool CanExecuteSave(object obj)
        {
            if (IsBusy)
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(ItemBindProp.Name))
            {
                return false;
            }
            return true;
        }
        private async void OnSave(object obj)
        {
            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                var itemForCreate = new ItemForCreateDto(ItemBindProp);
                itemForCreate.CategoryId = CategoryBindProp.Id;
                var json = JsonConvert.SerializeObject(itemForCreate);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                // Thuc hien cong viec tai day
                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    if (ItemBindProp.Id == Guid.Empty)
                    {
                        response = await client.PostAsync(Properties.Resources.BaseUrl + "items/", content);
                    }
                    else
                    {
                        response = await client.PutAsync(Properties.Resources.BaseUrl + "items/", content);
                    }
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.NoContent:
                            TempItem.Name = ItemBindProp.Name;
                            TempItem.Price = ItemBindProp.Price;
                            break;
                        case HttpStatusCode.Created:
                            var item = JsonConvert.DeserializeObject<ItemDto>(await response.Content.ReadAsStringAsync());
                            ListItemBindProp.Add(item);
                            ItemBindProp = new ItemDto();
                            break;
                        case HttpStatusCode.BadRequest:
                            await PageDialogService.DisplayAlertAsync("Lỗi", $"{await response.Content.ReadAsStringAsync()}", "Đóng");
                            break;
                        default:
                            await PageDialogService.DisplayAlertAsync("Lỗi", $"Lỗi hệ thống!", "Đóng");
                            break;
                    }
                };
                IsOpen = false;
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
            SaveCommand = new DelegateCommand<object>(OnSave, CanExecuteSave);
            SaveCommand.ObservesProperty(() => IsNotBusy);
        }

        #endregion

        #region SelectItemCommand

        public DelegateCommand<ItemDto> SelectItemCommand { get; private set; }
        private async void OnSelectItem(ItemDto itemDto)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                TempItem = itemDto;
                ItemBindProp = new ItemDto(itemDto);
                IsOpen = true;
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

        #region DeleteItemCommand

        public DelegateCommand<ItemDto> DeleteItemCommand { get; private set; }
        private async void OnDeleteItem(ItemDto itemDto)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    response = await client.DeleteAsync(Properties.Resources.BaseUrl + "items/" + ItemBindProp.Id);

                    if (response.IsSuccessStatusCode)
                    {
                        ListItemBindProp.Remove(TempItem);
                    }
                }
                IsOpen = false;
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
        private void InitDeleteItemCommand()
        {
            DeleteItemCommand = new DelegateCommand<ItemDto>(OnDeleteItem);
            DeleteItemCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region SelectCategoryCommand

        public DelegateCommand<object> SelectCategoryCommand { get; private set; }
        private async void OnSelectCategory(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                IsOpen = false;
                var param = new NavigationParameters();
                param.Add(nameof(ListCategoryBindProp), ListCategoryBindProp);
                param.Add(nameof(CategoryBindProp), CategoryBindProp);
                param.Add("Page", nameof(ItemPage));
                await NavigationService.NavigateAsync(nameof(CategoryPage), param);
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
            SelectCategoryCommand = new DelegateCommand<object>(OnSelectCategory);
            SelectCategoryCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            switch (parameters.GetNavigationMode())
            {
                case NavigationMode.Back:
                    IsOpen = true;
                    if (parameters.ContainsKey("Category"))
                    {
                        CategoryBindProp = parameters["Category"] as CategoryDto;
                    }
                    break;
                case NavigationMode.New:
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
