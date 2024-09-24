using System.Collections.Generic;
using System.Threading.Tasks;

using StudentManager.DTOs;
using StudentManager.Models;

namespace StudentManager.Repositories
{
    public interface IStudentRepository
    {
        void InsertStudentCategoryAsync(StudentCategory studentcategory);
        void UpdateStudentCategoryAsync(StudentCategory studentcategory);
        void InsertStudentDetailsAsync(StudentDetails studentdetails);
        Task<bool> SaveAsync();

        Task<IEnumerable<StudentCategory>> GetStudentCategoriesAsync(long? id);
        Task<StudentDetails> GetLoggedStudentDetailAsync(long id);
    }
}
