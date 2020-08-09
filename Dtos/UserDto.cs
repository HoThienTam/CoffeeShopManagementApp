using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos
{
    public class UserDto : BaseDto
    {
        public UserDto()
        {
        }

        public UserDto(UserDto user)
        {
            Id = user.Id;
            Username = user.Username;
            Fullname = user.Fullname;
            Role = user.Role;
        }

        #region Username
        private string _Username;
        public string Username
        {
            get { return _Username; }
            set { SetProperty(ref _Username, value); }
        }
        #endregion

        public string Password { get; set; }


        #region Fullname
        private string _Fullname;
        public string Fullname
        {
            get { return _Fullname; }
            set { SetProperty(ref _Fullname, value); }
        }
        #endregion

        #region Role
        private int _Role;
        public int Role
        {
            get { return _Role; }
            set { SetProperty(ref _Role, value); }
        }
        #endregion

    }
}
