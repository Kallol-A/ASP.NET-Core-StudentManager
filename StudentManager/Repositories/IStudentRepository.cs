using System.Collections.Generic;
using System.Threading.Tasks;

using StudentManager.DTOs;
using StudentManager.Models;

namespace StudentManager.Repositories
{
    public interface IStudentRepository
    {
        void InsertStudentCategoryAsync(StudentCategory studentcategory);
        Task<bool> SaveAsync();
        Task<IEnumerable<StudentCategory>> GetStudentCategoriesAsync(long? id);
        void UpdateStudentCategoryAsync(StudentCategory studentcategory);
    }
}
