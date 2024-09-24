using System.Collections.Generic;
using System.Threading.Tasks;

using StudentManager.DTOs;
using StudentManager.Models;

namespace StudentManager.Services
{
    public interface IStudentService
    {
        Task<bool> IsStudent(StudentDetails studentdetails);
        Task<bool> InsertStudentDetailsAsync(StudentDetails studentdetails);
        Task<bool> InsertStudentCategoryAsync(StudentCategory studentcategory);
        Task<bool> UpdateStudentCategoryAsync(long id_student_category, StudentCategory studentcategory);

        Task<IEnumerable<StudentCategoryDTO>> GetStudentCategoriesAsync(long? id);
        Task<LoggedStudentDTO> GetLoggedStudentDetailAsync(long id);
    }
}
