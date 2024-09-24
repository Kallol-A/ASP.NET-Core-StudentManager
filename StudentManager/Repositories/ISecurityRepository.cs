using System.Collections.Generic;
using System.Threading.Tasks;

using StudentManager.DTOs;
using StudentManager.Models;

namespace StudentManager.Repositories
{
    public interface ISecurityRepository
    {
        void InsertRoleAsync(Role role);
        void InsertPermissionAsync(Permission permission);
        void LinkPermissionRoleAsync(RolePermission rolepermission);
        void UpdateRoleAsync(Role role);
        void UpdatePermissionAsync(Permission permission);
        Task<bool> SaveAsync();
        Task<IEnumerable<Role>> GetRolesAsync(long? id);
        Task<IEnumerable<Permission>> GetPermissionsAsync(long? id);
        Task<IEnumerable<RolePermission>> GetRolePermissionsAsync();
    }
}
