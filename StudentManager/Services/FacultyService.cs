using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using StudentManager.Models;
using StudentManager.Repositories;
using StudentManager.DTOs;

namespace StudentManager.Services
{
    public class FacultyService : IFacultyService
    {
        private readonly IFacultyRepository _facultyRepository;
        private readonly IUserRepository _userRepository;

        // Constructor
        public FacultyService(IFacultyRepository facultyRepository, IUserRepository userRepository)
        {
            _facultyRepository = facultyRepository ?? throw new ArgumentNullException(nameof(facultyRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository)); // Injecting the user repository
        }

        public async Task<bool> InsertFacultyDetailsAsync(FacultyDetails facultydetails)
        {
            try
            {
                // Create a new Student Category object
                var _facultydetails = new FacultyDetails
                {
                    id_user = facultydetails.id_user,
                    id_department = facultydetails.id_department,
                    faculty_type = facultydetails.faculty_type,
                    faculty_first_name = facultydetails.faculty_first_name,
                    faculty_middle_name = facultydetails.faculty_middle_name,
                    faculty_last_name = facultydetails.faculty_last_name,
                    faculty_address1 = facultydetails.faculty_address1,
                    faculty_address2 = facultydetails.faculty_address2,
                    faculty_city = facultydetails.faculty_city,
                    faculty_district = facultydetails.faculty_district,
                    faculty_state = facultydetails.faculty_state,
                    faculty_pin = facultydetails.faculty_pin,
                    faculty_probationary_over = facultydetails.faculty_probationary_over,
                    created_by_user = facultydetails.created_by_user,
                    created_at = DateTime.Now
                };

                // Add Student Category through repository
                _facultyRepository.InsertFacultyDetailsAsync(_facultydetails);
                var result = await _facultyRepository.SaveAsync();

                return result; // Operation succeeded
            }
            catch (Exception ex)
            {
                // Log the exception
                // e.g., _logger.LogError(ex, "Error occurred while adding a user.");

                return false; // Operation failed
            }
        }

        public async Task<bool>  IsFaculty(FacultyDetails facultydetails)
        {
            // Check if the user is a faculty by validating their role
            var user = await _userRepository.GetUserById(facultydetails.id_user); // Assuming GetUserByIdAsync fetches user data including role info

            if (user == null || user.id_role != 5)
            {
                return false;
            }

            return true;
        }
    }
}
