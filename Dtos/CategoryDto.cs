using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Dtos
{
    public class CategoryDto : BaseDto
    {
        public CategoryDto()
        {
        }

        public CategoryDto(CategoryDto categoryDto)
        {
            Id = categoryDto.Id;
            Name = categoryDto.Name;
            Icon = categoryDto.Icon;
            Items = new ObservableCollection<ItemDto>(categoryDto.Items.Select(t => new ItemDto 
            { 
                Id = t.Id,
                Name = t.Name,
                Image = t.Image,
                Price = t.Price, 
                IsManaged = t.IsManaged,
                MinQuantity = t.MinQuantity 
            }).ToList());
        }

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
        private ObservableCollection<ItemDto> _Items;
        public ObservableCollection<ItemDto> Items
        {
            get { return _Items; }
            set { SetProperty(ref _Items, value); }
        }
        #endregion
    }
}
