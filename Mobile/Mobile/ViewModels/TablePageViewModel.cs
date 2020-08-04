using Dtos;
using ImTools;
using Mobile.Models;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace Mobile.ViewModels
{
    public class TablePageViewModel : ViewModelBase
    {
        public TablePageViewModel(InitParams initParams) : base(initParams)
        {
        }

        #region ZoneBindProp
        private ZoneDto _ZoneBindProp;
        public ZoneDto ZoneBindProp
        {
            get { return _ZoneBindProp; }
            set { SetProperty(ref _ZoneBindProp, value); }
        }
        #endregion

        #region TempZone
        private ZoneDto _TempZone;
        public ZoneDto TempZone
        {
            get { return _TempZone; }
            set { SetProperty(ref _TempZone, value); }
        }
        #endregion

        #region SaveCommand

        public DelegateCommand<object> SaveCommand { get; private set; }
        private async void OnSave(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                var json = JsonConvert.SerializeObject(TempZone);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                // Thuc hien cong viec tai day
                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    response = await client.PutAsync(Properties.Resources.BaseUrl + "zones/", content);
                    if (response.IsSuccessStatusCode)
                    {
                        ZoneBindProp.Name = TempZone.Name;
                        ZoneBindProp.Tables = TempZone.Tables;
                        await NavigationService.GoBackAsync();
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
        private void InitSaveCommand()
        {
            SaveCommand = new DelegateCommand<object>(OnSave);
            SaveCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region AddNewTableCommand

        public DelegateCommand<object> AddNewTableCommand { get; private set; }
        private async void OnAddNewTable(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                TempZone.Tables.Add(new TableDto { Name = (TempZone.Tables.Count + 1).ToString() });
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
        private void InitAddNewTableCommand()
        {
            AddNewTableCommand = new DelegateCommand<object>(OnAddNewTable);
            AddNewTableCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region DeleteTableCommand

        public DelegateCommand<TableDto> DeleteTableCommand { get; private set; }
        private async void OnDeleteTable(TableDto obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                var ok = await DisplayDeleteAlertAsync();
                if (ok)
                {
                    TempZone.Tables.Remove(obj);
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
        private void InitDeleteTableCommand()
        {
            DeleteTableCommand = new DelegateCommand<TableDto>(OnDeleteTable);
            DeleteTableCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region DeleteZoneCommand

        public DelegateCommand<object> DeleteZoneCommand { get; private set; }
        private async void OnDeleteZone(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                var ok = await DisplayDeleteAlertAsync();
                if (ok)
                {
                    using (var client = new HttpClient())
                    {
                        HttpResponseMessage response = new HttpResponseMessage();
                        response = await client.DeleteAsync(Properties.Resources.BaseUrl + "zones/" + ZoneBindProp.Id);
                        if (response.IsSuccessStatusCode)
                        {
                            var param = new NavigationParameters();
                            param.Add(nameof(ZoneBindProp), ZoneBindProp);
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
        private void InitDeleteZoneCommand()
        {
            DeleteZoneCommand = new DelegateCommand<object>(OnDeleteZone);
            DeleteZoneCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            switch (parameters.GetNavigationMode())
            {
                case NavigationMode.Back:
                    break;
                case NavigationMode.New:
                    TempZone = new ZoneDto(ZoneBindProp);
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
