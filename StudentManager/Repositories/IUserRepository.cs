using System.Collections.Generic;
using System.Threading.Tasks;

using StudentManager.DTOs;
using StudentManager.Models;

namespace StudentManager.Repositories
{
    public interface IUserRepository
    {
        void InsertUserAsync(User user);
        Task<bool> SaveAsync();

        Task<User> FindByEmailAsync(string userEmail);
        Task<User> GetUserById(long userId);
        Task<Role> GetUserRole(long roleId);
        Task<IEnumerable<User>> GetUsersAsync();
        IEnumerable<User> GetUsersByRole(List<long> roleIds);
        IEnumerable<User> GetUsersExceptRole(long roleId);
    }
}
