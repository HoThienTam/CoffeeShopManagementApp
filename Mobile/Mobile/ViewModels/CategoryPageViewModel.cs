using Dtos;
using Mobile.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Mobile.ViewModels
{
    public class CategoryPageViewModel : ViewModelBase
    {
        public CategoryPageViewModel(InitParams initParams) : base(initParams)
        {
        }

        #region ListCategoryBindProp
        private ObservableCollection<CategoryDto> _ListCategoryBindProp = null;
        public ObservableCollection<CategoryDto> ListCategoryBindProp
        {
            get { return _ListCategoryBindProp; }
            set { SetProperty(ref _ListCategoryBindProp, value); }
        }
        #endregion

    }
}
