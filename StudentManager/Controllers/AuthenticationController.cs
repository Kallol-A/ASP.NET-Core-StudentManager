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

        [HttpPost("user/login")]
        public async Task<IActionResult> UserLogin([FromBody] LoginRequest request)
        {
            var result = await _userService.LoginUser(request.Email, request.Password);

            if (result.Success)
            {
                return Ok(new { message = result.Message, token = result.Token });
            }

            return new ObjectResult(new { message = result.Message }) { StatusCode = 401 };
        }

        [HttpPost("user/register")]
        public ActionResult<bool> UserRegister([FromBody] User inputModel)
        {
            if (inputModel == null)
            {
                return BadRequest("Invalid input data.");
            }

            bool result = _userService.InsertUser(inputModel.id_role, inputModel.user_phone, inputModel.user_email,
                inputModel.user_password, inputModel.created_by_user);

            if (result)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Failed to register new user.");
            }
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
