using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using StudentManager.DTOs;
using StudentManager.Models;
using StudentManager.Services;

namespace StudentManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        // Constructor
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // GET api/student/studentcategory/ or api/student/studentcategory/{id}
        [HttpGet("studentcategory/{id?}")]
        public async Task<ActionResult<IEnumerable<StudentCategoryDTO>>> GetStudentCategoriesAsync(long? id)
        {
            var studentCategories = await _studentService.GetStudentCategoriesAsync(id);

            if (id.HasValue && studentCategories == null)
            {
                return NotFound("No Records Found"); // Return 404 if the specific category is not found
            }

            return Ok(studentCategories);
        }

        // POST api/student/studentcategory/
        [HttpPost("studentcategory")]
        public async Task<IActionResult> InsertStudentCategoryAsync([FromBody] StudentCategory inputModel)
        {
            if (inputModel == null)
            {
                return BadRequest("Invalid input data.");
            }

            var result = await _studentService.InsertStudentCategoryAsync(inputModel);

            if (result)
            {
                return Ok("New student category added successfully");
            }
            else
            {
                return BadRequest("Failed to add new student category");
            }
        }

        // PUT api/student/studentcategory/{id}
        [HttpPut("studentcategory/{id}")]
        public async Task<IActionResult> UpdateStudentCategoryAsync(long id, [FromBody] StudentCategory inputModel)
        {
            // Call the Update method in the service
            var result = await _studentService.UpdateStudentCategoryAsync(id, inputModel);

            if (!result)
            {
                return NotFound("Failed to update student category");
            }

            return Ok("Student category updated successfully");
        }

        // POST /api/student/detail/
        [HttpPost("detail")]
        public async Task<IActionResult> InsertStudentDetailsAsync([FromBody] StudentDetails inputModel)
        {
            var validationresult = await _studentService.IsStudent(inputModel);
            if (validationresult == false)
            {
                return BadRequest("User not a registered student in the system");
            }


            if (inputModel == null)
            {
                return BadRequest("Invalid input data");
            }

            var result = await _studentService.InsertStudentDetailsAsync(inputModel);

            if (result)
            {
                return Ok("New student details data added successfully");
            }
            else
            {
                return BadRequest("Failed to add new student details data");
            }
        }

        // GET api/student/logged_student_detail
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
            var LoggedStudentDTO = await _studentService.GetLoggedStudentDetailAsync(userID);

            if (LoggedStudentDTO == null)
            {
                return NotFound("No records found");
            }

            return Ok(LoggedStudentDTO);
        }
    }
}
