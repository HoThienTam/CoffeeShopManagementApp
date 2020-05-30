using Dtos;
using Mobile.Models;
using Mobile.Views;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Mobile.ViewModels
{
    public class EmployeePageViewModel : ViewModelBase
    {
        public EmployeePageViewModel(InitParams initParams) : base(initParams)
        {
            ListEmployeeBindProp = new ObservableCollection<UserDto>();
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

        #region AddNewEmployeeCommand

        public DelegateCommand<object> AddNewEmployeeCommand { get; private set; }
        private async void OnAddNewEmployee(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                await NavigationService.NavigateAsync(nameof(NewEmployeePage));
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
        private void InitAddNewEmployeeCommand()
        {
            AddNewEmployeeCommand = new DelegateCommand<object>(OnAddNewEmployee);
            AddNewEmployeeCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region SelectEmployeeCommand

        public DelegateCommand<UserDto> SelectEmployeeCommand { get; private set; }
        private async void OnSelectEmployee(UserDto obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                EmployeeBindProp = obj;
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
        private void InitSelectEmployeeCommand()
        {
            SelectEmployeeCommand = new DelegateCommand<UserDto>(OnSelectEmployee);
            SelectEmployeeCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region ChangePasswordCommand

        public DelegateCommand<object> ChangePasswordCommand { get; private set; }
        private async void OnChangePassword(object obj)
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
                param.Add(nameof(EmployeeBindProp), EmployeeBindProp);
                await NavigationService.NavigateAsync(nameof(NewEmployeePage), param);
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
        private void InitChangePasswordCommand()
        {
            ChangePasswordCommand = new DelegateCommand<object>(OnChangePassword);
            ChangePasswordCommand.ObservesCanExecute(() => IsNotBusy);
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
                    ListEmployeeBindProp.Remove(EmployeeBindProp);
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

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            switch (parameters.GetNavigationMode())
            {
                case NavigationMode.Back:
                    if (parameters.ContainsKey(nameof(EmployeeBindProp)))
                    {
                        var employee = parameters[nameof(EmployeeBindProp)] as UserDto;
                        ListEmployeeBindProp.Add(employee);
                    }
                    if (parameters.ContainsKey("IsDeleted"))
                    {
                        var employee = parameters["IsDeleted"] as UserDto;
                        ListEmployeeBindProp.Remove(employee);
                    }
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
