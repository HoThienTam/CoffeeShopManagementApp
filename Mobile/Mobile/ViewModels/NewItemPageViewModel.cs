using Dtos;
using Mobile.Models;
using Mobile.Views;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Telerik.XamarinForms.Input.Calendar;

namespace Mobile.ViewModels
{
    public class NewItemPageViewModel : ViewModelBase
    {
        public NewItemPageViewModel(InitParams initParams) : base(initParams)
        {
            Title = "Mặt hàng mới";
        }

        #region ItemBindProp
        private ItemDto _ItemBindProp = new ItemDto();
        public ItemDto ItemBindProp
        {
            get { return _ItemBindProp; }
            set { SetProperty(ref _ItemBindProp, value); }
        }
        #endregion  

        #region TempItem
        private ItemDto _TempItem;
        public ItemDto TempItem
        {
            get { return _TempItem; }
            set { SetProperty(ref _TempItem, value); }
        }
        #endregion

        #region IsEditing
        private bool _IsEditing;
        public bool IsEditing
        {
            get { return _IsEditing; }
            set { SetProperty(ref _IsEditing, value); }
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
                TempItem.CategoryId = CategoryBindProp.Id;
                var json = JsonConvert.SerializeObject(TempItem);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                // Thuc hien cong viec tai day
                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    if (!IsEditing)
                    {
                        response = await client.PostAsync(Properties.Resources.BaseUrl + "items/", content);
                        if (response.IsSuccessStatusCode)
                        {
                            var item = JsonConvert.DeserializeObject<ItemDto>(await response.Content.ReadAsStringAsync());
                            var param = new NavigationParameters();
                            param.Add(nameof(ItemBindProp), item);
                            await NavigationService.GoBackAsync(param);
                        }
                    }
                    else
                    {
                        response = await client.PutAsync(Properties.Resources.BaseUrl + "items/", content);
                        if (response.IsSuccessStatusCode)
                        {
                            ItemBindProp.Name = TempItem.Name;
                            ItemBindProp.Image = TempItem.Image;
                            ItemBindProp.IsManaged = TempItem.IsManaged;
                            ItemBindProp.Price = TempItem.Price;
                            if (TempItem.IsManaged)
                            {
                                ItemBindProp.MinQuantity = TempItem.MinQuantity;
                            }
                            else
                            {
                                ItemBindProp.CurrentQuantity = 0;
                                ItemBindProp.MinQuantity = 0;
                            }
                            await NavigationService.GoBackAsync();
                        }
                    }
                };
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

        #region DeleteCommand

        public DelegateCommand<object> DeleteCommand { get; private set; }
        private async void OnDelete(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                var ok = await DisplayDeleteAlertAsync();
                if (ok)
                {
                    using (var client = new HttpClient())
                    {
                        HttpResponseMessage response = new HttpResponseMessage();

                        response = await client.DeleteAsync(Properties.Resources.BaseUrl + "items/" + TempItem.Id);
                        if (response.IsSuccessStatusCode)
                        {
                            var param = new NavigationParameters();
                            param.Add("Deleted", ItemBindProp);
                            await NavigationService.GoBackAsync(param);
                        }
                    };
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
        private void InitDeleteCommand()
        {
            DeleteCommand = new DelegateCommand<object>(OnDelete);
            DeleteCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region SelectItemImageCommand

        public DelegateCommand<object> SelectItemImageCommand { get; private set; }
        private async void OnSelectItemImage(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                await NavigationService.NavigateAsync(nameof(ItemPicturePage));
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
        private void InitSelectItemImageCommand()
        {
            SelectItemImageCommand = new DelegateCommand<object>(OnSelectItemImage);
            SelectItemImageCommand.ObservesCanExecute(() => IsNotBusy);
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
                    TempItem = new ItemDto(ItemBindProp);
                    if (TempItem.Id != Guid.Empty)
                    {
                        IsEditing = true;
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
