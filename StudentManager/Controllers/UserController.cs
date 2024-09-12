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
                    var filteredUsers = _userService.GetUsersByRole(2);
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

        // GET api/user/logged_student_detail
        [HttpGet("logged_student_detail")]
        public async Task<ActionResult<LoggedStudentDTO>> GetLoggedStudentDetailsAsync()
        {
            var roleIDClaim = User.FindFirst("roleID");
            if (roleIDClaim == null || !int.TryParse(roleIDClaim.Value, out var roleID) || roleID != 2)
            {
                return NotFound("Logged user is not a student.");
            }

            // Extract userID from the JWT token in the Authorization Header
            var userIDClaim = User.FindFirst("userID");
            if (userIDClaim == null || !long.TryParse(userIDClaim.Value, out var userID))
            {
                return Unauthorized();
            }

            // Get the user details from the service
            var LoggedStudentDTO = await _userService.GetLoggedStudentDetailAsync(userID);

            if (LoggedStudentDTO == null)
            {
                return NotFound("No records found");
            }

            return Ok(LoggedStudentDTO);
        }
    }
}
