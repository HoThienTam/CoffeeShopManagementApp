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
using Telerik.XamarinForms.Input.Calendar;

namespace Mobile.ViewModels
{
    public class AddQuantityPageViewModel : ViewModelBase
    {
        private string _reason;
        public AddQuantityPageViewModel(InitParams initParams) : base(initParams)
        {
            ImportReasonsBindProp = new ObservableCollection<SelectionModel>
            {
                new SelectionModel{Name = "Nhập thêm", IsSelected = false},
                new SelectionModel{Name = "Điều chỉnh số dư", IsSelected = false}
            };
        }

        #region ImportReasonsBindProp
        private ObservableCollection<SelectionModel> _ImportReasonsBindProp = null;
        public ObservableCollection<SelectionModel> ImportReasonsBindProp
        {
            get { return _ImportReasonsBindProp; }
            set { SetProperty(ref _ImportReasonsBindProp, value); }
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

        public DelegateCommand<object> SelectReasonCommand { get; private set; }
        private async void OnSelectReason(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                var reason = ImportReasonsBindProp.FirstOrDefault(i => i.IsSelected);
                if (reason != null)
                {
                    reason.IsSelected = false;
                }
                if (obj is SelectionModel)
                {
                    var selection = obj as SelectionModel;
                    selection.IsSelected = true;
                    IsSelectingOtherReason = false;
                    _reason = selection.Name;
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
            SelectReasonCommand = new DelegateCommand<object>(OnSelectReason);
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
                        Quantity = QuantityBindProp,
                        ItemId = ItemBindProp.Id,
                        ItemName = ItemBindProp.Name,
                        Reason = IsSelectingOtherReason == true ? OtherReasonBindProp : _reason
                    };
                    var json = JsonConvert.SerializeObject(history);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    using (var client = new HttpClient())
                    {
                        var response = await client.PostAsync(Properties.Resources.BaseUrl + "histories/", content);
                        if (response.IsSuccessStatusCode)
                        {
                            var newHistory = JsonConvert.DeserializeObject<ImportExportHistoryDto>(await response.Content.ReadAsStringAsync());
                            ItemBindProp.CurrentQuantity += newHistory.Quantity;
                            param.Add("History", newHistory);
                            await NavigationService.GoBackAsync(param);
                        }
                        else
                        {
                            await PageDialogService.DisplayAlertAsync("Lỗi hệ thống", await response.Content.ReadAsStringAsync(), "Đóng");
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
