﻿using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        public InvoiceDto(InvoiceDto invoice)
        {
            Id = invoice.Id;
            TotalPrice = invoice.TotalPrice;
            TableId = invoice.TableId;
            Table = invoice.Table;
            Items = new ObservableCollection<ItemForInvoiceDto>(invoice.Items.Select(t => new ItemForInvoiceDto
            {
                Id = t.Id,
                Name = t.Name,
                Discounts = t.Discounts,
                Quantity = t.Quantity,
                Value = t.Value
            }).ToList());
            Discounts = new ObservableCollection<DiscountForInvoiceDto>(invoice.Discounts.Select(t => new DiscountForInvoiceDto
            {
                Id = t.Id,
                Name = t.Name,
                Value = t.Value
            }).ToList());
        }

        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }

        public string CreatedDate { get { return CreatedAt.Date.ToShortDateString(); } }

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
        public double PaidAmount { get; set; }
    }
}
