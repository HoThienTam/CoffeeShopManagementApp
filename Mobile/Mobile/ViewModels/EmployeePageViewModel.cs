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

        #region IsSelfBindProp
        private bool _IsSelfBindProp;
        public bool IsSelfBindProp
        {
            get { return _IsSelfBindProp; }
            set { SetProperty(ref _IsSelfBindProp, value); }
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
                ListEmployeeBindProp.ToList().ForEach(c => c.IsSelected = false);
                var id = Xamarin.Essentials.Preferences.Get("token", string.Empty);
                if (obj.Id == Guid.Parse(id))
                {
                    IsSelfBindProp = true;
                }
                else
                {
                    IsSelfBindProp = false;
                }
                obj.IsSelected = true;
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

        #region UpdateEmployeeCommand

        public DelegateCommand<object> UpdateEmployeeCommand { get; private set; }
        private async void OnUpdateEmployee(object obj)
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
        private void InitUpdateEmployeeCommand()
        {
            UpdateEmployeeCommand = new DelegateCommand<object>(OnUpdateEmployee);
            UpdateEmployeeCommand.ObservesCanExecute(() => IsNotBusy);
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
                    EmployeeBindProp = null;
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
                    break;
                case NavigationMode.New:
                    using (var client = new HttpClient())
                    {
                        var response = await client.GetAsync(Properties.Resources.BaseUrl + "users/");
                        var id = Xamarin.Essentials.Preferences.Get("token", string.Empty);
                        if (response.IsSuccessStatusCode)
                        {
                            var employees = JsonConvert.DeserializeObject<IEnumerable<UserDto>>(await response.Content.ReadAsStringAsync());
                            foreach (var employee in employees)
                            {
                                ListEmployeeBindProp.Add(employee);
                            }
                            var em = ListEmployeeBindProp.FirstOrDefault(e => e.Id == Guid.Parse(id));
                            em.IsSelected = true;
                            EmployeeBindProp = em;
                            IsSelfBindProp = true;
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
