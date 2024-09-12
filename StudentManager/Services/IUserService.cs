using System.Collections.Generic;
using System.Threading.Tasks;

using StudentManager.DTOs;
using StudentManager.Models;

namespace StudentManager.Services
{
    public interface IUserService
    {
        bool InsertUser(long roleID, string userPhone, string userEmail, string userPassword, string createdBy);

        Task<LoginResult> LoginUser(string userEmail, string userPassword);

        Task<IEnumerable<UserDTO>> GetUsersAsync();
        IEnumerable<UserDTO> GetUsersByRole(long roleId);
        IEnumerable<UserDTO> GetUsersExceptRole(long roleId);
        Task<LoggedStudentDTO> GetLoggedStudentDetailAsync(long userID);
    }
}
