using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using WebApi.Models;
using WebApi.DTOs;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // GET api/student
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var students = _studentService.GetAllStudents();
            return Ok(students);
        }

        //GetAllStudentsWithAllDetails
        [HttpGet("full-detail")]
        public ActionResult<IEnumerable<string>> GetAllStudentsWithAllDetails()
        {
            var studentfulldetails = _studentService.GetAllStudentsWithAllDetails();
            return Ok(studentfulldetails);
        }

        // GET api/student/details
        [HttpGet("detail")]
        public ActionResult<IEnumerable<string>> GetStudentDetails()
        {
            var studentdetails = _studentService.GetAllStudentDetails();
            return Ok(studentdetails);
        }

        // POST api/student/details
        [HttpPost("detail")]
        public ActionResult<IEnumerable<string>> AddStudentDetails([FromBody] StudentDetails inputModel)
        {
            var studentdetails = _studentService.AddStudentDetails(inputModel.id_student,
                inputModel.student_first_name, inputModel.student_middle_name, inputModel.student_last_name,
                inputModel.student_address1, inputModel.student_address2, inputModel.student_city,
                inputModel.student_district, inputModel.student_state, inputModel.student_pin,
                inputModel.student_feebook_given, inputModel.created_by_user);
            return Ok(studentdetails);
        }

        [HttpPost("detail_by_id")]
        public async Task<IActionResult> GetStudentDetail([FromBody] StudentIdDto StudentData)
        {
            var studentDetail = await _studentService.GetStudentDetailAsync(Convert.ToInt64(StudentData.StudentId));

            if (studentDetail == null)
            {
                return NotFound();
            }

            return Ok(studentDetail);
        }
    }
}
