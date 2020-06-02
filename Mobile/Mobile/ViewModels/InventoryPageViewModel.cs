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

namespace Mobile.ViewModels
{
    public class InventoryPageViewModel : ViewModelBase
    {
        public InventoryPageViewModel(InitParams initParams) : base(initParams)
        {
            ListItemBindProp = new ObservableCollection<ItemDto>();
            ListItemOutOfStockBindProp = new ObservableCollection<ItemDto>();
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


        #region ModifyQuantityCommand

        public DelegateCommand<string> ModifyQuantityCommand { get; private set; }
        private async void OnModifyQuantity(string obj)
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
                    case "add":
                        await NavigationService.NavigateAsync(nameof(AddQuantityPage));
                        break;
                    case "reduce":
                        await NavigationService.NavigateAsync(nameof(ReduceQuantityPage));
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
        private void InitModifyQuantityCommand()
        {
            ModifyQuantityCommand = new DelegateCommand<string>(OnModifyQuantity);
            ModifyQuantityCommand.ObservesCanExecute(() => IsNotBusy);
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
                        var response = await client.GetAsync(Properties.Resources.BaseUrl + "items/");
                        if (response.IsSuccessStatusCode)
                        {
                            var items = JsonConvert.DeserializeObject<IEnumerable<ItemDto>>(await response.Content.ReadAsStringAsync());
                            ListItemBindProp = new ObservableCollection<ItemDto>(items.Where(i => i.IsManaged && i.CurrentQuantity >= i.MinQuantity));
                            ListItemOutOfStockBindProp = new ObservableCollection<ItemDto>(items.Where(i => i.IsManaged && i.CurrentQuantity < i.MinQuantity));
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
