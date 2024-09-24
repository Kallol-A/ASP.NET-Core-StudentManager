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
        private readonly IUserRepository _userRepository;

        // Constructor
        public StudentService(IStudentRepository studentRepository, IUserRepository userRepository)
        {
            _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(StudentRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository)); // Injecting the user repository
        }

        public async Task<bool> InsertStudentCategoryAsync(StudentCategory studentcategory)
        {
            try
            {
                // Create a new Student Category object
                var _studentcategory = new StudentCategory
                {
                    student_category_name = studentcategory.student_category_name,
                    created_by_user = studentcategory.created_by_user,
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

        public async Task<bool> UpdateStudentCategoryAsync(long idStudentCategory, StudentCategory studentcategory)
        {
            try
            {
                // Create a new Student Category object
                var _studentcategory = new StudentCategory
                {
                    id_student_category = idStudentCategory,
                    student_category_name = studentcategory.student_category_name,
                    last_updated_by_user = studentcategory.last_updated_by_user,
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

        public async Task<bool> IsStudent(StudentDetails studentdetails)
        {
            // Check if the user is a faculty by validating their role
            var user = await _userRepository.GetUserById(studentdetails.id_user); // Assuming GetUserByIdAsync fetches user data including role info

            if (user == null || user.id_role != 2)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> InsertStudentDetailsAsync(StudentDetails studentdetails)
        {
            try
            {
                // Create a new Student Category object
                var _studentdetails = new StudentDetails
                {
                    id_user = studentdetails.id_user,
                    id_student_category = studentdetails.id_student_category,
                    student_type = studentdetails.student_type,
                    student_first_name = studentdetails.student_first_name,
                    student_middle_name = studentdetails.student_middle_name,
                    student_last_name = studentdetails.student_last_name,
                    student_address1 = studentdetails.student_address1,
                    student_address2 = studentdetails.student_address2,
                    student_city = studentdetails.student_city,
                    student_district = studentdetails.student_district,
                    student_state = studentdetails.student_state,
                    student_pin = studentdetails.student_pin,
                    student_feebook_given = studentdetails.student_feebook_given,
                    created_by_user = studentdetails.created_by_user,
                    created_at = DateTime.Now
                };

                // Add Student Category through repository
                _studentRepository.InsertStudentDetailsAsync(_studentdetails);
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

        public async Task<LoggedStudentDTO> GetLoggedStudentDetailAsync(long id)
        {
            // Fetch users from the repository
            var StudentDetails = await _studentRepository.GetLoggedStudentDetailAsync(id);

            if (StudentDetails == null)
            {
                return null;
            }

            // Map the fetched data to the DTOs
            var loggedStudentDTO = new LoggedStudentDTO
            {
                UserId = StudentDetails.id_user,
                StudentPhone = StudentDetails.Student.user_phone,
                StudentEmail = StudentDetails.Student.user_email,
                StudentDetails = new List<StudentDetailDTO>
                {
                    new StudentDetailDTO
                    {
                        StudentDetailsId = StudentDetails.id_student_details,
                        StudentType = StudentDetails.student_type,
                        StudentFirstName = StudentDetails.student_first_name,
                        StudentMiddleName = StudentDetails.student_middle_name,
                        StudentLastName = StudentDetails.student_last_name,
                        StudentAddress1 = StudentDetails.student_address1,
                        StudentAddress2 = StudentDetails.student_address2,
                        StudentCity = StudentDetails.student_city,
                        StudentDistrict = StudentDetails.student_district,
                        StudentState = StudentDetails.student_state,
                        StudentPIN = StudentDetails.student_pin,
                        StudentFeebookGiven = StudentDetails.student_feebook_given,
                        StudentCategory = new List<StudentCategoryDTO>
                        {
                            new StudentCategoryDTO
                            {
                                StudentCategoryId = StudentDetails.StudentCategory.id_student_category,
                                StudentCategoryName = StudentDetails.StudentCategory.student_category_name
                            }
                        }
                    }
                },
                StudentFees = StudentDetails.Student.StudentFees
                    .Select(fees => new StudentFeesDTO
                    {
                        StudentFeesId = fees.id_student_fees,
                        FeesForMonth = fees.fees_for_month,
                        FeesForYear = fees.fees_for_year,
                        FeeTotalAmount = fees.fee_total_amount,
                        FeePaidAmount = fees.fee_paid_amount,
                        FeeDiscountAmount = fees.fee_discount_amount,
                        FeeDueAmount = fees.fee_due_amount,
                        FeeBookEntryDone = fees.feebook_entry_done
                    }).ToList()
            };

            return loggedStudentDTO;
        }
    }
}
