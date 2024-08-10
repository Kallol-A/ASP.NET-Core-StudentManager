using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentCategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public StudentCategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET api/studentcategory
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var categories = _categoryService.GetAllCategories();
            return Ok(categories);
        }

        // POST api/studentcategory
        [HttpPost]
        public ActionResult<bool> Post([FromBody] StudentCategory inputModel)
        {
            if (inputModel == null)
            {
                return BadRequest("Invalid input data.");
            }

            bool result = _categoryService.AddStudentCategory(inputModel.student_category,
                inputModel.created_by_user);

            if (result)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Failed to add student category.");
            }
        }

        // PUT api/studentcategory
        [HttpPut]
        public ActionResult<bool> Put([FromBody] StudentCategory updateModel)
        {
            if (updateModel == null || updateModel.id_student_category <= 0)
            {
                return BadRequest("Invalid input data or missing ID.");
            }

            bool result = _categoryService.UpdateStudentCategory(updateModel.id_student_category,
                updateModel.student_category, updateModel.last_updated_by_user);

            if (result)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest($"Failed to update student category with ID {updateModel.id_student_category}.");
            }
        }

        // DELETE api/studentcategory/{studentId}
        [HttpDelete("{studentId}")]
        public ActionResult<bool> Delete(long studentId, [FromBody] StudentCategory deleteModel)
        {
            if (studentId <= 0 || string.IsNullOrEmpty(deleteModel.deleted_by_user))
            {
                return BadRequest("Invalid ID.");
            }

            bool result = _categoryService.DeleteStudentCategory(studentId, deleteModel.deleted_by_user);

            if (result)
            {
                return Ok(result);
            }
            else
            {
                return NotFound($"Student category with ID {studentId} not found or failed to delete.");
            }
        }
    }
}