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

        [HttpPost("studentcategory")]
        public async Task<IActionResult> InsertStudentCategoryAsync([FromBody] StudentCategory inputModel)
        {
            if (inputModel == null)
            {
                return BadRequest("Invalid input data.");
            }

            var result = await _studentService.InsertStudentCategoryAsync(inputModel.student_category_name, inputModel.created_by_user);

            if (result)
            {
                return Ok("New student category added successfully");
            }
            else
            {
                return BadRequest("Failed to add new student category");
            }
        }

        // GET api/studentcategory or api/studentcategory/{id}
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

        // PUT api/studentcategory/{id}
        [HttpPut("studentcategory/{id}")]
        public async Task<IActionResult> UpdateStudentCategoryAsync(long id, [FromBody] StudentCategory inputModel)
        {
            // Call the Update method in the service
            var result = await _studentService.UpdateStudentCategoryAsync(id,
                                                    inputModel.student_category_name,
                                                    inputModel.last_updated_by_user);

            if (!result)
            {
                return NotFound("Failed to add new student category");
            }

            return Ok("Student category updated successfully");
        }
    }
}
