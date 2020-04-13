using Mobile.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mobile.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware
    {
        protected INavigationService NavigationService { get; private set; }
        protected IPageDialogService PageDialogService { get; private set; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        #region IsBusy
        private bool _IsBusy = false;
        public bool IsBusy
        {
            get { return _IsBusy; }
            set
            {
                if (SetProperty(ref _IsBusy, value))
                {
                    RaisePropertyChanged(nameof(IsNotBusy));
                }
            }
        }
        #endregion
        public bool IsNotBusy { get { return !_IsBusy; } }

        public ViewModelBase(InitParams initParams) : this()
        {
            NavigationService = initParams.NavigationService;
            PageDialogService = initParams.PageDialogService;
        }

        public ViewModelBase()
        {
            Type type = GetType();

            do
            {
                var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                                      .Where(h => h.GetCustomAttributes<InitializeAttribute>().Any())
                                      .ToList();
                foreach (var method in methods)
                {
                    method.Invoke(this, null);
                }

                if (type == typeof(BindableBase))
                {
                    break;
                }

                type = type.BaseType;
            } while (true);
        }

        #region GoBackCommand

        public DelegateCommand<object> GoBackCommand { get; private set; }
        private async void OnGoBack(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;
            try
            {
                await NavigationService.GoBackAsync();
            }
            catch (Exception ex)
            {
                await ShowErrorAsync(ex);
            }
            finally
            {
                IsBusy = false;
            }

        }
        [Initialize]
        private void InitGoBackCommand()
        {
            GoBackCommand = new DelegateCommand<object>(OnGoBack);
            GoBackCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        protected async Task ShowErrorAsync(Exception ex)
        {
            await PageDialogService.DisplayAlertAsync("Lỗi hệ thống", $"Đã có lỗi trong quá trình xử lý:\n{ex.Message}", "Đóng");
        }

        protected async Task<bool> DisplayDeleteAlertAsync()
        {
            var result = await PageDialogService.DisplayAlertAsync("Cảnh báo", "Xác nhận xóa?", "Đồng ý", "Không");
            return result;
        }

        public virtual void Initialize(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public virtual void Destroy()
        {

        }
    }
}
