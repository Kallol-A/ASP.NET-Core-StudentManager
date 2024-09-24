using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using StudentManager.Data;
using StudentManager.Models;

namespace StudentManager.Repositories
{
    public class SecurityRepository : BaseRepository, ISecurityRepository
    {
        private new readonly AppDbContext _dbContext;

        // Constructor
        public SecurityRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async void InsertRoleAsync(Role role)
        {
            var validRole = role ?? throw new ArgumentNullException(nameof(role));
            await _dbContext.Roles.AddAsync(validRole);
        }

        public async void InsertPermissionAsync(Permission permission)
        {
            var validPermission = permission ?? throw new ArgumentNullException(nameof(permission));
            await _dbContext.Permissions.AddAsync(validPermission);
        }

        public async void LinkPermissionRoleAsync(RolePermission rolepermission)
        {
            var validRolePermission = rolepermission ?? throw new ArgumentNullException(nameof(rolepermission));
            await _dbContext.RolePermissions.AddAsync(validRolePermission);
        }

        public async Task<IEnumerable<Role>> GetRolesAsync(long? id)
        {
            if (id.HasValue)
            {
                return await _dbContext.Roles
                    .Where(u => u.deleted_at == null && u.id_role == id.Value)
                    .ToListAsync(); // Will return one or none records in a list
            }

            // Fetch all records if no id is passed
            return await _dbContext.Roles
                .Where(u => u.deleted_at == null)
                .ToListAsync();
        }

        public async void UpdateRoleAsync(Role role)
        {
            // Find the existing role by ID
            var filteredRole = await _dbContext.Roles
                .FirstOrDefaultAsync(sc => sc.id_role == role.id_role && sc.deleted_at == null);

            var validRole = filteredRole ?? throw new ArgumentNullException(nameof(filteredRole));

            // Update the fields
            filteredRole.role_name = role.role_name;
            filteredRole.last_updated_by_user = role.last_updated_by_user;
            filteredRole.updated_at = role.updated_at;

            // Save the changes in the database
            _dbContext.Roles.Update(filteredRole);
        }

        public async Task<IEnumerable<Permission>> GetPermissionsAsync(long? id)
        {
            if (id.HasValue)
            {
                return await _dbContext.Permissions
                    .Where(u => u.deleted_at == null && u.id_permission == id.Value)
                    .ToListAsync(); // Will return one or none records in a list
            }

            // Fetch all records if no id is passed
            return await _dbContext.Permissions
                .Where(u => u.deleted_at == null)
                .ToListAsync();
        }

        public async void UpdatePermissionAsync(Permission permission)
        {
            // Find the existing permission by ID
            var filteredPermission = await _dbContext.Permissions
                .FirstOrDefaultAsync(sc => sc.id_permission == permission.id_permission && sc.deleted_at == null);

            var validPermission = filteredPermission ?? throw new ArgumentNullException(nameof(filteredPermission));

            // Update the fields
            filteredPermission.permission_name = permission.permission_name;
            filteredPermission.last_updated_by_user = permission.last_updated_by_user;
            filteredPermission.updated_at = permission.updated_at;

            // Save the changes in the database
            _dbContext.Permissions.Update(filteredPermission);
        }

        public async Task<IEnumerable<RolePermission>> GetRolePermissionsAsync()
        {
            // Fetch role permissions from the database
            return await _dbContext.RolePermissions
                .Include(rp => rp.Role) // Include Role details
                .Include(rp => rp.Permission) // Include Permission details
                .Where(rp => rp.deleted_at == null) // Optional: if soft deletion is used
                .ToListAsync();
        }
    }
}
