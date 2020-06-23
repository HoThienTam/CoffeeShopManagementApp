using ImTools;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Dtos
{
    public class ZoneDto : BaseDto
    {
        public ZoneDto()
        {
        }
        public ZoneDto(ZoneDto zone)
        {
            Id = zone.Id;
            Name = zone.Name;
            Tables = new ObservableCollection<TableDto>(zone.Tables.Select(t => new TableDto { Id = t.Id, Name = t.Name }).ToList());
        }

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
