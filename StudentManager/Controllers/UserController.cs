using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using StudentManager.DTOs;
using StudentManager.Services;

namespace StudentManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        // Constructor
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET api/user
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> Get()
        {
            var roleIDClaim = User.FindFirst("roleID");
            if (roleIDClaim != null && long.TryParse(roleIDClaim.Value, out var roleID))
            {
                if (roleID == 1)
                {
                    var filteredUsers = await _userService.GetUsersAsync();
                    return Ok(filteredUsers);
                }
                else if (roleID == 2)
                {
                    var filteredUsers = _userService.GetUsersByRole(new List<long> { 2 });
                    return Ok(filteredUsers);
                }
                else if (roleID == 5)
                {
                    var filteredUsers = _userService.GetUsersByRole(new List<long> { 2,5 });
                    return Ok(filteredUsers);
                }
                else
                {
                    var filteredUsers = _userService.GetUsersExceptRole(1);
                    return Ok(filteredUsers);
                }
            }

            // Fallback if no filtered list is available (shouldn't happen)
            var users = await _userService.GetUsersAsync();
            return Ok(users);
        }
    }
}
