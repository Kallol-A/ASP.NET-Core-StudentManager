using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManager.DTOs
{
    public class CreateUserDTO
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public string UserPhone { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
    }

    public class UserDTO
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public string UserPhone { get; set; }
        public string UserEmail { get; set; }
        public string RoleName { get; set; }
    }

    public class UserByIdDTO
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public string UserPhone { get; set; }
        public string UserEmail { get; set; }
    }
}
