using Mobile.Models;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using Telerik.XamarinForms.Input.Calendar;

namespace Mobile.ViewModels
{
    public class CalendarPageViewModel : ViewModelBase
    {
        private DateTime _today;
        private DateTime _yesterday;
        private DateTime _thisWeekStart;
        private DateTime _thisWeekEnd;
        private DateTime _lastWeekStart;
        private DateTime _lastWeekEnd;
        private DateTime _thisMonthStart;
        private DateTime _thisMonthEnd;
        private DateTime _lastMonthStart;
        private DateTime _lastMonthEnd;
        public CalendarPageViewModel(InitParams initParams) : base(initParams)
        {
            DateTime baseDate = DateTime.Today;
            _today = baseDate;
            _yesterday = baseDate.AddDays(-1);
            _thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
            _thisWeekEnd = _thisWeekStart.AddDays(7).AddSeconds(-1);
            _lastWeekStart = _thisWeekStart.AddDays(-7);
            _lastWeekEnd = _thisWeekStart.AddSeconds(-1);
            _thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
            _thisMonthEnd = _thisMonthStart.AddMonths(1).AddSeconds(-1);
            _lastMonthStart = _thisMonthStart.AddMonths(-1);
            _lastMonthEnd = _thisMonthStart.AddSeconds(-1);
        }

        #region DateRangeBindProp
        private DateTimeRange _DateRangeBindProp = null;
        public DateTimeRange DateRangeBindProp
        {
            get { return _DateRangeBindProp; }
            set { SetProperty(ref _DateRangeBindProp, value); }
        }
        #endregion

        #region SelectTimeCommand

        public DelegateCommand<string> SelectTimeCommand { get; private set; }
        private async void OnSelectTime(string time)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                switch (time)
                {
                    case "today":
                        DateRangeBindProp = new DateTimeRange(_today.Date, _today.Date);
                        break;
                    case "yesterday":
                        DateRangeBindProp = new DateTimeRange(_yesterday.Date, _yesterday.Date);
                        break;
                    case "thisweek":
                        DateRangeBindProp = new DateTimeRange(_thisWeekStart.Date, _lastWeekEnd.Date);
                        break;
                    case "lastweek":
                        DateRangeBindProp = new DateTimeRange(_lastWeekStart.Date, _lastWeekEnd.Date);
                        break;
                    case "thismonth":
                        DateRangeBindProp = new DateTimeRange(_thisMonthStart.Date, _thisMonthEnd.Date);
                        break;
                    case "lastmonth":
                        DateRangeBindProp = new DateTimeRange(_lastMonthStart.Date, _lastMonthEnd.Date);
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
        private void InitSelectTimeCommand()
        {
            SelectTimeCommand = new DelegateCommand<string>(OnSelectTime);
            SelectTimeCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region SaveCommand

        public DelegateCommand<string> SaveCommand { get; private set; }
        private async void OnSave(string time)
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
                param.Add(nameof(DateRangeBindProp), DateRangeBindProp);
                await NavigationService.GoBackAsync(param);

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
            SaveCommand = new DelegateCommand<string>(OnSave);
            SaveCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion
    }
}
