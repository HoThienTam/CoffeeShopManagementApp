using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Models
{
    public class User : BaseModel
    {
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Fullname { get; set; }
        public string Role { get; set; }
    }
}
