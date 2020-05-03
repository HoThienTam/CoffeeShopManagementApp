﻿using Prism.Mvvm;
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
            CategoryId = itemDto.CategoryId;
            Category = itemDto.Category;
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

        public string Image { get; set; }
        public bool IsManaged { get; set; }
        public int MinQuantity { get; set; }
        public int CurrentQuantity { get; set; }

        #region CategoryId
        private Guid _CategoryId = Guid.Empty;

        public Guid CategoryId
        {
            get { return _CategoryId; }
            set { SetProperty(ref _CategoryId, value); }
        }
        #endregion

        #region Category
        private CategoryForCreateDto _Category;
        public CategoryForCreateDto Category
        {
            get { return _Category; }
            set { SetProperty(ref _Category, value); }
        }
        #endregion

    }
}
