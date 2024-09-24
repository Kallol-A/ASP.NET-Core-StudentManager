using System.Collections.Generic;
using System.Threading.Tasks;

using StudentManager.DTOs;
using StudentManager.Models;

namespace StudentManager.Services
{
    public interface IInstituteService
    {
        Task<bool> InsertDepartmentAsync(Department inputModel);
    }
}
