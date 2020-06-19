using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Dtos
{
    public class ItemForInvoiceDto : BindableBase
    {
        public ItemForInvoiceDto(ItemDto item)
        {
            Id = item.Id;
            Name = item.Name;
            Price = item.Price;
            Discounts = new ObservableCollection<DiscountDto>();
        }

        public ItemForInvoiceDto()
        {
        }

        #region Id
        private Guid _Id = Guid.Empty;
        public Guid Id
        {
            get { return _Id; }
            set { SetProperty(ref _Id, value); }
        }
        #endregion

        #region Name
        private string _Name = null;
        public string Name
        {
            get { return _Name; }
            set { SetProperty(ref _Name, value); }
        }
        #endregion

        #region Price
        private double _Price = 0;
        public double Price
        {
            get { return _Price; }
            set { SetProperty(ref _Price, value); }
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

        #region Quantity
        private int _Quantity = 1;
        public int Quantity
        {
            get { return _Quantity; }
            set { SetProperty(ref _Quantity, value); }
        }
        #endregion

    }
}
