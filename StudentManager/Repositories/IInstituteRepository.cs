using System.Collections.Generic;
using System.Threading.Tasks;

using StudentManager.DTOs;
using StudentManager.Models;

namespace StudentManager.Repositories
{
    public interface IInstituteRepository
    {
        Task<bool> SaveAsync();
        void InsertDepartmentAsync(Department department);
    }
}
