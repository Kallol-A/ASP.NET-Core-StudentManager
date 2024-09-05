using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using StudentManager.Models;

namespace StudentManager.Services
{
    public interface IPermissionService
    {
        bool AddPermission(string Permission, string createdBy);
        bool LinkPermission(long RoleId, long PermissionId, string createdBy);
        IEnumerable<Permission> GetAllPermissions();
    }
}
