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
            Discounts = new ObservableCollection<DiscountForInvoiceDto>();
        }
       
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }

        #region TotalPrice
        private double _TotalPrice;
        public double TotalPrice
        {
            get { return _TotalPrice; }
            set { SetProperty(ref _TotalPrice, value); }
        }
        #endregion

        #region TableId
        private Guid? _TableId = null;
        public Guid? TableId
        {
            get { return _TableId; }
            set { SetProperty(ref _TableId, value); }
        }
        #endregion

        //#region TableName
        //private string _TableName;
        //public string TableName
        //{
        //    get { return _TableName; }
        //    set { SetProperty(ref _TableName, value); }
        //}
        //#endregion

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
        private ObservableCollection<DiscountForInvoiceDto> _Discounts;
        public ObservableCollection<DiscountForInvoiceDto> Discounts
        {
            get { return _Discounts; }
            set { SetProperty(ref _Discounts, value); }
        }
        #endregion
        public DateTime ClosedAt { get; set; }
    }
}
