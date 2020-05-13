using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Dtos
{
    public class InvoiceDto : BindableBase
    {
        public InvoiceDto()
        {
            Items = new ObservableCollection<ItemForInvoiceDto>();
            Discounts = new ObservableCollection<DiscountDto>();
        }
       
        public Guid Id { get; set; }

        #region InvoiceNumber
        private string _InvoiceNumber;
        public string InvoiceNumber
        {
            get { return _InvoiceNumber; }
            set { SetProperty(ref _InvoiceNumber, value); }
        }
        #endregion

        #region Status
        private int _Status;
        public int Status
        {
            get { return _Status; }
            set { SetProperty(ref _Status, value); }
        }
        #endregion

        #region ClosedAt
        private DateTime _ClosedAt;
        public DateTime ClosedAt
        {
            get { return _ClosedAt; }
            set { SetProperty(ref _ClosedAt, value); }
        }
        #endregion

        #region TotalPrice
        private double _TotalPrice;
        public double TotalPrice
        {
            get { return _TotalPrice; }
            set { SetProperty(ref _TotalPrice, value); }
        }
        #endregion

        #region PaidAmount
        private double _PaidAmount;
        public double PaidAmount
        {
            get { return _PaidAmount; }
            set { SetProperty(ref _PaidAmount, value); }
        }
        #endregion

        #region Tip
        private double _Tip;
        public double Tip
        {
            get { return _Tip; }
            set { SetProperty(ref _Tip, value); }
        }
        #endregion

        #region TableId
        private Guid _TableId = Guid.Empty;
        public Guid TableId
        {
            get { return _TableId; }
            set { SetProperty(ref _TableId, value); }
        }
        #endregion

        #region Table
        private TableDto _Table;
        public TableDto Table
        {
            get { return _Table; }
            set { SetProperty(ref _Table, value); }
        }
        #endregion

        #region Items
        private ObservableCollection<ItemForInvoiceDto> _Items;
        public ObservableCollection<ItemForInvoiceDto> Items
        {
            get { return _Items; }
            set { SetProperty(ref _Items, value); }
        }
        #endregion

        #region Discounts
        private ObservableCollection<DiscountDto> _Discounts;
        public ObservableCollection<DiscountDto> Discounts
        {
            get { return _Discounts; }
            set { SetProperty(ref _Discounts, value); }
        }
        #endregion
    }
}
