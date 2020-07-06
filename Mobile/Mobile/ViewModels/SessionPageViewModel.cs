using Mobile.Models;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Mobile.ViewModels
{
    public class SessionPageViewModel : ViewModelBase
    {
        public SessionPageViewModel(InitParams initParams) : base(initParams)
        {
            SideBarBindProp = new ObservableCollection<SelectionModel>
            {
                new SelectionModel{ Name = "Phiên làm việc của két tiền", IsSelected = true },
                new SelectionModel{ Name = "Lịch sử két tiền", IsSelected = false },
            };
            Title = "Két tiền";
        }

        #region SideBarBindProp
        private ObservableCollection<SelectionModel> _SideBarBindProp;
        public ObservableCollection<SelectionModel> SideBarBindProp
        {
            get { return _SideBarBindProp; }
            set { SetProperty(ref _SideBarBindProp, value); }
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
                        Title = "Doanh thu tổng quan";
                        break;
                    case "Mặt hàng bán chạy":
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

    }
}
