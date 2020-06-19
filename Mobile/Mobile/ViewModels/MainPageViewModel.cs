﻿using Dtos;
using Microsoft.AspNetCore.SignalR.Client;
using Mobile.Models;
using Mobile.Views;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Telerik.XamarinForms.Input.Calendar;
using Xamarin.Forms;

namespace Mobile.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private HubConnection _connection;
        public MainPageViewModel(InitParams initParams) : base(initParams)
        {
            StartSignalRAsync();
            ListCategoryBindProp = new ObservableCollection<CategoryDto>();
            ListDiscountBindProp = new ObservableCollection<DiscountDto>();
            ListInvoiceBindProp = new ObservableCollection<InvoiceDto>();
            ListZoneBindProp = new ObservableCollection<ZoneDto>();
        }

        #region Bindprops

        #region ListInvoiceFrameVisibleBindProp
        private bool _ListInvoiceFrameVisibleBindProp = true;
        public bool ListInvoiceFrameVisibleBindProp
        {
            get { return _ListInvoiceFrameVisibleBindProp; }
            set { SetProperty(ref _ListInvoiceFrameVisibleBindProp, value); }
        }
        #endregion

        #region ZoneFrameVisibleBindProp
        private bool _ZoneFrameVisibleBindProp = false;
        public bool ZoneFrameVisibleBindProp
        {
            get { return _ZoneFrameVisibleBindProp; }
            set { SetProperty(ref _ZoneFrameVisibleBindProp, value); }
        }
        #endregion

        #region InvoiceFrameVisibleBindProp
        private bool _InvoiceFrameVisibleBindProp = true;
        public bool InvoiceFrameVisibleBindProp
        {
            get { return _InvoiceFrameVisibleBindProp; }
            set { SetProperty(ref _InvoiceFrameVisibleBindProp, value); }
        }
        #endregion

        #region MenuFrameVisibleBindProp
        private bool _MenuFrameVisibleBindProp = false;
        public bool MenuFrameVisibleBindProp
        {
            get { return _MenuFrameVisibleBindProp; }
            set { SetProperty(ref _MenuFrameVisibleBindProp, value); }
        }
        #endregion

        #region ItemFrameVisibleBindProp
        private bool _ItemFrameVisibleBindProp = true;
        public bool ItemFrameVisibleBindProp
        {
            get { return _ItemFrameVisibleBindProp; }
            set { SetProperty(ref _ItemFrameVisibleBindProp, value); }
        }
        #endregion

        #region DiscountFrameVisibleBindProp
        private bool _DiscountFrameVisibleBindProp = false;
        public bool DiscountFrameVisibleBindProp
        {
            get { return _DiscountFrameVisibleBindProp; }
            set { SetProperty(ref _DiscountFrameVisibleBindProp, value); }
        }
        #endregion

        #region SettingFrameVisibleBindProp
        private bool _SettingFrameVisibleBindProp = false;
        public bool SettingFrameVisibleBindProp
        {
            get { return _SettingFrameVisibleBindProp; }
            set { SetProperty(ref _SettingFrameVisibleBindProp, value); }
        }
        #endregion

        #region ListZoneBindProp
        private ObservableCollection<ZoneDto> _ListZoneBindProp = null;
        public ObservableCollection<ZoneDto> ListZoneBindProp
        {
            get { return _ListZoneBindProp; }
            set { SetProperty(ref _ListZoneBindProp, value); }
        }
        #endregion

        #region ListInvoiceBindProp
        private ObservableCollection<InvoiceDto> _ListInvoiceBindProp = null;
        public ObservableCollection<InvoiceDto> ListInvoiceBindProp
        {
            get { return _ListInvoiceBindProp; }
            set { SetProperty(ref _ListInvoiceBindProp, value); }
        }
        #endregion

        #region ListCategoryBindProp
        private ObservableCollection<CategoryDto> _ListCategoryBindProp = null;
        public ObservableCollection<CategoryDto> ListCategoryBindProp
        {
            get { return _ListCategoryBindProp; }
            set { SetProperty(ref _ListCategoryBindProp, value); }
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

        #region CurrentCategory
        private CategoryDto _CurrentCategory;
        public CategoryDto CurrentCategory
        {
            get { return _CurrentCategory; }
            set { SetProperty(ref _CurrentCategory, value); }
        }
        #endregion

        #region CurrentZone
        private ZoneDto _CurrentZone;
        public ZoneDto CurrentZone
        {
            get { return _CurrentZone; }
            set { SetProperty(ref _CurrentZone, value); }
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

        #region InvoiceBindProp
        private InvoiceDto _InvoiceBindProp = null;
        public InvoiceDto InvoiceBindProp
        {
            get { return _InvoiceBindProp; }
            set { SetProperty(ref _InvoiceBindProp, value); }
        }
        #endregion

        #region IsTakeAway
        private bool _IsTakeAway;
        public bool IsTakeAway
        {
            get { return _IsTakeAway; }
            set { SetProperty(ref _IsTakeAway, value); }
        }
        #endregion

        #region IsOpenTakeAway
        private bool _IsOpenTakeAway;
        public bool IsOpenTakeAway
        {
            get { return _IsOpenTakeAway; }
            set { SetProperty(ref _IsOpenTakeAway, value); }
        }
        #endregion

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

        #region SelectSettingCommand

        public DelegateCommand<string> SelectSettingCommand { get; private set; }
        private async void OnSelectSetting(string settings)
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

                switch (settings)
                {
                    case "Category":
                        param.Add(nameof(ListCategoryBindProp), ListCategoryBindProp);
                        await NavigationService.NavigateAsync(nameof(CategoryPage), param);
                        break;
                    case "Discount":
                        param.Add(nameof(ListDiscountBindProp), ListDiscountBindProp);
                        await NavigationService.NavigateAsync(nameof(DiscountPage), param);
                        break;
                    case "Zone":
                        param.Add(nameof(ListZoneBindProp), ListZoneBindProp);
                        await NavigationService.NavigateAsync(nameof(ZonePage), param);
                        break;
                    case "Employee":
                        await NavigationService.NavigateAsync(nameof(EmployeePage));
                        break;
                    case "Inventory":
                        await NavigationService.NavigateAsync(nameof(InventoryPage));
                        break;
                    case "History":
                        await NavigationService.NavigateAsync(nameof(InvoicePage));
                        break;
                    case "Report":
                        await NavigationService.NavigateAsync(nameof(ReportPage));
                        break;
                    case "Session":
                        await NavigationService.NavigateAsync(nameof(EmployeePage));
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
        private void InitSelectSettingCommand()
        {
            SelectSettingCommand = new DelegateCommand<string>(OnSelectSetting);
            SelectSettingCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region SelectCategoryCommand

        public DelegateCommand<CategoryDto> SelectCategoryCommand { get; private set; }
        private async void OnSelectCategory(CategoryDto obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                CurrentCategory = obj;
                InvoiceFrameVisibleBindProp = false;
                MenuFrameVisibleBindProp = true;
                ItemFrameVisibleBindProp = true;
                DiscountFrameVisibleBindProp = false;
                SettingFrameVisibleBindProp = false;
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
        private void InitSelectCategoryCommand()
        {
            SelectCategoryCommand = new DelegateCommand<CategoryDto>(OnSelectCategory);
            SelectCategoryCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region SelectZoneCommand

        public DelegateCommand<ZoneDto> SelectZoneCommand { get; private set; }
        private async void OnSelectZone(ZoneDto obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                CurrentZone = obj;
                ZoneFrameVisibleBindProp = true;
                ListInvoiceFrameVisibleBindProp = false;
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
        private void InitSelectZoneCommand()
        {
            SelectZoneCommand = new DelegateCommand<ZoneDto>(OnSelectZone);
            SelectZoneCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region SelectItemCommand

        public DelegateCommand<ItemDto> SelectItemCommand { get; private set; }
        private async void OnSelectItem(ItemDto item)
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
                param.Add(nameof(ListDiscountBindProp), ListDiscountBindProp);
                param.Add("ItemBindProp", item);

                await NavigationService.NavigateAsync(nameof(ItemDiscountPage), param);
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
        private void InitSelectItemCommand()
        {
            SelectItemCommand = new DelegateCommand<ItemDto>(OnSelectItem);
            SelectItemCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region SelectFrameCommand

        public DelegateCommand<string> SelectFrameCommand { get; private set; }
        private async void OnSelectFrame(string obj)
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
                    case "setting":
                        MenuFrameVisibleBindProp = false;
                        InvoiceFrameVisibleBindProp = false;
                        SettingFrameVisibleBindProp = true;
                        break;
                    case "invoice":
                        if (InvoiceBindProp != null)
                        {
                            var ok = await PageDialogService.DisplayAlertAsync("Cảnh báo", "Hủy hóa đơn?", "Đồng ý", "Không");
                            if (ok)
                            {
                                MenuFrameVisibleBindProp = false;
                                InvoiceFrameVisibleBindProp = true;
                                SettingFrameVisibleBindProp = false;
                                InvoiceBindProp = null;
                            }
                        }
                        else
                        {
                            MenuFrameVisibleBindProp = false;
                            InvoiceFrameVisibleBindProp = true;
                            SettingFrameVisibleBindProp = false;
                        }
                        break;
                    case "discount":
                        MenuFrameVisibleBindProp = true;
                        InvoiceFrameVisibleBindProp = false;
                        ItemFrameVisibleBindProp = false;
                        DiscountFrameVisibleBindProp = true;
                        SettingFrameVisibleBindProp = false;
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
        private void InitSelectFrameCommand()
        {
            SelectFrameCommand = new DelegateCommand<string>(OnSelectFrame);
            SelectFrameCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region SaveInvoiceCommand

        public DelegateCommand<object> SaveInvoiceCommand { get; private set; }
        private async void OnSaveInvoice(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                var invoiceToCreate = new InvoiceForCreateDto(InvoiceBindProp);
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
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.Created:
                            var invoice = JsonConvert.DeserializeObject<InvoiceDto>(await response.Content.ReadAsStringAsync());
                            ListInvoiceBindProp.Add(invoice);
                            await _connection.InvokeAsync("SendInvoice", invoice);
                            InvoiceBindProp = null;
                            break;
                    }
                };
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
        private void InitSaveInvoiceCommand()
        {
            SaveInvoiceCommand = new DelegateCommand<object>(OnSaveInvoice);
            SaveInvoiceCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region SelectDiscountCommand

        public DelegateCommand<DiscountDto> SelectDiscountCommand { get; private set; }
        private async void OnSelectDiscount(DiscountDto obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                if (InvoiceBindProp == null)
                {
                    InvoiceBindProp = new InvoiceDto();
                }
                InvoiceBindProp.Discounts.Add(obj);
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

        #region SelectTableCommand

        public DelegateCommand<object> SelectTableCommand { get; private set; }
        private async void OnSelectTable(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                if (obj is TableDto)
                {
                    if (InvoiceBindProp == null)
                    {
                        InvoiceBindProp = new InvoiceDto();
                    }
                    var table = obj as TableDto;
                    InvoiceBindProp.TableName = table.Name;
                    InvoiceBindProp.TableId = table.Id;
                    MenuFrameVisibleBindProp = true;
                    InvoiceFrameVisibleBindProp = false;
                }
                else
                {
                    MenuFrameVisibleBindProp = false;
                    InvoiceFrameVisibleBindProp = true;
                    ListInvoiceFrameVisibleBindProp = false;
                    ZoneFrameVisibleBindProp = true;
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
        private void InitSelectTableCommand()
        {
            SelectTableCommand = new DelegateCommand<object>(OnSelectTable);
            SelectTableCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region OpenTakeAwayCommand

        public DelegateCommand<object> OpenTakeAwayCommand { get; private set; }
        private async void OnOpenTakeAway(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                IsOpenTakeAway = true;
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
        private void InitOpenTakeAwayCommand()
        {
            OpenTakeAwayCommand = new DelegateCommand<object>(OnOpenTakeAway);
            OpenTakeAwayCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region SelectTakeAwayCommand

        public DelegateCommand<string> SelectTakeAwayCommand { get; private set; }
        private async void OnSelectTakeAway(string mode)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                switch (mode)
                {
                    case "togo":
                        IsTakeAway = true;
                        break;
                    case "inplace":
                        IsTakeAway = false;
                        break;
                }
                IsOpenTakeAway = false;
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
        private void InitSelectTakeAwayCommand()
        {
            SelectTakeAwayCommand = new DelegateCommand<string>(OnSelectTakeAway);
            SelectTakeAwayCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        private async void StartSignalRAsync()
        {
            _connection = new HubConnectionBuilder()
                     .WithUrl(Properties.Resources.SignalR)
                     .Build();

            _connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await _connection.StartAsync();
            };

            _connection.On<InvoiceDto>("ReceiveInvoice", (invoice) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    ListInvoiceBindProp.Add(invoice);
                });
            });

            try
            {
                await _connection.StartAsync();
            }
            catch (Exception ex)
            {
                await ShowErrorAsync(ex);
            }
        }
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            switch (parameters.GetNavigationMode())
            {
                case NavigationMode.Back:
                    if (parameters.ContainsKey("item"))
                    {
                        var item = parameters["item"] as ItemForInvoiceDto;
                        if (InvoiceBindProp == null)
                        {
                            InvoiceBindProp = new InvoiceDto();
                        }
                        InvoiceBindProp.Items.Add(item);
                        InvoiceBindProp.TotalPrice += item.Price;
                    }
                    break;
                case NavigationMode.New:
                    using (var client = new HttpClient())
                    {
                        var categoryTask = client.GetAsync(Properties.Resources.BaseUrl + "categories/");
                        var discountTask = client.GetAsync(Properties.Resources.BaseUrl + "discounts/");
                        var invoiceTask = client.GetAsync(Properties.Resources.BaseUrl + "invoices/");
                        var zoneTask = client.GetAsync(Properties.Resources.BaseUrl + "zones/");

                        var allTasks = new List<Task> { categoryTask, discountTask, invoiceTask, zoneTask };
                        while (allTasks.Any())
                        {
                            Task finished = await Task.WhenAny(allTasks);
                            if (finished == categoryTask)
                            {
                                if (categoryTask.Result.IsSuccessStatusCode)
                                {
                                    var categories = JsonConvert.DeserializeObject<IEnumerable<CategoryDto>>(await categoryTask.Result.Content.ReadAsStringAsync());
                                    foreach (var category in categories)
                                    {
                                        ListCategoryBindProp.Add(category);
                                    }
                                }
                                else
                                {
                                    await PageDialogService.DisplayAlertAsync("Lỗi", $"{await categoryTask.Result.Content.ReadAsStringAsync()}", "Đóng");
                                }
                            }
                            else if (finished == discountTask)
                            {
                                if (discountTask.Result.IsSuccessStatusCode)
                                {
                                    var discounts = JsonConvert.DeserializeObject<IEnumerable<DiscountDto>>(await discountTask.Result.Content.ReadAsStringAsync());
                                    foreach (var discount in discounts)
                                    {
                                        ListDiscountBindProp.Add(discount);
                                    }
                                }
                                else
                                {
                                    await PageDialogService.DisplayAlertAsync("Lỗi", $"{await discountTask.Result.Content.ReadAsStringAsync()}", "Đóng");
                                }
                            }
                            else if (finished == invoiceTask)
                            {
                                if (invoiceTask.Result.IsSuccessStatusCode)
                                {
                                    var invoices = JsonConvert.DeserializeObject<IEnumerable<InvoiceDto>>(await invoiceTask.Result.Content.ReadAsStringAsync());
                                    foreach (var invoice in invoices)
                                    {
                                        ListInvoiceBindProp.Add(invoice);
                                    }
                                }
                                else
                                {
                                    await PageDialogService.DisplayAlertAsync("Lỗi", $"{await invoiceTask.Result.Content.ReadAsStringAsync()}", "Đóng");
                                }
                            }
                            else if (finished == zoneTask)
                            {
                                if (zoneTask.Result.IsSuccessStatusCode)
                                {
                                    var zones = JsonConvert.DeserializeObject<IEnumerable<ZoneDto>>(await zoneTask.Result.Content.ReadAsStringAsync());
                                    foreach (var zone in zones)
                                    {
                                        ListZoneBindProp.Add(zone);
                                    }
                                }
                                else
                                {
                                    await PageDialogService.DisplayAlertAsync("Lỗi", $"{await zoneTask.Result.Content.ReadAsStringAsync()}", "Đóng");
                                }
                            }
                            allTasks.Remove(finished);
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
