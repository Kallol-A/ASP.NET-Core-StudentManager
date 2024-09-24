using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using StudentManager.Models;
using StudentManager.Services;

namespace StudentManager.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [EnableCors("ReactAppPolicy")]
    public class AuthenticationController : ControllerBase
    {
        //private readonly IStudentService _studentService;
        private readonly IUserService _userService;

        public AuthenticationController(IUserService userService)
        {
            //_studentService = studentService;
            _userService = userService;
        }

        // GET api/auth/user/login
        [HttpPost("user/login")]
        public async Task<IActionResult> UserLogin([FromBody] User inputModel)
        {
            var result = await _userService.LoginUser(inputModel);

            if (result.Success)
            {
                return Ok(new { message = result.Message, token = result.Token });
            }

            return new ObjectResult(new { message = result.Message }) { StatusCode = 401 };
        }

        // POST api/auth/user/register
        [HttpPost("user/register")]
        public async Task<IActionResult> UserRegister([FromBody] User inputModel)
        {
            if (inputModel == null)
            {
                return BadRequest("Invalid input data.");
            }

            var result = await _userService.InsertUserAsync(inputModel);

            if (result)
            {
                return Ok("User registered successfully");
            }
            else
            {
                return BadRequest("Failed to register new user.");
            }
        }
    }
}
