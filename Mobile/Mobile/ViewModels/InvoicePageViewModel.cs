using Dtos;
using Mobile.Models;
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
    public class InvoicePageViewModel : ViewModelBase
    {
        public InvoicePageViewModel(InitParams initParams) : base(initParams)
        {
            ListInvoiceBindProp = new ObservableCollection<InvoiceDto>();
        }

        #region ListInvoiceBindProp
        private ObservableCollection<InvoiceDto> _ListInvoiceBindProp = null;
        public ObservableCollection<InvoiceDto> ListInvoiceBindProp
        {
            get { return _ListInvoiceBindProp; }
            set { SetProperty(ref _ListInvoiceBindProp, value); }
        }
        #endregion

        #region CurrentInvoiceBindProp
        private InvoiceDto _CurrentInvoiceBindProp;
        public InvoiceDto CurrentInvoiceBindProp
        {
            get { return _CurrentInvoiceBindProp; }
            set { SetProperty(ref _CurrentInvoiceBindProp, value); }
        }
        #endregion

        #region SelectInvoiceCommand

        public DelegateCommand<InvoiceDto> SelectInvoiceCommand { get; private set; }
        private async void OnSelectInvoice(InvoiceDto obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                CurrentInvoiceBindProp = obj;
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
        private void InitSelectInvoiceCommand()
        {
            SelectInvoiceCommand = new DelegateCommand<InvoiceDto>(OnSelectInvoice);
            SelectInvoiceCommand.ObservesCanExecute(() => IsNotBusy);
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
                        var response = await client.GetAsync(Properties.Resources.BaseUrl + "invoices/GetPaidInvoices");
                        if (response.IsSuccessStatusCode)
                        {
                            var invoices = JsonConvert.DeserializeObject<IEnumerable<InvoiceDto>>(await response.Content.ReadAsStringAsync());
                            foreach (var invoice in invoices)
                            {
                                ListInvoiceBindProp.Add(invoice);
                            }
                        }
                    }
                    CurrentInvoiceBindProp = ListInvoiceBindProp.FirstOrDefault();
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
