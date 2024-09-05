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
            var filteredUsers = HttpContext.Items["FilteredUsers"] as List<User>;

            // If the filtered list is available, convert it to DTOs and return it
            if (filteredUsers != null)
            {
                var userDTOs = filteredUsers.Select(user => new UserDTO
                {
                    // Map properties from User to UserDTO
                    UserId = user.id_user,
                    RoleId = user.id_role,
                    UserEmail = user.user_email,
                    UserPhone = user.user_phone,
                    RoleName = user.Role.role_name
                }).ToList();

                return Ok(userDTOs);
            }

            // Fallback if no filtered list is available (shouldn't happen)
            var users = await _userService.GetUsersAsync();
            return Ok(users);
        }
    }
}
