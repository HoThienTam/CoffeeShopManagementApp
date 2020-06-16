using Mobile.Models;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Mobile.ViewModels
{
    public class ReportPageViewModel : ViewModelBase
    {
        public ReportPageViewModel(InitParams initParams) : base(initParams)
        {
            SideBarBindProp = new ObservableCollection<SelectionModel>
            {
                new SelectionModel{ Name = "Doanh thu tổng quan", IsSelected = true },
                new SelectionModel{ Name = "Mặt hàng bán chạy", IsSelected = false },
            };
            Title = "Thong ke";
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

        #region SelectedDateBindProp
        private DateTime _SelectedDateBindProp;
        public DateTime SelectedDateBindProp
        {
            get { return _SelectedDateBindProp; }
            set { SetProperty(ref _SelectedDateBindProp, value); }
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
                        break;
                    case "Mặt hàng bán chạy":
                        TopSellersVisibleBindProp = true;
                        OverallVisibleBindProp = false;
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
    }
}
