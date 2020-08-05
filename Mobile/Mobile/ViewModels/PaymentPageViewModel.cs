using Dtos;
using Mobile.Models;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace Mobile.ViewModels
{
    public class PaymentPageViewModel : ViewModelBase
    {
        public PaymentPageViewModel(InitParams initParams) : base(initParams)
        {
        }

        #region IsCompletedBindProp
        private bool _IsCompletedBindProp = false;
        public bool IsCompletedBindProp
        {
            get { return _IsCompletedBindProp; }
            set { SetProperty(ref _IsCompletedBindProp, value); }
        }
        #endregion

        #region ReceivedMoneyBindProp
        private double _ReceivedMoneyBindProp = 0;
        public double ReceivedMoneyBindProp
        {
            get { return _ReceivedMoneyBindProp; }
            set { SetProperty(ref _ReceivedMoneyBindProp, value); }
        }
        #endregion

        #region ChangeMoneyBindProp
        private double _ChangeMoneyBindProp = 0;
        public double ChangeMoneyBindProp
        {
            get { return _ChangeMoneyBindProp; }
            set { SetProperty(ref _ChangeMoneyBindProp, value); }
        }
        #endregion

        #region TipBindProp
        private double _TipBindProp = 0;
        public double TipBindProp
        {
            get { return _TipBindProp; }
            set { SetProperty(ref _TipBindProp, value); }
        }
        #endregion

        #region InvoiceBindProp
        private InvoiceDto _InvoiceBindProp;
        public InvoiceDto InvoiceBindProp
        {
            get { return _InvoiceBindProp; }
            set { SetProperty(ref _InvoiceBindProp, value); }
        }
        #endregion

        #region TapNumberCommand
        public DelegateCommand<object> TapNumberCommand { get; private set; }
        private async void OnTapNumber(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day 
                if (obj is decimal d)
                {
                    if (ReceivedMoneyBindProp == 0)
                    {
                        ReceivedMoneyBindProp = (double)d;
                    }
                    else
                    {
                        ReceivedMoneyBindProp = ReceivedMoneyBindProp * 10 + (double)d;
                    }
                }
                if (obj is string s)
                {
                    switch (s)
                    {
                        case "X":
                            if (ReceivedMoneyBindProp >= 10)
                            {
                                ReceivedMoneyBindProp = Math.Floor(ReceivedMoneyBindProp /= 10);
                            }
                            else
                            {
                                ReceivedMoneyBindProp = 0;
                            }
                            break;
                        case "C":
                            ReceivedMoneyBindProp = 0;
                            break;
                        case "00":
                            ReceivedMoneyBindProp *= 100;
                            break;
                        case "000":
                            ReceivedMoneyBindProp *= 1000;
                            break;
                    }
                }
                if (ReceivedMoneyBindProp > InvoiceBindProp.TotalPrice)
                {
                    ChangeMoneyBindProp = ReceivedMoneyBindProp - InvoiceBindProp.TotalPrice;
                }
                else
                {
                    ChangeMoneyBindProp = 0;
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
        private void InitTapNumberCommand()
        {
            TapNumberCommand = new DelegateCommand<object>(OnTapNumber);
            TapNumberCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region CompeletePaymentCommand
        public DelegateCommand<object> CompeletePaymentCommand { get; private set; }

        private async void OnCompeletePayment(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            if (ReceivedMoneyBindProp < InvoiceBindProp.TotalPrice)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                if (!IsCompletedBindProp)
                {
                    IsCompletedBindProp = true;
                }
                else
                {
                    var invoiceToCreate = new InvoiceForCreateDto(InvoiceBindProp);
                    invoiceToCreate.Tip = TipBindProp;
                    invoiceToCreate.IsPaid = true;
                    invoiceToCreate.ClosedAt = DateTime.Now;
                    invoiceToCreate.PaidAmount = ReceivedMoneyBindProp;
                    var json = JsonConvert.SerializeObject(invoiceToCreate);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    // Thuc hien cong viec tai day
                    using (var client = new HttpClient())
                    {
                        HttpResponseMessage response = new HttpResponseMessage();
                        if (InvoiceBindProp.Id == Guid.Empty)
                        {
                            response = await client.PostAsync(Properties.Resources.BaseUrl + "invoices/", content);
                        }
                        else
                        {
                            response = await client.PutAsync(Properties.Resources.BaseUrl + "invoices/", content);
                        }
                        if (response.IsSuccessStatusCode)
                        {
                            var session = Application.Current.Properties["session"] as SessionDto;
                            session.Revenue += InvoiceBindProp.TotalPrice;
                            session.Tip += TipBindProp;
                            session.ExpectedMoney += InvoiceBindProp.TotalPrice + TipBindProp;

                            var param = new NavigationParameters();
                            param.Add(nameof(InvoiceBindProp), InvoiceBindProp);
                            await NavigationService.GoBackAsync(param);
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
        private void InitCompeletePaymentCommand()
        {
            CompeletePaymentCommand = new DelegateCommand<object>(OnCompeletePayment);
            CompeletePaymentCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region TipCommand

        public DelegateCommand<object> TipCommand { get; private set; }
        private async void OnTip(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                TipBindProp = ChangeMoneyBindProp;
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
        private void InitTipCommand()
        {
            TipCommand = new DelegateCommand<object>(OnTip);
            TipCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion
    }
}
