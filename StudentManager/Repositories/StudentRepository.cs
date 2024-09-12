using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using StudentManager.Data;
using StudentManager.Models;
using StudentManager.Services;

namespace StudentManager.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _dbContext;

        // Constructor
        public StudentRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async void InsertStudentCategoryAsync(StudentCategory studentcategory)
        {
            var validStudentCategory = studentcategory ?? throw new ArgumentNullException(nameof(studentcategory));
            await _dbContext.StudentCategory.AddAsync(validStudentCategory);
        }

        public async Task<bool> SaveAsync()
        {
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<StudentCategory>> GetStudentCategoriesAsync(long? id)
        {
            if (id.HasValue)
            {
                return await _dbContext.StudentCategory
                    .Where(u => u.deleted_at == null && u.id_student_category == id.Value)
                    .ToListAsync(); // Will return one or none records in a list
            }

            // Fetch all records if no id is passed
            return await _dbContext.StudentCategory
                .Where(u => u.deleted_at == null)
                .ToListAsync();
        }

        public async void UpdateStudentCategoryAsync(StudentCategory studentcategory)
        {
            // Find the existing student category by ID
            var filteredStudentCategory = await _dbContext.StudentCategory
                .FirstOrDefaultAsync(sc => sc.id_student_category == studentcategory.id_student_category && sc.deleted_at == null);

            var validStudentCategory = filteredStudentCategory ?? throw new ArgumentNullException(nameof(filteredStudentCategory));

            // Update the fields
            filteredStudentCategory.student_category_name = studentcategory.student_category_name;
            filteredStudentCategory.last_updated_by_user = studentcategory.last_updated_by_user;
            filteredStudentCategory.updated_at = studentcategory.updated_at;

            // Save the changes in the database
            _dbContext.StudentCategory.Update(filteredStudentCategory);
        }
    }
}
