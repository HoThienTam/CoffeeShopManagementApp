using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos
{
    public class ItemForCreateDto : BindableBase
    {
        public ItemForCreateDto(ItemDto itemDto)
        {
            Id = itemDto.Id;
            Name = itemDto.Name;
            Price = itemDto.Price;
            CategoryId = itemDto.CategoryId;
        }

        public ItemForCreateDto()
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

        #region CategoryId
        private Guid _CategoryId = Guid.Empty;

        public Guid CategoryId
        {
            get { return _CategoryId; }
            set { SetProperty(ref _CategoryId, value); }
        }
        #endregion
    }
}
