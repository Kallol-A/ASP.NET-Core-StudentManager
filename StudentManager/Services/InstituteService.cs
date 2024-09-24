using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using StudentManager.Models;
using StudentManager.Repositories;
using StudentManager.DTOs;

namespace StudentManager.Services
{
    public class InstituteService : IInstituteService
    {
        private readonly IInstituteRepository _instituteRepository;

        // Constructor
        public InstituteService(IInstituteRepository instituteRepository)
        {
            _instituteRepository = instituteRepository ?? throw new ArgumentNullException(nameof(instituteRepository));
        }

        public async Task<bool> InsertDepartmentAsync(Department department)
        {
            try
            {
                // Create a new Department object
                var _department = new Department
                {
                    department_name = department.department_name,
                    department_type = department.department_type,
                    created_by_user = department.created_by_user,
                    created_at = DateTime.Now
                };

                // Add Student Category through repository
                _instituteRepository.InsertDepartmentAsync(_department);
                var result = await _instituteRepository.SaveAsync();

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
