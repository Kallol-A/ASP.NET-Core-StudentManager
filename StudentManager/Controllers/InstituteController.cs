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
    public class InstituteController : ControllerBase
    {
        private readonly IInstituteService _instituteService;

        // Constructor
        public InstituteController(IInstituteService instituteService)
        {
            _instituteService = instituteService;
        }

        // POST /api/institute/department/
        [HttpPost("department")]
        public async Task<IActionResult> InsertDepartmentAsync([FromBody] Department inputModel)
        {
            if (inputModel == null)
            {
                return BadRequest("Invalid input data");
            }

            var result = await _instituteService.InsertDepartmentAsync(inputModel);

            if (result)
            {
                return Ok("New department added successfully");
            }
            else
            {
                return BadRequest("Failed to add new department");
            }
        }
    }
}
