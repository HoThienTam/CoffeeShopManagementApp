using Dtos;
using Mobile.Models;
using Mobile.Views;
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
    public class ZonePageViewModel : ViewModelBase
    {
        public ZonePageViewModel(InitParams initParams) : base(initParams)
        {
            ListZoneBindProp = new ObservableCollection<ZoneDto>();
        }

        #region ListZoneBindProp
        private ObservableCollection<ZoneDto> _ListZoneBindProp = null;
        public ObservableCollection<ZoneDto> ListZoneBindProp
        {
            get { return _ListZoneBindProp; }
            set { SetProperty(ref _ListZoneBindProp, value); }
        }
        #endregion

        #region AddNewZoneCommand

        public DelegateCommand<object> AddNewZoneCommand { get; private set; }
        private async void OnAddNewZone(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                var zoneToCreate = new ZoneDto { Name = "New Zone" };
                var json = JsonConvert.SerializeObject(zoneToCreate);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                // Thuc hien cong viec tai day
                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    response = await client.PostAsync(Properties.Resources.BaseUrl + "zones/", content);
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.Created:
                            var zone = JsonConvert.DeserializeObject<ZoneDto>(await response.Content.ReadAsStringAsync());
                            ListZoneBindProp.Add(zone);
                            break;
                        case HttpStatusCode.BadRequest:
                            await PageDialogService.DisplayAlertAsync("Lỗi", $"{await response.Content.ReadAsStringAsync()}", "Đóng");
                            break;
                        default:
                            await PageDialogService.DisplayAlertAsync("Lỗi", $"Lỗi hệ thống!", "Đóng");
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
        private void InitAddNewZoneCommand()
        {
            AddNewZoneCommand = new DelegateCommand<object>(OnAddNewZone);
            AddNewZoneCommand.ObservesCanExecute(() => IsNotBusy);
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
                var param = new NavigationParameters();
                param.Add("ZoneBindProp", obj);
                await NavigationService.NavigateAsync(nameof(TablePage), param);
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

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            switch (parameters.GetNavigationMode())
            {
                case NavigationMode.Back:
                    if (parameters.ContainsKey("ZoneBindProp"))
                    {
                        var zone = parameters["ZoneBindProp"] as ZoneDto;
                        ListZoneBindProp.Remove(zone);
                    }
                    break;
                case NavigationMode.New:
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
