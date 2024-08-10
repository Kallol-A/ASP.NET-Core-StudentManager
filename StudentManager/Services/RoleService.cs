using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebApi.Data;
using WebApi.Models;

namespace WebApi.Services
{
    public interface IRoleService
    {
        bool AddRole(string role, string createdBy);

        IEnumerable<Role> GetAllRoles();
    }

    public class RoleService : IRoleService
    {
        private readonly AppDbContext _dbContext;

        // Constructor
        public RoleService(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public bool AddRole(string role, string createdBy)
        {
            try
            {
                var _role = new Role
                {
                    role = role,
                    created_by_user = createdBy,
                    created_at = DateTime.Now
                };

                _dbContext.Roles.Add(_role);
                _dbContext.SaveChanges();

                return true; // Operation succeeded
            }
            catch (Exception ex)
            {
                // Log the exception

                return false; // Operation failed
            }
        }

        public IEnumerable<Role> GetAllRoles()
        {
            return _dbContext.Roles.ToList();
        }
    }
}
