using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Dtos
{
    public class ZoneDto : BindableBase
    {
        public ZoneDto()
        {
        }
        public ZoneDto(ZoneDto zone)
        {
            Id = zone.Id;
            Name = zone.Name;
            Tables = new ObservableCollection<TableDto>(zone.Tables);
        }

        public Guid Id { get; set; }

        #region Name
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { SetProperty(ref _Name, value); }
        }
        #endregion

        #region Tables
        private ObservableCollection<TableDto> _Tables = new ObservableCollection<TableDto>();
        public ObservableCollection<TableDto> Tables
        {
            get { return _Tables; }
            set { SetProperty(ref _Tables, value); }
        }
        #endregion

    }
}
