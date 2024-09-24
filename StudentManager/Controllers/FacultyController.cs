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
    public class FacultyController : ControllerBase
    {
        private readonly IFacultyService _facultyService;

        // Constructor
        public FacultyController(IFacultyService facultyService)
        {
            _facultyService = facultyService;
        }

        // POST /api/faculty/detail/
        [HttpPost("detail")]
        public async Task<IActionResult> InsertFacultyDetailsAsync([FromBody] FacultyDetails inputModel)
        {
            var validationresult = await _facultyService.IsFaculty(inputModel);
            if (validationresult == false)
            {
                return BadRequest("User not a registered faculty in the system");
            }


            if (inputModel == null)
            {
                return BadRequest("Invalid input data");
            }

            var result = await _facultyService.InsertFacultyDetailsAsync(inputModel);

            if (result)
            {
                return Ok("New faculty details data added successfully");
            }
            else
            {
                return BadRequest("Failed to add new faculty details data");
            }
        }
    }
}
