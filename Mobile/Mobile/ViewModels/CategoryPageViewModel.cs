using Dtos;
using Mobile.Models;
using Mobile.Views;
using Newtonsoft.Json;
using Prism.AppModel;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Mobile.ViewModels
{
    public class CategoryPageViewModel : ViewModelBase
    {
        public CategoryPageViewModel(InitParams initParams) : base(initParams)
        {
            ListIconBindProp = new ObservableCollection<Icon>();
            ListIconBindProp.Add(new Icon { Name = FontAwesomeIcon.Coffee, IsSelected = true });
            ListIconBindProp.Add(new Icon { Name = FontAwesomeIcon.CoffeeTogo, IsSelected = false });
            ListIconBindProp.Add(new Icon { Name = FontAwesomeIcon.Cocktail, IsSelected = false });
            ListIconBindProp.Add(new Icon { Name = FontAwesomeIcon.FrenchFries, IsSelected = false });
            ListIconBindProp.Add(new Icon { Name = FontAwesomeIcon.Beer, IsSelected = false });
            ListIconBindProp.Add(new Icon { Name = FontAwesomeIcon.BreadSlice, IsSelected = false });
            ListIconBindProp.Add(new Icon { Name = FontAwesomeIcon.Pie, IsSelected = false });
            ListIconBindProp.Add(new Icon { Name = FontAwesomeIcon.Soup, IsSelected = false });
            ListIconBindProp.Add(new Icon { Name = FontAwesomeIcon.Salad, IsSelected = false });
            ListIconBindProp.Add(new Icon { Name = FontAwesomeIcon.WineGlass, IsSelected = false });
            ListIconBindProp.Add(new Icon { Name = FontAwesomeIcon.IceCream, IsSelected = false });
            ListIconBindProp.Add(new Icon { Name = FontAwesomeIcon.Pizza, IsSelected = false });
            ListIconBindProp.Add(new Icon { Name = FontAwesomeIcon.Burrito, IsSelected = false });
            ListIconBindProp.Add(new Icon { Name = FontAwesomeIcon.Sandwich, IsSelected = false });
        }

        #region ListCategoryBindProp
        private ObservableCollection<CategoryDto> _ListCategoryBindProp = null;
        public ObservableCollection<CategoryDto> ListCategoryBindProp
        {
            get { return _ListCategoryBindProp; }
            set { SetProperty(ref _ListCategoryBindProp, value); }
        }
        #endregion

        #region ListIconBindProp
        private ObservableCollection<Icon> _ListIconBindProp = null;
        public ObservableCollection<Icon> ListIconBindProp
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
        private CategoryDto _TempCategory = null;
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

        #region IsEditing
        private bool _IsEditing = true;
        public bool IsEditing
        {
            get { return _IsEditing; }
            set { SetProperty(ref _IsEditing, value); }
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
                    CategoryBindProp = new CategoryDto();
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
                CategoryBindProp.Icon = FontAwesomeIcon.Coffee;
                var categoryToCreate = new CategoryForCreateDto(CategoryBindProp);
                var json = JsonConvert.SerializeObject(categoryToCreate);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                // Thuc hien cong viec tai day
                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    if (CategoryBindProp.Id == Guid.Empty)
                    {
                        response = await client.PostAsync(Properties.Resources.BaseUrl + "categories/", content);
                    }
                    else
                    {
                        response = await client.PutAsync(Properties.Resources.BaseUrl + "categories/", content);
                    }
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.NoContent:
                            TempCategory.Name = CategoryBindProp.Name;
                            TempCategory.Icon = CategoryBindProp.Icon;
                            break;
                        case HttpStatusCode.Created:
                            var category = JsonConvert.DeserializeObject<CategoryDto>(await response.Content.ReadAsStringAsync());
                            ListCategoryBindProp.Add(category);
                            CategoryBindProp = new CategoryDto();
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

        #region SelectCategoryCommand

        public DelegateCommand<CategoryDto> SelectCategoryCommand { get; private set; }
        private async void OnSelectCategory(CategoryDto category)
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
                    TempCategory = category;
                    CategoryBindProp = new CategoryDto(category);
                    var icon = ListIconBindProp.FirstOrDefault(i => i.IsSelected = true);
                    icon.IsSelected = false;
                    ListIconBindProp.FirstOrDefault(i => i.Name == category.Icon).IsSelected = true;
                    IsOpen = true;
                }
                else
                {
                    CategoryBindProp.Name = category.Name;
                    CategoryBindProp.Id = category.Id;
                    var param = new NavigationParameters();
                    param.Add("Category", CategoryBindProp);
                    await NavigationService.GoBackAsync(param);
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
        private void InitModifyCategoryCommand()
        {
            SelectCategoryCommand = new DelegateCommand<CategoryDto>(OnSelectCategory);
            SelectCategoryCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region DeleteCategoryCommand

        public DelegateCommand<object> DeleteCategoryCommand { get; private set; }
        private async void OnDeleteCategory(object obj)
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
                    response = await client.DeleteAsync(Properties.Resources.BaseUrl + "categories/" + CategoryBindProp.Id);

                    if (response.IsSuccessStatusCode)
                    {
                        ListCategoryBindProp.Remove(TempCategory);
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
        private void InitDeleteCategoryCommand()
        {
            DeleteCategoryCommand = new DelegateCommand<object>(OnDeleteCategory);
            DeleteCategoryCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region SelectIconCommand

        public DelegateCommand<Icon> SelectIconCommand { get; private set; }
        private async void OnSelectIcon(Icon icon)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                var selectedIcon = ListIconBindProp.FirstOrDefault(i => i.IsSelected = true);
                selectedIcon.IsSelected = false;

                icon.IsSelected = true;
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
            SelectIconCommand = new DelegateCommand<Icon>(OnSelectIcon);
            SelectIconCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            switch (parameters.GetNavigationMode())
            {
                case NavigationMode.Back:
                    break;
                case NavigationMode.New:
                    if (parameters.ContainsKey("Page") && parameters["Page"] as string == nameof(ItemPage))
                    {
                        IsEditing = false;
                        CategoryBindProp = new CategoryDto();
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
