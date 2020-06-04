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

namespace Mobile.ViewModels
{
    public class ReduceQuantityPageViewModel : ViewModelBase
    {
        private string _reason;
        public ReduceQuantityPageViewModel(InitParams initParams) : base(initParams)
        {
            ExportReasonsBindProp = new ObservableCollection<ReasonModel>
            {
                new ReasonModel{Reason = "Bị hỏng", IsSelected = false},
                new ReasonModel{Reason = "Đã hết hạn", IsSelected = false},
                new ReasonModel{Reason = "Điều chỉnh số dư", IsSelected = false}
            };
        }
        #region ExportReasonsBindProp
        private ObservableCollection<ReasonModel> _ExportReasonsBindProp = null;
        public ObservableCollection<ReasonModel> ExportReasonsBindProp
        {
            get { return _ExportReasonsBindProp; }
            set { SetProperty(ref _ExportReasonsBindProp, value); }
        }
        #endregion

        #region QuantityBindProp
        private int _QuantityBindProp = 0;
        public int QuantityBindProp
        {
            get { return _QuantityBindProp; }
            set { SetProperty(ref _QuantityBindProp, value); }
        }
        #endregion

        #region OtherReasonBindProp
        private string _OtherReasonBindProp = string.Empty;
        public string OtherReasonBindProp
        {
            get { return _OtherReasonBindProp; }
            set { SetProperty(ref _OtherReasonBindProp, value); }
        }
        #endregion

        #region IsSelectingOtherReason
        private bool _IsSelectingOtherReason = false;
        public bool IsSelectingOtherReason
        {
            get { return _IsSelectingOtherReason; }
            set { SetProperty(ref _IsSelectingOtherReason, value); }
        }
        #endregion

        #region ItemBindProp
        private ItemDto _ItemBindProp;
        public ItemDto ItemBindProp
        {
            get { return _ItemBindProp; }
            set { SetProperty(ref _ItemBindProp, value); }
        }
        #endregion

        #region SelectReasonCommand

        public DelegateCommand<ReasonModel> SelectReasonCommand { get; private set; }
        private async void OnSelectReason(ReasonModel obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                var reason = ExportReasonsBindProp.FirstOrDefault(i => i.IsSelected);
                if (reason != null)
                {
                    reason.IsSelected = false;
                }
                if (obj is ReasonModel)
                {
                    obj.IsSelected = true;
                    IsSelectingOtherReason = false;
                    _reason = obj.Reason;
                }
                else
                {
                    IsSelectingOtherReason = true;
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
        private void InitSelectReasonCommand()
        {
            SelectReasonCommand = new DelegateCommand<ReasonModel>(OnSelectReason);
            SelectReasonCommand.ObservesCanExecute(() => IsNotBusy);
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

                var param = new NavigationParameters();

                if (QuantityBindProp > 0)
                {
                    var history = new ImportExportHistoryDto
                    {
                        ItemId = ItemBindProp.Id,
                        IsImported = false,
                        Quantity = QuantityBindProp,
                        Reason = IsSelectingOtherReason == true ? OtherReasonBindProp : _reason
                    };
                    var json = JsonConvert.SerializeObject(history);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    using (var client = new HttpClient())
                    {
                        var response = await client.PostAsync(Properties.Resources.BaseUrl + "histories/", content);
                        if (response.IsSuccessStatusCode)
                        {
                            ItemBindProp.CurrentQuantity -= history.Quantity;
                            param.Add("History", history);
                            await NavigationService.GoBackAsync(param);
                        }
                    }
                }
                else
                {
                    await PageDialogService.DisplayAlertAsync("Cảnh báo", "Bạn chưa nhập số lượng giảm!", "OK");
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
        private void InitSaveCommand()
        {
            SaveCommand = new DelegateCommand<object>(OnSave);
            SaveCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

    }
}
