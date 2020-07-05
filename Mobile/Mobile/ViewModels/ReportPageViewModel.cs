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
using Telerik.XamarinForms.Input.Calendar;

namespace Mobile.ViewModels
{
    public class ReportPageViewModel : ViewModelBase
    {
        private List<InvoiceDto> _listInvoice;
        public ReportPageViewModel(InitParams initParams) : base(initParams)
        {
            SideBarBindProp = new ObservableCollection<SelectionModel>
            {
                new SelectionModel{ Name = "Doanh thu tổng quan", IsSelected = true },
                new SelectionModel{ Name = "Mặt hàng bán chạy", IsSelected = false },
            };
            Title = "Doanh thu tổng quan";
        }

        #region SideBarBindProp
        private ObservableCollection<SelectionModel> _SideBarBindProp;
        public ObservableCollection<SelectionModel> SideBarBindProp
        {
            get { return _SideBarBindProp; }
            set { SetProperty(ref _SideBarBindProp, value); }
        }
        #endregion

        #region OverallVisibleBindProp
        private bool _OverallVisibleBindProp = true;
        public bool OverallVisibleBindProp
        {
            get { return _OverallVisibleBindProp; }
            set { SetProperty(ref _OverallVisibleBindProp, value); }
        }
        #endregion

        #region TopSellersVisibleBindProp
        private bool _TopSellersVisibleBindProp;
        public bool TopSellersVisibleBindProp
        {
            get { return _TopSellersVisibleBindProp; }
            set { SetProperty(ref _TopSellersVisibleBindProp, value); }
        }
        #endregion

        #region TotalMoneyBindProp
        private double _TotalMoneyBindProp = 0;
        public double TotalMoneyBindProp
        {
            get { return _TotalMoneyBindProp; }
            set { SetProperty(ref _TotalMoneyBindProp, value); }
        }
        #endregion

        #region TotalTransactionBindProp
        private double _TotalTransactionBindProp = 0;
        public double TotalTransactionBindProp
        {
            get { return _TotalTransactionBindProp; }
            set { SetProperty(ref _TotalTransactionBindProp, value); }
        }
        #endregion

        #region AverageMoneyBindProp
        private double _AverageMoneyBindProp = 0;
        public double AverageMoneyBindProp
        {
            get { return _AverageMoneyBindProp; }
            set { SetProperty(ref _AverageMoneyBindProp, value); }
        }
        #endregion

        #region RevenuePerDayVisibleBindProp
        private bool _RevenuePerDayVisibleBindProp = false;
        public bool RevenuePerDayVisibleBindProp
        {
            get { return _RevenuePerDayVisibleBindProp; }
            set { SetProperty(ref _RevenuePerDayVisibleBindProp, value); }
        }
        #endregion

        #region RevenuePerHourVisibleBindProp
        private bool _RevenuePerHourVisibleBindProp = false;
        public bool RevenuePerHourVisibleBindProp
        {
            get { return _RevenuePerHourVisibleBindProp; }
            set { SetProperty(ref _RevenuePerHourVisibleBindProp, value); }
        }
        #endregion

        #region SelectedDateBindProp
        private DateTime _SelectedDateBindProp;
        public DateTime SelectedDateBindProp
        {
            get { return _SelectedDateBindProp; }
            set { SetProperty(ref _SelectedDateBindProp, value); }
        }
        #endregion

        #region Data
        private ObservableCollection<CategoricalData> _Data = null;
        public ObservableCollection<CategoricalData> Data
        {
            get { return _Data; }
            set { SetProperty(ref _Data, value); }
        }
        #endregion

        #region AverageData
        private ObservableCollection<DateTimeContinuousData> _AverageData = null;
        public ObservableCollection<DateTimeContinuousData> AverageData
        {
            get { return _AverageData; }
            set { SetProperty(ref _AverageData, value); }
        }
        #endregion

        #region Interval
        private double _Interval = 1;
        public double Interval
        {
            get { return _Interval; }
            set { SetProperty(ref _Interval, value); }
        }
        #endregion

        #region DateRangeBindProp
        private DateTimeRange _DateRangeBindProp = null;
        public DateTimeRange DateRangeBindProp
        {
            get { return _DateRangeBindProp; }
            set { SetProperty(ref _DateRangeBindProp, value); }
        }
        #endregion

        #region TopSellersBindProp
        private ObservableCollection<RevenueModel> _TopSellersBindProp = null;
        public ObservableCollection<RevenueModel> TopSellersBindProp
        {
            get { return _TopSellersBindProp; }
            set { SetProperty(ref _TopSellersBindProp, value); }
        }
        #endregion

        #region SelectSidebarCommand

