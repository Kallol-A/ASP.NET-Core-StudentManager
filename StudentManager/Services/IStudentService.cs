using System.Collections.Generic;
using System.Threading.Tasks;

using StudentManager.DTOs;
using StudentManager.Models;

namespace StudentManager.Services
{
    public interface IStudentService
    {
        Task<bool> InsertStudentCategoryAsync(string studentCategoryName, string createdBy);
        Task<IEnumerable<StudentCategoryDTO>> GetStudentCategoriesAsync(long? id);
        Task<bool> UpdateStudentCategoryAsync(long id_student_category, string student_category_name, string last_updated_by_user);
    }
}
