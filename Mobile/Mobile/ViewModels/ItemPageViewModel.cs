using Dtos;
using Mobile.Models;
using Mobile.Views;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.SymbolStore;
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
            ListIconBindProp = new List<string>();
            ListIconBindProp.Add(FontAwesomeIcon.Coffee);
            ListIconBindProp.Add(FontAwesomeIcon.CoffeeTogo);
            ListIconBindProp.Add(FontAwesomeIcon.Cocktail);
            ListIconBindProp.Add(FontAwesomeIcon.FrenchFries);
            ListIconBindProp.Add(FontAwesomeIcon.Beer);
            ListIconBindProp.Add(FontAwesomeIcon.BreadSlice);
            ListIconBindProp.Add(FontAwesomeIcon.Pie);
            ListIconBindProp.Add(FontAwesomeIcon.Soup);
            ListIconBindProp.Add(FontAwesomeIcon.Salad);
            ListIconBindProp.Add(FontAwesomeIcon.IceCream);
            ListIconBindProp.Add(FontAwesomeIcon.Pizza);
            ListIconBindProp.Add(FontAwesomeIcon.Sandwich);
        }

        #region ListIconBindProp
        private List<string> _ListIconBindProp;
        public List<string> ListIconBindProp
        {
            get { return _ListIconBindProp; }
            set { SetProperty(ref _ListIconBindProp, value); }
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

        #region TempCategory
        private CategoryDto _TempCategory;
        public CategoryDto TempCategory
        {
            get { return _TempCategory; }
            set { SetProperty(ref _TempCategory, value); }
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

        #region SaveCommand

        public DelegateCommand<object> SaveCommand { get; private set; }
        private bool CanExecuteSave(object obj)
        {
            if (IsBusy)
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(CategoryBindProp.Name))
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
                var json = JsonConvert.SerializeObject(TempCategory);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                // Thuc hien cong viec tai day
                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    response = await client.PutAsync(Properties.Resources.BaseUrl + "categories/", content);
                    if (response.IsSuccessStatusCode)
                    {
                        CategoryBindProp.Name = TempCategory.Name;
                        CategoryBindProp.Icon = TempCategory.Icon;
                        CategoryBindProp.Items = TempCategory.Items;
                        await NavigationService.GoBackAsync();
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
            SaveCommand = new DelegateCommand<object>(OnSave, CanExecuteSave);
            SaveCommand.ObservesProperty(() => IsNotBusy);
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
                var param = new NavigationParameters();
                param.Add(nameof(CategoryBindProp), CategoryBindProp);
                param.Add("ItemBindProp", obj);
                await NavigationService.NavigateAsync(nameof(NewItemPage), param);
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

        #region AddNewItemCommand

        public DelegateCommand<object> AddNewItemCommand { get; private set; }
        private async void OnAddNewItem(object obj)
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
                param.Add(nameof(CategoryBindProp), CategoryBindProp);
                await NavigationService.NavigateAsync(nameof(NewItemPage), param);
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
        private void InitAddNewItemCommand()
        {
            AddNewItemCommand = new DelegateCommand<object>(OnAddNewItem);
            AddNewItemCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region ChangeIconCommand

        public DelegateCommand<object> ChangeIconCommand { get; private set; }
        private async void OnChangeIcon(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
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
        private void InitChangeIconCommand()
        {
            ChangeIconCommand = new DelegateCommand<object>(OnChangeIcon);
            ChangeIconCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region SelectIconCommand

        public DelegateCommand<string> SelectIconCommand { get; private set; }
        private async void OnSelectIcon(string obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                TempCategory.Icon = obj;
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
        private void InitSelectIconCommand()
        {
            SelectIconCommand = new DelegateCommand<string>(OnSelectIcon);
            SelectIconCommand.ObservesCanExecute(() => IsNotBusy);
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

                        response = await client.DeleteAsync(Properties.Resources.BaseUrl + "categories/" + TempCategory.Id);
                        if (response.IsSuccessStatusCode)
                        {
                            var param = new NavigationParameters();
                            param.Add(nameof(CategoryBindProp), CategoryBindProp);
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

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            switch (parameters.GetNavigationMode())
            {
                case NavigationMode.Back:
                    if (parameters.ContainsKey("ItemBindProp"))
                    {
                        var item = parameters["ItemBindProp"] as ItemDto;
                        TempCategory.Items.Add(item);
                        CategoryBindProp.Items.Add(item);
                    }
                    if (parameters.ContainsKey("Deleted"))
                    {
                        var item = parameters["Deleted"] as ItemDto;
                        TempCategory.Items.Remove(item);
                        CategoryBindProp.Items.Remove(item);
                    }
                    break;
                case NavigationMode.New:
                    TempCategory = new CategoryDto(CategoryBindProp);
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
