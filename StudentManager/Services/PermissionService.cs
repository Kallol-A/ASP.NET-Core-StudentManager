using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebApi.Data;
using WebApi.Models;

namespace WebApi.Services
{
    public interface IPermissionService
    {
        bool AddPermission(string Permission, string createdBy);
        bool LinkPermission(long RoleId, long PermissionId, string createdBy);

        IEnumerable<Permission> GetAllPermissions();
    }

    public class PermissionService : IPermissionService
    {
        private readonly AppDbContext _dbContext;

        // Constructor
        public PermissionService(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public bool AddPermission(string Permission, string createdBy)
        {
            try
            {
                var _permission = new Permission
                {
                    permission = Permission,
                    created_by_user = createdBy,
                    created_at = DateTime.Now
                };

                _dbContext.Permissions.Add(_permission);
                _dbContext.SaveChanges();

                return true; // Operation succeeded
            }
            catch (Exception ex)
            {
                // Log the exception

                return false; // Operation failed
            }
        }

        public IEnumerable<Permission> GetAllPermissions()
        {
            return _dbContext.Permissions.ToList();
        }

        public bool LinkPermission(long RoleId, long PermissionId, string createdBy)
        {
            try
            {
                var _rolepermission = new RolePermission
                {
                    id_role = RoleId,
                    id_permission = PermissionId,
                    created_by_user = createdBy,
                    created_at = DateTime.Now
                };

                _dbContext.RolePermissions.Add(_rolepermission);
                _dbContext.SaveChanges();

                return true; // Operation succeeded
            }
            catch (Exception ex)
            {
                // Log the exception
                var a = ex.Message;

                return false; // Operation failed
            }
        }
    }
}
