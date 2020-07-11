﻿using Dtos;
using Mobile.Enums;
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
using Xamarin.Forms;

namespace Mobile.ViewModels
{
    public class SessionPageViewModel : ViewModelBase
    {
        public SessionPageViewModel(InitParams initParams) : base(initParams)
        {
            ListOpenSessionBindProp = new ObservableCollection<SessionDto>();
            ListClosedSessionBindProp = new ObservableCollection<SessionDto>(); 
            SideBarBindProp = new ObservableCollection<SelectionModel>
            {
                new SelectionModel{ Name = "Phiên làm việc của két tiền", IsSelected = true },
                new SelectionModel{ Name = "Lịch sử két tiền", IsSelected = false },
            };
            Title = "Phiên làm việc của két tiền";
        }

        #region SideBarBindProp
        private ObservableCollection<SelectionModel> _SideBarBindProp;
        public ObservableCollection<SelectionModel> SideBarBindProp
        {
            get { return _SideBarBindProp; }
            set { SetProperty(ref _SideBarBindProp, value); }
        }
        #endregion

        #region SessionVisibleBindProp
        private bool _SessionVisibleBindProp = true;
        public bool SessionVisibleBindProp
        {
            get { return _SessionVisibleBindProp; }
            set { SetProperty(ref _SessionVisibleBindProp, value); }
        }
        #endregion

        #region SessionDetailVisibleBindProp
        private bool _SessionDetailVisibleBindProp = false;
        public bool SessionDetailVisibleBindProp
        {
            get { return _SessionDetailVisibleBindProp; }
            set { SetProperty(ref _SessionDetailVisibleBindProp, value); }
        }
        #endregion

        #region SessionHistoryVisibleBindProp
        private bool _SessionHistoryVisibleBindProp;
        public bool SessionHistoryVisibleBindProp
        {
            get { return _SessionHistoryVisibleBindProp; }
            set { SetProperty(ref _SessionHistoryVisibleBindProp, value); }
        }
        #endregion

        #region SessionBindProp
        private SessionDto _SessionBindProp;
        public SessionDto SessionBindProp
        {
            get { return _SessionBindProp; }
            set { SetProperty(ref _SessionBindProp, value); }
        }
        #endregion

        #region InitMoneyBindProp
        private double _InitMoneyBindProp = 0;
        public double InitMoneyBindProp
        {
            get { return _InitMoneyBindProp; }
            set { SetProperty(ref _InitMoneyBindProp, value); }
        }
        #endregion

        #region ListOpenSessionBindProp
        private ObservableCollection<SessionDto> _ListOpenSessionBindProp;
        public ObservableCollection<SessionDto> ListOpenSessionBindProp
        {
            get { return _ListOpenSessionBindProp; }
            set { SetProperty(ref _ListOpenSessionBindProp, value); }
        }
        #endregion

        #region ListClosedSessionBindProp
        private ObservableCollection<SessionDto> _ListClosedSessionBindProp;
        public ObservableCollection<SessionDto> ListClosedSessionBindProp
        {
            get { return _ListClosedSessionBindProp; }
            set { SetProperty(ref _ListClosedSessionBindProp, value); }
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
                    case "Phiên làm việc của két tiền":
                        Title = "Phiên làm việc của két tiền";
                        SessionVisibleBindProp = Application.Current.Properties.ContainsKey("session") ? false : true;
                        SessionDetailVisibleBindProp = !SessionVisibleBindProp;
                        SessionHistoryVisibleBindProp = false;
                        break;
                    case "Lịch sử két tiền":
                        Title = "Lịch sử két tiền";
                        SessionVisibleBindProp = false;
                        SessionDetailVisibleBindProp = false;
                        SessionHistoryVisibleBindProp = true;
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

        #region StartSessionCommand

        public DelegateCommand<object> StartSessionCommand { get; private set; }
        private async void OnStartSession(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            // Thuc hien cong viec tai day
            try
            {
                var ok = await PageDialogService.DisplayAlertAsync("", "Xác nhận bắt đầu thu tiền", "Đồng ý", "Không");
                if (ok)
                {
                    var json = JsonConvert.SerializeObject(InitMoneyBindProp);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    using (var client = new HttpClient())
                    {
                        var response = await client.PostAsync(Properties.Resources.BaseUrl + "sessions", content);
                        if (response.IsSuccessStatusCode)
                        {
                            var session = JsonConvert.DeserializeObject<SessionDto>(await response.Content.ReadAsStringAsync());
                            SessionBindProp = new SessionDto(session);
                            Application.Current.Properties["session"] = SessionBindProp;
                            await Application.Current.SavePropertiesAsync();
                            SessionDetailVisibleBindProp = true;
                            SessionVisibleBindProp = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await ShowErrorAsync(ex);
            }
            finally
            {
                IsBusy = false;
            }

        }

        [Initialize]
        private void InitStartSessionCommand()
        {
            StartSessionCommand = new DelegateCommand<object>(OnStartSession);
            StartSessionCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region EndSessionCommand

        public DelegateCommand<object> EndSessionCommand { get; private set; }
        private async void OnEndSession(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                Application.Current.Properties.Remove("Session");
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
        private void InitEndSessionCommand()
        {
            EndSessionCommand = new DelegateCommand<object>(OnEndSession);
            EndSessionCommand.ObservesCanExecute(() => IsNotBusy);
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
                        var task = client.GetAsync(Properties.Resources.BaseUrl + "sessions");
                        if (Application.Current.Properties.ContainsKey("session"))
                        {
                            SessionBindProp = Application.Current.Properties["session"] as SessionDto;
                            SessionDetailVisibleBindProp = true;
                            SessionVisibleBindProp = false;
                        }
                        var response = await task;
                        if (response.IsSuccessStatusCode)
                        {
                            var sessions = JsonConvert.DeserializeObject<IEnumerable<SessionDto>>(await response.Content.ReadAsStringAsync());
                            foreach (var session in sessions)
                            {
                                switch (session.Status)
                                {
                                    case (int)SessionStatus.Open:
                                        ListOpenSessionBindProp.Add(session);
                                        break;
                                    case (int)SessionStatus.Closed:
                                        ListClosedSessionBindProp.Add(session);
                                        break;
                                }
                            }
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
