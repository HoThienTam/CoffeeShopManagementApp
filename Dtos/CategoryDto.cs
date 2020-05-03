using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos
{
    public class CategoryDto : BindableBase
    {
        public CategoryDto()
        {
        }
        public CategoryDto(CategoryDto categoryDto)
        {
            Id = categoryDto.Id;
            Name = categoryDto.Name;
            Icon = categoryDto.Icon;
        }
        public CategoryDto(CategoryForCreateDto categoryDto)
        {
            Id = categoryDto.Id;
            Name = categoryDto.Name;
            Icon = categoryDto.Icon;
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
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { SetProperty(ref _Name, value); }
        }
        #endregion

        #region Icon
        private string _Icon;
        public string Icon
        {
            get { return _Icon; }
            set { SetProperty(ref _Icon, value); }
        }
        #endregion

        #region Items
        private ICollection<ItemDto> _Items;
        public ICollection<ItemDto> Items
        {
            get { return _Items; }
            set { SetProperty(ref _Items, value); }
        }
        #endregion
    }
}
