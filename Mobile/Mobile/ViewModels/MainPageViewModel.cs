using Mobile.Models;
using Mobile.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Mobile.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel(InitParams initParams) : base(initParams)
        {
            ListZoneBindProp = new ObservableCollection<string>();
            ListInvoiceBindProp = new ObservableCollection<string>();
            for (int i = 0; i < 5; i++)
            {
                ListZoneBindProp.Add($"Section {i}");
                ListInvoiceBindProp.Add(i.ToString());
            }
        }

        #region ListZoneBindProp
        private ObservableCollection<string> _ListZoneBindProp = null;
        public ObservableCollection<string> ListZoneBindProp
        {
            get { return _ListZoneBindProp; }
            set { SetProperty(ref _ListZoneBindProp, value); }
        }
        #endregion

        #region ListInvoiceBindProp
        private ObservableCollection<string> _ListInvoiceBindProp = null;
        public ObservableCollection<string> ListInvoiceBindProp
        {
            get { return _ListInvoiceBindProp; }
            set { SetProperty(ref _ListInvoiceBindProp, value); }
        }
        #endregion

        #region IsEditing
        private bool _IsEditing = false;
        public bool IsEditing
        {
            get { return _IsEditing; }
            set
            {
                SetProperty(ref _IsEditing, value);
                RaisePropertyChanged(nameof(IsNotEditting));
            }
        }


        public bool IsNotEditting { get { return !_IsEditing; } }
        #endregion

        #region ChangeEditModeCommand

        public DelegateCommand<object> ChangeEditModeCommand { get; private set; }
        private async void OnChangeEditMode(object obj)
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
                    IsEditing = false;
                }
                else
                {
                    IsEditing = true;
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
        private void InitChangeEditModeCommand()
        {
            ChangeEditModeCommand = new DelegateCommand<object>(OnChangeEditMode);
            ChangeEditModeCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region SelectPageCommand

        public DelegateCommand<object> SelectPageCommand { get; private set; }
        private async void OnSelectPage(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                switch (obj as string)
                {
                    case "Category":
                        await NavigationService.NavigateAsync(nameof(CategoryPage));
                        break;
                    default:
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
        private void InitSelectPageCommand()
        {
            SelectPageCommand = new DelegateCommand<object>(OnSelectPage);
            SelectPageCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

    }
}
