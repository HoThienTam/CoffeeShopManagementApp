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
using System.Text;
using Telerik.XamarinForms.Input.Calendar;

namespace Mobile.ViewModels
{
    public class InventoryPageViewModel : ViewModelBase
    {
        public InventoryPageViewModel(InitParams initParams) : base(initParams)
        {
            ListItemBindProp = new ObservableCollection<ItemDto>();
            ListItemOutOfStockBindProp = new ObservableCollection<ItemDto>();
            ListHistoryBindProp = new ObservableCollection<ImportExportHistoryDto>();
            SideBarBindProp = new ObservableCollection<SelectionModel>
            {
                new SelectionModel{ Name = "Danh sách hàng tồn", IsSelected = true },
                new SelectionModel{ Name = "Danh sách hàng sắp hết", IsSelected = false },
                new SelectionModel{ Name = "Lịch sử", IsSelected = false }
            };
            Title = "Danh sách hàng tồn";
        }

        #region ListItemBindProp
        private ObservableCollection<ItemDto> _ListItemBindProp;
        public ObservableCollection<ItemDto> ListItemBindProp
        {
            get { return _ListItemBindProp; }
            set { SetProperty(ref _ListItemBindProp, value); }
        }
        #endregion

        #region ListItemOutOfStockBindProp
        private ObservableCollection<ItemDto> _ListItemOutOfStockBindProp;
        public ObservableCollection<ItemDto> ListItemOutOfStockBindProp
        {
            get { return _ListItemOutOfStockBindProp; }
            set { SetProperty(ref _ListItemOutOfStockBindProp, value); }
        }
        #endregion

        #region ListHistoryBindProp
        private ObservableCollection<ImportExportHistoryDto> _ListHistoryBindProp;
        public ObservableCollection<ImportExportHistoryDto> ListHistoryBindProp
        {
            get { return _ListHistoryBindProp; }
            set { SetProperty(ref _ListHistoryBindProp, value); }
        }
        #endregion

        #region ListItemVisibleBindProp
        private bool _ListItemVisibleBindProp = true;
        public bool ListItemVisibleBindProp
        {
            get { return _ListItemVisibleBindProp; }
            set { SetProperty(ref _ListItemVisibleBindProp, value); }
        }
        #endregion

        #region ListItemOutOfStockVisibleBindProp
        private bool _ListItemOutOfStockVisibleBindProp;
        public bool ListItemOutOfStockVisibleBindProp
        {   
            get { return _ListItemOutOfStockVisibleBindProp; }
            set { SetProperty(ref _ListItemOutOfStockVisibleBindProp, value); }
        }
        #endregion

        #region HistoryVisibleBindProp
        private bool _HistoryVisibleBindProp;
        public bool HistoryVisibleBindProp
        {
            get { return _HistoryVisibleBindProp; }
            set { SetProperty(ref _HistoryVisibleBindProp, value); }
        }
        #endregion

        #region SideBarBindProp
        private ObservableCollection<SelectionModel> _SideBarBindProp;
        public ObservableCollection<SelectionModel> SideBarBindProp
        {
            get { return _SideBarBindProp; }
            set { SetProperty(ref _SideBarBindProp, value); }
        }
        #endregion

        #region AddQuantityCommand

        public DelegateCommand<ItemDto> AddQuantityCommand { get; private set; }
        private async void OnAddQuantity(ItemDto obj)
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
                param.Add("ItemBindProp", obj);
                await NavigationService.NavigateAsync(nameof(AddQuantityPage), param);
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
        private void InitAddQuantityCommand()
        {
            AddQuantityCommand = new DelegateCommand<ItemDto>(OnAddQuantity);
            AddQuantityCommand.ObservesCanExecute(() => IsNotBusy);
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
                    case "Danh sách hàng tồn":
                        ListItemVisibleBindProp = true;
                        ListItemOutOfStockVisibleBindProp = false;
                        HistoryVisibleBindProp = false;
                        break;
                    case "Danh sách hàng sắp hết":
                        ListItemOutOfStockVisibleBindProp = true;
                        ListItemVisibleBindProp = false;
                        HistoryVisibleBindProp = false;
                        break;
                    case "Lịch sử":
                        HistoryVisibleBindProp = true;
                        ListItemVisibleBindProp = false;
                        ListItemOutOfStockVisibleBindProp = false;
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
        private void InitSelectSidebarCommand()
        {
            SelectSidebarCommand = new DelegateCommand<SelectionModel>(OnSelectSidebar);
            SelectSidebarCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region ReduceQuantityCommand

        public DelegateCommand<ItemDto> ReduceQuantityCommand { get; private set; }
        private async void OnReduceQuantity(ItemDto obj)
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
                param.Add("ItemBindProp", obj);
                await NavigationService.NavigateAsync(nameof(ReduceQuantityPage), param);
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
        private void InitReduceQuantityCommand()
        {
            ReduceQuantityCommand = new DelegateCommand<ItemDto>(OnReduceQuantity);
            ReduceQuantityCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            switch (parameters.GetNavigationMode())
            {
                case NavigationMode.Back:
                    if (parameters.ContainsKey("History"))
                    {
                        var history = parameters["History"] as ImportExportHistoryDto;
                        ListHistoryBindProp.Insert(0, history);
                    }
                    break;
                case NavigationMode.New:
                    using (var client = new HttpClient())
                    {
                        var response = await client.GetAsync(Properties.Resources.BaseUrl + "items/");
                        if (response.IsSuccessStatusCode)
                        {
                            var items = JsonConvert.DeserializeObject<IEnumerable<ItemDto>>(await response.Content.ReadAsStringAsync());
                            ListItemBindProp = new ObservableCollection<ItemDto>(items.Where(i => i.IsManaged && i.CurrentQuantity >= i.MinQuantity));
                            ListItemOutOfStockBindProp = new ObservableCollection<ItemDto>(items.Where(i => i.IsManaged && i.CurrentQuantity < i.MinQuantity));                      
                        }
                        response = await client.GetAsync(Properties.Resources.BaseUrl + "histories/");
                        if (response.IsSuccessStatusCode)
                        {
                            var histories = JsonConvert.DeserializeObject<IEnumerable<ImportExportHistoryDto>>(await response.Content.ReadAsStringAsync());
                            ListHistoryBindProp = new ObservableCollection<ImportExportHistoryDto>(histories);
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
