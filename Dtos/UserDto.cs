using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos
{
    public class UserDto
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

        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        public int Role { get; set; }
    }
}
