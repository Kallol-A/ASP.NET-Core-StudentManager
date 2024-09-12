using System.Collections.Generic;
using System.Threading.Tasks;

using StudentManager.DTOs;
using StudentManager.Models;

namespace StudentManager.Repositories
{
    public interface IUserRepository
    {
        void InsertUser(User user);
        void Save();

        Task<User> FindByEmailAsync(string userEmail);
        Task<Role> GetUserRole(long id_role);
        Task<IEnumerable<User>> GetUsersAsync();
        IEnumerable<User> GetUsersByRole(long roleId);
        IEnumerable<User> GetUsersExceptRole(long roleId);
        Task<StudentDetails> GetLoggedStudentDetailAsync(long userID);
    }
}
