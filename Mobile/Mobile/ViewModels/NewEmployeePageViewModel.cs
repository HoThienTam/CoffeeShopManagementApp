using Dtos;
using ImTools;
using Mobile.Enums;
using Mobile.Models;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Mobile.ViewModels
{
    public class NewEmployeePageViewModel : ViewModelBase
    {
        public NewEmployeePageViewModel(InitParams initParams) : base(initParams)
        {
            Title = "Nhân viên mới";
        } 

        #region IsEditing
        private bool _IsEditing;
        public bool IsEditing
        {
            get { return _IsEditing; }
            set { SetProperty(ref _IsEditing, value); }
        }
        #endregion

        #region EmployeeBindProp
        private UserDto _EmployeeBindProp = new UserDto();
        public UserDto EmployeeBindProp
        {
            get { return _EmployeeBindProp; }
            set { SetProperty(ref _EmployeeBindProp, value); }
        }
        #endregion

        #region TempEmployee
        private UserDto _TempEmployee;
        public UserDto TempEmployee
        {
            get { return _TempEmployee; }
            set { SetProperty(ref _TempEmployee, value); }
        }
        #endregion

        #region PasswordBindProp
        private string _PasswordBindProp;
        public string PasswordBindProp
        {
            get { return _PasswordBindProp; }
            set { SetProperty(ref _PasswordBindProp, value); }
        }
        #endregion

        #region ConfirmedPasswordBindProp
        private string _ConfirmedPasswordBindProp;
        public string ConfirmedPasswordBindProp
        {
            get { return _ConfirmedPasswordBindProp; }
            set { SetProperty(ref _ConfirmedPasswordBindProp, value); }
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
                if (string.IsNullOrWhiteSpace(TempEmployee.Username))
                {
                    await PageDialogService.DisplayAlertAsync("Lỗi", "Tên tài khoản không được để trống", "Đóng");
                }
                else if (string.IsNullOrWhiteSpace(TempEmployee.Fullname))
                {
                    await PageDialogService.DisplayAlertAsync("Lỗi", "Tên nhân viên không được để trống", "Đóng");
                }
                else if (string.IsNullOrWhiteSpace(PasswordBindProp) || PasswordBindProp.Length < 8)
                {
                    await PageDialogService.DisplayAlertAsync("Lỗi", "Mật khẩu phải tối thiểu 8 ký tự", "Đóng");
                }
                else if (!ConfirmedPasswordBindProp.SequenceEqual(PasswordBindProp))
                {
                    await PageDialogService.DisplayAlertAsync("Lỗi", "Mật khẩu xác nhận không trùng khớp", "Đóng");
                }
                else
                {
                    TempEmployee.Password = PasswordBindProp;
                    TempEmployee.Username = TempEmployee.Username.ToLower().Trim();
                    var json = JsonConvert.SerializeObject(TempEmployee);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    // Thuc hien cong viec tai day
                    using (var client = new HttpClient())
                    {
                        HttpResponseMessage response = new HttpResponseMessage();
                        if (!IsEditing)
                        {
                            response = await client.PostAsync(Properties.Resources.BaseUrl + "users/register", content);
                            if (response.IsSuccessStatusCode)
                            {
                                var user = JsonConvert.DeserializeObject<UserDto>(await response.Content.ReadAsStringAsync());
                                var param = new NavigationParameters();
                                param.Add(nameof(EmployeeBindProp), user);
                                await NavigationService.GoBackAsync(param);
                            }
                        }
                        else
                        {
                            response = await client.PutAsync(Properties.Resources.BaseUrl + "users/", content);
                            if (response.IsSuccessStatusCode)
                            {
                                EmployeeBindProp.Fullname = TempEmployee.Fullname;
                                EmployeeBindProp.Role = TempEmployee.Role;
                                await NavigationService.GoBackAsync();
                            }
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
        private void InitSaveCommand()
        {
            SaveCommand = new DelegateCommand<object>(OnSave);
            SaveCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region SelectedChangedCommand

        public DelegateCommand<string> SelectedChangedCommand { get; private set; }
        private async void OnSelectedChanged(string obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day         
                switch (obj)
                {
                    case "Nhân viên":
                        TempEmployee.Role = (int)Role.Staff;
                        break;
                    case "Quản lý":
                        TempEmployee.Role = (int)Role.Manager;
                        break;
                    default:
                        TempEmployee.Role = (int)Role.Staff;
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
        private void InitSelectedChangedCommand()
        {
            SelectedChangedCommand = new DelegateCommand<string>(OnSelectedChanged);
            SelectedChangedCommand.ObservesCanExecute(() => IsNotBusy);
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
                    TempEmployee = new UserDto(EmployeeBindProp);
                    if (TempEmployee.Id != Guid.Empty)
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
