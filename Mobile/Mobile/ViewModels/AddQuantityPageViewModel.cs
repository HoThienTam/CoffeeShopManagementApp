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
            ImportReasonsBindProp = new ObservableCollection<ReasonModel>
            {
                new ReasonModel{Reason = "Nhập thêm", IsSelected = false},
                new ReasonModel{Reason = "Điều chỉnh số dư", IsSelected = false}
            };
        }

        #region ImportReasonsBindProp
        private ObservableCollection<ReasonModel> _ImportReasonsBindProp = null;
        public ObservableCollection<ReasonModel> ImportReasonsBindProp
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
                var reason = ImportReasonsBindProp.FirstOrDefault(i => i.IsSelected);
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
                        IsImported = true,
                        Quantity = QuantityBindProp,
                        ItemId = ItemBindProp.Id,
                        Reason = IsSelectingOtherReason == true ? OtherReasonBindProp : _reason
                    };
                    using(var client = new HttpClient())
                    {
                        var response = await client.GetAsync(Properties.Resources.BaseUrl + "histories/");
                        if (response.IsSuccessStatusCode)
                        {
                            var historyDto = JsonConvert.DeserializeObject<IEnumerable<ImportExportHistoryDto>>(await response.Content.ReadAsStringAsync());
                            ItemBindProp.CurrentQuantity += history.Quantity;
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
