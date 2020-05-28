using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos
{
    public class ItemDto : BindableBase
    {
        public ItemDto()
        {
        }
        public ItemDto(ItemDto itemDto)
        {
            Id = itemDto.Id;
            Name = itemDto.Name;
            Image = itemDto.Image;
            Price = itemDto.Price;
            IsManaged = itemDto.IsManaged;
            MinQuantity = itemDto.MinQuantity;
            CurrentQuantity = itemDto.CurrentQuantity;
        }

        public Guid Id { get; set; }

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

        public string Image { get; set; }

        #region IsManaged
        private bool _IsManaged;
        public bool IsManaged
        {
            get { return _IsManaged; }
            set { SetProperty(ref _IsManaged, value); }
        }
        #endregion

        public Guid CategoryId { get; set; }
        public int MinQuantity { get; set; }
        public int CurrentQuantity { get; set; }
    }
}
