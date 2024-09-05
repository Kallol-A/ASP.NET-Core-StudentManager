using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using StudentManager.DTOs;
using StudentManager.Models;

namespace StudentManager.Services
{
    public interface IUserService
    {
        bool AddUser(long roleID, string userPhone, string userEmail, string userPassword, string createdBy);

        Task<LoginResult> LoginUser(string userEmail, string userPassword);

        Task<IEnumerable<UserDTO>> GetUsersAsync();
        IEnumerable<UserDTO> GetUsersByRole(long roleId);
        IEnumerable<UserDTO> GetUsersExceptRole(long roleId);
    }
}
