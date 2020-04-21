using Dtos;
using Mobile.Models;
using Newtonsoft.Json;
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
            ListCategoryBindProp = new ObservableCollection<CategoryDto>();
        }

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
                var json = JsonConvert.SerializeObject(CategoryBindProp);
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

        #region ModifyCategoryCommand

        public DelegateCommand<CategoryDto> ModifyCategoryCommand { get; private set; }
        private async void OnModifyCategory(CategoryDto category)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                //var category = obj as CategoryDto;
                CategoryBindProp = category;
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
        private void InitModifyCategoryCommand()
        {
            ModifyCategoryCommand = new DelegateCommand<CategoryDto>(OnModifyCategory);
            ModifyCategoryCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion


        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            switch (parameters.GetNavigationMode())
            {
                case NavigationMode.Back:
                    break;
                case NavigationMode.New:
                    IsBusy = true;
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
                    IsBusy = false;
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
