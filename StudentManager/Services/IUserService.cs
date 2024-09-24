using System.Collections.Generic;
using System.Threading.Tasks;

using StudentManager.DTOs;
using StudentManager.Models;

namespace StudentManager.Services
{
    public interface IUserService
    {
        Task<bool> InsertUserAsync(User user);

        Task<LoginResult> LoginUser(User user);

        Task<IEnumerable<UserDTO>> GetUsersAsync();
        IEnumerable<UserDTO> GetUsersByRole(List<long> roleIds);
        IEnumerable<UserDTO> GetUsersExceptRole(long roleId);
    }
}
