using System.Collections.Generic;


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
}
