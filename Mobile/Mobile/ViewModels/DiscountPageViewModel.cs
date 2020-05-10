using Dtos;
using Mobile.Models;
using Mobile.ViewModels;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Mobile.ViewModels
{
    public class DiscountPageViewModel : ViewModelBase
    {
        public DiscountPageViewModel(InitParams initParams) : base(initParams)
        {
            ListDiscountBindProp = new ObservableCollection<DiscountDto>();
        }

        #region IsOpen
        private bool _IsOpen = false;
        public bool IsOpen
        {
            get { return _IsOpen; }
            set { SetProperty(ref _IsOpen, value); }
        }
        #endregion

        #region DiscountBindProp
        private DiscountDto _DiscountBindProp = null;
        public DiscountDto DiscountBindProp
        {
            get { return _DiscountBindProp; }
            set { SetProperty(ref _DiscountBindProp, value); }
        }
        #endregion

        #region IsEditing
        private bool _IsEditing = false;
        public bool IsEditing
        {
            get { return _IsEditing; }
            set { SetProperty(ref _IsEditing, value); }
        }
        #endregion

        #region ListDiscountBindProp
        private ObservableCollection<DiscountDto> _ListDiscountBindProp = null;
        public ObservableCollection<DiscountDto> ListDiscountBindProp
        {
            get { return _ListDiscountBindProp; }
            set { SetProperty(ref _ListDiscountBindProp, value); }
        }
        #endregion

        #region IsPercentage
        private bool _IsPercentage = true;
        public bool IsPercentage
        {
            get { return _IsPercentage; }
            set 
            {
                if (SetProperty(ref _IsPercentage, value))
                {
                    RaisePropertyChanged(nameof(IsValue));
                }
            }
        }
        public bool IsValue { get { return !_IsPercentage; } }
        #endregion

        #region TempDiscount
        private DiscountDto _TempDiscount = null;
        public DiscountDto TempDiscount
        {
            get { return _TempDiscount; }
            set { SetProperty(ref _TempDiscount, value); }
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
                    DiscountBindProp = new DiscountDto();
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
            if (string.IsNullOrWhiteSpace(DiscountBindProp.Name))
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
                var discountToCreate = new DiscountForCreateDto(DiscountBindProp);
                var json = JsonConvert.SerializeObject(discountToCreate);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                // Thuc hien cong viec tai day
                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    if (DiscountBindProp.Id == Guid.Empty)
                    {
                        response = await client.PostAsync(Properties.Resources.BaseUrl + "discounts/", content);
                    }
                    else
                    {
                        response = await client.PutAsync(Properties.Resources.BaseUrl + "discounts/", content);
                    }
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.NoContent:
                            TempDiscount.Name = DiscountBindProp.Name;
                            TempDiscount.Value = DiscountBindProp.Value;
                            TempDiscount.MaxValue = DiscountBindProp.MaxValue;
                            TempDiscount.IsPercentage = DiscountBindProp.IsPercentage;
                            break;
                        case HttpStatusCode.Created:
                            var discount = JsonConvert.DeserializeObject<DiscountDto>(await response.Content.ReadAsStringAsync());
                            ListDiscountBindProp.Add(discount);
                            DiscountBindProp = new DiscountDto();
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

        #region SelectDiscountCommand

        public DelegateCommand<DiscountDto> SelectDiscountCommand { get; private set; }
        private async void OnSelectDiscount(DiscountDto discount)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                TempDiscount = discount;
                DiscountBindProp = new DiscountDto(discount);
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
        private void InitSelectDiscountCommand()
        {
            SelectDiscountCommand = new DelegateCommand<DiscountDto>(OnSelectDiscount);
            SelectDiscountCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region DeleteDiscountCommand

        public DelegateCommand<object> DeleteDiscountCommand { get; private set; }
        private async void OnDeleteDiscount(object obj)
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
                    response = await client.DeleteAsync(Properties.Resources.BaseUrl + "discounts/" + DiscountBindProp.Id);

                    if (response.IsSuccessStatusCode)
                    {
                        ListDiscountBindProp.Remove(TempDiscount);
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
        private void InitDeleteDiscountCommand()
        {
            DeleteDiscountCommand = new DelegateCommand<object>(OnDeleteDiscount);
            DeleteDiscountCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region SelectDiscountTypeCommand

        public DelegateCommand<string> SelectDiscountTypeCommand { get; private set; }
        private async void OnSelectDiscountType(string obj)
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
                    case "percent":
                        IsPercentage = true;
                        break;
                    case "value":
                        IsPercentage = false;
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
        private void InitSelectDiscountTypeCommand()
        {
            SelectDiscountTypeCommand = new DelegateCommand<string>(OnSelectDiscountType);
            SelectDiscountTypeCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

    }
}
