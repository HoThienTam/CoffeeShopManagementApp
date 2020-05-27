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

        }

        #region ListCategoryBindProp
        private ObservableCollection<CategoryDto> _ListCategoryBindProp = null;
        public ObservableCollection<CategoryDto> ListCategoryBindProp
        {
            get { return _ListCategoryBindProp; }
            set { SetProperty(ref _ListCategoryBindProp, value); }
        }
        #endregion

        #region AddNewCategoryCommand

        public DelegateCommand<object> AddNewCategoryCommand { get; private set; }
        private async void OnAddNewCategory(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                var categoryToCreate = new CategoryDto { Name = "New Category", Icon = FontAwesomeIcon.Coffee };
                var json = JsonConvert.SerializeObject(categoryToCreate);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                // Thuc hien cong viec tai day
                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    response = await client.PostAsync(Properties.Resources.BaseUrl + "categories/", content);
                    if (response.IsSuccessStatusCode)
                    {
                        var category = JsonConvert.DeserializeObject<CategoryDto>(await response.Content.ReadAsStringAsync());
                        ListCategoryBindProp.Add(category);
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
        private void InitAddNewCategoryCommand()
        {
            AddNewCategoryCommand = new DelegateCommand<object>(OnAddNewCategory);
            AddNewCategoryCommand.ObservesCanExecute(() => IsNotBusy);
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
                var param = new NavigationParameters();
                param.Add("CategoryBindProp", obj);
                await NavigationService.NavigateAsync(nameof(ItemPage), param);
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

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            switch (parameters.GetNavigationMode())
            {
                case NavigationMode.Back:
                    if (parameters.ContainsKey("CategoryBindProp"))
                    {
                        var category = parameters["CategoryBindProp"] as CategoryDto;
                        ListCategoryBindProp.Remove(category);
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