        public DelegateCommand<SelectionModel> SelectSidebarCommand { get; private set; }
        private async void OnSelectSidebar(SelectionModel obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                var sidebar = SideBarBindProp.FirstOrDefault(i => i.IsSelected);
                if (sidebar != null)
                {
                    sidebar.IsSelected = false;
                }
                obj.IsSelected = true;
                switch (obj.Name)
                {
                    case "Doanh thu tổng quan":
                        OverallVisibleBindProp = true;
                        TopSellersVisibleBindProp = false;
                        Title = "Doanh thu tổng quan";
                        break;
                    case "Mặt hàng bán chạy":
                        TopSellersVisibleBindProp = true;
                        OverallVisibleBindProp = false;
                        Title = "Mặt hàng bán chạy";
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
        private void InitSelectSidebarCommand()
        {
            SelectSidebarCommand = new DelegateCommand<SelectionModel>(OnSelectSidebar);
            SelectSidebarCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        
        private async void GetOverallData()
        {
            try
            {

                TotalMoneyBindProp = 0;
                TotalTransactionBindProp = 0;
                //Doanh thu theo gio
                var data = new ObservableCollection<CategoricalData>();
                var value = new double[24];
                //Doanh thu theo ngay
                var averageData = new ObservableCollection<DateTimeContinuousData>();
                double days = 1;

                //Khoi tao list
                for (int i = 0; i < 24; i++)
                {
                    data.Add(new CategoricalData { Category = i, Value = 0 });
                    value[i] = 0;
                }
                days = (DateRangeBindProp.To - DateRangeBindProp.From).TotalDays + 1;
                for (int i = 0; i < days; i++)
                {
                    averageData.Add(new DateTimeContinuousData { DateTime = DateRangeBindProp.From.AddDays(i).ToString("dd/MM"), Value = 0 });
                }
                //Doanh thu tong quan
                foreach (var invoice in _listInvoice)
                {
                    if (Convert.ToDateTime(invoice.ClosedAt).Date == DateRangeBindProp.From && DateRangeBindProp.From == DateRangeBindProp.To)
                    {
                        TotalMoneyBindProp += invoice.TotalPrice;
                        TotalTransactionBindProp++;
                        //Tinh tong value gio
                        var gio = Convert.ToDateTime(invoice.ClosedAt).Hour;
                        value[gio] += invoice.TotalPrice;
                        //Tinh trung binh gio
                        data[gio].Value = value[gio] / days;
                    }
                    //Chon date range
                    else if (Convert.ToDateTime(invoice.ClosedAt).Date >= DateRangeBindProp.From && Convert.ToDateTime(invoice.ClosedAt).Date <= DateRangeBindProp.To)
                    {
                        TotalMoneyBindProp += invoice.TotalPrice;
                        TotalTransactionBindProp++;
                        var gio = Convert.ToDateTime(invoice.ClosedAt).Hour;
                        var ngay = (Convert.ToDateTime(invoice.ClosedAt).Date - DateRangeBindProp.From).Days;
                        value[gio] += invoice.TotalPrice;
                        data[gio].Value = value[gio] / days;
                        //Tinh value theo ngay
                        averageData[ngay].Value += invoice.TotalPrice;
                    }
                }

                //Nếu có giá trị mới hiện chart
                if (TotalMoneyBindProp > 0)
                {
                    AverageMoneyBindProp = TotalMoneyBindProp / TotalTransactionBindProp;
                    RevenuePerHourVisibleBindProp = true;
                }
                else
                {
                    AverageMoneyBindProp = 0;
                    RevenuePerHourVisibleBindProp = false;
                    RevenuePerDayVisibleBindProp = false;
                }
                // round to 8 ticks
                Interval = Math.Max(1, days / 8);
                Data = new ObservableCollection<CategoricalData>(data);
                AverageData = new ObservableCollection<DateTimeContinuousData>(averageData);
            }
            catch (Exception ex)
            {
                await ShowErrorAsync(ex);
            }
        }

        private void GetTopSeller()
        {
            TopSellersBindProp = new ObservableCollection<RevenueModel>();

            var listTopSeller = _listInvoice.Where(h => Convert.ToDateTime(h.ClosedAt).Date >= DateRangeBindProp.From
                && Convert.ToDateTime(h.ClosedAt).Date <= DateRangeBindProp.To)
                .SelectMany(h => h.Items)
                .GroupBy(h => h.Id)
                .Select(h => new
                {
                    Name = h.FirstOrDefault().Name,
                    Quantity = h.Sum(c => c.Quantity),
                    Revenue = h.Sum(c => c.Value),
                })
                .OrderByDescending(h => h.Quantity).Take(10).ToList();

            var totalTransaction = (int)listTopSeller.Sum(c => c.Quantity);
            var totalRevenue = listTopSeller.Sum(c => c.Revenue);
            // Tinh doanh thu theo danh muc
            foreach (var item in listTopSeller)
            {
                TopSellersBindProp.Add(new RevenueModel
                {
                    Name = item.Name,
                    Type = $"{Math.Round(item.Quantity / (double)totalTransaction * 100, 2)}%",
                    TransactionCount = (int)item.Quantity,
                    Revenue = item.Revenue
                });
            }
            //Tinh tong
            TopSellersBindProp.Add(new RevenueModel
            {
                Name = "Tổng",
                TransactionCount = totalTransaction,
                Revenue = totalRevenue
            });
        }

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
                        var invoiceTask = client.GetAsync(Properties.Resources.BaseUrl + "invoices/GetPaidInvoices");
                        DateRangeBindProp = new DateTimeRange(DateTime.Today.Date, DateTime.Today.Date);
                        _listInvoice = new List<InvoiceDto>();
                        var response = await invoiceTask;
                        if (response.IsSuccessStatusCode)
                        {
                            var invoices = JsonConvert.DeserializeObject<IEnumerable<InvoiceDto>>(await response.Content.ReadAsStringAsync());
                            foreach (var invoice in invoices)
                            {
                                _listInvoice.Add(invoice);
                            }
                        }
                    }
                    GetOverallData();
                    GetTopSeller();
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
