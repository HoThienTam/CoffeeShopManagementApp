using Dtos;
using Mobile.Models;
using Mobile.Views;
using Newtonsoft.Json;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace Mobile.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        public LoginPageViewModel(InitParams initParams) : base(initParams)
        {
        }
        #region UsernameBindProp
        private string _UsernameBindProp = null;
        public string UsernameBindProp
        {
            get { return _UsernameBindProp; }
            set { SetProperty(ref _UsernameBindProp, value); }
        }
        #endregion

        #region PasswordBindProp
        private string _PasswordBindProp = null;
        public string PasswordBindProp
        {
            get { return _PasswordBindProp; }
            set { SetProperty(ref _PasswordBindProp, value); }
        }
        #endregion

        #region LoginCommand

        public DelegateCommand<object> LoginCommand { get; private set; }
        private async void OnLogin(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                var user = new LoginDto 
                { 
                    Password = PasswordBindProp, 
                    Username = UsernameBindProp 
                };

                var json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync(Properties.Resources.BaseUrl + "users/login", content);
                    var token = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(token))
                    {
                        Application.Current.Properties["token"] = token;
                        await Application.Current.SavePropertiesAsync();
                    }
                }

                await NavigationService.NavigateAsync(nameof(MainPage));
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
        private void InitLoginCommand()
        {
            LoginCommand = new DelegateCommand<object>(OnLogin);
            LoginCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

    }
}
