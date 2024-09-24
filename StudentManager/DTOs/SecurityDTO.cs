using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManager.DTOs
{
    public class SecurityDTO
    {
    }

    public class RoleDTO
    {
        public long RoleId { get; set; }
        public string RoleName { get; set; }
    }

    public class PermissionDTO
    {
        public long PermissionId { get; set; }
        public string PermissionName { get; set; }
    }

    public class RolePermissionDTO
    {
        public long PermissionId { get; set; }
        public long RoleId { get; set; }
        public string PermissionName { get; set; }
        public string RoleName { get; set; }
    }
}
