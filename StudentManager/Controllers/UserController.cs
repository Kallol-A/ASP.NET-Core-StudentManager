using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using StudentManager.DTOs;
using StudentManager.Models;
using StudentManager.Services;

namespace StudentManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET api/user
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> Get()
        {
            // Retrieve the filtered users from HttpContext.Items
            var filteredUsers = HttpContext.Items["FilteredUsers"] as IEnumerable<UserDTO>;

            // If the filtered list is available, convert it to DTOs and return it
            if (filteredUsers != null)
            {
                return Ok(filteredUsers);
            }

            // Fallback if no filtered list is available (shouldn't happen)
            var users = await _userService.GetUsersAsync();
            return Ok(users);
        }

        // GET api/user/logged_user_detail
        [HttpGet("logged_student_detail")]
        public async Task<ActionResult<IEnumerable<LoggedStudentDTO>>> GetLoggedInStudentDetails()
        {
            // Retrieve the filtered users from HttpContext.Items
            var loggedStudentDetails = "hello";

            // If the filtered list is available, convert it to DTOs and return it
            if (loggedStudentDetails != null)
            {
                return Ok(loggedStudentDetails);
            }

            // Fallback if no filtered list is available (shouldn't happen)
            var users = await _userService.GetUsersAsync();
            return Ok(users);
        }
    }
}
