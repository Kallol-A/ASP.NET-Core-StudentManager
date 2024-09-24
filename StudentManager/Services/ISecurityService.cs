using System.Collections.Generic;
using System.Threading.Tasks;

using StudentManager.DTOs;
using StudentManager.Models;

namespace StudentManager.Services
{
    public interface ISecurityService
    {
        Task<bool> InsertRoleAsync(Role role);
        Task<bool> InsertPermissionAsync(Permission permission);
        Task<bool> LinkPermissionRoleAsync(RolePermission rolepermission);

        Task<bool> UpdateRoleAsync(long id_role, Role role);
        Task<bool> UpdatePermissionAsync(long id_permission, Permission permission);

        Task<IEnumerable<RoleDTO>> GetRolesAsync(long? id);
        Task<IEnumerable<PermissionDTO>> GetPermissionsAsync(long? id);
        Task<IEnumerable<RolePermissionDTO>> GetRolePermissionsAsync();
    }
}
