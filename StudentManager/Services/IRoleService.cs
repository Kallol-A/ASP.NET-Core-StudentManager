using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using StudentManager.Models;

namespace StudentManager.Services
{
    public interface IRoleService
    {
        bool AddRole(string role, string createdBy);
        IEnumerable<Role> GetAllRoles();
    }
}
