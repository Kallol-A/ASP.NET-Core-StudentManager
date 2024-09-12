using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using StudentManager.Models;
using StudentManager.Repositories;
using StudentManager.DTOs;

namespace StudentManager.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        // Constructor
        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(StudentRepository));
        }

        public async Task<bool> InsertStudentCategoryAsync(string studentCategoryName, string createdBy)
        {
            try
            {
                // Create a new Student Category object
                var _studentcategory = new StudentCategory
                {
                    student_category_name = studentCategoryName,
                    created_by_user = createdBy,
                    created_at = DateTime.Now
                };

                // Add Student Category through repository
                _studentRepository.InsertStudentCategoryAsync(_studentcategory);
                var result = await _studentRepository.SaveAsync();

                return result; // Operation succeeded
            }
            catch (Exception ex)
            {
                // Log the exception
                // e.g., _logger.LogError(ex, "Error occurred while adding a user.");

                return false; // Operation failed
            }
        }

        public async Task<IEnumerable<StudentCategoryDTO>> GetStudentCategoriesAsync(long? id)
        {
            // Fetch data from the repository based on the presence of id
            var studentCategories = await _studentRepository.GetStudentCategoriesAsync(id);

            // If an id is provided, return the single record as a list
            if (id.HasValue)
            {
                var studentCategory = studentCategories.FirstOrDefault();
                if (studentCategory == null)
                {
                    return null;
                }
                return new List<StudentCategoryDTO>
                {
                    new StudentCategoryDTO
                    {
                        StudentCategoryId = studentCategory.id_student_category,
                        StudentCategoryName = studentCategory.student_category_name
                    }
                };
            }

            // Otherwise, return all records
            var studentCategoryDTOs = studentCategories.Select(u => new StudentCategoryDTO
            {
                StudentCategoryId = u.id_student_category,
                StudentCategoryName = u.student_category_name
            });

            return studentCategoryDTOs;
        }

        public async Task<bool> UpdateStudentCategoryAsync(long idStudentCategory, string studentCategoryName, string updatedBy)
        {
            try
            {
                // Create a new Student Category object
                var _studentcategory = new StudentCategory
                {
                    id_student_category = idStudentCategory,
                    student_category_name = studentCategoryName,
                    last_updated_by_user = updatedBy,
                    updated_at = DateTime.Now
                };

                // Add Student Category through repository
                _studentRepository.UpdateStudentCategoryAsync(_studentcategory);
                var result = await _studentRepository.SaveAsync();

                return result; // Operation succeeded
            }
            catch (Exception ex)
            {
                // Log the exception
                // e.g., _logger.LogError(ex, "Error occurred while adding a user.");

                return false; // Operation failed
            }
        }
    }
}
