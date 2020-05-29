using Dtos;
using Mobile.Models;
using Newtonsoft.Json;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;

namespace Mobile.ViewModels
{
    public class EmployeePageViewModel : ViewModelBase
    {
        public EmployeePageViewModel(InitParams initParams) : base(initParams)
        {
        }

        #region EmployeeBindProp
        private UserDto _EmployeeBindProp;
        public UserDto EmployeeBindProp
        {
            get { return _EmployeeBindProp; }
            set { SetProperty(ref _EmployeeBindProp, value); }
        }
        #endregion

        #region ListEmployeeBindProp
        private ObservableCollection<UserDto> _ListEmployeeBindProp;
        public ObservableCollection<UserDto> ListEmployeeBindProp
        {
            get { return _ListEmployeeBindProp; }
            set { SetProperty(ref _ListEmployeeBindProp, value); }
        }
        #endregion

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            switch (parameters.GetNavigationMode())
            {
                case NavigationMode.Back:
                    break;
                case NavigationMode.New:
                    using (var client = new HttpClient())
                    {
                        var response = await client.GetAsync(Properties.Resources.BaseUrl + "users/");
                        if (response.IsSuccessStatusCode)
                        {
                            var employees = JsonConvert.DeserializeObject<IEnumerable<UserDto>>(await response.Content.ReadAsStringAsync());
                            foreach (var employee in employees)
                            {
                                ListEmployeeBindProp.Add(employee);
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
