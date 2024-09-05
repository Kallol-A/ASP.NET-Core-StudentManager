using System.Collections.Generic;
using System.Threading.Tasks;

using StudentManager.Models;

namespace StudentManager.Repositories
{
    public interface IUserRepository
    {
        void AddUser(User user);
        void Save();

        Task<User> FindByEmailAsync(string userEmail);
        Task<Role> GetUserRole(long id_role);

        Task<IEnumerable<User>> GetUsersAsync();
        IEnumerable<User> GetUsersByRole(long roleId);
        IEnumerable<User> GetUsersExceptRole(long roleId);
        IEnumerable<User> GetLoggedInStudentDetails(long userId);
    }
}
