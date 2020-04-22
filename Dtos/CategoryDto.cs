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
        public Guid Id { get; set; }

        #region Name
        private string _Name = null;
        public string Name
        {
            get { return _Name; }
            set { SetProperty(ref _Name, value); }
        }
        #endregion

        #region Icon
        private string _Icon = null;
        public string Icon
        {
            get { return _Icon; }
            set { SetProperty(ref _Icon, value); }
        }
        #endregion

    }
}
